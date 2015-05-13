// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NoRotation.cs
// Author   : SK
// Created  : 2013/8/14
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class NoRotation : MonoBehaviour
{
	public bool fixYToZero = false;
	
	private Transform localTransform = null;
	void Start()
	{
		localTransform = this.transform;
	}
	
	void LateUpdate()
	{
		localTransform.eulerAngles = Vector3.zero;
		
		if (fixYToZero){
			localTransform.position = new Vector3(localTransform.position.x, 0.01f, localTransform.position.z);
			localTransform.localPosition = new Vector3(0f, localTransform.localPosition.y, 0f);
		}		
	}
}

