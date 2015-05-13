// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentShopTipGroup.cs
// Author   : willson
// Created  : 2015/3/3 
// Porpuse  : 
// **********************************************************************

using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.player.msg;

public class EquipmentShopTipGroup : EquipmentTipGroup
{
	public string extraInfo;

	override protected void initUI(PackItemDto itemDto)
	{
		Equipment item = _dto.item as Equipment;
		
		addName(itemDto);
		addSpaceHeight();
		
		AddIntroductionlbl(item);
		addSpaceHeight();

		if(!string.IsNullOrEmpty(extraInfo))
			addLabel(extraInfo);
		
		addSpaceHeight();
		AddDescriptionlbl(item);
	}
}