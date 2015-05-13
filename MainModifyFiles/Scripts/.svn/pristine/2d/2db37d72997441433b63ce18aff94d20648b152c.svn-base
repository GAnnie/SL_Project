using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using System;

public class MainCharactorUpgradeSceneNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		MainCharactorUpgradeSceneNotify notify = message as MainCharactorUpgradeSceneNotify;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("MainCharactorUpgradeSceneNotify id:{0} level:{1}",notify.playerId,notify.level).WrapColorWithLog());
#endif
		WorldManager.Instance.GetModel().HandleMainCharactorUpgradeSceneNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(MainCharactorUpgradeSceneNotify);
	}
}
