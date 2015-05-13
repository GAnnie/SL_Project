using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.title.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class ProxyHyperlinkMissionModule : MonoBehaviour {

	private const string NAME = "Prefabs/Module/FriendModule/HyperlinkMission";
	
	public static void Open(PlayerMissionDto dto)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.Guide,false);

		var controller = ui.GetMissingComponent<HyperlinkMissionController>();
		controller.InitView();
		controller.SetData(dto);
	}

	public static void Open(Title dto)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.Guide,false);
		var controller = ui.GetMissingComponent<HyperlinkMissionController>();
		controller.InitView();
		controller.SetData(dto);
	}

	public static void Open(AssistSkillDto dto)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.Guide,false);
		var controller = ui.GetMissingComponent<HyperlinkMissionController>();
		controller.InitView();
		controller.SetData(dto);
	}

	public static void Open(FactionSkillDto dto){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.Guide,false);
		var controller = ui.GetMissingComponent<HyperlinkMissionController>();
		controller.InitView();
		controller.SetData(dto);
	}
	
	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule, true);
	}
	
	public static void Hide()
	{
		UIModuleManager.Instance.HideModule(NAME);	
	}
	
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule(NAME);
	}

}
