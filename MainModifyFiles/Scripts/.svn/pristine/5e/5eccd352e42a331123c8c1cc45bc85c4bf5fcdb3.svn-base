using UnityEngine;
using System.Collections;

public class ProxyTradeModule
{
	#region Trade
	private const string NAME_TradeBaseView = "Prefabs/Module/TradeModule/TradeBaseView";

	public static void Open()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_TradeBaseView, UILayerType.DefaultModule, true);
		
		var controller = ui.GetMissingComponent<TradeBaseController>();
		controller.Open();
	}
	public static void Close()
	{
		UIModuleManager.Instance.CloseModule (NAME_TradeBaseView);
	}
	#endregion


	#region BatchSellToMarketController
	private const string NAME_BatchSellToMarketView = "Prefabs/Module/TradeModule/Market/BatchSellToMarketView";
	
	public static void OpenBatchSellToMarket()
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_BatchSellToMarketView, UILayerType.ThreeModule, true);
		
		var controller = ui.GetMissingComponent<BatchSellToMarketController>();
		controller.Open ();
	}
	public static void CloseBatchSellToMarket()
	{
		UIModuleManager.Instance.CloseModule (NAME_BatchSellToMarketView);
	}
	#endregion

	#region 商品下架
	private const string NAME_ShelvesSellToMarketView = "Prefabs/Module/TradeModule/Market/ShelvesView";
	public static void OpenShelvesView() {
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_ShelvesSellToMarketView, UILayerType.ThreeModule, true);
		
		ShelvesController controller = ui.GetMissingComponent<ShelvesController>();
		controller.Open ();
	}

	public static void CloseShelvesView() {
		UIModuleManager.Instance.CloseModule (NAME_ShelvesSellToMarketView);
	}
	#endregion
	
	#region 商品拍卖竞价
	private const string NAME_BiddingWindowView = "Prefabs/Module/TradeModule/Auction/BiddingWindowView";
	public static void OpenBiddingWindowView() {
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_BiddingWindowView, UILayerType.ThreeModule, true);
		
		AuctionBuyBiddingController controller = ui.GetMissingComponent<AuctionBuyBiddingController>();
		controller.Open ();
	}
	
	public static void CloseBiddingWindowView() {
		UIModuleManager.Instance.CloseModule (NAME_BiddingWindowView);
	}
	#endregion
}

