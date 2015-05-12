// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TextureExporter.cs
// Author   : wenlin
// Created  : 2013/3/4 
// Purpose  : 
// **********************************************************************
using System;
using System.IO; 
using UnityEngine;
using UnityEditor;

public sealed class TextureExporter
{
//	private static int terrainWith 	  = 0;
//	private static int terrainHeight  = 0;
//	private static int alphaMapWith   = 0;
//	private static int alphaMapHeight = 0;
	
	private static float withRatio	  = 0;
	private static float heightRatio  = 0;	
	
	[@MenuItem("Terrain/Export Terrain Texture")]
	public static void ExportTerrainTexture()
	{
		TerrainData terrainData  = Selection.activeObject as TerrainData ; 
		if( terrainData == null )
		{
			 EditorUtility.DisplayDialog("Select Terrain", "You Must Select a Terrain first!", "Ok"); 
     		 return; 
		}
		
		EditorUtility.DisplayDialog("Select Terrain", "You select the Terrain of " + terrainData.name  , "Ok"); 
		var path = EditorUtility.SaveFilePanel ("Save Resource", "", terrainData.name, "png");
		if ( path.Length  == 0) return;
		
		//Get Ratio
		withRatio 	= terrainData.size.x / terrainData.alphamapWidth;
		heightRatio	= terrainData.size.z /  terrainData.alphamapHeight;
		
//		//get Terrain size 
//		terrainWith   = terrainData.size.x;
//		terrainHeight = terrainData.size.z;
//		
//		//Get AlphaMap size
//		alphaMapWith   = terrainData.alphamapWidth;
//		alphaMapHeight = terrainData.alphamapHeight;
		
//		return;
		//Create return texture
		Texture2D texture = new Texture2D( terrainData.alphamapWidth, terrainData.alphamapHeight,TextureFormat.RGB24, false );
		
		
		//blend map
		float [,,] splatmapData = terrainData.GetAlphamaps( 0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight );
		
//		Color[] colorList = new Color[ splatmapData.GetUpperBound(2) ];//newList<Color>( splatmapData.GetUpperBound(2) );
       	for ( int i = splatmapData.GetLowerBound(0); i <= splatmapData.GetUpperBound(0); i++ )
		{
          	for ( int j = splatmapData.GetLowerBound(1); j <= splatmapData.GetUpperBound(1); j++ )
			{
				
				Color blendColor = new Color();
				for ( int k = splatmapData.GetLowerBound(2); k <= splatmapData.GetUpperBound(2); k++ )
				{
					Color color = GetSplatColor( terrainData.splatPrototypes[k], i , j );
					blendColor += color * splatmapData[i, j, k];
				}
				texture.SetPixel(  i, j, blendColor );				
			}
		}
		
		
//		float [,,] splatmapData = terrainData.GetAlphamaps( 0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight );
//		Texture2D texture = new Texture2D( terrainData.alphamapWidth, terrainData.alphamapHeight,TextureFormat.RGB24, false );

		byte[] bytes = texture.EncodeToPNG(); 
  	 	File.WriteAllBytes( path , bytes); 
	}
	
	private static Color GetSplatColor( SplatPrototype splatPrototype, float x , float y )
	{
		Texture2D texture = splatPrototype.texture;
		Vector2 tileSize  = splatPrototype.tileSize;
		
		int temp_x = Mathf.FloorToInt( x * withRatio );
		int temp_y = Mathf.FloorToInt( y * heightRatio );
		
	 	temp_x =  Mathf.FloorToInt( (temp_x % tileSize.x) * (texture.width  / tileSize.x) );
		temp_y =  Mathf.FloorToInt( (temp_y % tileSize.y) * (texture.height / tileSize.y) ); 
		
		Color color = texture.GetPixel( temp_x, temp_y );
		return color;
	}
	
	
	[@MenuItem("Terrain/Export Blend Texture")]
	public static void ExportBlendTexture()
	{
		Texture2D texture  = Selection.activeObject as Texture2D;
		if( texture == null )
		{
		      EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok"); 
		      return; 
		}
		
		var path = EditorUtility.SaveFilePanel ("Save Resource", "", "BlendTexture", "png");
		if ( path.Length  == 0) return;
		
		Texture2D exportTexture = new Texture2D( texture.width, texture.height );
		for ( int i = 0 ; i < texture.width ; i ++ )
		{
			for( int j = 0 ; j < texture.height ; j ++ )
			{
//				exportTexture.SetPixel( j , texture.width - i , texture.GetPixel( i, j ));
				exportTexture.SetPixel( j , i , texture.GetPixel( i, j ));

			}
		}
		
		
		byte[] bytes = exportTexture.EncodeToPNG(); 
  	 	File.WriteAllBytes( path , bytes); 
	}
}




