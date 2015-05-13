// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleActionPlayer.cs
// Author   : SK
// Created  : 2013/3/8
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.battle.data;
using System.Xml;
 
public class BattleActionPlayer : MonoBehaviour
{
	
	private BaseBattleInstPlayer _gamePlayer;
	private BattleInstController _instController;
	private VideoSkillAction _gameAction;
	
	private bool _finishAction = false;
	private bool _finishTime = false;
	
	private SkillConfigInfo _skillConfigInfo;
	
	private List<BaseActionInfo> _actionList;
	private int _actionIndex;
	private BaseActionInfo _actionNode;
	
	private bool _isAttack = false;
	private bool _completed = false;
	
    private bool _playerSideAction = false;
	
	private bool _needCameraEffect = false;

	private MonsterController _monster;

	private bool _atOnce = false; //是否一次性播放
	private bool _sameTarget = false; //是否多次打同一个人
	private int _takeDamageActionIndex = 0;  //受击停留动作序号

	private VideoTargetStateGroup _stateGroup;

	private bool _lastAction = false;
	private long _injurerId;
	private List<long> _injurerIds;
	private List<long> _oriInjurerIds;

	private long _targetId;

	public void SetInjureInfo(List<long> injureIds, bool hasSameInjurer = false)
	{
		_oriInjurerIds = new List<long> ();
		_oriInjurerIds.AddRange (injureIds.ToArray());
		_sameTarget = hasSameInjurer;
	}

	public void Init (BaseBattleInstPlayer instPlayer, BattleInstController instController, VideoSkillAction gameAction, bool isAttack, SkillConfigInfo skillConfigInfo, long targetId, bool playerSideAction = true, bool atOnce = false)
	{
		_gamePlayer = instPlayer;
		_instController = instController;
		_gameAction = gameAction;
		_isAttack = isAttack;
		_skillConfigInfo = skillConfigInfo;
		_targetId = targetId;

		_atOnce = atOnce;

		if (_isAttack){
			_actionList = _skillConfigInfo.attackerActions;
			_takeDamageActionIndex = GetTakeDamageActionIndex(_actionList);
		}else{
			_actionList = _skillConfigInfo.injurerActions;
		}
		
        _playerSideAction = playerSideAction;

//		Skill skill = _gameAction.skill;
//		if (_isAttack && skill.selfNextRoundForceSkillId > 0)
//		{
//			_actionIndex = _actionList.Count;
//			_completed = true;
//			return;
//		}
	}

	public void Reset()
	{
		_finishAction = false;
		_finishTime = false;
		_needWaitNormalAction = false;
		_actionIndex = 0;

		if (_isAttack) 
		{
			HasPlayFulEff = false;
		}

		//the same target and not atOnce play mode
		if (_isAttack && _completed && !_atOnce && _sameTarget)
		{
			_actionIndex = _takeDamageActionIndex;
		}

		if (_monster != null)
		{
			_monster.actionEnd = null;
		}
		
		if (_protectMonster != null)
		{
			_protectMonster.actionEnd = null;
		}
		
		_completed = false;
		_startTime = 0;
		_maxWaitTime = 0;
	}

	public void Play(VideoTargetStateGroup stateGroup, long injureId, bool lastAction)
	{
		_stateGroup = stateGroup;
		_injurerId = injureId;
		_lastAction = lastAction;

		Play(stateGroup);
	}

	public void Play(VideoTargetStateGroup stateGroup, List<long> injureIds, bool lastAction)
	{
		_stateGroup = stateGroup;
		_injurerIds = injureIds;
		if (_injurerIds.Count > 0)
		{
			_injurerId = injureIds[0];
		}
		_lastAction = lastAction;
		
		Play(stateGroup);
	}

	public void Play(VideoTargetStateGroup stateGroup)
	{
		_stateGroup = stateGroup;

		_monster = _instController.GetBattleController().GetMonsterFromSoldierID(_targetId);
		
		if (_monster == null){
			Debug.Log("战斗指令对应的怪物已经不存在 怪物id="+_targetId);
			_actionIndex = _actionList.Count;
			_completed = true;
		}

		Reset ();
		PlayNextAction ();
	}

	public void PlayNextAction ()
	{
		if (_isAttack && _monster != null && _gameAction.skillId == BattleManager.GetRetreatSkillId())
		{
			VideoRetreatState retreatState = GetRetreatState();
			if (retreatState != null)
			{
				TipManager.AddTip(string.Format("逃跑成功率{0}%", retreatState.rate*100));
				DoRetreatAction(retreatState.success);
			}
			else
			{
				CompleteActions();
			}
			return;
		}

		if (_isAttack && _monster != null && _gameAction.skillId == BattleManager.GetDefenseSkillId())
		{
			CompleteActions();
			return;
		}

		if (_isAttack && _monster != null && _gameAction.skillId == BattleManager.GetProtectSkillId())
		{
			CompleteActions();
			return;
		}

		LogPlayerInfo ();
		
		if (_actionIndex < _actionList.Count){
			if (_isAttack && !_atOnce && _sameTarget && _actionIndex == (_takeDamageActionIndex+1) && _lastAction == false)
			{
				CompleteActions();
			}else{
				if (_isAttack && _stateGroup.strikeBackAction != null && _actionIndex == (_takeDamageActionIndex+1))
				{
					CompleteActions();
				}
				else
				{
					if (_monster != null)
					{
						BaseActionInfo actionNode = _actionList[_actionIndex];
						_actionIndex ++;
						PlayActionInfo(actionNode);
					}
					else
					{
						Debug.LogError("战斗指令对应的怪物已经不存在 怪物id="+_targetId);
						CompleteActions();
					}
				}
			}
		}else{
			CompleteActions();
		}
	}

