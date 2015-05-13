using UnityEngine;
using System.Collections;
using System;
using com.nucleus.commons.message;

public class ProxyRoleCreateModule
{
	private const string NAME = "Prefabs/Module/RoleCreateModule/RoleCreateView";

	public static void Open(ServerInfo info, Action<GeneralResponse> onCreatePlayerSuccess)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.SubModule, false);
		
		var controller = ui.GetMissingComponent<RoleCreateController>();
		controller.InitView();
		controller.SetData (info, onCreatePlayerSuccess);
	}

	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
}