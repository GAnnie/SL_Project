﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for SkillButtonCell.
/// </summary>
public class SkillButtonCell : BaseView
{
	public UISprite SkillSprite_UISprite;
	public GameObject SkillIconGroup;
	public UIButton SkillButtonCell_UIButton;
	public UILabel NameLabel_UILabel;
	public UISprite SkillIcon_UISprite;

	public void Setup (Transform root)
	{
		SkillSprite_UISprite = root.Find("SkillSprite").GetComponent<UISprite>();
		SkillIconGroup = root.Find("SkillIconGroup").gameObject;
	SkillButtonCell_UIButton = root.GetComponent<UIButton>();
		NameLabel_UILabel = root.Find("SkillIconGroup/NameLabel").GetComponent<UILabel>();
		SkillIcon_UISprite = root.Find("SkillIconGroup/SkillIcon").GetComponent<UISprite>();
	}
}