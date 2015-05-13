// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  UseItemCellController.cs
// Author   : willson
// Created  : 2015/3/14 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.msg;

public class UseItemCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";
	
	private UseItemCell _view;
	private ItemCellController _cell;

	private int _currCount;

	private PackItemDto _itemDto;
	private bool _isMultiple;

	private System.Action<UseItemCellController> _onClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<UseItemCell>();
		_view.Setup(this.transform);

		_isMultiple = false;

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		_cell.InitView();
		_cell.CanDisplayCount(false);

		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.RomeveBtn.onClick,OnRomeveBtn);
	}

	public void SetData(PackItemDto itemDto,System.Action<UseItemCellController> onClickCallBack,bool isMultiple = false)
	{
		_isMultiple = isMultiple;
		_onClickCallBack = onClickCallBack;

		SetData(itemDto);
	}

	public void SetData(PackItemDto itemDto)
	{
		_currCount = 0;
		_cell.isSelect = false;
		
		_itemDto = itemDto;
		
		_cell.SetData(_itemDto,OnItemClick);
		if(_itemDto == null)
		{
			_view.RomeveBtn.gameObject.SetActive(false);
			_view.CountLabel.text = "";
		}
		else
		{
			if(_isMultiple)
			{
				SetMultipleState();
			}
			else
			{
				if(_itemDto.count > 1)
				{
					_view.CountLabel.text = _itemDto.count.ToString();
				}
				else
				{
					_view.CountLabel.text = "";
				}
				_view.RomeveBtn.gameObject.SetActive(false);
			}
		}
	}

	private void SetMultipleState()
	{
		if(_itemDto.item.maxOverlay > 1)
		{
			_view.RomeveBtn.gameObject.SetActive(_currCount > 0);
			_view.CountLabel.text = string.Format("{0}/{1}",_currCount,_itemDto.count);
		}
		else
		{
			_view.RomeveBtn.gameObject.SetActive(false);
			_view.CountLabel.text = "";
		}
	}

	public PackItemDto GetData()
	{
		return _itemDto;
	}

	public void SelectSingle(bool b)
	{
		_cell.isSelect = b;
		_currCount = b? 1:0;
	}

	public void SelectMultiple()
	{
		if(_itemDto.item.maxOverlay == 1)
		{
			_cell.isSelect = !_cell.isSelect;
			if(_cell.isSelect)
			{
				_currCount = 1;
			}
			else
			{
				_currCount = 0;
			}
		}
		else
		{
			_cell.isSelect = true;
		}

		if(_currCount < _itemDto.count)
			_currCount++;
		if(_isMultiple)
		{
			SetMultipleState();
		}
	}

	private void OnItemClick(ItemCellController cell)
	{
		if(_onClickCallBack != null)
			_onClickCallBack(this);
	}

	private void OnRomeveBtn()
	{
	}

	public void Dispose()
	{
	}
}