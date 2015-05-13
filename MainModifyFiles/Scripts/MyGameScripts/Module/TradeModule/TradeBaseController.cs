// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	TradeBaseController.cs
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

public class TradeBaseController : MonoBehaviour, IViewController
{
	private const string centerViewPath = "Prefabs/Module/TradeModule/TradeCenter/TradeCenterView";
	private const string marketViewPath = "Prefabs/Module/TradeModule/Market/MarketView";
	private const string auctionViewPath = "Prefabs/Module/TradeModule/Auction/AuctionView";
	private string _currentViewPath = "";

	private TradeBaseView _view;
	
	private List<TabBtnController> _rightTabBtnList = new List<TabBtnController>();
	private TabBtnController _lastRightTabBtn = null;
	
	private string[] strsTabName = {"商会", "摆摊", "拍卖"};
	private string[] strsViewPath = {centerViewPath, marketViewPath, auctionViewPath};

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<TradeBaseView>();
		_view.Setup (this.transform);
	}

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		EventDelegate.Set (_view.CloseButton.onClick, OnClickCloseButton);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		//	关闭各个窗口
		for (int i = 0, len = strsViewPath.Length; i < len; i++) {
			UIModuleManager.Instance.CloseModule (strsViewPath[i]);
		}
	}
	#endregion

	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();
			
			InitTabBtn();
		}
		OnSelectRightTabBtn(TradeDataModel.Instance.defaultTradeBaseTabNum);
	}
	
	private void InitTabBtn(){
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i = 0, len = strsTabName.Length; i < len; i++) {
			GameObject item = NGUITools.AddChild (_view.TabButtonGrid_UIGrid.gameObject, tabBtnPrefab);
			
			TabBtnController tTabBtnController = item.GetMissingComponent<TabBtnController>();
			_rightTabBtnList.Add (tTabBtnController);

			tTabBtnController.InitItem (i, OnSelectRightTabBtn);
			tTabBtnController.SetBtnName(strsTabName[i]);
		}
	}

	private void OnSelectRightTabBtn(int index){
		//	判断是否是相同的View T:return F:go on
		if (_lastRightTabBtn == _rightTabBtnList[index]) return;

		if(_lastRightTabBtn != null)
			_lastRightTabBtn.SetSelected(false);
		
		_lastRightTabBtn = _rightTabBtnList[index];
		_lastRightTabBtn.SetSelected(true);
			
		//	影藏上一个View 打开下一个View
		if (_currentViewPath != "") {
			UIModuleManager.Instance.HideModule(_currentViewPath);
		}
		_currentViewPath = strsViewPath[index];
		GameObject ui = UIModuleManager.Instance.OpenFunModule(_currentViewPath, UILayerType.SubModule, false);

		string tNameSpriteStr = "";

		switch (index) {
		case 0:
			//	交易中心Tab按下
			TradeCenterController tTradeCenterController = ui.GetMissingComponent<TradeCenterController>();
			tTradeCenterController.Open ();
			tNameSpriteStr = "commerceTitle";

			break;
		case 1:
			//	摆摊Tab按下
			MarketController tMarketController = ui.GetMissingComponent<MarketController>();
			tMarketController.Open ();
			tNameSpriteStr = "marketTitle";

			break;
		case 2:
			//	拍卖行Tab按下
			AuctionController tAuctionController = ui.GetMissingComponent<AuctionController>();
			tAuctionController.Open ();
			tNameSpriteStr = "auctionTitle";

			break;
		default:
			break;
		}

		_view.NameSprite.spriteName = tNameSpriteStr;
		_view.NameSprite.MakePixelPerfect ();
	}

	private void OnClickCloseButton() {
		ProxyTradeModule.Close ();
	}
}

