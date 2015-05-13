using UnityEngine;
using System.Collections;

public class ProxyScreenPresureModule : MonoBehaviour {

	private const string NAME = "Prefabs/Module/StoryScreenModule/ScreenPresure";
	public static PresureController Open(ScreenPresureInst PresureInst)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,false);
		PresureController con = ui.GetMissingComponent<PresureController>();
		con.InitView();
		con.SetData (PresureInst);
		return con;
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
