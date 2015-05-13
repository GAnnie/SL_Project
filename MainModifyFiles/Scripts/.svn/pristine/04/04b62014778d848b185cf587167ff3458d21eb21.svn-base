// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyPetTipsModule.cs
// Author   : willson
// Created  : 2015/3/27 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;

public class ProxyPetTipsModule
{
	private const string NAME = "Prefabs/Module/TipsModule/PetInfoWinUI";
	
	public static void Open(PetPropertyInfo petInfo,int depath = UILayerType.ThreeModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath,true);
		var controller = view.GetMissingComponent<PetInfoWinUIController>();
		controller.InitView();
		controller.SetData(petInfo);
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