using System;
using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class SceneDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		SceneDto sceneDto = message as SceneDto;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("SceneDto sceneId:{0} 场景名:{1} 场景人数:{2}",sceneDto.id,sceneDto.name,sceneDto.playerDtos.Count).WrapColorWithLog());
#endif
		WorldManager.Instance.EnterWithSceneDto(sceneDto);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(SceneDto);
	}
}
