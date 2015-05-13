// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldNotifyListener.cs
// Author   : willson
// Created  : 2014/12/2 
// Porpuse  : 
// **********************************************************************

using System.Collections;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;

public class WorldNotifyListener : IDtoListenerExcute
{
	private MultipleNotifyListener _multiListener;
	private WorldModel _worldModel;

	public WorldNotifyListener(WorldModel worldModel){
		_worldModel = worldModel;
	}

	public void ExcuteDto(object dto)
	{
		if (dto is EnterSceneNotify)
		{
			_worldModel.HandleEnterSceneNotify(dto as EnterSceneNotify);
		}
		else if (dto is KickoutSceneNotify)
		{
			_worldModel.HandleKickoutSceneNotify(dto as KickoutSceneNotify);
		}
		else if (dto is LeaveSceneNotify)
		{
			_worldModel.HandleLeaveSceneNotify(dto as LeaveSceneNotify);
		}
		else if (dto is PlanWalkPointNotify)
		{
			_worldModel.HandlePlanWalkPointNotify(dto as PlanWalkPointNotify);
		}
		else if (dto is WeaponNotify)
		{
			_worldModel.HandlePlayerWeaponChange(dto as WeaponNotify);
		}
	}

	public void Start()
	{
		if (_multiListener == null)
		{
			_multiListener = new MultipleNotifyListener();
			_multiListener.AddNotify(typeof(EnterSceneNotify));
			_multiListener.AddNotify(typeof(LeaveSceneNotify));
			_multiListener.AddNotify(typeof(KickoutSceneNotify));
			_multiListener.AddNotify(typeof(PlanWalkPointNotify));
			_multiListener.AddNotify(typeof(WeaponNotify));
			_multiListener.Start(this);
		}
	}

	public void Stop()
	{
		if (_multiListener != null)
		{
			_multiListener.Stop();
			_multiListener = null;
		}
	}
}