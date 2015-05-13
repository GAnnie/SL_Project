// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	SellItemCellController.cs
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
using System;
using com.nucleus.h1.logic.whole.modules.stall.dto;
using com.nucleus.player.data;
using com.nucleus.player.msg;

public class SellItemCellController : MonoBehaviour,IViewController
{
	private SellItemCell _view;

	//	临时存储在cell中的Dto
	private StallGoodsDto _stallGoodsDto = null;
	//private PackItemDto _packItemDto = null;

	public event Action<SellItemCellController> _OnItemCellSelect;

	public enum TypeItemStatus {
		//	不做处理
		nothing = 0,
		//	其他玩家摊位使用
		//	售罄
		soldout = 1,
		//	自己摊位使用
		//	提现\超时
		withdrawals = 2,
		timeout = 3
	}
	private TypeItemStatus _typeItemStatus = TypeItemStatus.nothing;

	public enum TypeItemItemCell {
		marketBuy = 0,
		marketSell = 1,
	}
	private TypeItemItemCell _typeItemItemCell = TypeItemItemCell.marketBuy;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView()
	{
		_view = this.gameObject.GetMissingComponent<SellItemCell>();
		_view.Setup (this.transform);
	}

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView后调用
	/// </summary>
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.SellItemCell_UIButton.onClick, OnClickItemCell);
		EventDelegate.Set(_view.BgSprite_UIButton.onClick, OnClickIconCell);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose()
	{
	}
	#endregion

	#region Setup SellItemCell
	public void OpenSellItemCell() {
		InitView();
		RegisterEvent();
	}
	#endregion
	
	#region GetThe MetaData
	public StallGoodsDto GetStallGoodsDto() {
		return _stallGoodsDto;
	}

	/*
	public PackItemDto GetPackItemDto() {
		return _packItemDto;
	}
	*/
	#endregion

	#region 设置 StallGoodsDto / PackItemDto 类型数据信息
	//	StallGoods Type
	public void SetStallGoodsDto(StallGoodsDto stallGoodsDto, TypeItemItemCell typeItemItemCell, Action<SellItemCellController> OnItemCellSelect) {
		_stallGoodsDto = stallGoodsDto;
		_typeItemItemCell = typeItemItemCell;
		_OnItemCellSelect = OnItemCellSelect;

		//	Clean
		CleanItemData();

		ShowSellGroup();
	}

	//	PackItem Type
	/*
	public void SetPackItemDto(PackItemDto packItemDto, TypeItemItemCell typeItemItemCell, Action<SellItemCellController> OnItemCellSelect) {
		_packItemDto = packItemDto;
		_typeItemItemCell = typeItemItemCell;
		_OnItemCellSelect = OnItemCellSelect;

		//	Clean
		CleanItemData();
		
		ShowSellGroup();
	}
	*/
	#endregion

	#region 根据isStallItem类型显示Group
	private void ShowSellGroup() {
		if (_stallGoodsDto != null) {
			GeneralItem tGreeralItem = DataCache.getDtoByCls<GeneralItem>(_stallGoodsDto.id);

			SetItemCellData(tGreeralItem.icon, tGreeralItem.name, _stallGoodsDto.amount);

			_view.CostLabel_UILabel.text = string.Format("{0}", _stallGoodsDto.price);
			ShowWidgetsActivity(false, false, true);
		} else {
			ShowWidgetsActivity(false, true, false);
		}
		
		/*
		else if (_typeItemItemCell == TypeItemItemCell.marketSell) {
			if(_packItemDto != null) {
				SetItemCellData(_packItemDto.item.icon, _packItemDto.item.name, _packItemDto.count);

				_view.CostLabel_UILabel.text = string.Format("{0}{1}", _packItemDto.tradePrice, ItemIconConst.Copper);
			}
			ShowWidgetsActivity(false, false, true);
		}
		 */
	}
	#endregion

	#region 设置ItemCell Data
	private void SetItemCellData(string iconName, string itemName, int itemCountNum) {
		_view.ItemIcon_UISprite.spriteName = iconName;
		_view.NameLabel_UILabel.text = itemName;
		_view.ItemCountLabel_UILabel.text = string.Format("{0}", itemCountNum);

 		if (itemCountNum <= 0) {
			if (_typeItemItemCell == TypeItemItemCell.marketBuy) {
				_typeItemStatus = TypeItemStatus.soldout;
			} else if (_typeItemItemCell == TypeItemItemCell.marketSell) {
				_typeItemStatus = TypeItemStatus.withdrawals;
			}
			SetStatusState(true);
		}
	}
	#endregion
	
	#region 设置各个Group Activity
	private void ShowWidgetsActivity(bool lockSta = false, bool addSta = false, bool sellSta = false) {
		_view.LockGroup_Transform.gameObject.SetActive(lockSta);
		_view.AddGroup_Transform.gameObject.SetActive(addSta);
		_view.SellGroup_Transform.gameObject.SetActive(sellSta);
	}
	#endregion

	#region 设置是否选中效果
	public void Selected(bool selectSta) {
		_view.SelectSprite_UISprite.gameObject.SetActive (selectSta);
	}
	#endregion

	#region 设置 ItemCell Count (并判断是否售罄)
	public void SetItemCellCount() {
		_stallGoodsDto.amount--;

		_view.ItemCountLabel_UILabel.text = string.Format("{0}", _stallGoodsDto.amount);

		//	售罄
		if (_stallGoodsDto.amount <= 0) {
			_typeItemStatus = TypeItemStatus.soldout;
			SetStatusState(true);
		}
	}
	#endregion

	#region 获取当前类型
	public TypeItemStatus GetTypeItemStatus() {
		return _typeItemStatus;
	}
	#endregion

	#region 设置是否提现
	public void SetItemCellWithdrawals() {
		_typeItemStatus = TypeItemStatus.withdrawals;
		SetStatusState(true);
	}
	#endregion
	
	#region 设置是否超时
	public void SetItemCellTimeOut() {
		_typeItemStatus = TypeItemStatus.timeout;
		SetStatusState(true);
	}
	#endregion

	#region 设置提现 超时 售罄
	public void SetStatusState(bool statusSta) {
		_view.StatusSprite_UISprite.enabled = statusSta;

		if (statusSta && _typeItemStatus != TypeItemStatus.nothing) {
			switch (_typeItemStatus) {
			case TypeItemStatus.soldout:
				_view.StatusSprite_UISprite.spriteName = "icon-shouqing";
				_view.StatusSprite_UISprite.isGrey = true;

				_view.SellItemCell_UISprite.isGrey = true;
				break;
			case TypeItemStatus.timeout:
				_view.StatusSprite_UISprite.spriteName = "icon-chaoshi";
				break;
			case TypeItemStatus.withdrawals:
				_view.StatusSprite_UISprite.spriteName = "icon-ketixian";
				break;
			}
		}
	}
	#endregion

	#region 设置SellItem有锁
	public void SetItemCellLockState () {
		ShowWidgetsActivity(true);
		SetStatusState(true);
		_view.MoneyLabel_UILabel.text = string.Format("{0}", TradeDataModel.Instance.unlockIngotNum);
	}
	#endregion

	#region 是否有锁
	public bool IsLock() {
		return _view.LockGroup_Transform.gameObject.activeSelf;
	}
	#endregion

	#region Clean ItemCell Data
	private void CleanItemData() {
		Selected(false);
		SetStatusState(false);

		_view.StatusSprite_UISprite.isGrey = false;
		_view.SellItemCell_UISprite.isGrey = false;
	}
	#endregion

	#region Btn Handel
	private void OnClickItemCell() {
		if (_OnItemCellSelect != null) {
			_OnItemCellSelect(this);
		}
	}
	#endregion
	
	#region Btn Handel
	private void OnClickIconCell() {
		/*
		if (_OnItemCellSelect != null) {
			_OnItemCellSelect(this);
		}
		*/
		if (_stallGoodsDto != null) {
			ProxyItemTipsModule.Open(_stallGoodsDto.id, _view.BgSprite_UIButton.gameObject);
		} else {
			OnClickItemCell();
		}
	}
	#endregion
}

