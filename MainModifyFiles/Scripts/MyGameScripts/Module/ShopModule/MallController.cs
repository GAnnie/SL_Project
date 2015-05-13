// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MallController.cs
// Author   : willson
// Created  : 2015/1/29 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.shop.data;

public interface MallController
{
	void OnWealthChanged(WealthNotify notify);
	bool CanBuy(ShopItem dto,int count,bool isTips);
}