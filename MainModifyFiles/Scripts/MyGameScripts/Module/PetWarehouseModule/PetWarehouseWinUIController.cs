// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetWarehouseWinUIController.cs
// Author   : willson
// Created  : 2015/3/20 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class PetWarehouseWinUIController : MonoBehaviourBase,IViewController
{
	private const string NPCPetWarehouseName = "Prefabs/Module/PetWarehouseModule/NPCPetWarehouseView";

	private PetWarehouseWinUI _view;

	private NPCPetWarehouseViewController _NPCPetWarehouseViewController;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetWarehouseWinUI>();
		_view.Setup(this.transform);

		OnSelectNPCPetWarehouseView();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Add(_view.CloseBtn.onClick,OnCloseBtn);
	}


	public void OnSelectNPCPetWarehouseView()
	{
		if(_NPCPetWarehouseViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( NPCPetWarehouseName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_NPCPetWarehouseViewController = module.GetMissingComponent<NPCPetWarehouseViewController>();
			_NPCPetWarehouseViewController.InitView();
		}
		_NPCPetWarehouseViewController.SetActive(true);
		_NPCPetWarehouseViewController.SetData();
		
		//if(_practiceSkillViewController != null) _practiceSkillViewController.SetActive(false);
		
		//UpdateTabBtnState (0);
	}

	private void OnCloseBtn()
	{
		ProxyPetWarehouseModule.Close();
	}

	public void Dispose()
	{
		if(_NPCPetWarehouseViewController != null)
			_NPCPetWarehouseViewController.Dispose();
	}
}