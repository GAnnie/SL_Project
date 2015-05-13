// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyTradePetModule.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class ProxyTradePetModule
{
	private const string NAME = "Prefabs/Module/TradeModule/TradePet/PetShopWinUI";

	public static void Open(int depath = UILayerType.DefaultModule)
	{
		TradePetModel.Instance.PetShopEnter(()=>
		{
			ItemsContainerConst.ModuleType = ItemsContainerConst.ModuleType_Warehouse;
			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
			var controller = view.GetMissingComponent<PetShopWinUIController>();
			controller.InitView();
			controller.SetData();
		});
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