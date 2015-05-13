using UnityEngine;
using System.Collections;

public class ProxyScreenMaskModule : MonoBehaviour {

	private const string NAME = "Prefabs/Module/StoryScreenModule/ScreenMask";
	public static MaskController Open(ScreenMaskInst screenMaskInst)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,false);
		MaskController con = ui.GetMissingComponent<MaskController>();
		con.InitView();
		con.SetData (screenMaskInst);
		con.ShowMask ();
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
