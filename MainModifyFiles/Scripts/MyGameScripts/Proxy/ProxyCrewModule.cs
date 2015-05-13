using UnityEngine;
using System.Collections;

public class ProxyCrewModule{
	private const string NAME = "Prefabs/Module/CrewModule/CrewMainView";

	public static void Open(){
		CrewModel.Instance.RequestCrewInfo(OpenView);
	}

	private static void OpenView(){
		if(UIModuleManager.Instance.ContainsModule(ProxyPlayerTeamModule.NAME))
			ProxyPlayerTeamModule.Hide();

		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule,true);
		
		var controller = ui.GetMissingComponent<CrewMainViewController>();
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

