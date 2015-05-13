// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PackItemDiscardNotifyListener.cs
// Author   : willson
// Created  : 2015/3/27 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.dto;
using System;

public class PackItemDiscardNotifyListener : BaseDtoListener 
{
	override protected Type getDtoClass()
	{
		return typeof(PackItemDiscardNotify);
	}
	
	override public void process( object message )
	{
		PackItemDiscardNotify dto = message as PackItemDiscardNotify;
		for(int index = 0;index < dto.uniqueIds.Count;index++)
		{
			BackpackModel.Instance.DeleteHideItem(dto.uniqueIds[index]);
		}
	}
}