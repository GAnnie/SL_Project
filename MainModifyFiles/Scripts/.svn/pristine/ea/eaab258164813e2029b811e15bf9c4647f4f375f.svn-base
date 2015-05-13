using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.chat.modules.dto;

public class ChatHearsayNotifyListener : BaseDtoListener {

	override protected Type getDtoClass()
	{
		return typeof(HearsayNotify);
	}
	
	override public void process( object message )
	{
		HearsayNotify Notify = message as HearsayNotify;
		ChatModel.Instance.AddHearsayNotify (Notify);
		MarqueeModel.Instance.AddHear(Notify.content);
	}

}
