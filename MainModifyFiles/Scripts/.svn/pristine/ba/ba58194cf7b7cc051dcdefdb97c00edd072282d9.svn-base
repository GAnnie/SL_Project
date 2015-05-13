using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.mission.data;

public class ProxyWorldMapModule
{
	#region WorldMapController
	private const string NAME = "Prefabs/Module/WorldMapModule/WorldMapView";
	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.ThreeModule, true);
		
		var controller = ui.GetMissingComponent<WorldMapController>();
		controller.Open ();
	}
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
	#endregion

	#region MiniMapController
	private const string NAME_MINIMAP = "Prefabs/Module/WorldMapModule/MiniMapView";
	public static void OpenMiniMap()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_MINIMAP, UILayerType.ThreeModule, true);
		
		var controller = ui.GetMissingComponent<MiniMapController>();
		controller.Open ();
	}
	public static void CloseMiniMap()
	{
		UIModuleManager.Instance.CloseModule (NAME_MINIMAP);
	}
	#endregion

    #region MiniWorldMap
    private const string NAME_MINIWORLDMAP = "Prefabs/Module/WorldMapModule/MiniWorldMapView";
    public static void OpenMiniWorldMap()
    {
        GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_MINIWORLDMAP, UILayerType.ThreeModule, true);

        var controller = ui.GetMissingComponent<MiniWorldMapViewController>();
        controller.InitView();
        controller.Open();
    }
    public static void CloseMiniWorldMap()
    {
        UIModuleManager.Instance.CloseModule(NAME_MINIWORLDMAP);
    }
    #endregion

    #region NpcDialogueController
    private const string NAME_NpcDialogue = "Prefabs/Module/WorldMapModule/NpcDialogue/NpcDialogueView";
	private static NpcDialogueController CreateDialogueController() {
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_NpcDialogue, UILayerType.FourModule, true);
		return ui.GetMissingComponent<NpcDialogueController>();
	}

	public static void OpenNpcDialogue(NpcGeneral generalNpc) {
		NpcDialogueController tController = CreateDialogueController();
		tController.Open(generalNpc);
	}

	//	明雷怪对话
	public static void OpenNpcDialogue(NpcMonster monsterNpc) {
		NpcDialogueController tController = CreateDialogueController();
		tController.Open(monsterNpc);
	}

	public static void OpenNpcDialogueInAcceptMission(Mission mission) {
		NpcDialogueController tController = CreateDialogueController();
		tController.OpenQuickFindPath(mission);
	}

	public static void OpenCommonDialogue(string npcName,string content,List<string> optionList,System.Action<int> onSelect,int optionBgWidth=270){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_NpcDialogue,UILayerType.FourModule, true);
		var controller = ui.GetMissingComponent<NpcDialogueController>();
		controller.OpenCommonDialogue (npcName,content,optionList,onSelect,optionBgWidth);
	}

	public static void CloseNpcDialogue()
	{
		UIModuleManager.Instance.CloseModule (NAME_NpcDialogue);
	}
	#endregion
}