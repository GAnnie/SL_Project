// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyEquipmentOptModule.cs
// Author   : willson
// Created  : 2015/1/20 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;

public class ProxyEquipmentOptModule
{
	private const string NAME = "Prefabs/Module/EquipmentOptModule/EquipmentOptWinUI";

	public static void Open(int depath = UILayerType.DefaultModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath, true);
		var controller = view.GetMissingComponent<EquipmentOptWinUIController>();
		controller.InitView();
		controller.OnSelectTabBtn(0);
	}

	public static void ShowEquipmentGem(PackItemDto dto = null,int depath = UILayerType.DefaultModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath, true);
		var controller = view.GetMissingComponent<EquipmentOptWinUIController>();
		controller.InitView();
		controller.OnSelectTabBtn(1);
        if (dto != null)
        {
            controller.SetEquipmentGemCurrEquip(dto);
        }
	}

    public static void ShowEquipmentProperty(int depath = UILayerType.DefaultModule)
    {
        GameObject view = UIModuleManager.Instance.OpenFunModule(NAME, depath, true);
        var controller = view.GetMissingComponent<EquipmentOptWinUIController>();
        controller.InitView();
        controller.OnSelectTabBtn(2);
    }

    public static void ShopEquipmentManufacturing(int grade, int depath = UILayerType.DefaultModule)
    {
        GameObject view = UIModuleManager.Instance.OpenFunModule(NAME, depath, true);
        var controller = view.GetMissingComponent<EquipmentOptWinUIController>();
        controller.InitView();
        controller.OnSelectTabBtn(0);
        controller.SetEquipmentManufacturingGrade(grade);
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