	private int GetTakeDamageActionIndex(List<BaseActionInfo> list)
	{
		for (int i=0; i<list.Count; i++)
		{
			BaseActionInfo actionInfo = list[i];
			if (actionInfo.effects != null)
			{
				foreach(BaseEffectInfo info in actionInfo.effects)
				{
					if (info is TakeDamageEffectInfo)
					{
						return i;
					}
				}
			}
		}
		return 0;
	}

	private void DoRetreatAction(bool success)
	{
		_monster.RotationToBack ();
		_monster.PlayAnimation (ModelHelper.Anim_run);
		if (success)
		{
			Invoke ("DelayRetreatActionSuccess", 1.3f);
		}
		else
		{
			Invoke ("DelayRetreatActionFail", 1.3f);
		}
	}

	private void DelayRetreatActionSuccess()
	{
		string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_Retreat);

		Vector3 addVec = Vector3.zero;
		if (_monster.side == MonsterController.MonsterSide.Player)
		{
			addVec =  new Vector3 (-6f,0f,0f);
		}
		else
		{
			addVec =  new Vector3 (6f,0f,0f);
		}

		if (_monster.IsMainCharactor())
		{
			MonsterController mc = _instController.GetBattleController().GetPlayerPet(_monster.GetPlayerId());
			if (mc != null && mc.IsDead() == false)
			{
				OneShotSceneEffect.BeginFollowEffect (effpath, mc.transform, 0.7f, 1f);
				mc.RotationToBack ();
				mc.Goto (mc.transform.position + addVec, 0.7f, ModelHelper.Anim_run);
			}
		}

