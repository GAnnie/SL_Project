// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NpcConfigInfo.cs
// Author   : wenlin
// Created  : 2013/1/28 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcConfigInfo
{
	public int id;
	public float x = 100;
	public float y = 100;
    public float z = 100;

    public float rotateY = 0;

    public float scale = 1;

	//double teleport msg
	public int toSceneId;
    public float toX;
    public float toY;
    public float toRadius;
	public int toSceneCameraID;

    //Plot Appear
    public int missionAppear = 0; //PlotId
    //Plot DisAppear
    public int missionDisAppear = 0; //PlotId

    //POPO message
    public string popoMsg = "";
    //POPO stay time
    public float popoStayTime = 0.0f;
    //POPO delay
    public float popoDelay = 0.0f;

    //FindPath Type 
    public int findPathType = 0; // 0: No FindPathing 1 : RandomFindPathing  2: AssignationFindPathing

    public float randomFindPathRadius       = 1;
    public float randomFindPathRunbackDelay = 1;
    public float randomFindPathAIDelay      = 3;

    public List<FindPathPoint> findPathPoints;
    public int assignRunBackType        = 0;   // 0 : 直接返回   1 :原路返回
    public float assignFindPathAIDelay  = 3;

    public string defaultAnim = string.Empty;

	public NpcConfigInfo()
	{}
	
	public NpcConfigInfo( int id )
	{
		this.id = id;
        findPathPoints = new List<FindPathPoint>();
	}
}

public class FindPathPoint
{
    public float x = 0.0f;
    public float y = 0.0f;
    public float z = 0.0f;
    public float stopTime = 1.0f;

    public FindPathPoint()
    { }

    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}


