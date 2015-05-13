// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NumericInputWinUIController.cs
// Author   : willson
// Created  : 2015/1/27 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;

public class NumericInputWinUIController : MonoBehaviourBase,IViewController
{
	private NumericInputWinUI _view;
	private List<UIButton> _btnList;

	private System.Action<int> _onClickNumerCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<NumericInputWinUI>();
		_view.Setup(this.transform);

		_btnList = new List<UIButton>();
		_btnList.Add(_view.Btn_0);
		_btnList.Add(_view.Btn_1);
		_btnList.Add(_view.Btn_2);
		_btnList.Add(_view.Btn_3);
		_btnList.Add(_view.Btn_4);
		_btnList.Add(_view.Btn_5);
		_btnList.Add(_view.Btn_6);
		_btnList.Add(_view.Btn_7);
		_btnList.Add(_view.Btn_8);
		_btnList.Add(_view.Btn_9);

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		//UICamera.onClick += ClickEventHandler;
		EventDelegate.Set(_view.BgBoxCollider.onClick,OnClose);

		EventDelegate.Set(_view.Btn_0.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_1.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_2.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_3.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_4.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_5.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_6.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_7.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_8.onClick,OnClickNumer);
		EventDelegate.Set(_view.Btn_9.onClick,OnClickNumer);

		EventDelegate.Set(_view.Btn_backspace.onClick,OnClickBackspace);
		EventDelegate.Set(_view.Btn_enter.onClick,OnClickEnter);

		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);
	}

	public void SetData(GameObject anchor,UIAnchor.Side side,Vector2 pixelOffset,System.Action<int> onClickNumerCallBack)
	{
		_view.ViewAnchor.container = anchor;
		_view.ViewAnchor.pixelOffset = pixelOffset;
		_view.ViewAnchor.side = side;
		_view.ViewAnchor.enabled = true;

		_onClickNumerCallBack = onClickNumerCallBack;
	}

	private void OnClickNumer()
	{
		for(int index = 0;index < _btnList.Count;index++)
		{
			if(_btnList[index] == UIButton.current)
			{
				if(_onClickNumerCallBack != null)
					_onClickNumerCallBack(index);
			}
		}
	}

	private void OnClickBackspace()
	{
		if(_onClickNumerCallBack != null)
			_onClickNumerCallBack(-1);
	}

	private void OnClickEnter()
	{
		OnClose();
	}

	private void ClickEventHandler(GameObject go)
	{
		OnClose();
	}

	public void OnClose()
	{
		ProxyNumericInputModule.Close();
	}
	
	public void Dispose()
	{
		//UICamera.onClick -= ClickEventHandler;
	}
}