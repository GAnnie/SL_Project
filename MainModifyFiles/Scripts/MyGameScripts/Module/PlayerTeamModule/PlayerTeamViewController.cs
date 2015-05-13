using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.core.modules.team.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.formation.data;

public class PlayerTeamViewController : MonoBehaviour,IViewController
{
	private PlayerTeamView _view;
	private List<CharacterInfoItemController> _teamMemberList;
	private List<TabBtnController> _tabBtnList;

	#region TabContent_AutoGroup Member 
	private List<FilterItemController> _channelFilterItemList;
	private const int CHANNEL_GROUP_ID = 10;
	private const int RANK_GROUP_ID = 20;
	private List<FilterItemController> _rankFilterItemList;
	private List<ApplicationItemController> _applicationItemList;
	private const string ApplicationItemWidgetName = "Prefabs/Module/PlayerTeamModule/ApplicationItemWidget";
	private const string FilterItemWidgetName = "Prefabs/Module/PlayerTeamModule/FilterItemWidget";
	private GameObject _applicationItemPrefab;
	#endregion

	#region TabContent_Matrix Member
	private List<LayoutItemController> _acquiredLayoutItemList;
	private List<LayoutItemController> _moreLayoutItemList;
	private GameObject _moreLayoutItemPanel;
	private const string LayoutItemWidgetName = "Prefabs/Module/PlayerTeamModule/LayoutItemWidget";

	private const string FormationItemWidgetName = "Prefabs/Module/CrewModule/FormationItemWidget";
	private List<FormationItemSlotController> _formationItemList;
	#endregion

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PlayerTeamView> ();
		_view.Setup (this.transform);

		InitTabBtn ();
		InitAutoGroupTabContent ();
		InitMatrixTabContent ();
		RegisterEvent ();

		_UpdateMainViewState ();
		_UpdateTeamMemberList ();

		_UpdateApplicationItemList ();
		_UpdateTeamMatchInfo ();

		//加入队伍后，默认打开队伍布阵面板
		if (TeamModel.Instance.HasTeam () && !TeamModel.Instance.IsTeamLeader ())
			OnSelectMatrix ();
		else
			OnSelectAutoGroup ();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set (_view.closeBtn.onClick, CloseView);
		EventDelegate.Set (_view.joinOrLeaveBtn.onClick, OnClickJoinOrLeaveBtn);
		EventDelegate.Set (_view.callOrAwayBtn.onClick, OnClickCallOrAway);
		EventDelegate.Set (_view.moreMatrixInfoBtn.onClick,OnClickMoreMatrixInfo);

