using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class LeaveTeamNotifyListener : BaseDtoListener {
	
	override public void process( object message )
	{
		LeaveTeamNotify notify = message as LeaveTeamNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("LeaveTeamNotify 离队队员id:{0} 新队长id:{1} 是否解散:{2}",notify.playerId,notify.leaderPlayerId,notify.dissmissed).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId))
		{
			if(notify.dissmissed)
				TeamModel.Instance.CleanUpTeamInfo();
			else
			{
				TeamModel.Instance.RemoveTeamMember(notify.playerId);
				TeamModel.Instance.ChangeLeader(notify.leaderPlayerId);
			}
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(LeaveTeamNotify);
	}
}
