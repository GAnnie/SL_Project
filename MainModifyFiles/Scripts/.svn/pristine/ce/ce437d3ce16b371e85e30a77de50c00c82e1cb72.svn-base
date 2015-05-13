// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldManager.cs
// Author   : willson
// Created  : 2014/12/1 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.commons.dispatcher;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.team.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.scene.data;

public class WorldManager
{
	private static readonly WorldManager _instance = new WorldManager ();

	public static WorldManager Instance {
		get {
			return _instance;
		}
	}

	private WorldModel _worldModel;
	private WorldView _worldView;
	private WorldNotifyListener _listener;

	public static bool FirstEnter = true;

	private WorldManager ()
	{
		_worldModel = new WorldModel ();
		_worldView = new WorldView (_worldModel);
		_listener = new WorldNotifyListener (_worldModel);
	}

	public event System.Action<SceneDto> OnSceneChanged;

	private bool _needDelayLoadScene;		//用于标记是否需要加载新场景
	private Vector3 _cachedGameCameraPos;

	#region Getter
	public WorldModel GetModel ()
	{
		return _worldModel;
	}

	public WorldView GetView ()
	{
		return _worldView;
	}

	public NpcViewManager GetNpcViewManager ()
	{
		return _worldView.GetNpcViewManager ();
	}

	public HeroView GetHeroView ()
	{
		return _worldView.GetHeroView ();
	}

	public CharacterController GetHeroCharacterController() {
		return _worldView.GetHeroCharacterController();
	}
	#endregion

	#region 跳转场景
	public void Enter (int sceneId = 0, bool isTeleport = false,bool isAutoFramEnter = false)
	{
		if(JoystickModule.DisableMove){
			TipManager.AddTip("你正在组队无法跳转场景");
			return;
		}

		GameDebuger.Log ("Enter scene: " + sceneId);
        if (!isAutoFramEnter)
        {
            PlayerModel.Instance.StopAutoFram();
        }
        else
        {
            PlayerModel.Instance.IsAutoFram = true;
        }

		if (sceneId == 0) {
			sceneId = PlayerModel.Instance.GetPlayer ().sceneId;
		}
		GoToNextScene (sceneId, isTeleport);
	}

	private void GoToNextScene (int sceneId, bool isTeleport)
	{
		LayerManager.UILOCK = true;
		 
		if(isTeleport == false)
		{
			if (FirstEnter)
			{
				ServiceRequestAction.requestServer (SceneService.enter (sceneId), "请求进入场景", (e) => {
					EnterWithSceneDto (e as SceneDto);
				});
			}
			else
			{
				ServiceRequestAction.requestServer (SceneService.fly (sceneId), "请求进入场景", (e) => {
					EnterWithSceneDto (e as SceneDto);
				});
			}
		}
		else
		{
			ServiceRequestAction.requestServer (SceneService.enter (sceneId), "请求进入场景", (e) => {
				EnterWithSceneDto (e as SceneDto);
			});
		}
	}

	public void EnterWithSceneDto (SceneDto sceneDto)
	{
		if(sceneDto == null)
			return;

		ProxyWorldMapModule.CloseNpcDialogue ();
		ProxyWorldMapModule.CloseMiniMap ();

		Debug.Log ("EnterWithSceneDto = " + sceneDto.id + " " + sceneDto.name);

		PlayerModel.Instance.GetPlayer ().sceneId = sceneDto.id;

		int oldSceneId = _worldModel.GetSceneId ();
		_worldModel.SetupSceneDto (sceneDto);
		_listener.Start ();

		//在战斗中，延迟加载场景
		if (BattleManager.Instance.IsInBattle ()) {
			_needDelayLoadScene = true;
		} else {
			Debug.Log("EnterWithSceneDto " + sceneDto.id + " " + oldSceneId);
			if (sceneDto.id != oldSceneId) {
				if (FirstEnter)
				{
					FirstEnter = false;
					OnLoadMapFinish();
					NGUIFadeInOut.FadeIn();
				}
				else
				{
					//不在战斗状态且场景ID不同，进行新场景的加载
					SceneFadeEffectController.Show (sceneDto, CleanUpWorldView, OnLoadMapFinish);
				}
			} else {
				Debug.Log("相同场景ID但SceneDto发生了变更，更新WorldView的数据");
				//相同场景ID但SceneDto发生了变更，更新WorldView的数据
				CleanUpWorldView ();
				_worldView.InitView ();
			}
		}

		if (OnSceneChanged != null) {
			OnSceneChanged (sceneDto);
		}
	}
	#endregion

	private void CleanUpWorldView ()
	{
		_worldView.Destroy ();
	}

	public void Destroy()
	{
		_worldModel.SetupSceneDto (null);
		_listener.Stop ();
		CleanUpWorldView ();
	}

	private void OnLoadMapFinish ()
	{
		LayerManager.UILOCK = false;

		_worldView.InitView ();
	}

