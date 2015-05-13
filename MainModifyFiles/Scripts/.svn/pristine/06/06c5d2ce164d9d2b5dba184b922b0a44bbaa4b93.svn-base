// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SceneObjectData.cs
// Author   : wenlin
// Created  : 2013/6/18 14:32:26
// Purpose  : 场景上物品的数据结构， 每个一个Data数据代表着场景上的一个prefab节点
// **********************************************************************

using UnityEngine;
using System;
using System.Collections.Generic;

public class SceneObjectData
{
    public string objectName;

    public string objectPathName;

    public string parentName;

    public List<SceneObjectTransFormData> objectTransFormDataList = null;

    public SceneObjectData()
    { }

    public SceneObjectData(string objcetName, string objectPathName, string parentName)
    {
        this.objectName = objcetName;
        this.objectPathName = objectPathName;
        this.parentName = parentName;
        objectTransFormDataList = new List<SceneObjectTransFormData>();
    }
}

public class SceneObjectTransFormData
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

    public Dictionary<string, SubObject> subObjects;

	public SceneObjectLightmapInfo lightmapInfo = null;
	
    public SceneObjectTransFormData()
    { }

    public SceneObjectTransFormData(Transform transform, SceneObjectLightmapInfo lightmapInfo = null )
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

        this.subObjects = new Dictionary<string, SubObject>();
		
		this.lightmapInfo = lightmapInfo;
    }
}