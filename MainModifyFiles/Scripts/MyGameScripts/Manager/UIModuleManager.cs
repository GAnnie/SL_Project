// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  UIModuleManager.cs
// Author   : jiaye.lin
// Created  : 2014/11/26
// Purpose  : 
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIModuleManager
{
	private static readonly UIModuleManager instance = new UIModuleManager();
   
	public static UIModuleManager Instance
	{
		get
		{
			return instance; 
		}
	}

	private Dictionary< string, GameObject > _moduleCacheDic; //用于缓存当前UI模块的GameObject对象
	private Dictionary< int,string > _layerCacheDic; 		  //用于缓存不同层当前模块的名称

	private UIModuleManager(){
		_moduleCacheDic = new Dictionary<string, GameObject>();
		_layerCacheDic = new Dictionary<int, string>();
	}
	
	public GameObject OpenFunModule(string moduleName,int depth, bool addBgMask, bool bgMaskClose = true)
	{
		if(_layerCacheDic.ContainsKey(depth)){
			string layerModuleName = _layerCacheDic[depth];
			if(!string.IsNullOrEmpty(layerModuleName) && moduleName != layerModuleName){
				//先关闭当前层模块
				CloseModule(layerModuleName);
				_layerCacheDic[depth] = "";
			}
			_layerCacheDic[depth] = moduleName;
		}else
			_layerCacheDic.Add(depth,moduleName);

		if(!_moduleCacheDic.ContainsKey(moduleName))
		{
			GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab( moduleName ) as GameObject;
			GameObject module = NGUITools.AddChild(LayerManager.Instance.uiModuleRoot,modulePrefab);
			module.AddMissingComponent<UIPanel>();
			NGUITools.AdjustDepth(module,depth);

			if (addBgMask)
			{
				AddBgMask(moduleName, module, bgMaskClose);
			}

			_moduleCacheDic.Add(moduleName, module);
			return module;
		}
		else
		{
			_moduleCacheDic[moduleName].SetActive(true);
			return _moduleCacheDic[moduleName];
		}	
	}

	//用于当前面板中创建子模块面板
	public GameObject AddChildPanel(string moduleName,Transform parent,int adjustment = 1){
		if(parent == null) return null;

		GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab( moduleName ) as GameObject;
		GameObject module = NGUITools.AddChild(parent.gameObject,modulePrefab);
		UIPanel parentPanel = UIPanel.Find(parent);
		if(parentPanel != null){
			NGUITools.AdjustDepth(module,parentPanel.depth + adjustment);
		}else
			NGUITools.AdjustDepth(module,adjustment);

		return module;
	}

	public void CloseModule(string moduleName)
	{
		GameObject module = null;
		_moduleCacheDic.TryGetValue( moduleName, out module );
		if (module != null){
			IViewController viewController = GetViewController(module);
			if (viewController != null)
			{
				viewController.Dispose();
			}

			_moduleCacheDic.Remove(moduleName);

			GameObject.Destroy(module);
			
			//如果剩余内存小于50mb,才进行回收
			long memory = BaoyugameSdk.getFreeMemory() / 1024;	
			if (memory < 50){
				Resources.UnloadUnusedAssets();
				System.GC.Collect();
			}
		}
	}

	private IViewController GetViewController(GameObject module)
	{
		MonoBehaviour[] list = module.GetComponents<MonoBehaviour>();
		for (int i=0,len=list.Length; i<len; i++)
		{
			MonoBehaviour mono = list[i];
			if (mono is IViewController)
			{
				return mono as IViewController;
			}
		}
		return null;
	}

	public bool ContainsModule(string moduleName)
	{
		return _moduleCacheDic.ContainsKey(moduleName);
	}
	
	public GameObject HideModule(string moduleName)
	{
		GameObject module = null;
		_moduleCacheDic.TryGetValue( moduleName, out module );
		if (module != null){
			module.SetActive(false);
		}
		
		return module;
	}

    public GameObject GetModule(string moduleName)
    {
        GameObject module = null;
        _moduleCacheDic.TryGetValue(moduleName, out module);
        return module;
    }

	private void AddBgMask(string moduleName, GameObject module, bool bgMaskClose)
	{
		GameObject bgMask = NGUITools.AddChild(module, (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BaseUI/ModuleBgBoxCollider"));

		if (bgMaskClose)
		{
			UIEventTrigger button = bgMask.GetMissingComponent<UIEventTrigger>();
			EventDelegate.Set (button.onClick, () => {
				CloseModule(moduleName);
			});
		}
		UIWidget uiWidget = bgMask.GetMissingComponent<UIWidget>();
		uiWidget.depth = -1;
		uiWidget.autoResizeBoxCollider = true;
		uiWidget.SetAnchor(module,-10,-10,10,10);
		NGUITools.AddWidgetCollider(bgMask);
	}
}

