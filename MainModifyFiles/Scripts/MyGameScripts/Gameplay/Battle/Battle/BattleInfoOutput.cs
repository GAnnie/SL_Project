// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleInfoOutput.cs
// Author   : SK
// Created  : 2013/3/5
// Purpose  : 战斗信息输出
// **********************************************************************

using UnityEngine;
using System.Collections;
//using com.nucleus.h1.logic.core.modules.game.dto;
using System.Collections.Generic;
//using com.nucleus.h1.logic.core.modules.buff.dto;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BattleInfoOutput
{
	private static readonly BattleInfoOutput instance = new BattleInfoOutput();
    public static BattleInfoOutput Instance
    {
        get
		{
			return instance;
		}
    }
//	
//	public void ShowGameVideoApplyItemAction(GameVideoApplyItemAction gameAction, BattleController bc)
//	{
//		GameDebuger.Log("物品效果:");
//		ShowGameVideoStateAction(gameAction, bc);
//	}
//	
//	public void ShowGameVideoBuffAction(GameVideoBuffAction gameAction, BattleController bc)
//	{
//		GameDebuger.Log("Buff效果:");
//		ShowGameVideoStateAction(gameAction, bc);
//	}
//	
//	public void ShowGameVideoTrickAction(GameVideoStateAction gameAction, BattleController bc)
//	{
//		GameDebuger.Log("特性效果:");
//		ShowGameVideoStateAction(gameAction, bc);
//	}
//
	public void ShowVideoAction(VideoAction gameAction, BattleController bc)
	{
		GameDebuger.Log("基础效果:");
		ShowGameVideoStateAction(gameAction, bc);
	}

	public void ShowGameVideoStateAction(VideoAction gameAction, BattleController bc){
		GameDebuger.Log( GetTargetStateGroupsInfo(gameAction, bc) );
	}	

	public void showVideoSkillAction(VideoSkillAction gameAction, BattleController bc, bool strikeBack = false)
	{
		if (gameAction.skill == null){
			return;
		}
		
		MonsterController monster = bc.GetMonsterFromSoldierID(gameAction.actionSoldierId);
		if (monster == null){
			GameDebuger.Log("Error: attacker not exist id="+gameAction.actionSoldierId);
			return;
		}
		
		string desc = (strikeBack?"[反击]":"")+monster.GetDebugInfo()+" 使用【"+gameAction.skill.name+"】" + ",造成" + GetTargetStateGroupsInfo(gameAction, bc);
		GameDebuger.LogBattleInfo(desc);
	}

	private string GetTargetStateGroupsInfo(VideoAction gameAction, BattleController bc)
	{
		List<string> effects = new List<string>();
		foreach(VideoTargetStateGroup targetStateGroup in gameAction.targetStateGroups){
			effects.Add( GetTargetStatesInfo(targetStateGroup, bc) );
		}
		
		return string.Join(";", effects.ToArray());
	}

	private string GetTargetStatesInfo(VideoTargetStateGroup groups, BattleController bc)
	{
		List<long> targetList = new List<long>();
		foreach(VideoTargetState targetState in groups.targetStates){
			if (targetList.Contains(targetState.id) == false){
				targetList.Add(targetState.id);
			}
		}
		
		List<string> effects = new List<string>();
		foreach(long targetId in targetList){
			string effectInfo = GetTargetEffectInfo(targetId, groups.targetStates, bc);
			if (!string.IsNullOrEmpty(effectInfo)){
				effects.Add( effectInfo );
			}
		}

		if (groups.strikeBackAction != null)
		{
			showVideoSkillAction(groups.strikeBackAction, bc, true);
		}

		return string.Join(",", effects.ToArray());
	}

	private List<VideoTargetState> getTargetStates(List<VideoTargetState> arr, long petId)
	{
		List<VideoTargetState> states = new List<VideoTargetState>();
		foreach(VideoTargetState state in arr){
			if (petId == 0){
				states.Add(state);
			}else{
				if(state.id == petId){
					states.Add(state);
				}
			}
		}
		return states;
	}	
	
	private string GetTargetEffectInfo(long targetId, List<VideoTargetState> stateList, BattleController bc)
	{
		stateList = getTargetStates(stateList, targetId);
		
		MonsterController monster = bc.GetMonsterFromSoldierID(targetId);
		if (monster == null){
			GameDebuger.Log("Error: target not exist! id="+targetId);
			return "";
		}
		
		List<string> effect = new List<string>();
		bool dead = false;
		foreach(VideoTargetState state in stateList){
			if (!dead){
				dead = state.dead;
			}
			
			if (state is VideoActionTargetState)
			{
				effect.Add( GetActionTargetState(state as VideoActionTargetState) );
			}

			if (state is VideoBuffAddTargetState)
			{
				effect.Add( GetBuffAddTargetState(state as VideoBuffAddTargetState) );
			}

			if (state is VideoBuffRemoveTargetState)
			{
				effect.Add( GetBuffRemoveTargetState(state as VideoBuffRemoveTargetState) );
			}

			if (state is VideoDodgeTargetState)
			{
				effect.Add( GetDodgeTargetState(state as VideoDodgeTargetState) );
			}

			if (state is VideoRageTargetState)
			{
				effect.Add( GetRageTargetState(state as VideoRageTargetState) );
			}

			if (state is VideoCallSoldierState)
			{
				effect.Add( GetVideoCallSoldierState(state as VideoCallSoldierState) );
			}

			if (state is VideoCallSoldierLeaveState)
			{
				effect.Add( GetVideoCallSoldierLeaveState(state as VideoCallSoldierLeaveState) );
			}

			if (state is VideoSwtichPetState)
			{
				effect.Add( GetVideoSwtichPetState(state as VideoSwtichPetState) );
			}

			if (state is VideoTargetExceptionState)
			{
				effect.Add( GetVideoTargetExceptionState(state as VideoTargetExceptionState) );
			}

			if (state is VideoRetreatState)
			{
				effect.Add( GetVideoRetreatState(state as VideoRetreatState) );
			}

			if (state is VideoCaptureState)
			{
				effect.Add( GetVideoCaptureState(state as VideoCaptureState) );
			}
		}
		
		if (dead){
			effect.Add("死亡");
		}
		if (effect.Count > 0){
			return monster.GetDebugInfo()+" "+string.Join(",", effect.ToArray());
		}else{
			return "";	
		}
	}
	
	private string GetActionTargetState(VideoActionTargetState state)
	{
		List<string> effect = new List<string>();
		
		if (state.hp != 0){
			effect.Add("HP"+state.hp);
		}
		
		return string.Join(",", effect.ToArray());
	}
//	
//	private string getAbsorbState(AbsorbState state)
//	{
//		return "吸血"+state.hp+" 吸魔"+state.mp;
//	}
//	
	private string GetDodgeTargetState(VideoDodgeTargetState state)
	{
		return "闪避";
	}

	private string GetRageTargetState(VideoRageTargetState state)
	{
		return "怒气值:" + state.rage;
	}

	private string GetVideoCallSoldierState(VideoCallSoldierState state)
	{
		return "召唤小怪:" + state.soldier.id;
	}

	private string GetVideoCallSoldierLeaveState(VideoCallSoldierLeaveState state)
	{
		return "小怪撤离:" + state.id;
	}

	private string GetVideoSwtichPetState(VideoSwtichPetState state)
	{
		return "召唤宠物:" + state.switchPetSoldier.name;
	}

	private string GetVideoTargetExceptionState(VideoTargetExceptionState state)
	{
		return "异常状态:" + state.message;
	}

	private string GetVideoRetreatState(VideoRetreatState state)
	{
		return "撤退:" + state.success;
	}

	private string GetVideoCaptureState(VideoCaptureState state)
	{
		return "捕捉:" + state.success;
	}

//	
//	private string getNumberEffectState(NumberEffectState state)
//	{
//		foreach(NumberEffectDto effect in state.numberEffectDtos){
//			if (effect.numberEffectId == NumberEffectDto.NumberEffectType_Hp){
//				return "HP"+effect.value;
//			}else{
//				return "MP"+effect.value;
//			}
//		}
//		return "";
//	}
//	
//	private string getParrySuccessState(ParrySuccessState state)
//	{
//		return "招架";
//	}
//	
//	private string getReboundState(ReboundState state)
//	{
//		return "反震hp"+state.hp;
//	}
//	
//	private string getSquelchState(SquelchState state)
//	{
//		return "反击hp"+state.hp;
//	}
//	
//	private string getProtectState(ProtectState state)
//	{
//		return "保护";
//	}
//	
	private string GetBuffAddTargetState(VideoBuffAddTargetState state)
	{
		return "Buff 剩余回合"+state.round+" id:"+state.battleBuffId+" 效果:"+state.battleBuff.name;
	}
//	
//	private string getBufferClearState(BufferClearState state)
//	{
//		return "Buff全清";
//	}
//	
	private string GetBuffRemoveTargetState(VideoBuffRemoveTargetState state)
	{
		return "Buff移除 id:"+state.buffId.ToArray().ToString();
		//return "Buff移除 id:"+string.Join(",", state.buffId.ToArray());
	}
//	
//	private string getBufferAiTypeClearState(BufferAiTypeClearState state){
//		return "Buff移除aiType id:"+state.types.ToArray().ToString();
//	}
//	
//	private string getTrickState(TrickState state){
//		return "特技 id:"+state.trickId + " name:"+state.trick.name+" 作用者:"+state.id;
//	}	
//	
//	private string getAbsorbDamageState(AbsorbDamageState state){
//		return "吸收伤害";
//	}

	public void OutputTargetStateGroups(List<VideoTargetStateGroup> groups)
	{
		string infos = "OutputTargetStateGroups=============";

		foreach (VideoTargetStateGroup group in groups)
		{
			infos += OutputTargetStates(group.targetStates);
			if (group.strikeBackAction != null)
			{
				infos += "\n\t" + "[反击]";
				infos += OutputTargetStates(group.strikeBackAction.targetStateGroups[0].targetStates);
			}
		}

		infos += "\n" + "===============OutputTargetStateGroups";
		Debug.Log(infos);
	}

	public string OutputTargetStates(List<VideoTargetState> targetStates)
	{
		string infos = "\n\t" + "OutputVideoTargetState-----------------";
		
		List<string> outputs = GetVideoTargetStates(targetStates);
		
		foreach(string info in outputs){
			infos += "\n\t" + info;
		}
		
		infos += "\n\t" + "-----------------OutputVideoTargetState";

		return infos;
	}
	
	private List<string> GetVideoTargetStates(List<VideoTargetState> targetStates)
	{
		List<string> outputs = new List<string>();
		foreach(VideoTargetState state in targetStates)
		{
			outputs.Add(state.ToString());
		}
		
		return outputs;
	}
}

