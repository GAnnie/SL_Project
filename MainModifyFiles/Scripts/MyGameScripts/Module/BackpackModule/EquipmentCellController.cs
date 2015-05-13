// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentCellController.cs
// Author   : willson
// Created  : 2015/1/31 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.services;

public class EquipmentCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private EquipmentCell _view;
	private ItemCellController _cell;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentCell>();
		_view.Setup(this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		UIHelper.AdjustDepth(module,2);
		
		_cell.InitView();
		_cell.SetPackId(ItemCellController.PackEnum_Equipment);
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		UIEventTrigger trigger = _cell.DragDropItem.AddMissingComponent<UIEventTrigger>();
		if(trigger)
		{
			EventDelegate.Set(trigger.onPress,OnPressItem);
			EventDelegate.Set(trigger.onRelease,OnReleaseItem);
		}
	}

	public void SetData(int partType)
	{
		_view.PartIconSprite.spriteName = "part_" + partType;

		PackItemDto dto = BackpackModel.Instance.GetEquipByPartType(partType);
		_cell.Index = partType; // 用index来标示装备类型
		if(dto == null)
		{
			_view.PartIconSprite.enabled = true;
			_cell.SetData(null);
		}
		else
		{
			HidePartIcon();
			_cell.SetData(dto,OnClickItem);
		}
	}

	public void HidePartIcon()
	{
		_view.PartIconSprite.enabled = false;
	}

	private void OnClickItem(ItemCellController cell)
	{
		ProxyItemTipsModule.Open(_cell,true,takeoffEquipment);
	}

	private void takeoffEquipment(PackItemDto dto)
	{
		if(dto != null)
		{
			BackpackModel.Instance.EquipTakeoff(dto);
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