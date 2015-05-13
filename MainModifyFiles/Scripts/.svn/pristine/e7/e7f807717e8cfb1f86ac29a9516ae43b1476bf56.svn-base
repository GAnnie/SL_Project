using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules;

public class SubWealtthNotifyListener : BaseDtoListener {

	override protected Type getDtoClass()
	{
		return typeof(SubWealthNotify);
	}
	
	override public void process( object message )
	{
		SubWealthNotify subWealthNotify = message as SubWealthNotify;
		
		if (subWealthNotify.traceTypeId == H1TraceTypes.MINE_BATTLE_GAIN) 
		{
			BattleManager.Instance.AddBattleRewardNotiryList(subWealthNotify);
		}
		else 
		{
			PlayerModel.Instance.UpdateSubWealth(subWealthNotify);
		}
	}
}
