// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ServerManager.cs
// Author   : SK
// Created  : 2013/9/6
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//服务器管理器
using System;


public class ServerManager
{
	private static readonly ServerManager instance = new ServerManager();
    public static ServerManager Instance
    {
        get
		{
			return instance;
		}
    }	
	
	public delegate void OnRequestTokenDelegate(string token, string errorMsg);
	public OnRequestTokenDelegate OnRequestToken;		

	public delegate void OnRequestOrderIdDelegate(string orderId, string errorMsg);
	public OnRequestOrderIdDelegate OnRequestOrderId;		
	
	public static string gservice = "";
	
	private string _sid = "";
	
	private string _uid = "";
	
	private ServerInfo _serverInfo;

	public AccountSession accountSession;
	
	public LoginAccountDto loginAccountDto;

	public void SetServerInfo(ServerInfo info){
		_serverInfo = info;
	}
	
	public ServerInfo GetServerInfo(){
		return _serverInfo;
	}
	
	public void SetLoginInfo(string sid, string uid){
		_sid = sid;
		_uid = uid;
	}
	
	public string GetSid(){
		return _sid;
	}
	
	public string GetUid(){
		return _uid;
	}

	public AccountPlayerDto HasPlayerAtServer(int gameServerId)
	{
		if (loginAccountDto != null)
		{
			foreach(AccountPlayerDto dto in loginAccountDto.players)
			{
				if (dto.gameServerId == gameServerId)
				{
					return dto;
				}
			}
		}
		return null;
	}

	//请求gmtoken
	public void RequestGmToken(OnRequestTokenDelegate callback, string name, string password)
	{
		OnRequestToken = callback;
		DoRequestGmToken(name, password, _serverInfo.serviceId);
	}	
	
	public void DoRequestGmToken(string name, string password, int serviceId){
		string requestUrl = GetServerInfo().loginUrl;
		requestUrl += "?name="+name;
		requestUrl += "&password="+password;
		requestUrl += "&serviceId="+serviceId;
		
		GameDebuger.Log("DoRequestGmToken url=" + requestUrl);
		
		HttpController.Instance.DownLoadAnyThread(requestUrl, delegate(byte[] bytes) {
			string returnStr = System.Text.UTF8Encoding.UTF8.GetString(bytes);
			GameDebuger.Log("DoRequestGmToken returnStr=" + requestUrl);	
			
			Dictionary<string , object> json = DataHelper.GetJsonFile(bytes, false);
			if ( json.ContainsKey ( "token" ) )
			{
				string token = GetJsonValue(json, "token");
				OnRequestToken(token, null);
				OnRequestToken = null;
				GameDebuger.Log("token=" + token);
			}
			else
			{
				string msg = GetJsonValue(json, "msg");
				OnRequestToken(null, msg);
				OnRequestToken = null;
            }			
		},null,delegate(System.Exception obj) {
			OnRequestToken(null, " 网络请求失败");
			OnRequestToken = null;
			GameDebuger.Log("DoRequestGmToken Error :" + obj.ToString());
		}, false, SimpleWWW.ConnectionType.Short_Connect);		
	}
	
	//请求token
	public void RequestToken(OnRequestTokenDelegate callback)
	{
//		OnRequestToken = callback;
//		string comeFrom = ServiceProviderManager.GetComeFrom();
//		string origin = ServiceProviderManager.GetOrigin();
//		int serviceId = GetServerInfo().serviceId;
//		DoRequestToken(comeFrom, serviceId, _sid, _uid, origin);
	}	
	
    public void DoRequestToken(string comeFrom, int serviceId, string sid, string uid, string origin)
    {
		sid = WWW.EscapeURL(sid);
		
		string requestUrl = GetServerInfo().loginUrl;
		requestUrl += "?comeFrom="+comeFrom;
		requestUrl += "&origin="+origin;
		requestUrl += "&serviceId="+serviceId;
		requestUrl += "&sid="+sid;
		requestUrl += "&uid="+uid;
		
#if UNITY_PIPAW
		requestUrl += "&username="+PipawConfig.username;
		requestUrl += "&time="+PipawConfig.time;
#endif		

#if UNITY_HUAWEI
		requestUrl += "&nsp_ts="+DateTime.Now.Ticks;
#endif

		GameDebuger.Log("RequestToken url=" + requestUrl);
		
		HttpController.Instance.DownLoadAnyThread(requestUrl, delegate(byte[] bytes) {
			string returnStr = System.Text.UTF8Encoding.UTF8.GetString(bytes);
			GameDebuger.Log("RequestToken returnStr=" + requestUrl);	
			
			Dictionary<string , object> json = DataHelper.GetJsonFile(bytes, false);
			if ( json.ContainsKey ( "token" ) )
			{
				string token = GetJsonValue(json, "token");
				uid = GetJsonValue(json, "uid");
				SetLoginInfo(sid, uid);
				OnRequestToken(token, null);
				OnRequestToken = null;
				GameDebuger.Log("token=" + token);
			}
			else
			{
				string msg = GetJsonValue(json, "msg");
				OnRequestToken(null, msg);
				OnRequestToken = null;
            }			
		},null,delegate(System.Exception obj) {
			OnRequestToken(null, " 网络请求失败");
			OnRequestToken = null;
			GameDebuger.Log("DoRequestToken Error :" + obj.ToString());
		},false, SimpleWWW.ConnectionType.Short_Connect);
    }
	
	private string GetJsonValue(Dictionary<string , object> json, string key){
		if (json.ContainsKey(key)){
			return json[key] as string;
		}else{
			return "";
		}
	}
	
	//请求订单号
	public void RequestOrderId(OnRequestOrderIdDelegate callback)
	{
		if (GetServerInfo().payUrl == ""){
			callback("", null);
			return;
		}
		
		OnRequestOrderId = callback;	
		string comeFrom = "11";//ServiceProviderManager.GetComeFrom();
		int serviceId = GetServerInfo().serviceId;		
		
		string requestUrl = GetServerInfo().payUrl+"/order/id";
		requestUrl += "?token=11";//PlayerModel.Instance.GetToken();
		requestUrl += "&comefrom="+comeFrom;
		requestUrl += "&serviceId="+serviceId;
		requestUrl += "&uid="+_uid;
		
		GameDebuger.Log("RequestOrderId url=" + requestUrl);
		
		HttpController.Instance.DownLoad(requestUrl, delegate(System.Byte[] bytes) {
			string returnStr = System.Text.UTF8Encoding.UTF8.GetString(bytes);
			GameDebuger.Log("RequestOrderJson returnStr=" + returnStr);	
			
			Dictionary<string , object> json = DataHelper.GetJsonFile(bytes, false);
			
			if ( json.ContainsKey ( "orderId" ) )
			{
				string orderId = GetJsonValue(json, "orderId");
				orderId = orderId.Trim();
				OnRequestOrderId(orderId, null);
				OnRequestOrderId = null;
			}else{
				string errorMsg = GetJsonValue(json, "errorMsg");
				OnRequestOrderId(null, errorMsg);
				OnRequestOrderId = null;				
			}
		},null,delegate(System.Exception obj) {
			OnRequestOrderId(null, "网络请求失败");
			OnRequestOrderId = null;
			GameDebuger.Log("RequestOrderId Error :" + obj.ToString());
		},false, SimpleWWW.ConnectionType.Short_Connect);
	}
}

