// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyItemRetrieveModule.cs
// Author   : willson
// Created  : 2015/1/24 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.player.msg;

public class ProxyItemRetrieveModule
{
	private const string NAME = "Prefabs/Module/BackpackModule/ItemRetrieveWinUI";
	
	public static void Open(int depath = UILayerType.DefaultModule)
	{
		List<PackItemDto> items = BackpackModel.Instance.GetResumeItems();
		if(items != null & items.Count > 0)
		{
			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath, true);
			var controller = view.GetMissingComponent<ItemRetrieveWinUIController>();
			controller.InitView();
			controller.SetData(items);
		}
		else
		{
			TipManager.AddTip("没有可找回的装备");
		}
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