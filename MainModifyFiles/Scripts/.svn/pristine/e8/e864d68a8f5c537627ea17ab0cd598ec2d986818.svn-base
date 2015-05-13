using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class BackTeamNotifyListener : BaseDtoListener {
	
	override public void process( object message )
	{
		BackTeamNotify notify = message as BackTeamNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("BackTeamNotify id:{0} teamUID:{1} SceneDto:{2} Video:{3}",
		                        notify.playerId,
		                        notify.teamUniqueId,
		                        notify.sceneDto==null?"null":notify.sceneDto.id.ToString(),
		                        notify.battleVideo==null?"null":notify.battleVideo.id.ToString()).WrapColorWithLog());
#endif
		if(notify.playerId == PlayerModel.Instance.GetPlayerId()){
			if(notify.battleVideo != null){
				BattleManager.Instance.PlayBattle(notify.battleVideo);
				WorldManager.Instance.EnterWithSceneDto(notify.sceneDto);
			}
			else if(notify.sceneDto != null){
				WorldManager.Instance.EnterWithSceneDto(notify.sceneDto);
        	}
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(BackTeamNotify);
	}
}
