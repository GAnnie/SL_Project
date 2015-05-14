﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyBackpackModule.cs
// Author   : willson
// Created  : 2015/1/10 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using com.nucleus.player.msg;

public class ProxyBackpackModule
{
	private const string NAME = "Prefabs/Module/BackpackModule/BackpackWinUI";

    private static BackpackWinUIController controller;
	public static void Open(int depath = UILayerType.DefaultModule)
	{
        if (controller != null)
        {
            Show();
        }
        else
        {
            PlayerModel.Instance.RequestCharacterDto(() =>
            {
                ItemsContainerConst.ModuleType = ItemsContainerConst.ModuleType_Backpack;
                GameObject view = UIModuleManager.Instance.OpenFunModule(NAME, depath, false);
                controller = view.GetMissingComponent<BackpackWinUIController>();
                controller.InitView();
            });
        }
	}

	//通过packItem 重新定位 index

	public static void ShowItemTipsByPackItem(PackItemDto dto){
		int itemIndex = BackpackModel.Instance.GetIndexByPackItemDto(dto);
		if( itemIndex != -1){
			ShowItemTips(itemIndex);
		}
		else{

			Debug.Log("<color=yellow> ############### 没找到该物品 ############## </color>");
		}

	}

    public static void ShowItemTips(int itemIndex, int depath = UILayerType.DefaultModule)
    {
        if(controller != null)
        {
            controller.ShowItemTips(itemIndex);
        }
        else
        {
            PlayerModel.Instance.RequestCharacterDto(() =>
            {
                ItemsContainerConst.ModuleType = ItemsContainerConst.ModuleType_Backpack;
                GameObject view = UIModuleManager.Instance.OpenFunModule(NAME, depath, true);
                controller = view.GetMissingComponent<BackpackWinUIController>();
                controller.InitView();
                controller.ShowItemTips(itemIndex);
            });
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
        controller = null;
	}
}