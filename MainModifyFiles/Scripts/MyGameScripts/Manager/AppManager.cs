// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AppManager.cs
// Author   : SK
// Created  : 2013/1/30
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.dto;

public class AppManager
{
    public static bool NewPlayerMode = false;
	public static bool SkillConfigMode = false;
	
	private static readonly AppManager instance = new AppManager();
	public static AppManager Instance
	{
		get
		{
			return instance;
		}
	}

	public static void InitAppManager()
	{
		Instance.ReInitAppManager();
	}
	
	private AppManager(){
	}

	public void ReInitAppManager()
	{
		if (AppManager.NewPlayerMode)
		{
			GotoCreatePlayerScene();			
		}
		else
		{
			InitUserData();
			StartGame();
		}
	}

	private void GotoCreatePlayerScene(){
		Debug.Log("进入创建角色场景");
	}


	//初始化用户数据
	private void InitUserData()
	{
	}

    private void StartGame()
    {
		ProxyMainUIModule.Open();
		JoystickModule.Instance.Setup();
		
		if (PlayerModel.Instance.GetPlayer().curGameVideo != null)
		{	
			BattleManager.Instance.PlayBattle(PlayerModel.Instance.GetPlayer().curGameVideo);
		}
		else
		{
			WorldManager.Instance.Enter();
		}

		TipManager.AddTip(string.Format("欢迎进入{0}服务器", ServerManager.Instance.GetServerInfo().name.WrapColor(ColorConstant.Color_Name)));
		
		ReserveExpDto reserveExpDto = PlayerModel.Instance.ReserveExpDto;
		if (reserveExpDto != null)
		{
			string reserveExpTip = string.Format("距上次离线时间{0}分钟，共获得{1}储备经验。详情打开人物属性界面点击经验条查询", reserveExpDto.minutes, reserveExpDto.value);
			TipManager.AddTip(reserveExpTip);
			PlayerModel.Instance.ReserveExpDto = null;
		}
	}
}