﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  LayerManager.cs
// Author   : SK
// Created  : 2013/1/30
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class UILayerType
{
	#region 玩法功能模块层级
	public const int BaseModule = 10;//底层模块
	
	public const int DefaultModule = 20; //模块
	
	public const int SubModule = 30; //2层子模块
	
	public const int ThreeModule = 40; //3层子模块
	
	public const int FourModule = 50; //4层子模块	
	#endregion
	
	public const int Guide= 60; //引导层
	
	public const int Plot = 70; //剧情对话层

	public const int FloatTip = 80; //飘窗提示
	
	public const int FadeInOut = 90; //黑屏过渡

	public const int LockScreen = 100; //锁屏

	public const int Dialogue = 110; //对话框

	public const int RenderQueue_UIEffect = 3080; //
}

public class GameTag
{
	public const string Tag_WorldActor = "WorldActor";

	public const string Tag_BattleActor = "BattleActor";

	public const string Tag_Terrain = "Terrain";

	public const string Tag_UI = "UI";

	public const string Tag_Npc = "Npc";

	public const string Tag_Teleport = "Teleport";

	public const string Tag_Player = "Player";

	public const string Tag_Default = "Default";
}

public class UIMode
{
	public const string Mode_Game = "Game";
	
	public const string Mode_Battle = "Battle";
	
	public const string Mode_Story = "Story";
}

public class LayerManager:MonoBehaviour
{
	public static GameObject rootUI = null;
	
	public GameObject frontAnchor = null;

	public GameObject sceneHudTextAnchor;
	public GameObject storyHudTextAnchor;
	public GameObject battleHudTextAnchor;

	public GameObject floatTipAnchor;

	public GameObject uiModuleRoot = null;

	public GameObject audioManager = null;
	
	public Camera GameCamera = null;

	public Camera BattleCamera = null;
	
	public Camera UICamera = null;

	public Camera NameUICamera = null;
	public GameObject NameUIAnchor;
	public GameObject NameUIBgSprite;

	public Animator GameCameraAnimator = null;
	
	public GameObject GameCameraGo = null;
	
	public GameObject DynamicObjects = null;
	
	public GameObject Gameplay = null;
	public GameObject BattleActors = null;
	public GameObject WorldActors = null;
	public GameObject StoryActors = null;
	public GameObject EffectsAnchor = null;
	
	public GameObject Management = null;
	
	public GameObject World = null;
	// 场景 是world字节点
	public GameObject SceneLayer = null;
	// 战斗 是world字节点
	public GameObject BattleLayer = null;

	public GameObject GameUpdateGroup = null;
	public UILabel GameReadyTipLabel = null;
	public UITexture InitBgTextture = null;
	
	static public bool UILOCK = false;

	private static LayerManager _instance = null;
	public static LayerManager Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
		
		rootUI = this.gameObject;
	}
	
	void Start()
	{
		//LayerManager.Instance.GameCamera.audio.volume = 0.2f;
	}

	public void SwitchLayerMode(string mode)
	{
		if (mode == UIMode.Mode_Battle)
		{
			AudioManager.Instance.PlayMusic ("music_battle/music_boss_1");
		}
		else
		{
			AudioManager.Instance.PlayMusic ("music_world/music_wonderland_1");
		}


		if (MainUIViewController.Instance != null)
		{
			MainUIViewController.Instance.ChangeMode(mode);
		}
		JoystickModule.Instance.SetActive(mode == UIMode.Mode_Game);

		BattleLayer.SetActive (mode == UIMode.Mode_Battle);
		SceneLayer.SetActive (mode != UIMode.Mode_Battle || !BattleManager.NeedBattleMap);

		BattleActors.SetActive (mode == UIMode.Mode_Battle);
		WorldActors.SetActive (mode == UIMode.Mode_Game);
		StoryActors.SetActive (mode == UIMode.Mode_Story);

		BattleCamera.gameObject.SetActive (mode == UIMode.Mode_Battle);
		NameUICamera.gameObject.SetActive (mode == UIMode.Mode_Battle);
		NameUIBgSprite.SetActive (!BattleManager.NeedBattleMap);

		battleHudTextAnchor.SetActive(mode == UIMode.Mode_Battle);
		sceneHudTextAnchor.SetActive(mode == UIMode.Mode_Game);
		storyHudTextAnchor.SetActive(mode == UIMode.Mode_Story);

		AdjustCameraPosition.ChangeCMode (mode == UIMode.Mode_Battle);

		if (mode==UIMode.Mode_Battle && !BattleManager.NeedBattleMap)
		{
			EffectsAnchor.layer = LayerMask.NameToLayer(GameTag.Tag_BattleActor);
		}
		else
		{
			EffectsAnchor.layer = LayerMask.NameToLayer(GameTag.Tag_Default);
		}
	}

	public Camera GetBattleFollowCamera()
	{
//		if (BattleManager.NeedBattleMap)
//		{
//			return GameCamera;
//		}
//		else
//		{
//			return BattleCamera;
//		}

		return BattleCamera;
	}
}

