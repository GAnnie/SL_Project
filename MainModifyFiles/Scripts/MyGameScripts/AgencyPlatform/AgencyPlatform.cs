using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgencyPlatform
{
	//测试模式
	public static int ClientMode_test = 0;

	//Release模式
	public static int ClientMode_release = 100;

	//appstore
	public static int ClientMode_appstore = 1;
	
	//  BaoyuSDK
	public static int ClientMode_baoyugame = 2;
	
	//  AnySDK
	public static int ClientMode_anysdk = 3;
	
	//  UCSDK
	public static int ClientMode_9game = 4;
	
	//  BaiduSDK
	public static int ClientMode_baidu = 5;
	
	//  10086SDK
	public static int ClientMode_10086 = 6;
	
	//  DownjoySDK
	public static int ClientMode_downjoy = 7;
	
	//  XiaomiSDK
	public static int ClientMode_xiaomi = 8;
	
	//MsdkSdk
	public static int ClientMode_tencent = 9;
	
	//SinaSdk
	public static int ClientMode_sina = 10;
	
	//LenovoSdk
	public static int ClientMode_lenovo = 11;
	
	//gmtest
	public static int ClientMode_gmtest = 99;
	
	//平台comefrom
	public static readonly Dictionary<int, string> agencyPlatforms = null;
	
	//平台发布目录
	public static readonly Dictionary<int, string> agencyPlatformBuildDirs = null;
	
	//平台发布后缀
	public static readonly Dictionary<int, string> agencyPlatformBuildSuffix = null;
	
	static AgencyPlatform()
	{
		//平台comefrom,平台资源目录名
		agencyPlatforms = new Dictionary<int, string>();
		agencyPlatforms.Add(ClientMode_test, "test");
//		agencyPlatforms.Add(ClientMode_baoyugame, "baoyugame");
//		agencyPlatforms.Add(ClientMode_anysdk, "anysdk");
//		agencyPlatforms.Add(ClientMode_9game, "9game");
//		agencyPlatforms.Add(ClientMode_baidu, "baidu");
//		agencyPlatforms.Add(ClientMode_10086, "10086");
//		agencyPlatforms.Add(ClientMode_downjoy, "downjoy");
//		agencyPlatforms.Add(ClientMode_xiaomi, "xiaomi");
//		agencyPlatforms.Add(ClientMode_tencent,"tencent");
//		agencyPlatforms.Add (ClientMode_sina, "sina");
//		agencyPlatforms.Add (ClientMode_lenovo, "lenovo");
		
		//打包放置目录
		agencyPlatformBuildDirs = new Dictionary<int, string>();
		agencyPlatformBuildDirs.Add(ClientMode_test, "test");
//		agencyPlatformBuildDirs.Add(ClientMode_baoyugame, "BaoyuGame");
//		agencyPlatformBuildDirs.Add(ClientMode_anysdk, "AnySDK");
//		agencyPlatformBuildDirs.Add(ClientMode_9game, "UC");
//		agencyPlatformBuildDirs.Add(ClientMode_baidu, "Baidu");
//		agencyPlatformBuildDirs.Add(ClientMode_10086, "10086");
//		agencyPlatformBuildDirs.Add(ClientMode_downjoy, "Downjoy");
//		agencyPlatformBuildDirs.Add(ClientMode_xiaomi, "Xiaomi");
//		agencyPlatformBuildDirs.Add (ClientMode_tencent, "Tencent");
//		agencyPlatformBuildDirs.Add (ClientMode_sina, "Sina");
//		agencyPlatformBuildDirs.Add (ClientMode_lenovo, "Lenovo");
		
		//包名后缀,平台发布后缀,bundleIdentifier后缀
		agencyPlatformBuildSuffix = new Dictionary<int, string>();
		agencyPlatformBuildSuffix.Add(ClientMode_test, "dev");
//		agencyPlatformBuildSuffix.Add(ClientMode_baoyugame, "baoyu");
//		agencyPlatformBuildSuffix.Add(ClientMode_anysdk, "anysdk");
//		agencyPlatformBuildSuffix.Add(ClientMode_9game, "uc");
//		agencyPlatformBuildSuffix.Add(ClientMode_baidu, "baidu");
//		agencyPlatformBuildSuffix.Add(ClientMode_10086, "mm10086");
//		agencyPlatformBuildSuffix.Add(ClientMode_downjoy, "downjoy");
//		agencyPlatformBuildSuffix.Add(ClientMode_xiaomi, "xiaomi");
//		agencyPlatformBuildSuffix.Add(ClientMode_tencent,"tencent");
//		agencyPlatformBuildSuffix.Add (ClientMode_sina, "sina");
//		agencyPlatformBuildSuffix.Add (ClientMode_lenovo, "lenovo");
		#if UNITY_IPHONE
		//平台comefrom,平台资源目录名
		agencyPlatforms.Add(ClientMode_appstore, "appstore");
		
		//打包放置目录
		agencyPlatformBuildDirs.Add(ClientMode_appstore, "appstore");
		
		//包名后缀,平台发布后缀,bundleIdentifier后缀
		agencyPlatformBuildSuffix.Add(ClientMode_appstore, "appstore");
		#elif UNITY_ANDROID
		//平台comefrom,平台资源目录名
		
		//打包放置目录
		
		//包名后缀,平台发布后缀,bundleIdentifier后缀
		#endif
	}
	
	public static string NullComeFrom = "NULL";
	public static string GetComeFrom(int index)
	{
		if (agencyPlatforms != null && agencyPlatforms.ContainsKey(index))
		{
			return agencyPlatforms[index];
		}
		
		return NullComeFrom;
	}
	
	public static int GetComeFromIndex(string comeFrom)
	{
		if (agencyPlatforms != null)
		{
			foreach (KeyValuePair<int, string> item in agencyPlatforms)
			{
				if (item.Value == comeFrom)
				{
					return item.Key;
				}
			}
		}
		return 0;
	}
	
	public static string GetBuildDir(int index)
	{
		if (agencyPlatformBuildDirs != null && agencyPlatformBuildDirs.ContainsKey(index))
		{
			return agencyPlatformBuildDirs[index];
		}
		
		return NullComeFrom;
	}
	
	public static string GetBuildSuffix(int index)
	{
		if (agencyPlatformBuildSuffix != null && agencyPlatformBuildSuffix.ContainsKey(index))
		{
			return agencyPlatformBuildSuffix[index];
		}
		
		return NullComeFrom;
	}
}
