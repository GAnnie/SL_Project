// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemUseViewController.cs
// Author   : willson
// Created  : 2015/3/14 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using System.Collections.Generic;
using System;

public class ItemUseViewController : MonoBehaviour,IViewController 
{
	private const string UseItemCellName = "Prefabs/Module/ItemUseModule/UseItemCell";

	protected ItemUseView _view;

	protected UseLeftViewCell _leftView;

	protected PackItemDto _useDto;

	protected bool _isMultiple;
	protected bool _isBefore;
	protected bool _isCanReClick;

	protected List<PackItemDto> _items;
	protected List<UseItemCellController> _itemCellList;

	protected const int PAGE_COUNT = 12;
	protected Summary _summary;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemUseView> ();
		_view.Setup (this.transform);

		_itemCellList = new List<UseItemCellController>();

		InitLeftGroup();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Add(_view.CloseBtn.onClick,OnClose);
		EventDelegate.Add(_view.OptBtn.onClick,OnOptBtn);
	}

	virtual protected void InitLeftGroup()
	{
	}
	
	public void SetData(PackItemDto useDto,List<PackItemDto> items,bool isMultiple = false,bool isBefore = false,bool isCanReClick = true)
	{
		_useDto = useDto;
		_items = items;

		_isMultiple = isMultiple;
		_isBefore = isBefore;
		_isCanReClick = isCanReClick;

		int iTotalPage = (int)Math.Ceiling((double)_items.Count / PAGE_COUNT);
		_summary = Summary.create(_items.Count,iTotalPage,1,PAGE_COUNT);
		iTotalPage = iTotalPage == 0 ? 1:iTotalPage;

		AddItem(iTotalPage);

		_leftView.SetUseDto(useDto);
	}
	
	private void AddItem(int page)
	{
		for (int index =0,totalCount = PAGE_COUNT*page; index < totalCount; index++) 
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( UseItemCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.itemGrid.gameObject,prefab);
			UseItemCellController cell = module.GetMissingComponent<UseItemCellController>();
			cell.InitView();
			if(index < _items.Count)
			{
				cell.SetData(_items[index],OnItemClick,_isMultiple);
			}
			else
			{
				cell.SetData(null,OnItemClick,_isMultiple);
			}
			_itemCellList.Add(cell);
		}
	}

	private void OnItemClick(UseItemCellController cell)
	{
		if(cell.GetData() != null)
		{
			if(_isMultiple)
			{
				cell.SelectMultiple();
			}
			else
			{
				for(int index = 0;index < _itemCellList.Count;index++)
				{
					_itemCellList[index].SelectSingle(cell == _itemCellList[index]);
				}
			}
			_leftView.SetData(cell.GetData());
		}
	}

	virtual protected void OnOptBtn()
	{
	}

	private void OnClose()
	{
		ProxyItemUseModule.Close();
	}

	virtual public void Dispose()
	{
	}
}