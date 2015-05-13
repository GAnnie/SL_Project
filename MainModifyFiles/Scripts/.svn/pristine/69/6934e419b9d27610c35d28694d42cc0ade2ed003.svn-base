
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class AwayTeamNotifyListener : BaseDtoListener {
	
	override public void process( object message )
	{
		AwayTeamNotify notify = message as AwayTeamNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("AwayTeamNotify id:{0} teamUID:{1}",notify.playerId,notify.teamUniqueId).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId))
			TeamModel.Instance.UpdateTeamMemberStatus(notify.playerId,PlayerDto.PlayerTeamStatus_Away);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(AwayTeamNotify);
	}
}
