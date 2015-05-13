using System;
using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.team.dto;

public class KickOutMemberNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		KickOutMemberNotify notify = message as KickOutMemberNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("KickOutMemberNotify 被踢队员id:{0} teamUID:{1}",notify.playerId,notify.teamUniqueId).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId))
		{
			if(notify.playerId == PlayerModel.Instance.GetPlayerId())
				TeamModel.Instance.CleanUpTeamInfo();
			else
				TeamModel.Instance.RemoveTeamMember(notify.playerId);
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(KickOutMemberNotify);
	}
}
