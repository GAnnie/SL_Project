﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PetInfoWinUI.
/// </summary>
public class PetInfoWinUI : BaseView
{
	public UIButton CloseBtn;

	public void Setup (Transform root)
	{
		CloseBtn = root.Find("CloseBtn").GetComponent<UIButton>();
	}
}
