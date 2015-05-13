// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  ServerInfo.cs
// Author   : senkay
// Created  : 4/8/2013 5:47:24 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;

public class ServerInfo
{		
	
    //服务器名称
    public string name;	
	
	//HA 接入ID
	public int accessId;
	
	//游戏服编号
	public int serviceId;
	
    //服务器状态 0：流畅  1：火爆 2: 繁忙
    public int runState;

    //服务器维护状态 0：关闭   1:开放   2:维护中
    public int dboState = 1;
	
	//是否推荐  0不是新服 1新服
	public int recommend;

	//服务器id targetServiceId
	public int targetServiceId;
	
	//服务器端口
    public int port;
	
	//是否需要充值url
	public bool needPayUrl;	
	
	//版本限制
	public int limitVer = 0;
	
	//版本最大限制
	public int limitMaxVer = 0;	


	//------------------------------

	//gservice
	public string gservice;

	//host
	public string host;

	//登陆url
	public string loginUrl;

	//支付url
	public string payUrl;

	//------------------------------

	//获取服务器唯一id
	public string GetServerUID(){
		return host + "|" + accessId + "|" + serviceId;
	}

	//是否审核服务器
	public bool isTestServer()
	{
		return serviceId == 1000;
	}
}
