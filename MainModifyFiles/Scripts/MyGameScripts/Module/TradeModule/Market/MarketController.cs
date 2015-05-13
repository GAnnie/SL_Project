// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MarketController.cs
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
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.trade.dto;
using com.nucleus.h1.logic.whole.modules.trade.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;

public class MarketController : MonoBehaviour,IViewController {
	private MarketView _view;

	//	保持引用
	private MarketBuyController marketBuyController = null;
	private MarketSellController marketSellController = null;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<MarketView>();
		_view.Setup (this.transform);

		marketBuyController = _view.BuyGroup_Transform.gameObject.GetMissingComponent<MarketBuyController>();
		marketBuyController.ProvidedExternallyView(_view);

		marketSellController = _view.SellGroup_Transform.gameObject.GetMissingComponent<MarketSellController>();
		marketSellController.ProvidedExternallyView(_view);
	}

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
	}

	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;

		//	停止market计数器
		if (CoolDownManager.Instance.IsCoolDownExist(marketBuyController.tradeMarketCoolDownName)) {
			CoolDownManager.Instance.CancelCoolDown(marketBuyController.tradeMarketCoolDownName);
		}
	}
	#endregion
	
	private void OnWealthChanged(WealthNotify notify) {
		_view.MoneyLbl_UILabel.text = string.Format("{0}", notify.copper);
	}

	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();
		}

		OnWealthChanged(PlayerModel.Instance.GetWealth());

		if (TradeDataModel.Instance.marketTabNum == 0) {
			//	获取其他玩家摊位数据(必须先去拿自己摊位的数据（服务端要求）)
			TradeDataModel.Instance.EnterMarket(delegate() {
				int tMenuID = TradeDataModel.Instance.menuHierarchy.settinged?
					TradeDataModel.Instance.menuHierarchy.mainMenu : TradeDataModel.Instance.defaultTradeMenuNum;
				if (!TradeDataModel.Instance.GetPlayerStallCenterDtoByMenuidDic().ContainsKey(tMenuID)) {
					TradeDataModel.Instance.MenuMarket(tMenuID, OnFinishCallback);
				} else {
					OnFinishCallback();
				}
			}, SellToMarkCallback);
		} else if (TradeDataModel.Instance.marketTabNum == 1) {
			//	获取我的摊位数据
			TradeDataModel.Instance.EnterMarket(OnFinishCallback, SellToMarkCallback);
		}
	}

	private void OnFinishCallback() {
		InitTabBtn ();
		OnSelectTopTabBtn(TradeDataModel.Instance.marketTabNum);
	}

	private void SellToMarkCallback() {
		if (marketSellController != null) {
			marketSellController.RefreshMarketSellTabView();
		}
	}

	#region Init TabBtn
	private string[] _tStrs = {"购买", "出售"};
	private List<TabBtnController> _topTabBtnList;
	private TabBtnController _lastTopTabBtn;
	
	private void InitTabBtn(){
		if (_topTabBtnList == null) {
			_topTabBtnList = new List<TabBtnController>(_tStrs.Length);
		} else {
			return;
		}

		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i = 0, len = _tStrs.Length; i < len; i++) {
			GameObject item = NGUITools.AddChild (_view.TabGrid_UIGrid.gameObject, tabBtnPrefab);
			
			TabBtnController tTabBtnController= item.GetMissingComponent<TabBtnController> ();
			_topTabBtnList.Add (tTabBtnController);

			tTabBtnController.InitItem (i, OnSelectTopTabBtn);
			tTabBtnController.SetBtnLblSpac (20);
			tTabBtnController.SetBtnName(_tStrs[i]);
		}
		
		_view.TabGrid_UIGrid.Reposition();
	}
	#endregion

	private void OnSelectTopTabBtn(int index){
		//	判断如果是当前，则不刷新
		if (_lastTopTabBtn == _topTabBtnList[index]) return;

		if(_lastTopTabBtn != null) {
			_lastTopTabBtn.SetSelected(false);
		}
		
		_lastTopTabBtn = _topTabBtnList[index];
		_lastTopTabBtn.SetSelected(true);

		_view.BuyGroup_Transform.gameObject.SetActive(index == 0);
		_view.SellGroup_Transform.gameObject.SetActive(index == 1);

		switch (index) {
		case 0:
			marketBuyController.OpenMarketBuyTab();
			break;
		case 1:
			marketSellController.OpenMarketSellTab();
			break;
		default:
			break;
		}

		TradeDataModel.Instance.marketTabNum = index;
	}
}

