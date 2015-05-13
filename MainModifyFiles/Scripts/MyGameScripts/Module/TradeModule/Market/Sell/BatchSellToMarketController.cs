// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	BatchSellToMarketController.cs
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

using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.whole.modules.stall.data;
using System.Collections.Generic;

//	市场上架数据保存--------------------Struct Begin----------
public struct SellToMark {
	public MarketSellItemCellController cell;

	//	上架单价
	private int _currentPrice;
	public int currentPrice{
		get { return _currentPrice; }
		set { _currentPrice = value; }
	}
	//	上架数量
	private int _currentCount;
	public int currentCount{
		get { return _currentCount; }
		set { _currentCount = value; }
	}
	private int _index;
	public int index{
		get { return _index; }
		set { _index = value; }
	}
	
	public SellToMark(int index, int currentPrice, int currentCount, MarketSellItemCellController tcell) {
		this._index = index;
		this._currentPrice = currentPrice;
		this._currentCount = currentCount;
		this.cell = tcell;
	}
}
//	市场上架数据保存--------------------Struct End----------

public class BatchSellToMarketController : MonoBehaviour,IViewController
{
	private const string ItemsContainerViewName = "Prefabs/Module/TradeModule/Market/BatchSellItemsContainerView";
	private BatchSellToMarketView _view;

	private BatchSellItemsContainerViewController _batchSellItemsContainerViewController = null;
	
	//	上架信息记录
	public SellToMark sellToMark;
	
	//	选择出售物品列表Dic
	private Dictionary<int, SellToMark> _sellToMarkDic = new Dictionary<int, SellToMark>();

	//	缓存数据
	StallGoods _stallGoods = null;
	PackItemDto _packItemDto = null;
	
	//	物品基础价格
	private int _basePrice = 0;
	//	价格增量 **%
	private float _increment = 1.0f;
	//	刷新价格
	private int _price = 0;
	//	物品数量 
	private int _count = 1;
	//	上架总价值
	private int _totalPrice = 0;
	//	刷新上架所需税
	private int _fee = 0;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView()
	{
		_view = this.gameObject.GetMissingComponent<BatchSellToMarketView>();
		_view.Setup (this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemsContainerViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.LeftGroup_Transform.gameObject, prefab);

		_batchSellItemsContainerViewController = module.GetMissingComponent<BatchSellItemsContainerViewController>();
		_batchSellItemsContainerViewController.InitView();
		_batchSellItemsContainerViewController.SetData(H1Item.PackEnum_Backpack, BackpackModel.Instance.GetDto().capability, OnSelectedCallback);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseButton_UIButton.onClick, OnClickCloseButton);
		EventDelegate.Set(_view.QuickAddButton_UIButton.onClick, OnClickQuickAddButton);
		EventDelegate.Set(_view.IncreaseButton_UIButton.onClick, OnClickIncreaseBtn);
		EventDelegate.Set(_view.CutbackButton_UIButton.onClick, OnClickCutbackBtn);
		EventDelegate.Set(_view.IncreaseCountButton_UIButton.onClick, OnClickIncreaseCountBtn);
		EventDelegate.Set(_view.CutbackCountButton_UIButton.onClick, OnClickCutbackCountBtn);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose()
	{
	}
	#endregion

	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();

