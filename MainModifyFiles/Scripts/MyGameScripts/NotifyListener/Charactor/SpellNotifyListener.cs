// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SpellNotifyListener.cs
// Author   : willson
// Created  : 2015/4/1 
// Porpuse  : 
// **********************************************************************

using com.nucleus.h1.logic.core.modules.player.dto;
using System;
using com.nucleus.h1.logic.core.modules.spell.dto;

public class SpellNotifyListener : BaseDtoListener 
{
	override protected Type getDtoClass()
	{
		return typeof(SpellNotify);
	}
	
	override public void process( object message )
	{
		SpellNotify notify = message as SpellNotify;
		if(notify.remainCount > 0){
			TipManager.AddTip(string.Format("你使用了修炼丹，今天还能使用{0}个修炼丹",
							notify.remainCount.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency)));
		}

		TipManager.AddTip(string.Format("你获得了{0}点{1}{2}",
	                        notify.amount.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),
	                        notify.spell.name,
	                        ItemIconConst.Exp2));

	}
}