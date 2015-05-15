using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.player.model;

public class TreasureMapNotifyLiitener : BaseDtoListener {


	override protected Type getDtoClass()
	{
		return typeof(TreasureEventNotify);
	}
	
	override public void process( object message )
	{
		TreasureEventNotify Notify = message as TreasureEventNotify;
		ProxyTreasureMapModule.Open(Notify);
	}

}
