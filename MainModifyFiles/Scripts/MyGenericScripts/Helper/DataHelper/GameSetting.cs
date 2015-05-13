// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  GameSetting.cs
// Author   : wenlin
// Created  : 2013/5/6 15:21:22
// Purpose  : 
// **********************************************************************

using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameSetting
{
	private static string _httpResPath = "";
	private static string _httpStaticPath = "";	
	
	// 0 : load local assets
	// 1 : load remote assets
	// 2 : load local unity3d assets
	// 3 : Experience Pack 
	private static int _loadMode = 0;
	private static bool _collectDebug = false;
	public  static bool IsLoadLocalAssets()         { return (_loadMode == 0); }
	public  static bool IsLoadLocalU3DAssets()      { return (_loadMode == 2); }  
	
	public  static bool IsCollectDebugInformation() { return _collectDebug; }
	
	private static bool _mobileModel = false;
	public static bool mobileModel()                { return _mobileModel; }
	
	private static bool _isLocalVersionDateNotExist = false;
	
	public static bool IsUpdateLocalPackage() 
	{
		GameDebuger.Log("_loadMode=" + _loadMode);
		#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
		return (IsLoadLocalU3DAssets() ) && ((!PlayerPrefs.HasKey("GameAsset" + Version.ver) || _isLocalVersionDateNotExist));
		#else
		return true;
		#endif
	}
	
	public static void SetUpdateLocalPackageFlag()
	{
		#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
		if (GameSetting.IsLoadLocalU3DAssets() )
		{
			if (!PlayerPrefs.HasKey("GameAsset" + Version.ver))
			{
				PlayerPrefs.SetInt("GameAsset" + Version.ver, 1);
				PlayerPrefs.Save();
			}
			
			_isLocalVersionDateNotExist = false;
		}
		#endif
	}
	
	public static string httpResPath { get { return _httpResPath; } }
	
	public static string httpStaticPath { get { return _httpStaticPath; } }
	
	
	private static string dataPath = "Setting/GameSettingData";
	public static void Setup()
	{
		TextAsset asset = Resources.Load( dataPath ) as TextAsset;
		if (asset != null)
		{
			//GameSettingData configInfo = Utility.LoadObjectFromBytes<GameSettingData>(asset.bytes);
			
			Dictionary<string, object> obj = DataHelper.GetJsonFile(asset.bytes, false);
			JsonGameSettingDataParser parser = new JsonGameSettingDataParser();
			GameSettingData configInfo = parser.DeserializeJson_GameSettingData(obj);
			
			if (configInfo != null)
			{
				_httpResPath = configInfo.resPath;
				_httpStaticPath = configInfo.httpPath;
				_loadMode = configInfo.loadMode;
				_collectDebug = configInfo.collectDebugInfo;
				_mobileModel = configInfo.mobileMode;
				Version.ClientMode = configInfo.clientMode;
				Version.Release = configInfo.release;
				Version._androidTestMode = configInfo.androidSPTestMode;
				
				GameDebuger.Log(" GameSettingData loadMode="+_loadMode);
			}
			else
			{
				GameDebuger.Log(" GameSettingData Setup Error !!");
			}
		}
		
		//判断本地版本时间是否存在
		#if UNITY_STANDALONE
		string localVersionPath = Application.persistentDataPath + "STANDALONE" + "/" + AssetsLoader._localAssetVerCtrl;
		#else
		string localVersionPath = Application.persistentDataPath + "/" + AssetsLoader._localAssetVerCtrl;
		#endif
		
		_isLocalVersionDateNotExist = !File.Exists(localVersionPath);
		
		//如果本地的版本资源不存在， 则删除Unity本地信息
		if (_isLocalVersionDateNotExist && PlayerPrefs.HasKey("GameAsset" + Version.ver))
		{
			PlayerPrefs.DeleteKey("GameAsset" + Version.ver);
			PlayerPrefs.Save();
		}
	}
}


public class GameSettingData
{
	public string resPath = "";
	public string httpPath = "";
	
	public int loadMode = 0;
	public bool mobileMode = false;
	public int clientMode = 0;
	public int gameType = 0;
	
	public bool unityDebugMode = false;
	public bool unityDebugLogMode = false;
	
	public bool release = false;
	public bool collectDebugInfo = false;
	
	public bool androidSPTestMode = false;
	
	public GameSettingData()
	{
		
	}
}