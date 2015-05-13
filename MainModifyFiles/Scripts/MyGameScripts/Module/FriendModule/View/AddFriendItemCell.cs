//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for AddFriendItemCell.
/// </summary>
public class AddFriendItemCell : BaseView
{
	public UIButton addBtn;
	public UISprite addBtnSprite;
	public UISprite icon;
	public UILabel PositionLbl;
	public UILabel LvLbl;
	public UILabel NameLbl;

	public void Setup (Transform root)
	{
		addBtn = root.Find("ContentFrame/addBtn").GetComponent<UIButton>();
		addBtnSprite = root.Find("ContentFrame/addBtn/Sprite").GetComponent<UISprite>();
		icon = root.Find("ContentFrame/Face/icon").GetComponent<UISprite>();
		PositionLbl = root.Find("ContentFrame/PositionLbl").GetComponent<UILabel>();
		LvLbl = root.Find("ContentFrame/LvLbl").GetComponent<UILabel>();
		NameLbl = root.Find("ContentFrame/NameLbl").GetComponent<UILabel>();
	}
}
