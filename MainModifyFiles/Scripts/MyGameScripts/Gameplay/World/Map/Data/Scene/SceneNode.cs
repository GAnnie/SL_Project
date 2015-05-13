// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SceneNode.cs
// Author   : wenlin
// Created  : 2013/6/18 14:28:54
// Purpose  : 场景节点的数据结构， 代表着场景固定的节点
// **********************************************************************

using System;
using System.Collections.Generic;

public class SceneNode
{
    public string nodeName;

    public Dictionary<string, SceneObjectData> sceneObjectDataDic;

    //存在Collider数据的节点
    public Dictionary<string, SceneObjectData> colliderObjectDataDic;

    public SceneNode()
    { }


    public SceneNode(string nodeName)
    {
        this.nodeName = nodeName;
        this.sceneObjectDataDic = new Dictionary<string, SceneObjectData>();
        this.colliderObjectDataDic = new Dictionary<string, SceneObjectData>();
    }
}