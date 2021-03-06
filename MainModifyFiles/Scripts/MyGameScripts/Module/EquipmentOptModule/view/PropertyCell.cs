﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PropertyCell.
/// </summary>
public class PropertyCell : BaseView
{
	public UILabel PropertyLabel;
	public UISprite SelectSprite;

	public void Setup (Transform root)
	{
		PropertyLabel = root.Find("PropertyLabel").GetComponent<UILabel>();
		SelectSprite = root.Find("SelectSprite").GetComponent<UISprite>();
	}
}
