// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WarehouseWinUIController.cs
// Author   : willson
// Created  : 2015/1/16 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;

public class WarehouseWinUIController : MonoBehaviourBase,IViewController
{
	private const string ItemsContainerViewName = "Prefabs/Module/BackpackModule/ItemsContainerView";

	private WarehouseWinUI _view;

	private ItemsContainerViewController _backpack;
	private ItemsContainerViewController _warehouse;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<WarehouseWinUI>();
		_view.Setup(this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemsContainerViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.BackpackPos,prefab);
		_backpack = module.GetMissingComponent<ItemsContainerViewController>();
		_backpack.InitView();
		_backpack.SetData(H1Item.PackEnum_Backpack,BackpackModel.Instance.GetDto().capability);

		module = GameObjectExt.AddChild(_view.WarehousePos,prefab);
		_warehouse = module.GetMissingComponent<ItemsContainerViewController>();
		_warehouse.InitView();
		_warehouse.SetData(H1Item.PackEnum_Warehouse,WarehouseModel.Instance.GetDto().capability);

		OnWealthChanged(PlayerModel.Instance.GetWealth());
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;

		UIHelper.CreateBaseBtn(_view.WarehouseArrangeBtnPos,"整理",OnWarehouseArrangeBtn);
		UIHelper.CreateBaseBtn(_view.BackpackArrangeBtnPos,"整理",OnBackpackArrangeBtn);

		EventDelegate.Set(_view.AddCopperBtn.onClick,OnAddCopperBtn);
		EventDelegate.Set(_view.AddSilverBtn.onClick,OnAddSilverBtn);

		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);
	}

	private void OnWealthChanged(WealthNotify notify)
	{
		_view.CopperValLbl.text = notify.copper.ToString();
		_view.SilverValLbl.text = notify.silver.ToString();
	}

	private long _backpackArrangeSpaceTime = 0;

	public void OnBackpackArrangeBtn()
	{
		long time = SystemTimeManager.Instance.GetUTCTimeStamp();
		int spaceTime = (int)(5 - (time - _backpackArrangeSpaceTime)/1000);
		if(spaceTime <= 0)
		{
			_backpackArrangeSpaceTime = time;
			BackpackModel.Instance.Sort();
		}
		else
		{
			TipManager.AddTip(string.Format("你刚刚进行了整理，请隔{0}秒再试",spaceTime));
		}
	}

	private long _warehouseArrangeSpaceTime = 0;

	public void OnWarehouseArrangeBtn()
	{
		long time = SystemTimeManager.Instance.GetUTCTimeStamp();
		int spaceTime = (int)(5 - (time - _warehouseArrangeSpaceTime)/1000);
		if(spaceTime <= 0)
		{
			_warehouseArrangeSpaceTime = time;
			WarehouseModel.Instance.Sort(_warehouse.GetCurrentPage());
		}
		else
		{
			TipManager.AddTip(string.Format("你刚刚进行了整理，请隔{0}秒再试",spaceTime));
		}
	}

	private void OnAddCopperBtn()
	{
		ProxyPayModule.OpenCopper();
	}
	
	private void OnAddSilverBtn()
	{
		ProxyPayModule.OpenSilver();
	}

	public void OnClose()
	{
		ProxyWarehouseModule.Close();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
		_backpack.Dispose();
		_warehouse.Dispose();
	}
}