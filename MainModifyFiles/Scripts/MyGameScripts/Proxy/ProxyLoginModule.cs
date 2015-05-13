using UnityEngine;
using System.Collections;

public class ProxyLoginModule
{
	private const string NAME = "Prefabs/Module/LoginModule/LoginView";
	
	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule, false);
		
		var controller = ui.GetMissingComponent<LoginController>();
		controller.Open ();
	}
	
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
}

