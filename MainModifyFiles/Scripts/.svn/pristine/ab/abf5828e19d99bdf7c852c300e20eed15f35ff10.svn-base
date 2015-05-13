using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;

public class LeaderChangeNotifyListener : BaseDtoListener {
	
	override public void process( object message )
	{
		LeaderChangeNotify notify = message as LeaderChangeNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("LeaderChangeNotify 新队长id:{0} teamUID:{1}",notify.playerId,notify.teamUniqueId).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId))
			TeamModel.Instance.ChangeLeader(notify.playerId);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(LeaderChangeNotify);
	}
}