			CleanItemData();
		}
	}

	//	callback
	private void OnSelectedCallback(MarketSellItemCellController cell) {
		if (cell == null) {
			GameDebuger.OrangeDebugLog("当前回调Cell -> Null");
			return;
		}

		if (cell.GetData() == null) {
			GameDebuger.OrangeDebugLog("当前回调Cell.GetData() -> PackItem -> Null");
			return;
		}

		_stallGoods = DataCache.getDtoByCls<StallGoods>(cell.GetData().itemId);
		_packItemDto = cell.GetData();
		
		if (_stallGoods == null) {
			GameDebuger.OrangeDebugLog("赠送、逛摊、专用以及商城道具，无法上架");
			TipManager.AddTip("赠送、逛摊、专用以及商城道具，无法上架");
			return;
		}

		//	物品基础价格
		_basePrice = LuaManager.Instance.DoStallGoodsBasePriceFormula("StallGoods_"+_stallGoods.id, _stallGoods.basePriceFormula);
		//	价格增量 **%
		_increment = 1.0f;
		//	刷新价格
		_price = _basePrice;
		//	物品数量 
		_count = 1;

		//	加入到SellToMarkDic SetSellRatio
		cell.GetItemCellController().isSelect = !_sellToMarkDic.ContainsKey(_packItemDto.index);
		if (_sellToMarkDic.ContainsKey(_packItemDto.index)) {
			//_sellToMarkDic[_packItemDto.index].
			//cell.GetItemCellController().isSelect = false;
			_sellToMarkDic.Remove(_packItemDto.index);

			CleanItemData();
			cell.SetSellRatio(false);
			//	直接返回
			return;
		} else {
			SellToMark tSellToMark = new SellToMark(_packItemDto.index, _price, _count, cell);

			//cell.GetItemCellController().isSelect = true;
			_sellToMarkDic.Add(_packItemDto.index, tSellToMark);

			cell.SetSellRatio(true, 100);

			//	重新绑定按钮监听
			RegisterEvent();
			//	Grey
			SetBtnSpriteGrey(false);
		}
		if (_stallGoods == null) {
			GameDebuger.OrangeDebugLog("_stallGoods is null");
			return;
		}
		GameDebuger.OrangeDebugLog(_stallGoods.basePriceFormula.ToString());

		SetItemData(_packItemDto.item.icon, _packItemDto.item.name, _packItemDto.count);

		ReSetPriceLabel();
	}

	//	设置ItemCell信息
	private void SetItemData(string iconName, string itemName, int itemCount) {
		_view.ItemIconSprite_UISprite.spriteName = iconName;
		_view.ItemNameLabel_UILabel.text = itemName;
		_view.ItemCountLabel_UILabel.text = itemCount.ToString();
	}
	
	//	刷新AllLabel 设置单价	数量	总价	手续费用
	private void ReSetPriceLabel() {
		_price = Mathf.CeilToInt(_basePrice * _increment);
		_totalPrice = Mathf.CeilToInt(_price * _count);
		_fee = Mathf.CeilToInt(_totalPrice * (_stallGoods == null? 0 : _stallGoods.taxRate));

		if (_packItemDto != null && _sellToMarkDic.ContainsKey(_packItemDto.index)) {
			_sellToMarkDic[_packItemDto.index] = new SellToMark(_sellToMarkDic[_packItemDto.index].index, _price, _count, _sellToMarkDic[_packItemDto.index].cell);
			_sellToMarkDic[_packItemDto.index].cell.SetSellRatio(true, Mathf.CeilToInt(_increment*100));
			GameDebuger.OrangeDebugLog(string.Format("Price -> {0} | Count -> {1}",
				_sellToMarkDic[_packItemDto.index].currentPrice, _sellToMarkDic[_packItemDto.index].currentCount));
		}
		
		_view.IncrementLabel_UILabel.text = string.Format("{0}%", Mathf.CeilToInt(_increment*100));
		_view.PriceValLbl_UILabel.text = string.Format("{0}", _price);
		_view.CountValLbl_UILabel.text = string.Format("{0}", _count);
		_view.TotalPriceValLbl_UILabel.text = string.Format("{0}", _totalPrice);
		_view.FeeValLbl_UILabel.text = string.Format("{0}", _fee);
	}

	//	设置剩余货架数量
	private void SetShelfLabelNum() {
		int countNum = TradeDataModel.maxBatchSellShelfCapability - TradeDataModel.Instance.GetPlayerStallGoodsDto().playerStallItems.Count;
		_view.LeftCountLabel_UILabel.text = string.Format("剩余{0}格", countNum);
	}

	//	重置ItenCell信息
	private void CleanItemData() {
		CleanBtnListen();
		
		SetItemData("0", "", 0);
		SetShelfLabelNum();

		//	物品基础价格
		_basePrice = 0;
		//	价格增量 **%
		_increment = 0.0f;
		//	刷新价格
		_price = _basePrice;
		//	物品数量 
		_count = 0;

		ReSetPriceLabel();
	}

	//	设置按钮图片显示Grey
	private void SetBtnSpriteGrey(bool bState) {
		_view.IncreaseButton_UIButton.GetComponent<UISprite>().isGrey = bState;
		_view.CutbackButton_UIButton.GetComponent<UISprite>().isGrey = bState;
		_view.IncreaseCountButton_UIButton.GetComponent<UISprite>().isGrey = bState;
		_view.CutbackCountButton_UIButton.GetComponent<UISprite>().isGrey = bState;
	}

	//	清除按钮回调
	private void CleanBtnListen() {
		EventDelegate.Set(_view.IncreaseButton_UIButton.onClick, OnNullCallback);
		EventDelegate.Set(_view.CutbackButton_UIButton.onClick, OnNullCallback);
		EventDelegate.Set(_view.IncreaseCountButton_UIButton.onClick, OnNullCallback);
		EventDelegate.Set(_view.CutbackCountButton_UIButton.onClick, OnNullCallback);

		SetBtnSpriteGrey(true);
	}
	private void OnNullCallback() {

	}
	//	---------------------------------------------------------------------
	
	private void OnClickIncreaseBtn() {
		GameDebuger.OrangeDebugLog("点击加5%");
		
		_increment += 0.05f;

		if (_increment >= _stallGoods.maxSaleFactor) {
			_increment = _stallGoods.maxSaleFactor;
		}

		ReSetPriceLabel();
	}
	
	private void OnClickCutbackBtn() {
		GameDebuger.OrangeDebugLog("点击减5%");
		
		_increment -= 0.05f;

		if (_increment < _stallGoods.minSaleFactor) {
			_increment = _stallGoods.minSaleFactor;
		}
		
		ReSetPriceLabel();
	}
	
	private void OnClickIncreaseCountBtn() {
		GameDebuger.OrangeDebugLog("数量加加");
		if (_count >= _packItemDto.count) {
			GameDebuger.OrangeDebugLog("不能再多了");
			return;
		}

		_count++;

		ReSetPriceLabel();
	}
	
	private void OnClickCutbackCountBtn() {
		GameDebuger.OrangeDebugLog("数量减减");
		if (_count <= 1) {
			GameDebuger.OrangeDebugLog("不能再少了");
			return;
		}

		_count--;

		ReSetPriceLabel();
	}

	private void OnClickQuickAddButton() {
		GameDebuger.OrangeDebugLog("快速上架");

		if (_sellToMarkDic.Count > 0) {
			string tStr = "";
			
			bool tFirstIndex = false;
			foreach (SellToMark sellToMark in _sellToMarkDic.Values) {
				if (!tFirstIndex) {
					tFirstIndex = true;
					tStr += string.Format("{0}:{1}:{2}", sellToMark.index, sellToMark.currentPrice, sellToMark.currentCount);
				} else {
					tStr += string.Format(",{0}:{1}:{2}", sellToMark.index, sellToMark.currentPrice, sellToMark.currentCount);
				}
			}
			
			GameDebuger.OrangeDebugLog(string.Format("快速上架 传入参数 -> {0}", tStr));
			
			TradeDataModel.Instance.QuickSellToMark(tStr);
			
			OnClickCloseButton();
		} else {
			GameDebuger.OrangeDebugLog("请选择上架物品");

			TipManager.AddTip("请选择上架物品");
		}
	}

	private void OnClickCloseButton() {
		//	清空
		_sellToMarkDic.Clear();

		ProxyTradeModule.CloseBatchSellToMarket ();
	}
}

