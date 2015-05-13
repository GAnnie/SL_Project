﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for AuctionView.
/// </summary>
public class AuctionView : BaseView
{
	public UIGrid TabGrid_UIGrid;
	public UIButton PreviousBtn_UIButton;
	public UIButton OnePriceBtn_UIButton;
	public UIButton NextpageBtn_UIButton;
	public UIButton BiddingBtn_UIButton;
	public PageScrollView ItemCellListGrid_PageScrollView;
	public UIGrid TypeListGrid_UIGrid;
	public UILabel IngotNumLabel_UILabel;
	public Transform BuyTabGroup_Transform;
	public Transform BidAndSellTabGroup_Transform;
	public UILabel PageLabel_UILabel;
	public UIScrollView TypeListScrollView_UIScrollView;
	public UIScrollView BidScrollView_UIScrollView;
	public UIGrid BidAndSellListGrid_UIGrid;

	public void Setup (Transform root)
	{
		TabGrid_UIGrid = root.Find("TabGrid").GetComponent<UIGrid>();
		PreviousBtn_UIButton = root.Find("BtnControls/PreviousBtn").GetComponent<UIButton>();
		OnePriceBtn_UIButton = root.Find("BtnControls/OnePriceBtn").GetComponent<UIButton>();
		NextpageBtn_UIButton = root.Find("BtnControls/NextpageBtn").GetComponent<UIButton>();
		BiddingBtn_UIButton = root.Find("BtnControls/BiddingBtn").GetComponent<UIButton>();
		ItemCellListGrid_PageScrollView = root.Find("BuyTabGroup/RightInfoGroup/ItemCellScrollView/ItemCellListGrid").GetComponent<PageScrollView>();
		TypeListGrid_UIGrid = root.Find("BuyTabGroup/TypeListGroup/TypeListScrollView/TypeListGrid").GetComponent<UIGrid>();
		IngotNumLabel_UILabel = root.Find("LabelInfoGroup/IngotGroup/IngotNumLabel").GetComponent<UILabel>();
		BuyTabGroup_Transform = root.Find("BuyTabGroup");
		BidAndSellTabGroup_Transform = root.Find("BidAndSellTabGroup");
		PageLabel_UILabel = root.Find("LabelInfoGroup/PageGroup/PageLabel").GetComponent<UILabel>();
		TypeListScrollView_UIScrollView = root.Find("BuyTabGroup/TypeListGroup/TypeListScrollView").GetComponent<UIScrollView>();
		BidScrollView_UIScrollView = root.Find("BidAndSellTabGroup/ItemScrollerVeiw/BidScrollView").GetComponent<UIScrollView>();
		BidAndSellListGrid_UIGrid = root.Find("BidAndSellTabGroup/ItemScrollerVeiw/BidScrollView/BidAndSellListGrid").GetComponent<UIGrid>();
	}
}
