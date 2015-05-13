using System;
using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.team.dto;

public class TeamMemberUpgradeNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamMemberUpgradeNotify notify = message as TeamMemberUpgradeNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("TeamMemberUpgradeNotify id:{0} level:{1} teamUID:{2}",notify.playerId,notify.level,notify.teamUniqueId).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId))
			TeamModel.Instance.UpdateTeamMemberInfo(notify.playerId,notify.level);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamMemberUpgradeNotify);
	}
}
