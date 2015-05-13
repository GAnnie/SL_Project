using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.chat.modules.dto;


public class ChatSysNotifyListener : BaseDtoListener {

	
	override protected Type getDtoClass()
	{
		return typeof(SystemNotify);
	}
	
	override public void process( object message )
	{
		SystemNotify Notify = message as SystemNotify;
		ChatModel.Instance.AddSysNotify (Notify);
	}
}
