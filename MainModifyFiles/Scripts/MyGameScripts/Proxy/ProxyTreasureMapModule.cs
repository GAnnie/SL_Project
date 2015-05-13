using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.scene.data;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ProxyTreasureMapModule : MonoBehaviour {


	private const string NAME = "Prefabs/Module/TreasureMapModule/TreasureMapView";

	public static PackItemDto dto;
	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,false);
		var controller = ui.GetMissingComponent<TreasureMapViewController>();
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
