// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	TradeDataModel.cs
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
using com.nucleus.h1.logic.whole.modules.stall.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.whole.modules.trade.data;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.whole.modules.stall.data;
using com.nucleus.player.data;

/// <summary>
/// 游戏中保存交易中心打开的菜单选项 ----------Struct Begin----------
/// </summary>
public struct MenuHierarchy {
	private int _mainMenu;
	public int mainMenu{
		get { return _mainMenu; }
		set { _mainMenu = value; }
	}
	private int _subMenu;
	public int subMenu{
		get { return _subMenu; }
		set { _subMenu = value; }
	}
	private int _selectedItem;
	public int selectedItem{
		get { return _selectedItem; }
		set { _selectedItem = value; }
	}
	private bool _settinged;
	public bool settinged {
		get { return _settinged; }
		set { _settinged = value; }
	}
	
	public MenuHierarchy(int mainM, int subM, int selectedI, bool settinged) {
		this._mainMenu = mainM;
		this._subMenu = subM;
		this._selectedItem = selectedI;
		this._settinged = settinged;
	}
}
//	游戏中保存交易中心打开的菜单选项 ----------Struct Endf----------

public class TradeDataModel {

	private static readonly TradeDataModel instance = new TradeDataModel();
	public static TradeDataModel Instance {
		get {
			return instance;
		}
	}


	//	debug first state -- BackpackOrWarehouseItemCellController 调试使用
	public static bool debugFirstSta = false;

	//	是否使用uLua方式调用
	public bool useDynamic = false;

	//	通用配置
	//	是否菜单倒序
	public readonly bool reverseOrder = false;

	//	SellItemCell Lock锁状态开启需要元宝数量
	public int unlockIngotNum = 20;

	//----------------------------记录信息--------------------------------
	//	摆摊购买记录菜单选项
	public MenuHierarchy menuHierarchy;

	//	拍卖购买记录菜单选项
	public MenuHierarchy auctionMenuHierarchy;

	//	Trade分类选项卡
	public readonly int defaultTradeBaseTabNum = 0;//2;

	//	市场记录Tab选项卡
	public int marketTabNum = 0;
	
	//	拍卖记录Tab选项卡
	public int auctionTabNum = 0;

	//	默认市场一级菜单Num
	public int defaultTradeMenuNum = 2100;

	//----------------------------市场购买---------------------------------
	//	摆摊显示总数量
	public int maxStallTotalCount = 0;
	//	摆摊每页显示数量
	public int maxStallEachPagCount = 0;

	//	菜单信息
	private List<TradeMenu> _tradeMenuList = null;

	//	玩家自己摆摊信息
	private PlayerStallGoodsDto _playerStallGoodsDto = null;
	//	其他玩家摆摊信息
	private Dictionary<int, StallCenterDto> _stallCenterDtoDic = new Dictionary<int, StallCenterDto>();

	//----------------------------市场出售---------------------------------
	//	一键上架每页显示最大ItemCell
	public static readonly int maxBatchSellPageCapability = 15;
	
	//	剩余可上架数量
	public static int maxBatchSellShelfCapability = 10;

	//	上一次下架的商品
	public StallGoodsDto lastStallGoodsDto = null;
	
	//----------------------------拍卖行相关信息---------------------------------
	public bool fitstAuctionOpened = false;

