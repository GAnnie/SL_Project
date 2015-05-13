// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	AuctionBuyBiddingController.cs
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

public class AuctionBuyBiddingController : MonoBehaviour,IViewController {
	private BiddingWindowView _view;
	
	//	缓存数据
	//StallGoods _stallGoods = null;
	//StallGoodsDto _curretnStallGoodsDto;
	
	//	物品基础价格
	private int _basePrice = 0;
	//	价格增量 **%
	private float _reIncrement = 1.0f;
	//	价格
	private int _rePrice = 0;
	//	总价值
	private int _reTotalPrice = 0;
	
	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<BiddingWindowView>();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView后调用
	/// </summary>
	public void RegisterEvent() {
		EventDelegate.Set (_view.IncreaseButton_UIButton.onClick, OnClickIncreaseBtn);
		EventDelegate.Set (_view.CutbackButton_UIButton.onClick, OnClickCutbackBtn);
		EventDelegate.Set (_view.BiddingButton_UIButton.onClick, OnClickBiddingBtn);
		EventDelegate.Set (_view.CloseButton_UIButton.onClick, OnClickCloseButton);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		
	}
	#endregion
	
	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();
		}

		/*
		_curretnStallGoodsDto = TradeDataModel.Instance.lastStallGoodsDto;
		_stallGoods = DataCache.getDtoByCls<StallGoods>(_curretnStallGoodsDto.id);
		
		GeneralItem tGeneralItem = DataCache.getDtoByCls<GeneralItem>(_curretnStallGoodsDto.id);
		
		//	物品基础价格
		_basePrice = 10000;//_stallGoods.basePriceFormula;
		//	价格增量 **%
		_reIncrement = 1.0f;
		//	刷新价格
		_rePrice = _basePrice;
		//	物品数量 
		_reCount = _curretnStallGoodsDto.amount;

		SetItemCellData(tGeneralItem.icon, tGeneralItem.name, _reCount);
		ReSetPriceLabel();
		*/
	}
	
	public void SetItemCellData(string iconName, string itemName, int itemCount) {
		_view.IconSprite_UISprite.spriteName = iconName;
		_view.NameLabel_UILabel.text = itemName;
		_view.CountLabel_UILabel.text = itemCount.ToString();
	}
	
	//	刷新AllLabel 设置单价	数量	总价	手续费用
	public void ReSetPriceLabel() {
		/*
		_rePrice = Mathf.CeilToInt(_basePrice * _reIncrement);
		_reTotalPrice = Mathf.CeilToInt(_rePrice * _reCount);
		_reFee = Mathf.CeilToInt(_reTotalPrice * _stallGoods.taxRate);
		
		_view.IncrementLabel_UILabel.text = string.Format("{0}%", Mathf.CeilToInt(_reIncrement*100));
		_view.Prive_UILabel.text = string.Format("{0} {1}", _rePrice, ItemIconConst.Copper);
		_view.TotalPrice_UILabel.text = string.Format("{0} {1}", _reTotalPrice, ItemIconConst.Copper);
		_view.Fee_UILabel.text = string.Format("{0} {1}", _reFee, ItemIconConst.Copper);
		*/
	}
	
	private void OnClickIncreaseBtn() {
		GameDebuger.OrangeDebugLog("点击加加");
		_reIncrement += 0.05f;

		/*
		if (_reIncrement >= _stallGoods.maxSaleFactor) {
			_reIncrement = _stallGoods.maxSaleFactor;
		}
		*/
		
		ReSetPriceLabel();
	}
	
	private void OnClickCutbackBtn() {
		GameDebuger.OrangeDebugLog("点击减减");
		
		_reIncrement -= 0.05f;

		/*
		if (_reIncrement < _stallGoods.minSaleFactor) {
			_reIncrement = _stallGoods.minSaleFactor;
		}
		*/
		
		ReSetPriceLabel();
	}
	
	private void OnClickBiddingBtn() {
		GameDebuger.OrangeDebugLog("点击竞价");
		
		//TradeDataModel.Instance.ItemShelves(TradeDataModel.Instance.lastStallGoodsDto.stallId, OnRefreshCallback);
	}
	
	private void OnRefreshCallback() {

	}
	
	private void OnClickCloseButton() {
		//	清除数据
		TradeDataModel.Instance.lastStallGoodsDto = null;
		
		ProxyTradeModule.CloseBiddingWindowView();
	}
}
