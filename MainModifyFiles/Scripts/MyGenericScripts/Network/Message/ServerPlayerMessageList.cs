// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ServerPlayerMessageList.cs
// Author   : wenlin
// Created  : 2013/10/28 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;

/// <summary>
/// Server player message list.
/// </summary>
public class ServerPlayerMessageList
{
	public List< ServerPlayerMessage > players = null;
	
	public ServerPlayerMessageList ()
	{
	}
}



/// <summary>
/// Server player message.
/// </summary>
public class ServerPlayerMessage
{
	/**玩家编号*/
	public string id;
	
	/**昵称*/
	public string nickname;
	
	/**等级*/
	public int grade;
	
	/**性别 Gender*/
	public int gender;
	
	
	/** 服务器ID **/
	public int serviceId;
	
	public ServerPlayerMessage()
	{}
}

