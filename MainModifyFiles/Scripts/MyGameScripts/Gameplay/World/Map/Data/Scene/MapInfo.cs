// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MapInfo.cs
// Author   : wenlin
// Created  : 2013/1/28 
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapInfo
{
	/**
	 *提供给服务器用的场景数据信息   xx.map
	 * @author SK
	 * 
	 */	
	
	
	// 地图id
	public int id;
	
	public List< NpcConfigInfo > npc; //[NpcConfigInfo]
	
	// 地图行走 (是否可行走)
	public string grids;
	
	//人物初始范围
	public float x  = 0; //初始坐标X
    public float y = 0;  //初始坐标Y
    public float rotationY = 0; //初始旋转
    public float radius = 10; //坐标半径
	
	public MapInfo( )
	{}
	
	public MapInfo( int id )
	{
		this.id = id;
		this.npc = new List<NpcConfigInfo>();
	}
}


