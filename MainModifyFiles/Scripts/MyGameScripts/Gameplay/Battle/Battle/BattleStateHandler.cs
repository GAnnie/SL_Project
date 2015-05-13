using UnityEngine;
using System.Collections;
//using com.nucleus.h1.logic.core.modules.game.dto;
//using com.nucleus.h1.logic.core.modules.buff.dto;
using System.Collections.Generic;
using System;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BattleStateHandler
{

	public static void PlayVideoSkillAction(BattleController bc, MonsterController mc, VideoSkillAction action)
	{
		if (mc != null)
		{
			mc.ClearMessageEffect(true);
			mc.modifyHP = action.hpSpent;
			mc.modifyMP = action.mpSpent;
			mc.modifySP = action.spSpent;
			mc.PlayInjure();
		}
	}

	public static void PlayState( BattleController bc, MonsterController mc, VideoTargetState bas)
	{
		if (mc != null)
		{
			mc.ClearMessageEffect(true);
		}

		if (bas is VideoActionTargetState)
		{
			VideoActionTargetState action = (VideoActionTargetState)bas;

			if ( action.crit )
				mc.AddMessageEffect( MonsterController.ShowMessageEffect.CRITICAL );

			mc.modifyHP = action.hp;
			mc.modifyMP = action.mp;
			mc.modifySP = action.sp;
		}
		else if (bas is VideoDodgeTargetState)
		{
			mc.AddMessageEffect( MonsterController.ShowMessageEffect.DODGE );
		}
		else if (bas is VideoBuffAddTargetState)
		{
			mc.AddBuffState( (VideoBuffAddTargetState)bas );
		}
		else if (bas is VideoBuffRemoveTargetState)
		{
			mc.RemoveBuffs( (VideoBuffRemoveTargetState)bas );
		}
		else if (bas is VideoRageTargetState)
		{
			VideoRageTargetState rageTargetState = bas as VideoRageTargetState;
		}
		else if (bas is VideoCallSoldierLeaveState)
		{
			VideoCallSoldierLeaveState rageTargetState = bas as VideoCallSoldierLeaveState;
			MonsterController leaveSoldier = bc.GetMonsterFromSoldierID (rageTargetState.id);
			if (leaveSoldier != null)
			{
				leaveSoldier.LeaveBattle();
			}
			return;
		}
		else if (bas is VideoTargetExceptionState)
		{
			VideoTargetExceptionState targetExceptionState = bas as VideoTargetExceptionState;
			TipManager.AddTip(targetExceptionState.message);
		}

		if (mc != null)
		{
			mc.dead = bas.dead;
			mc.leave = bas.leave;
			mc.PlayInjure();
		}
	}

	static public void HanderOtherTargetState(List<long> checkList, List<VideoTargetState> targetStates, BattleController bc){
		
		List<VideoTargetState> states = new List<VideoTargetState>();
		
		foreach(VideoTargetState state in targetStates){
			if(!checkList.Contains(state.id))
			{
				states.Add(state);
			}
		}
		
		foreach(VideoTargetState doState in states){
			PlayState(bc, bc.GetMonsterFromSoldierID(doState.id), doState);
		}		
	}

	static public void HandleBattleStateGroup(
		long petId, 
		List<VideoTargetStateGroup> targetStateGroups, 
		BattleController bc)
	{
		foreach (VideoTargetStateGroup group in targetStateGroups)
		{
			HandleBattleState(petId, group.targetStates, bc);
		}
	}

//	static public void CheckDeadStates(List<VideoTargetState> targetStates)
//	{
//		foreach (VideoTargetState state in targetStates)
//		{
//			CheckDeadState(state);
//		}
//	}

	static public void CheckDeadState(MonsterController mc, VideoTargetState bas)
	{
		float modifyHP = 0;
		if (bas is VideoActionTargetState)
		{
			VideoActionTargetState action = (VideoActionTargetState)bas;
			
			modifyHP = action.hp;
		}
		
		if (mc != null)
		{
			mc.dead = bas.dead;
			mc.leave = bas.leave;
			
			if (mc.currentHP + modifyHP <= 0)
			{
				mc.dead = true;
			}
			else
			{
				mc.dead = false;
			}
		}
	}

	static public void HandleBattleState(
		long petId, 
		List<VideoTargetState> targetStates, 
		BattleController bc)
	{
		HandleAllBattleState(targetStates, bc);
//		if (petId == 0) 
//		{
//			HandleAllBattleState(targetStates, bc);
//		}
//		else 
//		{
//			List<VideoTargetState> newStates = getTargetStates(targetStates, petId);
//			HandleAllBattleState(newStates, bc);
//		}
	}

	static public void HandleAllBattleState(
		List<VideoTargetState> targetStates, 
		BattleController bc)
	{
		foreach(VideoTargetState state in targetStates)
		{
			if (state.id == 0)
			{
				Debug.LogError("state.id = 0");
			}
			PlayState(bc, bc.GetMonsterFromSoldierID(state.id), state);
		}
	}

	public static List<VideoTargetState> getTargetStates(List<VideoTargetState> arr, long petId)
	{
		List<VideoTargetState> states = new List<VideoTargetState>();
		foreach(VideoTargetState state in arr)
		{
			if (petId == 0)
			{
				states.Add(state);
			}
			else
			{
				if(state.id == petId)
				{
					states.Add(state);
				}
			}
		}
		return states;
	}

	public static VideoTargetState getTargetState(List<VideoTargetState> arr, long petId)
	{
		VideoTargetState select = null;
		int count = 0;
		foreach(VideoTargetState state in arr)
		{
			if(state.id == petId)
			{
				if (select == null)
				{
					select = state;
				}
				count++;
			}
		}

		if (select != null && count > 1)
		{
			//这里因为要针对不同的攻击次数做状态处理， 所以把相同状态旧的删掉，只保留最后一个做后续的死亡判断
			arr.Remove(select);
		}

		return select;
	}
}
