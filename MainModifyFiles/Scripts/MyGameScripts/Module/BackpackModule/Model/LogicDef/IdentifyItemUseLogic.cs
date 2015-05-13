// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  IdentifyItemUseLogic.cs
// Author   : willson
// Created  : 2015/3/16 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.equipment.data;

public class IdentifyItemUseLogic :IPropUseLogic
{
	private PackItemDto _itemDto;
	
	public IdentifyItemUseLogic()
	{
	}
	
	public bool usePropByPos(PackItemDto packItem)
	{
		ProxyItemUseModule.OpenIdentifyItem(packItem,BackpackModel.Instance.GetIdentifyList(packItem.itemId));
		return false;
	}
}
