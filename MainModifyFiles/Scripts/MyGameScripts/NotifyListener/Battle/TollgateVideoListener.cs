using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class TollgateVideoListener : BaseDtoListener
{
	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		TollgateVideo video = message as TollgateVideo;
		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TollgateVideo);
	}	
}

