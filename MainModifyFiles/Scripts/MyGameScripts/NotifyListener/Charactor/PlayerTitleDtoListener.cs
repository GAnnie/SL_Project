using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.title.dto;

public class PlayerTitleDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		PlayerTitleDto newTitle = message as PlayerTitleDto;
		PlayerModel.Instance.GainNewTitle(newTitle);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PlayerTitleDto);
	}
}
