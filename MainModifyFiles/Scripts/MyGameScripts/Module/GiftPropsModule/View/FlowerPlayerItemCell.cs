//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for FlowerPlayerItemCell.
/// </summary>
public class FlowerPlayerItemCell : BaseView
{
	public UISprite Bg;
	public UISprite icon;
	public UILabel LVLbl;
	public UILabel NameLbl;
	public UILabel RelationShipLbl;
	public UISprite FactionIcon;
	public UIButton ClickBtn;

	public void Setup (Transform root)
	{
		Bg = root.Find("Content").GetComponent<UISprite>();
		icon = root.Find("Content/iconBg/icon").GetComponent<UISprite>();
		LVLbl = root.Find("Content/iconBg/LVLbl").GetComponent<UILabel>();
		NameLbl = root.Find("Content/NameLbl").GetComponent<UILabel>();
		RelationShipLbl = root.Find("Content/RelationShip").GetComponent<UILabel>();
		FactionIcon = root.Find("Content/FactionIcon").GetComponent<UISprite>();
	ClickBtn = root.GetComponent<UIButton>();
	}
}