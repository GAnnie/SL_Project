// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SceneObjectsInfo.cs
// Author   : wenlin
// Created  : 2013/6/18 11:55:32
// Purpose  : 储存当前场景物体的相关信息。 用于在场景初始化的时候还原场景物件的信息
// **********************************************************************


using System;
using UnityEngine;
using System.Collections.Generic;

public class SceneObjectsInfoSet
{
    public string sceneID;

    public Dictionary<string, SceneNode> sceneNodeDic;

	public SceneSkyboxData skyboxData = null;
	
    public SceneObjectsInfoSet()
    { }


    public SceneObjectsInfoSet(string sceneID, Transform skybox = null)
    {
        this.sceneID      = sceneID;
        this.sceneNodeDic = new Dictionary<string, SceneNode>(); 
		
		if( skybox != null )
		{
			this.skyboxData = new SceneSkyboxData( skybox );
		}
    }
}