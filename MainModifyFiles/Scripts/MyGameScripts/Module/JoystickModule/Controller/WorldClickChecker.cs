// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldJoystick.cs
// Author   : willson
// Created  : 2014/12/3 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class WorldClickChecker : MonoBehaviour
{
	public Camera cam;
	public NpcViewManager npcView;
	public GameObject heroGO;
	
	[System.NonSerialized]
	public bool isJoystickControlling = false;
	
	// Use this for initialization
	void Start ()
	{
		cam = LayerManager.Instance.GameCamera;
		//------------------------ Init World Joystick ---------------------------- 
		GameObject virtualJoystick = JoystickModule.Instance.GetJoystickModuleObj();
		if (virtualJoystick != null)
		{
			WorldJoystick worldJoystick = virtualJoystick.GetComponent< WorldJoystick >();
			if ( worldJoystick != null )
			{
				worldJoystick.Init( this );
				if ( WorldJoystick.isControlByJoystick )
					worldJoystick.EnableJoystick = true;
				else
					worldJoystick.EnableJoystick = false;
			}
		}
	}
	
	const float TIME_TO_SYNC = 1.0f;
	private float timeCounter = 0.0f;	
	// Update is called once per frame
	void Update ()
	{
		if (heroGO == null){
			timeCounter = 0;
			return;
		}
		
		if (npcView != null){
			npcView.Tick();
		}
		
//		if (GamePauseController.Instance.IsPause()){
//			timeCounter = 0;
//			return;
//		}
		
//		if (UIManager.UILOCK)
//		{
//			timeCounter = 0;
//			return;
//		}
		
		timeCounter += Time.deltaTime;
		
		if ( timeCounter >= TIME_TO_SYNC )
		{
			timeCounter = TIME_TO_SYNC - timeCounter;
			
			SyncWithServer();
		}
	}
	
	private float _lastX = 0f;
	private float _lastZ = 0f;
	
	void SyncWithServer()
	{
		float newX = heroGO.transform.position.x;
		float newZ = heroGO.transform.position.z;		
		
		if (_lastX != newX || _lastZ != newZ)
		{
			_lastX = newX;
			_lastZ = newZ;
			WorldManager.Instance.VerifyWalk(newX, newZ);
		}
	}	
}
