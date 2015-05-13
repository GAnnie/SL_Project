// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyAutoFramModule.cs
// Author   : willson
// Created  : 2015/1/8 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class ProxyAutoFramModule
{
	private const string NAME = "Prefabs/Module/AutoFramModule/AutoFramWinUI";

	public static void Open(int depath = UILayerType.DefaultModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath, true);
		var controller = view.GetMissingComponent<AutoFramWinUIController>();
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
	}
}