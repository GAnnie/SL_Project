using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.team.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.mission.data;

public class MainUIExpandContentController : MonoBehaviour,IViewController {

	private const string TEAM_ITEM ="Prefabs/Module/MainUIModule/ExpandContentTeamItem";
	GameObject _teamItemPrefab = null;
	private const string MISSION_ITEM ="Prefabs/Module/MissionModule/MissionCellView";
	GameObject _missionItemPrefab = null;
	MainUIExpandContentView _view = null;

	public enum ExpandContentType {
		nothing = 0,
		mission = 1,
		team = 2
	}
	private ExpandContentType _expandContentType = ExpandContentType.nothing;

	#region IViewController implementation
	public void InitView () {
		if (_view == null) {
			_view = gameObject.GetMissingComponent<MainUIExpandContentView> ();
			_view.Setup(this.transform);
			RegisterEvent();

			if (_teamItemPrefab == null) {
				_teamItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(TEAM_ITEM) as GameObject;
			}
			if (_missionItemPrefab == null) {
				_missionItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(MISSION_ITEM) as GameObject;
			}
		}
		_expandContentType = ExpandContentType.nothing;
		RefreshMissionCallback();
		UpdateTeamItemView();
		//	默认选择任务
		OnClickTaskBtn();
	}

	public void RegisterEvent () {
		EventDelegate.Set(_view.taskBtn.onClick, OnClickTaskBtn);
		EventDelegate.Set(_view.teamBtn.onClick, OnClickTeamBtn);
		EventDelegate.Set(_view.callOrAwayBtn.onClick, OnClickCallOrAway);
		EventDelegate.Set(_view.joinOrLeaveBtn.onClick,OnClickJoinOrLeaveBtn);

		MissionDataModel.Instance.refreshUICallback += RefreshMissionCallback;
		TeamModel.Instance.OnTeamStateUpdate += UpdateTeamItemView;
		TeamModel.Instance.OnTeamMemberInfoUpdate += UpdateTeamMemberInfo;
	}

	public void Dispose () {
		MissionDataModel.Instance.refreshUICallback -= RefreshMissionCallback;
		TeamModel.Instance.OnTeamStateUpdate -= UpdateTeamItemView;
		TeamModel.Instance.OnTeamMemberInfoUpdate -= UpdateTeamMemberInfo;
	}
	#endregion

	public void ToggleContentRoot(){
		_view.posTweener.Toggle();
		_view.alphaTweener.Toggle();
	}

	private void OnClickTaskBtn(){
		if (_expandContentType != ExpandContentType.mission) {
			_expandContentType = ExpandContentType.mission;

			_view.TabCotentRoot_UISprite.enabled = false;
			
			ChangeTabContentState(true);
		} else {
			ProxyMissionModule.Open();
		}
	}

	private void OnClickTeamBtn(){
		if (_expandContentType != ExpandContentType.team) {
			_expandContentType = ExpandContentType.team;
			
			_view.TabCotentRoot_UISprite.enabled = true;
			
			ChangeTabContentState(false);
		} else {
			ProxyPlayerTeamModule.Open();
		}
	}

	private void ChangeTabContentState(bool missionActive) {
		_view.taskTabContent.SetActive(missionActive);
		_view.teamTabContent.SetActive(!missionActive);
		_view.itemScrollView.ResetPosition();

		_view.taskBtnSelectEffect.spriteName = missionActive? "expandButton-on" : "expandButton-off";
		_view.teamBtnSelectEffect.spriteName = missionActive? "expandButton-off" : "expandButton-on";
		if(missionActive)
			SetTeamBtnGroupActive(false);
		else{
			if(!TeamModel.Instance.HasTeam())
				SetTeamBtnGroupActive(true);
		}
	}

	#region 任务相关
	private List<MissionCellController> _missionItemList = new List<MissionCellController>();
	private void RefreshMissionCallback() {
		//	获取当前任务列表
		List<Mission> tCurMissionMenuList = MissionDataModel.Instance.GetSubMissionMenuListInMainUIExpand();

		for (int i = 0, len = _missionItemList.Count; i < len; i++) {
			_missionItemList[i].gameObject.SetActive(i < tCurMissionMenuList.Count);
			if (i < tCurMissionMenuList.Count) {
				Mission tMission = tCurMissionMenuList[i];
				
				MissionCellController tMissionCellController = _missionItemList[i];
				tMissionCellController.name = string.Format("{0}_{1}", tMission.type, tMission.id);
				tMissionCellController.SetMissionCellData(tMission, MissionCellCallback);
			}
		}
		
		for (int i = _missionItemList.Count, len = tCurMissionMenuList.Count; i < len; i++) {
			Mission tMission = tCurMissionMenuList[i];
			
			GameObject go = NGUITools.AddChild(_view.taskItemTable_UITable.gameObject, _missionItemPrefab);
			go.name = string.Format("{0}_{1}", tMission.type, tMission.id);
			
			MissionCellController tMissionCellController = go.AddMissingComponent<MissionCellController>();
			tMissionCellController.SetMissionCellData(tMission, MissionCellCallback);
			
			_missionItemList.Add(tMissionCellController);
			
			//	默认选择菜单（注释代码同上）
		}

		_view.taskItemTable_UITable.Reposition();
		//_view..ResetPosition();
	}

