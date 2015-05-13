using UnityEngine;
using System.Collections;

public class WindowView : MonoBehaviour 
{
	public UILabel infoLabel;
	public UISprite okSprite;
	public UISprite cancelSprite;
	public UILabel okLabel;
	public UILabel cancelLabel;
	public UILabel thildLabel;
	
	private GameObject _view;
	private WindowManager.CallBackOkHandler _callBack;
	private WindowManager.CallBackCanelHandler _callBackCanel;

    private System.Action<bool> _closeCallBack = null;

	void Start () {
	
	}
	
	void Update () {
	
	}
	
    public void Open(GameObject view, string msg , WindowManager.CallBackOkHandler callBack, WindowManager.CallBackCanelHandler callBackCanel, System.Action<bool> closeCallBack = null  , string thildLbl = null)
	{
		_view = view;
        _view.SetActive(true);

		_callBack = callBack;
		_callBackCanel = callBackCanel;
        _closeCallBack = closeCallBack;
		SetContentLbl( msg );
	}

	public void SetBtnDate(string okSpriteName, string cancelSpriteName, string strOk, string strCancel) {
		if (okSpriteName != null) okSprite.spriteName = okSpriteName;
		if (cancelSpriteName != null) cancelSprite.spriteName = cancelSpriteName;

		okLabel.text = strOk;
		cancelLabel.text = strCancel;
	}

	public void OnCloseBtn()
	{
		CloseWindow(true);
	}
	
	public void OnOkBtn()
	{
		CloseWindow(false);
		if(_callBack != null)
			_callBack();
	}
	
	public void OnCancelBtn()
	{
		CloseWindow(false);
		if(_callBackCanel != null)
			_callBackCanel();
	}
	
	private void CloseWindow(bool onlyClosed){
        if( _closeCallBack != null )
        {
            _closeCallBack(onlyClosed);
        }		
	}

	private void SetContentLbl( string msg )
	{
		infoLabel.text = msg;
		
//		if( msg.Length > 18)
//		{
//			infoLabel.pivot = UILabel.Pivot.Left;
//		}
//		else
//		{
//			infoLabel.pivot = UILabel.Pivot.Center;
//		}
	}
}
