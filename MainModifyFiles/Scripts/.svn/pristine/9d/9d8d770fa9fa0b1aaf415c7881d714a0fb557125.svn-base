// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SilverMallController.cs
// Author   : willson
// Created  : 2015/1/28 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class SilverMallController : MallController
{
	private ShopWinUI _view;
	private MallShoppingWinUIController _controller;
	
	public SilverMallController(MallShoppingWinUIController controller)
	{
		_controller = controller;
		_view = controller.GetView();
	}

	public void Open()
	{
		_view.BuyWealthButton.gameObject.SetActive(true);
		_view.ItemTabsGrid.gameObject.SetActive(false);

		int shopId = 201;
		Shop shop = DataCache.getDtoByCls<Shop>(shopId);
		if(shop != null)
		{
			_view.TitleSprite.spriteName = shop.res;
			_view.TitleSprite.MakePixelPerfect();
			_view.RefreshTipsLabel.text = shop.description;
			_view.RefreshTipsLabel.gameObject.transform.localPosition = new Vector3(-348,260,0);
		}
		
		_view.PayBtnPos.SetActive(false);
		_view.BuyBtnPos.transform.localPosition = new Vector3(0,_view.BuyBtnPos.transform.localPosition.y,0);
		
		_view.HaveIconSprite.spriteName = ItemIconConst.SilverAltas;
		_view.PriceIconSprite.spriteName = ItemIconConst.SilverAltas;
		OnWealthChanged(PlayerModel.Instance.GetWealth());
		
		_controller.BeginItemCell();
		List<ShopItem> items = DataCache.getArrayByClsWithoutSort<ShopItem>();
		for(int index = 0;index < items.Count;index++)
		{
			if(items[index].shopId == shopId && items[index].grade <= PlayerModel.Instance.GetPlayerLevel())
			{
				if(items[index].item != null)
				{
					_controller.AddItemCell(items[index],MallShoppingModel.Instance.GetCount(items[index]));
				}
				else
				{
					Debug.LogError("商店数据出错: id = " + items[index].id + " ,itemID = " + items[index].itemId + " 无数据");
				}
			}
		}
		_controller.EndAddItemCell();
		_controller.SelectFirstItem();

		_view.ItemScrollView.ResetPosition();
	}
	
	public void OnWealthChanged(WealthNotify notify)
	{
		_view.HaveLbl.text = MallShoppingModel.Instance.GetBold(notify.silver);
	}

	public bool CanBuy(ShopItem dto,int count,bool isTips)
	{
		int cost = dto.virtualCount * count;
		return PlayerModel.Instance.isEnoughSilver (cost, isTips);
	}
}