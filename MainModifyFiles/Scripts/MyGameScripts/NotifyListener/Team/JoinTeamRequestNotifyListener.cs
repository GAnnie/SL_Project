using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class JoinTeamRequestNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		JoinTeamRequestNotify notify = message as JoinTeamRequestNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("JoinTeamRequestNotify 申请者id:{0} 申请者名称:{1}",notify.requestJoinPlayerId,notify.requestJoinPlayerNickname).WrapColorWithLog());
#endif
		TeamModel.Instance.AddJoinTeamRequestNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(JoinTeamRequestNotify);
	}
}
