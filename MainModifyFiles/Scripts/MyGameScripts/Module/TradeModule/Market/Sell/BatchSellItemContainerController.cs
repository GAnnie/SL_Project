// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemContainerController.cs
// Author   : willson
// Created  : 2015/1/10 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.msg;
using System.Collections.Generic;

public class BatchSellItemContainerController : MonoBehaviourBase,IViewController
{
	private const string BackpackItemCellInMarketSellName = "Prefabs/Module/TradeModule/Market/BackpackItemCellInMarketSellView";

	private ItemContainer _view;
	private int _packEnum;

	private List<MarketSellItemCellController> _cells;
	private System.Action<MarketSellItemCellController> _onClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemContainer> ();
		_view.Setup(this.transform);

		_cells = new List<MarketSellItemCellController>();
	}

	public void RegisterEvent()
	{
	}

	public void SetData(int packEnum,int page,int pageCapability,System.Action<MarketSellItemCellController> onClickCallBack)
	{
		_packEnum = packEnum;
		_view.PageInfo.page = page;
		_onClickCallBack = onClickCallBack;

		int index = 0;
		while(index < pageCapability)
		{
			AddBackpackOrWarehouseItemCell(packEnum,false,index);
			index++;
		}

		while(index < TradeDataModel.maxBatchSellPageCapability)
		{
			AddBackpackOrWarehouseItemCell(packEnum,true,index);
			index++;
		}

		/*
		if(packEnum == H1Item.PackEnum_Backpack)
		{
			BackpackModel.Instance.OnUpdateItem += OnUpdateItem;
			BackpackModel.Instance.OnDeleteItem += OnDeleteItem;
		}
		else if(packEnum == H1Item.PackEnum_Warehouse)
		{
			WarehouseModel.Instance.OnUpdateItem += OnUpdateItem;
			WarehouseModel.Instance.OnDeleteItem += OnDeleteItem;
		}
		*/
	}

	private void AddBackpackOrWarehouseItemCell(int packEnum,bool isLock,int index)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(BackpackItemCellInMarketSellName) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemGrid.gameObject, prefab);
		MarketSellItemCellController cell = module.GetMissingComponent<MarketSellItemCellController>();
		cell.InitView();

		int itemIndex = (_view.PageInfo.page - 1)*TradeDataModel.maxBatchSellPageCapability + index;

		cell.SetDataInBatchSellItem(packEnum, itemIndex, _onClickCallBack);
		cell.SetLock(isLock);

		cell.name = string.Format("{0}_{1}", cell.Index, cell.name);

		_cells.Add(cell);
	}

	private void OnUpdateItem(PackItemDto dto)
	{
		if(_cells[0].Index <= dto.index && dto.index <= _cells[_cells.Count - 1].Index)
		{
			int index = dto.index % TradeDataModel.maxBatchSellPageCapability;
			_cells[index].SetData(_packEnum,dto.index,null);
		}
	}

	private void OnDeleteItem(int index)
	{
		if(_cells[0].Index <= index && index <= _cells[_cells.Count - 1].Index)
		{
			int cellIndex = index % TradeDataModel.maxBatchSellPageCapability;
			_cells[cellIndex].SetData(_packEnum,index,null);
		}
	}

	public void AddCapability(int capability)
	{
		// 修改lock状态
		for(int index = 0;index < _cells.Count;index++)
		{
			if(_cells[index].Index < capability)
			{
				_cells[index].SetLock(false);
			}
		}
	}

	public void SelectItem(MarketSellItemCellController cell)
	{
		GameDebuger.OrangeDebugLog(string.Format("选择高亮 name cell -> {0}", cell.GetData().item.name));
	}

	public void Dispose()
	{
		/*
		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			BackpackModel.Instance.OnUpdateItem -= OnUpdateItem;
			BackpackModel.Instance.OnDeleteItem -= OnDeleteItem;
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			WarehouseModel.Instance.OnUpdateItem -= OnUpdateItem;
			WarehouseModel.Instance.OnDeleteItem -= OnDeleteItem;
		}
		*/
	}
}