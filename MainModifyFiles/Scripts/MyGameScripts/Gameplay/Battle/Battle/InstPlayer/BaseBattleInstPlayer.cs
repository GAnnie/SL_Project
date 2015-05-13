// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaseBattleInstPlayer.cs
// Author   : SK
// Created  : 2013/3/8
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using com.nucleus.h1.logic.core.modules.game.dto;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BaseBattleInstPlayer
{
	protected BattleInstController _instController;
	
	public void Setup(BattleInstController controller){
		_instController = controller;
	}
	
	private VideoAction _action;
	virtual public void Excute (VideoAction action)
	{
		_action = action;
		DoExcute(action);
	}
	
	virtual public void DoExcute (VideoAction action)
	{
	}	
	
	virtual public void Destroy ()
	{
	
	}
	
	virtual public void CheckFinish(){
	}
	
	virtual public void PlayInjureAction(List<long> ids){
		
	}
	
	protected void CheckMonsterDead(){
		List<long> monsterList = new List<long>();
		
		foreach(VideoTargetStateGroup stateGroup in _action.targetStateGroups){
			foreach(VideoTargetState state in stateGroup.targetStates)
			{
				if (monsterList.Contains(state.id) == false){
					monsterList.Add(state.id);
				}
			}
		}
		
		foreach(long monsterId in monsterList){
			MonsterController monster = _instController.GetBattleController().GetMonsterFromSoldierID(monsterId);
			if (monster != null){
				if (monster.IsDead()){
					monster.PlayDieAnimation();
				}else{
					monster.PlayStateAnimation();
				}
			}
		}
	}
	
	virtual public void Finish()
	{
		CheckMonsterDead();
		
		_instController.FinishInst();
	}
	
	protected void DelayFinish(float delayTime){
		_instController.GetBattleController().GetMonoTimer().Setup2Time(delayTime, OnMonoTimer);
		_instController.GetBattleController().GetMonoTimer().Play();			
	}
	
	private void OnMonoTimer()
	{
		_instController.GetBattleController().GetMonoTimer().Stop();
		_instController.GetBattleController().GetMonoTimer().RemoveHandler(OnMonoTimer);
		Finish();
	}	
}