		OneShotSceneEffect.BeginFollowEffect (effpath, _monster.transform, 0.7f, 1f);
		_monster.Goto (_monster.transform.position + addVec, 0.7f, ModelHelper.Anim_run);
		Invoke ("DelayRetreatActionSuccess2", 0.7f);
	}

	private void DelayRetreatActionSuccess2()
	{
		MonsterController mc = _instController.GetBattleController().GetPlayerPet(_monster.GetPlayerId());
		if (mc != null)
		{
			mc.RetreatFromBattle();
		}
		_monster.RetreatFromBattle ();
		CompleteActions();
	}

	private void DelayRetreatActionFail()
	{
		_monster.PlayAnimation (ModelHelper.Anim_death);
		Invoke ("DelayRetreatActionFail2", 0.5f);
	}

	private void DelayRetreatActionFail2()
	{
		_monster.ResetRotation ();
		_monster.PlayAnimation (ModelHelper.Anim_battle);
		CompleteActions();
	}

	private VideoRetreatState GetRetreatState()
	{
		for (int i=0,len=_stateGroup.targetStates.Count; i<len; i++)
		{
			VideoTargetState state = _stateGroup.targetStates[i];
			if (state is VideoRetreatState)
			{
				return state as VideoRetreatState;
			}
		}
		return null;
	}

	private VideoSwtichPetState GetSwtichPetState()
	{
		for (int i=0,len=_stateGroup.targetStates.Count; i<len; i++)
		{
			VideoTargetState state = _stateGroup.targetStates[i];
			if (state is VideoSwtichPetState)
			{
				return state as VideoSwtichPetState;
			}
		}
		return null;
	}

	private VideoCaptureState GetCaptureState()
	{
		for (int i=0,len=_stateGroup.targetStates.Count; i<len; i++)
		{
			VideoTargetState state = _stateGroup.targetStates[i];
			if (state is VideoCaptureState)
			{
				return state as VideoCaptureState;
			}
		}
		return null;
	}

	private VideoCallSoldierState GetVideoCallSoldierState()
	{
		for (int i=0,len=_stateGroup.targetStates.Count; i<len; i++)
		{
			VideoTargetState state = _stateGroup.targetStates[i];
			if (state is VideoCallSoldierState)
			{
				return state as VideoCallSoldierState;
			}
		}
		return null;
	}

	private void CompleteActions()
	{
		//GameDebuger.LogWithColor(GetAttackType()+"CompleteActions");

		_completed = true;
		
		if (_monster != null){
			//GameDebuger.LogWithColor("CompleteActions" + _monster.GetDebugInfo());
	        if (_monster.IsDead() == false)
	        {
	            _monster.PlayStateAnimation();
			}else{
				if (_monster.NeedLeave())
				{
					_monster.RetreatFromBattle(1);
				}
				else
				{
					_monster.PlayDieAnimation();
				}
			}
		}

		if (_isAttack && _monster != null && _gameAction.skillId == BattleManager.GetRetreatSkillId() && _monster.IsMainCharactor())
		{
			VideoRetreatState retreatState = GetRetreatState();
			if (retreatState.success)
			{
				_instController.GetBattleController().RetreatBattle(_monster.GetPlayerId());
			}
			else
			{
				_gamePlayer.CheckFinish();
			}
		}
		else if (_isAttack && _monster != null && _gameAction.skillId == BattleManager.GetSummonSkillId() && _monster.IsMainCharactor())
		{
			VideoSwtichPetState switchPetState = GetSwtichPetState();
			if (switchPetState.switchPetSoldier != null)
			{
				_instController.GetBattleController().SwitchPet(switchPetState.switchPetSoldier);
				Invoke ("DelaySwtichPetAction", 1f);
			}
			else
			{
				_gamePlayer.CheckFinish();
			}
		}
		else if (_isAttack && _monster != null && GetVideoCallSoldierState() != null)
		{
			VideoCallSoldierState callSoldierState = GetVideoCallSoldierState();
			if (callSoldierState.soldier != null)
			{
				_instController.GetBattleController().CallPet(callSoldierState.soldier);
				Invoke ("DelayCallSoldierAction", 1f);
			}
			else
			{
				_gamePlayer.CheckFinish();
			}
		}
		else
		{
			_gamePlayer.CheckFinish();
		}
	}

	private void DelayCallSoldierAction()
	{
		PlayTakeDamage(null);
		_gamePlayer.CheckFinish();
	}

	private void DelaySwtichPetAction()
	{
		_gamePlayer.CheckFinish();
	}

	private void PlayActionInfo(BaseActionInfo node)
	{
		_actionNode = node;
		
		_finishAction = false;
		_finishTime = false;		
		
		_monster.actionEnd = OnActionEnd;

        _waitingEffects = new List<BaseEffectInfo>(_actionNode.effects.ToArray());

        _maxWaitTime = 0f;
		if (node is NormalActionInfo)
		{
			NormalActionInfo normalActionInfo = node as NormalActionInfo;
			_maxWaitTime = normalActionInfo.startTime + normalActionInfo.delayTime;

			if (_isAttack == false)
			{
				_maxWaitTime += 0.4f;
			}

			if (_isAttack)
			{
				_protectMonster = GetNextProtectMonster(false);
				if (_protectMonster != null)
				{
					MonsterController targetMonster = _instController.GetBattleController().GetMonsterFromSoldierID(getTargetId());
					_protectMonster.MoveTo(targetMonster.transform, 0.05f, null, 0.05f, Vector3.zero);
				}
			}
		}

		foreach (BaseEffectInfo effectNode in _waitingEffects)
        {
			float playTime = effectNode.playTime;
			if (effectNode is TakeDamageEffectInfo && HasVideoDodgeTargetState(_stateGroup.targetStates, getTargetId()))
			{
				playTime = 0.1f;
			}
			if (playTime > _maxWaitTime)
            {
				_maxWaitTime = playTime;
			}
		}

        if (_maxWaitTime == 0f)
        {
            _finishTime = true;
        }

		if ( _actionNode is MoveActionInfo)
		{
			DoMoveAction( (MoveActionInfo)node );
		}
		else if ( _actionNode is NormalActionInfo )
		{
			_needWaitNormalAction = true;
		}
		else if ( _actionNode is MoveBackActionInfo )
		{
			_monster.PlayMoveBackNode( (MoveBackActionInfo)node );
		}

		_startTime = Time.time;
		Update();
	}

	private bool _needWaitNormalAction = false;

	private void CaptureSuccess()
	{
		VideoCaptureState captureState = GetCaptureState();
		if (captureState.wildToBaobao)
		{
			TipManager.AddTip(string.Format("喜从天降，这是一只{0}宝宝", captureState.pet.name.WrapColor(ColorConstant.Color_Tip_Item)));
		}

		_monster.LeaveBattle();
		_finishAction = true;
		CheckPlayerFinish();
	}

	private void CaptureFail()
	{
		_monster.Goto(_monster.originPosition, 1f, ModelHelper.Anim_run, 0, true);
	}

	private void DoMoveAction(MoveActionInfo node){
		if (node.center == false){
			MonsterController target = _instController.GetBattleController().GetMonsterFromSoldierID(getTargetId());
            _monster.SetSkillTarget(target);
		}
		_monster.PlayMoveNode(node);
	}	
	
	private void DoNormalAttackAction(NormalActionInfo node){
		_monster.PlayNormalNode(node);
	}
	
	private void DoNormalInjureAction(NormalActionInfo node)
	{
		if (_monster != null)
		{
			PlayInjureAction(_monster, node);
		}
	}

	private MonsterController _protectMonster = null;
	private VideoInsideSkillAction _videoInsideSkillAction = null;

	private void PlayInjureAction(MonsterController mc, NormalActionInfo node)
	{
		VideoTargetState state;
		
		state = GetTargetState(_stateGroup.targetStates, mc.videoSoldier.id);
		BattleStateHandler.CheckDeadState(mc, state);

		string actionName = ModelHelper.Anim_battle;

		bool needHit = false;

		if (state is VideoDodgeTargetState)
		{
			needHit = true;
			actionName = ModelHelper.Anim_battle;
			mc.BodyShift( 1.0f,0.1f,3, 0.2f);
		}
		else if (state is VideoCaptureState)
		{
			PlayCatchffect (mc);
		}
		else
		{
			VideoActionTargetState videoActionTargetStat = state as VideoActionTargetState;
			if (videoActionTargetStat != null && videoActionTargetStat.hp < 0) 
			{
				needHit = true;
			}
			else
			{
				needHit = false;
			}

			if (needHit)
			{
				if (_gameAction.targetSoldierStatus == VideoSoldier.SoldierStatus_SelfDefense)
				{
					if (_monster.dead)
					{
						if (_monster.IsMonster())
						{
							actionName = ModelHelper.Anim_def;
						}
						else
						{
							actionName = ModelHelper.Anim_death;
						}
					}
					else
					{
						actionName = ModelHelper.Anim_def;
						mc.BodyShift( 0.2f,0.2f,3 ,node.delayTime);
					}
					PlayDefenseEffect (mc);
				}
				else
				{
					if (_monster.dead)
					{
						if (_monster.IsMonster())
						{
							actionName = ModelHelper.Anim_hit;
						}
						else
						{
							actionName = ModelHelper.Anim_death;
						}
					}
					else
					{
						actionName = ModelHelper.Anim_hit;
						mc.BodyShift( 0.4f,0.2f,3, node.delayTime);
					}
				}

				_videoInsideSkillAction = null;
				_protectMonster = GetNextProtectMonster(true);
				if (_protectMonster != null)
				{
					_videoInsideSkillAction = GetNextVideoInsideSkillAction();
					_protectMonster.BodyShift( 0.4f,0.2f,3, node.delayTime+0.3f, true);
					_protectMonster.actionEnd = OnProtectActionEnd;
					DoPlayNormalAction(_protectMonster, ModelHelper.Anim_hit, node.delayTime);
				}
				
				string soundName = string.Format ("pet_{0}_{1}", mc.GetModel(), "hit");
				AudioManager.Instance.PlaySound ("sound_pet/"+soundName);
			}
		}

		float delayTime = node.delayTime;
		if (actionName == ModelHelper.Anim_def)
		{
			delayTime = 0.3f;
		}

		if (actionName == "")
		{
			delayTime = 0.05f;
		}
		
		DoPlayNormalAction(mc, actionName, delayTime);
	}
	
	private MonsterController GetNextProtectMonster(bool needRemove)
	{
		long monsterId = 0;

		if (_gameAction.protectTargetSoldierIds != null && _gameAction.protectTargetSoldierIds.Count > 0)
		{
			monsterId =  _gameAction.protectTargetSoldierIds[0];
			if (needRemove)
			{
				_gameAction.protectTargetSoldierIds.RemoveAt(0);
			}
		}

		MonsterController mc = _instController.GetBattleController().GetMonsterFromSoldierID(monsterId);
		return mc;
	}

	private VideoInsideSkillAction GetNextVideoInsideSkillAction()
	{
		VideoInsideSkillAction action = null;
		
		if (_gameAction.protectActions != null && _gameAction.protectActions.Count > 0)
		{
			action =  _gameAction.protectActions[0];
			_gameAction.protectActions.RemoveAt(0);
		}
		
		return action;
	}

	private void PlayDefenseEffect(MonsterController mc)
	{
		NormalEffectInfo node = new NormalEffectInfo ();
		node.fly = false;
		node.target = 0;
		node.mount = ModelHelper.Mount_hit;
		PlaySpecialEffect (node, GameEffectConst.Effect_Defence, node.target, node.mount, _monster, mc, 1);
	}

	private void PlayCatchffect(MonsterController mc)
	{
		NormalEffectInfo node = new NormalEffectInfo ();
		node.fly = false;
		node.target = 0;
		node.mount = ModelHelper.Mount_shadow;
		PlaySpecialEffect (node, GameEffectConst.Effect_Catch, node.target, node.mount, _monster, mc, 1);
	}

	private void DoPlayNormalAction(MonsterController mc, string action, float delayTime)
	{
//		if (action == ModelHelper.Anim_death || action == ModelHelper.Anim_hit)
//		{
//			DoPlayCameraShake( 0.5f, 3 );
//		}

		if (action == ModelHelper.Anim_death)
		{
			mc.PlayDieAnimation();
		}
		else
		{
			mc.PlayNormalActionInfo(action, delayTime);
		}
	}


	private VideoTargetState GetTargetState(List<VideoTargetState> arr, long id)
	{
		foreach(VideoTargetState state in arr){
			if (state.id == id){			
				if (state is VideoRageTargetState)
				{
					continue;
				}
				if (state is VideoRetreatState)
				{
					continue;
				}
				if (state is VideoBuffRemoveTargetState)
				{
					continue;
				}
				if (state is VideoSwtichPetState)
				{
					continue;
				}
				return state;
			}
		}
		return null;
	}

	private bool HasVideoDodgeTargetState(List<VideoTargetState> arr, long id)
	{
		bool hasDodge = false;
		foreach(VideoTargetState state in arr)
		{
			if (state.id == id){			
				if (state is VideoDodgeTargetState)
				{
					hasDodge = true;
					break;
				}
			}
		}
		return hasDodge;
	}

	private long getTargetId()
	{
		return GetNextInjureId();
	}
	
	/**
	 *是否最后一个节点 
	 * @return 
	 * 
	 */		 
	public bool IsLastActionInfo(){
		return _actionList.IndexOf(_actionNode) == (_actionList.Count-1);
	}	
	
	private void OnActionEnd(string type){
		_finishAction = true;

		LogPlayerInfo ();

        if (_finishTime == false)
        {
            if (_monster.IsDead() == false)
            {
                _monster.PlayStateAnimation();
            }
            else
            {
				if (_monster.NeedLeave())
				{
					_monster.RetreatFromBattle(1);
				}
				else
				{
					_monster.PlayDieAnimation();
				}
            }
        }else{
			if (IsLastActionInfo()){
	            if (_monster.IsDead() == false)
	            {
	                _monster.PlayStateAnimation();
	            }
	            else
	            {
					if (_monster.NeedLeave())
					{
						_monster.RetreatFromBattle(1);
					}
					else
					{
						_monster.PlayDieAnimation();
					}
	            }
			}else{
				_monster.PlayIdleAnimation();
			}
		}

		if (_finishTime)
		{
			CheckPlayerFinish();
		}
	}
	
	private void CheckPlayerFinish(){
		LogPlayerInfo ();

		if (_finishAction && _finishTime && _completed == false){
			_monster.actionEnd = null;
			PlayNextAction();
		}
	}	

	private void LogPlayerInfo()
	{
//		if (_isAttack) {
//			GameDebuger.LogBattleInfo (GetAttackType() + " finishAction=" + _finishAction + " finishTime=" + _finishTime + " completed=" + _completed + " playActionIndex="+_actionIndex + "/" + _actionList.Count);
//		}
	}

	private void OnProtectActionEnd(string type)
	{
		_protectMonster.actionEnd = null;
		if (_protectMonster.IsDead())
		{
			if (_protectMonster.NeedLeave())
			{
				_protectMonster.RetreatFromBattle(1);
			}
			else
			{
				_protectMonster.PlayDieAnimation();
			}
		}
		else
		{
			_protectMonster.GoBack (0.05f, Vector3.zero, null, false, 0.5f);
		}
		//_protectMonster = null;
	}

	public bool IsComplete()
	{
		//GameDebuger.LogWithColor(GetAttackType()+"IsComplete="+_completed);
		return _completed;
	}

	private string GetAttackType()
	{
		string str = "";
		if (_isAttack)
		{
			str = "攻击方 ";
		}
		else
		{
			str = "受击方 ";
		}

		if (_monster != null)
		{
			str += _monster.GetDebugInfo();
		}

		return str;
	}

	public void CleanWaitingEffects()
	{
		_waitingEffects = null;
	}
	
	private List<BaseEffectInfo> _waitingEffects;
	private float _startTime;
    private float _maxWaitTime;
	void Update()
	{
		float passTime = Time.time - _startTime;

		if (_needWaitNormalAction)
		{
			float startTime = (_actionNode as NormalActionInfo).startTime;
			if (passTime >= startTime)
			{
				if (_isAttack){
					DoNormalAttackAction( (NormalActionInfo)_actionNode );
				}else{
					DoNormalInjureAction( (NormalActionInfo)_actionNode );
				}
				_needWaitNormalAction = false;
			}
		}
		
		if (_waitingEffects != null)
		{
			List<BaseEffectInfo> removeNodes = null;
			foreach( BaseEffectInfo node in  _waitingEffects )
			{
				float playTime = node.playTime;
				if (node is TakeDamageEffectInfo && HasVideoDodgeTargetState(_stateGroup.targetStates, getTargetId()))
				{
					playTime = 0.1f;
				}

				if (passTime >= playTime)
				{
					PlayEffect( node );
					if (removeNodes == null){
						removeNodes = new List<BaseEffectInfo>();
					}
					removeNodes.Add(node);
				}
			}
			
			if (removeNodes != null){
				foreach( BaseEffectInfo removeNode in removeNodes)
				{
					_waitingEffects.Remove(removeNode);
				}
				
				if (_waitingEffects.Count == 0){
					_waitingEffects = null;
				}
			}
		}

		if (_finishTime == false)
		{
			if (passTime >= _maxWaitTime && _finishTime == false)
			{
				_finishTime = true;
				CheckPlayerFinish();
			}
		}
	}	
	
	public void PlayEffect( BaseEffectInfo node )
	{
//		if ( node.type == "screenShake" )
//			PlayCameraShake( (ScreenShakeEffectInfo)(node) );
		if ( node is TakeDamageEffectInfo)
			PlayTakeDamage( (TakeDamageEffectInfo)(node) );
//		else if ( node.type == "bodyShift" )
//			_monster.PlayBodyShift( (BodyShiftEffectInfo)(node) );
		else if (node is NormalEffectInfo && SystemSetting.ShowEffect)
		{
			if (_isAttack == false)
			{
				bool hasDodge = HasVideoDodgeTargetState(_stateGroup.targetStates, _monster.GetId());

				if (hasDodge && (node as NormalEffectInfo).hitEff)
				{
					return;
				}
			}

			PlayNormalEffect( (NormalEffectInfo)(node) );
		}
//		else if ( node.type == "hide" )
//			_monster.PlayBodyHide(  (HideEffectInfo)(node));
//		else if ( node.type == "ghost" && SystemSetting.ShowEffect)
//			_monster.PlayShadowMotionNode(  (GhostEffectInfo)(node));
		else if (node is ShowInjureEffectInfo)
			ShowInjureEffect( (ShowInjureEffectInfo)(node));
		else if (node is SoundEffectInfo)
			PlaySoundEffect( (SoundEffectInfo)(node));
//		else if ( node.type == "trail" ) //暂时屏蔽掉刀光效果
//			_monster.PlayTrailEffect( (TrailEffectInfo)(node));
//		else if ( node.type == "camera")
//			PlayCameraEffect( (CameraEffectInfo)(node));		
	}

	private string GetSkillEffectName(Skill skill, string skillName)
	{
		if (skillName == null)
		{
			skillName = "";
		}

		if (skillName != "")
		{
			if (skillName.Contains("skill_") == false && skillName.Contains("game_") == false)
			{
				int effId = skill.clientEffectType;
				
				if (effId == 0)
				{
					effId = skill.clientSkillType;
				}
				
				skillName = string.Format ("skill_eff_{0}_{1}", effId, skillName);
			}

			skillName = skillName.ToLower();
			
			string skillPath = "";
			if (skillName.IndexOf("game_") != -1)
			{
				skillPath = string.Format (PathHelper.GAME_EFFECT_PATH, skillName);
			}
			else
			{
				skillPath = string.Format (PathHelper.SKILL_EFFECT_PATH, skillName);
			}
		}

		return skillName;
	}

	private static bool HasPlayFulEff = false;
    private void PlayNormalEffect(NormalEffectInfo node)
    {
		int skillId = _gameAction.skillId;
		Skill skill = DataCache.getDtoByCls<Skill>(skillId);

		string skillName = GetSkillEffectName(skill, node.name);

		if (node.name == "full")
		{
			if (HasPlayFulEff)
			{
				return;
			}

			HasPlayFulEff = true;
		}

        int targetType = node.target;
        string mount = node.mount;

		int clientSkillScale = 10000;

        if (_isAttack)
        {
            if (node.fly && node.flyTarget == 0)
            {
				if (_atOnce == false)
				{
					long id = GetNextInjureId();
					MonsterController mc = _instController.GetBattleController().GetMonsterFromSoldierID(id);
					PlaySpecialEffect(node, skillName, targetType, mount, _monster, mc, clientSkillScale);
				}
				else
				{
					//特效目标是敌方身上并且是飞行特效,同时飞多个特效到目标上
					List<long> ids = GetInjurerIds();
					foreach (long id in ids)
					{
						MonsterController mc = _instController.GetBattleController().GetMonsterFromSoldierID(id);
						if (mc != null)
						{
							PlaySpecialEffect(node, skillName, targetType, mount, _monster, mc, clientSkillScale);
						}
					}
				}
            }
            else
            {
				PlaySpecialEffect(node, skillName, targetType, mount, _monster, _monster.GetSkillTarget(), clientSkillScale);
            }
        }
        else
        {
            if (AppManager.SkillConfigMode == false)
            {
                //处理个体和区域的选择
//                if (skill != null)
//                {
//					if (skill.clientEffectType != 0 && skill.clientEffectType != skill.id)
//                    {
//                        skillId = skill.clientEffectType;
//                        skill = DataCache.getDtoByCls<Skill>(skillId);
//                    }
//                }
//
//                if (skill != null)
//                {
//                    //1 原点  2 地面
//                    if (skill.clientHitEffectPosition == 2)
//                    {
//                        targetType = 0;
//						mount = ModelHelper.Mount_shadow;
//					}
//
//                    //1 单体   区域
//                    if (skill.clientHitEffectScope == 2)
//                    {
//                        targetType = 2;
//                    }
//                }
            }

            if (targetType == 0)
            {
				if (_monster != null)
				{
					PlaySpecialEffect(node, skillName, targetType, mount, _monster, _monster.GetSkillTarget(), clientSkillScale);
				}
			}
			else
			{
				PlaySpecialEffect(node, skillName, targetType, mount, _monster, _monster.GetSkillTarget(), clientSkillScale);
            }
        }
    }
	
	public void PlaySpecialEffect(NormalEffectInfo node, string skillName, int targetType, string mountName, MonsterController monster, MonsterController target, int clientSkillScale)
    {
        //	  public float delayTime;
        //    public int target;//特效目标  0默认， 1，场景中心 2，我方中心   3， 敌军中心
        //    public bool loop;//是否循环
        //    public int loopCount;//循环次数
        //    public int scale;//缩放 单位100

		if (clientSkillScale == 0)
		{
			clientSkillScale = 10000;
		}

        skillName = skillName.ToLower();

		string skillPath = "";
		if (skillName.IndexOf("game_") != -1)
		{
			skillPath = string.Format (PathHelper.GAME_EFFECT_PATH, skillName);
		}
		else
		{
			skillPath = string.Format (PathHelper.SKILL_EFFECT_PATH, skillName);
		}
		
		ResourcePoolManager.Instance.Spawn(skillPath, 
		delegate(UnityEngine.Object inst)
		{
	        GameObject effectGO = inst as GameObject;
			if (effectGO == null)
	        {
	            return;
	        }

			GameObjectExt.AddPoolChild(LayerManager.Instance.EffectsAnchor, effectGO);

			AudioHelper.PlayEffectSound(skillName);
	
	        Vector3 effectStartPosition = new Vector3();
	
	        switch (targetType)
	        {
	            case 0://默认
	                Transform mountTransform = null;
	                if (string.IsNullOrEmpty(mountName) == false)
	                {
						mountTransform = monster.transform.GetChildTransform(mountName);
	                }
	                if (mountTransform == null)
	                {
	                    mountTransform = monster.gameObject.transform;
	                }
					if (mountName == ModelHelper.Mount_shadow)
	                {
	                    effectStartPosition = new Vector3(mountTransform.position.x, 0, mountTransform.position.z);
	                }else{
	                    effectStartPosition = mountTransform.position;
	                }
	                break;
	            case 1://场景中心
	                effectStartPosition = Vector3.zero;
	                break;
	            case 2://我方中心
	                effectStartPosition = BattlePositionCalculator.GetZonePosition(monster.side, _instController.GetBattleController());
	                break;
	            case 3://敌方中心
	                effectStartPosition = BattlePositionCalculator.GetZonePosition((monster.side == MonsterController.MonsterSide.Player ? MonsterController.MonsterSide.Enemy : MonsterController.MonsterSide.Player), _instController.GetBattleController());
	                break;
	        }
	
	        //位移
	        Vector3 offVec = new Vector3(node.offX, node.offY, node.offZ);
	        effectStartPosition = effectStartPosition + offVec;
			
	        Transform trans = effectGO.transform;
			trans.position = effectStartPosition;
			trans.rotation = Quaternion.identity;
	
	        //特效时间
	        EffectTime effectTime = effectGO.GetComponent<EffectTime>();
	        if (effectTime == null)
	        {
	            effectTime = effectGO.AddComponent<EffectTime>();
	            effectTime.time = 5;
	        }
	
	        if (node.delayTime > 0)
	        {
	            effectTime.time = node.delayTime;
	        }
	        if (node.loop)
	        {
	            effectTime.loopCount = node.loopCount;
	        }
	
	        float scaleValue = (float)node.scale / 100f;
	
			scaleValue *= (float)clientSkillScale / 10000f;

	        //Set Effect Scale
	        ParticleScaler scaler = effectGO.GetComponent<ParticleScaler>();
	        if (scaler == null)
	        {
	            scaler = effectGO.AddComponent<ParticleScaler>();
	        }

			scaler.SetscaleMultiple(scaleValue);

			//旋转
			int rotOffY = 0;
			//如果是我方受击的时候， 旋转特效180度
			if (_isAttack == false && monster.side == MonsterController.MonsterSide.Player)
			{
				rotOffY = 180;
			}

			trans.eulerAngles = new Vector3(node.rotX, node.rotY + rotOffY, node.rotZ);
			
			
			//跟随
	        if (node.follow){
				trans.parent = monster.gameObject.transform;
			}
	
	        //飞行
	        if (node.fly)
	        {
	            Vector3 targetPoint = Vector3.zero;
	            switch (node.flyTarget)
	            {
	                case 0://默认
	                    Transform flyTargetTransform = null;
	                    if (string.IsNullOrEmpty(node.mount) == false)
	                    {
							flyTargetTransform = target.transform.GetChildTransform(node.mount);
	                    }
	                    if (flyTargetTransform == null)
	                    {
	                        flyTargetTransform = target.gameObject.transform;
	                    }
						if (mountName == ModelHelper.Mount_shadow)
						{
							targetPoint = new Vector3(flyTargetTransform.position.x, 0, flyTargetTransform.position.z);
	                    }
	                    else
	                    {
	                        targetPoint = flyTargetTransform.position;
	                    }
	                    break;
	                case 1://场景中心
	                    targetPoint = Vector3.zero;
	                    break;
	                case 2://我方中心
	                    effectStartPosition = BattlePositionCalculator.GetZonePosition(monster.side, _instController.GetBattleController());
	                    break;
	                case 3://敌方中心
	                    effectStartPosition = BattlePositionCalculator.GetZonePosition((monster.side == MonsterController.MonsterSide.Player ? MonsterController.MonsterSide.Enemy : MonsterController.MonsterSide.Player), _instController.GetBattleController());
	                    break;
	            }
	
	            //飞行位移
	            Vector3 flyOffVec = new Vector3(node.flyOffX, node.flyOffY, node.flyOffZ);
	            targetPoint = targetPoint + flyOffVec;
	
				float flyTime = node.delayTime;
				if (flyTime == 0)
	            {
	                flyTime = 1f;
	            }
	            iTween.MoveTo(effectGO, iTween.Hash("x", targetPoint.x, "y", targetPoint.y, "z", targetPoint.z, "time", flyTime, "looktarget",
	                                        targetPoint, "looktime", 1, "looptype", "none", "easetype", "linear"));
	        }			
		}, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
    }
	
	void ShowInjureEffect(ShowInjureEffectInfo node)
	{
		if (_isAttack == false)
		{
			if (_monster != null)
			{
				BattleStateHandler.HandleBattleState( _monster.GetId(), _stateGroup.targetStates, _instController.GetBattleController());
			}

			if (_protectMonster != null && _videoInsideSkillAction != null)
			{
				BattleStateHandler.HandleBattleState( _protectMonster.GetId(), _videoInsideSkillAction.targetStateGroups[0].targetStates, _instController.GetBattleController());
			}

			VideoCaptureState captureState = GetCaptureState();
			if (captureState != null)
			{
				float moveBackTime = 3.4f;
				
				MonsterController attackMonster = _instController.GetBattleController().GetMonsterFromSoldierID(getAttackerId());
				_monster.Goto(attackMonster.originPosition, moveBackTime+0.2f, ModelHelper.Anim_run, 0.05f);
				
				if (captureState.success)
				{
					Invoke("CaptureSuccess", moveBackTime);
				}
				else
				{
					float ranMoveTime = Random.Range(moveBackTime/2,moveBackTime-1);
					Invoke("CaptureFail", ranMoveTime);
				}
			}
		}
	}

	private void PlaySoundEffect(SoundEffectInfo info)
	{
		string soundName = info.name;
		AudioManager.Instance.PlaySound ("sound_effect/"+soundName);
	}

	//是否普攻技能
	private bool IsNormalAttack(int skillId)
	{
		return skillId == BattleManager.GetNormalAttackSkillId();
	}

	void PlayTakeDamage(TakeDamageEffectInfo node)
	{
		List<long> ids = null;

		if (_atOnce)
		{
			ids = GetInjurerIds();
		}
		else
		{
			ids = new List<long>();
			ids.Add(getTargetId());
		}

		_gamePlayer.PlayInjureAction(ids);
	}
	
//	void PlayCameraShake( ScreenShakeEffectInfo node )
//	{
//		DoPlayCameraShake(node.delayTime, node.power);
//	}	

	void DoPlayCameraShake( float delayTime, int power )
	{
		int skillId = _gameAction.skillId;
		if (IsNormalAttack(skillId))
		{
			return;
		}
		
		MonsterController attacker = _instController.GetBattleController().GetMonsterFromSoldierID(getAttackerId());
		if (attacker != null)
		{
			int quality = attacker.GetSoldierQuality();
			if (quality < 5)
			{
				return;
			}
		}
		
		CameraShake.Instance.UpdatePostionAndRotation();
		CameraShake.Instance.Launch(delayTime, power);
	}	

	public long getAttackerId ()
	{
		return _gameAction.actionSoldierId;
	}
	
	public List<long> GetInjurerIds ()
	{
		return _injurerIds;
	}

	public List<long> GetOriInjurerIds()
	{
		return _oriInjurerIds;
	}

	public long GetNextInjureId()
	{
		return _injurerId;
	}

    public void DelayPlay()
    {
        Invoke("Play", 0.1f);
    }

	public void Continue()
	{
		_finishAction = false;
		_finishTime = false;
		_needWaitNormalAction = false;
		
		if (_monster != null)
		{
			_monster.actionEnd = null;
		}
		
		if (_protectMonster != null)
		{
			_protectMonster.actionEnd = null;
		}
		
		_completed = false;
		_startTime = 0;
		_maxWaitTime = 0;

		PlayNextAction ();
	}

	public void Destroy()
	{
		if (_monster != null){
			_monster.actionEnd = null;
			_monster = null;
		}

		if (_protectMonster != null)
		{
			_protectMonster.actionEnd = null;
			_protectMonster = null;
		}
		_videoInsideSkillAction = null;

		GameObject.Destroy(this);	
	}
}

