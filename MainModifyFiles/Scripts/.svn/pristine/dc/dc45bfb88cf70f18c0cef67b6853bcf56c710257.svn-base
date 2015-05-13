// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  ExitGameScript.cs
// Author   : senkay
// Created  : 6/26/2013 9:29:59 AM
// Purpose  : 检查是否按了退出按钮
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.services;

public class ExitGameScript : MonoBehaviour {

    private static ExitGameScript _instance = null;

    public static ExitGameScript Instance
    {
        get
        {
            return _instance;
        }
    }

    private bool isClick;
	
	static public bool CheckConnected = false;
	
	void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 30;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Thread thread = new Thread(new ThreadStart(ReadyProtoByteArray));
        thread.Start();
    }

    private bool protobufReady = false;

    private void ReadyProtoByteArray()
    {
        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.PREPROTOBUF_DATA_TIME_TYPE);

        //提前序列化
        ProtoByteArray ba = new ProtoByteArray();
        ba.WriteObject(new GeneralRequest());

        protobufReady = true;

        GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.PREPROTOBUF_DATA_TIME_TYPE);
    }

    // Update is called once per frame
    void Update()
    {
        if (protobufReady)
        {
            protobufReady = false;
            DataManager.Instance.PreLoadDataCache();
        }

        if (Input.GetKey(KeyCode.Escape) && SystemSetting.IsMobileRuntime)
        {
            if (isClick == false)
            {
                isClick = true;

				GameDebuger.Log("InputKey is Escape");	
				OpenConfirmWindow();
            }
        }

		if (CheckConnected)
		{
			if (reLogining){
				reLogining = false;
				RequestLoadingTip.Stop();
			}
			
			if (!WindowManager.isToolUse){
				if (SocketManager.IsOnLink == false)
				{
					if (LoginManager.DuplicateLogin)
					{
						OpenDuplicateLoginTip();
					}
					else if (SocketManager.HAClose)
					{
						OpenServerCloseTip();
					}
					else
					{
						OpenReloginTip();
					}
					
					CheckConnected = false;
				}				
			}else{
				CheckConnected = false;
			}
		}
    }
	
	void OnApplicationPause(bool paused)
	{
	}
	
	void OnApplicationQuit()
	{
		FriendModel.Instance.Save();
		FriendModel.Instance.ClearData();
		SystemTimeManager.Instance.Stop();
		PlayerPrefs.Save();
		GameDataManager.Instance.SaveData ();
		VoiceRecognitionManager.Instance.DelTalkCache();
        BaoyugameSdk.UnregisterPower();
        //BaoyugameSdk.UnregisterGsmSignalStrength();
	}

	private void OpenConfirmWindow(){
		ProxyWindowModule.OpenConfirmWindow ("退出游戏\n\n离线自动挂机", "",
		    ()=>{
				isClick = false;
				HanderExitGame();
			}, 
			()=>{
				isClick = false;
			}
		);
	}
	
    public void HanderExitGame()
    {
		_relogin = false;

		LoginManager.Instance.RemoveListener();
		CheckConnected = false;
		
		if (SocketManager.IsOnLink)
        {
            ServiceRequestAction.requestServer(PlayerService.logout(), "", OnLogoutResponse, OnLogoutResponse);
            Invoke("ExitGame", 1f);
        }
        else
        {
            ExitGame();
        }
    }
	
    private void OnLogoutResponse(GeneralResponse e)
    {
        WindowManager.CleanAllView();
		
		ExitGame();
    }	
	
	public void HanderRelogin(){
		_relogin = false;

		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		SocketManager.Instance.Close(false);

		OnApplicationQuit ();

		LoginManager.Instance.RemoveListener();
		CheckConnected = false;
		LoginManager.DuplicateLogin = false;
		WorldManager.FirstEnter = true;
		WorldManager.Instance.Destroy ();
		BattleManager.Instance.Destroy ();
		BackpackModel.Instance.Destroy ();
		WarehouseModel.Instance.Destroy ();
		PlayerModel.Instance.Dispose ();
        SystemDataModel.Instance.Dispose();

		PlayerBuffModel.Instance.Dispose();
		
        if (SocketManager.IsOnLink)
        {
			ServiceRequestAction.requestServer(PlayerService.logout(), "", OnLogoutResponse2, OnLogoutResponse2);
            Invoke("GotoLoginScene", 1f);
        }
        else
        {
            GotoLoginScene();
        }
	}
	
    private void OnLogoutResponse2(GeneralResponse e)
    {
        WindowManager.CleanAllView();
		
		GotoLoginScene();
    }

    private bool _exited = false;

    private void ExitGame()
    {
#if UNITY_ANDROID
        if (_exited)
        {
            return;
        }
		
        _exited = true;
		
//		//ServiceProviderManager.Exit();
		
        // 关闭统计
        //UmengAnalyticsHelper.onPause();

        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        GameDebuger.Log("Exit Game!!!");
		
		//MachineManager.Instance.EndErrorInformation();

        //等待0.5秒后，关闭
        Invoke("_ExitGame", 0.2f);	

#else
		GotoLoginScene();
#endif
    }
	
	private bool _relogin = false;
	private void GotoLoginScene(){
        if (_relogin)
        {
            return;
        }
		
        _relogin = true;			
		
		CancelInvoke("GotoLoginScene");

		LoginManager.Instance.GotoLoginScene ();
	}
	
    private void _ExitGame()
    {
        Application.Quit();
    }
	
    void ChangeIsClick()
    {
        isClick = false;
    }
	
	public void EnableClick(){
		ChangeIsClick();
	}
	
	private void OpenServerCloseTip(){
		string tip = "网络中断, 请重新进入游戏";
		
		WindowManager.showMessageBox(tip,
		                             delegate()
		                             {
			HanderRelogin();
		}, 
		false,
		WindowManager.WINDOW_LEVEL.TOP_LEVEL
		);
	}
	
	private void OpenDuplicateLoginTip()
	{
		string tip = "你的角色已从其它客户端登录\n请注意账号安全";
		
		WindowManager.showMessageBox(tip,
		                             delegate()
		                             {
			HanderRelogin();
		},false,
		WindowManager.WINDOW_LEVEL.TOP_LEVEL
		);
	}
	
	private void OpenReloginTip()
	{
		string tip = "网络不稳定，请重新连接";
		
		if (forceCheck){
			tip = "网络中断了，请重新游戏";
		}
		
		if (forceCheck)
		{
			WindowManager.showMessageBox(tip,
			                             delegate()
			                             {
				HanderRelogin();
			},
			false,
			WindowManager.WINDOW_LEVEL.TOP_LEVEL
			);
		}
		else
		{
			WindowManager.showMessageBox(tip,
			                             delegate()
			                             {
				CheckRelogin();
			},
			false,
			WindowManager.WINDOW_LEVEL.TOP_LEVEL,
			"重新连接"
			);
		}
	}
	
	private static bool reLogining = false;
	
	private void CheckRelogin()
	{
		if (!LoginManager.DuplicateLogin)
		{
			reLogining = true;
			
			RequestLoadingTip.Show("正在连接服务器", true);
			LoginManager.Instance.Relogin();
			//TipManager.AddTip("正在尝试重新连接");
		}
	}
	
	private static bool forceCheck = false;
	//当网络断开时检查是否需要重连
	static public void CheckReloginWhenConnectClose(bool forceCheck_ = false){
		forceCheck = forceCheck_;
		
		if (forceCheck){
			reLogining = true;
		}
		
		if (reLogining == true){
			CheckConnected = true;	
		}
	}
}
