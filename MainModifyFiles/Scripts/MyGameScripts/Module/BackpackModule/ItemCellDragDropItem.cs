// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemCellDragDropItem.cs
// Author   : willson
// Created  : 2015/1/14 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.equipment.data;

public class ItemCellDragDropItem : UIDragDropItem
{
	public ItemCellController fromItemCell;
	private ItemCellController toItemCell;

	protected override void StartDragging ()
	{
		if (!mDragging)
		{
			fromItemCell.StartDragging();
		}

		base.StartDragging();
	}

	protected override void OnDragDropRelease (GameObject surface)
	{
		fromItemCell.ReleaseDragging();

		if(surface != null)
		{
			toItemCell = surface.GetComponentInParent<ItemCellController>();
			if(toItemCell != null)
			{
				MoveTo();
			}
		}

		base.OnDragDropRelease(surface);

		fromItemCell.ResetDragging();
	}

	private void MoveTo()
	{
		if(fromItemCell.GetPackId() == toItemCell.GetPackId())
		{
			// if 背包 
			if(fromItemCell.GetPackId() == H1Item.PackEnum_Backpack)
			{
				ServiceRequestAction.requestServer(BackpackService.move(fromItemCell.GetData().index,toItemCell.Index),"",OnMoveToSuccess);
			}
			// if 仓库 
			else if(fromItemCell.GetPackId() == H1Item.PackEnum_Warehouse)
			{
				ServiceRequestAction.requestServer(WarehouseService.move(fromItemCell.GetData().index,toItemCell.Index),"",OnMoveToSuccess);
			}
		}
		else
		{
			// if 背包 to 仓库 
			if(fromItemCell.GetPackId() == H1Item.PackEnum_Backpack && toItemCell.GetPackId() == H1Item.PackEnum_Warehouse)
			{
				ServiceRequestAction.requestServer(BackpackService.moveTo(fromItemCell.GetData().index,toItemCell.Index),"",OnMoveToSuccess);
			}
			// if 仓库 to 背包 
			else if(fromItemCell.GetPackId() == H1Item.PackEnum_Warehouse && toItemCell.GetPackId() == H1Item.PackEnum_Backpack)
			{
				ServiceRequestAction.requestServer(WarehouseService.moveTo(fromItemCell.GetData().index,toItemCell.Index),"",OnMoveToSuccess);
			}
			// 装备 背包 to 身上
			else if(fromItemCell.GetPackId() == H1Item.PackEnum_Backpack && toItemCell.GetPackId() == ItemCellController.PackEnum_Equipment)
			{
				if(fromItemCell.GetData() != null && fromItemCell.GetData().item != null && fromItemCell.GetData().item.itemType == H1Item.ItemTypeEnum_Equipment)
				{
					Equipment equip = fromItemCell.GetData().item as Equipment;
					if(toItemCell.Index == equip.equipPartType && EquipmentModel.CanWearEquip(fromItemCell.GetData()))
					{
						fromItemCell.enabledIconSprite = false;
						BackpackModel.Instance.EquipWear(fromItemCell.GetData());
					}
				}
				return;
			}
			// 装备 身上 to 背包
			else if(fromItemCell.GetPackId() == ItemCellController.PackEnum_Equipment && toItemCell.GetPackId() == H1Item.PackEnum_Backpack)
			{
				if(toItemCell.GetData() != null && toItemCell.GetData().item != null)
				{
					Equipment equip = toItemCell.GetData().item as Equipment;
					if(toItemCell.GetData().item.itemType == H1Item.ItemTypeEnum_Equipment)
					{
						if(fromItemCell.Index == equip.equipPartType && EquipmentModel.CanWearEquip(toItemCell.GetData()))
						{
							fromItemCell.enabledIconSprite = false;
							BackpackModel.Instance.EquipWear(toItemCell.GetData());
						}
					}
					else
					{

					}
				}
				else if(toItemCell.GetData() == null)
				{
					fromItemCell.enabledIconSprite = false;
					BackpackModel.Instance.EquipTakeoff(fromItemCell.GetData(),toItemCell.Index);
				}
				return;
			}
		}

		// 先换在请求,交互 item,根据通知下发修复数据
		PackItemDto from = fromItemCell.GetData();
		fromItemCell.SetData(toItemCell.GetData());
		toItemCell.SetData(from);
	}

	private void OnMoveToSuccess(GeneralResponse e)
	{
		if(fromItemCell.GetData() != null)
			fromItemCell.GetData().index = fromItemCell.Index;

		if(toItemCell.GetData() != null)
			toItemCell.GetData().index = toItemCell.Index;
	}
}