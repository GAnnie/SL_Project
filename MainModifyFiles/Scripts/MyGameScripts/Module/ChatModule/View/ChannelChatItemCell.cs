//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ChannelChatItemCell.
/// </summary>
public class ChannelChatItemCell : BaseView
{
	public UILabel contentLbl;
	public UIWidget ItemCellWidget;
	public UISprite ChannelBg;

	public void Setup (Transform root)
	{
		contentLbl = root.Find("content").GetComponent<UILabel>();
	ItemCellWidget = root.GetComponent<UIWidget>();
		ChannelBg = root.Find("ChannelBg").GetComponent<UISprite>();
	}
}
