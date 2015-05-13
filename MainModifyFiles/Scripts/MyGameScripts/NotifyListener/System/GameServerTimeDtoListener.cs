using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.system.dto;

public class GameServerTimeDtoListener : BaseDtoListener
{

	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		GameServerTimeDto dto = message as GameServerTimeDto;
		//Debug.Log ("GameServerTimeDtoListener = " + dto.time);
		SystemTimeManager.Instance.SyncServerTime (dto.time);
		PlayerModel.Instance.GetPlayer().gameServerTime = dto.time;
	}
	
	override protected Type getDtoClass()
	{
		return typeof(GameServerTimeDto);
	}	
}

