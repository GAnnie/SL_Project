// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  DoubleTeleportUnit.cs
// Author   : willson
// Created  : 2014/12/24 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.scene.data;

public class DoubleTeleportUnit : TriggerNpcUnit
{
	override protected bool NeedTrigger(){
		return true;
	}
	
	override public bool NeedClose(){
		return true;
	}
	
	override protected void AfterInit()
	{
		//		if(WorldManager.Instance.getWorldModel().GetSceneDto().sceneMap.sceneType == SceneMap.SceneType_Duplication)
		//		{
		//			if(WorldManager.Instance.getWorldModel().GetSceneDto().personalNpcStates.Count > 0)
		//			{
		//				GetUnitGO().SetActive(false);
		//			}
		//			else
		//			{
		//				GetUnitGO().SetActive(true);
		//			}
		//		}
	}
	
	override protected int GetNpcModel(){
		return 10001;
	}	
	
	override protected float CheckDistance(){
		return 3f;
	}	
	
	override public void Reset(){
		base.Reset();
		waitingForTrigger = false;
	}
	
	private bool waitingForTrigger = false;
	
	override public void DoTrigger()
	{
		base.DoTrigger();
		enabled = false;

		GameDebuger.Log("DoTrigger DoubleTeleportUnit");

		if( _heroPlayer != null )
		{
			_heroPlayer.GetComponent< PlayerView >().StopAndIdle();
		}

		NpcViewManager.EnableTrigger = false;
		enterTeleport();
	}
	
	
	public void CheckTrigger(){
		if (waitingForTrigger){
			enterTeleport();
		}
	}
	
	private void enterTeleport(){
		GameDebuger.Log("enterTeleport!!");
		
		waitingForTrigger = false;
		
		if (_npc is NpcDoubleTeleport){
			enterDoubleTeleport();
		}else{
			enterSingleTeleport();
		}
	}
	
	private void enterSingleTeleport()
	{
		if (_npc is NpcDoubleTeleport)
		{
			WorldManager.Instance.Enter((_npc as NpcDoubleTeleport).toSceneId, true);	
		}
		else
		{
			/*
			NpcSingleTeleport npcTeleport = _npc as NpcSingleTeleport;
			if (npcTeleport.toSceneId.Count > 0)
			{
				WorldManager.Instance.Enter(npcTeleport.toSceneId[0], -1, true);	
			}else{
				TipManager.AddRedTip("传送点没有配置数据");
			}
			*/
		}
	}
	
	private void enterDoubleTeleport()
	{
		// 挂机中忽略传送点
		if(PlayerModel.Instance.IsAutoFram)
			return;

		if (_npc is NpcDoubleTeleport)
		{
			WorldManager.Instance.Enter((_npc as NpcDoubleTeleport).toSceneId,true);	
		}
		else
		{
			NpcDoubleTeleport npcTeleport = _npc as NpcDoubleTeleport;
			WorldManager.Instance.Enter(npcTeleport.toSceneId,true);	
		}			
	}
	
	public void StopTrigger()
	{
		Reset();
		//		waitingForTrigger = false;
	}
}