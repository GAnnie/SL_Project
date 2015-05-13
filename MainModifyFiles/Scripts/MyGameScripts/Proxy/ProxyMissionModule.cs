// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	ProxyMissionModule.cs
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

public class ProxyMissionModule {
	#region Mission
	private const string NAME_MissionView = "Prefabs/Module/MissionModule/MissionView";
	
	public static void Open() {
		GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_MissionView, UILayerType.DefaultModule, true);
		
		var controller = ui.GetMissingComponent<MissionController>();
		controller.Open();
	}

	public static void Close() {
		UIModuleManager.Instance.CloseModule (NAME_MissionView);
	}
	#endregion
}
