// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GameVideoGeneralActionPlayer.cs
// Author   : SK
// Created  : 2013/3/8
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
//using com.nucleus.h1.logic.core.modules.game.dto;
using com.nucleus.h1.logic.core.modules.player.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.battle.data;

public class GameVideoGeneralActionPlayer : BaseBattleInstPlayer
{

	private VideoSkillAction  _gameAction;
	private SkillConfigInfo _skillConfig;
	
	private BattleActionPlayer _attackActionPlayer;
	private Dictionary<long, BattleActionPlayer> _injureActionPlayerDic;

	private int _totalWaitingRound = 0;
	private int _currentFinishRound = 0;

	private int _totalWaitingCount = 0;
	private int _currentFinishCount = 0;

	private List<long> _injureIds = null;

	private Skill _skill;

	private StrikeBackActionPlayer _strikeBackActionPlayer;
	
	override public void DoExcute (VideoAction inst)
	{
		_gameAction = inst as VideoSkillAction;
		DoSkillBeforeActions();
		DoSkillAction();
	}

	private void DoSkillBeforeActions()
	{
		foreach (VideoAction action in _gameAction.beforeActions)
		{
			BattleInfoOutput.Instance.ShowVideoAction(action, _instController.GetBattleController());
			BattleStateHandler.HandleBattleStateGroup( 0, action.targetStateGroups, _instController.GetBattleController());
		}
	}

	private void DoSkillEndActions()
	{
		foreach (VideoAction action in _gameAction.afterActions)
		{
			BattleInfoOutput.Instance.ShowVideoAction(action, _instController.GetBattleController());
			BattleStateHandler.HandleBattleStateGroup( 0, action.targetStateGroups, _instController.GetBattleController());
		}
	}

	private string GetSkillStatusDesc(int code)
	{
		switch(code)
		{
			case Skill.SkillActionStatus_SkillApplyHpExceeded:
			return "气血过多无法施放技能";
			case Skill.SkillActionStatus_SkillApplyHpNotEnough:
			return "气血不足无法施放技能";
			case Skill.SkillActionStatus_SkillApplyMpNotEnough:
			return "魔法不足无法施放技能";
			case Skill.SkillActionStatus_SkillApplySpNotEnough:
			return "愤怒不足无法施放技能";
			case Skill.SkillActionStatus_ActionBanned:
			return "行动被封印";
			case Skill.SkillActionStatus_Defense:
			return "防御";
			case Skill.SkillActionStatus_CatchPetFailure:
			return "不能捕捉宠物";
			case Skill.SkillActionStatus_NoMoreMonsterCall:
			return "我方只能存在一只小妖";
			case Skill.SkillActionStatus_ForDeadTarget:
			return "目标不需要复活";
			case Skill.SkillActionStatus_ForLiveTarget:
			return "目标已倒地，无法对其使用该药物";
			default:
			return "";
		}
	}

