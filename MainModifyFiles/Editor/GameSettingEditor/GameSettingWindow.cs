// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  GameSettingWindow.cs
// Author   : wenlin
// Created  : 2013/5/6 20:15:32
// Purpose  : 
// **********************************************************************

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class GameSettingWindow : EditorWindow
{
    private static GameSettingWindow win = null;
    private static bool isExport = false;

    [MenuItem("Edit/Game Setting")]
    public static void ShowWin()
    {
        isExport = false;
        if( win == null )
		{
			win = (GameSettingWindow)EditorWindow.GetWindow(typeof(GameSettingWindow));
		}
		else
		{
			win.Show();
		}
		
		win.minSize = new Vector2(465, 562);
    }


    public delegate void SettingCallBack();
    private static SettingCallBack settingCallBackFunc = null;
    public static void ShowWinWithExport( SettingCallBack func)
    {
        isExport = true;
        settingCallBackFunc = func;
        EditorWindow.GetWindow(typeof(GameSettingWindow));
    }

	#if UNITY_IPHONE
	private static string ResourcesNetworkPath    = "http://play.baoyugame.com/ts/static/xxwq/ios/resources/";

	private static string _InternalNetworkPath    = "http://s.xy.baoyu.com/Assets/";
	private static string _ExternalNetworkPath    = "http://play.baoyugame.com/ts/static/xxwq/ios/release/";
	private static string _ExternalDevNetworkPath = "http://play.baoyugame.com/ts/static/xxwq/ios/dev_test2/";
	#else
	private static string ResourcesNetworkPath    = "http://play.baoyugame.com/ts/static/xxwq/android/resources/";

	private static string _InternalNetworkPath    = "http://s.xy.baoyu.com/Assets/";
	private static string _ExternalNetworkPath    = "http://play.baoyugame.com/ts/static/xxwq/android/release/";
	private static string _ExternalDevNetworkPath = "http://play.baoyugame.com/ts/static/xxwq/android/experience/";
	#endif
	
    private enum GameHttpPath
    {
        Internal_Network = 0,
        External_Network = 1,
		ExternalDev_Network = 2
    }
    private GameHttpPath gameHttpPath;
	
	#region GameLoadMode Mode
	/// <summary>
	/// 游戏加载
	/// Local_Assets ： 读取本地Unity资源
	/// Remote_Assets： 程序包 全部资源都要下载
	/// Local_Packet_Assets : 整包 全部资源都在包里面
	/// Local_Experience_Pack_Assets ： 体验包 部分体验的资源在包里面
	/// </summary>
    private enum GameLoadMode
    {
        Local_Assets        = 0,
        Remote_Assets       = 1,
        Local_Packet_Assets = 2,
		Local_Experience_Pack_Assets = 3
    }
	#endregion
	
	
    private GameLoadMode gameLoadMode = GameLoadMode.Local_Assets;
	
	
    private enum GameType
    {
        XXWQ       = 0
    }	
	private GameType gameType = GameType.XXWQ;

   // private ClientSetMode clientMode = ClientSetMode.ClientMode_Test;

    private int originPlatForm;
    private List<string> agencyPlatform = new List<string>();
	private int           clientModeSelectIndex      = 0;
    private int           clientMode      = 0;
    private bool          unityDebugMode = false;
	private bool          unityDebugLogMode = false;
    private bool          mobileModel     = false;
    private bool          isClientRelease = false;
    private bool          isCollectDebugInfo = false;
	private bool 		  androidSPTestMode  = false;

    //m_myItemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", m_myItemType );

    private GameSettingData data = null;
    private string gamesettingDataPath = Application.dataPath + "/Resources/Setting/" ;
    private string gamesettingJsonFile = "GameSettingData.txt";
    private void  GetGameSettingData()
    {
        foreach (KeyValuePair<int, string> item in AgencyPlatform.agencyPlatforms)
        {
            agencyPlatform.Add(item.Value);
        }

        if (File.Exists(gamesettingDataPath + gamesettingJsonFile))
        {
            Dictionary<string, object> obj = GameEditorUtils.GetJsonFile(gamesettingDataPath + gamesettingJsonFile, false);
            JsonGameSettingDataParser parser = new JsonGameSettingDataParser();
            data = parser.DeserializeJson_GameSettingData(obj);
			if (data == null){
				data = new GameSettingData();
			}
        }
        else
        {
            data = new GameSettingData();
        }
		
        if (_InternalNetworkPath == data.httpPath)      	gameHttpPath = GameHttpPath.Internal_Network;
        else if (_ExternalNetworkPath == data.httpPath) 	gameHttpPath = GameHttpPath.External_Network;
		else if (_ExternalDevNetworkPath == data.httpPath)  gameHttpPath = GameHttpPath.ExternalDev_Network;


        mobileModel  = data.mobileMode;
        gameLoadMode = (GameLoadMode)data.loadMode;
		gameType     = (GameType)data.gameType;
        isClientRelease = data.release;
        isCollectDebugInfo = data.collectDebugInfo;
        unityDebugMode    = data.unityDebugMode;
		unityDebugLogMode  = data.unityDebugLogMode;
        originPlatForm = data.clientMode;
		clientMode = data.clientMode;
		androidSPTestMode = data.androidSPTestMode;
		string comefrom = AgencyPlatform.GetComeFrom(clientMode);
        if (agencyPlatform.Contains(comefrom))
        {
            for (int i = 0; i < agencyPlatform.Count; i ++ )
            {
                if (agencyPlatform[i] == comefrom)
                {
					clientModeSelectIndex = i;
                    break;
                }
            }
        }
        //SaveGameSettingData();
    }

    private void SaveGameSettingData()
    {
        if (data != null)
        {
            CheckAndCreateDirectory(gamesettingDataPath);
            
            //Utility.SaveObjectAsXML<GameSettingData>(gamesettingDataPath + gamesettingName, data);

			data.resPath = ResourcesNetworkPath;

            JsonGameSettingDataParser parser = new JsonGameSettingDataParser();
            Dictionary<string, object> obj = parser.SerializeJson_GameSettingData(data);
            if (obj != null)
            {
                GameEditorUtils.SaveJsonFile(obj, gamesettingDataPath + gamesettingJsonFile, false);
            }
        }

        AssetDatabase.Refresh();
    }

    private void CheckAndCreateDirectory(string fullpath)
    {
        if (Directory.Exists(fullpath)) return;

        Directory.CreateDirectory(fullpath);
    }


    private void OnGUI()
    {
        if (data == null)
            GetGameSettingData();

		bool buildCurrentClientMode = false;
		bool buildAllClientMode = false;
		
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
            if ( data != null )
            {
                EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Bundle Identifier ： " + PlayerSettings.bundleIdentifier);
				EditorGUILayout.LabelField( "Bundle Version ： " + PlayerSettings.bundleVersion);
				EditorGUILayout.LabelField( "Bundle Version Code： " + PlayerSettings.Android.bundleVersionCode);
				EditorGUILayout.Space();

                //data.httpPath = EditorGUILayout.TextField("HttpPath : ", data.httpPath);
                gameHttpPath = (GameHttpPath)EditorGUILayout.EnumPopup("Http Path :", gameHttpPath);
			
				switch(gameHttpPath){
					case GameHttpPath.Internal_Network:
						data.httpPath = _InternalNetworkPath;
						break;
					case GameHttpPath.External_Network:
						data.httpPath = _ExternalNetworkPath;
						break;				
					case GameHttpPath.ExternalDev_Network:
						data.httpPath = _ExternalDevNetworkPath;
						break;				
				}

                EditorGUILayout.Space();
			
				EditorGUILayout.LabelField( "Local_Assets ： 读取本地Unity资源" );
				EditorGUILayout.LabelField( "Remote_Assets： 程序包 全部资源都要下载" );
				EditorGUILayout.LabelField( "Local_Packet_Assets : 整包 全部资源都在包里面" );
				EditorGUILayout.LabelField( "Local_Experience_Pack_Assets ： 体验包 部分体验的资源在包里面" );
			
                gameLoadMode = (GameLoadMode)EditorGUILayout.EnumPopup("Load Mode :", gameLoadMode);
                data.loadMode = (int)gameLoadMode;
                //AssetBundleEditor.AssetBundleExport.Instance.data = null;

                if (gameLoadMode == GameLoadMode.Remote_Assets)
                {
                    isCollectDebugInfo = true;
                    //_DeleteStreamPath();
                }
                else if (gameLoadMode == GameLoadMode.Local_Assets)
                {
                    isCollectDebugInfo = false;
                }
			
				EditorGUILayout.Space();
			
                gameType = (GameType)EditorGUILayout.EnumPopup("Game Type :", gameType);
                data.gameType = (int)gameType;			
			
                EditorGUILayout.Space();
                mobileModel = EditorGUILayout.Toggle("Mobile Model : ", mobileModel);
                data.mobileMode = mobileModel;
			
				switch(gameHttpPath){
					case GameHttpPath.Internal_Network:
						mobileModel = false;
						break;
				}
 

                EditorGUILayout.Space();
				int select = EditorGUILayout.Popup("AgencyPlatform : ", clientModeSelectIndex, agencyPlatform.ToArray());
				if (clientModeSelectIndex != select)
				{
					clientModeSelectIndex = select;
					clientMode = AgencyPlatform.GetComeFromIndex(agencyPlatform[select]);
					data.clientMode = clientMode;
				}

                EditorGUILayout.Space();
                unityDebugMode = EditorGUILayout.Toggle("Is UnityDebug Model : ", unityDebugMode);
			
//				if (clientMode != AgencyPlatform.ClientMode_test)
//				{
//					unityDebugMode = false;
//				}else{
//					unityDebugMode = true;
//				}		
				data.unityDebugMode = unityDebugMode;

				EditorGUILayout.Space();
				unityDebugLogMode = EditorGUILayout.Toggle("Is UnityDebugLog Model : ", unityDebugLogMode);
				data.unityDebugLogMode = unityDebugLogMode;
			
                EditorGUILayout.Space();
                isClientRelease = EditorGUILayout.Toggle("Client Release : ", isClientRelease);
		
				//暂时取消
//				if (clientMode != AgencyPlatform.ClientMode_test && clientMode != AgencyPlatform.ClientMode_gmtest)
//				{
//					isClientRelease = true;
//				}else{
//					isClientRelease = false;
//				}
			
                data.release    = isClientRelease;

                isCollectDebugInfo = EditorGUILayout.Toggle("Collect Debug Info : ", isCollectDebugInfo);
                data.collectDebugInfo = isCollectDebugInfo;
                EditorGUILayout.Space();
				 
				//android的审核包，获取的客户端配置需要特殊处理//
				androidSPTestMode = EditorGUILayout.Toggle( "Android SP Test Mode :" , androidSPTestMode  );
				EditorGUILayout.LabelField( "Android SP Test Mode ： 打android审核包的设定，勾选了之后，除了test渠道，其他任何渠道，客户端版本配置Json都指向test" );
				EditorGUILayout.Space();
				
				data.androidSPTestMode = androidSPTestMode;
				
                if (isExport)
                {
                    if (GUILayout.Button("Export Assetbundles "))
                    {
                        SaveGameSettingData();
                        this.Close();

                        if (settingCallBackFunc != null)
                        {
                            settingCallBackFunc();
                        }
                    }
                }
                else
                {
                    GUI.color = Color.yellow;
			
                    if (GUILayout.Button("保存配置", GUILayout.Height(40)))
                    {
						SaveComeFromConfig(clientMode);
                    }
				
					GameEditorUtils.Space(2);

				#if UNITY_ANDROID
					if (GUILayout.Button("打包当前渠道", GUILayout.Height(40)))
					{
						if (gameLoadMode == GameLoadMode.Local_Assets){
							ShowNotification(new GUIContent("当前设置不可以打包"));
							return;
						}
						
						if (EditorUtility.DisplayDialog("打包确认", "是否确认打包当前渠道?","确认打包")){
							buildCurrentClientMode = true;
						}					
					}
					
					GameEditorUtils.Space(2);
					
					if (GUILayout.Button("打包所有商务渠道", GUILayout.Height(40)))
					{
						if (gameLoadMode == GameLoadMode.Local_Assets){
							ShowNotification(new GUIContent("当前设置不可以打包"));
							return;
						}
						
						if (EditorUtility.DisplayDialog("打包确认", "是否确认打包所有渠道?","确认打包")){
							buildAllClientMode = true;
						}
					}	

					GameEditorUtils.Space(2);
				#endif
                    
                    GUI.color = Color.green;
                    if (GUILayout.Button("一键移动资源至StreamAsset", GUILayout.Height(40)))
                    {
						if (gameLoadMode == GameLoadMode.Remote_Assets || gameLoadMode == GameLoadMode.Local_Assets)
						{
							return;
						}

						if( gameLoadMode == GameLoadMode.Local_Experience_Pack_Assets )
						{
							isExPackage = true;
						}
						else
						{
							isExPackage = false;
						}
					
                        _MoveAssetBundle();
                    }
				
                    GUI.color = Color.white;
                }
            }
            else
            {
                EditorGUILayout.LabelField("GameSetting is null !!!!!!");
            }
        EditorGUILayout.EndVertical();
		
		if (buildCurrentClientMode){
			buildCurrentClientMode = false;
			SaveComeFromConfig(clientMode);
			BulidTarget(clientMode);			
		}
		
//		if (buildAllClientMode){			
//			buildAllClientMode = false;
//			
//			List<int> comeFroms = new List<int>();
//							
//			if (gameType == GameType.TSJJ){
//				//comeFroms.Add(AgencyPlatform.ClientMode_test);
//				comeFroms.Add(AgencyPlatform.ClientMode_9game);
//				comeFroms.Add(AgencyPlatform.ClientMode_xiaomi);
//				comeFroms.Add(AgencyPlatform.ClientMode_360);
//				comeFroms.Add(AgencyPlatform.ClientMode_91);
//				comeFroms.Add(AgencyPlatform.ClientMode_duoku);
//				comeFroms.Add(AgencyPlatform.ClientMode_downjoy);
//				comeFroms.Add(AgencyPlatform.ClientMode_appchina);
//				comeFroms.Add(AgencyPlatform.ClientMode_wandoujia);
//				comeFroms.Add(AgencyPlatform.ClientMode_yayawan);
//				comeFroms.Add(AgencyPlatform.ClientMode_pipaw);
//				comeFroms.Add(AgencyPlatform.ClientMode_baoruan);
//				comeFroms.Add(AgencyPlatform.ClientMode_gmtest);
//				comeFroms.Add(AgencyPlatform.ClientMode_baoyugame);
//				comeFroms.Add(AgencyPlatform.ClientMode_shoumeng);
//				comeFroms.Add(AgencyPlatform.ClientMode_anzhi);
//				comeFroms.Add(AgencyPlatform.ClientMode_zhidian);
//				comeFroms.Add(AgencyPlatform.ClientMode_shoumeng360);
//				comeFroms.Add(AgencyPlatform.ClientMode_gfan);
//				comeFroms.Add(AgencyPlatform.ClientMode_jb2324);
//				comeFroms.Add(AgencyPlatform.ClientMode_mumayi);
//				comeFroms.Add(AgencyPlatform.ClientMode_samsung);
//				comeFroms.Add(AgencyPlatform.ClientMode_uucun);
//				comeFroms.Add(AgencyPlatform.ClientMode_huawei);
//				comeFroms.Add(AgencyPlatform.ClientMode_oppo);
//				comeFroms.Add(AgencyPlatform.ClientMode_koudai360);
//				
//				if (gameLoadMode == GameLoadMode.Remote_Assets)
//				{
//					comeFroms.Add(AgencyPlatform.ClientMode_10086);
//				}				
//			}else if (gameType == GameType.KDXJ){
//				//comeFroms.Add(AgencyPlatform.ClientMode_test);
////				comeFroms.Add(AgencyPlatform.ClientMode_9game);
////				comeFroms.Add(AgencyPlatform.ClientMode_91);
////				comeFroms.Add(AgencyPlatform.ClientMode_duoku);
////				comeFroms.Add(AgencyPlatform.ClientMode_downjoy);
////				comeFroms.Add(AgencyPlatform.ClientMode_wandoujia);
////				comeFroms.Add(AgencyPlatform.ClientMode_baoyugame);
////				comeFroms.Add(AgencyPlatform.ClientMode_shoumeng);
//			}else if (gameType == GameType.YDMM){
//				if (gameLoadMode == GameLoadMode.Remote_Assets)
//				{
//					comeFroms.Add(AgencyPlatform.ClientMode_10086);
//				}
//			}
			
//			foreach (int comeFrom in comeFroms){
//				SaveComeFromConfig(comeFrom);
//				BulidTarget(comeFrom);
//			}			
//		}
    }
	
	//保存渠道配置
	private void SaveComeFromConfig(int clientMode){
		if (data != null){
			data.clientMode = clientMode;
		}

		//修改游戏签名
		ChangeKeystorePass(clientMode);
		
		//修改游戏测试模式
		ChangeClientReleaseState(clientMode);
		
        //修改版本号
        ChangeBundleVersion();
		
        //修改游戏ICON
        ChangeICON(clientMode);
        
        //修改游戏标签
        ChangeBundleIdentifier(clientMode);
		
        //移动渠道资源（bin、lib、res、androidMainfest）
		#if UNITY_ANDROID
		ChangComeFrom(clientMode);
		#endif
        
		
        //改变需要的宏
        ChangDefineSymbols(clientMode);
		
		//修改产品名
		ChangeProductName();
		
        //修改BuildeSettings
        ChangBuildSettings(gameLoadMode);
		
		//修改资源目录
		ChangStreammingAssetPath( gameLoadMode );
	
        SaveGameSettingData();
	}
	
	private void ChangeKeystorePass(int clientMode)
	{
//#if UNITY_EDITOR && UNITY_ANDROID
//        if (clientMode == AgencyPlatform.ClientMode_downjoy) {
//            PlayerSettings.Android.keystoreName = "PublishKey/downjoy_1162_CGSn7li2Tkq1bph.keystore";
//            PlayerSettings.keystorePass = "downjoy_1162";
//            
//            PlayerSettings.Android.keyaliasName = "1162";
//            PlayerSettings.Android.keyaliasPass = "downjoy_1162";
//        } else {
//            PlayerSettings.Android.keystoreName = "PublishKey/nucleus.keystore";
//            PlayerSettings.keystorePass = "nucleus123";
//            
//            PlayerSettings.Android.keyaliasName = "nucleus";
//            PlayerSettings.Android.keyaliasPass = "nucleus123";
//        }
//#endif
	}
	
	private void ChangeClientReleaseState(int clientMode)
	{
//		if (clientMode != AgencyPlatform.ClientMode_test && clientMode != AgencyPlatform.ClientMode_gmtest)
//		{
//			isClientRelease = true;
//		}else{
//			isClientRelease = false;
//		}
//	
        data.release    = isClientRelease;		
		
//		if (clientMode != AgencyPlatform.ClientMode_test)
//		{
//			unityDebugMode = false;
//		}else{
//			unityDebugMode = true;
//		}		
		
		data.unityDebugMode = unityDebugMode;		
	}
	
    private void ChangeBundleVersion()
    {
        PlayerSettings.bundleVersion = Version.bundleVersion;
		PlayerSettings.Android.bundleVersionCode = Version.bundleVersionCode;
    }


    private void ChangDefineSymbols(int clientMode)
    {
		List<string> defineSymbolsList = new List<string>();

        string defineSymbols = string.Empty;
        if (unityDebugMode)
        {
			defineSymbolsList.Add("UNITY_DEBUG");
        }

		if (unityDebugLogMode)
		{
			defineSymbolsList.Add("UNITY_DEBUG_LOG");
		}

        if (clientMode != AgencyPlatform.ClientMode_test)
        {
			defineSymbolsList.Add("UNITY_" + AgencyPlatform.GetComeFrom(clientMode).ToUpper());
        } else {
            defineSymbolsList.Add("UNITY_CHANNEL_TEST;");
        }

		defineSymbolsList.Add("GAME_" + GetGameTypeShortName());

		defineSymbols = string.Join(";", defineSymbolsList.ToArray());
		
#if UNITY_IPHONE
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, defineSymbols); 
#elif UNITY_ANDROID
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbols);
#else
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defineSymbols);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebPlayer, defineSymbols);
#endif
    }
    
	private void ChangeProductName(){
		if (clientMode == AgencyPlatform.ClientMode_test)
		{
			if (isClientRelease)
			{
				PlayerSettings.productName = "仙侠问情";
			}
			else
			{
				PlayerSettings.productName = "仙侠问情Dev";
			}
		}
		else
		{
			PlayerSettings.productName = "仙侠问情";
		}	
	}
	
    private string[] copyFolder = new string[] { "assets", "bin", "libs", "res" };
    private string copyFileName = "AndroidManifest.xml";
    private string pluginPath = Application.dataPath + "/Plugins/Android/";
    private void ChangComeFrom(int clientMode)
    {
        //if (originPlatForm == clientMode) return;
		Version.ClientMode = clientMode;
        string applicationPath = Application.dataPath;
        int index = applicationPath.LastIndexOf("Assets");
        applicationPath = applicationPath.Substring(0, index);

        string comeFromPath = applicationPath + "SPRes/" + GetGameTypeShortName() + "/";

        //复制AndroidManifest.xml --  begin
        Debug.Log("dest AndroidManifest : " + pluginPath + copyFileName);
        if (File.Exists(pluginPath + copyFileName))
        {
            File.Delete(pluginPath + copyFileName);
        }

        string mainfespath = comeFromPath + AgencyPlatform.GetComeFrom(clientMode) + "/" + copyFileName;
        Debug.Log("source AndroidManifest : " + mainfespath);
        if (File.Exists(mainfespath))
        {
            File.Copy(mainfespath, pluginPath + copyFileName);
        }
        //复制AndroidManifest.xml --  end

        //	特殊的文件复制
//        string anySdkSoFileName = "libanysdk.so";
//		if (AgencyPlatform.ClientMode_anysdk == clientMode) {
//			//复制libanysdk.so
//	        Debug.Log("dest libanysdk.so : " + pluginPath + anySdkSoFileName);
//	        if (File.Exists(pluginPath + anySdkSoFileName))
//	        {
//	            File.Delete(pluginPath + anySdkSoFileName);
//	        }
//
//	        string soFilePath = comeFromPath + AgencyPlatform.GetComeFrom(clientMode) + "/" + anySdkSoFileName;
//	        Debug.Log("source libanysdk.so : " + soFilePath);
//	        if (File.Exists(soFilePath))
//	        {
//	            File.Copy(soFilePath, pluginPath + anySdkSoFileName);
//	        }
//		} else {
//			if (File.Exists(pluginPath + anySdkSoFileName))
//	        {
//	            File.Delete(pluginPath + anySdkSoFileName);
//	        }
//		}

        //复制目录
        foreach (string folderName in copyFolder)
        {
            string sourceFolder = comeFromPath + AgencyPlatform.GetComeFrom(clientMode) + "/" + folderName;
            string deseFolder = pluginPath + folderName;

            Debug.Log("sourceFolder : " + sourceFolder);
            Debug.Log("deseFolder : " + deseFolder);
 

            if (Directory.Exists(deseFolder))
            {
                Directory.Delete(deseFolder, true);
            }

            if (Directory.Exists(sourceFolder))
            {
                DirectoryCopy(sourceFolder, deseFolder, true);
            }
        }
    }
	
	private string GetAgencyPlatformPrefix()
	{
#if UNITY_IPHONE
		return "com.baoyugame.xxwq.ios.";
#elif UNITY_ANDROID
		return "com.baoyugame.xxwq.android.";
#else
		return "";
#endif
	}
	
    private void ChangeBundleIdentifier(int clientMode)
    {
//#if UNITY_IPHONE
//		if (clientMode == AgencyPlatform.ClientMode_test)
//		{
//			if (isClientRelease)
//			{
//				PlayerSettings.bundleIdentifier = "com.baoyugame.xxwq.release";
//			}
//			else
//			{
//				PlayerSettings.bundleIdentifier = "com.baoyugame.xxwq.dev";
//			}
//		}
//		else
//		{
//			PlayerSettings.bundleIdentifier = "com.baoyugame.xxwq.dev";
//		}
//#else
//		if (clientMode == AgencyPlatform.ClientMode_test)
//		{
//			if (isClientRelease)
//			{
//				PlayerSettings.bundleIdentifier = "com.baoyugame.pocketts.release";
//			}
//			else
//			{
//				PlayerSettings.bundleIdentifier = "com.baoyugame.pocketts.dev";
//			}
//		}
//		else
//		{
//			PlayerSettings.bundleIdentifier = "com.baoyugame.pocketts.dev";
//		}
//#endif

		if (AgencyPlatform.agencyPlatformBuildSuffix.ContainsKey(clientMode))
        {
            //设置程序标签
			PlayerSettings.bundleIdentifier = GetAgencyPlatformPrefix() + AgencyPlatform.agencyPlatformBuildSuffix[clientMode];
        }
        else
        {
            //设置程序标签
            PlayerSettings.bundleIdentifier = GetAgencyPlatformPrefix() + "dev";
        }
    }

    private string splashPath = "Assets/UI/AppSplash/splash.jpg";
    private string iconPath = "Assets/UI/PlatformRes/";
    private string icon114  = "icon/114.png";
    private string icon96   = "icon/96.png";
    private string icon72   = "icon/72.png";
    private string icon48   = "icon/48.png";
    private string icon36   = "icon/36.png";
    private string splash   = "splash.jpg";
    private void ChangeICON(int clientMode)
    {
        //if (originPlatForm == clientMode) return;
		
		string theIconPath =  iconPath + GetGameTypeShortName() + "/";
		string platformIconPath =  theIconPath + AgencyPlatform.GetComeFrom(clientMode) + "/";

		//小米渠道打包icon配置修改
		/*
        if (clientMode == AgencyPlatform.ClientMode_xiaomi || clientMode == AgencyPlatform.ClientMode_10086){
			PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[1] { null });  
			PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new Texture2D[4] { null, null, null, null });
			
	        //设置开始界面
	        if( File.Exists(splashPath))
	        {
	            File.Delete(splashPath);
	        }
	        File.Copy(platformIconPath + splash, splashPath);			
			
			return;
		}
        */

        //修改默认ICON
        Texture2D defaultIcon = AssetDatabase.LoadMainAssetAtPath( platformIconPath + icon114) as Texture2D;
        if (defaultIcon != null)
        {
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[1] { defaultIcon });        
        }
        else
        {
            Debug.Log("Set Defaut ICON Error!!!");
        }


        //修改平台ICON
        Texture2D texture96 = AssetDatabase.LoadMainAssetAtPath(platformIconPath + icon96) as Texture2D;
        Texture2D texture72 = AssetDatabase.LoadMainAssetAtPath(platformIconPath + icon72) as Texture2D;
        Texture2D texture48 = AssetDatabase.LoadMainAssetAtPath(platformIconPath + icon48) as Texture2D;
        Texture2D texture36 = AssetDatabase.LoadMainAssetAtPath(platformIconPath + icon36) as Texture2D;

        if (texture36 != null && texture72 != null && texture48 != null && texture96 != null)
        {
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new Texture2D[4] { texture96, texture72, texture48, texture36 });
        }
        else
        {
            Debug.Log(" Set Platform ICON Error!!!");
        }

        //设置开始界面
        if( File.Exists(splashPath))
        {
            File.Delete(splashPath);
        }

        File.Copy(platformIconPath + splash, splashPath);
    }


    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory doesn't exist, create it. 
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location. 
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
	
	private void ChangStreammingAssetPath(GameLoadMode loadMode)
	{
		string oriDir ="";
		string copyToDir = "";
		
		if( loadMode == GameLoadMode.Local_Packet_Assets || loadMode == GameLoadMode.Local_Experience_Pack_Assets )
		{
			oriDir = Application.dataPath + "/" + "StreamingAssets_s";
			copyToDir = Application.dataPath + "/" + "StreamingAssets";
		}
		else
		{
			oriDir = Application.dataPath + "/" + "StreamingAssets";
			copyToDir = Application.dataPath + "/" + "StreamingAssets_s";
		}
		
		if( Directory.Exists( oriDir ) && !Directory.Exists( copyToDir ))
		{
			Directory.Move( oriDir, copyToDir );    
			
			if (Directory.Exists( oriDir ))
			{
				Directory.Delete( oriDir );
			}
		}
	}

    private void ChangBuildSettings(GameLoadMode loadMode)
    {

        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length];
        System.Array.Copy(original, newSettings, original.Length);

        if (loadMode == GameLoadMode.Local_Assets)
        {
            foreach (EditorBuildSettingsScene scene in newSettings)
            {
                    scene.enabled = true;
            }
        }
        else
        {
            foreach (EditorBuildSettingsScene scene in newSettings)
            {
                if (scene.path != "Assets/Scenes/OriginScene.unity" &&
                    scene.path != "Assets/Scenes/GameScene.unity")
				{
					scene.enabled = false;
				}
				else
				{
					scene.enabled = true;
				}
            }
        }
        EditorBuildSettings.scenes = newSettings;
    }
	
	private string GetGameTypeShortName(){
		if (gameType == GameType.XXWQ){
			return "XXWQ";
		}else{
			return "";
		}
	}
	
	#region 批量打包渠道
    //这里封装了一个简单的通用方法。
	private void BulidTarget(int comeFrom)
	{
		string gameMode = "";
		if (isClientRelease)
		{
			gameMode = "Release";
		}
		else
		{
			gameMode = "Develop";
		}

		string gameLoadModeStr = "";
		if (gameLoadMode == GameLoadMode.Local_Packet_Assets){
			gameLoadModeStr = "all";
		}else if(gameLoadMode == GameLoadMode.Local_Experience_Pack_Assets){
			gameLoadModeStr = "mini";
		}else{
			gameLoadModeStr = "update";
		}
		
		string app_name = "XianXiaWenQing_";

		string comeFromStr = null;
		if( androidSPTestMode )
		{
			comeFromStr = "androidTestMode";
		}
		else
		{
			comeFromStr = Version.GetComeFrom();
		}

		app_name += gameMode + "_" +comeFromStr +"_" + PlayerSettings.bundleVersion + "_" + GetBuildDateTime()+ "_"+gameLoadModeStr;
	    string target_name = app_name + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
	    BuildTarget buildTarget = BuildTarget.Android;

	    string applicationPath = Application.dataPath.Replace("/Assets","");	
		string target_dir = "";
        target_dir = applicationPath + "/APK/" + AgencyPlatform.GetBuildDir(comeFrom);;
		
		ShowNotification(new GUIContent("正在打包:"+target_name));
		
        //每次build删除之前的残留
		if(Directory.Exists(target_dir)) 
  		{
			if (File.Exists(target_name))
			{
			  File.Delete(target_name);
			}
	    }else
		{
			Directory.CreateDirectory(target_dir); 
		}

	
		string[] scenes = FindEnabledEditorScenes();
	
		//开始Build场景，等待吧～
        GenericBuild(scenes, target_dir + "/" + target_name, buildTarget,BuildOptions.None);
	}

	private string GetBuildDateTime()
	{
		DateTime dateTtime = DateTime.UtcNow.ToLocalTime();
		string str = string.Format("{0:D2}{1:D2}{2:D2}_{3:D2}{4:D2}" ,dateTtime.Year , dateTtime.Month , dateTtime.Day ,  dateTtime.Hour , dateTtime.Minute );
		return str;
	}

	private string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

    private void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {   
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);

            if (res.Length > 0) {
                    throw new Exception("BuildPlayer failure: " + res);
            }
    }	
	#endregion
	
	private const string minipackFolder = "minipackage/";
	private static string assetExportPath = string.Empty;
    private static string exportPath = "Export";
    private int curCopyAsset = 0;
    private int totalCopyAsset = 0;
	private bool isExPackage = false;
	
    private void _MoveAssetBundle()
    {
        assetExportPath = Application.dataPath;
        assetExportPath = assetExportPath.Substring(0, assetExportPath.LastIndexOf("/") + 1);
        assetExportPath += exportPath + "/";

        string assetbundleVersionDataPath = assetExportPath + "ResourceData.bytes";//"ResourceVersionData.bytes";

        if (File.Exists(assetbundleVersionDataPath))
        {
            //删除SteamPath
            _DeleteStreamPath();

            curCopyAsset = 0;
            totalCopyAsset = 0;

            //移动XML资源
            ResourcesVersionData versionData  = _GetResourceVersionData( assetbundleVersionDataPath );
			
            MergePackageList mergePackageList = new MergePackageList();
            CollectNeedDownLoadPackage(versionData, mergePackageList);
            mergePackageList.AnalyMessage();
            //totalCopyAsset = mergePackageList.GetPackageNumber();

            while (!mergePackageList.IsEnd())
            {
                MergePackage meragePackage = mergePackageList.NextPackage();
                _CopyToStreamAsset(assetExportPath, meragePackage.packageName, meragePackage.assetPackageList); 
            }

            mergePackageList.Dispose();

			if( isExPackage /*&& EditorUtility.DisplayDialog( "提示", "是否生成Minipackage ?" , "生成", "取消" )*/)
			{
				string miniPackage = assetExportPath + minipackFolder;
				_CreateMiniPack( miniPackage, versionData );
			}
			
		    EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Tips", "移动完成", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Tips", "ResourceData.bytes文件不存在", "OK");
        }
    }
	

    /// <summary>
    /// 读取ResourceVersionData 文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private ResourcesVersionData _GetResourceVersionData(string path  )
    {
		byte[] resourceData =  GameEditorUtils.GetFileBytes( path, false  );
		
		ResourcesVersionData 	resVerdata = null;
		AssetPathReferenceList  refList    = null;
		
		if( resourceData != null )
		{
			 byte[]	resVersionDataBytes	= null;
			 byte[]	assetRefListBytes	= null;
			 byte[]	mapAreaDataBytes	= null;			
			
			ResourceDataManager.AnalyResourceData( 	resourceData,
													out resVersionDataBytes,
													out assetRefListBytes
				);
													
			
			//生成ResourceVersion
	        Dictionary< string , object > resVerObj =  GameEditorUtils.GetJsonFile( resVersionDataBytes, true );
	        JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
	        resVerdata = parser.DeserializeJson_ResourcesVersionData( resVerObj );			
			
			
			//生成AssetReferenceList
			Dictionary< string , object > refListObj =  GameEditorUtils.GetJsonFile( assetRefListBytes, true );
			JsonAssetPathReferenceListParser assetRefParser = new JsonAssetPathReferenceListParser();
			refList = assetRefParser.DeserializeJson_AssetPathReferenceList( refListObj );
			
			AssetPathReferenceManager.Instance.Setup( refList );
	        //totalCopyAsset = 0;
	
            foreach( KeyValuePair< string, AssetPrefab > item in resVerdata.assetPrefabs )
            {
                item.Value.isLocalPack = true;
            }

            foreach( KeyValuePair< string, AssetPrefab > item in resVerdata.commonObjs )
            {
                item.Value.isLocalPack = true;
            }

            foreach( KeyValuePair< string, AssetPrefab > item in resVerdata.scenes )
            {
                item.Value.isLocalPack = true;
                item.Value.needDecompress = true;
            }
			
			
			//重新保存资源
			//重新生成ResourceVersionData
			resVerObj = parser.SerializeJson_ResourcesVersionData( resVerdata );
			resVersionDataBytes = GameEditorUtils.GetJsonFileByte( resVerObj, true );
			
			byte[] newResourceByte = ResourceDataManager.AssembleResourceData( resVersionDataBytes,
																				assetRefListBytes
																				);
			
			_SaveResourceData(newResourceByte, assetExportPath, "ResourceData.bytes");	
			
		}
        
		return resVerdata;
    }

    /// <summary>
    /// 保存整包版本的ResourceVersionData
    /// </summary>
    /// <param name="data"></param>
    /// <param name="rootPath"></param>
    /// <param name="assetPath"></param>
