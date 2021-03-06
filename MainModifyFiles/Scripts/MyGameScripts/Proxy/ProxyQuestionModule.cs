﻿using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.question.dto;
using com.nucleus.h1.logic.core.modules.question.data;
using System;

public class ProxyQuestionModule
{
    private const string NAME = "Prefabs/Module/QuestionModule/QuestionWinUI";
	
	public static void Open(int type)
	{
        ServiceRequestAction.requestServer(QuestionService.enter(type), "QuestionEnter",
        (e) =>
        {
            PlayerQuestionDto dto = e as PlayerQuestionDto;

            if(dto.noAnswers == null || dto.noAnswers.Count == 0 || dto.expiredTime <= 0)
            {
                if (dto.questionTypeId == QuestionType.TYPE_KJ)
                {
                    if (dto.questionType.openTimes.Count >= 2)
                    {
                        string timeStr = dto.questionType.openTimes[1];
                        timeStr = timeStr.Split('-')[0];
                        DateTime lastTime = Convert.ToDateTime(timeStr);
                        DateTime date = SystemTimeManager.Instance.GetServerTime();

                        if (date.Hour >= lastTime.Hour && date.Minute >= lastTime.Minute && date.Second > 0)
                        {
                            TipManager.AddTip("今日科举乡试已结束，请明天继续努力");
                        }
                        else
                        {
                            TipManager.AddTip("今日首次科举乡试已结束，请留意第二次乡试时间");
                        }
                    }
                    else
                    {
                        TipManager.AddTip("今日科举乡试已结束，请明天继续努力");
                    }
                }
                else
                {
                    TipManager.AddTip("本周科举殿试已结束，名次排行稍后公布，敬请留意");
                }
                return;
            }

            GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule, false);
            var controller = ui.GetMissingComponent<QuestionWinUIController>();
            controller.InitView();
            controller.SetData(dto);
        });
	}
	
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
}

