using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.charactor.dto;

//主角经验和出战宠物经验下发，一般在战斗结束时下发
using com.nucleus.h1.logic.core.modules;


public class CharactorExpNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		CharactorExpNotify expNotify = message as CharactorExpNotify;

		if (expNotify.traceTypeId == H1TraceTypes.MINE_BATTLE_GAIN) 
		{
			BattleManager.Instance.AddBattleRewardNotiryList(expNotify);
		}
		else 
		{
			GameLogicHelper.UpdateCharactorExp(expNotify);
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(CharactorExpNotify);
	}
}
