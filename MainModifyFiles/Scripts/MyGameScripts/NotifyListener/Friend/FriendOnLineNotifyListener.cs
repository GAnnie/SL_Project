using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.friend.dto;

public class FriendOnLineNotifyListener : BaseDtoListener {


	override protected Type getDtoClass()
	{
		return typeof(FriendOnlineNotify);
	}
	
	override public void process( object message )
	{
		FriendOnlineNotify Notify = message as FriendOnlineNotify;
		
		FriendModel.Instance.FriendOnOFFLine(Notify);
	}
}
