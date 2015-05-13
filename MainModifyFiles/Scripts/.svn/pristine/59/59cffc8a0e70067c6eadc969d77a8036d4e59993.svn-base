using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class TeamMemberOfflineNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamMemberOfflineNotify notify = message as TeamMemberOfflineNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("TeamMemberOfflineNotify 离线队员id:{0} 新队长id:{1} 是否解散:{2}",notify.teamMemberPlayerId,notify.leaderPlayerId,notify.dismissed).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId)){
			if(notify.dismissed)
				TeamModel.Instance.CleanUpTeamInfo();
			else
			{
				TeamModel.Instance.ChangeLeader(notify.leaderPlayerId);
			}
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamMemberOfflineNotify);
	}
}
