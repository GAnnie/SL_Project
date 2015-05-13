// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GameDebugerText.cs
// Author   : SK
// Created  : 2013/3/4
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using Junfine.Debuger;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class GameDebuger : MonoBehaviour
{  
	public Rect startRect; // The rect the window is initially displayed at.	

	public Rect startBgRect; // The rect the window is initially displayed at.	
	
	private float windowPosition = -1000.0f;
	private bool showDebug = true;
	private static string windowText = "";
	private Vector2 scrollViewVector = Vector2.zero;
	private GUIStyle debugBoxStyle;
	private float leftSide = 0.0f;
	private float debugWidth = 720.0f;

	public static bool debugIsOn = false;

	public static void Log (object message, bool logToFile=true)
	{
		string str = (message == null) ? "Null" : message.ToString ();
		str = str.Replace ("{", "[");
		str = str.Replace ("}", "]");
		Debuger.Log (str);
		iOSUtility.XCodeLog(str);

//#if UNITY_DEBUG
//		if (logToFile){
//			windowText = str + "\n" + windowText;
//		}
//#endif
    }

	public static void LogWithColor(object message)
	{
		string str = (message == null) ? "Null" : message.ToString ();
		str = StringHelper.WrapColorWithLog (str);
		Log(message);
	}

	public static void LogBattleInfo(object message)
	{
		LogWithColor (message);
	}

    private static long lastTime = 0;

    public static void SetNowTime()
    {
        lastTime = DateTime.Now.Ticks;
    }

    public static void LogTime(string str)
    {
#if UNITY_DEBUG
        long delayTime = DateTime.Now.Ticks - lastTime;
        TimeSpan elapsedSpan = new TimeSpan(delayTime);
        SetNowTime();
        string info = "[Debug LogTime] " + str + " T=" + elapsedSpan.TotalSeconds + "s";
        UnityEngine.Debug.Log(info);
        windowText = info + "\n" + windowText;
#endif
    }

#if UNITY_DEBUG_LOG && !UNITY_DEBUG
	private bool sendDebuging = false;
	void OnGUI (){
		if (sendDebuging == false){
			if ( GUI.Button( new Rect ( 0, 0, 100, 100), "发送调试信息" ))
			{
				sendDebuging = true;
				MachineManager.Instance.SendDebugLog(delegate() {
					sendDebuging = false;
				});
			}
		}
	}
#endif

//#if UNITY_DEBUG
	void Start ()
	{  
		startRect = new Rect(0f, Screen.height-50f, 75, 50f );

		startBgRect = new Rect (0f, 0f, Screen.width, Screen.height-50);

		debugBoxStyle = new GUIStyle ();  
		debugBoxStyle.fontSize = 14;
		debugBoxStyle.normal.textColor = Color.green;
		debugBoxStyle.alignment = TextAnchor.UpperLeft;

		Debuger.enableLog = true;
	}
   
//	void OnGUI ()
//	{  
//		if (debugIsOn) {  
//			if (GUI.Button (startRect, showDebug?"Hide DB":"Show DB")) {  
//				showDebug = !showDebug;
//				if (showDebug) {  
//					windowPosition = 0;
//				} else {  
//					windowPosition = -1000.0f;
//				}
//			}  
//
//			if (showDebug)
//			{
//				GUI.color = Color.gray;
//				startBgRect = GUI.Window(1, startBgRect, DoMyWindow, "");
//			}
//		}  
//	}  
//#endif

	void DoMyWindow(int windowID)
	{
		GUI.depth = 0;    
		GUI.BeginGroup (new Rect (windowPosition, 80.0f, 700, 350.0f));
		
		scrollViewVector = GUI.BeginScrollView (new Rect (0, 0.0f, debugWidth, 350.0f), 
		                                        scrollViewVector, 
		                                        new Rect (0.0f, 0.0f, 700.0f, 2000.0f));  
		GUI.Box (new Rect (0, 0.0f, debugWidth - 20.0f, 2000.0f), windowText, debugBoxStyle);  
		GUI.EndScrollView ();  
		
		GUI.EndGroup ();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SystemSetting.IsMobileRuntime)
        {
			string info = "";

			info += " 账号:" + LoginManager.Instance.loginId;
			PlayerDto playerDto = PlayerModel.Instance.GetPlayer();
			if (playerDto != null)
			{
                info += " ID:" + playerDto.id;
				info += " 昵称:" + playerDto.nickname;
			}
			//info += " token:" + LoginManager.Instance.GetPlayerID();
			
			ServerInfo serverInfo = ServerConfig.GetServerInfo(PlayerPrefs.GetString("lastServerId"));
			info += " 服务器:" + serverInfo.name;
			
			info += " 时间:" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");

			SceneDto sceneDto = WorldManager.Instance.GetModel().GetSceneDto();
			if (sceneDto != null)
			{
				info += (" 当前场景:" + sceneDto.name);
			}

			info += BattleManager.Instance.GetBattleInfo();

			clipBoard = info;
			
			GameDebuger.Log(info);

			TipManager.AddTip(info);

//			if (ProxyMainUIModule.IsShow())
//			{
//				ProxyMainUIModule.Hide();
//			}
//			else
//			{
//				ProxyMainUIModule.Show();
//			}
        }
	}
	
	#region clipBoard
	private static PropertyInfo m_systemCopyBufferProperty = null;
	private static PropertyInfo GetSystemCopyBufferProperty()
	{
		if (m_systemCopyBufferProperty == null)
		{
			Type T = typeof(GUIUtility);
			m_systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
			if (m_systemCopyBufferProperty == null)
				throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
		}
		return m_systemCopyBufferProperty;
	}
	public static string clipBoard
	{
		get
		{
			PropertyInfo P = GetSystemCopyBufferProperty();
			return (string)P.GetValue(null, null);
		}
		set
		{
			PropertyInfo P = GetSystemCopyBufferProperty();
			P.SetValue(null, value, null);
		}
	}	
	#endregion

	#region DebugError -> Use color=orange
	private static bool _openDebugLogOrange = false;
	public static bool openDebugLogOrange {
		get { return _openDebugLogOrange; }
		set { _openDebugLogOrange = value; }
	}
	public static void OrangeDebugLog(string s) {
		if (_openDebugLogOrange) {
			Debug.LogError(string.Format("<color=orange> ## {0} ## </color>", s));
		}
	}
	public static void AquaDebugLog(string s) {
		if (_openDebugLogOrange) {
			Debug.LogError(string.Format("<color=aqua> ## {0} ## </color>", s));
		}
	}
	#endregion
}
