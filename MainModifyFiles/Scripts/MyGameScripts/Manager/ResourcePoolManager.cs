// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ResourcePoolManager.cs
// Author   : SK
// Created  : 2013/8/26
// Update   : wenlin  ： 2013/10/24
// Purpose  : 
// 		ResourcePoolManager 的作用是管理资源缓存， 根据游戏的具体情况， ResourcePoolManager 里面分别存在三种不同的缓存池， 三种不同的
// 		缓存池是依据资源的不同而响应定制,  三种不同的缓存池的说明如下：
// 		1】 常驻的缓存池 （PoolType.DONT_DESTROY)
// 			这个缓存池主要是缓存一些整个游戏里面都存在的， 而且比较容易出现的资源， 存储在里面的缓存的prefab， 一当进入缓存池， 将会整个
//			游戏生命周期内都不会被回收， 但是实例化的数量会以一定机制保持在一定的数量， 以确保实例化出来的内存得以回收。
// 	
// 		2】 不存在引用自动删除的缓存池 ( PoolType.DESTROY_NO_REFERENCE )
//     		这个缓存池主要是处理一些这样的资源，  资源是随时都会出现， 同时这些资源是在后续的使用中， 只是一种不定期才使用一次的。 这样的
// 			资源如果已经没有被引用到，  应该在一段时间内对其实例和prefab 删除。  以清空这些资源的内存占用
// 
// 		3】 跳转场景后全清空的缓存池 ( PoolType.DESTROY_CHANGE_SCENE)
// 			这个缓存池主要处理一些这样的资源， 这些资源在当前场景是比较容易出现， 但是在下一个场景可能就不会出现的资源， 这样的资源在当前
// 			场景是不会销毁， 并缓存起来， 以便资源可以循环利用。  但是在跳转场景的时候， 这个缓存池会清空里面所有资源的引用。 下一个场景
// 			就需要重新的创建和实例化
// 
// 
// 		三个缓冲池对根据不同的情况而使用，  现在三个缓存池之间的可以出现相同的资源。  相互之间是一个没有交集的集合。 
// 
// 		使用的例子：
// 
// 		string path = "testPath";
// 		ResourcePoolManager.Spawn( path , delegate(AssetBase assetBase )
// 		{
// 			if( assetbase != null )
// 			{
// 				----- do something
// 			}
// 		}, 
// 		PoolType.DONT_DESTROY );
// 
// **********************************************************************


