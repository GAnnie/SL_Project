// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SceneObjectLightmapInfo.cs
// Author   : wenlin
// Created  : 2013/10/14 
// Purpose  : 
// **********************************************************************
using System;
using UnityEngine;


public class SceneObjectLightmapInfo
{
	public int lightmapIndex = -1;
	
	public double x;
	
	public double y;
	
	public double z;
	
	public double w;
	
	public SceneObjectLightmapInfo()
	{}
	
	public SceneObjectLightmapInfo( Renderer renderer )
	{
		if( renderer != null )
		{
			this.lightmapIndex = renderer.lightmapIndex;
			
			this.x = renderer.lightmapTilingOffset.x;

			this.y = renderer.lightmapTilingOffset.y;

			this.z = renderer.lightmapTilingOffset.z;

			this.w = renderer.lightmapTilingOffset.w;
			
			//this.lightmapTilingOffset = renderer.lightmapTilingOffset;
		}
	}
}

