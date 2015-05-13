// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NpcShopWinUIController.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.shop.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using UnityEngine;
using com.nucleus.h1.logic.services;

public class NpcShopWinUIController : ShopWinUIController
{
	public void SetData(int shopId)
	{
		_view.BuyWealthButton.gameObject.SetActive(true);

		Shop shop = DataCache.getDtoByCls<Shop>(shopId);
		if(shop != null)
		{
			_view.TitleSprite.spriteName = shop.res;
			_view.TitleSprite.MakePixelPerfect();
			_view.RefreshTipsLabel.text = shop.description;
		}

		_view.PayBtnPos.SetActive(false);
		_view.BuyBtnPos.transform.localPosition = new Vector3(0,_view.BuyBtnPos.transform.localPosition.y,0);
		
		_view.HaveIconSprite.spriteName = ItemIconConst.CopperAltas;
		_view.PriceIconSprite.spriteName = ItemIconConst.CopperAltas;
		OnWealthChanged(PlayerModel.Instance.GetWealth());

		List<ShopItem> items = DataCache.getArrayByClsWithoutSort<ShopItem>();

		for(int index = 0;index < items.Count;index++)
		{
			if(items[index].shopId == shopId && items[index].grade <= PlayerModel.Instance.GetPlayerLevel())
			{
				AddItemCell(items[index]);
			}
		}

		if(_itemList.Count > 0)
		{
			SelectItem(_itemList[0]);
		}
	}

	protected override void OnWealthChanged(WealthNotify notify)
	{
		_view.HaveLbl.text = MallShoppingModel.Instance.GetBold(notify.copper.ToString());
	}

	protected override void OnBuyBtn()
	{
		ShopItem dto = _currShopItem.GetData();
		int count = _buyCount;

		if(count == 0)
		{
			TipManager.AddTip("请输入购买数量");
			return;
		}

		if(BackpackModel.Instance.IsFull())
		{
			TipManager.AddTip("物品栏已满,无法购买");
			return;
		}

		if(MallShoppingModel.Instance.GetCount(dto) != -1 && MallShoppingModel.Instance.GetCount(dto) == 0)
		{
			TipManager.AddTip("该物品本周购买次数已达到上限，无法继续购买");
			return;
		}

		if(PlayerModel.Instance.isEnoughCopper(dto.virtualCount*count, true))
		{
			ServiceRequestAction.requestServer(ShopService.buy(dto.id,count),"buy",
			(e)=>{
				GameLogicHelper.HandlerItemTipDto(e as ItemTipDto, true);
			});
		}
	}

	protected override void SetPrice(int count)
	{
		_buyCount = count;
		ShopItem dto = _currShopItem.GetData();
		_view.CountLbl.text = MallShoppingModel.Instance.GetBold(count);

		int price = dto.virtualCount * count;
		_view.PriceLbl.text = MallShoppingModel.Instance.GetBold(
			string.Format("[{0}]{1}[-]", PlayerModel.Instance.isEnoughCopper(price)? "78f378" : "f57a7a", price));
	}

	protected override void OnBuyWealthBtn()
	{
		ProxyPayModule.OpenCopper();
	}
}