	private void ResetCamera ()
	{
		if (_worldView != null) {
			_worldView.CameraController.cameraMove.SyncTargetPos();
			_worldView.CameraController.cameraMove.target = null;
		}

		_cachedGameCameraPos = LayerManager.Instance.GameCameraGo.transform.localPosition;

		if (BattleManager.NeedBattleMap)
		{
			LayerManager.Instance.GameCameraGo.transform.localPosition = new Vector3 (0, 0, 0);
		}
	}

	public void HideScene ()
	{
		ResetCamera ();

		LayerManager.Instance.SwitchLayerMode (UIMode.Mode_Battle);

		if (BattleManager.NeedBattleMap)
		{
			LightmapSettings.lightmaps = LightmapManager.GetLightmaps (BattleDemoController.BattleSceneId);
		}
	}

	public void ResumeScene ()
	{
		LayerManager.Instance.SwitchLayerMode (UIMode.Mode_Game);

		if (_worldModel.GetSceneDto() != null)
		{
			LightmapSettings.lightmaps = LightmapManager.GetLightmaps (_worldModel.GetSceneDto ().sceneMap.resId);
		}
		
		//战斗结束,检查是否需要加载新场景
		if (_needDelayLoadScene) {
			SceneFadeEffectController.Show (_worldModel.GetSceneDto (), CleanUpWorldView, OnLoadMapFinish);
			_needDelayLoadScene = false;
		} else {
			LayerManager.Instance.GameCameraGo.transform.localPosition = new Vector3 (_cachedGameCameraPos.x, _cachedGameCameraPos.y, _cachedGameCameraPos.z);
			if (_worldView.CameraController.heroGO != null)
			{
				_worldView.CameraController.cameraMove.target = _worldView.CameraController.heroGO.transform;
			}
		}
	
		if (FirstEnter)
		{
			Enter();
		}
	}

	public void PlayCameraAnimator (int sceneId, int cameraId)
	{
		if (cameraId > 0) {
			_worldView.CameraController.PlayCameraAnimator (sceneId, cameraId);
		} 
	}

	#region WalkNotify Request
	//没有队伍或者是队长时才发送验证信息
	public void PlanWalk (float targetX, float targetZ)
	{
		if (!BattleManager.Instance.IsInBattle () && !LayerManager.UILOCK) {
			if (!JoystickModule.DisableMove)
				ServiceRequestAction.requestServer (SceneService.planWalk (_worldModel.GetSceneId (), targetX, targetZ));
		}
	}

	//WorldClickCheck 每隔一段时间请求服务器验证玩家当前位置
	public void VerifyWalk (float toX, float toY)
	{
		if (!BattleManager.Instance.IsInBattle () && !LayerManager.UILOCK) {
			if (!JoystickModule.DisableMove)
				ServiceRequestAction.requestServer (SceneService.verifyWalk (_worldModel.GetSceneId (), toX, toY));
		}
	}
	#endregion

	#region 寻路
	private Npc _targetNpc;

	public void CheckTargetNpc(){
		if(_targetNpc != null) {
			//	任务：查看是否寻路可穿越传送阵，设置CharacterController.enable MissionDataModel.Instance.heroCharacterControllerEnable
			if (MissionDataModel.Instance.heroCharacterControllerEnable) {
				WalkToByNpc(_targetNpc);
			} else {
				WalkToByNpc(_targetNpc, _toTargetCallback);
			}
		}
	}

	public void WalkToByNpcId(int npcId){
		Npc target = DataCache.getDtoByCls<Npc>(npcId);
		WalkToByNpc(target);
	}

	private System.Action _toTargetCallback = null;
	public void WalkToByNpc(Npc npc, System.Action toTargetCallback = null){
		if(npc == null) 
		{
			Debug.LogError("Npc is null,can not to find");
			return;
		}

		_toTargetCallback = toTargetCallback;
		MissionDataModel.Instance.HeroCharacterControllerEnable(toTargetCallback == null);

		if(_worldModel.GetSceneId() == npc.sceneId){
			INpcUnit iNpcUnit = _worldView.GetNpcViewManager().GetNpcUnit(npc);
			if(iNpcUnit != null){
				iNpcUnit.Trigger();
			} else {
				float tX = GetHeroView().cachedTransform.position.x;
				float tZ = GetHeroView().cachedTransform.position.z;
				if (npc.x == tX && npc.z == tZ) {
					return;
				}

				Vector3 tLastNpcPos = new Vector3(npc.x/10, npc.y, npc.z/10);
				NavMeshHit hit;
				NavMesh.SamplePosition(tLastNpcPos, out hit, 10, -1);
				//tLastNpcPos = MissionDataModel.GetSceneStandPosition(tLastNpcPos, Vector3.zero);
				GetHeroView().WalkToPoint(hit.position, toTargetCallback);
			}
			_targetNpc = null;
		}else{
			_targetNpc = npc;
			Enter(npc.sceneId,false);
		}
	}
	#endregion
}