	//	回调
	private void MissionCellCallback(MissionCellController missionCellController) {
		Mission tMission = missionCellController.GetMissionInMissionCell();
		GameDebuger.OrangeDebugLog(string.Format("点击MissionCell回调 | Mission ID:{0} | Name:{1} - {2}",
		                                         tMission.id, tMission.missionType.name, tMission.name));
		MissionDataModel.Instance.FindToMissionNpcByMission(tMission, true);
	}

	#endregion

	#region 队伍相关
	private List<ExpandContentTeamItemController> _teamItemList = new List<ExpandContentTeamItemController>(5);
	private void UpdateTeamItemView(){
		if(TeamModel.Instance.HasTeam()) {
			_view.teamItemGrid.gameObject.SetActive(true);
			_view.teamMatrixInfoRoot.SetActive(true);
			_view.teamMatrixInfoLbl.text = "普通阵";

			List<TeamMemberDto> teamMemberInfo = TeamModel.Instance.GetTeamInfo().teamMembers;
			if(_teamItemList.Count < teamMemberInfo.Count) {
				for(int i=_teamItemList.Count;i<teamMemberInfo.Count;++i){
					GameObject item = NGUITools.AddChild (_view.teamItemGrid.gameObject, _teamItemPrefab);
					ExpandContentTeamItemController com = item.GetMissingComponent<ExpandContentTeamItemController> ();
					com.InitItem(i,OnSelectTeamItem);
					_teamItemList.Add (com);
				}
			}

			for(int i=0;i<teamMemberInfo.Count;++i){
				TeamMemberDto memberInfo = teamMemberInfo[i];
				_teamItemList[i].gameObject.SetActive(true);
//				_teamItemList[i].SetIcon("");
				_teamItemList[i].SetFactionIcon(memberInfo.factionId);
				_teamItemList[i].SetInfoLbl(string.Format("{0} {1}",memberInfo.level,memberInfo.nickname));
				if(memberInfo.memberStatus == PlayerDto.PlayerTeamStatus_Away)
					_teamItemList[i].SetStatusLbl("[b]暂_"+memberInfo.index);
				else if(memberInfo.memberStatus == PlayerDto.PlayerTeamStatus_Offline)
					_teamItemList[i].SetStatusLbl("[b]离_"+memberInfo.index);
				else if(memberInfo.memberStatus == PlayerDto.PlayerTeamStatus_Leader)
					_teamItemList[i].SetStatusLbl("[b]长_"+memberInfo.index);
				else if(memberInfo.memberStatus == PlayerDto.PlayerTeamStatus_Member)
					_teamItemList[i].SetStatusLbl("[b]员_"+memberInfo.index);
				else
					_teamItemList[i].SetStatusLbl("");
			}

			for(int i=teamMemberInfo.Count;i<_teamItemList.Count;++i) {
				_teamItemList[i].gameObject.SetActive(false);
			}

			_view.teamItemGrid.repositionNow = true;
		}
		else {
			_view.teamMatrixInfoRoot.SetActive(false);
			_view.teamItemGrid.gameObject.SetActive(false);
			_view.teamMatrixInfoLbl.text = "";
		}

		UpdateTeamGroupBtnState();
	}

	private void UpdateTeamMemberInfo(int index){
		if(index < _teamItemList.Count){
			TeamMemberDto memberDto = TeamModel.Instance.GetTeamMember(index);
			_teamItemList[index].SetInfoLbl(string.Format("{0} {1}",memberDto.level,memberDto.nickname));
		}
	}

	public void OnSelectTeamItem(int index){
		SetTeamBtnGroupActive(true);
	}

	public void OnClickCallOrAway ()
	{
		if(TeamModel.Instance.HasTeam()){
			if (TeamModel.Instance.IsTeamLeader ()) {
				TeamModel.Instance.SummonAwayTeamMembers();
			} else {
				if (TeamModel.Instance.IsAway ())
					TeamModel.Instance.BackTeam ();
				else
					TeamModel.Instance.AwayTeam ();
			}
		}else{
			TeamModel.Instance.CreateTeam ();
			ProxyPlayerTeamModule.Open();
		}
	}
	
	private void UpdateTeamGroupBtnState ()
	{
		if(TeamModel.Instance.HasTeam()){
			if (TeamModel.Instance.IsTeamLeader ()) {
				_view.callOrAwayBtnLbl.text = "召唤";
			} else {
				if (TeamModel.Instance.IsAway ())
					_view.callOrAwayBtnLbl.text = "归队";
				else
					_view.callOrAwayBtnLbl.text = "暂离";
			}
			_view.joinOrLeaveBtnLbl.text = "退队";
			SetTeamBtnGroupActive(false);
		}else{
			SetTeamBtnGroupActive(true);
			_view.callOrAwayBtnLbl.text = "创建";
			_view.joinOrLeaveBtnLbl.text = "找队";
		}
	}

	public void OnClickJoinOrLeaveBtn ()
	{
		if (TeamModel.Instance.HasTeam ()) {
			TeamModel.Instance.LeaveTeam ();
		} else {
			ProxyPlayerTeamModule.Open();
		}
	}

	public void SetTeamBtnGroupActive(bool active){
		_view.teamBtnGroupWidget.cachedGameObject.SetActive(active);
		_view.teamBtnGroupWidget.topAnchor.absolute = active?52:0;
	}
	#endregion
}
