// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  CopperWinUIController.cs
// Author   : willson
// Created  : 2015/1/30 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class CopperWinUIController : WealthBuyWinUIController
{
	private List<ShopItem> _shopItems;

	public CopperWinUIController()
	{
		_shopItems = new List<ShopItem>();
	}

	public void SetData()
	{
		_shopItems.Clear();
		int shopId = 303;
		//Shop shop = DataCache.getDtoByCls<Shop>(shopId);
		
		List<ShopItem> items = DataCache.getArrayByClsWithoutSort<ShopItem>();
		int icon = 1;
		for(int index = 0;index < items.Count;index++)
		{
			if(items[index].shopId == shopId)
			{
				_shopItems.Add(items[index]);
				AddWealthCell(icon - 1,"tong" + icon,ItemIconConst.IngotAltas,items[index].virtualCount,ItemIconConst.CopperAltas,(int)CurrencyExchange.IngotToCopper((long)items[index].virtualCount));
				icon++;
			}
		}
	}

	protected override void OnBuy(int index)
	{
		ProxyPayModule.Close();
		
		ShopItem item = _shopItems[index];

		if(BackpackModel.Instance.IsFull())
		{
			TipManager.AddTip("物品栏已满,无法购买");
			return;
		}

		/*
		if(MallShoppingModel.Instance.GetCount(item) != -1 && MallShoppingModel.Instance.GetCount(item) == 0)
		{
			TipManager.AddTip("该物品本周购买次数已达到上限，无法继续购买");
			return;
		}
		*/
		if(PlayerModel.Instance.isEnoughIngot(item.virtualCount, true))
		{
			ServiceRequestAction.requestServer(ShopService.buy(item.id,1),"buy",
			(e)=>{
				GameLogicHelper.HandlerItemTipDto(e as ItemTipDto, true);
			});
		}
	}
}