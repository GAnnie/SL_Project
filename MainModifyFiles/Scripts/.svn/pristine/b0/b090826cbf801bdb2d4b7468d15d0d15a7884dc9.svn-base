// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MarketBuyController.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.nucleus.commons.message;
using com.nucleus.h1.logic.whole.modules.stall.dto;
using com.nucleus.h1.logic.whole.modules.trade.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.whole.modules.stall.data;
using com.nucleus.h1.logic.core.modules;

public class MarketBuyController : MonoBehaviour {
	private MarketView _view;

	public const string buyTypeItemPath = "Prefabs/Module/TradeModule/Market/TypeCellPrefabView";
	private GameObject _buyTypeItemPrefabObj = null;
	
	public const string buyGoodsItemPath = "Prefabs/Module/TradeModule/Market/SellItemCell";
	private GameObject buyGoodsItemPrefabObj = null;

	private List<TradeMenu> _tradeMenuList = null;

	//	数据关键字
	private List<StallGoodsDto> _goodsList;
	public string tradeMarketCoolDownName = "TradeMarketRefreshTime_";

	private bool _unToggledMain = false;

	#region 外部提供的view
	public void ProvidedExternallyView(MarketView view) {
		_view = view;
		SetBtnClickCallback();

		if (_buyTypeItemPrefabObj == null) {
			_buyTypeItemPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(buyTypeItemPath);
		}
		if (buyGoodsItemPrefabObj == null) {
			buyGoodsItemPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(buyGoodsItemPath);
		}
	}
	#endregion
	
	/// <summary>
	/// Registers the btn click event.
	/// </summary>
	private void SetBtnClickCallback() {
		EventDelegate.Set (_view.RefreshButton_UIButton.onClick, OnClickRefreshButton);
		EventDelegate.Set (_view.BuyButton_UIButton.onClick, OnClickBuyButton);
		EventDelegate.Set (_view.PrePageButton_UIButton.onClick, OnClickPrePageButton);
		EventDelegate.Set (_view.NextPageButton_UIButton.onClick, OnClickNextPageButton);
	}

	#region 打开市场买入标签
	private bool _firstOpened = false;
	public void OpenMarketBuyTab() {
		//	保持tab只打开一次
		if (!_firstOpened) {
			_firstOpened = true;

			InitBuyTabView();
			StartTimeCoolDown();

			//	切页
			_view.ItemCellGrid_PageScrollView.onChangePage = delegate(int curPage) {
				_currentPag = curPage;
				_view.PageLabel_UILabel.text = string.Format("第{0}/{1}页", curPage, _view.ItemCellGrid_PageScrollView.MaxPage);
			};
		}
	}
	#endregion

	#region 一级菜单
	private void InitBuyTabView() {
		//	Get Data From Server
		if (_tradeMenuList == null) {
			_tradeMenuList = TradeDataModel.Instance.GetTradeMenu();
		}

		List<TradeMenu> tradeMenuList = new List<TradeMenu>();
		for (int i = 0, len = _tradeMenuList.Count; i < len; i++) {
			TradeMenu tTradeMenu = _tradeMenuList[i];
			if (tTradeMenu.parentId == 0 && tTradeMenu.type == TradeMenu.TradeMenuEnum_Stall && TradeDataModel.Instance.IsOpenMenu(tTradeMenu)) {
				tradeMenuList.Add(tTradeMenu);
			}
		}

		TypeCellPrefabController tTypeCellPrefabController = null;

		for (int i = 0, len = tradeMenuList.Count; i < len; i++) {
			TradeMenu tTradeMenu = tradeMenuList[i];

			GameObject go = NGUITools.AddChild(_view.TypeGrid_UIGrid.gameObject, _buyTypeItemPrefabObj);
			go.name = tTradeMenu.id.ToString();

			TypeCellPrefabController tTypeCellController = go.AddMissingComponent<TypeCellPrefabController>();
			tTypeCellController.StartController(tTradeMenu, OnTypeCellSelect);

			//	默认选择一级菜单
			if (TradeDataModel.Instance.menuHierarchy.settinged && tTradeMenu.id == TradeDataModel.Instance.menuHierarchy.mainMenu) {
				tTypeCellPrefabController = tTypeCellController;
			} else if (i == len - 1 && tTypeCellPrefabController == null) {
				tTypeCellPrefabController = tTypeCellController;
   			}
		}
		
		_view.TypeGrid_UIGrid.Reposition();

		if (tTypeCellPrefabController != null) {
			OnTypeCellSelect(tTypeCellPrefabController);
		}
	}
	#endregion

