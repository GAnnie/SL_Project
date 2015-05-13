// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldModel.cs
// Author   : willson
// Created  : 2014/12/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.scene.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.team.dto;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.title.dto;

public class WorldModel
{
	private SceneDto _sceneDto;
	private Dictionary<long,SimplePlayerDto> _playersDic;

	public WorldModel()
	{
		_playersDic = new Dictionary<long, SimplePlayerDto>(10);
	}

	public event System.Action<SimplePlayerDto> OnAddPlayer;
	public event System.Action<SimplePlayerDto> OnUpdatePlayer;
	public event System.Action<long> OnRemovePlayer;
	public event System.Action<long,float,float> OnUpdatePlayerPos;
	public event System.Action<long> OnChangeTeamStatus;
	public event System.Action<long,bool> OnChangeBattleStatus;
	public event System.Action<long,int> OnChangeWeapon;
	public event System.Action<long,int> OnChangePlayerTitle;
	public event System.Action<long> OnChangePlayerDye;

	#region Debug
	public string DumpPlayerDicInfo(){
		System.Text.StringBuilder debugInfo = new System.Text.StringBuilder();
		foreach(SimplePlayerDto playerDto in _playersDic.Values){
			debugInfo.AppendLine(string.Format("playId:{0} status:{1} index:{2} inBattle:{3} teamUID:{4}",
			                                   playerDto.id,
			                                   playerDto.teamStatus,
			                                   playerDto.teamIndex,
			                                   playerDto.inBattle,playerDto.teamUniqueId));
		}

		return debugInfo.ToString();
	}
	#endregion

	#region Getter
	public SceneDto GetSceneDto()
	{
		return _sceneDto;
	}

	public int GetSceneId(){
		if(_sceneDto == null) return 0;
		return _sceneDto.id;
	}
	
	public string GetBattleSceneName()
	{
		string battleSceneName = "Battle_" + _sceneDto.sceneMap.battleMapId;
		return battleSceneName;
	}

	public Dictionary<long,SimplePlayerDto> GetPlayersDic(){
		return _playersDic;
	}

	public SimplePlayerDto GetPlayerDto(long playerId)
	{
		if(_sceneDto != null){
			if(_playersDic.ContainsKey(playerId))
				return _playersDic[playerId];
		}

		return null;
	}

	public bool GetPlayerBattleStatus(long playerId){
		if(_sceneDto != null){
			if(_playersDic.ContainsKey(playerId))
				return _playersDic[playerId].inBattle;
		}

		return false;
	}

	public SimplePlayerDto GetTeamLeader(string teamUID){
		foreach(SimplePlayerDto player in _playersDic.Values){
			if(teamUID == player.teamUniqueId && player.teamStatus == PlayerDto.PlayerTeamStatus_Leader)
				return player;
		}

		return null;
	}
	#endregion

	#region Game Logic
	public void SetupSceneDto(SceneDto sceneDto)
	{
		_sceneDto = sceneDto;
		_playersDic.Clear();
		if (_sceneDto != null)
		{
			foreach(SimplePlayerDto playerDto in _sceneDto.playerDtos){
				_playersDic.Add(playerDto.id,playerDto);
			}
		}
    }

    public bool UpdatePlayer(SimplePlayerDto playerDto)
	{
		if(_playersDic.ContainsKey(playerDto.id))
		{
			_playersDic[playerDto.id] = playerDto;

			if(OnUpdatePlayer != null)
				OnUpdatePlayer(playerDto);
			return true;
		}
		
		return false;
	}

	public void AddPlayer(SimplePlayerDto playerDto)
	{
		//如果存在，直接更新已有玩家数据
		if(!UpdatePlayer(playerDto)){
			_playersDic.Add(playerDto.id,playerDto);

			if(OnAddPlayer != null)
				OnAddPlayer(playerDto);
		}
	}

	public bool RemovePlayer(long id)
	{
		bool remove =_playersDic.Remove(id);
		if(remove){
			if(OnRemovePlayer != null)
				OnRemovePlayer(id);
		}

		return remove;
	}

	public void UpdatePlayerPos(long playerId,float x,float z){
		if(_playersDic.ContainsKey(playerId))
		{
			SimplePlayerDto playerDto = _playersDic[playerId];
			playerDto.x = x;
			playerDto.z = z;

			if(OnUpdatePlayerPos != null)
				OnUpdatePlayerPos(playerId,x,z);
		}
	}

	public void ChangePlayerTeamStatus(long playerId,int teamStatus,string teamUID){
		if(_playersDic.ContainsKey(playerId)){
				_playersDic[playerId].teamStatus = teamStatus;
				_playersDic[playerId].teamUniqueId = teamUID;

				if(OnChangeTeamStatus != null)
					OnChangeTeamStatus(playerId);
		}
	}

	public void ChangePlayerBattleStatus(long playerId,bool inBattle){
		if(_playersDic.ContainsKey(playerId)){
			_playersDic[playerId].inBattle = inBattle;
			if(OnChangeBattleStatus != null)
				OnChangeBattleStatus(playerId,inBattle);
		}
	}
	#endregion

