// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentOptWinUIController.cs
// Author   : willson
// Created  : 2015/1/20 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.player.msg;

public class EquipmentOptWinUIController : MonoBehaviourBase,IViewController
{
	private const string EquipmentManufacturingName = "Prefabs/Module/EquipmentOptModule/EquipmentManufacturingView";
	private const string EquipmentGemName = "Prefabs/Module/EquipmentOptModule/EquipmentGemView";
	private const string EquipmentPropertyName = "Prefabs/Module/EquipmentOptModule/EquipmentPropertyView";
	private const string EquipmentGemTransferViewName = "Prefabs/Module/EquipmentOptModule/EquipmentGemTransferView";

	private EquipmentOptWinUI _view;
	private List<TabBtnController> _tabBtnList;

	private EquipmentManufacturingViewController _equipmentManufacturingViewController;
	private EquipmentGemViewController _equipmentGemViewController;
	private EquipmentPropertyViewController _equipmentPropertyViewController;
	private EquipmentGemTransferViewController _equipmentGemTransferViewController;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentOptWinUI> ();
		_view.Setup(this.transform);

		InitTabBtn();

		RegisterEvent();
	}

	private void InitTabBtn()
	{
		int size = 4;
		_tabBtnList = new List<TabBtnController> (size);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i = 0; i < size; ++i) 
		{
			GameObject item = NGUITools.AddChild(_view.TabGroup.gameObject, tabBtnPrefab);
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_tabBtnList.Add(com);
			_tabBtnList[i].InitItem (i,OnSelectTabBtn);
		}
		
		_tabBtnList[0].SetBtnName("装备打造");
		_tabBtnList[1].SetBtnName("宝石镶嵌");
		_tabBtnList[2].SetBtnName("属性转换");
		_tabBtnList[3].SetBtnName("宝石转移");
	}

	public void OnSelectTabBtn (int index)
	{
		if (index == 0)
			OnSelectEquipmentManufacturingView();
		else if (index == 1)
			OnSelectEquipmentGemView();
		else if (index == 2)
			//OnSelect2View();
			OnSelectEquipmentPropertyView();
		else if (index == 3)
			OnSelectEquipmentGemMoveView();
	}

	private void UpdateTabBtnState (int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i)
		{
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}

	public void OnSelectEquipmentManufacturingView()
	{
		if(_equipmentManufacturingViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentManufacturingName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_equipmentManufacturingViewController = module.GetMissingComponent<EquipmentManufacturingViewController>();
			_equipmentManufacturingViewController.InitView();
		}
		_equipmentManufacturingViewController.SetActive(true);
		_equipmentManufacturingViewController.SetData();
		UpdateTabBtnState (0);

		if(_equipmentGemViewController != null)
			_equipmentGemViewController.SetActive(false);
		if(_equipmentPropertyViewController != null)
			_equipmentPropertyViewController.SetActive(false);
		if(_equipmentGemTransferViewController != null)
			_equipmentGemTransferViewController.SetActive(false);
	}

    public void SetEquipmentManufacturingGrade(int grade)
    {
        if (_equipmentManufacturingViewController != null)
            _equipmentManufacturingViewController.SetEquipment(grade);
    }

	public void OnSelectEquipmentGemView()
	{
		if(_equipmentGemViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentGemName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_equipmentGemViewController = module.GetMissingComponent<EquipmentGemViewController>();
			_equipmentGemViewController.InitView();
		}

		_equipmentGemViewController.SetActive(true);
		_equipmentGemViewController.SetData();
		UpdateTabBtnState (1);

		if(_equipmentManufacturingViewController != null)
			_equipmentManufacturingViewController.SetActive(false);
		if(_equipmentPropertyViewController != null)
			_equipmentPropertyViewController.SetActive(false);
		if(_equipmentGemTransferViewController != null)
			_equipmentGemTransferViewController.SetActive(false);
	}

	public void SetEquipmentGemCurrEquip(PackItemDto dto)
	{
		if(_equipmentGemViewController != null)
			_equipmentGemViewController.SelectEquip(dto);
	}

	public void OnSelectEquipmentPropertyView()
	{
		if(_equipmentPropertyViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentPropertyName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_equipmentPropertyViewController = module.GetMissingComponent<EquipmentPropertyViewController>();
			_equipmentPropertyViewController.InitView();
		}
		
		_equipmentPropertyViewController.SetActive(true);
		_equipmentPropertyViewController.SetData();
		UpdateTabBtnState (2);

		if(_equipmentGemViewController != null)
			_equipmentGemViewController.SetActive(false);
		if(_equipmentManufacturingViewController != null)
			_equipmentManufacturingViewController.SetActive(false);
		if(_equipmentGemTransferViewController != null)
			_equipmentGemTransferViewController.SetActive(false);
	}

	public void OnSelectEquipmentGemMoveView()
	{
		if(_equipmentGemTransferViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentGemTransferViewName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_equipmentGemTransferViewController = module.GetMissingComponent<EquipmentGemTransferViewController>();
			_equipmentGemTransferViewController.InitView();
		}

		_equipmentGemTransferViewController.SetActive(true);
		_equipmentGemTransferViewController.SetData();

		UpdateTabBtnState (3);

		if(_equipmentGemViewController != null)
			_equipmentGemViewController.SetActive(false);
		if(_equipmentManufacturingViewController != null)
			_equipmentManufacturingViewController.SetActive(false);
		if(_equipmentPropertyViewController != null)
			_equipmentPropertyViewController.SetActive(false);
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);
	}

	public void OnClose()
	{
		ProxyEquipmentOptModule.Close();
	}

	public void Dispose()
	{
		if(_equipmentManufacturingViewController != null)
			_equipmentManufacturingViewController.Dispose();

		if(_equipmentGemViewController != null)
			_equipmentGemViewController.Dispose();

		if(_equipmentPropertyViewController != null)
			_equipmentPropertyViewController.Dispose();
	}
}