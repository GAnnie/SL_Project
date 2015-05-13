// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaseDtoExcuteListener.cs
// Author   : SK
// Created  : 2013/3/1
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;

public class BaseDtoExcuteListener : BaseDtoListener
{
	private Type _clsName;
	private IDtoListenerExcute _excuter;
	
	public BaseDtoExcuteListener(Type clsName, IDtoListenerExcute excuter){
		_clsName = clsName;
		_excuter = excuter;
	}

	/**
	 * 处理信息
	 */
	override public void process( object message ){
		_excuter.ExcuteDto(message);
	}
	
	override protected Type getDtoClass(){
		return _clsName;
	}
}

