using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class TeamMemberDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamMemberDto notify = message as TeamMemberDto;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("TeamMemberDto id:{0} name:{1}",notify.playerId,notify.nickname).WrapColorWithLog());
#endif
		TeamModel.Instance.AddTeamMember(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamMemberDto);
	}
}
