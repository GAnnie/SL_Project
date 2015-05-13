// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyShopModule.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************

using System;
using UnityEngine;

public class ProxyShopModule
{
	private const string NAME = "Prefabs/Module/ShopModule/ShopWinUI";
	
	public static void Open(int depath = UILayerType.DefaultModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<ShopWinUIController>();
		controller.InitView();
	}

	public static void OpenNpcShop(int shopId,int depath = UILayerType.DefaultModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<NpcShopWinUIController>();
		controller.InitView();
		controller.SetData(shopId);
	}

	public static void OpenMallShopping(int depath = UILayerType.DefaultModule)
	{
		MallShoppingModel.Instance.UpData(()=>{
			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
			var controller = view.GetMissingComponent<MallShoppingWinUIController>();
			controller.InitView();
			controller.Open();
		});
	}

	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule,true);
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