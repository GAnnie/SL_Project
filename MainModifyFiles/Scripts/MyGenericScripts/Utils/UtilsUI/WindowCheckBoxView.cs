// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  WindowCheckBoxView.cs
// Author   : willson
// Created  : 2013/8/9 10:16:23
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class WindowCheckBoxView : MonoBehaviour
{
    public UILabel titleLabel;
    public UILabel infoLabel;
    public UILabel okLabel;
    public UILabel cancelLabel;
    public UILabel checkboxLabel;
    public UIToggle checkbox;

    private GameObject _view;
    private WindowManager.CallBackOkHandler _callBack;
    private WindowManager.CallBackCanelHandler _callBackCanel;
    private WindowManager.CallBackIsChecked _callBackIsChecked;

    private System.Action _closeCallBack = null;
	
	void Awake()
	{
		if( checkbox == null )	return;
		EventDelegate.Set(checkbox.onChange,OnStateChange);
	}
	
	void OnStateChange()
	{
		if( _callBackIsChecked != null )
		{
			_callBackIsChecked( UIToggle.current.value );
		}
	}
	
	void OnDestroy()
	{
		if( checkbox == null )	return;
		EventDelegate.Remove(checkbox.onChange,OnStateChange);
	}
	

    public void Open(GameObject view, string msg, WindowManager.CallBackOkHandler callBack, WindowManager.CallBackCanelHandler callBackCanel, WindowManager.CallBackIsChecked callBackIsChecked, System.Action closeCallBack = null)
    {
        _view = view;
        _view.SetActive(true);

        _callBack = callBack;
        _callBackCanel = callBackCanel;
        _closeCallBack = closeCallBack;
        _callBackIsChecked = callBackIsChecked;

        //titleLabel.text = title;
        infoLabel.text = msg;
    }

    public void OnCloseBtn()
    {
        if (_closeCallBack != null)
        {
            _closeCallBack();
        }
        //GameObject.Destroy(_view);
        ShowMainUI();

    }

    public void OnOkBtn()
    {
        OnCloseBtn();
        if (_callBack != null)
            _callBack();
    }

    public void OnCancelBtn()
    {
        OnCloseBtn();
        if (_callBackCanel != null)
            _callBackCanel();
    }

    void ShowMainUI()
    {
        //UIManager.Instance.ShowMainUI();
    }
}