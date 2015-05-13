using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.chat.modules.dto;
using com.nucleus.h1.logic.chat.modules.data;

public class ChatNotifyListener : BaseDtoListener {
	
	
	override protected Type getDtoClass()
	{
		return typeof(ChatNotify);
	}
	
	override public void process( object message )
	{
		ChatNotify Notify = message as ChatNotify;

		if(Notify.channelId == 6){
			FriendModel.Instance.AddMsg(Notify);
		}
		else
			ChatModel.Instance.addNotify (Notify);



	}
	
	
}