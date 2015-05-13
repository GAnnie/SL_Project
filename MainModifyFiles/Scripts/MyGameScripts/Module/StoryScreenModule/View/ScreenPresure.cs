//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ScreenPresure.
/// </summary>
public class ScreenPresure : BaseView
{
	public UISprite Top;
	public UISprite Bottom;
	public GameObject Content;
	public UIButton EndBtn;

	public void Setup (Transform root)
	{
		Top = root.Find("Top").GetComponent<UISprite>();
		Bottom = root.Find("Bottom").GetComponent<UISprite>();
		Content = root.Find("Top/Content").gameObject;
		EndBtn = root.Find("Top/Content/EndBtn").GetComponent<UIButton>();
	}
}