using UnityEngine;
using System.Collections;

public class ProxyPlayerTeamModule  {

	public const string NAME = "Prefabs/Module/PlayerTeamModule/PlayerTeamView";
	public const string CHARACTERINFO_WIDGET = "Prefabs/Module/PlayerTeamModule/CharacterInfoWidget";

	public static void Open(){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule,true);
		
		var controller = ui.GetMissingComponent<PlayerTeamViewController>();
		controller.InitView();
	}
	
	public static void Close(){
		UIModuleManager.Instance.CloseModule(NAME);
	}
	
	public static void Hide(){
		UIModuleManager.Instance.HideModule(NAME);	
	}
	
	public static void Show(){
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule,true);
	}
}