//    private void _SaveResourceVersionData( ResourcesVersionData data,  string rootPath, string assetPath)
//    {
//        string destPath = Application.streamingAssetsPath + "/" + assetPath;
//        string destFolder = destPath.Substring(0, destPath.LastIndexOf("/") + 1);
//
//        if (!Directory.Exists(destFolder))
//        {
//            Directory.CreateDirectory(destFolder);
//        }
//
//        JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
//        Dictionary<string, object> obj = parser.SerializeJson_ResourcesVersionData(data);
//        GameEditorUtils.SaveJsonFile(obj, destPath, true);
//    }
//	
	
	
    private void _SaveResourceData( byte[] buff,  string rootPath, string assetPath)
    {
        string destPath = Application.streamingAssetsPath + "/" + assetPath;
        string destFolder = destPath.Substring(0, destPath.LastIndexOf("/") + 1);

        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }
		
		GameEditorUtils.SaveFileByte( buff, destPath );
    }
	
	
	
	
    private void CollectNeedDownLoadPackage(ResourcesVersionData data, MergePackageList list )
    {
        Dictionary<string, List<AssetPackage>> returnDic = new Dictionary<string, List<AssetPackage>>();

        if (data != null)
        {
            _GetPackageMessage(data.assetPrefabs, list);
            _GetPackageMessage(data.commonObjs,   list);
            _GetPackageMessage(data.scenes,       list);
        }
    }

    private void _GetPackageMessage(Dictionary<string, AssetPrefab> assetPrefabs, MergePackageList list)
    {
        foreach (KeyValuePair<string, AssetPrefab> item in assetPrefabs)
        {
			bool isAnaly = true;
			if( isExPackage )
			{
				if((item.Value.isEp == 0))
				{
					isAnaly = false;
				}
			}
				
			if( isAnaly )
			{
	            totalCopyAsset++;
	            list.AddUpdateAsset(item.Key, item.Value);				
			}
        }
    }

    //private void _CopyToStreamAsset( string rootPath, string package, List< AssetPackage > assetPrefabList )
    //{
    //    /*
    //    string sourcePath = rootPath + package + ".baoyu";
    //    MergeAssetParser parser = _GetMergeAssetParser(sourcePath);
    //    if (!parser.isCorrect)
    //    {
    //        Debug.LogError("Error : package - " + package + " is wrong file format ");
    //        return;
    //    }
    //    else
    //    {
    //        foreach (AssetPackage assetPackage in assetPrefabList)
    //        {
    //            byte[] assetBuff = parser.GetAssetBuffs(assetPackage.assetPrefab.packagePos);
    //            string destPath = Application.streamingAssetsPath + "/" + assetPackage.name + ".unity3d";
    //            _SaveAsset(assetBuff, destPath); 

    //            curCopyAsset++;
    //            EditorUtility.DisplayProgressBar("资源正在复制中",
    //                             "复制资源数 : " + curCopyAsset + "/" + totalCopyAsset,
    //                             (float)curCopyAsset / totalCopyAsset
    //                             );
    //        }
    //    }
    //   */

    //    string sourcePath = rootPath + package + ".baoyu";
    //    string destPath   = Application.streamingAssetsPath + "/" + package + ".baoyu";
    //    string destFolder = destPath.Substring(0, destPath.LastIndexOf("/"));

    //    if (!Directory.Exists(destFolder))
    //    {
    //        Directory.CreateDirectory(destFolder);
    //    }

    //    File.Copy(sourcePath, destPath);
        
    //    curCopyAsset++;
    //    EditorUtility.DisplayProgressBar("资源正在复制中",
    //                        "复制资源数 : " + curCopyAsset + "/" + totalCopyAsset,
    //                        (float)curCopyAsset / totalCopyAsset
    //                        );

    //}

    private void _CopyToStreamAsset(string rootPath, string package, List<AssetPackage> assetPrefabList)
    {
        string sourcePath = rootPath + package + ".baoyu";
        MergeAssetParser parser = _GetMergeAssetParser(sourcePath);
        if (!parser.isCorrect)
        {
            Debug.LogError("Error : package - " + package + " is wrong file format ");
            return;
        }
        else
        {
            foreach (AssetPackage assetPackage in assetPrefabList)
            {
                byte[] assetBuff = parser.GetAssetBuffs(assetPackage.assetPrefab.packagePos);
				
				string assetPath = AssetPathReferenceManager.Instance.GetAssetPath( assetPackage.name );
                string destPath = Application.streamingAssetsPath + "/" +  assetPath + ".unity3d";
                
#if UNITY_IPHONE
				//如果是IPHONE打整包， 里面的资源是不存在外部压缩的资源， 需要转移的时候进行解压
				//if( !assetPackage.assetPrefab.needDecompress )
				//{
				//	assetBuff = ZipLibUtils.Uncompress( assetBuff );
				//}
#endif				
				
				_SaveAsset(assetBuff, destPath); 

                curCopyAsset++;
                EditorUtility.DisplayProgressBar("资源正在复制中",
                                 "复制资源数 : " + curCopyAsset + "/" + totalCopyAsset,
                                 (float)curCopyAsset / totalCopyAsset
                                 );
            }
        }
    }


    /// <summary>
    /// 获取MergeAsetParser
    /// </summary>
    /// <param name="packagePath"></param>
    /// <returns></returns>
    private MergeAssetParser _GetMergeAssetParser(string packagePath)
    {
        MergeAssetParser parser = null;

        try
        {
            FileStream fs = new FileStream( packagePath, FileMode.Open );
            
            byte[] buff = new byte[ fs.Length];
            fs.Read( buff, 0, (int)fs.Length );
            fs.Close();

            parser = new MergeAssetParser( buff );
        }
        catch(FileNotFoundException ex )
        {
            Debug.LogException(ex);
        }

        return parser;
    }


    /// <summary>
    /// 保存资源信息
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="path"></param>
    private void _SaveAsset(byte[] buff, string path)
    {
        string destFolder = path.Substring(0, path.LastIndexOf("/") + 1);

        if (!Directory.Exists(destFolder))
        {
            Directory.CreateDirectory(destFolder);
        }

        try
        {
            if (File.Exists(path)) File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(buff);
            
            
            writer.Close();
            fs.Close();
        }
        catch (IOException e)
        {
            Debug.LogException(e);
        }
    }


    private void _DeleteStreamPath()
    {
        String path = Application.streamingAssetsPath;
        //path.Replace("/", "\\");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }


        String localPath = Application.dataPath + "/Resources/" + PathHelper.LOCALASSETPATH;
        if (Directory.Exists(localPath))
        {

            Directory.Delete(localPath, true);
        }
    }
	

	
	/// <summary>
	/// 生成MiniPackage
	/// </summary>
	private void _CreateMiniPack( string minipackagePath , ResourcesVersionData versionData )
	{
		if( Directory.Exists( minipackagePath ) )
		{
			Directory.Delete(minipackagePath, true);
		}
		
		Directory.CreateDirectory(minipackagePath);
		
		List< string > folderList = new List<string>();
		AnalyAssetPrefabs( folderList, versionData.assetPrefabs );
		AnalyAssetPrefabs( folderList, versionData.commonObjs );
		AnalyAssetPrefabs( folderList, versionData.scenes );
		
		for( int i = 0;  i < folderList.Count ; i ++ )
		{
			string folderId = folderList[i];
			
            EditorUtility.DisplayProgressBar("Minipackage生成中",
                             folderId + "/-" + i + "/" + folderList.Count,
                             (float)i / folderList.Count
                             );			
			
			string folderPath = AssetPathReferenceManager.Instance.GetFolderPathFormRefId( int.Parse( folderId ) );
			if( !string.IsNullOrEmpty( folderPath ))
			{
				string sourcePath = Application.streamingAssetsPath + "/" + folderPath;
				string packageName = minipackagePath + folderId + ".zBaoyu";
				
				ZipLibUtils.ZipFileDirectory(sourcePath,packageName);
			}
			else
			{
				Debug.LogError( string.Format( "Get FolderPath Error : {0}" , folderId ));
			}
		}
		
		EditorUtility.ClearProgressBar();
		
	}
	
	
	
	private void AnalyAssetPrefabs( List< string > collectList ,  Dictionary< string , AssetPrefab> dict )
	{
        foreach( KeyValuePair< string, AssetPrefab > item in dict )
        {
            if( item.Value.isEp == 1 )
			{
				string folder = GetFolderId( item.Key );
				if( !string.IsNullOrEmpty( folder ) && !collectList.Contains( folder ) )
				{
					collectList.Add( folder );
				}
			}
        }		
	}
	
	private string GetFolderId( string refId )
	{
		string[] split = refId.Split(':');
		
		if( split.Length == 2 )
		{
			return split[0];
		}
		else
		{
			return null;
		}
	}
}
