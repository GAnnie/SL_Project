using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class TeamInviteNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamInviteNotify notify = message as TeamInviteNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("TeamInviteNotify 邀请队伍teamUID:{0} 邀请队长名称:{1}",notify.teamUniqueId,notify.leaderNickname).WrapColorWithLog());
#endif
		TeamModel.Instance.AddTeamInviteNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamInviteNotify);
	}
}
