using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using System;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class SceneMineVideoListener : BaseDtoListener
{

	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		SceneMineVideo video = message as SceneMineVideo;
		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(SceneMineVideo);
	}	
}

