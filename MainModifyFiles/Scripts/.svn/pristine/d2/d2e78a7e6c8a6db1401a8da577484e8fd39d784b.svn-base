using UnityEngine;
using System.Collections;
using System;

public class WindowPrefabController : MonoBehaviour, IViewController
{
	private WindowPrefab _view;

	#region IViewController
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<WindowPrefab> ();
		_view.Setup (this.transform);
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set (_view.OKButton.onClick, OnClickOkButton);
		EventDelegate.Set (_view.CancelButton.onClick, OnClickCancelButton);
	}

	public void Dispose ()
	{
	}
	#endregion

	public event Action OnOkHandler;
	public event Action OnCancelHandler;

	public void OpenConfirmWindow (string msg, 
	                                     string title="",
	                            	 	 Action onHandler = null,
	                             		 Action cancelHandler = null,
	                              		 UIWidget.Pivot pivot = UIWidget.Pivot.Center,
	                                     string okLabelStr = "确定", string cancelLabelStr = "取消")
	{
		InitView ();
		RegisterEvent ();

		_view.InfoLabel.text = msg;
		_view.InfoLabel.pivot = pivot;

		_view.TitleLabel.text = title;
		_view.OKLabel.text = okLabelStr;
		_view.OKLabel.spacingX = GetLabelSpacingX (okLabelStr);
		_view.OKButton.transform.localPosition = new Vector3 (-118, -80, 0);
		_view.CancelLabel.text = cancelLabelStr;
		_view.CancelLabel.spacingX = GetLabelSpacingX (cancelLabelStr);
		_view.OKButton.gameObject.SetActive (true);
		_view.CancelButton.gameObject.SetActive (true);

		OnOkHandler = onHandler;
		OnCancelHandler = cancelHandler;
	}
	
	public void OpenMessageWindow (string msg, 
	                              string title="",
	                              Action onHandler = null,
	                              UIWidget.Pivot pivot = UIWidget.Pivot.Center,
	                              string okLabelStr = "确定")
	{
		InitView ();
		RegisterEvent ();
		
		_view.InfoLabel.text = msg;
		_view.InfoLabel.pivot = pivot;

		_view.TitleLabel.text = title;
		_view.OKLabel.text = okLabelStr;
		_view.OKLabel.spacingX = GetLabelSpacingX (okLabelStr);
		_view.OKButton.transform.localPosition = new Vector3 (0, -80, 0);
		_view.OKButton.gameObject.SetActive (true);
		_view.CancelButton.gameObject.SetActive (false);

		OnOkHandler = onHandler;
	}

	private int GetLabelSpacingX (string text)
	{
		if (text.Length <= 2) {
			return 12;
		} else if (text.Length <= 3) {
			return 6;
		} else {
			return 1;
		}
	}

	private void OnClickOkButton ()
	{
		ProxyWindowModule.Close ();		

		if (OnOkHandler != null) {
			OnOkHandler ();
		}
	}

	private void OnClickCancelButton ()
	{
		ProxyWindowModule.Close ();

		if (OnCancelHandler != null) {
			OnCancelHandler ();
		}
	}
}

