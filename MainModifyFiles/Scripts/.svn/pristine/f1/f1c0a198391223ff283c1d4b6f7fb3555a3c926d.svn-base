// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaseDtoListener.cs
// Author   : SK
// Created  : 2013/1/30
// Purpose  : 基本的的DTO监听处理, 需要被子类继承实现
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;

public class BaseDtoListener : MessageProcessor
{

	/**
	 * 获取激发消息处理器的事件类型
	 * 一般用 getQualifiedClassName(XXXDto);
	 */
	public string getEventType(){
		return getDtoClass().ToString();
	}

	/**
	 * 处理信息
	 */
	virtual public void process( object message ){
	}
	
	virtual protected Type getDtoClass(){
		return null;
	}	
}

