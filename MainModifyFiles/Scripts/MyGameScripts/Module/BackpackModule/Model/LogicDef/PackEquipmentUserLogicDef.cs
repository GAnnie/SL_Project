// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PackEquipmentUserLogicDef.cs
// Author   : willson
// Created  : 2015/2/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;

public class PackEquipmentUserLogicDef :IPropUseLogic
{
	private PackItemDto _itemDto;
	
	public PackEquipmentUserLogicDef()
	{
	}
	
	public bool usePropByPos(PackItemDto packItem)
	{
		EquipmentExtraDto extra = packItem.extra as EquipmentExtraDto;
		if(extra != null && extra.hasIdentified)
		{
			if(extra.duration == 0)
			{
				EquipmentModel.Instance.Repair(packItem);
			}
			else
			{
				BackpackModel.Instance.EquipWear(packItem);
			}
		}
		else
		{
			//TipManager.AddTip("装备未鉴定");
			ServiceRequestAction.requestServer(EquipmentService.identifyByProps(packItem.uniqueId),"identifyByProps", 
			(e) => {
				if(e is PackItemDto)
				{
					TipManager.AddTip("装备鉴定成功");
					PackItemDto itemDto = e as PackItemDto;
					BackpackModel.Instance.UpdateItem(itemDto);
				}
				else if(e is EquipmentSmithCostDto)
				{
					EquipmentSmithCostDto dto = e as EquipmentSmithCostDto;
					Equipment equip = packItem.item as Equipment;
					if(equip.identifyMaterials != null && equip.identifyMaterials.Count > 0)
					{
						EquipmentSmithMaterial material = equip.identifyMaterials[0];
						if(material.selectionItemIds != null && material.selectionItemIds.Count > 0)
						{
							ItemDto item = new ItemDto();
							item.itemId = material.selectionItemIds[0];
							item.itemCount = material.itemCount;
						
							List<ItemDto> items = new List<ItemDto>();
							items.Add(item);

							ProxyTipsModule.Open("鉴定",items,dto.identifyIngot,(ingot)=>{
								ServiceRequestAction.requestServer(EquipmentService.identify(packItem.uniqueId,dto.identifyIngot),"identify", 
								(ee) => {
									if(ee is PackItemDto)
									{
										TipManager.AddTip("装备鉴定成功");
										PackItemDto itemDto = ee as PackItemDto;
										BackpackModel.Instance.UpdateItem(itemDto);
									}
									else if(ee is EquipmentSmithCostDto)
									{
										TipManager.AddTip("鉴定价格发生变动,鉴定失败");
									}
								});
							});
						}
					}
				}
			});
		}

		return false;
	}
}
