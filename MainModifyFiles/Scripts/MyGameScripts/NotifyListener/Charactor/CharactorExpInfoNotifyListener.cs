using System;
using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.charactor.dto;

//角色经验下发，根据charactorType角色类型判断属于哪类角色
public class CharactorExpInfoNotifyListener : BaseDtoListener {

	override public void process( object message )
	{
		CharactorExpInfoNotify expNotify = message as CharactorExpInfoNotify;

		if(expNotify.simpleCharactorDto == null 
		   || expNotify.simpleCharactorDto.charactorType == GeneralCharactor.CharactorType_MainCharactor)
		{
			PlayerModel.Instance.UpdatePlayerExpInfo(expNotify);
		}else if(expNotify.simpleCharactorDto.charactorType == GeneralCharactor.CharactorType_Pet){
			PetModel.Instance.UpdatePetExpInfo(expNotify);
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(CharactorExpInfoNotify);
	}
}
