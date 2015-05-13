using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerDyeNotifyListener : BaseDtoListener {

	override protected Type getDtoClass()
	{
		return typeof(PlayerDyeNotify);
	}
	
	override public void process( object message )
	{
		PlayerDyeNotify notify = message as PlayerDyeNotify;
//		Debug.Log(string.Format("PlayerDyeNotify id:{0} dyeIds:{1} {2} {3}",notify.playerId,notify.hairDyeId,notify.dressDyeId,notify.accoutermentDyeId).WrapColorWithLog());
		WorldManager.Instance.GetModel().HandlePlayerDyeChange(notify);
	}
}
