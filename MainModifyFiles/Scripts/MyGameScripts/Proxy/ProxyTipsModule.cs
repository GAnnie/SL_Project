// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyTipsModule.cs
// Author   : willson
// Created  : 2015/1/19 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;

public class ProxyTipsModule
{
	private const string NAME = "Prefabs/Module/TipsModule/ConsumerTipsView";
	
	public static void Open(string tips,int itemId,int count,System.Action<List<ItemDto>> OnOptBtnClick,int depath = UILayerType.ThreeModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<ConsumerTipsViewController>();
		controller.InitView();

		controller.SetData(tips,itemId,count,OnOptBtnClick);
	}

	public static void Open(string tips,List<ItemDto> items,int ingot,System.Action<List<ItemDto>> OnOptBtnClick,int depath = UILayerType.ThreeModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<ConsumerTipsViewController>();
		controller.InitView();
		
		controller.SetData(tips,items,ingot,OnOptBtnClick);
	}

	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.ThreeModule, false);
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