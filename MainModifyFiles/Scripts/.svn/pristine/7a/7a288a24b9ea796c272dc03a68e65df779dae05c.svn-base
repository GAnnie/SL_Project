// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillViewController.cs
// Author   : willson
// Created  : 2015/4/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.services;

public class AssistSkillViewController : MonoBehaviourBase,IViewController
{
	private const string AssistSkillCellName = "Prefabs/Module/SkillModule/AssistSkillCell";
	private const string StorySkillCellName = "Prefabs/Module/SkillModule/StorySkillCell";

	private AssistSkillView _view;
    private UIEventListener _workBtnListener;

	private List<AssistSkillCellController> _assistSkillCells;
	private List<StorySkillCellController> _storySkillCells;

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillView>();
		_view.Setup(this.transform);

		InitAssistSkillCells();
		InitStorySkillCells();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.AssistSkillStudyBtn.onClick,OnAssistSkillStudyBtn);
		UIButton workBtn = UIHelper.CreateBaseBtn(_view.WorkBtnPos,"活力打工");
        _workBtnListener = workBtn.gameObject.GetMissingComponent<UIEventListener>();
        _workBtnListener.onPress += OnWorkBtn;

		EventDelegate.Set(_view.WorkNotesBtn.onClick,OnWorkNotesBtn);

		EventDelegate.Set(_view.VigourNotesBtn.onClick,OnVigourNotesBtn);

		EventDelegate.Set(_view.StorySkillStudyBtn.onClick,OnStorySkillStudyBtn);

		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
		AssistSkillModel.Instance.OnUpdateAssistSkill += OnUpdateAssistSkill;
		AssistSkillModel.Instance.OnUpdateAll += ShowAssistSkill;
	}

	private void InitAssistSkillCells()
	{
		_assistSkillCells = new List<AssistSkillCellController>(12);
		for(int index = 0;index < 12;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( AssistSkillCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.AssistSkillGrid.gameObject,prefab);
			AssistSkillCellController cell = module.GetMissingComponent<AssistSkillCellController>();
			cell.InitView();
			_view.AssistSkillGrid.Reposition();
			_assistSkillCells.Add(cell);
		}
	}

	private void InitStorySkillCells()
	{
		_storySkillCells = new List<StorySkillCellController>(6);
		for(int index = 0;index < 6;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( StorySkillCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.StorySkillGrid.gameObject,prefab);
			StorySkillCellController cell = module.GetMissingComponent<StorySkillCellController>();
			cell.InitView();
			_view.StorySkillGrid.Reposition();
			_storySkillCells.Add(cell);
		}
	}

	public void SetData()
	{
		OnSubWealthChanged();
		ShowAssistSkill();
	}

	private void ShowAssistSkill()
	{
		List<AssistSkillVo> skillVos = AssistSkillModel.Instance.GetAssistSkills();
		int index = 0;
		foreach(AssistSkillVo vo in skillVos)
		{
			if(vo.IsShow() && index < _assistSkillCells.Count)
			{
				_assistSkillCells[index].SetData(vo);
				index++;
			}
		}

		if(index < _assistSkillCells.Count)
		{
			_assistSkillCells[index].SetData(null);
			_assistSkillCells[index].SetAddMode();
			index++;
		}

		while(index < _assistSkillCells.Count)
		{
			_assistSkillCells[index].SetData(null);
			index++;
		}
	}

	private void OnUpdateAssistSkill(AssistSkillDto dto)
	{
		for(int index = 0;index < _assistSkillCells.Count;index++)
		{
			if(_assistSkillCells[index].GetData() != null && _assistSkillCells[index].GetData().id == dto.id)
			{
				_assistSkillCells[index].UpDataLv(dto.level);
			}
		}
	}

	private void OnSubWealthChanged(SubWealthNotify dto = null)
	{
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + PlayerModel.Instance.VigourMax;
	}

	private void OnAssistSkillStudyBtn()
	{
		ProxySkillModule.OpenAssistSkillLearningWin();
	}

    void Update()
    {
        if (!_pressWorkBtn)
            return;
        if (_mDragStartTime > RealTime.time)
            return;

        if (_pressWorkBtn)
        {
            _pressWorkBtn = false;
            handleWork(true);
        }
    }

    private float _mDragStartTime = 0f;
    private float _pressAndHoldDelay = 0.5f;

    private bool _pressWorkBtn;
    private void OnWorkBtn(GameObject go, bool state)
    {
        if (state)
        {
            _pressWorkBtn = state;
            _mDragStartTime = RealTime.time + _pressAndHoldDelay;
        }
        else
        {
            if (_pressWorkBtn)
            {
                _pressWorkBtn = false;
                handleWork(false);
            }
        }
    }

    private void handleWork(bool isAll)
    {
        if (PlayerModel.Instance.Vigour < 40)
        {
            TipManager.AddTip("你的活力不足40点，无法打工");
            return;
        }

        if (!isAll)
        {
            ServiceRequestAction.requestServer(AssistSkillService.vigourConvertCopper(40), "factionMake", (e) =>
            {
                TipManager.AddTip(string.Format("消耗了[{0}]40[-]点活力，你获得了[{1}]4000[-]{2}",
                    ColorConstant.Color_Tip_LostCurrency_Str, ColorConstant.Color_Tip_GainCurrency_Str, ItemIconConst.Copper));
            });
        }
        else
        {
            int multiple = PlayerModel.Instance.Vigour / 40;
            ServiceRequestAction.requestServer(AssistSkillService.vigourConvertCopper(multiple * 40), "factionMake", (e) =>
            {
                TipManager.AddTip(string.Format("消耗了[{0}]{1}[-]点活力，你获得了[{2}]{3}[-]{4}",
                    ColorConstant.Color_Tip_LostCurrency_Str, multiple * 40, ColorConstant.Color_Tip_GainCurrency_Str, multiple * 4000, ItemIconConst.Copper));
            });
        }
    }

	private void OnWorkNotesBtn()
	{
		GameHintManager.Open(_view.WorkNotesBtn.gameObject,"每次消耗40点活力，可获得4000铜币\n长按则一次性将所有活力用于打工");
	}

	private void OnVigourNotesBtn()
	{
		GameHintManager.Open(_view.VigourNotesBtn.gameObject,"???????");
	}

	private void OnStorySkillStudyBtn()
	{
		TipManager.AddTip("All you people you can't you see,can't you see ~~~");
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
		AssistSkillModel.Instance.OnUpdateAssistSkill -= OnUpdateAssistSkill;
		AssistSkillModel.Instance.OnUpdateAll -= ShowAssistSkill;
        _workBtnListener.onPress -= OnWorkBtn;
	}
}