﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NavigationPoins.cs
// Author   : willson
// Created  : 2014/12/15 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class NavigationPoin
{
	private Vector3 _position;

	private bool _canMove;

	public NavigationPoin(Vector3 position)
	{
		// transform.position 世界坐标
		// transform.localPosition 本地坐标


		_position = position;
		for(int index = 0;index < 100;index++)
		{
			RaycastHit hit;
			Ray ray;

			if(index > 0)
			{
				//x => Random.Range(0F, 1.0F)
				//z => Random.Range(0F, -1.0F)
				ray = new Ray(new Vector3(_position.x + Random.Range(0F, 1.0F), 200, _position.z + Random.Range(0F, -1.0F)), new Vector3(0, -1, 0));
			}
			else
			{
				ray = new Ray(new Vector3(_position.x, 200, _position.z), new Vector3(0, -1, 0));
			}

			_canMove = Physics.Raycast(ray, out hit, 250, -1);
			if(_canMove == false)
			{
				break;
			}
		}
	}

	public Vector3 GetPosition()
	{
		return _position;
	}

	public bool canMove()
	{
		return _canMove;
	}

	public string ToString()
	{
		return _position.x + ":" + _position.z + ":" + (_canMove?"1":"0");
	}
}