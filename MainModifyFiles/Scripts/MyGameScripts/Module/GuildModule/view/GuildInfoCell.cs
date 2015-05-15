//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for GuildInfoCell.
/// </summary>
public class GuildInfoCell : BaseView
{
	public UILabel IdLabel;
	public UILabel NameLabel;
	public UILabel LvLabel;
	public UILabel CountLabel;
	public UILabel BossLabel;
	public UISprite SelectSprite;

	public void Setup (Transform root)
	{
		IdLabel = root.Find("IdLabel").GetComponent<UILabel>();
		NameLabel = root.Find("NameLabel").GetComponent<UILabel>();
		LvLabel = root.Find("LvLabel").GetComponent<UILabel>();
		CountLabel = root.Find("CountLabel").GetComponent<UILabel>();
		BossLabel = root.Find("BossLabel").GetComponent<UILabel>();
		SelectSprite = root.Find("SelectSprite").GetComponent<UISprite>();
	}
}
