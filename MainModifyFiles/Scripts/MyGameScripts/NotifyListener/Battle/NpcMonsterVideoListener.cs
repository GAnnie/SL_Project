using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class NpcMonsterVideoListener : BaseDtoListener
{

	override public void process( object message )
	{
		NpcMonsterVideo video = message as NpcMonsterVideo;
		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(NpcMonsterVideo);
	}	
}

