using UnityEngine;
using System.Collections;

public class ProxyChatModule : MonoBehaviour {

	private const string NAME = "Prefabs/Module/ChatPrefab/ChatInfoView";
	
	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule,true);
		var controller = ui.GetMissingComponent<ChatInfoViewController>();
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
		ChatModel.Instance.SetLock(false);
	}
}
