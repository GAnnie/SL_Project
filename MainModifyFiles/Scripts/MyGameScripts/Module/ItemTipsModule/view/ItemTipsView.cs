﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ItemTipsView.
/// </summary>
public class ItemTipsView : BaseView
{
	public UISprite ContentBg;
	public UISprite IconSprite;
	public UILabel NameLabel;
	public UIAnchor OptBtnGroupAnchor;
	public GameObject RButtonPos;
	public GameObject LButtonPos;
	public UITable DecTable;
	public UIAnchor ItemTipsViewAnchor;
	public UISprite CirculationTypeSprite;

	public void Setup (Transform root)
	{
		ContentBg = root.Find("ContentBg").GetComponent<UISprite>();
		IconSprite = root.Find("ContentBg/TopGroup/IconSprite").GetComponent<UISprite>();
		NameLabel = root.Find("ContentBg/TopGroup/NameLabel").GetComponent<UILabel>();
		OptBtnGroupAnchor = root.Find("ContentBg/OptBtnGroup").GetComponent<UIAnchor>();
		RButtonPos = root.Find("ContentBg/OptBtnGroup/RButtonPos").gameObject;
		LButtonPos = root.Find("ContentBg/OptBtnGroup/LButtonPos").gameObject;
		DecTable = root.Find("ContentBg/DecTable").GetComponent<UITable>();
		ItemTipsViewAnchor = root.Find("ContentBg").GetComponent<UIAnchor>();
		CirculationTypeSprite = root.Find("ContentBg/TopGroup/CirculationTypeSprite").GetComponent<UISprite>();
	}
}
