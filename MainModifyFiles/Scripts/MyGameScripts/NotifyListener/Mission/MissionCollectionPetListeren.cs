// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionCollectionPetListeren.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.mission.dto;

public class MissionCollectionPetListeren : BaseDtoListener {
	override protected Type getDtoClass() {
		return typeof(CollectionPetSubmitNotify);
	}
	
	override public void process( object message ) {
		CollectionPetSubmitNotify notify = message as CollectionPetSubmitNotify;
		
		MissionDataModel.Instance.DeletePetByStateNotify(notify);
	}
}
