// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AutoFramWalk.cs
// Author   : willson
// Created  : 2015/3/4 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoFramWalk : MonoBehaviour 
{
	private	NavMeshAgent _navAgent;
	private float _timer;

	private int _coolDown;
	private List <Vector3> _mapPointList;

	private bool _isAutoFram;

	private HeroView _heroView;

	void Start()
	{
		_navAgent = this.gameObject.GetComponent<NavMeshAgent>();
		_heroView = WorldManager.Instance.GetHeroView();
	}

	void Update()
	{
		if(_isAutoFram)
		{
			_timer += Time.deltaTime;
			if (_timer > _coolDown) 
			{
				AutoWalk();
				_timer = 0;
				_coolDown = Random.Range (2, 10);
				return;
			}

			if(!_heroView.IsRunning())
			{
				AutoWalk();
			}
		}
	}

	public bool IsAutoFram
	{
		get
		{
			return _isAutoFram;
		}
		set
		{
			if(_isAutoFram != value)
			{
				_isAutoFram = value;
				_timer = 0;
				if(_isAutoFram)
				{
					_mapPointList = new List<Vector3>();
					_mapPointList.AddRange(NavMesh.CalculateTriangulation().vertices);
					_coolDown = Random.Range(2, 10);
				}
			}
		}
	}

	private void AutoWalk()
	{
		_navAgent.SetDestination (_mapPointList[Random.Range(0,_mapPointList.Count)]);
	}
	
}