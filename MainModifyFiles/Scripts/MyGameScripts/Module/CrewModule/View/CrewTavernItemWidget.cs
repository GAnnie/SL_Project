﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for CrewTavernItemWidget.
/// </summary>
public class CrewTavernItemWidget : BaseView
{
	public Transform modelAnchor;
	public UILabel nameLbl;
	public UILabel factionLbl;
	public UIButton hintBtn;
	public UILabel detailLbl;
	public UIButton buyBtn;
	public UISprite btnSprite;

	public void Setup (Transform root)
	{
		modelAnchor = root.Find("ModelAnchor");
		nameLbl = root.Find("ModelAnchor/nameLbl").GetComponent<UILabel>();
		factionLbl = root.Find("ModelAnchor/factionLbl").GetComponent<UILabel>();
		hintBtn = root.Find("ModelAnchor/factionLbl/hintBtn").GetComponent<UIButton>();
		detailLbl = root.Find("detailLbl").GetComponent<UILabel>();
		buyBtn = root.Find("Button").GetComponent<UIButton>();
		btnSprite = root.Find("Button").GetComponent<UISprite>();
	}
}
