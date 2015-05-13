using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using com.nucleus.h1.logic.chat.modules.dto;
using LITJson;

public class ProxyFriendModule : MonoBehaviour {
		
	private const string NAME = "Prefabs/Module/FriendModule/FriendView";
	
	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,true);
		var controller = ui.GetMissingComponent<FriendViewController>();
		controller.InitView();
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