	#region 菜单选中回调
	private TypeCellPrefabController _lastRootTypeCellController;
	private TypeCellPrefabController _lastSecondTypeCellController;
	private int _lastSubMenuID = 0;

	private void OnTypeCellSelect(TypeCellPrefabController typeCellController) {
		//	MainMenu Judge
		if (_lastRootTypeCellController == typeCellController) {
			_unToggledMain = !_unToggledMain;

			GameDebuger.OrangeDebugLog(_unToggledMain? "一级菜单 -> 折叠" : "一级菜单 -> 展开");

			if (_unToggledMain) {
				HideSubMenuItem();
				return;
			}
		} else {
			_unToggledMain = false;
		}

		//	SubMenu Judge
		if (_lastSecondTypeCellController != null && _lastSubMenuID == _lastSecondTypeCellController.GetTradeMenu().id) {
			return;
		}
		
		TradeMenu menu = typeCellController.GetTradeMenu ();
		if (menu.parentId > 0) {
			//	二级菜单id保存
			TradeDataModel.Instance.menuHierarchy.subMenu = menu.id;
			TradeDataModel.Instance.menuHierarchy.settinged = true;

			if (_lastSecondTypeCellController != null) {
				_lastSecondTypeCellController.Selected(false);
			}
			
			_lastSecondTypeCellController = typeCellController;
			_lastSecondTypeCellController.Selected (true);
			
			ShowItemListByType(menu.id);
		} else {
			//	一级菜单id保存
			TradeDataModel.Instance.menuHierarchy.mainMenu = menu.id;

			if (_lastRootTypeCellController != null) {
				_lastRootTypeCellController.Selected(false);
			}
			
			_lastRootTypeCellController = typeCellController;
			_lastRootTypeCellController.Selected (true);

			if (!TradeDataModel.Instance.GetPlayerStallCenterDtoByMenuidDic().ContainsKey(menu.id)) {
				TradeDataModel.Instance.MenuMarket(menu.id, delegate() {
					ShowSubTypeCell(menu.id);
				});
			} else {
				ShowSubTypeCell(menu.id);
			}
		}
	}
	#endregion

	#region 二级菜单
	private List<TypeCellPrefabController> _subTypeCellControllerList = new List<TypeCellPrefabController> ();
	private void ShowSubTypeCell(int menuId) {
		TypeCellPrefabController typeCellPrefab = null;

		if (_tradeMenuList == null) {
			return;
		}
		
		//	是否存在当前menuid的商品 begin
		List<TradeMenu> tTradeMenuList = TradeDataModel.Instance.GetSubTradeMenu(menuId);

		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(i < tTradeMenuList.Count);
			if (i < tTradeMenuList.Count) {
				TradeMenu tTradeMenu = tTradeMenuList[i];

				TypeCellPrefabController tTypeCellController = _subTypeCellControllerList[i];
				tTypeCellController.name = string.Format("{0}_{1}", tTradeMenu.parentId, tTradeMenu.id);
				tTypeCellController.StartController(tTradeMenu, OnTypeCellSelect);
				
				//	默认选择二级菜单
				if (TradeDataModel.Instance.menuHierarchy.settinged && tTradeMenu.id == TradeDataModel.Instance.menuHierarchy.subMenu) {
					typeCellPrefab = tTypeCellController;
				} else if (typeCellPrefab == null) {
					typeCellPrefab = tTypeCellController;
				}
			}
		}

		for (int i = _subTypeCellControllerList.Count, len = tTradeMenuList.Count; i < len; i++) {
			TradeMenu tTradeMenu = tTradeMenuList[i];
			
			GameObject go = NGUITools.AddChild(_view.TypeGrid_UIGrid.gameObject, _buyTypeItemPrefabObj);
			go.name = string.Format("{0}_{1}", tTradeMenu.parentId, tTradeMenu.id);

			TypeCellPrefabController tTypeCellController = go.AddMissingComponent<TypeCellPrefabController>();
			tTypeCellController.StartController(tTradeMenu, OnTypeCellSelect);
			
			_subTypeCellControllerList.Add(tTypeCellController);

			//	默认选择二级菜单
			if (TradeDataModel.Instance.menuHierarchy.settinged && tTradeMenu.id == TradeDataModel.Instance.menuHierarchy.subMenu) {
				typeCellPrefab = tTypeCellController;
			} else if (typeCellPrefab == null) {
				typeCellPrefab = tTypeCellController;
			}
		}
		
