﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for SkillLearningWinUI.
/// </summary>
public class SkillLearningWinUI : BaseView
{
	public UIButton CloseBtn;
	public UIGrid TabGroup;
	public GameObject Pos;

	public void Setup (Transform root)
	{
		CloseBtn = root.Find("CloseBtn").GetComponent<UIButton>();
		TabGroup = root.Find("TabGroup").GetComponent<UIGrid>();
		Pos = root.Find("Pos").gameObject;
	}
}
