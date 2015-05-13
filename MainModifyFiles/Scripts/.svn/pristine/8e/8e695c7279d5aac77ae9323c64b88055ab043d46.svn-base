using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using System;

public class PvpVideoListener : BaseDtoListener
{

	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		PvpVideo video = message as PvpVideo;
		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PvpVideo);
	}	
}

