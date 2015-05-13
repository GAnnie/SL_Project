// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BackpackItemCellController.cs
// Author   : willson
// Created  : 2015/1/12 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;

public class BackpackOrWarehouseItemCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private BackpackItemCell _view;
	private ItemCellController _cell;

	private int _packEnum;

	private System.Action<BackpackOrWarehouseItemCellController> _onClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<BackpackItemCell> ();
		_view.Setup(this.transform);
		_view.LockSprite.enabled = false;

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		//UIHelper.AdjustDepth(module,3);

		_cell.InitView();

		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		UIEventTrigger trigger = _cell.DragDropItem.AddMissingComponent<UIEventTrigger>();
		if(trigger)
		{
			EventDelegate.Set(trigger.onDoubleClick,OnDoubleClickItem);
			EventDelegate.Set(trigger.onPress,OnPressItem);
			EventDelegate.Set(trigger.onRelease,OnReleaseItem);
		}
	}

	public void SetData(int packEnum,int index,System.Action<BackpackOrWarehouseItemCellController> onClickCallBack)
	{
		_packEnum = packEnum;

		Index = index;
		_onClickCallBack = onClickCallBack;

		PackItemDto dto = null;
		if(packEnum == H1Item.PackEnum_Backpack)
			dto = BackpackModel.Instance.GetItemByIndex(index);
		else if(packEnum == H1Item.PackEnum_Warehouse)
			dto = WarehouseModel.Instance.GetItemByIndex(index);

		if (TradeDataModel.debugFirstSta && dto == null) {
			TradeDataModel.debugFirstSta = false;
			GameDebuger.OrangeDebugLog("SetData Method -> Set the itemcell data is NULL");
		}

		_cell.SetPackId(packEnum);

		// dto = null
		_cell.SetData(dto,SelectItem);
		//_cell.SetDragDropItem(true);
	}

	public PackItemDto GetData()
	{
		return _cell.GetData();
	}

	public void SetLock(bool b)
	{
		_view.LockSprite.enabled = b;
	}

	public bool IsLock()
	{
		return _view.LockSprite.enabled;
	}

	public int Index
	{ 
		get
		{
			return _cell.Index;
		}

		set
		{
			_cell.Index = value;
		}
	}

    public void SelectItem()
    {
        if (_packEnum == H1Item.PackEnum_Backpack)
        {
            ProxyItemTipsModule.Open(_cell);
        }
        else if (_packEnum == H1Item.PackEnum_Warehouse)
        {
            ProxyItemTipsModule.Open(_cell, false);
        }

        if (_onClickCallBack != null)
        {
            _onClickCallBack(this);
        }
    }

	public void SelectItem(ItemCellController cell)
	{
		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			ProxyItemTipsModule.Open(cell);
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			ProxyItemTipsModule.Open(cell,false);
		}

		if(_onClickCallBack != null)
		{
			_onClickCallBack(this);
		}
	}

	public void OnDoubleClickItem()
	{
		if(_cell.GetData() != null)
		{
			ProxyItemTipsModule.Close();
			if(_packEnum == H1Item.PackEnum_Backpack)
			{
				if(ItemsContainerConst.ModuleType == ItemsContainerConst.ModuleType_Backpack)
				{
					//背包模式,穿戴装备 : 无改模式
				}
				else if(ItemsContainerConst.ModuleType == ItemsContainerConst.ModuleType_Warehouse)
				{
					//仓库背包模式,移入仓库
					BackpackModel.Instance.MoveToWarehouse(_cell.GetData());
				}

			}
			else if(_packEnum == H1Item.PackEnum_Warehouse)
			{
				ServiceRequestAction.requestServer(WarehouseService.moveTo(_cell.GetData().index,-1));
			}
		}
	}

	#region 拖拽实现
	private bool isPress = false;
	private int isPressTimer = 0;

	void Update () 
	{
		if(isPress)
		{
			if(++isPressTimer == 15)
			{
				_cell.SetDragDropItem(true);
				_cell.UpDragItem();
			}
		}
		else
		{
			isPressTimer = 0;
		}
	}
	
	public void OnPressItem()
	{
		if(_cell.GetData() != null)
		{
			isPressTimer = 0;
			isPress = true;
		}
	}

	public void OnReleaseItem()
	{
		isPress = false;
		isPressTimer = 0;

		_cell.SetDragDropItem(false);
		_cell.DownDragItem();
	}
	#endregion

	public void Dispose()
	{
		
	}
}