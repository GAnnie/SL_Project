﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PetInfoCell.
/// </summary>
public class PetInfoCell : BaseView
{
	public UISprite iconFlagSprite;
	public UISprite iconSprite;
	public UILabel NameLbl;
	public UILabel LvLbl;
	public UISprite SelectSprite;
	public UIButton PetInfoCellBtn;
	public UIEventTrigger iconSpriteEventTrigger;

	public void Setup (Transform root)
	{
		iconFlagSprite = root.Find("iconBg/iconFlag").GetComponent<UISprite>();
		iconSprite = root.Find("iconBg/iconSprite").GetComponent<UISprite>();
		NameLbl = root.Find("NameLbl").GetComponent<UILabel>();
		LvLbl = root.Find("LvLbl").GetComponent<UILabel>();
	SelectSprite = root.GetComponent<UISprite>();
	PetInfoCellBtn = root.GetComponent<UIButton>();
		iconSpriteEventTrigger = root.Find("iconBg/iconSprite").GetComponent<UIEventTrigger>();
	}
}