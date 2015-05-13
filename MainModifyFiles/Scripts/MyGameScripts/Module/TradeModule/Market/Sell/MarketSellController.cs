// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MarketSellController.cs
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
using com.nucleus.player.msg;
using System.Collections.Generic;
using com.nucleus.h1.logic.whole.modules.stall.dto;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules;

public class MarketSellController : MonoBehaviour {
	private MarketView _view;

	public const string sellItemCell = "Prefabs/Module/TradeModule/Market/SellItemCell";
	private GameObject _sellItemCellPrefabObj = null;
	
	#region 外部提供的view
	public void ProvidedExternallyView(MarketView view) {
		_view = view;
		SetBtnClickCallback();

		if (_sellItemCellPrefabObj == null) {
			_sellItemCellPrefabObj = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(sellItemCell);
		}
	}
	#endregion
	
	/// <summary>
	/// Registers the btn click event.
	/// </summary>
	private void SetBtnClickCallback() {
		EventDelegate.Set (_view.QuickAddButton_UIButton.onClick, OnClickQuickAddButton);
		EventDelegate.Set (_view.QuickGainButton_UIButton.onClick, OnClickQuickGainButton);
	}

	#region 打开市场出售标签
	private bool _firstOpened = false;
	public void OpenMarketSellTab() {
		//	保持tab只打开一次
		if (!_firstOpened) {
			_firstOpened = true;

			InitMarketSellTabView();
		}
	}
	#endregion

	#region 上架物品实例
	private List<SellItemCellController> _sellItemCellControllerList = new List<SellItemCellController>();
	private void InitMarketSellTabView() {
		List<StallGoodsDto> tSellItemList = TradeDataModel.Instance.GetPlayerStallGoodsDto().playerStallItems;
		
		if (tSellItemList.Count > 0) {
			//	按id排序（升序）
			tSellItemList.Sort(delegate(StallGoodsDto x, StallGoodsDto y) {
				return x.id.CompareTo(y.id);
			});
		}

		SellItemCellController tSellItemCellController = null;

		for (int i = 0, len = _sellItemCellControllerList.Count; i < len; i++) {
			SellItemCellController tCell = _sellItemCellControllerList[i];
			tCell.name = string.Format("{0}", i);
			tCell.SetStallGoodsDto(i < tSellItemList.Count? tSellItemList[i] : null, SellItemCellController.TypeItemItemCell.marketSell, OnItemCellSelect);

			if (_sellItemCellControllerList.Count == TradeDataModel.maxBatchSellShelfCapability + 1) {
				if (i == len-1 && tSellItemCellController == null) {
					tSellItemCellController = tCell;
				}
			}
		}

		for (int i = _sellItemCellControllerList.Count, len = TradeDataModel.maxBatchSellShelfCapability + 1; i < len; i++) {
			SellItemCellController controller = CreateNewSellItemCell(i, i < tSellItemList.Count? tSellItemList[i] : null, false);

			if (i == len-1 && tSellItemCellController == null) {
				tSellItemCellController = controller;
			}
		}

		//	有锁
		if (tSellItemCellController != null) {
			tSellItemCellController.SetItemCellLockState();
		}

		_view.SellGrid_UIGrid.Reposition();
	}

	//	生成一个物品并返回
	private SellItemCellController CreateNewSellItemCell(int indexNum, StallGoodsDto stallGoodsDto, bool isNeedReposition = false) {
		GameObject go = NGUITools.AddChild(_view.SellGrid_UIGrid.gameObject, _sellItemCellPrefabObj);
		go.name = string.Format("{0}", indexNum);
		
		SellItemCellController controller = go.AddMissingComponent<SellItemCellController>();
		controller.OpenSellItemCell();
		controller.SetStallGoodsDto(stallGoodsDto, SellItemCellController.TypeItemItemCell.marketSell, OnItemCellSelect);
		
		_sellItemCellControllerList.Add(controller);

		if (isNeedReposition) {
			_view.SellGrid_UIGrid.Reposition();
		}

		return controller;
	}

	//	物品选择回调
	private void OnItemCellSelect(SellItemCellController itemCell) {
		StallGoodsDto tStallGoodsDto = itemCell.GetStallGoodsDto();
		GameDebuger.OrangeDebugLog(
			string.Format("RODO -> 选择商品 {0}", tStallGoodsDto == null?
		              "NULL" : DataCache.getDtoByCls<GeneralItem>(tStallGoodsDto.id).name));

		if (tStallGoodsDto == null) {
			if (itemCell.IsLock()) {
				GameDebuger.OrangeDebugLog(string.Format("RODO -> 开启上架格子"));

				ProxyWindowModule.OpenConfirmWindow(string.Format("是否消耗{0}{1}开启下一个摊位", TradeDataModel.Instance.unlockIngotNum, ItemIconConst.Ingot), "开启摊位", ()=>{
					TradeDataModel.Instance.ExpandMarket(delegate() {
						//	调整上一个ItemCell为可上架
						_sellItemCellControllerList[_sellItemCellControllerList.Count-1].SetStallGoodsDto(null, SellItemCellController.TypeItemItemCell.marketSell, OnItemCellSelect); 
						
						if (TradeDataModel.maxBatchSellShelfCapability >= 20) {
							GameDebuger.OrangeDebugLog("已达最大上限");
							return;
						}
						
						SellItemCellController controller = CreateNewSellItemCell(TradeDataModel.maxBatchSellShelfCapability, null, true);
						controller.SetItemCellLockState();
					});
				});
			} else {
				GameDebuger.OrangeDebugLog(string.Format("RODO -> 商品上架"));
				ProxyTradeModule.OpenBatchSellToMarket();
			}
		} else {
			//	提现 / 重新上架
			if (itemCell.GetTypeItemStatus() == SellItemCellController.TypeItemStatus.withdrawals) {
				GameDebuger.OrangeDebugLog(string.Format("RODO -> 商品可提现"));

				//TradeDataModel.Instance.CashMarket();
			} else {
				GameDebuger.OrangeDebugLog(string.Format("RODO -> 商品下架/重新上架"));

				TradeDataModel.Instance.lastStallGoodsDto = tStallGoodsDto;
				ProxyTradeModule.OpenShelvesView();
			}
		}
	}
	#endregion

	#region 上架物品刷新
	public void RefreshMarketSellTabView() {
		TradeDataModel.Instance.EnterMarket(delegate() {
			GameDebuger.OrangeDebugLog("上架物品刷新");
			InitMarketSellTabView();
		}, null);
	}
	#endregion

	#region Sell Handle
	private void OnClickQuickAddButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 一键上架"));
		//ProxyTradeModule.OpenBatchSellToMarket ();
	}
	
	private void OnClickQuickGainButton() {
		GameDebuger.OrangeDebugLog(string.Format("RODO -> 一键提现"));
		TradeDataModel.Instance.CashMarket(delegate() {
			InitMarketSellTabView();
		});
	}
	#endregion
}
