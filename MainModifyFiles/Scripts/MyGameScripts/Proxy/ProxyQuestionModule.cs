using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.question.dto;
using com.nucleus.h1.logic.core.modules.question.data;

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
                    TipManager.AddTip("科举乡试已结束，请明天继续努力");
                }
                else
                {
                    TipManager.AddTip("御前殿试已结束，稍后将公布殿试排名");
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

