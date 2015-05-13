// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ServiceProviderManager.cs
// Author   : SK
// Created  : 2013/8/27
// Purpose  : 

// Modifi   : Whale
// Change   : 2014/04/25
// Purpose  : 这是一个管理，实例化一个IServiceProvider接口，并对它进行管理
// Content  : 添加内容：各个开关控制方法(初始化、登录、登出、支付等)
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LITJson;

public class ServiceProviderManager
{
    //获取角色id
    static public long GetRoleId()
    {
        return PlayerModel.Instance.GetPlayerId();
    }

    //获取角色名
    static public string GetRoleName()
    {
        return PlayerModel.Instance.GetPlayer().nickname;
    }

    //获取角色等级
    static public int GetRoleGrade()
    {
        return PlayerModel.Instance.GetPlayerLevel();
    }

    //获取服务器ID
    static public int GetServerId()
    {
        return ServerManager.Instance.GetServerInfo().serviceId;
    }

    //获取服务器名称
    static public string GetServerName()
    {
        return ServerManager.Instance.GetServerInfo().name;
    }

    static private Dictionary<string, string> jsonDics = new Dictionary<string, string>();
    
    //请求版本信息 
//    static public void RequestClientversion(System.Action<VersionCheckData> downLoadFinishCallBack)
//    {
//        RequestChannelJson(Version.GetComeFrom(), "clientversion", delegate(Dictionary<string, object> obj)
//        {
//            JsonVersionCheckDataParser parser = new JsonVersionCheckDataParser();
//            VersionCheckData data = parser.DeserializeJson_VersionCheckData(obj);
//            downLoadFinishCallBack(data);
//        },false);
//    }

	static public void RequestSdkAccountLogin(string deviceId, int appId, int cpId, System.Action<AccountResponse> downLoadFinishCallBack)
    {
		string url = GameInfo.SDK_SERVER + "/sdkc/account/login.json?deviceId={0}&appId={1}&cpId={2}";
		url = string.Format (url, deviceId, appId, cpId);

		RequestJson(url, "SdkAccountLogin", delegate(string json)
		{
			AccountResponse data = (AccountResponse)JsonMapper.ToObject<AccountResponse>(json);
			downLoadFinishCallBack(data);
		}, true,true);
    }

	static public void RequestSsoAccountLogin(string sid, string channel, string subChannel, string loginWay, System.Action<LoginAccountDto> downLoadFinishCallBack)
	{
		string url = GameInfo.SSO_SERVER + "/gssoc/account/login.json?sid={0}&channel={1}&subChannel={2}&loginWay={3}";
		url = string.Format (url, sid, channel, subChannel, loginWay);
		
		RequestJson(url, "SsoAccountLogin", delegate(string json)
		            {
			LoginAccountDto data = (LoginAccountDto)JsonMapper.ToObject<LoginAccountDto>(json);
			downLoadFinishCallBack(data);
		}, true,true);
	}

    //服务器界面显示角色数据
    public static System.Action<ServerPlayerMessageList> _serverPlayerMessageCallBack = null;
    public static void SetServerPlayerMessageCallBack(System.Action<ServerPlayerMessageList> downLoadFinishCallBack)
    {
        _serverPlayerMessageCallBack = downLoadFinishCallBack;
    }

    public static void RemoveServerPlayerMessageCallBack()
    {
        _serverPlayerMessageCallBack = null;
    }

    //请求服务器列表
//    static public void RequestServerList(System.Action<ServerMessageList> downLoadFinishCallBack)
//    {
//        if (Version.ClientMode == AgencyPlatform.ClientMode_gmtest)
//        {
//            RequestGMTestChannelJson(Version.GetComeFrom(), "gameservers", delegate(Dictionary<string, object> obj)
//            {
//                JsonServerMessageListParser parser = new JsonServerMessageListParser();
//                ServerMessageList list = parser.DeserializeJson_ServerMessageList(obj);
//                downLoadFinishCallBack(list);
//            });
//        }
//        else
//        {
//            RequestChannelJson(Version.GetComeFrom(), "gameservers", delegate(Dictionary<string, object> obj)
//            {
//                JsonServerMessageListParser parser = new JsonServerMessageListParser();
//                ServerMessageList list = parser.DeserializeJson_ServerMessageList(obj);
//                downLoadFinishCallBack(list);
//            },true,true);
//        }
//    }
	
    //请求渠道json
    static private void RequestChannelJson(string spname, string jsonName, System.Action<Dictionary<string, object>> downLoadFinishCallBack, bool needLock = true,bool refresh = false)
    {

#if UNITY_APPSTORE
		string url = string.Format("http://play.baoyugame.com/ts/ios-channel/{0}/{1}?t={2}", spname, jsonName , DateTime.Now.Ticks );
		// string url = string.Format("http://play.baoyugame.com/ts/ios-channel/appstore/{0}?t={1}", jsonName , DateTime.Now.Ticks );
#else
        string url = string.Format(GameInfo.CHANNEL_ROOT + "/{0}/{1}?t={2}", spname, jsonName, DateTime.Now.Ticks);
#endif
       // RequestJson(url, jsonName, downLoadFinishCallBack, needLock,refresh);
    }

    //请求json

    static private void RequestJson(string url, string jsonName, System.Action<string> downLoadFinishCallBack, bool needLock = true,bool refresh = false)
    {
        if (!refresh && jsonDics.ContainsKey(jsonName))
        {
            string json = jsonDics[jsonName];
			downLoadFinishCallBack(json);
            return;
        }

        GameDebuger.Log("RequestJson " + url);


        if (needLock)
        {
            RequestLoadingTip.Show("", false);
        }

        HttpController.Instance.DownLoad(url, delegate(Byte[] bytes)
        {

            if (needLock)
            {
                RequestLoadingTip.Stop();
            }

			string json = System.Text.UTF8Encoding.UTF8.GetString (bytes);

			jsonDics[jsonName] = json;
			downLoadFinishCallBack(json);
        }, null, delegate(Exception obj)
        {

            if (needLock)
            {
                RequestLoadingTip.Stop();
            }

            downLoadFinishCallBack(null);
        }, false, SimpleWWW.ConnectionType.Short_Connect);
    }
}
