using UnityEngine;
using System.Collections;

/// <summary>
/// Scene helper.主要用于设置场景效果
/// </summary>
public static class SceneHelper
{
	#region Fog Setting
	public static void CleanMapFog(){
		RenderSettings.fogColor = Color.black;
		RenderSettings.skybox = null;
	}
	
	public static void SetMapFog( string sceneId, MapDataInfo info)
	{
		if (info == null)
		{
			SetLightMap(null);
			return;
		}
		else
		{
			//			//设置方向光颜色
			//			GameObject lightGo = GameObject.Find("Directional light");
			//			if ( lightGo != null )
			//			{
			//				Light light = lightGo.GetComponent<Light>();
			//				light.color = info.color;
			//				light.intensity = info.intensity;
			//			}
			
			RenderSettings.fog = info.fog;
			if (info.fog == true)
			{
				RenderSettings.fogColor = info.fogColor;
				RenderSettings.fogMode = (FogMode)info.fogMode;
				RenderSettings.fogDensity = info.fogDensity;
				RenderSettings.fogStartDistance = info.fogStartDistance;
				RenderSettings.fogEndDistance = info.fogEndDistance;
				RenderSettings.ambientLight = info.ambientLight;
				
				if (string.IsNullOrEmpty(info.skyBoxMaterialPath) == false)
				{
					GameDebuger.Log( "Load Scene " + sceneId + " skyBoxMaterialPath Start path=" + info.skyBoxMaterialPath);
					
					ResourceLoader.LoadAsync(info.skyBoxMaterialPath, delegate(AssetBase obj) {
						if (obj != null){
							RenderSettings.skybox = obj.asset as Material;	
						}else{
							GameDebuger.Log( "Load Scene " + sceneId + " skyBoxMaterialPath Error path=" + info.skyBoxMaterialPath);
						}
					}, "mat");
				}
				
				RenderSettings.haloStrength = info.haloStrength;
				RenderSettings.flareStrength = info.flareStrength;
				
//				ModelHelper.SetEchoLoginShaders();
			}
			
			UpdateSceneLightMap( sceneId );
		}
	}
	
	//	public static void SetCommonMapFog()
	//	{
	//		Utility.SetEchoLoginShaders();
	//
	//		Texture2D lightmapTexture = LightmapManager.GetCommonLightmapTexture();
	//		if( lightmapTexture != null )
	//		{
	//			SetLightMap( lightmapTexture );
	//		}
	//	}
	#endregion
	
	#region Lightmap Settings
	public static void UpdateSceneLightMap( string sceneid )
	{
		Texture2D lightmapTexture = LightmapManager.GetLightmapTexture( sceneid );
		if( lightmapTexture != null )
		{
			SetLightMap( lightmapTexture );
		}
		else
		{
			//string sceneLightmapPath = PathHelper.LIGHTMAP_PATH + sceneid + "/LightmapFar-" + sceneid ;
			string sceneLightmapPath = "ArtResources/Scenes_All/" + sceneid + "/LightmapFar-0" ;
			ResourceLoader.LoadAsync( sceneLightmapPath, delegate(AssetBase assetBase )
			                         {
				if( assetBase != null )
				{
					Texture2D map = assetBase.asset as Texture2D;
					LightmapManager.SetLightmapTexture( sceneid, map );
					SetLightMap( map );
				}
				else
				{
					//SetLightMap( null );
					GameDebuger.Log( "Load Scene " + sceneid + " Lightmap Error path=" + sceneLightmapPath);
				}
			}, "exr");			
		}
	}
	
	public static void SetLightMap(Texture2D lightmapTexture)
	{
		if( lightmapTexture != null )
		{
			LightmapData data = new LightmapData();
			
			data.lightmapFar = lightmapTexture;
			LightmapSettings.lightmaps = new LightmapData[1]{ data };
		}
		else
		{
			LightmapSettings.lightmaps = null;
		}
		
	}
	#endregion
	
