﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for RewardItemCell.
/// </summary>
public class RewardItemCell : BaseView
{
	public UISprite SelectSprite;
	public UISprite IconSprite;
	public UILabel CountLabel;

	public void Setup (Transform root)
	{
		SelectSprite = root.Find("SelectSprite").GetComponent<UISprite>();
		IconSprite = root.Find("IconSprite").GetComponent<UISprite>();
		CountLabel = root.Find("CountLabel").GetComponent<UILabel>();
	}
}
