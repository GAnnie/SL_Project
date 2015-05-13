using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.battle.data;
using System.Collections.Generic;

public class StrikeBackActionPlayer : BaseBattleInstPlayer
{

	private VideoSkillAction  _gameAction;
	private SkillConfigInfo _skillConfig;

	private BattleActionPlayer _attackActionPlayer;
	private BattleActionPlayer _injureActionPlayer;

	private int _totalWaitingCount = 0;
	private int _currentFinishCount = 0;

	public event System.Action OnActionFinish;
	
	override public void DoExcute (VideoAction inst)
	{
		_gameAction = inst as VideoSkillAction;
		DoSkillBeforeActions ();
		DoSkillAction();
	}

	private void DoSkillBeforeActions()
	{
		foreach (VideoAction action in _gameAction.beforeActions)
		{
			BattleStateHandler.HandleBattleStateGroup( 0, action.targetStateGroups, _instController.GetBattleController());
		}
	}
	
	private void DoSkillEndActions()
	{
		foreach (VideoAction action in _gameAction.afterActions)
		{
			BattleStateHandler.HandleBattleStateGroup( 0, action.targetStateGroups, _instController.GetBattleController());
		}
	}
	
	private void DoSkillAction()
	{
		//空的施放者
		if (_gameAction.actionSoldierId == 0)
		{
			Debug.LogError("DoSkillAction attacker is null");
			BattleStateHandler.HandleBattleStateGroup( 0, _gameAction.targetStateGroups, _instController.GetBattleController());
			DelayFinish(0.5f);
			return;
		}
		
		MonsterController attacker = _instController.GetBattleController().GetMonsterFromSoldierID(_gameAction.actionSoldierId);
		
		if (attacker == null)
		{
			Debug.LogError("attacker is null id = " + _gameAction.actionSoldierId);
			Finish();
			return;
		}
		
		Skill skill = DataCache.getDtoByCls<Skill>(_gameAction.skillId);
		
		int skillConfigId = skill.id;
		if (skillConfigId == 1)
		{
			skillConfigId = 101;
		}

		_skillConfig = BattleConfigManager.Instance.getSkillConfigInfo(skillConfigId);
		
		if (_skillConfig == null)
		{
			skillConfigId = skill.clientSkillType;
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
		
		_totalWaitingCount = 2;
		_currentFinishCount = 0;
		
		List<long> injurerIds = GetInjureIds (_gameAction);
		
		if (_attackActionPlayer == null)
		{
			_attackActionPlayer = _instController.GetBattleController().gameObject.AddComponent<BattleActionPlayer>();
			_attackActionPlayer.Init(this, _instController, _gameAction, true, _skillConfig, _gameAction.actionSoldierId, IsPlayerSideAction(attacker), skill.atOnce);
			_attackActionPlayer.SetInjureInfo(injurerIds, false);
		}

		if (_injureActionPlayer == null) 
		{
			_injureActionPlayer = _instController.GetBattleController().gameObject.AddComponent<BattleActionPlayer>();
			_injureActionPlayer.Init(this, _instController, _gameAction, false, _skillConfig, injurerIds[0], IsPlayerSideAction(attacker), skill.atOnce);
		}
		
		ShowSkillName(_gameAction.skill);
	}
	
	private List<long> GetInjureIds(VideoSkillAction gameAction)
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
			if (state is VideoActionTargetState && (state as VideoActionTargetState).hp == 0)
			{
				continue;
			}
			
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
			    && skill.id != BattleManager.GetSummonSkillId())
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
			_attackActionPlayer.Play(_gameAction.targetStateGroups[0]);
		}
		else
		{
			FinishPlayer();
		}
	}
	
	override public void CheckFinish()
	{
		_currentFinishCount ++;
		
		if (_currentFinishCount >= _totalWaitingCount)
		{
			FinishPlayer();
		}
	}
	
	private void FinishPlayer()
	{
		if (_attackActionPlayer != null){
			_attackActionPlayer.Destroy();
			_attackActionPlayer = null;
		}

		if (_injureActionPlayer != null){
			_injureActionPlayer.Destroy();
			_injureActionPlayer = null;
		}
		
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
	
	override public void PlayInjureAction(List<long> ids)
	{
		_injureActionPlayer.Play (_gameAction.targetStateGroups[0]);
	}
	
	override public void Finish()
	{
		DoSkillEndActions();
		if (OnActionFinish != null)
		{
			OnActionFinish();
		}
	}
}