	#region Notify Handler
	public void HandleEnterSceneNotify(EnterSceneNotify notify){
		if(notify.scene.id == _sceneDto.id){
			AddPlayer(notify.player);
			GameDebuger.Log(string.Format("Player EnterScene: id:{0} name:{1}",notify.player.id,notify.player.nickname));
		}
	}

	public void HandleLeaveSceneNotify(LeaveSceneNotify notify){
		if(notify.scene.id == _sceneDto.id)
		{
			if(RemovePlayer(notify.player.id)){
				GameDebuger.Log(string.Format("Player LeaveScene: id:{0} name:{1}",notify.player.id,notify.player.nickname));
			}
		}
	}

	public void HandleKickoutSceneNotify(KickoutSceneNotify notify){
		if(notify.scene.id == _sceneDto.id)
		{
			if(RemovePlayer(notify.player.id)){
				GameDebuger.Log(string.Format("Player KickOutScene: id:{0} name:{1}",notify.player.id,notify.player.nickname));
            }
        }
	}

	public void HandlePlanWalkPointNotify(PlanWalkPointNotify notify){
		if(notify.sceneId == _sceneDto.id){
			UpdatePlayerPos(notify.playerId,notify.x,notify.z);
		}
	}

	//需要更新完WorldModel的所有组队状态信息再广播OnChangeTeamStatusList消息
	public void HandlePlayerTeamStatusChangeNotify(PlayerTeamStatusChangeNotify notify){
		if(notify.sceneId == _sceneDto.id){
			HashSet<long> changedPlayerIdSet = new HashSet<long>();
			for(int i=0;i<notify.playerIds.Count;++i){
				long playerId = notify.playerIds[i];
				int teamStatus = notify.teamStatuses[i];
				string teamUID = notify.teamUniqueIds[i];
				if(_playersDic.ContainsKey(playerId)){
					_playersDic[playerId].teamStatus = teamStatus;
					//状态变更为NOTeam时，将teamUID清空
					if(teamStatus == PlayerDto.PlayerTeamStatus_NoTeam)
						_playersDic[playerId].teamUniqueId = "";
					else
						_playersDic[playerId].teamUniqueId = teamUID;

					changedPlayerIdSet.Add(playerId);
				}
			}

			//更新玩家队伍索引值
			if(notify.indexPlayerIds.Count > 0){
				for(int i=0;i<notify.indexPlayerIds.Count;++i){
					long playerId = notify.indexPlayerIds[i];
					if(_playersDic.ContainsKey(playerId)){
						_playersDic[playerId].teamIndex = i;
						changedPlayerIdSet.Add(playerId);
					}
				}
			}

			foreach(long playerId in changedPlayerIdSet){
				if(OnChangeTeamStatus != null)
					OnChangeTeamStatus(playerId);
			}
        }
	}

	public void HandleMainCharactorUpgradeSceneNotify(MainCharactorUpgradeSceneNotify notify){
		if(_playersDic.ContainsKey(notify.playerId)){
			_playersDic[notify.playerId].grade = notify.level;
		}
	}

	public void HandleBattleSceneNotify(BattleSceneNotify notify){
		for(int i=0;i<notify.leaderPlayerIds.Count;++i){
			ChangePlayerBattleStatus(notify.leaderPlayerIds[i],notify.inBattle);

			SimplePlayerDto leaderDto = null;
			if(_playersDic.TryGetValue(notify.leaderPlayerIds[i],out leaderDto)){
				if(leaderDto.teamStatus == PlayerDto.PlayerTeamStatus_Leader){
					//更新属于该队伍的队员战斗状态
					foreach(SimplePlayerDto teamMember in _playersDic.Values){
						if(teamMember.teamStatus == PlayerDto.PlayerTeamStatus_Member && teamMember.teamUniqueId == leaderDto.teamUniqueId){
							ChangePlayerBattleStatus(teamMember.id,notify.inBattle);
						}
					}
				}
			}
		}
	}

	public void HandlePlayerWeaponChange (WeaponNotify weaponNotify)
	{
		if(_playersDic.ContainsKey(weaponNotify.playerId)){
			_playersDic[weaponNotify.playerId].wpmodel = weaponNotify.wpmodel;

			if (OnChangeWeapon != null)
			{
				OnChangeWeapon(weaponNotify.playerId, weaponNotify.wpmodel);
			}
		}
	}

	public void HandlePlayerTitleChange(PlayerTitleNotify notify){
		if(_playersDic.ContainsKey(notify.playerId)){
			_playersDic[notify.playerId].titleId = notify.titleId;
			if(OnChangePlayerTitle != null)
				OnChangePlayerTitle(notify.playerId,notify.titleId);
		}
	}

	public void HandlePlayerDyeChange(PlayerDyeNotify notify){
		if(_playersDic.ContainsKey(notify.playerId)){
			_playersDic[notify.playerId].hairDyeId = notify.hairDyeId;
			_playersDic[notify.playerId].dressDyeId = notify.dressDyeId;
			_playersDic[notify.playerId].accoutermentDyeId = notify.accoutermentDyeId;
			if(OnChangePlayerDye != null)
				OnChangePlayerDye(notify.playerId);
		}
	}
	#endregion
}