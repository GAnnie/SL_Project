// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	AuctionController.cs
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

public class AuctionController : MonoBehaviour,IViewController {
	private AuctionView _view;
	
	//	保持引用
	private AuctionBuyController auctionBuyController = null;
	private AuctionSellController auctionSellController = null;
	private AuctionBidController auctionBidController = null;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<AuctionView>();
		_view.Setup(this.transform);
		
		auctionBuyController = _view.BuyTabGroup_Transform.gameObject.GetMissingComponent<AuctionBuyController>();
		auctionBuyController.ProvidedExternallyView(_view);
		
		auctionSellController = _view.BidAndSellTabGroup_Transform.gameObject.GetMissingComponent<AuctionSellController>();
		auctionSellController.ProvidedExternallyView(_view);
		
		auctionBidController = _view.BidAndSellTabGroup_Transform.gameObject.GetMissingComponent<AuctionBidController>();
		auctionBidController.ProvidedExternallyView(_view);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView后调用
	/// </summary>
	public void RegisterEvent() {
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
	}
	#endregion

	private void OnWealthChanged(WealthNotify notify) {
		_view.IngotNumLabel_UILabel.text = string.Format("{0}", notify.ingot);
	}

	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();
		}

		OnWealthChanged(PlayerModel.Instance.GetWealth());

		InitTabBtn ();
		OnSelectTopTabBtn(TradeDataModel.Instance.auctionTabNum);
	}
	
	#region Init TabBtn
	private string[] _tStrs = {"购买", "竞价", "出售"};
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
		
		_view.BuyTabGroup_Transform.gameObject.SetActive(index == 0);
		_view.BidAndSellTabGroup_Transform.gameObject.SetActive(index == 1 || index == 2);

		switch (index) {
		case 0:
			auctionBuyController.OpenAuctionBuyTab();
			break;
		case 1:
			auctionBidController.OpenAuctionBidTab();
			break;
		case 2:
			auctionSellController.OpenAuctionSellTab();
			break;
		default:
			break;
		}
		
		TradeDataModel.Instance.auctionTabNum = index;
	}
}