	private void DoSkillAction()
	{
		BattleInfoOutput.Instance.OutputTargetStateGroups (_gameAction.targetStateGroups);

		BattleInfoOutput.Instance.showVideoSkillAction(_gameAction, _instController.GetBattleController());		

		//空的施放者
		if (_gameAction.actionSoldierId == 0)
		{
			Debug.LogError("DoSkillAction attacker is null");
			BattleStateHandler.HandleBattleStateGroup( 0, _gameAction.targetStateGroups, _instController.GetBattleController());
			DelayFinish(0.5f);
			return;
		}

		if (_gameAction.skillId == BattleManager.GetDefenseSkillId())
		{
			Finish();
			return;
		}

		if (_gameAction.skillId == BattleManager.GetProtectSkillId())
		{
			Finish();
			return;
		}

		MonsterController attacker = _instController.GetBattleController().GetMonsterFromSoldierID(_gameAction.actionSoldierId);

		if (attacker == null)
		{
			Debug.LogError("attacker is null id = " + _gameAction.actionSoldierId);
			Finish();
			return;
		}

		if (_gameAction.skillStatusCode != Skill.SkillActionStatus_Ordinary)
		{
			Debug.Log(attacker.GetDebugInfo() + " " + GetSkillStatusDesc(_gameAction.skillStatusCode));
			if (attacker.IsPlayerCtrlCharactor())
			{
				TipManager.AddTip( GetSkillStatusDesc(_gameAction.skillStatusCode) );
				DelayFinish(0.5f);
			}
			else
			{
				Finish();
			}
			return;
		}
		
		_skill = DataCache.getDtoByCls<Skill>(_gameAction.skillId);

		int skillConfigId = _skill.id;

		if (skillConfigId == BattleManager.GetUseItemSkillId())
		{
			VideoActionTargetState actionTargetState = GetActionTargetState();
			if (actionTargetState != null)
			{
				if (actionTargetState.hp != 0)
				{
					skillConfigId = 102;
				}
				else if (actionTargetState.mp != 0)
				{
					skillConfigId = 103;
				}
				else if (actionTargetState.sp != 0)
				{
					skillConfigId = 104;
				}
				else
				{
					skillConfigId = 102;
				}
			}

			_instController.GetBattleController().AddItemUsedCount();
		}

		if (skillConfigId < 10)
		{
			skillConfigId = _skill.clientSkillType;
		}

		_skillConfig = BattleConfigManager.Instance.getSkillConfigInfo(skillConfigId);

		if (_skillConfig == null)
		{
			skillConfigId = _skill.clientSkillType;
			_skillConfig = BattleConfigManager.Instance.getSkillConfigInfo(skillConfigId);
		}

		if (_skillConfig == null){
			Debug.LogError(string.Format("无技能配置{0}， 使用默认的配置{1}", skillConfigId, 1));
			_skillConfig = BattleConfigManager.Instance.getSkillConfigInfo(1);
		}
		
		if (_skillConfig == null)
		{
			Debug.LogError("技能配置为空");
			return;
		}

		if (_injureActionPlayerDic == null)
		{
			_injureActionPlayerDic = new Dictionary<long, BattleActionPlayer>();
		}

		_totalWaitingRound = 0;
		_currentFinishRound = 0;
		_totalWaitingCount = 0;
		_currentFinishCount = 0;

		_waintingAttackFinish = false;

		_injureIds = GetInjureIds (_gameAction, _skill.atOnce);

		_totalWaitingCount += 1;
		
		if (_injureIds.Contains (_gameAction.actionSoldierId)) 
		{
			_waintingAttackFinish = true;
		}

		bool hasSameInjurer = false;
		foreach (long id in _injureIds)
		{
			if (_injureActionPlayerDic.ContainsKey(id) == false)
			{
				BattleActionPlayer player = _instController.GetBattleController().gameObject.AddComponent<BattleActionPlayer>();
				_injureActionPlayerDic.Add(id, player);
				player.Init(this, _instController, _gameAction, false, _skillConfig, id, IsPlayerSideAction(attacker), _skill.atOnce);
			}
			else
			{
				hasSameInjurer = true;
			}
		}

		if (_attackActionPlayer == null){
			_attackActionPlayer = _instController.GetBattleController().gameObject.AddComponent<BattleActionPlayer>();
			_attackActionPlayer.Init(this, _instController, _gameAction, true, _skillConfig, _gameAction.actionSoldierId, IsPlayerSideAction(attacker), _skill.atOnce);
			_attackActionPlayer.SetInjureInfo(_injureIds, hasSameInjurer);
		}

		if (_injureIds.Count == 0)
		{
			_totalWaitingRound = 1;
		}
		else
		{
			if (_skill.atOnce)
			{
				_totalWaitingRound = 1;
				_totalWaitingCount += _injureIds.Count;

				if (hasSameInjurer)
				{
					TipManager.AddTip("配置或者服务器下发有异常， 有可能卡住");
					Debug.LogError("配置或者服务器下发有异常， 有可能卡住");
				}
			}
			else
			{
				_totalWaitingRound += _injureIds.Count;
				_totalWaitingCount += 1;
			}
		}

		ShowSkillName(_gameAction.skill);
	}

	private VideoActionTargetState GetActionTargetState()
	{
		if (_gameAction.targetStateGroups.Count > 0)
		{
			for (int i=0,len=_gameAction.targetStateGroups[0].targetStates.Count; i<len; i++)
			{
				VideoTargetState state = _gameAction.targetStateGroups[0].targetStates[i];
				if (state is VideoActionTargetState)
				{
					return state as VideoActionTargetState;
				}
			}

			return null;
		}
		else
		{
			return null;
		}
	}

	private List<long> GetInjureIds(VideoSkillAction gameAction, bool atOnce)
	{
		List<long> injurerIds = new List<long>();

		foreach (VideoTargetStateGroup group in gameAction.targetStateGroups) {
			long injureId = GetInjureId(group);
			if (injureId > 0)
			{
				injurerIds.Add (injureId);
			}
		}

		return injurerIds;
	}

	private long GetInjureId(VideoTargetStateGroup group)
	{
		foreach (VideoTargetState state in group.targetStates) {
			if (state is VideoRageTargetState)
			{
				continue;
			}
			if (state is VideoBuffRemoveTargetState)
			{
				continue;
			}
//			if (state is VideoActionTargetState && (state as VideoActionTargetState).hp == 0)
//			{
//				continue;
//			}
			
			if (state.id > 0)
			{
				return state.id;
			}
		}

		return 0;
	}

	private bool IsPlayerSideAction(MonsterController attacker)
	{
		if (attacker == null) 
		{
			return true;
		} else {
			return attacker.side == MonsterController.MonsterSide.Player;
		}
    }

	private void ShowSkillName(Skill skill)
	{
		if (skill != null)
		{
			//普通攻击不显示技能名
			if (skill.id != BattleManager.GetNormalAttackSkillId() 
			    && skill.id != BattleManager.GetDefenseSkillId() 
			    && skill.id != BattleManager.GetProtectSkillId() 
			    && skill.id != BattleManager.GetCaptureSkillId()
			    && skill.id != BattleManager.GetRetreatSkillId()
			    && skill.id != BattleManager.GetSummonSkillId()
			    && skill.id != BattleManager.GetUseItemSkillId())
			{
				MonsterController attacker = _instController.GetBattleController().GetMonsterFromSoldierID(_gameAction.actionSoldierId);
				attacker.PlaySkillName(skill, OnSkillNameShowFinish);
			}
			else
			{
				OnSkillNameShowFinish();
			}
		}
	}

