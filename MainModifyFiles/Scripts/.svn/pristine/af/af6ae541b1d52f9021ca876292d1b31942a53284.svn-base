// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MallShoppingWinUIController.cs
// Author   : willson
// Created  : 2015/1/28 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.shop.data;

public class MallShoppingWinUIController : ShopWinUIController
{
	private List<TabBtnController> _tabBtnList;

	private IngotMallController _ingotMallController;
	private SilverMallController _silverMallController;
	private ScoreMallController _scoreMallController;
	private MallController _currMall;

	public override void InitView ()
	{
		base.InitView ();
		// 初始化tabs
		InitTabBtn();

		_ingotMallController = new IngotMallController(this);
		_silverMallController = new SilverMallController(this);
		_scoreMallController = new ScoreMallController(this);
	}

	private void InitTabBtn()
	{
		int size = 3;
		_tabBtnList = new List<TabBtnController> (size);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i = 0; i < size; ++i) 
		{
			GameObject item = NGUITools.AddChild(_view.TabsGrid.gameObject, tabBtnPrefab);
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_tabBtnList.Add(com);
			_tabBtnList[i].InitItem (i,OnSelectTabBtn);
		}
		
		_tabBtnList[0].SetBtnName("金币商城");
		_tabBtnList[1].SetBtnName("银币商城");
		_tabBtnList[2].SetBtnName("积分商城");
	}

	public void OnSelectTabBtn (int index)
	{
		if (index == 0)
			OnSelectIngotMallView();
		else if (index == 1)
			OnSelectSilverMallView();
		else if (index == 2)
			OnSelectScoreMallView();
	}

	public void Open()
	{
		OnSelectTabBtn(0);
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

	public void OnSelectIngotMallView()
	{
		UpdateTabBtnState(0);
		_currMall = _ingotMallController;
		_ingotMallController.Open();
	}

	public void OnSelectSilverMallView()
	{
		UpdateTabBtnState(1);
		_currMall = _silverMallController;
		_silverMallController.Open();
	}

	public void OnSelectScoreMallView()
	{
		UpdateTabBtnState(2);
		_currMall = _scoreMallController;
		_scoreMallController.Open();
	}

	public void SelectFirstItem()
	{
		_currShopItem = null;
		if(_itemList.Count > 0)
		{
			SelectItem(_itemList[0]);
		}
	}

	protected override void SetPrice(int count)
	{
		_buyCount = count;
		ShopItem dto = _currShopItem.GetData();
		_view.CountLbl.text = MallShoppingModel.Instance.GetBold(count);
		_view.PriceLbl.text = MallShoppingModel.Instance.GetBold(
			string.Format("[{0}]{1}[-]", _currMall.CanBuy(dto,count, false)? "78f378" : "f57a7a", dto.virtualCount * count));
	}

	protected override void OnWealthChanged(WealthNotify notify)
	{
		_currMall.OnWealthChanged(notify);
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

		if(_currMall.CanBuy(dto,count,true))
		{
			ServiceRequestAction.requestServer(ShopService.buy(dto.id,count),"buy",
			(e)=>{
				GameLogicHelper.HandlerItemTipDto(e as ItemTipDto, true);
				_currShopItem.SetRestrictCount(MallShoppingModel.Instance.BuyItem(dto,count));
			});
		}
	}

	protected override void OnBuyWealthBtn()
	{
		ProxyPayModule.OpenSilver();
	}
}