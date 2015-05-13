using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.team.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.dto;

public class TeamModel
{
	private static readonly TeamModel _instance = new TeamModel ();

	public static TeamModel Instance {
		get {
			return _instance;
		}
	}

	private TeamModel ()
	{

	}

	private TeamDto _teamInfoDto;
	private TeamMemberDto _ownTeamMemberDto;
	private bool isInTeamPanel = false;
	private bool _isMatching = false;
	private TeamMatchInfoNotify _matchInfo;
	private Dictionary<string,GeneralResponse> _teamInviteNotifyDic = new Dictionary<string,GeneralResponse> ();
	private Dictionary<long,GeneralResponse> _joinTeamRequestNotifyDic = new Dictionary<long,GeneralResponse> ();

	public event System.Action OnTeamStateUpdate;
	public event System.Action OnTeamMatchInfoUpdate;
	public event System.Action OnTeamApplicationUpdate;
	public event System.Action OnTeamMatchStateUpdate;
	public event System.Action<int> OnTeamMemberInfoUpdate;

	#region Getter
	public TeamDto GetTeamInfo ()
	{
		return _teamInfoDto;
	}

	public TeamMemberDto GetOwnTeamMemberDto(){
		return _ownTeamMemberDto;
	}

	public TeamMatchInfoNotify GetMatchInfo ()
	{
		return _matchInfo;
	}

	public List<GeneralResponse> GetTeamInviteInfo ()
	{
		return new List<GeneralResponse> (_teamInviteNotifyDic.Values);
	}

	public List<GeneralResponse> GetJoinTeamRequestInfo ()
	{
		return new List<GeneralResponse> (_joinTeamRequestNotifyDic.Values);
	}

	public bool HasTeam ()
	{
		return (_teamInfoDto != null);
	}

	public bool IsTeamLeader ()
	{
		if (_teamInfoDto == null)
			return false;

		return (PlayerModel.Instance.GetPlayerId () == _teamInfoDto.leaderPlayerId);
	}

	public bool IsAway ()
	{
		if (_ownTeamMemberDto != null) {
			return _ownTeamMemberDto.memberStatus == PlayerDto.PlayerTeamStatus_Away;
		}

		return false;
	}

