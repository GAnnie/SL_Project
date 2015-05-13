// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WealthNotifyListener.cs
// Author   : willson
// Created  : 2014/12/30 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.dto;
using System;
using com.nucleus.h1.logic.core.modules;

public class WealthNotifyListener : BaseDtoListener 
{
	override protected Type getDtoClass()
	{
		return typeof(WealthNotify);
	}

	override public void process( object message )
	{
		WealthNotify wealthNotify = message as WealthNotify;

		if (wealthNotify.traceTypeId == H1TraceTypes.MINE_BATTLE_GAIN) 
		{
			BattleManager.Instance.AddBattleRewardNotiryList(wealthNotify);
		}
		else 
		{
			PlayerModel.Instance.UpdateWealth(wealthNotify);
		}
	}
}