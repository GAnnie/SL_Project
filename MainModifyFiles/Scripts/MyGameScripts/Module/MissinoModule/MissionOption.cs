// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionOption.cs
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

using com.nucleus.h1.logic.core.modules.mission.data;

public class MissionOption {
	private Mission _mission = null;
	public Mission mission {
		get { return this._mission; }
	}

	private bool _optionType = false;
	public bool optionType {
		get { return this._optionType; }
	}

	public MissionOption (Mission mission, bool optionState) {
		this._mission = mission;
		this._optionType = optionState;
	}
}
