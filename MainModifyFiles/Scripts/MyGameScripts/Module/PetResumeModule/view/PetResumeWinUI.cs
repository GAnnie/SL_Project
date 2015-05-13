﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PetResumeWinUI.
/// </summary>
public class PetResumeWinUI : BaseView
{
	public UIGrid PetGrid;
	public UIButton CloseBtn;
	public GameObject CostBtnPos;
	public Transform LGroup;

	public void Setup (Transform root)
	{
		PetGrid = root.Find("RGroup/PetScrollView/Grid").GetComponent<UIGrid>();
		CloseBtn = root.Find("bgGroup/CloseBtn").GetComponent<UIButton>();
		CostBtnPos = root.Find("RGroup/CostBtnPos").gameObject;
		LGroup = root.Find("LGroup");
	}
}