	public bool isFullMember ()
	{
		if (_teamInfoDto == null)
			return false;

		int maxMemberCount = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAX_TEAM_MEMBER_SIZE, 5);
		if (_teamInfoDto.teamMembers.Count >= maxMemberCount)
			return true;
		else
			return false;
	}

	public TeamMemberDto GetTeamMemberByID (long playerId)
	{
		if (_teamInfoDto == null)
			return null;

		for (int i=0; i<_teamInfoDto.teamMembers.Count; ++i) {
			if (_teamInfoDto.teamMembers[i].playerId == playerId)
				return _teamInfoDto.teamMembers[i];
		}

		return null;
	}

	public TeamMemberDto GetTeamMember (int index)
	{
		if (_teamInfoDto == null)
			return null;

		if (index < _teamInfoDto.teamMembers.Count) {
			return _teamInfoDto.teamMembers [index];
		}

		return null;
	}

	public bool isOwnTeam (string teamUID)
	{
		if (_teamInfoDto == null)
			return false;

		if (_teamInfoDto.teamUniqueId == teamUID)
			return true;

		return false;
	}

	public bool HasApplicationInfo ()
	{
		if (_teamInviteNotifyDic.Count > 0 || _joinTeamRequestNotifyDic.Count > 0)
			return true;

		return false;
	}

	public bool IsMatching(){
		return _isMatching;
	}
	#endregion

	#region 组队请求接口
	public void EnterTeamPanel ()
	{
		if(!isInTeamPanel){
			ServiceRequestAction.requestServer (TeamService.enterTeamPanel (), "EnterTeamPanel", (e) => {
				isInTeamPanel = true;
				UpdateMatchInfo (e as TeamMatchInfoNotify);
			});
		}
	}

	public void LeaveTeamPanel ()
	{
		if(isInTeamPanel){
			ServiceRequestAction.requestServer (TeamService.leaveTeamPanel (), "LeaveTeamPanel", (e) => {
				isInTeamPanel = false;
			});
		}
	}

	public void PublishTeam (int teamType, int teamLevelScopeType)
	{
		ServiceRequestAction.requestServer (TeamService.publishTeam (teamType, teamLevelScopeType), "PublishTeam", (e) => {
			TipManager.AddTip ("发布队伍成功,系统正在匹配队员，请稍后……");
			SetupMatchState(true);
		});
	}

	public void AutoMatchTeam (string teamTypesStr, int teamLevelScopeType)
	{
		ServiceRequestAction.requestServer (TeamService.pendingJoinTeam (teamTypesStr, teamLevelScopeType), "AutoMatchTeam", (e) => {
			TipManager.AddTip ("系统正在匹配队伍，请稍后……");
			SetupMatchState(true);
		});
	}

	public void CancelMatching(){
		if(IsTeamLeader()){
			ServiceRequestAction.requestServer (TeamService.cancelPublishTeam (), "Cancel PublishTeam", (e) => {
				TipManager.AddTip ("取消发布队伍成功");
				SetupMatchState(false);
			});
		}
		else{
			ServiceRequestAction.requestServer (TeamService.cancelPendingJoinTeam (), "Cancel JoinTeam", (e) => {
				TipManager.AddTip ("取消匹配队伍成功");
				SetupMatchState(false);
			});
		}
	}

	public void CreateTeam ()
	{
		ServiceRequestAction.requestServer (TeamService.createTeam (), "CreateTeam", (e) => {
			TeamDto newTeam = e as TeamDto;
			SetupTeamInfo (newTeam);

			WorldManager.Instance.GetModel().ChangePlayerTeamStatus(PlayerModel.Instance.GetPlayerId (),PlayerDto.PlayerTeamStatus_Leader,newTeam.teamUniqueId);
		});
	}

	public void JoinTeam (long leaderPlayerId, System.Action onSuccess)
	{
		ServiceRequestAction.requestServer (TeamService.joinTeam (leaderPlayerId), "JoinTeam", (e) => {
			if (onSuccess != null)
				onSuccess ();
		});
	}

	public void ApproveJoinTeam (long pendingJoinPlayerId)
	{
		ServiceRequestAction.requestServer (TeamService.approveJoinTeam (pendingJoinPlayerId), "Approve JoinTeam", (e) => {
			TeamModel.Instance.RemoveJoinTeamRequestNotify (pendingJoinPlayerId);
		}, (e) => {
			TipManager.AddTip (e.message);
			TeamModel.Instance.RemoveJoinTeamRequestNotify (pendingJoinPlayerId);
		});
	}

	public void InviteMember (long playerId, System.Action onSuccess)
	{
		ServiceRequestAction.requestServer (TeamService.inviteMember (playerId), "InviteMember", (e) => {
			if (onSuccess != null)
				onSuccess ();
		});
	}

	public void ApproveInviteMember (string pendingTeamUID)
	{
		ServiceRequestAction.requestServer (TeamService.approveInviteMember (pendingTeamUID), "Approve InviteMember", (e) => {
			TeamModel.Instance.RemoveTeamInviteNotify (pendingTeamUID);
		}, (e) => {
			TipManager.AddTip (e.message);
			TeamModel.Instance.RemoveTeamInviteNotify (pendingTeamUID);
		});
	}

	public void LeaveTeam ()
	{
		ServiceRequestAction.requestServer (TeamService.leaveTeam (), "LeaveTeam", (e) => {
			if (ValidateTeamActionStatusDto (e as TeamActionStatusDto)) {
				CleanUpTeamInfo ();
			}
		});
	}

	public void AwayTeam ()
	{
		ServiceRequestAction.requestServer (TeamService.awayTeam (), "AwayTeam", (e) => {
			ValidateTeamActionStatusDto (e as TeamActionStatusDto);
		});
	}

	public void BackTeam ()
	{
		ServiceRequestAction.requestServer (TeamService.backTeam (), "BackTeam", (e) => {
			ValidateTeamActionStatusDto (e as TeamActionStatusDto);
		});
	}

	public void AssignLeader (int index)
	{
		TeamMemberDto memberDto = GetTeamMember(index);
		if(memberDto != null){
			if(memberDto.memberStatus == PlayerDto.PlayerTeamStatus_Away)
				TipManager.AddTip("暂离状态无法提升为队长");
			else if(memberDto.memberStatus == PlayerDto.PlayerTeamStatus_Offline)
				TipManager.AddTip("离线状态无法提升为队长");
			else{
				ServiceRequestAction.requestServer (TeamService.assignLeader (memberDto.playerId), "AssignLeader", (e) => {
					ChangeLeader (memberDto.playerId);
				});
			}
		}
	}

	public void KickOutMember (int index)
	{
		long playerId = _teamInfoDto.teamMembers [index].playerId;
		ServiceRequestAction.requestServer (TeamService.kickOutMember (playerId), "KickOutMember", (e) => {
			RemoveTeamMember(playerId);
		});
	}

	public void SummonAwayTeamMembers(){
		bool canSummon = false;
		for(int i=0;i<_teamInfoDto.teamMembers.Count;++i){
			if(_teamInfoDto.teamMembers[i].memberStatus == PlayerDto.PlayerTeamStatus_Away)
			{
				canSummon = true;
				break;
			}
		}

		if(canSummon){
			ServiceRequestAction.requestServer(TeamService.summonAwayTeamMembers(),"Summon TeamMembers",(e)=>{
				TipManager.AddTip("召唤成功");
			});
		}
	}
	#endregion

	#region Game Logic
	public void SetupTeamInfo (TeamDto teamDto)
	{
		if (teamDto != null) {
			CleanUpApplicationInfo();
			SetupMatchState(false);

			_teamInfoDto = teamDto;
			for (int i=0; i<_teamInfoDto.teamMembers.Count; ++i) {
				if (_teamInfoDto.teamMembers [i].playerId == PlayerModel.Instance.GetPlayerId ()) {
					_ownTeamMemberDto = _teamInfoDto.teamMembers [i];
					break;
				}
			}
			_teamInfoDto.teamMembers.Sort (SortByIndex);

			for(int i=0;i<teamDto.teamMembers.Count;++i){
				TeamMemberDto memberDto = teamDto.teamMembers[i];

				if(memberDto.playerId == PlayerModel.Instance.GetPlayerId())
				{
					_ownTeamMemberDto = memberDto;
					JoystickModule.DisableMove = (_ownTeamMemberDto.memberStatus == PlayerDto.PlayerTeamStatus_Member);
				}
			}

			PlayerModel.Instance.OnPlayerGradeUpdate += UpdateOwnTeamMemberInfo;

			if (OnTeamStateUpdate != null)
				OnTeamStateUpdate ();
		}
	}

	public void CleanUpTeamInfo ()
	{
		if(_teamInfoDto != null){
			CleanUpApplicationInfo();
			SetupMatchState(false);

			JoystickModule.DisableMove = false;
//			_teamInfoDto.teamMembers.Remove(_ownTeamMemberDto);
			_ownTeamMemberDto = null;
			_teamInfoDto = null;

			PlayerModel.Instance.OnPlayerGradeUpdate -= UpdateOwnTeamMemberInfo;
			if (OnTeamStateUpdate != null)
				OnTeamStateUpdate ();
		}
	}

	public void ChangeLeader (long newLeaderId)
	{
		if(newLeaderId != _teamInfoDto.leaderPlayerId){
			//队长发生变更，取消自动匹配状态
			SetupMatchState(false);
			_teamInfoDto.leaderPlayerId = newLeaderId;

			if (OnTeamStateUpdate != null)
				OnTeamStateUpdate ();
		}
	}

	public void UpdateTeamMemberStatus (long playerId, int teamStatus)
	{
		TeamMemberDto memberDto = GetTeamMemberByID(playerId);
		if (memberDto != null) {
			if(memberDto.memberStatus != teamStatus){
				memberDto.memberStatus = teamStatus;
				
				if (OnTeamStateUpdate != null)
					OnTeamStateUpdate ();
				
				//设置玩家控制器
				if(playerId == PlayerModel.Instance.GetPlayerId()){
					JoystickModule.DisableMove = (teamStatus == PlayerDto.PlayerTeamStatus_Member);
				}
			}
		}
	}

	public void AddTeamMember (TeamMemberDto memberDto)
	{
		if (memberDto == null)
			return;

		for(int i=0;i<_teamInfoDto.teamMembers.Count;++i){
			if(_teamInfoDto.teamMembers[i].playerId == memberDto.playerId)
				return;
		}

		_teamInfoDto.teamMembers.Add (memberDto);
		_teamInfoDto.teamMembers.Sort (SortByIndex);

		if(OnTeamStateUpdate != null)
			OnTeamStateUpdate();
	}

	public void RemoveTeamMember (long playerId)
	{
		int removeIndex = -1;
		for (int i=0; i<_teamInfoDto.teamMembers.Count; ++i) {
			if (_teamInfoDto.teamMembers [i].playerId == playerId) {
				removeIndex = i;
				break;
			}
		}

		if (removeIndex != -1) {
			_teamInfoDto.teamMembers.RemoveAt (removeIndex);

			if (OnTeamStateUpdate != null)
				OnTeamStateUpdate ();
		}
	}

	public int SortByIndex(TeamMemberDto a,TeamMemberDto b){
		return a.index.CompareTo(b.index);
	}

	public void UpdateMatchInfo (TeamMatchInfoNotify notify)
	{
		_matchInfo = notify;
		if (OnTeamMatchInfoUpdate != null)
			OnTeamMatchInfoUpdate ();
	}

	public void UpdateOwnTeamMemberInfo(){
		UpdateTeamMemberInfo(PlayerModel.Instance.GetPlayerId(),PlayerModel.Instance.GetPlayerLevel());
	}

	public void UpdateTeamMemberInfo(long playerId,int level){
		for(int i=0;i<_teamInfoDto.teamMembers.Count;++i){
			TeamMemberDto memberDto = _teamInfoDto.teamMembers[i];
			if(memberDto.playerId == playerId && memberDto.level != level){
				memberDto.level = level;

				if(OnTeamMemberInfoUpdate != null)
					OnTeamMemberInfoUpdate(i);

				return;
			}
		}
	}

	private bool ValidateTeamActionStatusDto (TeamActionStatusDto statusDto)
	{
		if (statusDto != null) {
			if (statusDto.errorCode == 0) {
//				TipManager.AddTip ("正常");
				return true;
			} else if (statusDto.errorCode == 4003)
				TipManager.AddTip ("已离队");
			else if (statusDto.errorCode == 4000)
				TipManager.AddTip ("队伍不存在");
			else if (statusDto.errorCode == 4004)
				TipManager.AddTip ("已归队");
			else if (statusDto.errorCode == 4005)
				TipManager.AddTip ("不在同一队伍里");
		} else
			Debug.LogError ("TeamActionStatusDto is null");

		return false;
	}

	private void SetupMatchState(bool matchState){
		if(matchState != _isMatching){
			_isMatching = matchState;
			if(OnTeamMatchStateUpdate != null)
				OnTeamMatchStateUpdate();
		}
	}

	public void HandlePlayerTeamStatusChangeNotify(PlayerTeamStatusChangeNotify notify){
		if(!HasTeam())
			return;

		if(notify.indexPlayerIds.Count != _teamInfoDto.teamMembers.Count)
			return;

		for(int i=0;i<notify.playerIds.Count;++i){
			string teamUID = notify.teamUniqueIds[i];
			long playerId = notify.playerIds[i];
			int teamStatus = notify.teamStatuses[i];
			TeamMemberDto memberDto = GetTeamMemberByID(playerId);
			//只更新自己队伍的成员状态
			if(teamUID == _teamInfoDto.teamUniqueId && memberDto != null){
				memberDto.memberStatus = teamStatus;

				//设置玩家控制器
				if(playerId == PlayerModel.Instance.GetPlayerId()){
					JoystickModule.DisableMove = (teamStatus == PlayerDto.PlayerTeamStatus_Member);
				}
			}
		}

		//刷新队伍成员index值
		for(int i=0;i<notify.indexPlayerIds.Count;++i){
			long playerId = notify.indexPlayerIds[i];
			TeamMemberDto memberDto = GetTeamMemberByID(playerId);
			if(memberDto != null){
				memberDto.index = i;
			}
		}

		_teamInfoDto.teamMembers.Sort (SortByIndex);
		if(OnTeamStateUpdate != null)
			OnTeamStateUpdate();
	}
	#endregion

	#region 组队申请
	//相同的teamUID邀请入队信息仅保留一条
	public void AddTeamInviteNotify (TeamInviteNotify notify)
	{
		_teamInviteNotifyDic [notify.teamUniqueId] = notify;

		if (OnTeamApplicationUpdate != null)
			OnTeamApplicationUpdate ();
	}

	public void RemoveTeamInviteNotify (string teamUID)
	{
		_teamInviteNotifyDic.Remove (teamUID);

		if (OnTeamApplicationUpdate != null)
			OnTeamApplicationUpdate ();
	}

	//相同的joinPlayerId申请入队玩家信息仅保留最近的一条
	public void AddJoinTeamRequestNotify (JoinTeamRequestNotify notify)
	{
		_joinTeamRequestNotifyDic [notify.requestJoinPlayerId] = notify;

		if (OnTeamApplicationUpdate != null)
			OnTeamApplicationUpdate ();
	}
	
	public void RemoveJoinTeamRequestNotify (long joinPlayerId)
	{
		_joinTeamRequestNotifyDic.Remove (joinPlayerId);

		if (OnTeamApplicationUpdate != null)
			OnTeamApplicationUpdate ();
	}

	private void CleanUpApplicationInfo(){
		_teamInviteNotifyDic.Clear ();
		_joinTeamRequestNotifyDic.Clear ();

		if (OnTeamApplicationUpdate != null)
			OnTeamApplicationUpdate ();
	}
	#endregion
}
