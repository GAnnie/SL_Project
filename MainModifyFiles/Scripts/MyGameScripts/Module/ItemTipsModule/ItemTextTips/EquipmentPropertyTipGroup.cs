// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentPropertyTipGroup.cs
// Author   : willson
// Created  : 2015/2/10 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.player.msg;

public class EquipmentPropertyTipGroup : EquipmentTipGroup
{
	override protected void initUI(PackItemDto itemDto)
	{
		Equipment item = _dto.item as Equipment;

		AddPropertylbl(item);
		AddGemlbl(item);
	}
}