	private void OnSkillNameShowFinish()
	{
		if (_attackActionPlayer != null)
		{
			if (_gameAction.targetStateGroups.Count <= _currentFinishRound)
			{
				FinishPlayer();
			}
			else
			{
				if (_skill.atOnce)
				{
					_attackActionPlayer.Play(_gameAction.targetStateGroups[_currentFinishRound], _injureIds, IsLastAction());
				}
				else
				{
					_attackActionPlayer.Play(_gameAction.targetStateGroups[_currentFinishRound], GetNextInjureId(), IsLastAction());
				}
			}
		}
		else
		{
			FinishPlayer();
		}
	}

	private long GetNextInjureId()
	{
		if (_injureIds.Count > 0)
		{
			return _injureIds[_currentFinishRound];
		}
		else
		{
			return 0;
		}
	}

	private bool IsLastAction()
	{
		return _currentFinishRound+1 >= _totalWaitingRound;
	}

	private void DoStrikeBackAction(VideoSkillAction action)
	{
		if (_strikeBackActionPlayer == null)
		{
			_strikeBackActionPlayer = new StrikeBackActionPlayer();
			_strikeBackActionPlayer.OnActionFinish += OnStrikeBackActionFinish;
			_strikeBackActionPlayer.Setup(_instController);
			_strikeBackActionPlayer.DoExcute(action);
		}
	}

	private void OnStrikeBackActionFinish()
	{
		_strikeBackActionPlayer.OnActionFinish -= OnStrikeBackActionFinish;
		_strikeBackActionPlayer = null;
		_attackActionPlayer.Continue ();
	}

    override public void CheckFinish()
    {
		_currentFinishCount ++;

		if (_currentFinishCount >= _totalWaitingCount)
		{
			VideoTargetStateGroup stateGroup = _gameAction.targetStateGroups[_currentFinishRound];
			if (stateGroup.strikeBackAction != null)
			{
				_currentFinishCount --;
				DoStrikeBackAction(stateGroup.strikeBackAction);
				stateGroup.strikeBackAction = null;
			}
			else
			{
				_currentFinishCount = 0;
				_currentFinishRound ++;
				if (_currentFinishRound >= _totalWaitingRound)
				{
					FinishPlayer();
				}
				else
				{
					foreach (BattleActionPlayer player in _injureActionPlayerDic.Values)
					{
						player.Reset();
					}
					_attackActionPlayer.Play(_gameAction.targetStateGroups[_currentFinishRound], GetNextInjureId(), IsLastAction());
				}
			}
		}
		else
		{
			if (_waintingAttackFinish)
			{
				_waintingAttackFinish = false;

				foreach (long playerId in _injureActionPlayerDic.Keys)
				{
					BattleActionPlayer player = _injureActionPlayerDic[playerId];
					player.Play( GetTargetStateGroup(playerId));
				}
			}
		}
	}

	private void FinishPlayer()
	{
		if (_attackActionPlayer != null){
			_attackActionPlayer.Destroy();
			_attackActionPlayer = null;
		}
		
		foreach (BattleActionPlayer player in _injureActionPlayerDic.Values)
		{
			player.Destroy();
		}
		_injureActionPlayerDic.Clear();
		
		MonsterController attacker = _instController.GetBattleController().GetMonsterFromSoldierID(_gameAction.actionSoldierId);
		BattleStateHandler.PlayVideoSkillAction(_instController.GetBattleController(), attacker, _gameAction);

		if (_gameAction.hpSpent != 0)
		{
			DelayFinish(0.2f);
		}
		else
		{
			Finish();
		}
	}

    private bool _waintingAttackFinish = false;

	override public void PlayInjureAction(List<long> ids)
    {
		if (_waintingAttackFinish == false)
		{
			if (ids.Count == 1)
			{
				if (_injureActionPlayerDic.ContainsKey(ids[0]))
				{
					BattleActionPlayer player = _injureActionPlayerDic[ids[0]];
					player.Play(_gameAction.targetStateGroups[_currentFinishRound]);
				}
			}
			else
			{
				foreach (long id in ids)
				{
					if (_injureActionPlayerDic.ContainsKey(id))
					{
						BattleActionPlayer player = _injureActionPlayerDic[id];
						player.Play(GetTargetStateGroup(id));
					}
				}
			}
		}
	}

	private VideoTargetStateGroup GetTargetStateGroup(long id)
	{
		foreach (VideoTargetStateGroup group in _gameAction.targetStateGroups)
		{
			long injureId = GetInjureId(group);
			if (injureId == id)
			{
				return group;
			}
		}
		return null;
	}

	override public void Finish()
	{
		DoSkillEndActions();
		base.Finish ();
	}
}

