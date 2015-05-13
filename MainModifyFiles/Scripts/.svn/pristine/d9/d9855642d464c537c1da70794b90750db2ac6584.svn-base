using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.dto;
using System.Collections.Generic;

public class BattleSceneLoader : MonoBehaviour
{
	static BattleSceneLoader mInstance;
	
	private GameObject attachParent;

	public delegate void LoadMapProgress(float precent);
	public LoadMapProgress loadMapProgress;

    public delegate void LoadMapFinish();
    public LoadMapFinish loadMapFinish;

	static public BattleSceneLoader Instance
	{
		get
		{
			if ( mInstance == null )
			{
				mInstance = Object.FindObjectOfType( typeof(BattleSceneLoader) ) as BattleSceneLoader;

				if (mInstance == null)
				{
					GameObject go = new GameObject("_BattleSceneLoader");
					DontDestroyOnLoad(go);
					mInstance = go.AddComponent< BattleSceneLoader >();
				}
			}
			
			return mInstance;
		}
	}

    private string _sceneName;
	private Queue<VideoSoldier> _soldierQueue;

	public void LoadScene( string sceneName, Video gameVideo, LoadMapProgress loadMapProgressDelegate, LoadMapFinish loadMapFinishDelegate)
	{
		loadMapProgress = loadMapProgressDelegate;
        loadMapFinish = loadMapFinishDelegate;

//        GameObject go = GameObject.Find( "BattleStage" );
//        if (go != null)
//        {
//            if (_sceneName != sceneName)
//            {
//                _sceneName = sceneName;
//                GameObject.Destroy(go);
//                ResourceLoader.LoadLevelAdditiveAsync(sceneName, null, LoadLevelFinish);
//            }
//            else
//            {
//                Invoke("DelayFinish", 0.5f);
//            }
//        }
//        else
//        {
//            _sceneName = sceneName;
//            ResourceLoader.LoadLevelAdditiveAsync(sceneName, null, LoadLevelFinish);
//        }
		
        _sceneName = sceneName;

		_soldierQueue = new Queue<VideoSoldier>();
		foreach(VideoSoldier soldier in gameVideo.ateam.teamSoldiers)
		{
			_soldierQueue.Enqueue(soldier);
		}

		foreach(VideoSoldier soldier in gameVideo.bteam.teamSoldiers)
		{
			_soldierQueue.Enqueue(soldier);
		}

//		if (WorldManager.KeepSceneMode)
//		{
//			ResourceLoader.LoadLevelAdditiveAsync(sceneName, null, LoadLevelFinish);		
//		}
//		else
//		{
//			ResourceLoader.LoadLevelAsync(sceneName, null, LoadLevelFinish);
//		}

		//ResourceLoader.LoadLevelAsync(sceneName, null, LoadLevelFinish);
	}

    private void DelayFinish()
    {
        if (loadMapFinish != null)
        {
            loadMapFinish();
            loadMapFinish = null;
        }
    }

    private void LoadLevelFinish()
    {
		CheckEditorObj();
		CheckTerrain();
		
        AttachStageToBattleScene();

		loadMapProgress(0.5f);

		LoadBattleSoldier();
    }

	private void LoadBattleSoldier()
	{
		if (_soldierQueue.Count > 0)
		{
			VideoSoldier soldier = _soldierQueue.Dequeue();
//			string petStyle = soldier.hero.model.ToString();
//			string petStylePath = ModelHelper.GetCharactorPrefabPath(petStyle);
//			if (ResourceLoader.IsAssetExist(petStylePath) == false)
//			{
//				petStyle = "1";
//				petStylePath = ModelHelper.GetCharactorPrefabPath(petStyle);
//			}
			
			//ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DESTROY_CHANGE_SCENE);
		}
		else
		{
			loadMapProgress(1f);

			if (loadMapFinish != null)
			{
				loadMapFinish();
				loadMapFinish = null;
			}
		}
	}

	private void OnLoadFinish(UnityEngine.Object inst)
	{
		GameObject petGO = inst as GameObject;
		ResourcePoolManager.Instance.Despawn(petGO, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);

		LoadBattleSoldier();
	}

    private void CheckTerrain()
    {
        GameObject sceneLayer = GameObject.Find("BattleStage");

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
        }
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
	
	public void AttachStageToBattleScene( )
	{
		GameObject go = GameObject.Find( "BattleStage" );
		if ( go == null )
			return;
		
		if ( go.transform.parent != null )
			return;

		go.transform.parent = LayerManager.Instance.World.transform;
		
		NGUITools.SetActive( go, true );
	}
}