		TeamModel.Instance.OnTeamStateUpdate += OnTeamStateUpdate;
		TeamModel.Instance.OnTeamMatchInfoUpdate += _UpdateTeamMatchInfo;
		TeamModel.Instance.OnTeamApplicationUpdate += _UpdateApplicationItemList;
		TeamModel.Instance.OnTeamMatchStateUpdate += _UpdateMatchBtnState;
		TeamModel.Instance.OnTeamMemberInfoUpdate += _UpdateTeamMemberInfo;
	}

	public void Dispose ()
	{
		TeamModel.Instance.OnTeamStateUpdate -= OnTeamStateUpdate;
		TeamModel.Instance.OnTeamMatchInfoUpdate -= _UpdateTeamMatchInfo;
		TeamModel.Instance.OnTeamApplicationUpdate -= _UpdateApplicationItemList;
		TeamModel.Instance.OnTeamMatchStateUpdate -= _UpdateMatchBtnState;
		TeamModel.Instance.OnTeamMemberInfoUpdate -= _UpdateTeamMemberInfo;
		TeamModel.Instance.LeaveTeamPanel();
	}
	#endregion

	#region TeamModel Event Handler
	void OnTeamStateUpdate ()
	{
		_UpdateTeamMemberList ();
		_UpdateFormationView ();
		_UpdateMainViewState ();
		_UpdateAutoGroupContentState ();
		_UpdateMatrixContentState ();
		_UpdateMatchBtnState();

		if (TeamModel.Instance.HasTeam () && !TeamModel.Instance.IsTeamLeader ())
			OnSelectMatrix ();
		else
			OnSelectAutoGroup();
	}
	#endregion

	#region Init Func 
	private void InitTabBtn ()
	{
		_tabBtnList = new List<TabBtnController> (3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.tabBtnGrid.gameObject, tabBtnPrefab);

			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_tabBtnList.Add (com);
			_tabBtnList [i].InitItem (i, OnSelectTabBtn);
		}

		_tabBtnList [0].SetBtnName ("自动组队");
		_tabBtnList [1].SetBtnName ("阵法");
		_tabBtnList [2].SetBtnName ("伙伴布阵");
	}

	private void InitAutoGroupTabContent ()
	{
		_applicationItemList = new List<ApplicationItemController> ();
		_applicationItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ApplicationItemWidgetName) as GameObject;

		GameObject filterItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (FilterItemWidgetName) as GameObject;
		_channelFilterItemList = new List<FilterItemController> (4);
		for (int i=0; i<4; ++i) {
			GameObject item = NGUITools.AddChild (_view.channelFilterItemGrid.gameObject, filterItemPrefab);
			FilterItemController com = item.GetMissingComponent<FilterItemController> ();
			_channelFilterItemList.Add (com);
		}
		
		_channelFilterItemList [0].InitItem ("任意目标",TeamDto.TeamType_Any, 0, null);
		_channelFilterItemList [1].InitItem ("野外打怪",TeamDto.TeamType_MonsterBattle, 0, null);
		_channelFilterItemList [2].InitItem ("抓鬼任务",TeamDto.TeamType_Any, 0, null);
		_channelFilterItemList [3].InitItem ("当前活动",TeamDto.TeamType_Any, 0, OnSelectActivityFilterItem);
		
		int playerGrade = PlayerModel.Instance.GetPlayerLevel ();
		_rankFilterItemList = new List<FilterItemController> (2);
		for (int i=1; i<=2; ++i) {
			GameObject item = NGUITools.AddChild (_view.rankFilterItemGrid.gameObject, filterItemPrefab);
			FilterItemController com = item.GetMissingComponent<FilterItemController> ();
			_rankFilterItemList.Add (com);
		}
		
		int rank = DataHelper.GetStaticConfigValue (H1StaticConfigs.TEAM_LEVEL_SCOPE_MP_1, 5);
		string rankDescStr = string.Format ("{0}-{1}", Mathf.Max (0, playerGrade - rank), playerGrade + rank);
		_rankFilterItemList [0].InitItem (rankDescStr,TeamDto.TeamLevelScopeType_LevelScope1, RANK_GROUP_ID, null);
		
		rank = DataHelper.GetStaticConfigValue (H1StaticConfigs.TEAM_LEVEL_SCOPE_MP_2, 10);
		rankDescStr = string.Format ("{0}-{1}", Mathf.Max (0, playerGrade - rank), playerGrade + rank);
		_rankFilterItemList [1].InitItem (rankDescStr,TeamDto.TeamLevelScopeType_LevelScope2, RANK_GROUP_ID, null);
		
		EventDelegate.Set (_view.matchBtn.onClick, OnClickMatchBtn);

		_rankFilterItemList [0].SetSelected (true);
	}

	private void InitMatrixTabContent ()
	{	
		GameObject uiPrefab;
		//阵法选择列表初始化
		List<Formation> allFormationList = DataCache.getArrayByCls<Formation>();
		if(allFormationList != null && allFormationList.Count > 0){
			List<int> acquiredIdList= PlayerModel.Instance.GetAcquiredFormationIdList();
			uiPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (LayoutItemWidgetName) as GameObject;
			_acquiredLayoutItemList = new List<LayoutItemController> (acquiredIdList.Count);
			_moreLayoutItemList = new List<LayoutItemController> (allFormationList.Count - acquiredIdList.Count);
			
			for(int i=0;i<allFormationList.Count;++i){
				Formation formation = allFormationList[i];
				if(acquiredIdList.Contains(formation.id)){
					GameObject item = NGUITools.AddChild (_view.acquiredLayoutItemGrid.gameObject, uiPrefab);
					LayoutItemController com = item.GetMissingComponent<LayoutItemController> ();
					com.InitItem (formation,OnSelectAcquiredLayoutItem);
					_acquiredLayoutItemList.Add (com);
				}else{
					GameObject item = NGUITools.AddChild (_view.moreLayoutItemGrid.gameObject, uiPrefab);
					LayoutItemController com = item.GetMissingComponent<LayoutItemController> ();
					com.InitItem (formation,OnSelectMoreLayoutItem);
					_moreLayoutItemList.Add (com);
				}
			}
			_view.acquiredMatrixListWidget.bottomAnchor.absolute = -(45 + _acquiredLayoutItemList.Count * 45);
		}
		_moreLayoutItemPanel =_view.moreLayoutItemGrid.transform.parent.gameObject;
		_moreLayoutItemPanel.SetActive(false);

		_view.matrixSelectedEffect.SetActive(false);

		//初始化阵法槽
		uiPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(FormationItemWidgetName) as GameObject;
		_formationItemList = new List<FormationItemSlotController>(5);
		for(int i=0;i<5;++i){
			GameObject item = NGUITools.AddChild(_view.matrixItemPanel,uiPrefab);
			FormationItemSlotController com = item.GetMissingComponent<FormationItemSlotController>();
			com.InitItem(i,OnDragStartItem,OnDragEndItem);
			_formationItemList.Add(com);
		}

		int activeFormationId = PlayerModel.Instance.GetFormationCaseId(PlayerModel.Instance.ActiveFormationCaseIndex);
		for(int i=0;i<_acquiredLayoutItemList.Count;++i){
			if(activeFormationId == _acquiredLayoutItemList[i].formation.id){
				OnSelectAcquiredLayoutItem(_acquiredLayoutItemList[i]);
				break;
			}
		}
	}
	#endregion

	#region 组队界面主面板
	private void OnSelectTabBtn (int index)
	{
		if (index == 0)
			OnSelectAutoGroup ();
		else if (index == 1)
			OnSelectMatrix ();
		else if (index == 2)
			OnSelectCrewMatrix ();
	}

	private void UpdateTabBtnState (int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i) {
			if (i != selectIndex)
				_tabBtnList [i].SetSelected (false);
			else
				_tabBtnList [i].SetSelected (true);
		}
	}

	private void _UpdateMainViewState ()
	{
		if (TeamModel.Instance.HasTeam ()) {
			_view.joinOrLeaveBtnLbl.text = "离开队伍";
			_tabBtnList [1].SetBtnName ("普通阵");
		} else {
			_view.joinOrLeaveBtnLbl.text = "创建队伍";
			_tabBtnList [1].SetBtnName ("阵法");
		}
	}

	private void _UpdateTeamMemberList ()
	{
		int maxTeamMemberCount = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAX_TEAM_MEMBER_SIZE, 5);
		if (_teamMemberList == null) {
			_teamMemberList = new List<CharacterInfoItemController> (maxTeamMemberCount);
			GameObject memberItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ProxyPlayerTeamModule.CHARACTERINFO_WIDGET) as GameObject;
			for (int i=0; i<maxTeamMemberCount; ++i) {
				GameObject item = NGUITools.AddChild (_view.teamMemberGrid.gameObject, memberItemPrefab);
				CharacterInfoItemController com = item.GetMissingComponent<CharacterInfoItemController> ();
				com.InitItem (i, OnSelectTeamMember);
				_teamMemberList.Add (com);
			}
		}

		for (int i=0; i<maxTeamMemberCount; ++i) {
			_teamMemberList [i].ResetItem ();
		}

		if (TeamModel.Instance.HasTeam ()) {
			TeamDto teamInfo = TeamModel.Instance.GetTeamInfo ();
			for (int i=0; i<teamInfo.teamMembers.Count; ++i) {
				if(teamInfo.teamMembers [i].memberStatus == PlayerDto.PlayerTeamStatus_Away)
					_teamMemberList[i].SetStatusSprite("flag_away");
				else if(teamInfo.teamMembers[i].memberStatus == PlayerDto.PlayerTeamStatus_Offline)
					_teamMemberList[i].SetStatusSprite("flag_offline");

				_teamMemberList [i].SetIndexSprite(i+1);
				_teamMemberList[i].SetIcon("0");
				_teamMemberList [i].SetInfoLbl (string.Format ("{0}\n{1}级{2}", teamInfo.teamMembers [i].nickname, teamInfo.teamMembers [i].level, teamInfo.teamMembers [i].faction.name));
			}
		}
	}

	private void _UpdateTeamMemberInfo(int index){
		if(index <_teamMemberList.Count){
			TeamMemberDto memberDto = TeamModel.Instance.GetTeamMember(index);
			_teamMemberList [index].SetInfoLbl (string.Format ("{0}\n{1}级{2}", memberDto.nickname, memberDto.level, memberDto.faction.name));
		}
	}
	
	private int _curSelectTeamMemberIndex = -1;
	private void OnSelectTeamMember (int index)
	{
		if (_curSelectTeamMemberIndex != -1)
			_teamMemberList [_curSelectTeamMemberIndex].SetSelected (false);

		_curSelectTeamMemberIndex = index;
		//选中自己时，不打开额外选项面板
		TeamMemberDto memberDto = TeamModel.Instance.GetTeamMember (index);
		if (memberDto != null && memberDto.playerId != PlayerModel.Instance.GetPlayerId ()) {
			List<string> btnNameList = new List<string> (3);
			btnNameList.Add ("开始聊天");
			btnNameList.Add ("加为好友");
			btnNameList.Add ("查看信息");
			if (TeamModel.Instance.IsTeamLeader ()) {
				btnNameList.Add ("升为队长");
				btnNameList.Add ("请离队员");
			}
			MultipleSelectionManager.Open (_teamMemberList [index].gameObject, btnNameList, OnSelectTeamMemberPopItem, false);
			_teamMemberList [index].SetSelected (true);
		}
	}

	private void OnSelectTeamMemberPopItem (int index)
	{
		if (index == 0)
			TipManager.AddTip ("聊天系统暂未开放");
		else if (index == 1)
			TipManager.AddTip ("好友系统暂未开放");
		else if (index == 2)
			TipManager.AddTip ("暂不能查看玩家信息");
		else if (index == 3) {
			TeamModel.Instance.AssignLeader (_curSelectTeamMemberIndex);
		} else if (index == 4) {
			TeamModel.Instance.KickOutMember (_curSelectTeamMemberIndex);
		}
	}

	public void OnClickJoinOrLeaveBtn ()
	{
		if (TeamModel.Instance.HasTeam ()) {
			TeamModel.Instance.LeaveTeam ();
		} else {
			TeamModel.Instance.CreateTeam ();
		}
	}
	#endregion

	#region 自动组队面板
	public void OnSelectAutoGroup ()
	{
		if (!TeamModel.Instance.HasTeam () || TeamModel.Instance.IsTeamLeader ()) {
			TeamModel.Instance.EnterTeamPanel ();
			NGUITools.SetActive (_view.autoGroupContentRoot, true);
			NGUITools.SetActive (_view.matrixContentRoot, false);
			_UpdateAutoGroupContentState ();
			_UpdateMatchBtnState();
			UpdateTabBtnState (0);
		} else
			TipManager.AddTip ("你已加入队伍");
	}

	private void _UpdateApplicationItemList ()
	{
		List<GeneralResponse> infoList;
		if (TeamModel.Instance.IsTeamLeader ())
			infoList = TeamModel.Instance.GetJoinTeamRequestInfo ();
		else
			infoList = TeamModel.Instance.GetTeamInviteInfo ();

		if (_applicationItemList.Count < infoList.Count) {
			for (int i=_applicationItemList.Count; i<infoList.Count; ++i) {
				GameObject item = NGUITools.AddChild (_view.applicationItemGrid.gameObject, _applicationItemPrefab);
				ApplicationItemController com = item.GetMissingComponent<ApplicationItemController> ();
				com.InitItem ();
				_applicationItemList.Add (com);
			}
		}

		for (int i=0; i<infoList.Count; ++i) {
			_applicationItemList [i].gameObject.SetActive (true);
			_applicationItemList [i].SetData (infoList [i]);
		}
		
		for (int i=infoList.Count; i<_applicationItemList.Count; ++i) {
			_applicationItemList [i].gameObject.SetActive (false);
		}
		
		_view.applicationItemGrid.repositionNow = true;
	}

	//选中当前活动Toggle
	public void OnSelectActivityFilterItem ()
	{
//		TipManager.AddTip ("当前暂无活动");
		if (_channelFilterItemList [3].GetSelected ()) {
			_channelFilterItemList [3].SetSelected (false);
		} else {
			List<string> _activityNameList = new List<string> (3);
			_activityNameList.Add ("野外封妖");
			_activityNameList.Add ("二十八星宿");
			_activityNameList.Add ("三十六天罡");

			MultipleSelectionManager.Open (_channelFilterItemList [3].gameObject, _activityNameList,
	      	(btnIndex) => {
				_channelFilterItemList [3].SetFilterName (_activityNameList [btnIndex]);
				_channelFilterItemList [3].SetSelected (true);
			}, true);
		}
	}

	public void OnClickMatchBtn ()
	{
		if(!TeamModel.Instance.IsMatching()){
			//当前选择的等级范围
			int teamLevelScopeType = TeamDto.TeamLevelScopeType_Unknown;
			for(int i=0;i<_rankFilterItemList.Count;++i){
				if(_rankFilterItemList[i].GetSelected())
				{
					teamLevelScopeType = _rankFilterItemList[i].GetFilterId();
					break;
				}
			}

			if (TeamModel.Instance.IsTeamLeader ()) {
				int teamType = -1;
				for(int i=0;i<_channelFilterItemList.Count;++i){
					if(_channelFilterItemList[i].GetSelected()){
						teamType = _channelFilterItemList[i].GetFilterId();
						break;
					}
				}

				if(teamType != -1){
					TeamModel.Instance.PublishTeam (teamType, teamLevelScopeType);
				}
				else
					TipManager.AddTip("请勾选组队目标和等级范围再发布组队信息");
			} else {
				List<string> selectedTeamTypes = new List<string>(4);
				for(int i=0;i<_channelFilterItemList.Count;++i){
					if(_channelFilterItemList[i].GetSelected())
						selectedTeamTypes.Add(_channelFilterItemList[i].GetFilterId().ToString());
				}

				if(selectedTeamTypes.Count > 0){
					string teamTypesStr = string.Join(",",selectedTeamTypes.ToArray());
					TeamModel.Instance.AutoMatchTeam (teamTypesStr, teamLevelScopeType);
				}else
					TipManager.AddTip("请勾选组队目标和等级范围再发布组队信息");
			}
		}
		else{
			TeamModel.Instance.CancelMatching();
		}
	}

	private void _UpdateAutoGroupContentState ()
	{
		bool hasTeam = TeamModel.Instance.HasTeam ();
		SetApplicationListTitle (hasTeam);
		SetFilterListState (hasTeam);
	}

	//加入队伍后，隐藏"任意目标"频道，且变为单选框
	private void SetFilterListState (bool hasTeam)
	{
		int groupId = hasTeam ? CHANNEL_GROUP_ID : 0;
		for (int i=0; i<_channelFilterItemList.Count; ++i) {
			_channelFilterItemList [i].SetSelected (false);
			_channelFilterItemList [i].SetToggleGroup (groupId);
		}

		_channelFilterItemList [0].gameObject.SetActive (!hasTeam);
		_view.channelFilterItemGrid.Reposition ();
	}

	private void _UpdateMatchBtnState ()
	{
		bool hasTeam = TeamModel.Instance.HasTeam();
		bool isMatching = TeamModel.Instance.IsMatching();
		if(!isMatching){
			if (hasTeam) {
				_view.matchBtnLbl.text = "发布组队";
			} else {
				_view.matchBtnLbl.text = "加入队列";
			}
		}
		else
			_view.matchBtnLbl.text = "取消排队";
	}

	private void SetApplicationListTitle (bool hasTeam)
	{
		if (hasTeam)
			_view.applicationListTitle.text = StringHelper.WrapColor ("申请列表", "ffd84c");
		else
			_view.applicationListTitle.text = StringHelper.WrapColor ("队伍列表", "ffd84c");
	}

	private void _UpdateTeamMatchInfo ()
	{
		TeamMatchInfoNotify matchInfo = TeamModel.Instance.GetMatchInfo ();
		if (matchInfo != null)
			_view.matchInfoLbl.text = string.Format ("当前队长排队数量：{0}\n当前队员排队人数：{1}", matchInfo.pendingTeamSize, matchInfo.pendingMemberSize);
		else
			_view.matchInfoLbl.text = "";
	}
	#endregion

	#region 队伍布阵面板
	public void OnSelectMatrix ()
	{
		if (TeamModel.Instance.HasTeam ()) {
			TeamModel.Instance.LeaveTeamPanel ();
			NGUITools.SetActive (_view.autoGroupContentRoot, false);
			NGUITools.SetActive (_view.matrixContentRoot, true);
			_UpdateMatrixContentState ();
			UpdateTabBtnState (1);
		} else
			TipManager.AddTip ("您不在组队状态，不能设置阵法");
	}

	private void _UpdateMatrixContentState ()
	{
		SetCallOrAwayBtnState ();
	}

	public void OnClickMoreMatrixInfo(){
		_moreLayoutItemPanel.SetActive(!_moreLayoutItemPanel.activeSelf);
	}

	public void OnClickCallOrAway ()
	{
		if (TeamModel.Instance.IsTeamLeader ()) {
			TeamModel.Instance.SummonAwayTeamMembers();
		} else {
			if (TeamModel.Instance.IsAway ())
				TeamModel.Instance.BackTeam ();
			else
				TeamModel.Instance.AwayTeam ();
		}
	}

	private void SetCallOrAwayBtnState ()
	{
		if (TeamModel.Instance.IsTeamLeader ()) {
			_view.callOrAwayBtnLbl.text = "召唤";
		} else {
			if (TeamModel.Instance.IsAway ())
				_view.callOrAwayBtnLbl.text = "归队";
			else
				_view.callOrAwayBtnLbl.text = "暂离";
		}
	}

	private LayoutItemController _lastSelectAcquireLayoutItem;
	private Formation _curSelectFormation;
	private void OnSelectAcquiredLayoutItem (LayoutItemController com)
	{
		if(_lastSelectAcquireLayoutItem != null)
			_lastSelectAcquireLayoutItem.SetSelected(false);
		
		_lastSelectAcquireLayoutItem = com;
		_lastSelectAcquireLayoutItem.SetSelected(true);
		_curSelectFormation = com.formation;

		if(com.formation.id != PlayerModel.Instance.GetFormationCaseId(PlayerModel.Instance.ActiveFormationCaseIndex)){
			_UpdateFormationView();
		}else
			_UpdateFormationView();
	}

	private void OnSelectMoreLayoutItem (LayoutItemController com)
	{
		_curSelectFormation = com.formation;
		_UpdateFormationView();
	}

	public void _UpdateFormationView(){
		_view.layoutDescLbl.text = _curSelectFormation.description;
		for(int i=0;i<_formationItemList.Count;++i){
			Vector3 itemPos = CrewModel.Instance.GetFormationPos(_curSelectFormation.playerPosition[i]);
			_formationItemList[i].transform.localPosition = new Vector3(itemPos.x*79.5f,itemPos.y*76.8f,0f);
			TeamMemberDto memberDto = TeamModel.Instance.GetTeamMember(i);
			if(memberDto != null){
				_formationItemList[i].SetIcon("unknow");
				_formationItemList[i].SetIcon("unknow");
				_formationItemList[i].SetNameLbl(memberDto.nickname);
				_formationItemList[i].SetPosLbl(i+1);
				_formationItemList[i].CanDrag = (TeamModel.Instance.IsTeamLeader() && i!=0);
			}else
				_formationItemList[i].ResetItem();
		}
	}

	private void OnDragStartItem(){
		_view.matrixSelectedEffect.SetActive(true);
	}

	private void OnDragEndItem(int dragItemIndex){
		_view.matrixSelectedEffect.SetActive(true);
	}
	#endregion  

	public void OnSelectCrewMatrix ()
	{
		ProxyCrewModule.Open();
	}

	public void CloseView ()
	{
		ProxyPlayerTeamModule.Close ();
	}
}
