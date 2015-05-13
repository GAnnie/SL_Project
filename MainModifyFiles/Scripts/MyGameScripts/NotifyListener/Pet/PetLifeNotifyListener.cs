using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using System;

public class PetLifeNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		PetLifeNotify notify = message as PetLifeNotify;
		PetModel.Instance.HandlePetLifeNotify(notify);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PetLifeNotify);
	}
}
