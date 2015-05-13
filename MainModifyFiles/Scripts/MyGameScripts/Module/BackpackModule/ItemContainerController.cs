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

public class ItemContainerController : MonoBehaviourBase,IViewController
{
	private const string BackpackItemCellName = "Prefabs/Module/BackpackModule/BackpackItemCell";

	private ItemContainer _view;
	private int _packEnum;

	private List<BackpackOrWarehouseItemCellController> _cells;
	private System.Action<BackpackOrWarehouseItemCellController> _onClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemContainer> ();
		_view.Setup(this.transform);

		_cells = new List<BackpackOrWarehouseItemCellController>();
	}

	public void RegisterEvent()
	{
	}

	public void SetData(int packEnum,int page,int pageCapability,System.Action<BackpackOrWarehouseItemCellController> onClickCallBack)
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

		while(index < ItemsContainerConst.PageCapability)
		{
			AddBackpackOrWarehouseItemCell(packEnum,true,index);
			index++;
		}

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
	}

	private void AddBackpackOrWarehouseItemCell(int packEnum,bool isLock,int index)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( BackpackItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemGrid.gameObject,prefab);
		BackpackOrWarehouseItemCellController cell = module.GetMissingComponent<BackpackOrWarehouseItemCellController>();
		cell.InitView();

		int itemIndex = (_view.PageInfo.page - 1)*ItemsContainerConst.PageCapability + index;

		cell.SetData(packEnum,itemIndex,_onClickCallBack);
		cell.SetLock(isLock);

		cell.name = cell.Index.ToString();

		_cells.Add(cell);
	}

	private void OnUpdateItem(PackItemDto dto)
	{
		if(_cells[0].Index <= dto.index && dto.index <= _cells[_cells.Count - 1].Index)
		{
			int index = dto.index % ItemsContainerConst.PageCapability;
			_cells[index].SetData(_packEnum,dto.index,null);
		}
	}

	private void OnDeleteItem(int index)
	{
		if(_cells[0].Index <= index && index <= _cells[_cells.Count - 1].Index)
		{
			int cellIndex = index % ItemsContainerConst.PageCapability;
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

	public void SelectItem(BackpackOrWarehouseItemCellController cell)
	{

	}


    public BackpackOrWarehouseItemCellController GetItemCell(int index)
    {
        for(int i = 0;i < _cells.Count;i++)
        {
            if(_cells[i].Index == index)
            {
                return _cells[i];
            }
        }
        return null;
    }
	public void Dispose()
	{
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
	}
}