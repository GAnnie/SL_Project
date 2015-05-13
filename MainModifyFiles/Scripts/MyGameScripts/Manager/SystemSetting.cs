// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SystemSetting.cs
// Author   : senkay
// Created  : 6/19/2013 3:47:51 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetting
{
	
#if UNITY_IPHONE		
	public const int LIMIT_MEMORY = 50;
#else
	public const int LIMIT_MEMORY = 80;
#endif
	
    //是否启用缓存机制,包括对象池，Asset的缓存等用来加速游戏体验的机制,缺点是会增加内存使用
    public static bool UsePool = false;

    //是否启用特效显示
    public static bool ShowEffect = true;

    //是否显示其它玩家
    public static bool ShowPlayer = true;

    //是否使用背景音乐
    public static bool EnableMusic = true;

    //是否使用音效
    public static bool EnableSound = true;
	
	//是否显示宠物跟随
	public static bool ShowFollowPet = true;

	//指令下达后自动开战
	public static bool AutoBattle = false;

	//是否低内存模式
	public static bool LowMemory = false;

	public static bool IsMobileRuntime
	{
		get{
			return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
		}
	}

	public static void CheckMemoryMode()
	{
		if (IsMobileRuntime)
		{
			long memory = BaoyugameSdk.getFreeMemory() / 1024;	
			if (memory < LIMIT_MEMORY){
				LowMemory = true;
			}else{
				LowMemory = false;
			}
		}

		//WorldView.MAX_PLAYER_COUNT = LowMemory?WorldView.MAX_PLAYER_COUNT_LOW:WorldView.MAX_PLAYER_COUNT_HIGH;
		GameObjectDisplayManager.Instance.Setup(LowMemory);
	}
	
    public static void Setup()
    {
        InitVolume();
        InitSystemSetting();
    }

    private static void InitVolume()
    {
		string musicVolume = PlayerPrefsExt.GetLocalString("MusicVolume");
        if (string.IsNullOrEmpty(musicVolume) == false)
        {
            //AudioManager.Instance.MusicVolume = float.Parse(musicVolume);
        }else{
			//AudioManager.Instance.MusicVolume = 0.7f;
		}

        string soundVolume = PlayerPrefsExt.GetLocalString("SoundVolume");
        if (string.IsNullOrEmpty(soundVolume) == false)
        {
            //AudioManager.Instance.SoundVolume = float.Parse(soundVolume);
        }else{
			//AudioManager.Instance.SoundVolume = 0.7f;
		}
		
		//UIButtonSound.globalVolume = AudioManager.Instance.SoundVolume;
    }

    private static void InitSystemSetting()
    {		
		//WorldManager.KeepSceneMode = !SystemSetting.LowMemory;
		
//        string showEffect = PlayerPrefsExt.GetLocalString("ShowEffect");
//        if (string.IsNullOrEmpty(showEffect) == false)
//        {
//            SystemSetting.ShowEffect = bool.Parse(showEffect);
//        }else{		
//			SystemSetting.ShowEffect = !LowMemory;
//		}

//        string showPlayer = PlayerPrefsExt.GetLocalString("ShowPlayer");
//        if (string.IsNullOrEmpty(showPlayer) == false)
//        {
//            SystemSetting.ShowPlayer = bool.Parse(showPlayer);
//        }else{
//			SystemSetting.ShowPlayer = !LowMemory;
//		}

        string usePool = PlayerPrefsExt.GetLocalString("UsePool");
        if (string.IsNullOrEmpty(usePool) == false)
        {
            SystemSetting.UsePool = bool.Parse(usePool);
        }
		
		string _followPet = PlayerPrefsExt.GetLocalString("ShowFollowPet");
		if(string.IsNullOrEmpty(_followPet) == false)
		{
			SystemSetting.ShowFollowPet = bool.Parse(_followPet);
		}
		else
		{
			SystemSetting.ShowFollowPet = !LowMemory;
		}
		
    }

	//调整游戏速度
	public static void SetTimeScale(bool inBattle)
	{
//		//如果是测试加速的速度， 则不要变更
//		if (Time.timeScale > 1.4f){
//			return;
//		}
//
//		if (inBattle){
//			Time.timeScale = 1.4f;
//		}else{
//			Time.timeScale = 1f;
//		}
	}
}
