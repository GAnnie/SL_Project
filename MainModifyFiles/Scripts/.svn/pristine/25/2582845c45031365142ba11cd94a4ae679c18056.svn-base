// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyPetResumeModule.cs
// Author   : willson
// Created  : 2015/3/18
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;

public class ProxyPetResumeModule
{
	private const string NAME = "Prefabs/Module/PetResumeModule/PetResumeWinUI";
	
	public static void Open(int depath = UILayerType.DefaultModule)
	{
		ServiceRequestAction.requestServer(PetService.drops(),"drops",(e) => {
			DataList list = e as DataList/*<PetCharactorDto>*/;
			if(list == null || list.items == null || list.items.Count == 0)
			{
				TipManager.AddTip("没有可以找回的宠物");
				return;
			}

			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,depath, true);
			var controller = view.GetMissingComponent<PetResumeWinUIController>();
			controller.InitView();
			controller.SetData(list.items);
		});
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