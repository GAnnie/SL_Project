// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldJoystick.cs
// Author   : willson
// Created  : 2014/12/3 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using System.Collections;

[RequireComponent (typeof ( FloatingJoystick ))]

public class WorldJoystick : MonoBehaviour
{
	private Transform heroGO;
	private HeroView heroPlayerView;
	private Camera cam;
	
	const int TIME_TO_GUIDE = 3;
	const float TIME_TO_CheckStand = 1.0f;
	private float checkStandTimeCounter = 0.0f;
	private int _standTime = 0;
	private float _lastStandX = 0f;
	private float _lastStandZ = 0f;
	
	const float TIME_TO_SYNC = 3.0f;
	private float timeCounter = 0.0f;
	
	private bool enableJoystick = false;
	
	private FloatingJoystick floatingJoystick;
	
	static public bool isControlByJoystick = true;
	
	private WorldClickChecker worldClickCheck;
	
	private bool moveing = false;
	
	void OnActivate( bool mChecked )
	{
		isControlByJoystick = mChecked;
		
		EnableJoystick = mChecked;
	}
	
	private void RefetchDataFromWroldClickCheck()
	{
		if ( worldClickCheck == null )
			return;
		
		if ( worldClickCheck.heroGO == null )
			return;
		
		cam = worldClickCheck.cam;
		heroGO = worldClickCheck.heroGO.transform;
		heroPlayerView = worldClickCheck.heroGO.GetComponent< HeroView >();
	}
	
	public void Init( WorldClickChecker _wcc )
	{
		worldClickCheck = _wcc;
		
		RefetchDataFromWroldClickCheck();
		
		floatingJoystick = gameObject.GetComponent< FloatingJoystick >();
		
		if ( floatingJoystick != null )
		{
			floatingJoystick.onDragEvent = JoystickMove;
			floatingJoystick.onDragEventEnd = JoystickStop;
		}
	}
	
	public bool EnableJoystick
	{
		get
		{
			return enableJoystick;
		}
		set
		{
			enableJoystick = value;
			
			if ( floatingJoystick != null )
				floatingJoystick.SetCollider( enableJoystick );
			
			timeCounter = 0.0f;
			
			if ( enableJoystick == false )
			{
				if ( floatingJoystick != null )
					floatingJoystick.ResetJoystick();
				
				//JoystickStop();
				if ( worldClickCheck != null )
					worldClickCheck.isJoystickControlling = false;
				
				return;
			}
			
			if ( worldClickCheck != null )
				worldClickCheck.isJoystickControlling = true;
			
			RefetchDataFromWroldClickCheck();
			
			//JoystickStop();
		}
	}
	
	void JoystickMove( float angle )
	{
		if ( heroPlayerView == null || heroGO == null )
		{
			RefetchDataFromWroldClickCheck();
			
			if ( heroPlayerView == null || heroGO == null )
				return;
		}
		
		if (moveing == false)
		{
			moveing = true;
			heroPlayerView.StopAndIdle();
		}
		
		Vector3 eulerAngles = cam.transform.eulerAngles;
		heroGO.eulerAngles = eulerAngles;
		
		float ang = - ( angle * Mathf.Rad2Deg - 90 );
		
		heroGO.Rotate( Vector3.up, ang, Space.Self );
		heroGO.eulerAngles = new Vector3( 0, heroGO.eulerAngles.y, 0 );
		
		Vector3 dir = heroGO.forward;
		dir.y = 0.0f;
		
		heroGO.Translate( dir * heroPlayerView.Speed * Time.deltaTime, Space.World );

		heroPlayerView.WalkWithJoystick();
	}
	
	void JoystickStop()
	{
		moveing = false;

		if ( heroPlayerView == null || heroGO == null && !heroPlayerView.enabled )
		{
			RefetchDataFromWroldClickCheck();
			
			if ( heroPlayerView == null || heroGO == null && !heroPlayerView.enabled )
				return;
		}

		heroPlayerView.StopAndIdle();
	}
	
	void Update()
	{
		if ( !enableJoystick )
			return;
		
		timeCounter += Time.deltaTime;
		if ( timeCounter >= TIME_TO_SYNC )
		{
			timeCounter = TIME_TO_SYNC - timeCounter;
			SyncWithServer();
		}
		
		checkStandTimeCounter += Time.deltaTime;
		if ( checkStandTimeCounter >= TIME_TO_CheckStand )
		{
			checkStandTimeCounter = TIME_TO_CheckStand - checkStandTimeCounter;
			CheckStandTime();
		}
	}
	
	private float _lastX = 0f;
	private float _lastZ = 0f;
	
	void SyncWithServer()
	{
		if ( heroPlayerView == null || heroGO == null )
		{
			RefetchDataFromWroldClickCheck();
			
			if ( heroPlayerView == null || heroGO == null )
				return;
		}
		
		float newX = heroGO.position.x;
		float newZ = heroGO.position.z;
		if (_lastX != newX || _lastZ != newZ)
		{
			_lastX = newX;
			_lastZ = newZ;
			WorldManager.Instance.PlanWalk(newX, newZ);
		}
	}
	
	void CheckStandTime()
	{
		if ( heroPlayerView == null || heroGO == null )
		{
			RefetchDataFromWroldClickCheck();
			
			if ( heroPlayerView == null || heroGO == null )
				return;
		}
		
		float newX = heroGO.position.x;
		float newZ = heroGO.position.z;
		if (_lastStandX != newX || _lastStandZ != newZ)
		{
			_lastStandX = newX;
			_lastStandZ = newZ;
			_standTime = 0;
		}
		else
		{
			_standTime ++;
		}
	}
}