//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for FactionCell.
/// </summary>
public class FactionCell : BaseView
{
	public GameObject FactionSelectSprite;
	public GameObject FactionDescSprite;
	public UILabel FactionDescLabel_UILabel;
	public UILabel FactionNameLabel_UILabel;
	public UISprite FactionIconSprite_UISprite;
	public UIButton FactionIconSprite_UIButton;

	public void Setup (Transform root)
	{
		FactionSelectSprite = root.Find("FactionSelectSprite").gameObject;
		FactionDescSprite = root.Find("FactionDescSprite").gameObject;
		FactionDescLabel_UILabel = root.Find("FactionDescSprite/FactionDescLabel").GetComponent<UILabel>();
		FactionNameLabel_UILabel = root.Find("FactionNameLabel").GetComponent<UILabel>();
		FactionIconSprite_UISprite = root.Find("FactionIconSprite").GetComponent<UISprite>();
		FactionIconSprite_UIButton = root.Find("FactionIconSprite").GetComponent<UIButton>();
	}
}