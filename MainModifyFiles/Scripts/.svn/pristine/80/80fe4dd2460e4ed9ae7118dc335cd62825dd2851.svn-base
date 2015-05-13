// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  JoystickModule.cs
// Author   : willson
// Created  : 2014/12/4 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class JoystickModule
{
	private static readonly JoystickModule instance = new JoystickModule();
	public static JoystickModule  Instance
	{
		get 
		{
			return instance;
		}
	}

	private const string NAME = "Prefabs/Module/JoystickModule/JoystickModule";
	private GameObject _JoystickModuleObj;
	public static bool DisableMove = false;

	public void Setup()
	{
		if (_JoystickModuleObj == null)
		{
			GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab (NAME) as GameObject;
			_JoystickModuleObj = NGUITools.AddChild (LayerManager.Instance.uiModuleRoot, modulePrefab);
			_JoystickModuleObj.name = "JoystickModule";
		}
	}

	public GameObject GetJoystickModuleObj()
	{
		return _JoystickModuleObj;
	}

	public void SetActive(bool active)
	{
		if(_JoystickModuleObj)
			_JoystickModuleObj.SetActive(active);
	}
}