using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.team.dto;
using System;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerTeamStatusChangeNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		PlayerTeamStatusChangeNotify notify = message as PlayerTeamStatusChangeNotify;
#if TEAM_DEBUGINFO
		System.Text.StringBuilder debugInfo = new System.Text.StringBuilder(string.Format("PlayerTeamStatusChangeNotify sceneId:{0} changeCount:{1}\n",notify.sceneId,notify.playerIds.Count).WrapColorWithLog());
		for(int i=0;i<notify.playerIds.Count;++i){
			debugInfo.AppendLine(string.Format("playId:{0} status:{1} teamUID:{2}",notify.playerIds[i],notify.teamStatuses[i],notify.teamUniqueIds[i]));
		}

		if(notify.indexPlayerIds.Count > 0){
			for(int i=0;i<notify.indexPlayerIds.Count;++i){
				debugInfo.AppendLine(string.Format("{0} ",notify.indexPlayerIds[i]));
			}
		}else
			debugInfo.AppendLine("indexPlayerIds is empty");
		Debug.Log(debugInfo.ToString());
#endif
		WorldManager.Instance.GetModel().HandlePlayerTeamStatusChangeNotify(notify);
		TeamModel.Instance.HandlePlayerTeamStatusChangeNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PlayerTeamStatusChangeNotify);
	}
}
