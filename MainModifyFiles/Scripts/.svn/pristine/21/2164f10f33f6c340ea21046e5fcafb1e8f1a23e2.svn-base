// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyBattleDemo.cs
// Author   : SK
// Created  : 2014/11/7
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class ProxyBattleDemo
{
	private const string NAME = "Prefabs/Module/Battle/BattleDemoView";

	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule( NAME ,UILayerType.DefaultModule, true);
		var controller = ui.GetMissingComponent<BattleDemoController>();
		controller.Open ();
	}

	public static void Close()
	{
		UIModuleManager.Instance.CloseModule( NAME );
	}

	public static void Hide()
	{
		UIModuleManager.Instance.HideModule( NAME );
	}
}

