// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GameInfo.cs
// Author   : SK
// Created  : 2013/11/30
// Purpose  : 游戏信息配置
// **********************************************************************
using UnityEngine;
using System.Collections;


public class GameInfo
{
	public static int AppId = 1;
	public static int CpId = 1;
	public static string Channel = "nucleus";
	public static string SubChannel = "nucleus";
	public static string LoginWay = "nucleus";

	public static string SDK_SERVER = "http://192.168.1.97:9993";
	public static string SSO_SERVER = "http://192.168.1.97:9999";

	//public static string SDK_SERVER = "http://t1.baoyugame.com/h1";
	//public static string SSO_SERVER = "http://t1.baoyugame.com/h1";
//#if GAME_YDMM
//	public static string HA_SERVICE_MAIN_TYPE = "com/baoyugame/xy";
//	
//	//获取服务器的用户角色信息
//	static public string SERVICE_ROOT = "http://s.mm.baoyugame.com";	
//	
//	//游戏渠道根Url
//	static public string CHANNEL_ROOT = "http://play.baoyugame.com/mm/channel";
//
//#elif GAME_KDXJ
//	public static string HA_SERVICE_MAIN_TYPE = "com/baoyugame/xy";
//	
//	//全局服务根Url
//	static public string SERVICE_ROOT = "http://s.xj.baoyugame.com";	
//	
//	//游戏渠道根Url
//	static public string CHANNEL_ROOT = "http://play.baoyugame.com/xj/channel";
//
//#else
	public static string HA_SERVICE_MAIN_TYPE = "com/baoyugame/core4h1";
	
	//全局服务根Url
	static public string SERVICE_ROOT = "http://s.tianshu9.com";
	
	//游戏渠道根Url
	#if UNITY_DEBUG
//	static public string CHANNEL_ROOT = "http://192.168.1.97/ts/channel";
	static public string CHANNEL_ROOT = "http://192.168.1.106/";
	
	#else	
	//static public string CHANNEL_ROOT = "http://play.baoyugame.com/ts/channel";
	static public string CHANNEL_ROOT = "http://113.107.89.8";
	#endif

//#endif

	//获取服务器的用户角色信息
	static public string GET_PLAYERS_URL = SERVICE_ROOT + "/gsc/player/players.json";	

	//获取游戏公告信息
	static public string CHANNEL_global_announcements = CHANNEL_ROOT + "/global_announcements";
	
	//获取SNS信息
	static public string CHANNEL_social = CHANNEL_ROOT + "/social";
	
	//服务器列表
	static public string CHANNEL_gameservers = CHANNEL_ROOT + "/inner/gameservers";	
	
	//获取登陆图片信息
	static public string CHANNEL_BACKGROUND_PIC = CHANNEL_ROOT + "/progress_background";	
	//登陆图片地址前缀
	static public string CHANNEL_BACKGROUND_PIC_ROOT = CHANNEL_ROOT + "/progress_image/";
	//游戏活动宣传图片地址前缀
	static public string CHANNEL_ACTIVITYTIPS_PIC_ROOT = CHANNEL_ROOT + "/promotion_image/";
	//获取游戏名
	static public string GameName
	{
		get
		{
			#if UNITY_ZHIDIAN3G
				return "口袋仙剑";
			#elif UNITY_SHOUMENG360
				return "仙剑外传";
			#elif UNITY_QH360KD
				return "口袋轩辕剑";
			#else
				return "天书九卷";
			#endif
		}
	}
}

