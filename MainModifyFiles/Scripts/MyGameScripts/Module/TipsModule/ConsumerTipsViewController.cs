// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ConsumerTipsViewController.cs
// Author   : willson
// Created  : 2015/1/19 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ConsumerTipsViewController : MonoBehaviourBase,IViewController
{
	private const string ItemCellExName = "Prefabs/Module/EquipmentOptModule/ItemCellEx";

	private ConsumerTipsView _view;

	private List<ConsumerTipsItemCellController> _cells;
	private List<ItemDto> _costItem;

	private UIButton _optBtn;
	private CostButton _optCostBtn;

	private System.Action<List<ItemDto>> _OnOptBtnClick;

	private int _ingot;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ConsumerTipsView> ();
		_view.Setup(this.transform);

		_cells = new List<ConsumerTipsItemCellController>();
		_costItem = new List<ItemDto>();
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseButton.onClick,OnClose);
		_optBtn = UIHelper.CreateBaseBtn(_view.OptButtonPos,"确定",OnOptBtn);
		_optCostBtn = UIHelper.CreateCostBtn(_view.OptButtonPos,"换取",ItemIconConst.IngotAltas,OnOptBtn);
	}

	public void SetData(string tips,int itemId,int needCount,System.Action<List<ItemDto>> OnOptBtnClick)
	{
		_ingot = 0;
		_OnOptBtnClick = OnOptBtnClick;
		_view.TipsLabel.text = tips;

		ItemDto dto = new ItemDto();
		dto.itemId = itemId;
		dto.itemCount = needCount;

		AddConsumerTipsItemCell(dto);
		SetOptBtn();
	}

	public void SetData(string tips,List<ItemDto> items,int ingot,System.Action<List<ItemDto>> OnOptBtnClick)
	{
		_ingot = ingot;
		_OnOptBtnClick = OnOptBtnClick;
		_view.TipsLabel.text = tips;
		
		for(int index = 0;index < items.Count;index++)
		{
			AddConsumerTipsItemCell(items[index]);
		}

		if(items.Count > 2)
			_view.ContentBg.width = _view.ContentBg.width + (items.Count - 2)*120;

		SetOptBtn();
	}

	private void AddConsumerTipsItemCell(ItemDto dto)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemCellGrid.gameObject,prefab);
		ConsumerTipsItemCellController cell = module.GetMissingComponent<ConsumerTipsItemCellController>();
		cell.InitView();
		cell.SetData(dto.itemId,dto.itemCount);
		_cells.Add(cell);
		_view.ItemCellGrid.Reposition();
	}

	private void SetOptBtn()
	{
		_costItem.Clear();
		if(_ingot == 0)
		{
			for(int index = 0;index < _cells.Count;index++)
			{
				_ingot += _cells[index].GetIngot();
				if(_cells[index].GetCost() != null)
				{
					_costItem.Add(_cells[index].GetCost());
				}
			}
		}

		if(_ingot > 0)
		{
			ItemDto ingotDto = new ItemDto();
			ingotDto.itemId = H1VirtualItem.VirtualItemEnum_INGOT;
			ingotDto.itemCount = _ingot;
			_costItem.Add(ingotDto);

			_optBtn.gameObject.SetActive(false);
			_optCostBtn.gameObject.SetActive(true);
			//_optBtn.GetComponentInChildren<UILabel>().text = string.Format("{0}{1}换取",_ingot,ItemIconConst.Ingot);
			_optCostBtn.Cost = _ingot;
		}
		else
		{
			_optBtn.gameObject.SetActive(true);
			_optCostBtn.gameObject.SetActive(false);
		}
	}
	
	private void OnOptBtn()
	{
		if(PlayerModel.Instance.isEnoughIngot(_ingot, true))
		{
			if(_OnOptBtnClick != null)
				_OnOptBtnClick(_costItem);
		}

		OnClose ();
	}

	public void OnClose()
	{
		ProxyTipsModule.Close();
	}

	public void Dispose()
	{

	}
}

