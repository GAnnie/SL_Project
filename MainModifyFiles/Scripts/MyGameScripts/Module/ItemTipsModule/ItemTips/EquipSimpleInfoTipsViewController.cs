// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipSimpleInfoTipsViewController.cs
// Author   : willson
// Created  : 2015/2/10 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.equipment.data;

public class EquipSimpleInfoTipsViewController : EquipmentTipsViewController
{
	protected override void ShowTips ()
	{
		Equipment item = _dto.item as Equipment;
		AddIntroductionlbl(item);
		AddDescriptionlbl(item);
	}
}