	//	------------	交易中心 Begin
	//	进入交易中心
	public void EnterTradeCenter(System.Action<GeneralResponse> actionCallback) {
		GameDebuger.OrangeDebugLog("开始进入交易中心");

		ServiceRequestAction.requestServer (TradeWindowService.enter(), "enter tradecenter", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("进入交易中心成功");

			if (actionCallback != null) {
				actionCallback(e);
			}
		});
	}

	//	交易中心中购买物品
	public void BuyInTradeCenter(int tradeID) {
		ServiceRequestAction.requestServer (TradeService.buy(tradeID), "buy in tradeCenter", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("交易中心购买成功");
			GameLogicHelper.HandlerItemTipDto(e as ItemTipDto, true);
		});
	}

	//	退出交易中心
	public void ExitTradeCenter() {
		ServiceRequestAction.requestServer (TradeWindowService.exit(), "exit tradecenter", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("退出交易中心成功");

		});
	}
	//	------------	交易中心 End

	//	------------	市场 Begin
	//	获取市场我的摊位数据
	public PlayerStallGoodsDto GetPlayerStallGoodsDto() {
		return _playerStallGoodsDto;
	}

	//	获取市场其他玩家摊位数据Dic
	public Dictionary<int, StallCenterDto> GetPlayerStallCenterDtoByMenuidDic() {
		return _stallCenterDtoDic;
	}
	//	获取市场其他玩家摊位数据
	public StallCenterDto GetPlayerStallCenterDtoByMenuid(int menuid) {
		return _stallCenterDtoDic[menuid];
	}

	//	进入市场摆摊（玩家自己摊位）
	private System.Action _refreshMarketCallback = null;
	public void EnterMarket(System.Action actionCallback, System.Action sellToMarkRefreshCallback) {
		GameDebuger.OrangeDebugLog("开始进入我的摊位");

		if (sellToMarkRefreshCallback != null) {
			_refreshMarketCallback = sellToMarkRefreshCallback;
		}

		ServiceRequestAction.requestServer (StallWindowService.enter(), "get my market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("进入我的摊位成功");
			PlayerStallGoodsDto tPlayerStallGoodsDto = e as PlayerStallGoodsDto;
			_playerStallGoodsDto = tPlayerStallGoodsDto;
			maxBatchSellShelfCapability = tPlayerStallGoodsDto.capability;
			unlockIngotNum = 20 + 2 * (maxBatchSellShelfCapability - 10);

			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
	
	//	进入市场摆摊(其他玩家摊位)
	public void MenuMarket(int menuID, System.Action actionCallback) {
		GameDebuger.OrangeDebugLog(string.Format("开始进入其他玩家摆摊 ID -> {0}", menuID));
		
		maxStallEachPagCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.STALL_ITEM_PAGE_SIZE, 10);
		maxStallTotalCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.STALL_ITEM_TOTAL_COUNT, 20);
		
		ServiceRequestAction.requestServer (StallWindowService.menu(menuID), "get menu with other", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("获取其他玩家摊位信息成功");
			
			_stallCenterDtoDic.Add(menuID, e as StallCenterDto);
			
			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
	
	//	刷新市场摆摊(其他玩家摊位数据->需要清除之前保存的数据信息)
	public void RefreshMarket(System.Action actionCallback) {
		
		ServiceRequestAction.requestServer (StallWindowService.refresh(menuHierarchy.mainMenu), "refresh market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("刷新市场摆摊成功");

			//	清除之前保存的数据信息
			_stallCenterDtoDic.Clear();
			/*                                                                                         
			if (_stallCenterDtoDic.ContainsKey(menuHierarchy.mainMenu)) {
				_stallCenterDtoDic.Remove(menuHierarchy.mainMenu);
			}
			*/
			_stallCenterDtoDic.Add(menuHierarchy.mainMenu, e as StallCenterDto);

			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
		
	//	市场摆摊扩容
	public void ExpandMarket(System.Action actionCallback) {
		ServiceRequestAction.requestServer (StallWindowService.expand(), "expand market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("摊位扩容成功");
			
			maxBatchSellShelfCapability++;
			unlockIngotNum = 20 + 2 * (maxBatchSellShelfCapability - 10);

			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
	
	//	市场摆摊提现
	public void CashMarket(System.Action actionCallback) {
		ServiceRequestAction.requestServer (StallWindowService.cash(), "cash market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("摊位提现成功");
			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
	
	//	市场摆摊购买
	public void BuyMarket(long stallID, System.Action<GeneralResponse> actionCallback) {
		ServiceRequestAction.requestServer(StallWindowService.buy(stallID), "buy in market", (GeneralResponse e) => {
			GameDebuger.OrangeDebugLog("摊位购买成功");

			GameLogicHelper.HandlerItemTipDto(e as ItemTipDto, true);

			if (actionCallback != null) {
				actionCallback(e);
			}
		});
	}

	//	一键上架
	public void QuickSellToMark(string str) {
		ServiceRequestAction.requestServer (StallService.up(str), "quick sell to market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("一键上架成功 需要刷新我的摊位");
			TipManager.AddTip("恭喜，你成功上架了商品");
			RefreshMySellMarket();
		});
	}

	//	下架
	public void ItemShelves(StallGoodsDto stallGoodsDto, System.Action actionCallback) {
		ServiceRequestAction.requestServer (StallService.down(stallGoodsDto.stallId), "item Shelves with market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("商品下架成功");
			GeneralItem tGreeralItem = DataCache.getDtoByCls<GeneralItem>(stallGoodsDto.id);
			TipManager.AddTip(string.Format("你下架了{0}",tGreeralItem.name));

			if (actionCallback != null) {
				actionCallback();
			}
		});
	}

	//重新上架
	public void ReSellToMark(StallGoodsDto stallGoodsDto, int price, System.Action actionCallback) {
		ServiceRequestAction.requestServer (StallService.reup(stallGoodsDto.stallId, price), "reSell to market", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("重新上架成功 需要刷新我的摊位");

			GeneralItem tGreeralItem = DataCache.getDtoByCls<GeneralItem>(stallGoodsDto.id);
			TipManager.AddTip(string.Format("你重新上架了{0}",tGreeralItem.name));

			if (actionCallback != null) {
				actionCallback();
			}
		});
	}
	//	------------	市场 End

	//	------------	拍卖行 Begin
	//	进入拍卖行
	public void EnterAuction(System.Action<GeneralResponse> actionCallback) {
		GameDebuger.OrangeDebugLog("开始进入拍卖行");
		
		ServiceRequestAction.requestServer (TradeWindowService.enter(), "enter auction", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("进入拍卖行成功");
			
			if (actionCallback != null) {
				actionCallback(e);
			}
		});
	}
	
	//	拍卖行购买物品
	public void BuyInAuction(int tradeID) {
		ServiceRequestAction.requestServer (TradeService.buy(tradeID), "buy in auction", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("拍卖行购买成功");
			
		});
	}
	
	//	退出拍卖行
	public void ExitAuction() {
		ServiceRequestAction.requestServer (TradeWindowService.exit(), "exit auction", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("退出拍卖行成功");
			
		});
	}
	//	------------	拍卖行 End

	//	刷新我的摊位(上架 下架之后的操作)
	public void RefreshMySellMarket() {
		if (_refreshMarketCallback != null) {
			_refreshMarketCallback();
		}
	}

	//	获取菜单数据信息
	public List<TradeMenu> GetTradeMenu() {
		if (_tradeMenuList == null) {
			_tradeMenuList = DataCache.getArrayByClsWithoutSort<TradeMenu>();
			
			if (reverseOrder) {
				_tradeMenuList.Sort(delegate(TradeMenu x, TradeMenu y) {
					return -x.id.CompareTo(y.id);
				});
			}
		}

		return _tradeMenuList;
	}

	//	获取二级菜单数据信息
	public List<TradeMenu> GetSubTradeMenu(int menuID) {
		List<TradeMenu> tTradeMenuList = new List<TradeMenu>();

		for(int i = 0, len = _tradeMenuList.Count; i < len; i++) {
			TradeMenu tTradeMenu = _tradeMenuList[i];
			if (tTradeMenu.parentId == menuID && IsOpenMenu(tTradeMenu)) {
				tTradeMenuList.Add(tTradeMenu);
			}
		}

		return tTradeMenuList;
	}

	//	服务器开放等级
	public bool IsOpenMenu(TradeMenu menu) {
		return menu.gameServerGrade <= PlayerModel.Instance.ServerGrade;
	}

	//	可上架物品获取
	public List<PackItemDto> GetSellItemList() {
		return BackpackModel.Instance.GetSellItemList();
	}
}

