// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GameVideoActionPlayer.cs
// Author   : SK
// Created  : 2014/3/14
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class GameVideoActionPlayer : BaseBattleInstPlayer
{
	
	override public void DoExcute (VideoAction inst)
	{
		VideoAction action = inst as VideoAction;
		BattleInfoOutput.Instance.ShowVideoAction(action, _instController.GetBattleController());
		
		BattleStateHandler.HandleBattleStateGroup( 0, action.targetStateGroups, _instController.GetBattleController());
		
		DelayFinish(0.5f);
	}
}

