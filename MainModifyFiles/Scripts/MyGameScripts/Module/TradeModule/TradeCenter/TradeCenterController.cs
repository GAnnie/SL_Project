// **********************************************************************
// Copyright (c) 2015 CILU. All rights reserved.
// File     : TradeCenterController.cs
// Author   : 
// Created  : 2015/05/07
// Purpose  : 
// Modify	: _Alot
// **********************************************************************

using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.whole.modules.trade.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.whole.modules.trade.dto;
using com.nucleus.h1.logic.core.modules.trade.dto;
using com.nucleus.player.data;

public class TradeCenterController : MonoBehaviour, IViewController,IDtoListenerExcute {
	private const string ItemsContainerViewName = "Prefabs/Module/BackpackModule/ItemsContainerView";
	private const string TradeCenterTypeCellPrefabPath = "Prefabs/Module/TradeModule/TradeCenter/TradeCenterTypeCellPrefab";
	private const string TradeCenterItemCellPrefabPath = "Prefabs/Module/TradeModule/TradeCenter/TradeCenterItemCellPrefab";

	private GameObject tradeCenterTypeCellPrefabObj = null;
	private GameObject tradeCenterItemCellPrefabObj = null;

	private TradeCenterView _view;

	private ItemsContainerViewController _backpack;
	
	private List<TradeMenu> _tradeMenuList = null;

	//	menu折叠
	private bool _unToggledMain = false;
	
	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<TradeCenterView>();
		_view.Setup (this.transform);

		if (tradeCenterTypeCellPrefabObj == null) {
			tradeCenterTypeCellPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(TradeCenterTypeCellPrefabPath);
		}
		if (tradeCenterItemCellPrefabObj == null) {
			tradeCenterItemCellPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(TradeCenterItemCellPrefabPath);
		}
		
