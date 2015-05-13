﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ServerListView.
/// </summary>
public class ServerListView : BaseView
{
	public UIButton CloseButton;
	public UITable LastLoginTable;
	public UITable ServerListTable;
	public ServerListItem CurServerListItem;

	public void Setup (Transform root)
	{
		CloseButton = root.Find("CloseButton").GetComponent<UIButton>();
		LastLoginTable = root.Find("ContentGroup/LastLoginTable").GetComponent<UITable>();
		ServerListTable = root.Find("ContentGroup/ServerDraggablePanel/ServerListTable").GetComponent<UITable>();
		CurServerListItem = root.Find("CurServerListItem").GetComponent<ServerListItem>();
	}
}