		_view.TypeGrid_UIGrid.Reposition();
		_view.ScrollView_UIScrollView.ResetPosition();
		
		if (typeCellPrefab != null) {
			OnTypeCellSelect(typeCellPrefab);
		}

		//	当没有二级菜单的时候
		if (tTradeMenuList.Count == 0) {
			ShowItemListByType (menuId);
			TradeDataModel.Instance.menuHierarchy.subMenu = menuId;
		}
	}
	#endregion

	#region 隐藏二级菜单
	private void HideSubMenuItem() {
		//	_unToggledMain
		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(false);
		}
		_view.TypeGrid_UIGrid.Reposition();
		_view.ScrollView_UIScrollView.ResetPosition();
	}
	#endregion

	#region 具体物品分类
	private int _currentShowMenuId = 0;
	private List<SellItemCellController> buyItemListOpenedList = new List<SellItemCellController>();
	private void ShowItemListByType(int menuId) {
		_goodsList = TradeDataModel.Instance.GetPlayerStallCenterDtoByMenuid(TradeDataModel.Instance.menuHierarchy.mainMenu).items;
		List<StallGoodsDto> tGoodList = new List<StallGoodsDto>();

		if (_goodsList != null) {
			bool fitstSta = false;
			//	是否存在当前menuid的商品 begin
			for(int i = 0, len = _goodsList.Count; i < len; i++) {
				StallGoodsDto tStallGoodsDto = _goodsList[i];
				StallGoods tStallGoods = DataCache.getDtoByCls<StallGoods>(tStallGoodsDto.id);
				if (tStallGoods == null) {
					if (!fitstSta) {
						fitstSta = true;
						GameDebuger.OrangeDebugLog(string.Format("StallGoods Is NULl, Not Exit The Goods By ID -> {0}", tStallGoodsDto.id));
					}
				} else if (tGoodList.Count < TradeDataModel.Instance.maxStallTotalCount && tStallGoods.tradeMenuId == menuId) {
					tGoodList.Add(tStallGoodsDto);
				}
			}
			if(tGoodList.Count > 0) {
				//	按id排序（升序）
				tGoodList.Sort(delegate(StallGoodsDto x, StallGoodsDto y) {
					return x.id.CompareTo(y.id);
				});
			}
		}
		//	end

		SellItemCellController tSellItemCellController = null;

		for (int i = 0, len = buyItemListOpenedList.Count; i < len; i++) {
			buyItemListOpenedList[i].gameObject.SetActive(i < tGoodList.Count);
			if (i < tGoodList.Count) {
				StallGoodsDto tStallGoodsDto = tGoodList[i];

				SellItemCellController controller = buyItemListOpenedList[i];
				controller.SetStallGoodsDto(tStallGoodsDto, SellItemCellController.TypeItemItemCell.marketBuy, OnItemCellSelect);
				//	默认选中第一个
				if (tSellItemCellController == null) {
					tSellItemCellController = controller;
				}
			}
		}

		for (int i = buyItemListOpenedList.Count, len = tGoodList.Count; i < len; i++) {
			StallGoodsDto tStallGoodsDto = tGoodList[i];
			
			GameObject go = NGUITools.AddChild(_view.ItemCellGrid_PageScrollView.gameObject, buyGoodsItemPrefabObj);
			SellItemCellController controller = go.AddMissingComponent<SellItemCellController>();
			controller.OpenSellItemCell();
			controller.SetStallGoodsDto(tStallGoodsDto, SellItemCellController.TypeItemItemCell.marketBuy, OnItemCellSelect);

			buyItemListOpenedList.Add(controller);
			
			//	默认选中第一个
			if (tSellItemCellController == null) {
				tSellItemCellController = controller;
			}
		}
		
		_view.ItemCellGrid_PageScrollView.PageGrid.Reposition();
		
		if (tSellItemCellController != null) {
			OnItemCellSelect(tSellItemCellController);
		}
	}
	#endregion

	#region 具体物品选中回调
	private SellItemCellController _lastSelectItemCell;
	private void OnItemCellSelect(SellItemCellController cellController) {
		//	TODO
		if (_lastSelectItemCell != null) {
			_lastSelectItemCell.Selected(false);
		}

		_lastSelectItemCell = cellController;
		_lastSelectItemCell.Selected(true);
	}
	#endregion

	#region 刷新市场购买标签
	private void StartTimeCoolDown() {
		long tSystemTime = SystemTimeManager.Instance.GetUTCTimeStamp();
		long tRefreshTime = TradeDataModel.Instance.GetPlayerStallGoodsDto().refreshTime;

		float tWaitTime = tRefreshTime - tSystemTime > 0? (tRefreshTime - tSystemTime) / 1000 : 0;

		if (tWaitTime > 0) {
			//	进行倒计时后才能刷新
			//	设置刷新需要的银币
			_view.ReGo_Transform.gameObject.SetActive(true);
			_view.RefLabel_UILabel.transform.localPosition = new Vector3(-36, 0, 0);
			_view.RefLabel_UILabel.spacingX = 0;

			_view.RefLabel_UILabel.text = string.Format("{0}", 100);

			CoolDownManager.Instance.SetupCoolDown(tradeMarketCoolDownName, tWaitTime, RefshreTimeUpdate, RefshreTimeOver);
		} else {
			//	可以直接刷新
			RefshreTimeOver();
		}
	}

	private void RefshreTimeUpdate(float remainTime) {
		System.TimeSpan span = System.TimeSpan.FromSeconds(remainTime);
		
		_view.TimeLabel_UILabel.text = string.Format("时钟：{1:D2}分钟{2:D2}秒", span.Hours, span.Minutes, span.Seconds);
	}

	private void RefshreTimeOver() {
		//	可以刷新
		_view.ReGo_Transform.gameObject.SetActive(false);

		_view.RefLabel_UILabel.transform.localPosition = Vector3.zero;
		_view.RefLabel_UILabel.spacingX = 20;
		_view.RefLabel_UILabel.text = string.Format("刷新");
		_view.TimeLabel_UILabel.text = "";
	}
	#endregion

	#region Buy Handle
	private void OnClickRefreshButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 刷新按钮"));
		TradeDataModel.Instance.RefreshMarket(delegate() {
			//	RODO
			ShowItemListByType(TradeDataModel.Instance.menuHierarchy.subMenu);

			_view.ReGo_Transform.gameObject.SetActive(true);
			_view.RefLabel_UILabel.transform.localPosition = new Vector3(-36, 0, 0);
			_view.RefLabel_UILabel.spacingX = 0;

			_view.RefLabel_UILabel.text = string.Format("{0}", 100);
			float tWaitTime = DataHelper.GetStaticConfigValue(H1StaticConfigs.STALL_ITEM_REFRESH_TIME, 300000) / 1000;
			CoolDownManager.Instance.SetupCoolDown(tradeMarketCoolDownName, tWaitTime, RefshreTimeUpdate, RefshreTimeOver);

			//ShowSubTypeCell(TradeDataModel.Instance.menuHierarchy.subMenu);
		});
	}
	
	private void OnClickBuyButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 购买按钮"));

		if (_lastSelectItemCell == null) {
			TipManager.AddTip(string.Format("请选择物品"));
		} else if (_lastSelectItemCell.GetStallGoodsDto().amount <= 0) {
			TipManager.AddTip(string.Format("该物品已经卖完"));
		} else{
			int tPrice = _lastSelectItemCell.GetStallGoodsDto().price;
			if (PlayerModel.Instance.isEnoughCopper(tPrice, true)) {
				TradeDataModel.Instance.BuyMarket(_lastSelectItemCell.GetStallGoodsDto().stallId, delegate(GeneralResponse obj) {
					GameDebuger.OrangeDebugLog(string.Format("RODO -> 获得物品ID为{0}", _lastSelectItemCell.GetStallGoodsDto().id));
					//	重新设置ItemCell Count并判断是否售罄
					_lastSelectItemCell.SetItemCellCount();
				});
			}
		}
	}

	//	循环翻页效果
	private int _currentPag = 0;
	private void OnClickPrePageButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 上页按钮"));

		if (_currentPag-- <= 1) {
			_currentPag = _view.ItemCellGrid_PageScrollView.MaxPage;
		}
		_view.ItemCellGrid_PageScrollView.SkipToPage(_currentPag, true);
	}
	
	private void OnClickNextPageButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 下页按钮"));
		
		if (_currentPag++ >= _view.ItemCellGrid_PageScrollView.MaxPage) {
			_currentPag = 1;
		}
		_view.ItemCellGrid_PageScrollView.SkipToPage(_currentPag, true);
	}
	#endregion
}
