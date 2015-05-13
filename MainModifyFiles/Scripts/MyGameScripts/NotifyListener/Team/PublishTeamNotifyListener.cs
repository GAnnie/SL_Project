using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;

public class PublishTeamNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		PublishTeamNotify notify = message as PublishTeamNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("PublishTeamNotify teamUID:{0} 队伍类型:{1}",notify.teamUniqueId,notify.teamType).WrapColorWithLog());
#endif
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PublishTeamNotify);
	}
}
