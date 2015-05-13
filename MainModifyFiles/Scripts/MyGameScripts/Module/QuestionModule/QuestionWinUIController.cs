using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.question.data;
using com.nucleus.h1.logic.core.modules.question.dto;
using com.nucleus.h1.logic.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class QuestionWinUIController : MonoBehaviourBase, IViewController
{
    private QuestionWinUI _view;
    
    private PlayerQuestionDto _dto;

    private Question _currQuestion;
    private List<int> _answerIndexs;

    private UILabel[] _questions;
    private UISprite[] _questionAnswers;
    private UIButton[] _btns;
    public void InitView()
    {
        _answerIndexs = new List<int>();

        _view = gameObject.GetMissingComponent<QuestionWinUI>();
        _view.Setup(this.transform);

        _questions = new UILabel[] {_view.AQuestionLabel,_view.BQuestionLabel,_view.CQuestionLabel,_view.DQuestionLabel};
        _questionAnswers = new UISprite[] {_view.AResultSprite,_view.BResultSprite,_view.CResultSprite,_view.DResultSprite};
        _btns = new UIButton[] { _view.AQuestionBtnSprite, _view.BQuestionBtnSprite, _view.CQuestionBtnSprite, _view.DQuestionBtnSprite };

        RegisterEvent();
    }

    public void RegisterEvent()
    {
        EventDelegate.Set(_view.CloseBtn.onClick, OnCloseBtn);

        for (int index = 0; index < _btns.Length;index++ )
        {
            int i = index;
            EventDelegate.Set(_btns[index].onClick, () =>
            {
                SetBtnsEnabled(false);

                if(_currQuestion != null)
                {
                    if (_answerIndexs[i] == _currQuestion.correctIndex)
                    {
                        _questionAnswers[i].enabled = true;
                        _questionAnswers[i].spriteName = "right";

                        _dto.correctCount += 1;
                        _view.ResultLabel.text = string.Format("正确率   {0}/{1}", _dto.correctCount, _dto.questionType.amount);
                    }
                    else
                    {
                        _questionAnswers[i].enabled = true;
                        _questionAnswers[i].spriteName = "wrong";

                        if (_dto.questionType.show)
                        {
                            if (_currQuestion.correctIndex < _questionAnswers.Length)
                            {
                                int correctIndex = _answerIndexs.IndexOf(_currQuestion.correctIndex);
                                _questionAnswers[correctIndex].enabled = true;
                                _questionAnswers[correctIndex].spriteName = "right";
                            }

                        }

                        if(_dto.questionType.spendTime > 0)
                        {
                            _dto.useTime += _dto.questionType.spendTime;
                            _view.TimeLabel.text = DateUtil.getTimeString(_dto.useTime / 1000);
                        }
                    }

                    ServiceRequestAction.requestServer(QuestionService.answer(_currQuestion.type, _answerIndexs[i]), "answer",
                    (e) =>
                    {
                        PlayerQuestionAnswerDto dto = e as PlayerQuestionAnswerDto;
                        // 显示奖励,如果有的话
                        if(dto.items != null && dto.items.Count != 0)
                        {
                            ItemTipDto tipDto = new ItemTipDto();
                            tipDto.lostItems = new List<ItemDto>();
                            tipDto.gainItems = dto.items;
							GameLogicHelper.HandlerQuestionItemTipDto(tipDto);
                        }

                        if (_dto.noAnswers != null && _dto.noAnswers.Count > 0)
                        {
                            _dto.noAnswers.RemoveAt(0);
                            if(_dto.noAnswers.Count > 0)
                            {
                                // 2秒后显示下一题
                                if (_dto.questionTypeId == QuestionType.TYPE_KJ)
                                {
                                    Invoke("ShowQuestion", 2);
                                }
                                else
                                {
                                    ShowQuestion();
                                }
                            }
                            else
                            {
                                // 问答结束
                                OnFinishQuestion();
                            }
                        }
                    });
                }
            });
        }
    }

    private void SetBtnsEnabled(bool b)
    {
        for(int index = 0;index < _btns.Length;index++)
        {
            BoxCollider box = _btns[index].GetComponent<BoxCollider>();
            if (box != null)
                box.enabled = b;

            if(b)
            {
                _btns[index].SetState(UIButtonColor.State.Normal,true);
            }
        }
    }
    public void SetData(PlayerQuestionDto dto)
    {
        _dto = dto;

        if(_dto.questionTypeId == QuestionType.TYPE_KJ)
        {
            handleKeju();
        }
        else
        {
            handleDianshi();
        }
    }

    private void handleKeju()
    {
        _view.TitleSprite.spriteName = "bone-of-exam";
        _view.TitleSprite.MakePixelPerfect();

        _view.NameLabel.text = _dto.questionType.name + "玩法说明";
        _view.TipsLabel.text = _dto.questionType.desc;

        _view.TimeTipsLabel.text = "活动剩余：";

        InvokeRepeating("KejuTimer", 0, 1);
        _view.ResultLabel.text = string.Format("正确率   {0}/{1}", _dto.correctCount, _dto.questionType.amount);
        ShowQuestion();
    }

    private void KejuTimer()
    {
        /** 答题过期时间 */
        if (_dto.expiredTime > 0)
        {
            _view.TimeLabel.text = DateUtil.getTimeString(_dto.expiredTime/1000);
            _dto.expiredTime -= 1000;
        }
        else
        {
            OnFinishQuestion();
        }
    }

    private void handleDianshi()
    {
        _view.TitleSprite.spriteName = "bone-of-ds";
        _view.TitleSprite.MakePixelPerfect();

        _view.NameLabel.text = _dto.questionType.name + "玩法说明";
        _view.TimeTipsLabel.text = "答题耗时：";

        InvokeRepeating("DianshiTimer", 0, 1);
        _view.ResultLabel.text = string.Format("正确率   {0}/{1}", _dto.correctCount, _dto.questionType.amount);
        ShowQuestion();
    }

    private void DianshiTimer()
    {
        /** 殿试使用时间 */
        _view.TimeLabel.text = DateUtil.getTimeString(_dto.useTime / 1000);
        _dto.useTime += 1000;

        /** 答题过期时间 */
        if (_dto.expiredTime > 0)
        {
            _dto.expiredTime -= 1000;
        }
        else
        {
            OnFinishQuestion();
        }
    }

    private void ShowQuestion()
    {
        if (_dto.noAnswers != null && _dto.noAnswers.Count > 0)
        {
            SetBtnsEnabled(true);
            _currQuestion = DataCache.getDtoByCls<Question>(_dto.noAnswers[0]);

            _view.NumberLabel.text = string.Format("第{0}题", 20 - _dto.noAnswers.Count + 1);

            /** 问题描述 */
            _view.QuestionLabel.text = _currQuestion.description;

            /** 问题答案选项 */
            _answerIndexs.Clear();
            System.Random num = new System.Random();
            while(_answerIndexs.Count != _currQuestion.answers.Count)
            {
                int index = num.Next(0,_currQuestion.answers.Count);
                if (_answerIndexs.IndexOf(index) == -1)
                {
                    _answerIndexs.Add(index);
                }
            }
            string[] abcd = new string[] { "A.","B.","C.","D."};
            for (int index = 0; index < _questions.Length; index++)
            {
                if (index < _answerIndexs.Count)
                {
                    _questions[index].text = abcd[index] + _currQuestion.answers[_answerIndexs[index]];
                }
                else
                {
                    _questions[index].text = "";
                }
                _questionAnswers[index].enabled = false;
            }
            /** 正确答案选项索引 */
            //question.correctIndex;
        }
        else
        {
            _currQuestion = null;
        }
    }

    private void OnFinishQuestion()
    {
        if (_dto.questionTypeId == QuestionType.TYPE_KJ)
        {
            TipManager.AddTip("科举乡试已结束，请明天继续努力");
            OnCloseBtn();
        }
        else
        {
            TipManager.AddTip("御前殿试已结束，稍后将公布殿试排名");
            OnCloseBtn();
        }
    }
    private void OnCloseBtn()
    {
        CancelInvoke();
        ProxyQuestionModule.Close();
    }

    public void Dispose()
    {
        ServiceRequestAction.requestServer(QuestionService.exit(_dto.questionTypeId));
    }
}