using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ResourcePoolManager
{
	private struct LoadingInfo
	{
		public bool isNoInstance;
		public string prefabPath;
		public Action<UnityEngine.Object> getter;
		public ResourcePoolManager.PoolType poolType;
		
		public LoadingInfo(string _prefabPath, Action<UnityEngine.Object> _getter, ResourcePoolManager.PoolType _poolType, bool _isNoInstance = false)
		{
			prefabPath = _prefabPath;
			getter = _getter;
			poolType = _poolType;
			isNoInstance = _isNoInstance;
		}
	}
	
	
    private static readonly ResourcePoolManager instance = new ResourcePoolManager();
    public static ResourcePoolManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	/// <summary>
	/// 缓存池的类型
	/// DONT_Destroy 的spwanpool是不会删除的， 缓冲中的prefab会一直驻留在内存里面
	/// DESTROY_NO_REFFENCE 的 spwanpool 在 完全没有引用的情况下， 经过一定的时间就会释放掉prefab的应用
	/// DESTROY_CHANGE_SCENE 的 spwanpool 指定在场景跳转之后就销毁里面所有的引用。（此类型可以使用在Material, Texture等类型的缓存)
	/// </summary>
	public enum PoolType
	{
		DONT_DESTROY 	= 0,   
		DESTROY_NO_REFERENCE,
		DESTROY_CHANGE_SCENE,
		PoolTypeNumber
	}
	
	//资源加载队列,让资源加载的时序平均点， 不要瞬时加载多个资源
	private Queue<LoadingInfo> loadingQueue = null;
	//资源是否在loading
	private bool loading = false;
	
	private GameSpawnPool [] gameSpawnPools = null;
    public void Setup()
    {
        GameObject poolManager = GameObject.Find("PoolManager");
        if (poolManager == null)
        {
            poolManager = new GameObject("PoolManager");
			GameObject.DontDestroyOnLoad( poolManager );
        }
		
		gameSpawnPools  = new GameSpawnPool[(int) PoolType.PoolTypeNumber ];
		
		gameSpawnPools[(int)PoolType.DONT_DESTROY]				 		 =  ConfigurationGameSpawnPool( GamePoolManager.Pools.Create( "DontDestroyPool", poolManager ), PoolType.DONT_DESTROY );
		gameSpawnPools[(int)PoolType.DONT_DESTROY].defaultOption 		 =  GenerateGamePrefabPoolOption( PoolType.DONT_DESTROY );
		
		gameSpawnPools[(int)PoolType.DESTROY_NO_REFERENCE] 				 =  ConfigurationGameSpawnPool( GamePoolManager.Pools.Create( "DestroyNoReference", poolManager ), PoolType.DESTROY_NO_REFERENCE );
		gameSpawnPools[(int)PoolType.DESTROY_NO_REFERENCE].defaultOption =  GenerateGamePrefabPoolOption( PoolType.DESTROY_NO_REFERENCE );

		gameSpawnPools[(int)PoolType.DESTROY_CHANGE_SCENE] 				 =  ConfigurationGameSpawnPool( GamePoolManager.Pools.Create( "DestroyChangeScene", poolManager ), PoolType.DESTROY_CHANGE_SCENE );
		gameSpawnPools[(int)PoolType.DESTROY_CHANGE_SCENE].defaultOption =  GenerateGamePrefabPoolOption( PoolType.DESTROY_CHANGE_SCENE );
		
		loadingQueue = new Queue<LoadingInfo>();
		loading = false;
    }
	
	/// <summary>
	/// 根据Type 来配置GameSpawnPool属性
	/// </summary>
	/// <param name='spawnPool'>
	/// Spawn pool.
	/// </param>
	/// <param name='type'>
	/// Type.
	/// </param>
	private GameSpawnPool  ConfigurationGameSpawnPool( GameSpawnPool spawnPool , PoolType type )
	{
		
		if( type == PoolType.DONT_DESTROY )
		{
			spawnPool.autoRemovePrefabPool = false;
		}
		else if( type == PoolType.DESTROY_NO_REFERENCE )
		{
			spawnPool.autoRemovePrefabPool = true;
		}
		else if ( type == PoolType.DESTROY_CHANGE_SCENE )
		{
			spawnPool.autoRemovePrefabPool = true;
		}
		
		return spawnPool ;
	}
	
	/// <summary>
	/// 更新SpawnPool的类型， 生成具体的GamePrefabPoolOption
	/// </summary>
	/// <param name='pool'>
	/// Pool.
	/// </param>
	/// <param name='type'>
	/// Type.
	/// </param>
	private GamePrefabPoolOption GenerateGamePrefabPoolOption( PoolType type )
	{
		GamePrefabPoolOption option = null;
		if( type == PoolType.DONT_DESTROY )
		{
			option = new GamePrefabPoolOption(); 
			
			//不进行预加载
			option.preloaded = false;
			
			//限制数量
			option.limitInstance = true;
			option.limitFIFO 	 = false;
			option.limitAmount 	 = 300;
			
			//Despawn  裁剪的控制
			option.cullDespawned = false;
			option.cullAbove     = 10;
			option.cullDelay     = 10;
			option.cullMaxPerPass = 2; 
			
			//不会自动删除
			option.autoDeletePrefab = false;
		}
		else if( type == PoolType.DESTROY_NO_REFERENCE )
		{
			option = new GamePrefabPoolOption(); 
			
			//不进行预加载
			option.preloaded = false;
			
			//不限制数量
			option.limitInstance = false;
			
			//不进行裁剪
			option.cullDespawned = false;
			
			//进行自动删减
			option.autoDeletePrefab = true;
			option.autoDeleteDelay  = 20;
		}
		else if ( type == PoolType.DESTROY_CHANGE_SCENE )
		{
			option = new GamePrefabPoolOption(); 
			
			//不进行预加载
			option.preloaded = false;
			
			//不限制数量
			option.limitInstance = true;
			option.limitFIFO     = true;
			option.limitAmount   = 300;
			
			//不进行裁剪
			option.cullDespawned = false;
			
			//不进行自动删减
			option.autoDeletePrefab = false;
			
		}
		return option;
	}	
	
	private void DoSpawn(string prefabPath, Action<UnityEngine.Object> getter, PoolType poolType = PoolType.DESTROY_CHANGE_SCENE, bool isNoInstance = false  )
	{
		if( gameSpawnPools == null)
		{
			GameDebuger.Log( "GameSpawnPools is null , please setup the ResourcePoolManager" );
			SpawnNext();
			return;
		}
		
		GameSpawnPool gameSpawnPool = gameSpawnPools[(int)poolType];
		if( gameSpawnPool != null )
		{
	        if (! gameSpawnPool.IsSpawned( prefabPath ) )
	        {
				//如果已经有过缓存， 则直接使用缓存的GamePrefabPool
				GamePrefabPool prePrefabPool = GamePoolManager.Pools.IsSpawned( prefabPath );
				if( prePrefabPool != null )
				{
		            gameSpawnPool.InsertGamePrefabPool(prePrefabPool);
	
					Debug.Log( "Copy Game Prefab Pool!!!!" + prefabPath);
	                if (getter != null) getter(ResetGameObject(gameSpawnPool.Spawn(prefabPath)));					
					SpawnNext();
				}
				else
				{
					ResourceLoader.LoadAsync(prefabPath, delegate(AssetBase assetbase) {
		                //prefabTransform = this.pool.GetPrefab(prefabPath);
		                if (gameSpawnPool.IsSpawned(prefabPath))
		                {
		                    getter(ResetGameObject(gameSpawnPool.Spawn(prefabPath)));
							SpawnNext();
							return;
						}
		
		                if (assetbase == null || assetbase.asset == null)
		                {
							GameDebuger.Log("Spawn asset not find and prefabPath = " + prefabPath);
							getter(null);
							SpawnNext();
							return;
						}
						
						if( (assetbase.asset is Material ) || (assetbase.asset is Texture2D ))
						{
							isNoInstance = true;
						}


		                GamePrefabPool prefabPool = new GamePrefabPool(prefabPath, assetbase.asset, null, isNoInstance );
		                
						//Debug.Log( "Insert Game Prefab Pool" + prefabPath );
			            gameSpawnPool.InsertGamePrefabPool(prefabPool);
		
		                if (getter != null) getter(ResetGameObject(gameSpawnPool.Spawn(prefabPath)));
						SpawnNext();
					});					
				}

	        }
	        else
	        {
	            if (getter != null) getter(ResetGameObject(gameSpawnPool.Spawn(prefabPath)));
				SpawnNext();
			}
		}		
	}
	
    public void Spawn(string prefabPath, Action<UnityEngine.Object> getter, PoolType poolType = PoolType.DESTROY_CHANGE_SCENE, bool isNoInstance = false )
    {
		loadingQueue.Enqueue(new LoadingInfo(prefabPath, getter, poolType, isNoInstance));
		
		if (loading == false)
		{
			SpawnNext();
		}
    }


	/// <summary>
	/// 专门用来缓存UI 实例化的缓存处理
	/// </summary>
	/// <returns>返回UI的实例化GameObject</returns>
	/// <param name="prefabPath">UI的资源地址</param>
	/// <param name="uiSpace">作为UI的存储空间， 用于区别不同模块使用相同UI</param>
	/// <param name="parentObj">指定UI的父节点</param>
	public GameObject SpawnUIGameObject( string prefabPath , string uiSpace ,  GameObject parentObj = null  )
	{
		UnityEngine.Object obj = SpawnSyn( prefabPath  , PoolType.DESTROY_NO_REFERENCE , false, false );
		SetSpawnSynObj( prefabPath , uiSpace,  obj );

		GameObject tempObj = obj as GameObject;

		if( parentObj != null )
		{
			Transform  trans = tempObj.transform;
			trans.parent = parentObj.transform;
			trans.localPosition = Vector3.zero;
			trans.localScale = Vector3.one;
			trans.localRotation = Quaternion.identity;
		}
		return  tempObj;
	}
	//

	/// <summary>
	/// 专门用来缓存UIPrefab的接口 
	/// </summary>
	/// <returns>返回UI的prefab</returns>
	/// <param name="prefabPath">UI的资源地址</param>
	public UnityEngine.Object SpawnUIPrefab( string prefabPath )
	{
		UnityEngine.Object obj = SpawnSyn( prefabPath  , PoolType.DESTROY_CHANGE_SCENE , false,  true );
		return obj;
	}

	/// <summary>
	/// 专门处理UI的接口 
	/// </summary>
	/// <returns>The U.</returns>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="getter">Getter.</param>
	public void SpawnUI( string prefabPath,Action<UnityEngine.Object> getter  )
	{
		Spawn( prefabPath, getter, PoolType.DESTROY_CHANGE_SCENE, true );
	}


	//用于同步缓存下来的需要还给缓存池的资源//
	private Dictionary< string , List < UnityEngine.Object >  > tempSpawnSynObjDic = null;
	private void SetSpawnSynObj(string prefabPath ,  string uiSpace, UnityEngine.Object obj )
	{
		if( tempSpawnSynObjDic == null )
		{
			tempSpawnSynObjDic = new Dictionary<string, List < UnityEngine.Object >>();
		}

		string uiId = uiSpace + prefabPath;
		if( !tempSpawnSynObjDic.ContainsKey( uiId ) )
		{
			tempSpawnSynObjDic.Add( uiId , new List< UnityEngine.Object >() );
		}

		tempSpawnSynObjDic[ uiId ].Add( obj );
	}

	/// <summary>
	/// 在一定时候将资源还给缓存池 , 只对单个有效
	/// </summary>
	/// <param name="prefabPath">UI的资源地址</param>
	/// <param name="uiSpace">作为UI的存储空间， 用于区别不同模块使用相同UI</param>
	public bool DesSpawnSynObject( string prefabPath, string uiSpace )
	{
		if( tempSpawnSynObjDic == null || tempSpawnSynObjDic.Count == 0 )	return false;

		string uiId = uiSpace + prefabPath;
		if( tempSpawnSynObjDic.ContainsKey( uiId ) )
		{
			List< UnityEngine.Object >  spawnUIObject = tempSpawnSynObjDic[ uiId ];
			int number = spawnUIObject.Count;
			for( int i = 0; i < number ; i ++ )
			{
				Despawn( spawnUIObject[i] );
			}
			tempSpawnSynObjDic.Remove( uiId );

			return true;
		}

		return false;
	}

	/// <summary>
	/// 缓存池调用， 使用同步接口生成
	/// </summary>
	/// <returns>The syn.</returns>
	/// <param name="prefabPath">UI的资源地址</param>
	/// <param name="poolType">缓存池的类型</param>
	/// <param name="isNotInstance">是否指定缓存池指定实例化当前资源</param>
	public UnityEngine.Object SpawnSyn( string prefabPath , PoolType poolType = PoolType.DESTROY_CHANGE_SCENE , bool isShare = true, bool isNotInstance = false )
	{
		if( gameSpawnPools == null)
		{
			GameDebuger.Log( "GameSpawnPools is null , please setup the ResourcePoolManager" );
			return null ;
		}
		
		GameSpawnPool gameSpawnPool = gameSpawnPools[(int)poolType];
		if( gameSpawnPool != null )
		{
			if (! gameSpawnPool.IsSpawned( prefabPath ) )
			{
				//如果已经有过缓存， 则直接使用缓存的GamePrefabPool
				GamePrefabPool prePrefabPool = GamePoolManager.Pools.IsSpawned( prefabPath );
				if( isShare && prePrefabPool != null )
				{
					gameSpawnPool.InsertGamePrefabPool(prePrefabPool);
					
					Debug.Log( "Copy Game Prefab Pool!!!!" + prefabPath);
					return ResetGameObject( gameSpawnPool.Spawn(prefabPath) );
				}
				else
				{
					UnityEngine.Object tempPrefab = ResourceLoader.Load( prefabPath );

					//因为是同步的接口， 所有不需要这行代码的操作
//					if( gameSpawnPool.IsSpawned( prefabPath ) )
//					{
//						return ResetGameObject( gameSpawnPool.Spawn( prefabPath ) );
//					}


					if( tempPrefab == null )
					{
						GameDebuger.Log( "Spawn asset not find and prefabPath = " + prefabPath );
						return null;
					}

					if( (tempPrefab is Material ) || (tempPrefab is Texture )  )
					{
						isNotInstance = true;
					}

					GamePrefabPool prefabPool = new GamePrefabPool(prefabPath, tempPrefab , null, isNotInstance );
					
//					Debug.Log( "Insert Game Prefab Pool" + prefabPath );
					gameSpawnPool.InsertGamePrefabPool(prefabPool);
					return ResetGameObject(gameSpawnPool.Spawn(prefabPath)); ;
				}
				
			}
			else
			{
//				Debug.Log( string.Format( "UI : {0} is Spawn" , prefabPath ));
				return ResetGameObject(gameSpawnPool.Spawn(prefabPath));
			}
		}
		else
		{
			Debug.Log( string.Format( "The pool type of {0} is not Exist " , (int)poolType ));
			return null;
		}
	}
	
	private void SpawnNext()
	{
		if (loadingQueue.Count > 0)
		{
			LoadingInfo info = loadingQueue.Dequeue();
			DoSpawn(info.prefabPath, info.getter, info.poolType, info.isNoInstance );
		}
	}
	
	
/*

    /// 准备指定列表数据的Prefab, 由此接口创建的prefab, PoolManager不会主动的销毁， 需要手动调用
    /// <param name="preloadAssetPathList"> 指定资源的地址</param>
    /// <param name="readyAssetFinish">资源准备完毕后的回调</param>
    /// <param name="readyAssetProgress">资源准备进度的回调</param>
    public void ReadyPreloadAssets(List<string> preloadAssetPathList,
                             Action<List< string >> readyAssetFinish    = null,
                             Action<int> readyAssetProgress             = null,
							 PoolType poolType = PoolType.DESTROY_CHANGE_SCENE)
    {
		
		
		GameSpawnPool gameSpawnPool = gameSpawnPools[ (int)poolType ];
		
		if( gameSpawnPool == null )
		{
			GameDebuger.Log( " ResourcePoolManager :: ReadyPreloadAssets  - gameSpawnPool is Null ");
			return;
		}
		
        List<string> readyToLoadAssets = new List<string>();

        foreach (string path in preloadAssetPathList)
        {
            if (!gameSpawnPool.IsSpawned(path))
            {
                if (!readyToLoadAssets.Contains(path))
                {
                    readyToLoadAssets.Add(path);
                }
            }
        }

        if (readyToLoadAssets.Count > 0)
        {
            ResourceLoader.LoadAssetListAsync(readyToLoadAssets, delegate(Dictionary<string, AssetBase> assetDic, List<string> donotExitsAssets)
            {
                foreach (KeyValuePair<string, AssetBase> item in assetDic)
                {
                    AssetBase assetbase = item.Value;
                    //Transform prefabTransform = this.pool.GetPrefab(item.Key);
                    if (gameSpawnPool.IsSpawned(item.Key))
                    {
                        continue;
                    }

                    if (assetbase == null || assetbase.asset == null)
                    {
                        GameDebuger.Log("Spawn asset not find and prefabPath = " + item.Key);
                        continue;
                    }
					
					bool isNoInstance = false;
					if( (assetbase.asset is Material ) || (assetbase.asset is Texture2D ))
					{
						isNoInstance = true;
					}
					
                    GamePrefabPool prefabPool = new GamePrefabPool(item.Key, assetbase.asset, null, isNoInstance);

                    gameSpawnPool.InsertGamePrefabPool(prefabPool); 
                }

                if (readyAssetFinish != null) readyAssetFinish(donotExitsAssets);
            }, readyAssetProgress);
        }
        else
        {
            if (readyAssetFinish != null) readyAssetFinish(null);
        }
        
        
    }
*/
	

	
    public UnityEngine.Object ResetGameObject(UnityEngine.Object go)
    {
        return go;
    }


	public void Despawn( UnityEngine.Object unitGo, PoolType poolType = PoolType.DESTROY_NO_REFERENCE)
    {
        if (unitGo == null || gameSpawnPools == null )
        {
			GameObject.Destroy( unitGo );
            return;
        }
		
		int poolTypeIndex = (int)poolType;
		GameSpawnPool gameSpawnPool = gameSpawnPools[poolTypeIndex];
		
		// 如果在整个缓冲池里面已经有缓冲， 则使用这个缓冲池
		if( gameSpawnPool == null ) 
		{
			GameObject.Destroy( unitGo );
			return;
		}
		
		
		bool isDespawnd = false;
		if ( gameSpawnPool != null )
		{
			if( gameSpawnPool.IsSpawned( unitGo ))
			{
				gameSpawnPool.Despawn( unitGo );
				isDespawnd = true;
			}
			else
			{
				for( int i = 0; i < (int)PoolType.PoolTypeNumber ; i ++ )
				{
					if( i != poolTypeIndex )
					{
						gameSpawnPool = gameSpawnPools[i];
						if( gameSpawnPool != null )
						{
							if( gameSpawnPool.IsSpawned( unitGo ))
							{
								gameSpawnPool.Despawn( unitGo );
								isDespawnd = true;
							}
						}
					}
				}
			}
			
			if( !isDespawnd )
			{
				GameObject.Destroy( unitGo );
			}
		}
    }
	
	
	/// <summary>
	/// 场景切换的时候， 清空gameSpawnPool的缓冲数据
	/// </summary>
	public void DespawnChangeScene()
	{
		GameSpawnPool gameSpawnPool = gameSpawnPools[ (int) PoolType.DESTROY_CHANGE_SCENE ];
		
		if( gameSpawnPool != null )
		{
			gameSpawnPool.DestroyAllSpwanPools();
			
//			Resources.UnloadUnusedAssets();
		}
	}
	
	/// <summary>
	/// 获取SpawnPools的描述
	/// </summary>
	/// <returns>
	/// The spawned situation.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public string GetSpawnedSituation( PoolType type )
	{
		if( gameSpawnPools == null ) return string.Empty;
		
		GameSpawnPool gameSpawnPool = gameSpawnPools[ (int)type ];
		
		if( gameSpawnPool != null )
		{
			return gameSpawnPool.Tostring();
		}
		else
		{
			return string.Empty;
		}
 	}
}

