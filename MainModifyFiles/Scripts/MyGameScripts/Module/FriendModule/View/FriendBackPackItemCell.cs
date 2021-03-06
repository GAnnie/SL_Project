//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for FriendBackPackItemCell.
/// </summary>
public class FriendBackPackItemCell : BaseView
{
	public UILabel CountLabel;
	public UISprite IconSprite;
	public UISprite EquipmentSprite;
	public UIButton FriendBackPackItemCellBtn;
	public UISprite SelectEff;

	public void Setup (Transform root)
	{
		CountLabel = root.Find("CountLabel").GetComponent<UILabel>();
		IconSprite = root.Find("IconSprite").GetComponent<UISprite>();
		EquipmentSprite = root.Find("Sprite").GetComponent<UISprite>();
	FriendBackPackItemCellBtn = root.GetComponent<UIButton>();
		SelectEff = root.Find("SelectEff").GetComponent<UISprite>();
	}
}
