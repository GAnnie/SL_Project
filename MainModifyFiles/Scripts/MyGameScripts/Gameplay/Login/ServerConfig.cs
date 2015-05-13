// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  ServerConfig.cs
// Author   : senkay
// Created  : 4/8/2013 5:45:25 PM
// Purpose  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;

public class ServerConfig
{
    public static List<ServerInfo> serverList;

#if GAME_KDXJ
	public static string ApkServerPath = "http://apk.baoyugame.com/xj/";
#else
    public static string ApkServerPath = "http://apk.baoyugame.com/ts/";
#endif


    public delegate void CallbackDelegate();

    public static event Action onServerListReturn;
	public static event Action onServerListRefresh;
    

    public static void Setup()
    {
		if (serverList ==  null)
		{
			serverList = new List<ServerInfo>();
		}
		else
		{
			serverList.Clear();
		}
		RefreshList(CompositionServerMessageList);
    }

    public static void Refresh()
    {		
		RefreshList(CompositionServerMessageList);
    }
	
	public static void RefreshList(System.Action<KDTSServerMessageList> downLoadFinishCallBack)
    {
		if (Version.ClientMode == AgencyPlatform.ClientMode_test)
		{
			if( Version.Release )
				SetupServerMessageList(ReadServerListFromFile(PathHelper.SETTING_PATH + "ReleaseServerConfig"));
			else
				SetupServerMessageList(ReadServerListFromFile(PathHelper.SETTING_PATH + "LocalServerConfig"));
		}
		else
		{
			//ServiceProviderManager.RequestServerList(downLoadFinishCallBack);
		}	
    }	
	
    private static ServerMessageList ReadServerListFromFile(string path_)
    {
        TextAsset textAsset = ResourceLoader.Load(path_, "txt") as TextAsset;

        if (textAsset != null)
        {

            Dictionary<string, object> obj = DataHelper.GetJsonFile(textAsset.bytes, false);
            JsonServerMessageListParser parser = new JsonServerMessageListParser();
            return parser.DeserializeJson_ServerMessageList(obj);
        }

        return null;

    }

    private static void SetupServerMessageList(ServerMessageList list)
    {
        if (list == null)
        {
            GameDebuger.Log("ServerMessageList is null ");
            return;
        }

        ServerManager.gservice = list.gservice;

        serverList.AddRange(list.list);

        if (onServerListReturn != null)
        {
            onServerListReturn();
        }

        //for (int i = 0; i < list.servers.Count; i++)
        //{
        //    ServerInfo info = list.servers[i];
        //    AddServerInfo( info.serverName, info
        //}
    }

	private static void CompositionServerMessageList( KDTSServerMessageList list )
	{
		ServerMessageList serverMessageList = new ServerMessageList();

		serverMessageList.gservice 	= list.gservice;
		serverMessageList.list 		= new List<ServerInfo>();

		string targetServiceId = "[targetServiceId]";

		//组装服务器列表
		for( int i = 0 ; i < list.list.Count ; i++ )
		{
			KDTSServerInfo info = list.list[i];
			ServerInfo serverInfo = new ServerInfo();

			serverInfo.host 	= list.host.Replace( targetServiceId,  info.targetServiceId.ToString() );
			serverInfo.loginUrl = list.loginUrl.Replace( targetServiceId,  info.targetServiceId.ToString() );

			if( info.needPayUrl )
			{
				serverInfo.payUrl = list.payUrl.Replace( targetServiceId,  info.targetServiceId.ToString() );
			}

			serverInfo.port 		= info.port;
			serverInfo.serviceId 	= info.serviceId;
			serverInfo.accessId		= info.accessId;
			serverInfo.name			= info.name;
			serverInfo.runState		= info.runState;
			serverInfo.dboState		= info.dboState;
			serverInfo.recommend	= info.recommend;
			serverInfo.limitVer		= info.limitVer;
			serverInfo.limitMaxVer	= info.limitMaxVer;

			serverMessageList.list.Add( serverInfo );
		}


		SetupServerMessageList( serverMessageList );
	}

    private static void UpdateServerMessageList(ServerMessageList list)
    {
        if (list == null)
        {
            GameDebuger.Log("ServerMessageList is null ");
            return;
        }

        //先清空列表信息
        serverList.Clear();

        ServerManager.gservice = list.gservice;

        serverList.AddRange(list.list);

        if (onServerListRefresh != null)
        {
            onServerListRefresh();
        }

    }

    private static void AddServerInfo(string name, string ip, int port, string serverId)
    {
        ServerInfo info = new ServerInfo();
        info.name = name;
        info.host = ip;
        info.port = port;
        info.serviceId = int.Parse(serverId);
        serverList.Add(info);
    }


	/**
	 * name的格式是  host + "|" + accessId + "|" + serviceId   192.168.1.90|65537|30
	 */ 
    public static ServerInfo GetServerInfo(string name)
    {
		string[] splits = name.Split('|');
		
        foreach (ServerInfo info in serverList)
        {
			if (splits.Length == 2){
				if (info.host == splits[0] && info.accessId == int.Parse(splits[1]))
				{
					return info;
				}
			}else if (splits.Length == 3){
				if (info.host == splits[0] && info.accessId == int.Parse(splits[1]) && info.serviceId == int.Parse(splits[2]))
				{
					return info;
				}
			}
        }
        return null;
    }

    public static void SortServerList()
    {
        serverList.Sort(SortByID);
    }

    static int SortByID(ServerInfo a, ServerInfo b)
    {
        return a.serviceId - b.serviceId;
    }

    public static string GetServerListInfo()
    {
        string tmp = "";
        foreach (ServerInfo info in serverList)
        {
            tmp += info.serviceId + ",";
        }
        return tmp;
    }
}