		if (_tradeMenuList == null) {
			_tradeMenuList = TradeDataModel.Instance.GetTradeMenu();
		}

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemsContainerViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.BackpackPos_Transform.gameObject, prefab);

		_backpack = module.GetMissingComponent<ItemsContainerViewController>();
		_backpack.InitView();
		_backpack.SetData(H1Item.PackEnum_Backpack,BackpackModel.Instance.GetDto().capability);
	}

	private MultipleNotifyListener _multiListener;

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;

		EventDelegate.Set (_view.BuyButton_UIButton.onClick, OnClickBuyButton);

		_multiListener = new MultipleNotifyListener ();
		_multiListener.AddNotify (typeof(TradeCenterDto));
		_multiListener.AddNotify (typeof(TradeGoodsDto));
		_multiListener.Start (this);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
		_backpack.Dispose ();

		_multiListener.Stop ();

		//	退出交易中心
		TradeDataModel.Instance.ExitTradeCenter();
	}
	#endregion

	public void ExcuteDto(object dto) {
		if (dto is TradeCenterDto) {
			TradeCenterDto tradeCenterDto = dto as TradeCenterDto;
			_goodsList = tradeCenterDto.items;

			for (int i = 0, len = _goodsList.Count; i < len; i++) {
				TradeGoodsDto gooldsDto = _goodsList[i];
				if (_goodsMap.ContainsKey(gooldsDto.id)) {
					_goodsMap[gooldsDto.id] = gooldsDto;
				}
				else {
					_goodsMap.Add(gooldsDto.id, gooldsDto);
				}
			}

			ShowItemListByType(_currentShowMenuId);
		} else if (dto is TradeGoodsDto) {
			TradeGoodsDto newGoods = dto as TradeGoodsDto;
			TradeGoodsDto oldGoods = GetTradeGoodsDto(newGoods.id);
			if (oldGoods != null) {
				oldGoods.price = newGoods.price;
				oldGoods.amount = newGoods.amount;

				Debug.Log("price="+newGoods.price + "originPrice="+newGoods.originalPrice);

				if (_lastSelectItemCell != null) {
					if (_lastSelectItemCell.GetGoods().id == newGoods.id) {
						OnItemCellSelect(_lastSelectItemCell);
					}
				}
			}
		}
	}

	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();

			//	进入交易中心
			TradeDataModel.Instance.EnterTradeCenter((GeneralResponse obj) => {
				OnEnterSuccess(obj);
			});
		}

		OnWealthChanged(PlayerModel.Instance.GetWealth());
	}

	private Dictionary<int, TradeGoodsDto> _goodsMap; //数据关键字
	private List<TradeGoodsDto> _goodsList;

	private void OnEnterSuccess(GeneralResponse response) {
		_goodsMap = new Dictionary<int, TradeGoodsDto> ();
		_goodsList = (response as TradeCenterDto).items;
		//	按id排序（升序）
		_goodsList.Sort(delegate(TradeGoodsDto x, TradeGoodsDto y) {
			return x.id.CompareTo(y.id);
		});

		for (int i = 0, len = _goodsList.Count; i < len; i++) {
			TradeGoodsDto gooldsDto = _goodsList[i];
			_goodsMap.Add(gooldsDto.id, gooldsDto);
		}

		InitTypeList ();
	}

	private TradeGoodsDto GetTradeGoodsDto(int id) {
		TradeGoodsDto oldGoods = null;
		if (_goodsMap == null) {
			GameDebuger.OrangeDebugLog("_goodsMap is NULL");
			return null;
		}
		_goodsMap.TryGetValue(id, out oldGoods);
		return oldGoods;
	}

	private void OnWealthChanged(WealthNotify notify) {
		_view.MoneyLbl_UILabel.text = string.Format ("{0}", notify.silver);
	}

	#region 一级菜单
	private void InitTypeList() {
		TradeCenterTypeCellPrefab typeCellPrefab = null;

		List<TradeMenu> list = new List<TradeMenu>();

		for (int i = 0, len = _tradeMenuList.Count; i < len; i++) {
			TradeMenu menu = _tradeMenuList[i];
			if (menu.parentId == 0 && menu.type == TradeMenu.TradeMenuEnum_Trade && TradeDataModel.Instance.IsOpenMenu(menu)) {
				list.Add(menu);
			}
		}
		list.Sort(delegate(TradeMenu x, TradeMenu y) {
			return -x.id.CompareTo(y.id);
		});

		for (int i = 0, len = list.Count; i < len; i++) {
			TradeMenu menu = list[i];
			GameObject go = NGUITools.AddChild(_view.TypeListGrid_UIGrid.gameObject, tradeCenterTypeCellPrefabObj);
			go.name = string.Format("{0}", menu.id);

			TradeCenterTypeCellPrefab typeCell = go.AddMissingComponent<TradeCenterTypeCellPrefab>();
			typeCell.SetData(menu, OnTypeCellSelect);

			//	默认选中第一个
			if (typeCellPrefab == null) {
				typeCellPrefab = typeCell;
			}
		}

		_view.TypeListGrid_UIGrid.Reposition();

		if (typeCellPrefab != null) {
			OnTypeCellSelect (typeCellPrefab);
		}
	}
	#endregion

	#region 点击菜单回调
	private TradeCenterTypeCellPrefab _lastParentTypeCellPrefab;
	private TradeCenterTypeCellPrefab _lastTypeCellPrefab;
	private void OnTypeCellSelect(TradeCenterTypeCellPrefab cell) {
		if (_lastParentTypeCellPrefab == cell) {
			_unToggledMain = !_unToggledMain;
			
			GameDebuger.OrangeDebugLog(_unToggledMain? "一级菜单 -> 折叠" : "一级菜单 -> 展开");
			
			if (_unToggledMain) {
				HideSubMenuItem();
				return;
			}
		} else {
			_unToggledMain = false;
		}

		TradeMenu menu = cell.GetMenu ();
		if (menu.parentId > 0) {
			if (_lastTypeCellPrefab != null) {
				_lastTypeCellPrefab.Select(false);
			}
			
			_lastTypeCellPrefab = cell;
			_lastTypeCellPrefab.Select (true);

			ShowItemListByType (menu.id);
		} else {
			if (_lastParentTypeCellPrefab != null) {
				_lastParentTypeCellPrefab.Select(false);
			}
			
			_lastParentTypeCellPrefab = cell;
			_lastParentTypeCellPrefab.Select (true);

			ShowSubTypeCell(menu.id);
		}
	}
	#endregion
	
	#region 二级菜单
	private List<TradeCenterTypeCellPrefab> _subTypeCellControllerList = new List<TradeCenterTypeCellPrefab> ();
	private void ShowSubTypeCell(int menuId) {

		TradeCenterTypeCellPrefab typeCellPrefab = null;
		bool tSavaTypeCell = false;

		//	是否存在当前menuid的商品 begin
		List<TradeMenu> tTradeMenuList = TradeDataModel.Instance.GetSubTradeMenu(menuId);
		//	End
		
		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(i < tTradeMenuList.Count);
			if (i < tTradeMenuList.Count) {
				TradeMenu tTradeMenu = tTradeMenuList[i];
				
				TradeCenterTypeCellPrefab tTypeCellController = _subTypeCellControllerList[i];
				tTypeCellController.name = string.Format("{0}_{1}", tTradeMenu.parentId, tTradeMenu.id);
				tTypeCellController.SetData(tTradeMenu, OnTypeCellSelect);
				
				//	默认选中第一个
				if (typeCellPrefab == null) {
					typeCellPrefab = tTypeCellController;
				}
			}
		}
		
		for (int i = _subTypeCellControllerList.Count, len = tTradeMenuList.Count; i < len; i++) {
			TradeMenu tTradeMenu = tTradeMenuList[i];
			
			GameObject go = NGUITools.AddChild(_view.TypeListGrid_UIGrid.gameObject, tradeCenterTypeCellPrefabObj);
			go.name = string.Format("{0}_{1}", tTradeMenu.parentId, tTradeMenu.id);
			
			TradeCenterTypeCellPrefab tTypeCellController = go.AddMissingComponent<TradeCenterTypeCellPrefab>();
			tTypeCellController.SetData(tTradeMenu, OnTypeCellSelect);
			
			_subTypeCellControllerList.Add(tTypeCellController);

			if (typeCellPrefab == null) {
				typeCellPrefab = tTypeCellController;
			}
		}

		_view.TypeListGrid_UIGrid.Reposition();
		_view.TypeScrollView_UIScrollView.ResetPosition();

		if (typeCellPrefab != null) {
			OnTypeCellSelect(typeCellPrefab);
		}

		if (tTradeMenuList.Count == 0) {
			ShowItemListByType (menuId);
		}
	}
	#endregion
	
	#region 隐藏二级菜单
	private void HideSubMenuItem() {
		//	_unToggledMain
		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(false);
		}
		_view.TypeListGrid_UIGrid.Reposition();
		_view.TypeScrollView_UIScrollView.ResetPosition();
	}
	#endregion

	#region 所有商品
	private int _currentShowMenuId = 0;
	private List<GameObject> itemListOpenedList = new List<GameObject>();
	private void ShowItemListByType(int menuId) {
		if (_goodsList == null) {
			return;
		}

		//	是否存在当前menuid的商品
		List<TradeGoodsDto> tGoodList = new List<TradeGoodsDto>();
		for(int i = 0,  len = _goodsList.Count; i <  len; i++) {
			TradeGoodsDto tTradeGoodsDto = _goodsList[i];
			TradeGoods tGoods = DataCache.getDtoByCls<TradeGoods>(tTradeGoodsDto.id);
			if (tGoods.tradeMenuId == menuId) {
				//GameDebuger.OrangeDebugLog(DataCache.getDtoByCls<GeneralItem>(tTradeGoodsDto.id).name);

				tGoodList.Add(tTradeGoodsDto);
			}
		}
		if(tGoodList.Count <= 0) {
			return;
		}

		_currentShowMenuId = menuId;

		TradeCenterItemCellPrefab tTradeCenterItemCellPrefab = null;
		bool tSavaTypeCell = false;

		int tItemListOpenedListCount = itemListOpenedList.Count;
		int tGoodListCount = tGoodList.Count;

		for (int i = 0; i < tItemListOpenedListCount; i++) { 
 			itemListOpenedList[i].SetActive(i < tGoodListCount);
			if (i < tGoodListCount) {
				TradeGoodsDto tTradeGoodsDto = tGoodList[i];

				TradeCenterItemCellPrefab controller = itemListOpenedList[i].GetComponent<TradeCenterItemCellPrefab>();
				controller.SetData(tTradeGoodsDto, OnItemCellSelect);

				//	默认选中第一个
				if (!tSavaTypeCell) {
					tSavaTypeCell = true;
					tTradeCenterItemCellPrefab = controller;
				}
			}
		}
		
		for (int i=tItemListOpenedListCount; i<tGoodListCount; i++) {
			TradeGoodsDto tTradeGoodsDto = tGoodList[i];

			GameObject go = NGUITools.AddChild(_view.ItemListGrid_UIGrid.gameObject, tradeCenterItemCellPrefabObj);
			TradeCenterItemCellPrefab controller = go.AddMissingComponent<TradeCenterItemCellPrefab>();
			controller.SetData(tTradeGoodsDto, OnItemCellSelect);
			
			itemListOpenedList.Add(go);
			
			//	默认选中第一个
			if (!tSavaTypeCell) {
				tSavaTypeCell = true;
				tTradeCenterItemCellPrefab = controller;
			}
		}
		
		_view.ItemListGrid_UIGrid.Reposition();
		_view.ScrollView_UIScrollView.ResetPosition();
		
		if (tTradeCenterItemCellPrefab != null) {
			OnItemCellSelect(tTradeCenterItemCellPrefab);
		}
	}
	#endregion

	#region 点击商品回调
	private TradeCenterItemCellPrefab _lastSelectItemCell;
	private void OnItemCellSelect(TradeCenterItemCellPrefab cell) {
		if (_lastSelectItemCell != null) {
			_lastSelectItemCell.Select(false);
		}

		_lastSelectItemCell = cell;
		_lastSelectItemCell.Select (true);

		int tPrice = Mathf.FloorToInt (_lastSelectItemCell.GetGoods().price);
		bool tIisEnoughSilver = PlayerModel.Instance.isEnoughSilver(tPrice);

		_view.PriceLbl_UILabel.color = Color.white;
		_view.PriceLbl_UILabel.text = string.Format ("[{0}]{1}[-]", tIisEnoughSilver? "2beb54" : "ee5d5d", tPrice);
		
		//	商品选项id保存
		//TradeDataModel.Instance.menuHierarchy.mainMenu = _lastSelectItemCell.GetGoods().id;
	}

	private void OnClickBuyButton() {
		if (_lastSelectItemCell.GetGoods().amount > 0)
		{
			if(PlayerModel.Instance.isEnoughSilver(Mathf.FloorToInt (_lastSelectItemCell.GetGoods().price), true))
			{
				TradeDataModel.Instance.BuyInTradeCenter(_lastSelectItemCell.GetGoods().id);
			}
		}
		else
		{
			GeneralItem item = DataCache.getDtoByCls<GeneralItem>(_lastSelectItemCell.GetGoods().id);
			if (item != null)
			{
				TipManager.AddTip(string.Format("{0}暂时缺货", item.name));
			}
		}
	}
	#endregion
}

