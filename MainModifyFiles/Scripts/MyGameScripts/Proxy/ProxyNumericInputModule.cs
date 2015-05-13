
// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyNumericInputModule.cs
// Author   : willson
// Created  : 2015/1/27 
// Porpuse  : 
// **********************************************************************
using UnityEngine;


public class ProxyNumericInputModule
{
	private const string NAME = "Prefabs/Module/ShopModule/NumericInputWinUI";
	
	public static void Open(GameObject anchor,UIAnchor.Side side,Vector2 pixelOffset,System.Action<int> onClickNumerCallBack)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.FourModule, false);
		var controller = view.GetMissingComponent<NumericInputWinUIController>();
		controller.InitView();
		controller.SetData(anchor,side,pixelOffset,onClickNumerCallBack);
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