#define PET_DEBUGINFO
using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.charactor.dto;

public class PetCharactorDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		PetCharactorDto petDto = message as PetCharactorDto;
#if PET_DEBUGINFO
		Debug.Log(string.Format("PetCharactorDto petUID:{0} name:{1}",petDto.id,petDto.name).WrapColorWithLog("olive"));
#endif
		PetModel.Instance.AddPet(petDto);
	}
	
	override protected Type getDtoClass()
	{
		return typeof(PetCharactorDto);
	}
}
