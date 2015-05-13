// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	AuctionBidController.cs
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

public class AuctionBidController : MonoBehaviour {
	private AuctionView _view;
	
	#region 外部提供的view
	public void ProvidedExternallyView(AuctionView view) {
		_view = view;
		SetBtnClickCallback();
		
		/*
		if (_buyTypeItemPrefabObj == null) {
			_buyTypeItemPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(buyTypeItemPath);
		}
		if (buyGoodsItemPrefabObj == null) {
			buyGoodsItemPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(buyGoodsItemPath);
		}
		*/
	}
	#endregion
	
	/// <summary>
	/// Registers the btn click event.
	/// </summary>
	private void SetBtnClickCallback() {
//		EventDelegate.Set (_view.BiddingBtn_UIButton.onClick, OnClickBidButton);
//		EventDelegate.Set (_view.OnePriceBtn_UIButton.onClick, OnClickOnePriceButton);
//		EventDelegate.Set (_view.PreviousBtn_UIButton.onClick, OnClickPrePageButton);
//		EventDelegate.Set (_view.NextpageBtn_UIButton.onClick, OnClickNextPageButton);
	}
	
	#region 打开市场买入标签
	private bool _firstOpened = false;
	public void OpenAuctionBidTab() {
		//	保持tab只打开一次
		if (!_firstOpened) {
			_firstOpened = true;
			
			/*
			_copper = PlayerModel.Instance.GetWealth().copper;
			
			InitBuyTabView();
			SetPlayerCopper();
			StartTimeCoolDown();
			
			//	切页
			_view.ItemCellGrid_PageScrollView.onChangePage = delegate(int curPage) {
				_currentPag = curPage;
				_view.PageLabel_UILabel.text = string.Format("第{0}/{1}页", curPage , _view.ItemCellGrid_PageScrollView.MaxPage);
			};
			*/
		}
	}
	#endregion
}
