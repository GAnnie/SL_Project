using UnityEngine;
using System.Collections;

public class SkyboxManager
{
	private static string farSkyboxMaterialPath = string.Empty;
	private static Material farSkyboxMaterial = null;
	
	private static string nearSkyboxMaterialPath = string.Empty;
	private static Material nearSkyboxMaterial = null;
	
	
	public static Material GetFarSkyboxMaterial( string path )
	{
		if( farSkyboxMaterialPath == path)
		{
			return farSkyboxMaterial;
		}
		else
		{
			return null;
		}
	}
	
	public static Material GetNearSkyboxMaterial( string path )
	{
		if( nearSkyboxMaterialPath == path)
		{
			return nearSkyboxMaterial;
		}
		else
		{
			return null;
		}		
	}
	
	public static void SetFarSkyboxMaterial( string path, Material material )
	{
		farSkyboxMaterialPath = path;
		farSkyboxMaterial     = material;
	}
	
	public static void SetNearSkyboxMaterial( string path, Material material )
	{
		nearSkyboxMaterialPath = path;
		nearSkyboxMaterial     = material;
	}
}
