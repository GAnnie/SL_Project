using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using System;

public class PetAptitudeNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		PetAptitudeNotify petApNotify = message as PetAptitudeNotify;
		PetModel.Instance.HandlePetAptitudeNofity(petApNotify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PetAptitudeNotify);
	}
}
