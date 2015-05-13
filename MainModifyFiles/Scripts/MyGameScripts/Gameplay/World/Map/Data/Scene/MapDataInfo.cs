// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MapDataInfo.cs
// Author   : wenlin
// Created  : 2013/1/28 
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class MapDataInfo
{
	/**
	 *客户端用的场景数据信息   xx.mapdata
	 * @author SK
	 * 
	 */	
	
	
	// 地图id
	public int id;

//	public var fogParam; //雾效
//	public var lightParam:Object; //光源
//	public var bloomParam:Object; //泛光
	
    //fog msg
    public bool fog = false;
    public Color fogColor;
    public int fogMode;
    public float fogDensity;
    public int fogStartDistance;
    public int fogEndDistance;
    //

    public Color ambientLight;
    public string skyBoxMaterialPath;
    public float haloStrength;
    public float flareStrength;

	public MapDataInfo()
	{}
	
	public MapDataInfo( int id )
	{
		this.id = id;
	}

    public MapDataInfo(string id)
    {
        if (Regex.IsMatch(id, @"^[-]?\d+[.]?\d*$"))
        {
            this.id = int.Parse(id);
        }
        else
        {
            this.id = -1;
        }
    }
}