	#region Skybox Settings	
	private static void UpdateSceneSkybox( string nearMaterialPath , string farMaterialPath )
	{
		
		//Far
		Transform farTrans = null;
		if( GameSetting.IsLoadLocalAssets())
		{
			GameObject farObj = GameObject.Find("skybox_far");
			if( farObj != null ) farTrans = farObj.transform; 
		}
		else
		{
			//			farTrans = LayerManager.Instance.getFarSkyboxTransform();	
		}
		
		if( farTrans != null )
		{
			if( !string.IsNullOrEmpty( farMaterialPath ) && farTrans.gameObject.activeSelf )
			{
				Material farMaterial = SkyboxManager.GetFarSkyboxMaterial(farMaterialPath);
				if( farMaterial != null )
				{
					SetSkyboxMaterial( farTrans, farMaterial );	
				}
				else
				{
					ResourceLoader.LoadAsync( farMaterialPath, delegate(AssetBase assetBase )
					                         {
						if( assetBase != null )
						{
							Material resultMaterial = assetBase.asset as Material;
							SetSkyboxMaterial( farTrans, resultMaterial );
							SkyboxManager.SetFarSkyboxMaterial( farMaterialPath, resultMaterial );
						}
						else
						{
							farTrans.gameObject.SetActive(false);
							GameDebuger.Log( "Load Scene Far Skybox" + farMaterialPath + " Error " );
						}
					}, "mat");					
				}
			}
			else
			{
				farTrans.gameObject.SetActive(false);
			}
		}
		
		
		//Near 
		Transform nearTrans = null;
		if( GameSetting.IsLoadLocalAssets())
		{
			GameObject nearObj = GameObject.Find("skybox_near");
			if( nearObj != null ) nearTrans = nearObj.transform;
		}
		else
		{
			//			nearTrans = LayerManager.Instance.getNearSkyboxTransform();
		}
		
		if( nearTrans != null )
		{
			if( !string.IsNullOrEmpty( nearMaterialPath ) && nearTrans.gameObject.activeSelf )
			{
				Material nearMaterial = SkyboxManager.GetNearSkyboxMaterial( nearMaterialPath );
				if( nearMaterial != null )
				{
					SetSkyboxMaterial( nearTrans, nearMaterial );	
				}
				else
				{
					ResourceLoader.LoadAsync( nearMaterialPath, delegate(AssetBase assetBase )
					                         {
						if( assetBase != null )
						{
							Material resultMaterial = assetBase.asset as Material;
							SetSkyboxMaterial( nearTrans, resultMaterial );
							SkyboxManager.SetNearSkyboxMaterial( nearMaterialPath, resultMaterial );
						}
						else
						{
							nearTrans.gameObject.SetActive(false);
							GameDebuger.Log( "Load Scene Near Skybox" + nearMaterialPath + " Error " );
						}
					}, "mat");		
				}
			}	
			else
			{
				nearTrans.gameObject.SetActive(false);
			}
		}
	}
	
	private static void SetSkyboxMaterial ( Transform transform , Material material )
	{
		MeshRenderer meshRenderer = transform.GetComponent< MeshRenderer >();
		if( meshRenderer != null )
		{
			meshRenderer.sharedMaterial = material;
		}
	}
	#endregion

	#region Scene Stand Position
	public static Vector3 GetSceneStandPosition(Vector3 sourcePos, Vector3 defaultPos)
	{
		Vector3 newPosition;
		
		RaycastHit hit;

		if (Physics.Raycast (new Vector3(sourcePos.x, 50, sourcePos.z), -Vector3.up, out hit, 100, 1 << LayerMask.NameToLayer (GameTag.Tag_Terrain))) 
		{
			newPosition = hit.point;
		}
		else
		{
			if (defaultPos.Equals(Vector3.zero))
			{
				newPosition = new Vector3(sourcePos.x, sourcePos.y, sourcePos.z);
			}
			else
			{
				newPosition = GetSceneStandPosition(defaultPos, Vector3.zero);
			}
		}
		
		return newPosition;

//		Vector3 newPosition;
//		
//		NavMeshHit hit;
//		if (!NavMesh.SamplePosition(sourcePos, out hit, 50,1 << NavMesh.GetNavMeshLayerFromName(GameTag.Tag_Default)))
//		{
//			newPosition = defaultPos;
//		}
//		else
//		{
//			newPosition = hit.position;
//		}
//		
//		return newPosition;
	}
	#endregion

	#region 测试人物的生成位置
	public static Vector3 GetRobotSceneStandPosition(Vector3 sourcePos, Vector3 defaultPos){
		Vector3 newPosition;
		
		NavMeshHit hit;
		if (!NavMesh.SamplePosition(sourcePos, out hit, 50,1 << NavMesh.GetNavMeshLayerFromName(GameTag.Tag_Default)))
		{
			newPosition = defaultPos;
		}
		else
		{
			newPosition = hit.position;
		}
		
		return newPosition;
	}
	#endregion
}
