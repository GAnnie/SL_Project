using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class TeamMatchInfoNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamMatchInfoNotify notify = message as TeamMatchInfoNotify;
		TeamModel.Instance.UpdateMatchInfo(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamMatchInfoNotify);
	}
}
