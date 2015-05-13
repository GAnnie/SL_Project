// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldView.cs
// Author   : willson
// Created  : 2014/12/2 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.team.dto;

public class WorldView
{
	private WorldModel _worldModel;
	private CameraController _cameraController;
	private Dictionary<long, PlayerView> _playerViewDic;
	private HeroView _heroPlayerView;
	private CharacterController _heroCharacterController;
	private NpcViewManager _npcViewManager;

	public WorldView (WorldModel model)
	{
		_worldModel = model;
		_cameraController = LayerManager.Instance.GameCameraGo.GetComponentInChildren<CameraController> ();
		_playerViewDic = new Dictionary<long, PlayerView> (10);
		_npcViewManager = new NpcViewManager ();
	}

	#region Getter
	public CameraController CameraController {
		get {
			return _cameraController;
		}
	}

    public HeroView GetHeroView ()
	{
		return _heroPlayerView;
	}

	public PlayerView GetPlayerView(long playerId){
		PlayerView playerView = null;
		_playerViewDic.TryGetValue (playerId, out playerView);
		return playerView;
	}

	public CharacterController GetHeroCharacterController() {
		if (_heroCharacterController == null) {
			_heroCharacterController = _heroPlayerView.gameObject.GetComponent<CharacterController>();
		}
		return _heroCharacterController;
	}

	public NpcViewManager GetNpcViewManager ()
	{
		return _npcViewManager;
	}
	#endregion

	public void InitView ()
	{
		_worldModel.OnAddPlayer += NewPlayerEnterScene;
		_worldModel.OnRemovePlayer += RemovePlayerView;
		_worldModel.OnChangeTeamStatus += UpdatePlayerViewTeamStatus;
		_worldModel.OnChangeBattleStatus += UpdatePlayerViewBattleStatus;
		_worldModel.OnUpdatePlayerPos += UpdatePlayerViewPos;
		_worldModel.OnChangeWeapon += UpdatePlayerWeapon;
		_worldModel.OnChangePlayerTitle += UpdatePlayerTitle;
		_worldModel.OnChangePlayerDye += UpdatePlayerDye;

		InitPlayers ();
		InitClickChecker ();
		InitNpc ();
	
		//场景加载完播放剧情
		StoryManager.Instance.JudgeStory();

		//	重新刷新任务面板
		MissionDataModel.Instance.GetSubMissionMenuListInMainUIExpand();
	}

	private void InitClickChecker ()
	{
		WorldClickChecker clickChecker = LayerManager.Instance.WorldActors.GetComponent<WorldClickChecker> ();
		if (clickChecker == null) {
			clickChecker = LayerManager.Instance.WorldActors.AddComponent<WorldClickChecker> ();
		}
		
		clickChecker.npcView = _npcViewManager;
		clickChecker.heroGO = _heroPlayerView.cachedGameObject;
	}

	private void InitPlayers ()
	{
		Dictionary<long,SimplePlayerDto> playersDic = _worldModel.GetPlayersDic ();
		foreach (SimplePlayerDto playerDto in playersDic.Values) {
			AddPlayerView (playerDto);
		}

		_cameraController.heroGO = _heroPlayerView.cachedGameObject;
		_cameraController.SetupCamera (LayerManager.Instance.GameCamera.gameObject);

		//初始化玩家组队状态
		foreach (SimplePlayerDto playerDto in playersDic.Values) {
			UpdatePlayerViewTeamStatus(playerDto.id);
		}
	}

	private void InitNpc ()
	{
		_npcViewManager.Start (_worldModel, _heroPlayerView.cachedGameObject);
	}

	private void AddPlayerView (SimplePlayerDto playerDto)
	{
		if (playerDto.sceneId != _worldModel.GetSceneId ()) {
			GameDebuger.Log ("AddPlayer Error uid=" + playerDto.id + " form " + playerDto.sceneId + " to " + _worldModel.GetSceneId ());
			return;
		}

		if (_playerViewDic.ContainsKey (playerDto.id)) {
			ResetPlayerViewPos (playerDto.id, playerDto.x, playerDto.z);
		} else {
			GameObject playerGo = new GameObject ();
			PlayerView newPlayerView;

			GameObjectExt.AddPoolChild (LayerManager.Instance.WorldActors, playerGo);

			CharacterController characterController = playerGo.GetMissingComponent<CharacterController> ();
			characterController.center = new Vector3 (0f, 0.75f, 0f);
			characterController.radius = 0.4f;
			characterController.height = 2f;

			playerGo.tag = GameTag.Tag_Player;
			Vector3 position = SceneHelper.GetSceneStandPosition (new Vector3 (playerDto.x,0f, playerDto.z), Vector3.zero);
			playerGo.transform.position = position;
            
			if (playerDto.id == PlayerModel.Instance.GetPlayerId ()) {
				_heroPlayerView = playerGo.AddComponent<HeroView> ();
				newPlayerView = _heroPlayerView;
				
				playerGo.name = "hero";
				playerGo.AddMissingComponent<JoystickTigger> ();
				playerGo.AddMissingComponent<PlayerUpgradeEffect>();
				newPlayerView.SetupPlayerDto (playerDto, true);

				// 加载完成如果是挂机状态,则执行挂机操纵
				if(PlayerModel.Instance.IsAutoFram)
				{
					PlayerModel.Instance.StartAutoFram();
				}
				else
				{
					_heroPlayerView.SetAutoFram(PlayerModel.Instance.IsAutoFram);
				}
			} else {
				newPlayerView = playerGo.AddComponent<PlayerView> ();
				playerGo.name = "player_" + playerDto.id;
				newPlayerView.SetupPlayerDto (playerDto, false);
			}

			_playerViewDic.Add (playerDto.id, newPlayerView);
		}
	}

