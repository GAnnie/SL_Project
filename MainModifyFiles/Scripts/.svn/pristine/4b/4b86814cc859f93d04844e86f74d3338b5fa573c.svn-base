// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MultipleNotifyListener.cs
// Author   : SK
// Created  : 2013/3/1
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MultipleNotifyListener : IDtoListenerExcute
{
	private List<MessageProcessor> _listener;
	private List<Type> _notifyClass;
	private IDtoListenerExcute _excuter;
	
	public MultipleNotifyListener()
	{
		_listener = new List<MessageProcessor>();
		_notifyClass = new List<Type>();
	}
	
	public void AddNotify(Type notifyClass)
	{
		_notifyClass.Add(notifyClass);
	}
	
	public void Start(IDtoListenerExcute excuter)
	{
		_excuter = excuter;
		foreach(Type notifyClass in _notifyClass){
			_listener.Add(NotifyListenerRegister.regDtoExcuter(notifyClass, this));
		}
		_notifyClass.Clear();
	}
	
	public void Stop()
	{
		foreach(MessageProcessor p in _listener){
			SocketManager.Instance.removeMessageProcessor(p);
		}
		_listener = null;
		_excuter = null;
		_notifyClass = null;		
	}
	
	public void ExcuteDto(object dto){
		if (_excuter != null){
			_excuter.ExcuteDto(dto);
		}
	}
}

