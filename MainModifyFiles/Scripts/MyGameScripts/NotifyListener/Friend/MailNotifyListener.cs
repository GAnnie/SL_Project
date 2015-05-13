using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.mail.dto;

public class MailNotifyListener : BaseDtoListener {


	override protected Type getDtoClass()
	{
		return typeof(MailDto);
	}
	
	override public void process( object message )
	{
		MailDto mailhNotify = message as MailDto;
		EmailModel.Instance.AddMailFromNotify (mailhNotify);
	}
}
