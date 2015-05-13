using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerMeritsNotifyListener : BaseDtoListener {

	override protected Type getDtoClass()
	{
		return typeof(PlayerMeritsNotify);
	}
	
	override public void process( object message )
	{
		PlayerMeritsNotify notify = message as PlayerMeritsNotify;
		PlayerModel.Instance.UpdateMerits(notify.merits);
	}
}
