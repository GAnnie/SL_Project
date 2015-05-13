// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PayWinUIController.cs
// Author   : willson
// Created  : 2015/1/30 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;

public class PayWinUIController : WealthBuyWinUIController
{
	private List<ShopItem> _shopItems;

	public PayWinUIController()
	{
		_shopItems = new List<ShopItem>();
	}

	public void SetData()
	{
		_shopItems.Clear();
		int shopId = 305;

		List<ShopItem> items = DataCache.getArrayByClsWithoutSort<ShopItem>();
		int icon = 1;
		for(int index = 0;index < items.Count;index++)
		{
			if(items[index].shopId == shopId)
			{
				_shopItems.Add(items[index]);
				AddWealthCell(icon - 1,"gold" + icon,ItemIconConst.RmbAltas,items[index].virtualCount,ItemIconConst.IngotAltas,items[index].itemCount);
				icon++;
			}
		}
	}
	
	protected override void OnBuy(int index)
	{
		ProxyPayModule.Close();
		
		ShopItem item = _shopItems[index];
		string cmd = "#add_ingot " + _shopItems[index].itemCount;
		ServiceRequestAction.requestServer (GmService.execute(cmd));
	}
}