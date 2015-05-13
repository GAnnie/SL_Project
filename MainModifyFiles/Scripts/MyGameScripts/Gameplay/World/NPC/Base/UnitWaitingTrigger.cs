// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  UnitWaitingTrigger.cs
// Author   : willson
// Created  : 2014/12/23 
// Porpuse  : 
// **********************************************************************

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitWaitingTrigger
{
	private List<TriggerNpcUnit> _units;
	//private bool _playing; // get warning
	private GameObject _heroPlayer;
	private Transform heroTransform = null;
	
	public UnitWaitingTrigger(){
		_units = new List<TriggerNpcUnit>();
		//_playing = false;
	}
	
	public void SetHeroPlayer(GameObject heroPlayer){
		_heroPlayer = heroPlayer;
		heroTransform = _heroPlayer.transform;
	}
	
	public void AddTriggerUnit(TriggerNpcUnit unit){
		_units.Add(unit);
		unit.SetHeroPlayer(_heroPlayer);
	}
	
	public void RemoveTriggerUnit(TriggerNpcUnit unit){
		_units.Remove(unit);
	}
	
	public void Tick()
	{
		foreach (TriggerNpcUnit unit in _units){
			if (unit.enabled == false)
			{
				continue;
			}
			
			//unit.FaceToHero();
			/*
			if (MissionGuidePathFinder.Instance.GetNextNpc() != null && MissionGuidePathFinder.Instance.GetNextNpc() != unit.GetNpc())
			{
				continue;
			}
			*/
			if (unit.touch)
			{
				unit.Trigger();
				
				if (unit.enabled == false)
				{
					Stop();
				}
				break;
			}
			
			if (unit.waitingTrigger)
			{
				Vector3 direction = unit.GetUnitGO().transform.position-heroTransform.position;
				if (direction.magnitude < 2)
				{
					if (unit.NeedClose() == false){
						_heroPlayer.GetComponent<PlayerView>().StopWalk(heroTransform.position);
					}
					unit.touch = true;
				}
			}
		}
	}
	
	public void Stop(){
		//_playing = false;
	}
	
	public void Play(){
		//_playing = true;
		foreach (TriggerNpcUnit unit in _units){
			unit.Reset();
		}
	}
	
	public void Destroy(){
		Stop();
		_heroPlayer = null;
	}
}