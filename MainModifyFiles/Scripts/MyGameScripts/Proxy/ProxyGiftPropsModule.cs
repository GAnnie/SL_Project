using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ProxyGiftPropsModule : MonoBehaviour {

	private const string NAME = "Prefabs/Module/GiftModule/GiftPropsView";

	public static void Open(SimplePlayerDto s)
	{
		GiftPropsModel.Instance.SetPlayer(s);

		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,true);
		var controller = ui.GetMissingComponent<GiftPropsViewController>();
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
