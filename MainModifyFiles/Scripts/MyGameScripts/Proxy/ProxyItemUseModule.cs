// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyItemUseModule.cs
// Author   : willson
// Created  : 2015/3/14 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using System.Collections.Generic;
using System;

public class ProxyItemUseModule
{
	private const string NAME = "Prefabs/Module/ItemUseModule/ItemUseView";
	
	public static void Open(PackItemDto useDto,List<PackItemDto> efectItemList,bool isMultiple,int depth = UILayerType.FourModule)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, depth, true);
		var controller = ui.GetMissingComponent<ItemUseViewController>();
		controller.InitView();
		controller.SetData(useDto,efectItemList,isMultiple);
	}

	public static void OpenIdentifyItem(PackItemDto useDto,List<PackItemDto> efectItemList)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.FourModule, true);
		var controller = ui.GetMissingComponent<IdentifyItemUseController>();
		controller.InitView();
		controller.SetData(useDto,efectItemList,false,true);
		//controller.SetOptBtn("鉴定");
	}

	public static void OpenBattleItem(int itemUsedCount, int charactorType, Action<PackItemDto> callBackDelegate)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.FourModule, true);
		var controller = ui.GetMissingComponent<BattleItemUseController>();
		controller.InitView();
		controller.SetOtherParam(itemUsedCount, callBackDelegate);
		controller.SetData(null, BackpackModel.Instance.GetBattleItemList(charactorType),false,true);
	}


	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME);
	}
}
