// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WorldMapLoader.cs
// Author   : SK
// Created  : 2013/2/27
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;

public class WorldMapLoader : MonoBehaviour
{
	private static WorldMapLoader _instance = null;
	
	public static WorldMapLoader Instance
	{
		get
		{
			CreateInstance ( );
			return _instance;
		}
	}
	
	static void CreateInstance ()
	{
		if (_instance == null)
		{
			_instance = GameObject.FindObjectOfType(typeof(WorldMapLoader)) as WorldMapLoader;
			
			if (_instance == null)
			{
				GameObject go = new GameObject("_WorldMapLoader");
				DontDestroyOnLoad(go);
				_instance = go.AddComponent<WorldMapLoader>();
				_instance.SceneLayer = LayerManager.Instance.SceneLayer;
				_instance.BattleLayer = LayerManager.Instance.BattleLayer;
			}
		}
	}

	void Start ()
	{
	}

	void Update ()
	{
		if (loadLevelProgress != null && _operation != null){
			loadLevelProgress(_operation.progress);
		}
	}

	public bool IsReady = false;
	
	public void SetReady(bool ready){
		IsReady = ready;
	}

	private AsyncOperation _operation;

	public delegate void LoadMapFinish();
	public LoadMapFinish loadMapFinish;

	public delegate void LoadLevelProgressDelegate(float percent);
	public LoadLevelProgressDelegate loadLevelProgress;

	private int _mapId;
	private bool _isBattleScene;

	// 场景
	private GameObject SceneLayer;
	private GameObject SceneMap;

	// 战斗
	private GameObject BattleLayer;
	private GameObject BattleMap;

	private bool LoadBeforeHandle(int mapId)
	{
		if(_isBattleScene)
		{
			// 战斗
			if(BattleLayer.transform.Find("Battle_"+mapId) == null)
			{
				GameObjectExt.RemoveChildren(BattleLayer);
				
				SceneLayer.SetActive(false);
				BattleLayer.SetActive(true);
				return false;
			}
			else
			{
				SceneLayer.SetActive(false);
				BattleLayer.SetActive(true);
				return true;
			}
		}
		else
		{
			// 场景
			if(SceneLayer.transform.Find("World_"+mapId) == null)
			{
				GameObjectExt.RemoveChildren(SceneLayer);
				
				SceneLayer.SetActive(true);
				BattleLayer.SetActive(false);
				return false;
			}
			else
			{
				SceneLayer.SetActive(true);
				BattleLayer.SetActive(false);
				return true;
			}
		}
		return true;
	}

	public void LoadBattleMap(int mapId)
	{
		LoadMap (mapId, true);
	}

	public void LoadWorldMap(int resId)
	{
		LoadMap (resId, false);
	}

	private void LoadMap(int mapId, bool isBattleScene)
	{
		GameDebuger.Log ("LoadMap = " + mapId + " " + isBattleScene);

		IsReady = false;

		_mapId = mapId;
		_isBattleScene = isBattleScene;

		if(LoadBeforeHandle(mapId) == true)
		{
			OnAllSceneLoaded();
		}
		else
		{
			if (_isBattleScene)
			{
				ResourceLoader.LoadLevelAdditiveAsync("Battle_"+mapId, LoadLevelProgress, OnAllSceneLoaded, AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj);
			}
			else
			{
				ResourceLoader.LoadLevelAsync("Scene_"+mapId, LoadLevelProgress, OnAllSceneLoaded, AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj);
			}
		}
	}

    private void OnAllSceneLoaded()
    {
		WorldMapLayerHandle();

		LightmapManager.SetLightmaps (_mapId, LightmapSettings.lightmaps);

		CheckEditorObj();
		CheckTerrain();

        if (loadMapFinish != null)
        {
			IsReady = false;
            loadMapFinish();
        }
		else
		{
			IsReady = true;
		}
		
		_operation = null;	
    }

    private void LoadLevelProgress(AsyncOperation operation)
    {
        _operation = operation; // get warning
    }
	
	//检查编辑器对象，如果有则删除，例如临时的摄像机， 灯光等
	private void CheckEditorObj()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("EditorOnly");
		foreach (GameObject obj in objs)
		{
			GameObject.Destroy(obj);
		}
	}

	private void CheckTerrain()
	{
		GameObject sceneLayer = GameObject.Find("SceneLayer");
		
		if (sceneLayer == null)
		{
			return;
		}
		
		//批量修改场景的碰撞为Terrain
		MeshCollider[] colliderList = sceneLayer.GetComponentsInChildren<MeshCollider>(false);
		int layer = LayerMask.NameToLayer(GameTag.Tag_Terrain);
		foreach (MeshCollider collider in colliderList)
		{
			collider.gameObject.layer = layer;
			collider.gameObject.tag = GameTag.Tag_Terrain;
			Vector3 localPosition = collider.gameObject.transform.localPosition;
			//collider.gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y + 0.2f, localPosition.z);
			//collider.gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
		}
	}

	private void WorldMapLayerHandle()
	{
		if(_isBattleScene)
		{
			GameObject state = GameObject.Find("BattleStage");
			if (state != null)
			{
				state.name = "Battle_" + _mapId;
				GameObjectExt.AddPoolChild(BattleLayer, state);
			}
		}
		else
		{
			GameObject state = GameObject.Find("WorldStage");

			if (state != null)
			{
				state.name = "World_" + _mapId;
				GameObjectExt.AddPoolChild(SceneLayer, state);
			}
		}
	}
}

