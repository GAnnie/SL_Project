// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  DoubleExpDtoListener.cs
// Author   : willson
// Created  : 2015/4/1 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.dto;
using System;

public class DoubleExpDtoListener : BaseDtoListener 
{
	override protected Type getDtoClass()
	{
		return typeof(DoubleExpDto);
	}
	
	override public void process( object message )
	{
		DoubleExpDto dto = message as DoubleExpDto;
		PlayerModel.Instance.UpdateDoubleExpDto(dto);
	}
}