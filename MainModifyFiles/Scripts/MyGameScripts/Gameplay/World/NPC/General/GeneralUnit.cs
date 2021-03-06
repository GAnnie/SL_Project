﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GeneralUnit.cs
// Author   : SK
// Created  : 2013/2/4
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;

public class GeneralUnit : TriggerNpcUnit
{
	private MonoTimer monoTimer = null;
	private Quaternion _oriQuaternion;

	override protected bool NeedTrigger(){
		return true;
	}
	
	override protected void AfterInit() {
		NpcGeneral npcGeneral = _npc as NpcGeneral;
		InitPlayerName (npcGeneral);

		if (IsMainCharactorModel() == false) {
			if (monoTimer == null) {
				monoTimer = TimerManager.GetTimer("GeneralUnit_" + _npc.id);
				monoTimer.Setup2Time(GetAnimRanTime(), NPCDoAction);
				monoTimer.Play();
			}
		}

		ModelHelper.UpdateModelWeapon(_uintNpcGO, GetNpcModel(), npcGeneral.wpmodel);
	}

	private bool IsMainCharactorModel() {
		return _npc.modelId < 100;
	}

	private int GetAnimRanTime() {
		return Random.Range (10,20);
	}
	
	override public void Trigger() {
		base.Trigger();
		SetFindPathEnabled(false);

		string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_CharactorClick);
		OneShotSceneEffect.BeginFollowEffect (effpath, _unitGameObject.transform, 2f, 1f);
	}

	override public void DoTrigger() {
		base.DoTrigger();
		
		_oriQuaternion = _unitGameObject.transform.rotation;

		NpcGeneral npc = _npc as NpcGeneral;

		if (npc.needDialog) {
			ProxyWorldMapModule.OpenNpcDialogue(npc);
		}
		else {
			if (npc.dialogFunctionId.Count > 1) {
				ProxyWorldMapModule.OpenNpcDialogue(npc);
			}
			else if(npc.dialogFunctionId.Count == 1) {
				DialogFunction dialogFunction = DataCache.getDtoByCls<DialogFunction>(npc.dialogFunctionId[0]);
				NpcDialogueController.OpenDialogueFunction(dialogFunction);
			}
		}
	}
	
	private bool NeedFaceToHero() {
		//		if (_npcConfig.defaultAnim == "die"){
		//			return false;
		//		}
		//		
		//        NpcGeneral npc = _npc as NpcGeneral;
		//        if (npc != null && npc.npcFunctionId != null)
		//        {
		//            foreach (int functionId in npc.npcFunctionId)
		//            {
		//                NpcFunction npcFunction = DataCache.getDtoByCls<NpcFunction>(functionId);
		//                if (npcFunction != null)
		//                {
		//                    if (npcFunction.type == NpcFunction.NpcFunctionType_Copy || npcFunction.type == NpcFunction.NpcFunctionType_HolyTree)
		//                    {
		//                        return false;
		//                    }
		//                }
		//            }
		//        }
		return true;
	}
	
	void NPCDoAction() {
		if (monoTimer != null) {
			monoTimer.Stop ();
			monoTimer.Setup2Time (GetAnimRanTime(), null);
			monoTimer.Play ();
		}

		if(IsPlayAction(ModelHelper.Anim_show) ) {
			return;
		}
		else {
			DoAction(ModelHelper.Anim_show);
		}
	}
	
	private void OnDialogueFinish() {
		//		//结束对话时,显示任务标识及npc名字//
		//		UIManager.Instance.ShowMissionMark();
		//		UIManager.Instance.ShowNpcName();
		//		//显示npc头顶对话气泡//
		//		UIManager.Instance.ShowNpcDialogueBubble();
		//		//显示玩家昵称及称号//
		//		UIManager.Instance.ShowPlayerNameMarksList();
		//		
		//		//显示玩家坐骑
		//		PetRidingModel.Instance.ShowRidingPet();
		//		
		//------------------销毁计时器------------------------//
		if (monoTimer != null){
			monoTimer.Stop();
			TimerManager.RemoveTimer(monoTimer);			
		}
		//		//--------------------------------------------------
	}
	
	override public void SetActiveUnit(bool showNpc) {
		
		base.SetActiveUnit(showNpc);
		SetFindPathEnabled(enabled);
		
		ShowMissionMarkOrNot(showNpc);
		
	}
	
	private void ShowMissionMarkOrNot(bool showNpc) {
		
		//		if(GetNpc() is NpcCycleMission  )
		//			return;
		//		if( GetNpc().id == AppUtils.GetStaticConfigValue( XYStaticConfigs.CYCLE_REFRESH_MISSION_NPCID ) )
		//			return;
		//
		//		NpcNameAndMissionMark mark = _unitGameObject.GetComponent<NpcNameAndMissionMark>();
		//		if(mark == null)	return;
		//		mark.SetActiveMissionMark( showNpc );
	}
	
	private Vector3 _firstTouchPosition;
	
	public void MarkFirstTouchPosition() {
		_firstTouchPosition = _heroPlayer.transform.position;
	}
	
	public void SetFindPathEnabled(bool enabled) {
	}
	
	override public void Destroy() {
		if (monoTimer != null){
			monoTimer.Stop();
			TimerManager.RemoveTimer(monoTimer);
			monoTimer = null;
		}

		GameObject.Destroy (_playerNameObj);
		base.Destroy();
	}
}

