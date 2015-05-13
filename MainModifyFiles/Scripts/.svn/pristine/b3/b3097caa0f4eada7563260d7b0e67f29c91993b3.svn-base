using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightmapManager
{
	private static string 	 _commonSceneId = string.Empty;
	private static Texture2D _commonSceneLightmap = null;
	
	private static string 	 _battleSceneId = string.Empty;
	private static Texture2D _battleSceneLightmap = null;

	public static Texture2D GetCommonLightmapTexture(  )
	{
		return _commonSceneLightmap;
	}

	public static Texture2D GetLightmapTexture( string sceneID )
	{
		//是否战斗场景
		if( sceneID.StartsWith("BattleScene" ) )
		{
			if( _battleSceneId == sceneID && _battleSceneLightmap != null )
			{
				return _battleSceneLightmap;
			}
		}
		else
		{
			if( _commonSceneId == sceneID && _commonSceneLightmap != null )
			{
				return _commonSceneLightmap;
			}
			
		}
		
		return null;
	}
	
	public static void SetLightmapTexture( string sceneID , Texture2D lightmapTexture )
	{
		//是否战斗场景
		if( sceneID.StartsWith("BattleScene" ) )
		{
			_battleSceneId 		 = sceneID;
			_battleSceneLightmap = lightmapTexture;
		}
		else
		{
			_commonSceneId 		 = sceneID;
			_commonSceneLightmap = lightmapTexture;
		}
	}

	private static Dictionary<int , LightmapData[] > lightMapsDict = new Dictionary<int, LightmapData[]> ();

	public static void SetLightmaps(int sceneId, LightmapData[] lightmaps)
	{
		if (lightMapsDict.ContainsKey(sceneId) == false)
		{
			lightMapsDict.Add (sceneId, lightmaps);
		}
	}

	public static LightmapData[] GetLightmaps(int sceneId)
	{
		LightmapData[] lightmaps = null;
		lightMapsDict.TryGetValue (sceneId, out lightmaps);
		return lightmaps;
	}
}
