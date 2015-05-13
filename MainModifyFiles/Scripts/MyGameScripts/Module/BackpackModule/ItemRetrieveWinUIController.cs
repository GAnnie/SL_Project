// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemRetrieveWinUIController.cs
// Author   : willson
// Created  : 2015/1/24 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.services;

public class ItemRetrieveWinUIController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private ItemRetrieveWinUI _view;
	private List<ItemCellController> _cells;
	private ItemCellController _currCell;

	private CostButton _costBtn;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemRetrieveWinUI>();
		_view.Setup(this.transform);

		_cells = new List<ItemCellController>();
		for(int index = 0;index < 25;index++)
		{
			AddItemCell();
		}

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);

		_costBtn = UIHelper.CreateCostBtn(_view.OptBtnPos,"找回","ICON_1",OnResume);
	}

	private void AddItemCell()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemsGrid.gameObject,prefab);
		ItemCellController cell = module.GetMissingComponent<ItemCellController>();
		UIHelper.AdjustDepth(module,3);
		
		cell.InitView();

		_cells.Add(cell);
	}

	public void SetData(List<PackItemDto> items)
	{
		for(int index = 0;index < items.Count;index++)
		{
			if(index < _cells.Count)
			{
				_cells[index].SetData(items[index],OnItemClick);
			}
			else
			{
				break;
			}
		}
	}

	private void OnItemClick(ItemCellController cell)
	{
		if(cell.GetData() == null)
			return;

		_costBtn.Cost = 0;
		_currCell = cell;
		if(_currCell != null)
		{
			for(int index = 0;index < _cells.Count;index++)
			{
				_cells[index].isSelect = _currCell == _cells[index];
			}

			ProxyItemTipsModule.Open(cell.GetData(),cell.gameObject,false);

			Equipment equip = _currCell.GetData().item as Equipment;
			if(equip != null)
			{
				ItemResume cost = DataCache.getDtoByCls<ItemResume>(equip.equipLevel);
				if(cost != null)
				{
					_costBtn.Cost = cost.ingot;
				}
			}
		}
	}

	private void OnResume()
	{
		if(_currCell == null || _currCell.GetData() == null)
		{
			TipManager.AddTip("请选择要找回的装备");
			return;
		}

		if(PlayerModel.Instance.isEnoughIngot(_costBtn.Cost,true))
		{
			ServiceRequestAction.requestServer(BackpackService.resume(_currCell.GetData().uniqueId),"resume", 
			(e) => {
				TipManager.AddTip(string.Format("成功找回{0}",_currCell.GetData().item.name));
				_currCell.isSelect = false;
				_currCell.SetData(null);
				_currCell = null;
			});
		}
	}

	public void OnClose()
	{
		ProxyItemRetrieveModule.Close();
	}

	public void Dispose()
	{
	}
}