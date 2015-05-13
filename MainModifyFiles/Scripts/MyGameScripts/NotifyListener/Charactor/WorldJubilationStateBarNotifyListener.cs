using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class WorldJubilationStateBarNotifyListener : BaseDtoListener {

	override protected Type getDtoClass()
	{
		return typeof(WorldJubilationStateBarNotify);
	}
	
	override public void process( object message )
	{
		WorldJubilationStateBarNotify notify = message as WorldJubilationStateBarNotify;
		PlayerBuffModel.Instance.UpdateWorldJubilationState(notify);
	}
}
