using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.title.dto;
using System;

public class PlayerTitleNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		PlayerTitleNotify notify = message as PlayerTitleNotify;
		if(PlayerModel.Instance.GetPlayerId() == notify.playerId) 
			PlayerModel.Instance.UpdateTitleId(notify.titleId);
		
		WorldManager.Instance.GetModel().HandlePlayerTitleChange(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PlayerTitleNotify);
	}
}
