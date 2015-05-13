// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemCellController.cs
// Author   : willson
// Created  : 2015/1/12 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using System.Collections.Generic;

public class ItemCellController : MonoBehaviourBase,IViewController
{
	// 客户端用来标示 _packId = 装备格子 (EquipmentCell),使用的
	public const int PackEnum_Equipment = 1000;

	private ItemCell _view;
	private ItemCellDragDropItem _dragDropItem;
	private PackItemDto _dto;
	private int _packId;
	
	private System.Action<ItemCellController> _onClickCallBack;

	public int Index{ get;set;}
	
	private bool _alwaysDisplayCount;
	private bool _canDisplayCount;

	private int _itemCount;

	public void InitView()
	{
		_packId = H1Item.PackEnum_Unknown;

		_view = gameObject.GetMissingComponent<ItemCell> ();
		_view.Setup(this.transform);

		_canDisplayCount = true;
		_alwaysDisplayCount = false;

		_dragDropItem = null;

		_view.SelectSprite.enabled = false;
		_view.CountLabel.text = "";
		_view.IconSprite.enabled = false;

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.DragDropItemBtn.onClick,OnClick);
	}

	public int GetPackId()
	{
		return _packId;
	}

	public void SetPackId(int packId)
	{
		_packId = packId;
	}

	public PackItemDto GetData()
	{
		return _dto;
	}

	public void AlwaysDisplayCount(bool b)
	{
		_alwaysDisplayCount = b;
	}

	public void CanDisplayCount(bool b)
	{
		_canDisplayCount = b;
	}

	public bool isGrey
	{
		get
		{
			return _view.IconSprite.isGrey;
		}
		set
		{
			if(_view.IconSprite.isGrey != value)
			{
				_view.IconSprite.isGrey = value;
			}
		}
	}

	public void SetData(PackItemDto dto)
	{
		_dto = dto;
		_view.DragDropItem.transform.localPosition = new Vector3(0,0,0);

		if(_dto != null)
		{
			//if(_dragDropItem != null)
			//	_dragDropItem.enabled = true;

			_itemCount = _dto.count;
			if(_canDisplayCount && (_alwaysDisplayCount || _dto.count > 1))
			{
				_view.CountLabel.text = _dto.count.ToString();
			}
			else
			{
				_view.CountLabel.text = "";
			}
			_view.IconSprite.enabled = true;

			if(_dto.item != null)
			{
				if(_view.IconSprite.atlas.GetSprite(_dto.item.icon) != null)
					_view.IconSprite.spriteName = _dto.item.icon;
				else
					_view.IconSprite.spriteName = "0";
			}
			else if(_dto.itemId < 100)
			{
				// 虚物
				H1VirtualItem item = DataCache.getDtoByCls<GeneralItem>(_dto.itemId) as H1VirtualItem;
				if(item != null)
				{
					if(_view.IconSprite.atlas.GetSprite(item.icon) != null)
						_view.IconSprite.spriteName = item.icon;
					else
						_view.IconSprite.spriteName = "0";
				}
			}
			_view.IconSprite.MakePixelPerfect();
		}
		else
		{
			//if(_dragDropItem != null)
			//	_dragDropItem.enabled = false;

			_view.CountLabel.text = "";
			_view.IconSprite.enabled = false;
			//_view.IconSprite.spriteName;
		}
	}

	public void SetData(PackItemDto dto,System.Action<ItemCellController> onClickCallBack)
	{
		SetData(dto);
		_onClickCallBack = onClickCallBack;
	}

	public int ItemCount
	{
		get
		{
			return _itemCount;
		}
		set
		{
			_itemCount = value;
			_view.CountLabel.text = _itemCount.ToString();
		}
	}

	public void SetDragDropItem(bool b)
	{
		if(_dragDropItem == null)
		{
			_dragDropItem = _view.DragDropItem.GetMissingComponent<ItemCellDragDropItem>();
			_dragDropItem.fromItemCell = this;
		}

		_dragDropItem.enabled = b && _dto != null;
	}

	public void UpDragItem()
	{
		_view.DragDropItem.transform.localPosition = new Vector3(5,5,0);
	}

	public void DownDragItem()
	{
		_view.DragDropItem.transform.localPosition = new Vector3(0,0,0);
	}

	public void StartDragging()
	{
		if(_view != null)
		{
			_view.IconSprite.depth += 5;
			_view.CountLabel.depth += 5;
		}
	}

	public void ReleaseDragging()
	{
		if(_view != null)
		{
			_view.IconSprite.depth -= 5;
			_view.CountLabel.depth -= 5;
		}
	}

	public void ResetDragging()
	{
		if(_view != null)
		{
			_view.DragDropItem.transform.localPosition = new Vector3(0,0,0);
		}
	}

	void OnClick()
	{
		if(_onClickCallBack != null)
			_onClickCallBack(this);
	}

	public GameObject DragDropItem
	{
		get{
			return _view.DragDropItem;
		}
	}

	public bool isSelect
	{
		get
		{
			return _view.SelectSprite.enabled;
		}
		set
		{
			if(_view.SelectSprite.enabled != value)
			{
				_view.SelectSprite.enabled = value;
			}
		}
	}

	public bool enabledIconSprite
	{
		get
		{
			return _view.IconSprite.enabled;
		}
		set
		{
			if(_view.IconSprite.enabled != value)
			{
				_view.IconSprite.enabled = value;
			}
		}
	}

	public void Dispose()
	{

	}
}