﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for SubSkillCell.
/// </summary>
public class SubSkillCell : BaseView
{
	public UIButton SubSkillCellBtn;
	public UISprite IconSprite;
	public UILabel NameLabel;

	public void Setup (Transform root)
	{
	SubSkillCellBtn = root.GetComponent<UIButton>();
		IconSprite = root.Find("IconSprite").GetComponent<UISprite>();
		NameLabel = root.Find("NameLabel").GetComponent<UILabel>();
	}
}
