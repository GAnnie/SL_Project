using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.demo.dto;
using System;

public class DemoVideoListener : BaseDtoListener
{
	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		DemoVideo video = message as DemoVideo;
		//场景编号:1\2\3  3D无镜头场景、3D普通镜头场景、3DBOSS镜头场景
		if (BattleDemoController.BattleCameraId == 0) {
			video.mapId = 0;
		} else {
			video.mapId = BattleDemoController.BattleSceneId;
			video.cameraId = BattleDemoController.BattleCameraId;
		}

		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(DemoVideo);
	}	
}

