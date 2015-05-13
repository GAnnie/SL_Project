using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using System;

public class BattleSceneNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		BattleSceneNotify notify = message as BattleSceneNotify;
#if TEAM_DEBUGINFO
		string idsInfo ="";
		if(notify.leaderPlayerIds.Count > 0){
			string []ids = new string[notify.leaderPlayerIds.Count];
			for(int i=0;i<notify.leaderPlayerIds.Count;++i){
				ids[i] = notify.leaderPlayerIds[i].ToString();
			}
			idsInfo = string.Join(",",ids);
		}
		Debug.Log(string.Format("BattleSceneNotify inBattle:{0} leaderPlayerIds:\n{1}",notify.inBattle,idsInfo).WrapColorWithLog());
#endif
		WorldManager.Instance.GetModel().HandleBattleSceneNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(BattleSceneNotify);
	}
}
