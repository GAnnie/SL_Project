// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  VideoListener.cs
// Author   : SK
// Created  : 2014/11/11
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using System;
using com.nucleus.h1.logic.core.modules.demo.dto;

public class VideoListener : BaseDtoListener
{
	/**
	 * 处理信息
	 */
	override public void process( object message )
	{
		Video video = message as Video;
		BattleManager.Instance.PlayBattle(video);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(Video);
	}	
}

