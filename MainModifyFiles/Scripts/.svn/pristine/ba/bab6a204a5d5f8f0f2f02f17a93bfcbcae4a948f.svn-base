using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class SummonAwayTeamPlayersNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		SummonAwayTeamPlayersNotify notify = message as SummonAwayTeamPlayersNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("SummonAwayTeamPlayersNotify teamUID:{0}",notify.teamUniqueId).WrapColorWithLog());
#endif
		if(TeamModel.Instance.isOwnTeam(notify.teamUniqueId)){
			ProxyWindowModule.OpenConfirmWindow("队长召唤你归队,是否归队？","", ()=>{
				TeamModel.Instance.BackTeam();
			},null,UIWidget.Pivot.Center,"归队");
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(SummonAwayTeamPlayersNotify);
	}
}
