// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GameServerGradeDtoListener.cs
// Author   : willson
// Created  : 2015/1/13 
// Porpuse  : 
// **********************************************************************
using System;
using com.nucleus.h1.logic.whole.modules.system.dto;

public class GameServerGradeDtoListener : BaseDtoListener
{
	override public void process( object message )
	{
		GameServerGradeDto dto = message as GameServerGradeDto;
		PlayerModel.Instance.UpdateServerGradeDto(dto);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(GameServerGradeDto);
	}	
}