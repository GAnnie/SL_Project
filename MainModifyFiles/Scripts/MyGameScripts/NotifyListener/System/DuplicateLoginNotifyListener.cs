using UnityEngine;
using System.Collections;
using System;
using com.nucleus.player.msg;

public class DuplicateLoginNotifyListener : BaseDtoListener
{

	override public void process( object message )
	{
		LoginManager.DuplicateLogin = true;
		GameDebuger.Log("DuplicateLoginNotifyListener");
		SocketManager.Instance.Close(true);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(DuplicateLoginNotify);
	}	
}

