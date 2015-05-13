// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ShopWinUIController.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ShopWinUIController : MonoBehaviourBase,IViewController
{
	private const string ShopItemCellNAME = "Prefabs/Module/ShopModule/ShopItemCell";

	protected ShopWinUI _view;
	protected List<ShopItemCellController> _itemList;

	protected ShopItemCellController _currShopItem;

	private float _mDragStartTime = 0f;
	private float _pressAndHoldDelay = 0.5f;

	protected int _buyCount;

	public ShopWinUI GetView()
	{
		return _view;
	}

	public virtual void InitView()
	{
		_view = gameObject.GetMissingComponent<ShopWinUI> ();
		_view.Setup(this.transform);

		_itemList = new List<ShopItemCellController>();
		RegisterEvent();
	}

	public virtual void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);

		//EventDelegate.Set(_view.AddCountButton.onClick,OnAddCountBtn);
		//EventDelegate.Set(_view.SubCountButton.onClick,OnSubCountBtn);

		//_view.AddBtnEventListener.onClick += OnAddBtn;
		//_view.SubBtnEventListener.onClick += OnSubBtn;

		_view.AddBtnEventListener.onPress += OnPressAddBtn;
		_view.SubBtnEventListener.onPress += OnPressSubBtn;

		EventDelegate.Set(_view.countBtn.onClick,OnCountBtn);

		UIHelper.CreateBaseBtn(_view.PayBtnPos,"充值",OnPayBtn);
		UIHelper.CreateBaseBtn(_view.BuyBtnPos,"购买",OnBuyBtn);

		EventDelegate.Set(_view.BuyWealthButton.onClick,OnBuyWealthBtn);
	}

	private int _itemIndex;
	public void BeginItemCell()
	{
		_itemIndex = 0;
	}

	public void AddItemCell(ShopItem item,int remain = -1)
	{
		if(_itemIndex < _itemList.Count)
		{
			_itemList[_itemIndex].SetActive(true);
			_itemList[_itemIndex].SetData(item,SelectItem,remain);
		}
		else
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ShopItemCellNAME ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.ItemsGrid.gameObject,prefab);
			ShopItemCellController cell = module.GetMissingComponent<ShopItemCellController>();
			//UIHelper.AdjustDepth(module,3);
			cell.InitView();
			cell.SetData(item,SelectItem,remain);

			_itemList.Add(cell);
		}
		_itemIndex++;
		_view.ItemsGrid.Reposition();
	}

	public void EndAddItemCell()
	{
		while(_itemIndex < _itemList.Count)
		{
			_itemList[_itemIndex].SetActive(false);
			_itemIndex++;
		}
	}

	protected void SelectItem(ShopItemCellController cell)
	{
		if(_currShopItem != null && _currShopItem.GetData() != null 
		   && cell != null && cell.GetData() != null
		   && _currShopItem.GetData().id == cell.GetData().id)
		{
			OnAddCountBtn();
		}
		else
		{
			_currShopItem = cell;
			for(int index = 0;index < _itemList.Count;index++)
			{
				_itemList[index].SetSelect(_itemList[index] == cell);
			}
			SetPrice(1);
		}

		ShopItemTips();
	}

	private void ShopItemTips()
	{
		ShopItem dto = _currShopItem.GetData();
		if(string.IsNullOrEmpty(dto.extraInfo))
			ItemTextTipManager.Instance.ShowItem(ItemHelper.H1ItemToPackItemDto(dto.item,dto.itemCount),_view.ItemTIpsLabel);
		else
			ItemTextTipManager.Instance.ShowShopEquip(ItemHelper.H1ItemToPackItemDto(dto.item,dto.itemCount),_view.ItemTIpsLabel,dto.extraInfo);
	}

	protected virtual void SetPrice(int count)
	{
		_buyCount = count;
		ShopItem dto = _currShopItem.GetData();
		_view.CountLbl.text = MallShoppingModel.Instance.GetBold(count);
		_view.PriceLbl.text = MallShoppingModel.Instance.GetBold(dto.virtualCount * count);
	}

	private bool _pressSubBtn;
	private void OnPressSubBtn(GameObject go, bool state){
		_pressSubBtn = state;
		if(state)
		{
			OnSubCountBtn();
			_mDragStartTime = RealTime.time + _pressAndHoldDelay;
		}
	}
	
	private bool _pressAddBtn;
	private void OnPressAddBtn(GameObject go, bool state)
	{
		_pressAddBtn = state;
		if(state)
		{
			OnAddCountBtn();
			_mDragStartTime = RealTime.time + _pressAndHoldDelay;
		}
	}

	void Update(){
		if(!_pressSubBtn && !_pressAddBtn)
			return;
		if(_mDragStartTime > RealTime.time)
			return;
		
		if(_pressSubBtn){
			OnSubCountBtn();
		}else if(_pressAddBtn){
			OnAddCountBtn();
		}
	}

	
	private void OnAddBtn(GameObject go){
		OnAddCountBtn();
	}
	
	private void OnSubBtn(GameObject go){
		OnSubCountBtn();
	}

	private void OnAddCountBtn()
	{
		int count = _buyCount;
		count += 1;
		if(_currShopItem.IsMax(count))
		{
			count = _currShopItem.GetRemain();
			TipManager.AddTip("没有更多了");
			_pressAddBtn = false;
		}
		else if(count >= 200)
		{
			_pressAddBtn = false;
			TipManager.AddTip("单次购买数量已达上限");
		}

		SetPrice(count >= 200?200:count);
	}

	private void OnSubCountBtn()
	{
		int count = _buyCount;
		count -= 1;
		if(count <= 1)
		{
			_pressSubBtn = false;
		}

		SetPrice(count <= 1?1:count);
	}

	private void OnCountBtn()
	{
		ProxyNumericInputModule.Open(_view.countBtn.gameObject,UIAnchor.Side.Left,new Vector2(-200f,-100f),onClickNumerCallBack);
	}

	private void onClickNumerCallBack(int num)
	{
		if(num >= 0)
		{
			int count = _buyCount;
			count = count*10+ num;

			if(_currShopItem.IsMax(count))
			{
				count = _currShopItem.GetRemain();
				TipManager.AddTip("没有更多了");
			}
			else if(count >= 200)
			{
				TipManager.AddTip("单次购买数量已达上限");
			}

			SetPrice(count >= 200?200:count);
		}
		else
		{
			int count = _buyCount;
			count = count/10;
			SetPrice(count <= 0?0:count);
		}
	}

	protected virtual void OnWealthChanged(WealthNotify notify)
	{
	}

	protected virtual void OnBuyBtn()
	{
	}

	protected virtual void OnBuyWealthBtn()
	{
	}

	protected void OnPayBtn()
	{
		ProxyPayModule.OpenPay();
	}

	public void OnClose()
	{
		ProxyShopModule.Close();
	}

	public void Dispose()
	{
		_view.AddBtnEventListener.onPress -= OnPressAddBtn;
		_view.SubBtnEventListener.onPress -= OnPressSubBtn;
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
	}
}