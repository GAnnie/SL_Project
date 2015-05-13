// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyWarehouseModule.cs
// Author   : willson
// Created  : 2015/1/16 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class ProxyWarehouseModule
{
	private const string NAME = "Prefabs/Module/BackpackModule/WarehouseWinUI";

	private static int depath;

	public static void Open(int _depath = UILayerType.DefaultModule)
	{
		depath = _depath;

		ItemsContainerConst.ModuleType = ItemsContainerConst.ModuleType_Warehouse;
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<WarehouseWinUIController>();
		controller.InitView();
	}

	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule, true);
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