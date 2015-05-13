using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.friend.dto;

public class FriendDegreeNotifyListener : BaseDtoListener {


	override protected Type getDtoClass()
	{
		return typeof(FriendDegreeNotify);
	}
	
	override public void process( object message )
	{
		FriendDegreeNotify Notify = message as FriendDegreeNotify;

		FriendModel.Instance.UpdateDegree(Notify);
	}
}
