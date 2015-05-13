// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TriggerNpcUnit.cs
// Author   : willson
// Created  : 2014/12/23 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;

public class TriggerNpcUnit : BaseNpcUnit
{
	protected GameObject _playerNameObj;

	public bool enabled;
	public bool waitingTrigger;
	public bool touch;
	
	protected GameObject _heroPlayer;
	protected NpcHUDView _npcHUDView = null;
	
	public TriggerNpcUnit(){
		Reset();
	}
	
	virtual public void Reset(){
		enabled = true;
		waitingTrigger = false;
		touch = false;
	}
	
	public void SetHeroPlayer(GameObject heroPlayer){
		_heroPlayer = heroPlayer;
	}	
	
	virtual public bool NeedClose(){
		return false;
	}
	
	protected void InitPlayerName(Npc npc) {
		if (_playerNameObj == null) {
			GameObject npcHUDPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/PlayerHUDModule/NpcHUDView") as GameObject;
			_playerNameObj = NGUITools.AddChild(LayerManager.Instance.sceneHudTextAnchor, npcHUDPrefab);
			_playerNameObj.name = string.Format("NpcHUDView_{0}", npc.id);
			_npcHUDView = _playerNameObj.GetMissingComponent<NpcHUDView>();
			_npcHUDView.Setup(_playerNameObj.transform);
			
			string appellationStr = "";
			if (npc is NpcGeneral) {
				if(!string.IsNullOrEmpty((npc as NpcGeneral).title)) {
					appellationStr = string.Format("{0}\n", (npc as NpcGeneral).title.WrapColor(ColorConstant.Color_Title_Str));
				}
			} else if (npc is NpcMonster) {
				if(!string.IsNullOrEmpty((npc as NpcMonster).title)) {
					appellationStr = string.Format("{0}\n", (npc as NpcMonster).title.WrapColor(ColorConstant.Color_Title_Str));
				}
			}
			_npcHUDView.nameLbl_UILabel.text = string.Format("[b]{0}{1}", appellationStr, npc.name.WrapColor(ColorConstant.Color_Battle_Enemy_Name));
			
			SetNPCMissionFlag(false, false);
			SetNPCFightFlag(false);
		}

		//	Bottom HUD
		_npcHUDView.BottomFollow_UIFollowTarget.gameCamera = LayerManager.Instance.GameCamera;
		_npcHUDView.BottomFollow_UIFollowTarget.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tMountShadow = ModelHelper.GetMountingPoint(_uintNpcGO, ModelHelper.Mount_shadow);
		_npcHUDView.BottomFollow_UIFollowTarget.target = tMountShadow;
		_npcHUDView.BottomFollow_UIFollowTarget.offset = new Vector3 (0f,-0.5f,0f);

		//	Top HUD
		_npcHUDView.TopFollow_UIFollowTarget.gameCamera = LayerManager.Instance.GameCamera;
		_npcHUDView.TopFollow_UIFollowTarget.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tMountHUD = ModelHelper.GetMountingPoint(_uintNpcGO, ModelHelper.Mount_hud);
		_npcHUDView.TopFollow_UIFollowTarget.target = tMountHUD;
		_npcHUDView.TopFollow_UIFollowTarget.offset = Vector3.zero;
	}
	
	//	NPC状态(是否主线 \ 战斗)
	/// <summary>
	/// NPC状态(是否主线 \ 战斗) | active为false时 mainMission值无效 -- Sets the NPC mission flag.
	/// </summary>
	/// <param name="active">If set to <c>true</c> active.</param>
	/// <param name="mainMission">If set to <c>true</c> main mission.</param>
	private void SetNPCMissionFlag(bool active, bool mainMission){
		_npcHUDView.missionType_UISprite.enabled = active;
		
		if (active) {
			_npcHUDView.missionType_UISprite.spriteName = mainMission? "mission_ThreadOfNPC" : "mission_BattleOfNPC";
		}
	}
	
	//	NPC进入战斗
	/// <summary>
	/// NPC进入战斗 -- Sets the NPC fight flag.
	/// </summary>
	/// <param name="active">If set to <c>true</c> active.</param>
	private void SetNPCFightFlag(bool active){
		if(_npcHUDView != null){
			_npcHUDView.fightFlag_UISprite.enabled = active;
			_npcHUDView.fightFlag_UISpriteAnimation.enabled = active;
		}
	}

	override public void Trigger(){
		
		if (enabled == false){
			return;
		}
		
		if (touch){
			DoTrigger();
			return;
		}
		
		
		float distance = GetDistanceToHero();
		
		if (distance < CheckDistance())
		{
			DoTrigger();
		}
		else
		{
			waitingTrigger = true;
			_heroPlayer.GetComponent<PlayerView>().WalkToPoint(GetUnitGO().transform.position);
		}
	}	
	
	virtual protected float CheckDistance(){
		return 2f;
	}
	
	virtual public void DoTrigger(){
		waitingTrigger = false;
		touch = false;

		//npc朝向玩家
		FaceToHero ();

//		MissionGuidePathFinder.Instance.CheckNpcFinded(_npc);
	}
	
	public void FaceToHero()
	{
		Vector3 position = _heroPlayer.transform.position;
		Transform  unitTransform = _unitGameObject.transform;
		unitTransform.LookAt ( position );//, targetOrientation);
		unitTransform.eulerAngles = new Vector3( 0, unitTransform.eulerAngles.y, 0 );
	}
	
	public float GetDistanceToHero()
	{
		Vector3 distance = _heroPlayer.transform.position-GetUnitGO().transform.position;
		
		float magnitude = distance.magnitude;
		
		return magnitude;
	}
	
	public Vector3 GetHeroPosition()
	{
		return _heroPlayer.transform.position;
	}
	
	public void StopHeroWalking(){
		_heroPlayer.GetComponent<PlayerView>().StopAndIdle();
	}
	
}

