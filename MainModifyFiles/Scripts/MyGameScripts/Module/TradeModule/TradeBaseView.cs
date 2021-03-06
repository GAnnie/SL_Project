﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for TradeBaseView.
/// </summary>
public class TradeBaseView : BaseView
{
	public UISprite NameSprite;
	public UIButton CloseButton;
	public UIGrid TabButtonGrid_UIGrid;

	public void Setup (Transform root)
	{
		NameSprite = root.Find("NameSprite").GetComponent<UISprite>();
		CloseButton = root.Find("BaseGroup/CloseButton").GetComponent<UIButton>();
		TabButtonGrid_UIGrid = root.Find("TabButtonGrid").GetComponent<UIGrid>();
	}
}