	#region 人数测试
	List<long> robotIdList  = new List<long>();
	Dictionary<long,PlayerView> robotList = new Dictionary<long, PlayerView>();
	List<RobotAutoWalk> robotWalkList = new List<RobotAutoWalk>();
	public void addRobotPlayer(SimplePlayerDto playerDto,int count = 1){

		GameObject playerGo = new GameObject ();
		PlayerView newPlayerView;
		
		GameObjectExt.AddPoolChild (LayerManager.Instance.WorldActors, playerGo);
		
		CharacterController characterController = playerGo.GetMissingComponent<CharacterController> ();
		characterController.center = new Vector3 (0f, 0.75f, 0f);
		characterController.radius = 0.4f;
		characterController.height = 2f;
		
		playerGo.tag = GameTag.Tag_Player;
		
		Vector3 position = SceneHelper.GetRobotSceneStandPosition (new Vector3 (playerDto.x,0f, playerDto.z), Vector3.zero);
		playerGo.transform.position = position;
//		playerGo.transform.rotation = Quaternion.Euler( new Vector3 (0, Random.Range (0, 360), 0));
		newPlayerView = playerGo.AddComponent<PlayerView> ();
		playerGo.name = "Robot" + playerDto.id;
		RobotAutoWalk w = playerGo.GetMissingComponent<RobotAutoWalk> ();
		newPlayerView.SetupPlayerDto (playerDto, false);
		playerGo.GetMissingComponent<NavMeshAgent> ();
		robotList.Add (playerDto.id,newPlayerView);
		robotIdList.Add (playerDto.id);

		for (int i = 0; i < count; i++) {
			RobotInfo.Instance.SetRobotList(w);		
		}

		RobotInfo.Instance.ShowObjList (playerGo);
	}

	public void DelRobotPLayerList(){
		for (int i = 0; i <robotList.Count; i++) {
//			RemovePlayerView(robotList[i]);
			PlayerView playerView = null;
			if (robotList.TryGetValue (robotIdList[i], out playerView)) {
				playerView.DestroyMe ();
			}
		}
		robotIdList.Clear ();
		robotList.Clear ();
		RobotInfo.Instance.DelRobotList ();
	}

	public int RobotCount(){
		if(robotList != null)
			return robotList.Count;
		else return 0;
	}
	#endregion


	private void NewPlayerEnterScene(SimplePlayerDto playerDto){
		AddPlayerView(playerDto);
		UpdatePlayerViewTeamStatus(playerDto.id);
	}

	private void RemovePlayerView (long playerId)
	{
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			Debug.Log("RemovePlayerView " + playerId);
			playerView.DestroyMe ();
			_playerViewDic.Remove (playerId);
		}
	}

	private void ResetPlayerViewPos (long playerId, float x, float z)
	{
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			Vector3 position = SceneHelper.GetSceneStandPosition (new Vector3 (x, 0, z), Vector3.zero);
			playerView.StopWalk (position);
		}
	}

	private void UpdatePlayerViewTeamStatus (long playerId)
	{
		SimplePlayerDto playerDto = _worldModel.GetPlayerDto(playerId);
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			playerView.UpdateTeamStatus();
		}		
	}

	private void UpdatePlayerViewBattleStatus(long playerId,bool inBattle){
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			playerView.SetFightFlag(inBattle);
		}
	}

	private void UpdatePlayerViewPos (long playerId, float x, float z)
	{
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			Vector3 position = SceneHelper.GetSceneStandPosition (new Vector3 (x, 0, z), Vector3.zero);
			playerView.WalkToPoint (position);
		}
	}

	private void UpdatePlayerWeapon (long playerId, int wpmodel)
	{
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			playerView.UpdateWeapon(wpmodel);
		}
	}

	private void UpdatePlayerTitle (long playerId,int titleId){
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			playerView.UpdatePlayerName();
		}
	}

	private void UpdatePlayerDye (long playerId){
		PlayerView playerView = null;
		if (_playerViewDic.TryGetValue (playerId, out playerView)) {
			playerView.UpdateModelHSV();
		}
	}

	public void Destroy ()
	{
		_worldModel.OnAddPlayer -= NewPlayerEnterScene;
		_worldModel.OnRemovePlayer -= RemovePlayerView;
		_worldModel.OnChangeTeamStatus -= UpdatePlayerViewTeamStatus;
		_worldModel.OnChangeBattleStatus -= UpdatePlayerViewBattleStatus;
		_worldModel.OnUpdatePlayerPos -= UpdatePlayerViewPos;
		_worldModel.OnChangeWeapon -= UpdatePlayerWeapon;
		_worldModel.OnChangePlayerTitle -= UpdatePlayerTitle;
		_worldModel.OnChangePlayerDye -= UpdatePlayerDye;

		_cameraController.heroGO = null;

		foreach (PlayerView playerView in _playerViewDic.Values) {
			playerView.DestroyMe ();
		}

		_playerViewDic.Clear ();
		_npcViewManager.Destroy ();
	}
}