﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for NPCPetWarehouseView.
/// </summary>
public class NPCPetWarehouseView : BaseView
{
	public UIGrid BodyGrid;
	public UIGrid WarehouseGrid;
	public GameObject OptBtnPos;

	public void Setup (Transform root)
	{
		BodyGrid = root.Find("LGroup/BodyScrollView/BodyGrid").GetComponent<UIGrid>();
		WarehouseGrid = root.Find("RGroup/WarehouseScrollView/WarehouseGrid").GetComponent<UIGrid>();
		OptBtnPos = root.Find("OptBtnPos").gameObject;
	}
}
