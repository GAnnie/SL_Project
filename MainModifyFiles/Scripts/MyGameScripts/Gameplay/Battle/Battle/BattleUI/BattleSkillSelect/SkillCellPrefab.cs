﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for SkillCellPrefab.
/// </summary>
public class SkillCellPrefab : BaseView
{
	public UIButton SkillCellPrefab_UIButton;
	public UILabel NameLabel_UILabel;
	public UILabel TypeLabel_UILabel;
	public UISprite IconSprite_UISprite;

	public void Setup (Transform root)
	{
	SkillCellPrefab_UIButton = root.GetComponent<UIButton>();
		NameLabel_UILabel = root.Find("NameLabel").GetComponent<UILabel>();
		TypeLabel_UILabel = root.Find("TypeLabel").GetComponent<UILabel>();
		IconSprite_UISprite = root.Find("IconGroup/IconSprite").GetComponent<UISprite>();
	}
}
