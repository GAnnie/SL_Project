// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  IngotMallController.cs
// Author   : willson
// Created  : 2015/1/28 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class IngotMallController : MallController
{
	private ShopWinUI _view;
	private MallShoppingWinUIController _controller;

	private List<TabBtnController> _tabBtnList;

	public IngotMallController(MallShoppingWinUIController controller)
	{
		_controller = controller;
		_view = controller.GetView();
		_tabBtnList = null;
	}

	private void InitTabBtn()
	{
		if(_tabBtnList == null)
		{
			int size = 2;
			_tabBtnList = new List<TabBtnController> (size);
			GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
			for (int i = 0; i < size; ++i) 
			{
				GameObject item = NGUITools.AddChild(_view.ItemTabsGrid.gameObject, tabBtnPrefab);
				TabBtnController com = item.GetMissingComponent<TabBtnController> ();
				_tabBtnList.Add(com);
				_tabBtnList[i].InitItem (i,OnSelectTabBtn);
			}
			
			_tabBtnList[0].SetBtnName("全  部");
			_tabBtnList[1].SetBtnName("每周限购");
		}
	}

	public void OnSelectTabBtn (int index)
	{
		if (index == 0)
			OnSelect301MallView();
		else if (index == 1)
			OnSelect302MallView();
	}

	private void UpdateTabBtnState(int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i)
		{
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}

	public void Open()
	{
		_view.BuyWealthButton.gameObject.SetActive(false);
		_view.ItemTabsGrid.gameObject.SetActive(true);
		InitTabBtn();

		OnSelectTabBtn(0);
	}

	public void OnSelect301MallView()
	{
		OpenShop(301);
		UpdateTabBtnState(0);
	}

	public void OnSelect302MallView()
	{
		OpenShop(302);
		UpdateTabBtnState(1);
	}

	private void OpenShop(int shopId)
	{
		Shop shop = DataCache.getDtoByCls<Shop>(shopId);
		if(shop != null)
		{
			_view.TitleSprite.spriteName = shop.res;
			_view.TitleSprite.MakePixelPerfect();
			_view.RefreshTipsLabel.text = shop.description;
			_view.RefreshTipsLabel.gameObject.transform.localPosition = new Vector3(-68,260,0);
		}
		
		_view.PayBtnPos.SetActive(true);
		_view.BuyBtnPos.transform.localPosition = new Vector3(89,_view.BuyBtnPos.transform.localPosition.y,0);

		
		_view.HaveIconSprite.spriteName = ItemIconConst.IngotAltas;
		_view.PriceIconSprite.spriteName = ItemIconConst.IngotAltas;
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
	}

	public void OnWealthChanged(WealthNotify notify)
	{
		_view.HaveLbl.text = MallShoppingModel.Instance.GetBold(notify.ingot);
	}

	public bool CanBuy(ShopItem dto,int count,bool isTips)
	{
		int cost = dto.virtualCount * count;
		return PlayerModel.Instance.isEnoughIngot (cost, isTips);
	}
}