// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyPayModule.cs
// Author   : willson
// Created  : 2015/1/30 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class ProxyPayModule
{
	private const string NAME = "Prefabs/Module/PayModule/WealthBuyWinUI";
	
	public static void OpenPay(int depth = UILayerType.FourModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depth, true);
		var controller = view.GetMissingComponent<PayWinUIController>();
		controller.InitView();
		controller.SetData();
	}

	public static void OpenSilver(int depth = UILayerType.FourModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depth, true);
		var controller = view.GetMissingComponent<SilverWinUIController>();
		controller.InitView();
		controller.SetData();
	}

	public static void OpenCopper(int depth = UILayerType.FourModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depth, true);
		var controller = view.GetMissingComponent<CopperWinUIController>();
		controller.InitView();
		controller.SetData();
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