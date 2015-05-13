// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleInstController.cs
// Author   : SK
// Created  : 2013/3/8
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BattleInstController
{
	private BattleController _battleController;
	private List<VideoAction> actionList;
	private Dictionary<Type, Type> typeMap;
	private Dictionary<Type, BaseBattleInstPlayer> pool;
	
	private Video _gameVideo;
	private List<VideoRound> _videoRounds;

	private int instIndex;
	private VideoAction _currentInst;
	private BaseBattleInstPlayer currentInstPlayer;
	
	private int _playRoundIndex = 0; //播放回合序号

	// Use this for initialization
	public void Start (BattleController battleController)
	{
		_battleController = battleController;

		actionList = new List<VideoAction> ();
		
		typeMap = new Dictionary<Type, Type>();
		
		typeMap.Add (typeof(VideoAction), typeof(GameVideoActionPlayer));
		typeMap.Add (typeof(VideoSkillAction), typeof(GameVideoGeneralActionPlayer));
		
//		typeMap.Add (typeof(GameVideoBuffAction), typeof(GameVideoBuffActionPlayer));
//		typeMap.Add (typeof(GameVideoCatchPetAction), typeof(GameVideoCatchPetActionPlayer));
//		typeMap.Add (typeof(CheckCatchPetResultAction), typeof(CatchPetResultPlayer));
//		typeMap.Add (typeof(GameVideoFleeFailedAction), typeof(GameVideoFleeFailedActionPlayer));
//		typeMap.Add (typeof(GameVideoApplyItemAction), typeof(GameVideoApplyItemActionPlayer));
//		typeMap.Add (typeof(GameVideoReadyTrickAction), typeof(GameVideoReadyTrickActionPlayer));
//		typeMap.Add (typeof(GameVideoRoundTrickAction), typeof(GameVideoRoundTrickActionPlayer));
//		typeMap.Add (typeof(GameReport), typeof(GameReportPlayer));
//		typeMap.Add (typeof(GameVideoStopAction), typeof(GameVideoStopActionPlayer));
//		typeMap.Add (typeof(GameVideoPromoteDefenseAction), typeof(GameVideoPromoteDefenseActionPlayer));
//        typeMap.Add (typeof(DrawResult), typeof(DrawResultPlayer));
//		typeMap.Add (typeof(GameVideoLineupAction), typeof(GameVideoLineupActionPlayer));
//		typeMap.Add (typeof(DrawAction),typeof( GameDrawPlayer ) );
//		typeMap.Add (typeof(CheckBattleMonsterAction), typeof(CheckBattleMonsterActionPlayer));
//		typeMap.Add (typeof(GameVideoTipsAction ) , typeof ( GameVideoTipsActionPlayer )   );
		
		pool = new Dictionary<Type, BaseBattleInstPlayer>();

		_videoRounds = new List<VideoRound>();

		instIndex = 0;
		_playing = false;
	}
	
	public void PlayGameVideo(Video gv){
		_gameVideo = gv;
		
		_playRoundIndex = 0;

		CheckNextRound();
	}
	
	
	/**
	 *是否自动播放模式 
	 * @return
	 * 
	 */		
	public bool IsAutoPlayMode()
	{
		return false;
	}	
	
	public bool PlayNextRound()
	{
		if (_videoRounds.Count > _playRoundIndex){
			VideoRound videoRound = _videoRounds[_playRoundIndex];
			_playRoundIndex++;

			PlayGameRound(videoRound);
			return true;
		}else{
			return false;
		}
	}
	
	public void PlayGameRound(VideoRound gameRound)
	{
        _battleController.SetBattleStat(BattleController.BattleSceneStat.ON_PROGRESS);

		if (gameRound.readyAction.targetStateGroups.Count > 0)
		{
			actionList.Add(gameRound.readyAction);
		}

		for (int i=0, count=gameRound.skillActions.Count; i<count; i++)
		{
			VideoAction action = gameRound.skillActions[i] as VideoAction;
			actionList.Add(action);
		}

		if (gameRound.endAction.targetStateGroups.Count > 0)
		{
			actionList.Add(gameRound.endAction);
		}

        _battleController.UpdateBattleRound(gameRound);
		
		GameDebuger.Log("回合:"+gameRound.count + " winId=" + gameRound.winId + " isGameOver="+gameRound.over);

		LogBattleInfo (gameRound.debugInfo);

		if (_playing == false){
			PlayNextInst();
		}
	}

	private void LogBattleInfo(DebugVideoRound debugInfo)
	{
		if (debugInfo != null && GameDebuger.debugIsOn)
		{
			GameDebuger.LogBattleInfo ("回合:" + debugInfo.round);
			GameDebuger.LogBattleInfo ("准备信息:");
			foreach(string info in debugInfo.readyInfo)
			{
				GameDebuger.LogBattleInfo (info);
			}
			GameDebuger.LogBattleInfo ("过程信息:");
			foreach(string info in debugInfo.progressInfo)
			{
				GameDebuger.LogBattleInfo (info);
			}
		}
	}

	private bool _playing = false;

	public void PlayNextInst ()
	{
        _currentTargetId = 0;

		if (instIndex < actionList.Count){
			_playing = true;
			
			_currentInst = actionList[instIndex];
			instIndex ++;
			currentInstPlayer = GetInstPlayer(_currentInst);
			
			if (currentInstPlayer != null){
				currentInstPlayer.Excute(_currentInst);
			}else{
				Debug.LogError("不能找到对应的ActionPlayer " + _currentInst.ToString());
			}
		}
		else
		{
            _playing = false;
			_battleController.CheckGameState();	
		}
	}

	public void CheckFinish()
	{
		if (currentInstPlayer != null)
		{
			currentInstPlayer.CheckFinish ();
		}
	}

	//check can start next round when gamestate is ready
	public void CheckNextRound()
	{
		if (_playing == true)
		{
			return;
		}

//		if (_battleController.NeedSwitchHero())
//		{
//			_battleController.AutoSwitchHero();
//			return;
//		}

		bool playNextRound = PlayNextRound();

		if (playNextRound == false)
		{
			_battleController.SetReadyState();
		}
	}

	private void DoRoundReadyAction()
	{
//		VideoRoundAction action = _battleController.GetBattle().GetRoundAction(true);
//		if (action != null && action.targetStates.Count > 0){
//			actionList.Add(action);
//		}
	}

	private bool haveDoRoundEndAction = false;
	private void DoRoundEndAction()
	{
//		VideoRoundAction action = _battleController.GetBattle().GetRoundAction(false);
//		if (action != null && action.targetStates.Count > 0)
//		{
//			haveDoRoundEndAction = true;
//
//			actionList.Add(action);
//			PlayNextInst();
//		}
//		else
//		{
//			haveDoRoundEndAction = false;
//
//			_battleController.CheckGameState();
//		}
	}

	public VideoAction GetCurrentInst(){
		return _currentInst;
	}

    private long _currentTargetId = 0;
	
	private BaseBattleInstPlayer GetInstPlayer(VideoAction action)
	{
		Type type = action.GetType();
		BaseBattleInstPlayer instPlayer = null;
		pool.TryGetValue(type, out instPlayer);
		if (instPlayer == null){
			if (typeMap.ContainsKey(type) == false){
				Type playerType = typeMap[typeof(VideoAction)];
				BaseBattleInstPlayer player = (BaseBattleInstPlayer)Activator.CreateInstance(playerType);
				player.Setup(this);
				return player;
			}else{
				Type playerType = typeMap[type];
				BaseBattleInstPlayer player = (BaseBattleInstPlayer)Activator.CreateInstance(playerType);
				player.Setup(this);
				return player;
			}
		}else{
			return instPlayer;
		}
	}
	
	public BattleController GetBattleController()
	{
		return _battleController;
	}
	
	public void FinishInst ()
	{
        _battleController.ResetPetMessageState();

		if (actionList != null) {
			PlayNextInst ();
		}
	}

	public void AddVideoRound(VideoRound videoRound)
	{
		_videoRounds.Add(videoRound);
	}

	//马上出战斗结果
	public void ShowBattleResult()
	{
		int lastRoundIndex = _videoRounds.Count;
		if (_playRoundIndex != lastRoundIndex){
			_playRoundIndex = lastRoundIndex-1;
			VideoRound gameRound = _videoRounds[_playRoundIndex];
			gameRound.over = true;

			gameRound.skillActions.Clear();
			actionList.Clear();
			instIndex = 0;
			CheckNextRound();
		}else{
			instIndex = actionList.Count-1;
		}
	}

	public void Destroy()
	{
		
	}
}

