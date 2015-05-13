using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class JoinTeamDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		JoinTeamDto notify = message as JoinTeamDto;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("JoinTeamDto id:{0} name:{1}",notify.playerId,notify.nickname).WrapColorWithLog());
#endif
		TeamModel.Instance.AddTeamMember(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(JoinTeamDto);
	}
}
