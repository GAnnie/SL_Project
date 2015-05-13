// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SubObject.cs
// Author   : wenlin
// Created  : 2013/6/18 14:34:10
// Purpose  : 场景上Prefab的子节点的数据体。  为了满足存在Prefab有子节点修改了Transform, 若子节点修改了Transform，则保存下来， 如果没有修改到， 则不作记录
// **********************************************************************

using System;
using UnityEngine;
using System.Collections.Generic;

public class SubObject
{
    public string objectName;
	
	public SceneObjectLightmapInfo lightMapInfo = null;
	
	public SubObjectTransformInfo transformInfo = null;
	
	public SubObject() { }
	
	public SubObject( string name, SceneObjectLightmapInfo lightMapInfo = null, SubObjectTransformInfo transformInfo = null )
	{
		objectName = name;
		
		this.lightMapInfo = lightMapInfo;	
		
		this.transformInfo = transformInfo;
	}
	
	
//    public double positionX;
//    public double positionY;
//    public double positionZ;
//
//    public double rotationX;
//    public double rotationY;
//    public double rotationZ;
//
//    public double scaleX;
//    public double scaleY;
//    public double scaleZ;
//
//    public SubObject() { }
//
//    public SubObject( string name, Transform transform ) 
//    {
//        objectName = name;
//
//        this.positionX = (double)transform.localPosition.x;
//        this.positionY = (double)transform.localPosition.y;
//        this.positionZ = (double)transform.localPosition.z;
//
//        this.rotationX = (double)transform.localEulerAngles.x;
//        this.rotationY = (double)transform.localEulerAngles.y;
//        this.rotationZ = (double)transform.localEulerAngles.z;
//
//        this.scaleX = (double)transform.localScale.x;
//        this.scaleY = (double)transform.localScale.y;
//        this.scaleZ = (double)transform.localScale.z;
//    }
}

public class SubObjectTransformInfo
{
	public double positionX;
    public double positionY;
    public double positionZ;

    public double rotationX;
    public double rotationY;
    public double rotationZ;

    public double scaleX;
    public double scaleY;
    public double scaleZ;

    public SubObjectTransformInfo() { }

    public SubObjectTransformInfo( Transform transform ) 
    {
        this.positionX = (double)transform.localPosition.x;
        this.positionY = (double)transform.localPosition.y;
        this.positionZ = (double)transform.localPosition.z;

        this.rotationX = (double)transform.localEulerAngles.x;
        this.rotationY = (double)transform.localEulerAngles.y;
        this.rotationZ = (double)transform.localEulerAngles.z;

        this.scaleX = (double)transform.localScale.x;
        this.scaleY = (double)transform.localScale.y;
        this.scaleZ = (double)transform.localScale.z;
    }
}
