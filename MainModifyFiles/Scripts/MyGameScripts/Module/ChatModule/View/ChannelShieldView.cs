//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ChannelShieldView.
/// </summary>
public class ChannelShieldView : BaseView
{
	public UIButton ConfirmBtn;
	public UIButton CloseBtn;
	public UIToggle WorldCheckbtnToggle;
	public UIToggle GuildCheckbtnToggle;
	public UIToggle TeamCheckbtnToggle;

	public void Setup (Transform root)
	{
		ConfirmBtn = root.Find("Content/ConfirmBtn").GetComponent<UIButton>();
		CloseBtn = root.Find("Content/CloseBtn").GetComponent<UIButton>();
		WorldCheckbtnToggle = root.Find("Content/WorldCheckbtn").GetComponent<UIToggle>();
		GuildCheckbtnToggle = root.Find("Content/GuildCheckbtn").GetComponent<UIToggle>();
		TeamCheckbtnToggle = root.Find("Content/TeamCheckbtn").GetComponent<UIToggle>();
	}
}
