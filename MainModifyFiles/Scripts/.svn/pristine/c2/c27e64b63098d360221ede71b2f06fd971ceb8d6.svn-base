using UnityEngine;
using System.Collections;
using System;

public class ProxyWindowModule
{
	#region WindowPrefabController
	private const string NAME_WindowPrefab = "Prefabs/Module/WindowModule/WindowPrefab";

	public static void OpenConfirmWindow (string msg, 
	                                     string title="",
	                                     Action onHandler = null,
	                                     Action cancelHandler = null,
	                                     UIWidget.Pivot pivot = UIWidget.Pivot.Center,
	                                     string okLabelStr = null, string cancelLabelStr = null)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule (NAME_WindowPrefab, UILayerType.FourModule, true, false);

		if (string.IsNullOrEmpty (title)) {
			title = "提示";
		}

		if (string.IsNullOrEmpty (okLabelStr)) {
			okLabelStr = "确定";
		}

		if (string.IsNullOrEmpty (cancelLabelStr)) {
			cancelLabelStr = "取消";
		}

		var controller = ui.GetMissingComponent<WindowPrefabController> ();
		controller.OpenConfirmWindow (msg, title, onHandler, cancelHandler, pivot, okLabelStr, cancelLabelStr);
	}

	public static void OpenMessageWindow (string msg, 
	                                     string title="",
	                                     Action onHandler = null,
	                                     UIWidget.Pivot pivot = UIWidget.Pivot.Center,
	                                     string okLabelStr = null)
	{
		
		GameObject ui = UIModuleManager.Instance.OpenFunModule (NAME_WindowPrefab, UILayerType.FourModule, true, false);

		if (string.IsNullOrEmpty (title)) {
			title = "提示";
		}

		if (string.IsNullOrEmpty (okLabelStr)) {
			okLabelStr = "确定";
		}

		var controller = ui.GetMissingComponent<WindowPrefabController> ();
		controller.OpenMessageWindow (msg, title, onHandler, pivot, okLabelStr);
	}
	
	public static void Close ()
	{
		UIModuleManager.Instance.CloseModule (NAME_WindowPrefab);
	}
	#endregion
}