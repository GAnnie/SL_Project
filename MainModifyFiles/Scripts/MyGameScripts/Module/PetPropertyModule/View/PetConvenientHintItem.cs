﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PetConvenientHintItem.
/// </summary>
public class PetConvenientHintItem : BaseView
{
	public UIEventTrigger eventTrigger;
	public UILabel hintLbl;

	public void Setup (Transform root)
	{
	eventTrigger = root.GetComponent<UIEventTrigger>();
	hintLbl = root.GetComponent<UILabel>();
	}
}