using UnityEngine;
using System.Collections;
using System;

public class ProxyServerListModule
{
	private const string NAME = "Prefabs/Module/ServerListModule/ServerListView";
	
	public static void Open(Action<ServerInfo> selectCallback)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.ThreeModule, true);
		
		var controller = ui.GetMissingComponent<ServerListController>();
		controller.Open (selectCallback);
	}
	
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
}

