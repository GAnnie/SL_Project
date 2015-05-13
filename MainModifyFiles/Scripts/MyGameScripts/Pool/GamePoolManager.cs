using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Game pool manager ， 是管理所有SpawnPools的主容器， 每生成一个SpawnPool都会自动添加到GamePoolManager进行管理
/// </summary>
public class GamePoolManager
{
	 public static readonly GameSpawnPoolsDict Pools = new GameSpawnPoolsDict();
}


#region Class of GameSpawnPoolsDict

/// <summary>
/// SpawnPool 的真正的容器类。
/// </summary>
public class GameSpawnPoolsDict 
{
	private Dictionary< string , GameSpawnPool > _gameSpwanPoolDict = new Dictionary<string, GameSpawnPool>();
	
	/// <summary>
	/// 生成一个GameSpawnPool ， 并且加入 owner 
	/// </summary>
	/// <param name='poolName'>
	/// Pool name.
	/// </param>
	/// <param name='owner'>
	/// Owner.
	/// </param>
	public GameSpawnPool Create( string poolName, GameObject owner)
	{
		if( owner == null ) return null;
		
		string ownerName = owner.name;
        try
        {
            owner.gameObject.name = poolName;

            return owner.AddComponent<GameSpawnPool>();
        }
        finally
        {
            owner.gameObject.name = ownerName;
        }
	}
	
	
	/// <summary>
	/// Adds the game spwan pool.
	/// </summary>
	/// <param name='spawnPool'>
	/// Spawn pool.
	/// </param>
	public void AddGameSpwanPool( GameSpawnPool spawnPool)
	{
		if( this._gameSpwanPoolDict.ContainsKey( spawnPool.poolName ))
		{
			Debug.LogError(string.Format("A pool with the name '{0}' already exists. " +
			                                            "This should only happen if a SpawnPool with " +
			                                            "this name is added to a scene twice.",
			                                         spawnPool.poolName));	
			
			return;
		}
		else
		{
			this._gameSpwanPoolDict.Add( spawnPool.poolName,  spawnPool );
		}
	}
	
	/// <summary>
	/// Removes the game spawn pool.
	/// </summary>
	/// <returns>
	/// The game spawn pool.
	/// </returns>
	/// <param name='spawnPool'>
	/// If set to <c>true</c> spawn pool.
	/// </param>
	public bool RemoveGameSpawnPool( GameSpawnPool spawnPool )
	{
        if (!this._gameSpwanPoolDict.ContainsKey(spawnPool.poolName))
        {
            Debug.LogError(string.Format("PoolManager: Unable to remove '{0}'. " +
                                            "Pool not in PoolManager",
                                        spawnPool.poolName));
            return false;
        }

        this._gameSpwanPoolDict.Remove(spawnPool.poolName);
        return true;		
	}
	
	
#region GamePrefabPoolCounter
	private class GamePrefabPoolCounter 
	{
		
		public UnityEngine.Object prefab = null;
		
		public string name 			     = string.Empty;
		
		public bool   isNoInst			 = false;
		
		public int count;
		
		
		public GamePrefabPoolCounter( GamePrefabPool pool)
		{
			this.prefab    = pool.prefab;
			this.name      = pool.prefabName;
			this.isNoInst  = pool.isNoInstance;
			
			this.count     = 1;
		}
	}
#endregion	
	


	private Dictionary< string , GamePrefabPoolCounter > _gamePrefabPools = new Dictionary<string, GamePrefabPoolCounter>();
	
	/// <summary>
	/// 总体管理资源缓冲情况
	/// </summary>
	/// <param name='spawnPoolName'>
	/// 资源的SpawnPool的名字
	/// </param>
	/// <param name='prefabPoolName'>
	/// 资源的PrefabPool的名字
	/// </param>
	public bool AddGamePrefabPool( GamePrefabPool prefabPool )
	{
		if( _gamePrefabPools.ContainsKey( prefabPool.prefabName ))
		{
			GamePrefabPoolCounter counter = _gamePrefabPools[ prefabPool.prefabName ];
			counter.count ++;
		}
		
		else
		{
			GamePrefabPoolCounter counter = new GamePrefabPoolCounter( prefabPool );
			_gamePrefabPools.Add( prefabPool.prefabName, counter );
		}
		return true;
	}
	
	/// <summary>
	/// 删除PrefabPool的缓冲
	/// </summary>
	/// <param name='prefabPoolName'>
	/// Prefab pool name.
	/// </param>
	public bool DeleteGamePrefabPool( GamePrefabPool prefabPool )
	{
		if( _gamePrefabPools.ContainsKey( prefabPool.prefabName))
		{
			GamePrefabPoolCounter counter = _gamePrefabPools[ prefabPool.prefabName ];
			
			counter.count --;
			
			if( counter.count <= 0 )
			{
				_gamePrefabPools.Remove( prefabPool.prefabName );
			}
			return true;
		}
		else
		{
			return false;
		}
	}
	
	/// <summary>
	/// 判断指定的prefab是否已经被缓冲， 这个判断是对所有的spawnPool的缓冲资源进行判断
	/// </summary>
	/// <returns>
	/// <c>true</c> 如果已经返回的非空， 则代表已经被某个spawnPool缓冲
	/// </returns>
	/// <param name='prefabPath'>
	/// Prefab path.
	/// </param>
	public GamePrefabPool IsSpawned( string prefabPath )
	{
		if( _gamePrefabPools.ContainsKey( prefabPath ))
		{
			GamePrefabPool newPool = new GamePrefabPool( _gamePrefabPools[prefabPath].name, _gamePrefabPools[prefabPath].prefab, null, _gamePrefabPools[prefabPath].isNoInst );
			return newPool;
		}
		else
		{
			return null;
		}
	}
}


#endregion

#region Class of GameSpwanPool
/// <summary>
/// Game spawn pool.  是缓存池的主体 ， 里面包含着Game Prefabe Pools
/// </summary>
public class GameSpawnPool : MonoBehaviour
{
	#region Parameters
	private GameSpawnPoolsDict _spawnPools = null;
	
	/// <summary>
	//GameSpawn名字
	/// </summary>
	public string poolName = string.Empty;
	
	/// <summary>
	//是否设置实例的Scale值， 如果matchPoolScale 为true ， 则将其scale值设置为Vector.zero;
	/// </summary>
	public bool matchPoolScale = false;

	/// <summary>
	//如果设置未True,则实例将会以当前gameobject的layer相同
	/// </summary>
	public bool matchPoolLayer = false;
	
	/// <summary>
	/// 是否删除长期没有使用的GamePrefabPool
	/// </summary>
	public bool autoRemovePrefabPool = false;
	
	/// <summary>
	/// 自动检测GamePrefabPool的相隔时间， 单位为 ： 秒
	/// </summary>
	public float samplePrefabPoolDelayTime = 20.0f;
	
	/// <summary>
	/// GamePrefabPool相隔多长时间没有进行实例， 就会自动删除
	/// </summary>
	public float removePrefabPoolTime    = 300.0f;
	
	/// <summary>
	/// SpawnPool的组
	/// </summary>
	public Transform group = null;
	
	/// <summary>
	//如果为true, 则会调用DontDestroyOnLoad
	/// </summary>
	public bool dontDestroyOnLoad = false;
	
	/// <summary>
	//是否答应logMessage 信息
	/// </summary>
	public bool logMessages = false;
	
	/// <summary>
	/// 对于加进SpawnPool的 PrefabPool, 都会默认使用这个Option作为设置
	/// </summary>
	public GamePrefabPoolOption defaultOption = null;
	
	/// <summary>
	//保存GamePrefabPool的Dictionary
	/// </summary>
	private Dictionary<string, GamePrefabPool> _prefabPools = new Dictionary<string, GamePrefabPool>();
	
	/// <summary>
	//保存 实例ID 和 GamePrefabPool的对应关系
	/// </summary>
	private Dictionary< int , GamePrefabPool > _prefabInstanceIDDict = new Dictionary<int, GamePrefabPool>();
	
	#endregion
	
	
	#region Constructor and Init
	private void Awake()
	{
        if (this.dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
		
		this.group = this.transform;
		
		if( this.poolName == string.Empty)
		{
			this.poolName = this.gameObject.name;
		}
		
		if( this.logMessages )
		{
			GameDebuger.Log( string.Format( "GameSpawn {0} : Initializing ...." , this.poolName ));
		}
		
		GamePoolManager.Pools.AddGameSpwanPool( this );
		
		_spawnPools = GamePoolManager.Pools;
	}
	
	private void Start()
	{
		if( autoRemovePrefabPool )
		{
			this.InvokeRepeating( "RemoveLongTimeNoInstanecPrefabPool", samplePrefabPoolDelayTime, samplePrefabPoolDelayTime );
		}
	}
	
	private void OnDestroy()
	{
		if( this.logMessages )
		{
			GameDebuger.Log(string.Format("SpawnPool {0}: Destroying...", this.poolName));
		}
		
		this.CancelInvoke();
		
		GamePoolManager.Pools.RemoveGameSpawnPool( this );
		
		DestroyAllSpwanPools();
	}
	
	
	/// <summary>
	/// 删除所有spawnpool实例
	/// </summary>
	public void DestroyAllSpwanPools()
	{
		this.StopAllCoroutines();
		
		this._prefabInstanceIDDict.Clear();
		
		// Clean-up
		foreach( KeyValuePair< string, GamePrefabPool > item in this._prefabPools )
		{
			item.Value.SelfDestruct();
			
			if ( _spawnPools != null )
			{
				_spawnPools.DeleteGamePrefabPool( item.Value );
			}
		}
		
		this._prefabPools.Clear();
	}
	
	#endregion
	
	/// <summary>
	/// 如果设置autoRemovePrefabPool， 会每隔samplePrefabPoolDelayTime调用此函数来清楚太久没有实例化的资源
	/// </summary>
	private void RemoveLongTimeNoInstanecPrefabPool()
	{
		if( !autoRemovePrefabPool ) return;

		float currentTime = UnityEngine.Time.time;
		
		List< GamePrefabPool > removeGamePrefabPoolList = null;
		foreach( KeyValuePair< string ,GamePrefabPool > item in this._prefabPools )
		{
			GamePrefabPool pool = item.Value;
			if( ((currentTime - pool.lastInstanceTime ) > removePrefabPoolTime) && 
				((pool.spawnedCount == 0 ) || ( pool.IsSpawnedSomeoneNull() )))
			{
				if( removeGamePrefabPoolList == null )
				{
					removeGamePrefabPoolList = new List<GamePrefabPool>();
				}
				
				if( removeGamePrefabPoolList != null )
				{
					removeGamePrefabPoolList.Add( pool );
				}
			}
		}
		
		if( removeGamePrefabPoolList != null )
		{
			for( int i = 0 ; i < removeGamePrefabPoolList.Count ;  i ++ )
			{
				RemoveGamePrefabPool( removeGamePrefabPoolList[i] );
			}
			
			removeGamePrefabPoolList.Clear();
			removeGamePrefabPoolList = null;
		}
	}
	
	
	/// <summary>
	/// 拷贝原本的GamePrefabPool
	/// </summary>
	/// <param name='gamePrefabPool'>
	/// Game prefab pool.
	/// </param>
	public void CopyGamePrefabPool( GamePrefabPool gamePrefabPool)
	{
		InsertGamePrefabPool( new GamePrefabPool( gamePrefabPool ));
	}

	/// <summary>
	/// 插进GamePrefabPool
	/// </summary>
	/// <param name='gamePrefabPool'>
	/// Game prefab pool.
	/// </param>
	public void InsertGamePrefabPool( GamePrefabPool gamePrefabPool )
	{
		bool isAlreadyPool = this.IsSpawned( gamePrefabPool.prefabName );
		if( !isAlreadyPool )
		{
			gamePrefabPool.spawnPool = this;
			
			this._prefabPools.Add( gamePrefabPool.prefabName, gamePrefabPool );
			
			if( gamePrefabPool.option.preloaded )
			{
				if( this.logMessages )
				{
					GameDebuger.Log( string.Format("SpawnPool {0}: Preloading {1}",
	                                           this.poolName,
	                                           gamePrefabPool.option.preloadAmount,
	                                           gamePrefabPool.prefabName));
				}
				
				
				//预实例化实例
				gamePrefabPool.PreloadInstances();
				
			}
			
			//设置统一的参数
			if( defaultOption != null )
			{
				gamePrefabPool.option = defaultOption;
			}
			
			if( _spawnPools != null )
			{
				_spawnPools.AddGamePrefabPool( gamePrefabPool );
			}
		}
		
	}
	
	public void RemoveGamePrefabPool( GamePrefabPool gamePrefabPool )
	{
		string prefabName = gamePrefabPool.prefabName;
		
		if( this._prefabPools.ContainsKey( prefabName ))
		{
			this._prefabPools.Remove( prefabName );
			
			gamePrefabPool.SelfDestruct();
			
			if( _spawnPools != null )
			{
				_spawnPools.DeleteGamePrefabPool( gamePrefabPool );
			}
		}
	}
	
	
	/// <summary>
	///获取指定PrefabPool的缓存实例
	/// </summary>
	/// <param name='prefabPath'>
	/// Prefab path.
	/// </param>
	public UnityEngine.Object Spawn( string prefabName, Vector3 pos,  Quaternion rot )
	{
		UnityEngine.Object inst = null;
		
		if( !this.IsSpawned( prefabName ))
		{
			if( this.logMessages )
			{
				GameDebuger.Log( string.Format( "PrefabPool : {0} can not found " , prefabName ));
			}
			return null;
		}
		
		#region Use form Pool
		if( _prefabPools.ContainsKey( prefabName ))
		{
			GamePrefabPool pool = _prefabPools[ prefabName ];
			
			//如果设置为没有实例
			if( pool.isNoInstance )
			{
				return pool.SpawnInstance();
			}
			
			inst = pool.SpawnInstance( pos, rot );// pool.SpwanInstance( pos, rot );
			
			if( inst == null )
			{
				if( this.logMessages )
				{
					GameDebuger.Log( string.Format( "PrefabPool : {0} Initialize Error!! " , prefabName ));					
				}
			}
			else
			{
				int instanceId = inst.GetInstanceID();
				
				if( _prefabInstanceIDDict.ContainsKey( instanceId ))
				{
					Debug.LogError( string.Format("Prefab : {0} 's instatnceid {1} is exits !!", prefabName, instanceId ));
				}
				else
				{
					_prefabInstanceIDDict.Add( instanceId, pool );
				}
			}
		}
		#endregion
		
		return inst;
	}
	
	/// <summary>
	///获取指定PrefabPool的缓存实例
	/// </summary>
	/// <param name='prefabPath'>
	/// Prefab path.
	/// </param>
	public UnityEngine.Object Spawn( string prefabPath )
	{
		return this.Spawn( prefabPath, Vector3.zero, Quaternion.identity );
	}
	
	public void Despawn( UnityEngine.Object inst )
	{
		if( inst == null ) return ;
		
		bool despawned = false;
		
		int instanceID = inst.GetInstanceID();
		if( _prefabInstanceIDDict.ContainsKey( instanceID ))
		{
			GamePrefabPool pool = _prefabInstanceIDDict[ instanceID ];
			
			if( pool.IsSpawnedContain( inst ))
			{
				pool.DespawnInstance( inst );
			}
			else
			{
				Debug.LogError( string.Format( "SpawnPool {0}: {1} has already been despawned. " +
                                   "You cannot despawn something more than once!",
                                    this.poolName,
                                    inst.name));
			}
			
			_prefabInstanceIDDict.Remove( instanceID );
		}
		else
		{
			if( this.logMessages )
			{
	            GameDebuger.Log(string.Format("SpawnPool {0}: {1} not found in SpawnPool",
	                           this.poolName,
	                           inst.name));				
			}
			
		}
		
	}
	
	public void Despawn( UnityEngine.Object inst,  float seconds)
	{
		if( seconds <= 0 )
		{
			this.Despawn( inst);
		}
		else
		{
			this.StartCoroutine( this.DoDespawnAfterSeconds( inst, seconds ));
		}
	}
	
	private IEnumerator DoDespawnAfterSeconds( UnityEngine.Object inst, float seconds)
	{
        yield return new WaitForSeconds(seconds);
        this.Despawn(inst);		
	}
	
	
	/// <summary>
	/// 获取缓存池里面的prefab
	/// </summary>
	/// <returns>
	/// 指定缓存池里面的prefab
	/// </returns>
	/// <param name='prefabPath'>
	/// 指定缓存池的地址
	/// </param>
	public UnityEngine.Object GetPrefab( string prefabPath )
	{
		if( _prefabPools.ContainsKey( prefabPath ))
		{
			return _prefabPools[prefabPath].prefab;
		}
		else
		{
			if( this.logMessages)
			{
				GameDebuger.Log( string.Format( "SpawnPool {0} : PrefabPools is not contain Prefab {1} ", 
												this.poolName, 
												prefabPath ));
			}
		}
		
		return null;
	}
	
	
	/// <summary>
	/// 判断指定prefabpool是否存在于spawnpool里面
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is spawned the specified prefabPath; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='prefabPath'>
	/// If set to <c>true</c> prefab path.
	/// </param>
	public bool IsSpawned( string prefabPath )
	{
		return _prefabPools.ContainsKey( prefabPath ); 
	}
	
	public bool IsSpawned( UnityEngine.Object inst )
	{
		return this.IsSpawned( inst.GetInstanceID());
	}
	
	public bool IsSpawned( int instanceId )
	{
		return _prefabInstanceIDDict.ContainsKey( instanceId );	
	}
	
	
	
	public string Tostring()
	{
#if UNITY_DEBUG || UNITY_EDITOR		
		List< string > returnString = new List<string>();
		foreach( KeyValuePair< string, GamePrefabPool > item in _prefabPools )
		{
			if(  item.Value.isNoInstance )
			{
				returnString.Add( string.Format( "Pn:{0}, NotInst", 
												item.Value.prefab.name
												));
			}
			else
			{
				returnString.Add( string.Format( "Pn:{0},s:[{1}{2}],ds:[{3}{4}]\r\n",
													item.Value.prefab.name,
													item.Value.spawnedCount,
													item.Value.IsSpawnedSomeoneNull() ? "y":"n",
													item.Value.despawnedCount,
													item.Value.IsDeSpawnedSomeoneNull() ? "y":"n"					
												));				
			}
		}
		
		return System.String.Join(" ", returnString.ToArray());
#else
		return string.Empty;
#endif

	}
}

#endregion

#region Class of GamePrefabPoolOption
public class GamePrefabPoolOption
{
	/// <summary>
	//是否预先加载资源
	/// </summary>
	public bool preloaded = false;
	
	/// <summary>
	//预加载多少个实例
	/// </summary>
	public int preloadAmount = 1;
	
	/// <summary>
	//是否延迟预加载
	/// </summary>
	public bool preloadTime = false;
	
	/// <summary>
	//需要多少帧去实例化所有需要预加载的资源
	/// </summary>
	public int preloadFrames = 2;
	
	/// <summary>
	//延迟预加载的时间(秒）
	/// </summary>
	public float preloadDelay = 0;
	
	
	/// <summary>
	//是否限制实例化的数量， 如果limitInstatnce 为 true , 当前实例化的数量超过最大数量， 则直接放回null
	/// </summary>
	public bool limitInstance = false;
	

	/// <summary>
	//当前最大实例化的数量， 只有当limitInstance 为 true的时候有效	
	/// </summary>
	public int limitAmount = 100;

	/// <summary>
	//正常情况下，  如果当前实例的数量超过 limitAmount的时候， 会放回null, 当此项如果为true, 会强制使 spawned的第一个
	//的实例despawned, 并且会放回此 实例， 这样可以保证所有的调用SpawnInstance都有返回
	/// </summary>
	public bool limitFIFO = false;
	
	/// <summary>
	/// 是否删减多余的实例来释放资源的内存
	/// </summary>
	public bool cullDespawned = false;
	
	/// <summary>
	/// 维持 spawned + despawned 实例的总数量
	/// </summary>
	public int cullAbove = 50;
	
	/// <summary>
	/// 删减之前的等待时间
	/// </summary>
	public int cullDelay = 60;
	
	/// <summary>
	/// 一帧最大的删除数量
	/// </summary>
	public int cullMaxPerPass = 5;
	
	/// <summary>
	/// 是否Prefab会自动删除， 设置了这个开关， 当_spawned 数量未0( 即没有引用 ）， 就会开启自动删去的功能
	/// </summary>
	public bool autoDeletePrefab = false;
	
	/// <summary>
	/// 自动删除的延迟时间
	/// </summary>
	public int autoDeleteDelay = 60;

	
	/// <summary>
	/// GamePrefabPool 的写log的控制
	/// </summary>
	public bool logMessages = false;  // Used by the inspector
	
}
#endregion

#region class of GamePrefabPool

/// <summary>
/// Game prefab pool.
/// </summary>
/// <exception cref='MissingReferenceException'>
/// Is thrown when the missing reference exception.
/// </exception>
public class GamePrefabPool
{
	private static GamePrefabPoolOption defaultPrefabPoolOption = new GamePrefabPoolOption();
	
	/// <summary>
	//资源名字
	/// </summary>
    public string prefabName;

	/// <summary>
	//保存资源prefab
	/// </summary>
    public UnityEngine.Object prefab;
	
	/// <summary>
	//指向于GameSpawnPool
	/// </summary>
	public GameSpawnPool spawnPool = null;
	
	/// <summary>
	/// 最后GamePrefabPool实例化的时间， 这个时间用来判断个如果资源长期没有实例化， 则会自动销毁
	/// </summary>
	public float lastInstanceTime  { get; private set;}
	
	/// <summary>
	//是否不存在实例
	/// </summary>
	public bool isNoInstance { get ; private set;}
	
	/// <summary>
	//强制控制GamePrefabPool不答应log信息
	/// </summary>/
    private bool forceLoggingSilent = false;
	
	/// <summary>
	/// GamePrefabPool的参数项
	/// </summary>
	private GamePrefabPoolOption _option = null;
	
	public GamePrefabPoolOption option 
	{
		get
		{
			return _option;
		}
		set
		{
			if( value != null )
			{
				_option = value;
			}
		}
	}
	
	
    public bool logMessages            // Read-only
    {
        get 
        {
            if (forceLoggingSilent) return false;

            if (this.spawnPool.logMessages)
                return this.spawnPool.logMessages;
            else
                return this._option.logMessages; 
        }
    }
	
	

	/// <summary>
	/// _spawned 记录当前PrefabPool 的所有spawn 的实例
	/// </summary>
	private List< UnityEngine.Object > _spawned = new List<UnityEngine.Object>();
	public int spawnedCount { get { return this._spawned.Count;}}
	public bool IsSpawnedContain( UnityEngine.Object inst )
	{
		return _spawned.Contains( inst );
	}
	
	public bool IsSpawnedSomeoneNull()
	{
		for( int i= 0 ; i < _spawned.Count ; i ++)
		{
			if( _spawned[i] == null ) 
			{
				return true;
			}
		}
		return false;
	}
	
	
	/// <summary>
	/// _despawned  记录了当前PrefabPool 的所有 despawned的实例
	/// </summary>
	private List< UnityEngine.Object > _despawned = new List<UnityEngine.Object>();
	public int despawnedCount { get { return this._despawned.Count;}}
	public bool IsDespawnedContain( UnityEngine.Object inst )
	{
		return _despawned.Contains( inst );
	}
	
	public bool IsDeSpawnedSomeoneNull()
	{
		for( int i= 0 ; i < _despawned.Count ; i ++)
		{
			if( _despawned[i] == null ) 
			{
				return true;
			}
		}
		return false;
	}	
	
	/// <summary>
	/// spawned + despawned的总量
	/// </summary>
	public int totalCount
	{
		get
		{
			int count = _spawned.Count + _despawned.Count;
			return count;
		}
	}
	
	public GamePrefabPool( GamePrefabPoolOption option = null,  bool isNoInstance = false )
	{
		this.isNoInstance = isNoInstance;
		
		if( option == null )
		{
			this._option = defaultPrefabPoolOption;
		}
		else
		{
			this._option = option;
		}
	}
	
	
    public GamePrefabPool(string prefabName, UnityEngine.Object prefab, GamePrefabPoolOption option = null, bool isNoInstance = false )
    {
        this.prefabName 	= prefabName;

        this.prefab 		= prefab;
		
		this.isNoInstance 	= isNoInstance;
		
		if( option == null )
		{
			this._option = defaultPrefabPoolOption;
		}
		else
		{
			this._option = option;
		}		
    }
	
	public GamePrefabPool( GamePrefabPool pool )
	{
		this.prefabName  = pool.prefabName;
		
		this.prefab      = pool.prefab;
		
		this.isNoInstance = isNoInstance;
		
		this._option      = pool._option;
	}
	
	
	/// <summary>
	/// GamePrefabPool自身的析构函数, 返回正在缓冲池外的资源ID
	/// </summary>
	public void SelfDestruct()
	{
		this.prefab = null;
		
		this.spawnPool = null;
		
		if( !isNoInstance )
		{
			foreach( UnityEngine.Object inst in  this._spawned )
			{
				GameObject.Destroy( inst );
			}
			this._spawned.Clear();
			
			
			foreach( UnityEngine.Object inst in  this._despawned )
			{
				GameObject.Destroy( inst );
			}
			this._despawned.Clear();
		}
	}
	
	
	/// <summary>
	/// 预加载指定数量实例
	/// </summary>
	public void PreloadInstances()
	{
        // If this has already been run for this PrefabPool, there is something
        //   wrong!
        if (!this._option.preloaded)
        {
            return;
        }
		
		
        if (this.prefab == null)
        {
            Debug.LogError(string.Format("SpawnPool {0} ({1}): Prefab cannot be null.",
                                         this.spawnPool.poolName,
                                         this.prefabName));

            return;
        }

        // Protect against preloading more than the limit amount setting
        //   This prevents an infinite loop on load if FIFO is used.
        if (this._option.limitInstance && this._option.preloadAmount > this._option.limitAmount)
        {
            Debug.LogWarning
            (
                string.Format
                (
                    "SpawnPool {0} ({1}): " +
                        "You turned ON 'Limit Instances' and entered a " +
                        "'Limit Amount' greater than the 'Preload Amount'! " +
                        "Setting preload amount to limit amount.",
                     this.spawnPool.poolName,
                     this.prefab.name
                )
            );
            
            this._option.preloadAmount = this._option.limitAmount;
        }

        // Notify the user if they made a mistake using Culling
        //   (First check is cheap)
        if (this._option.cullDespawned && this._option.preloadAmount > this._option.cullAbove)
        {
            Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
				                "You turned ON Culling and entered a 'Cull Above' threshold " +
				                "greater than the 'Preload Amount'! This will cause the " +
				                "culling feature to trigger immediatly, which is wrong " +
				                "conceptually. Only use culling for extreme situations. " +
				                "See the docs.",
				                this.spawnPool.poolName,
				                this.prefab.name
				            ));
        }

        if (this._option.preloadTime)
        {
            if (this._option.preloadFrames > this._option.preloadAmount)
            {
                Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
                    "Preloading over-time ishao  on but the frame duration is greater " +
                    "than the number of instances to preload. The minimum spawned " +
                    "per frame is 1, so the maximum time is the same as the number " +
                    "of instances. Changing the preloadFrames value...",
                    this.spawnPool.poolName,
                    this.prefab.name
                ));

                this._option.preloadFrames = this._option.preloadAmount;
            }

            this.spawnPool.StartCoroutine(this.PreloadOverTime());
        }
        else
        {
            // Reduce debug spam: Turn off this.logMessages then set it back when done.
            this.forceLoggingSilent = true;

            UnityEngine.Object inst;
            while (this.totalCount < this._option.preloadAmount) // Total count will update
            {
                // Preload...
                // This will parent, position and orient the instance
                //   under the SpawnPool.group
                inst = this.SpawnNew();
                this.DespawnInstance(inst, false);
            }
            
            // Restore the previous setting
            this.forceLoggingSilent = false;
        }		
	}
	
	
	private IEnumerator PreloadOverTime()
    {
        yield return new WaitForSeconds(this._option.preloadDelay);

        UnityEngine.Object inst;

        // subtract anything spawned by other scripts, just in case
        int amount = this._option.preloadAmount - this.totalCount;
        if (amount <= 0)
            yield break;

        // This does the division and sets the remainder as an out value.
        int remainder;
        int numPerFrame = System.Math.DivRem(amount, this._option.preloadFrames, out remainder);

        // Reduce debug spam: Turn off this.logMessages then set it back when done.
        this.forceLoggingSilent = true;

        int numThisFrame;
        for (int i = 0; i < this._option.preloadFrames; i++)
        {
            // Add the remainder to the *last* frame
            numThisFrame = numPerFrame;
            if (i == this._option.preloadFrames - 1)
            {
                numThisFrame += remainder;
            }

            for (int n = 0; n < numThisFrame; n++)
            {
                // Preload...
                // This will parent, position and orient the instance
                //   under the SpawnPool.group
                inst = this.SpawnNew();
                if (inst != null)
                    this.DespawnInstance(inst, false);

                yield return null;
            }

            // Safety check in case something else is making instances. 
            //   Quit early if done early
            if (this.totalCount > this._option.preloadAmount)
                break;
        }

        // Restore the previous setting
        this.forceLoggingSilent = false;
    }
	
	
	/// <summary>
	/// 缓存中获取实例化资源
	/// </summary>
	/// <returns>
	/// 返回实例化资源
	/// </returns>
	/// <param name='pos'>
	/// 实例的position
	/// </param>
	/// <param name='rot'>
	/// 实例的rotation
	/// </param>
	public UnityEngine.Object SpawnInstance( Vector3 pos, Quaternion rot )
	{
		//Set Last Instance Time
		this.lastInstanceTime = UnityEngine.Time.time;		
		
		//如果没有产生实例的， 直接返回prefab， 并且不做任何缓存操作
		if( isNoInstance )
		{
			return this.prefab;
		}
		
		
		//如果设置了limitFIFO 的选项， 如果超过能够实例化的数量， 就会从_spawned 池里面强制回收第一个实例， 并且在这一次调用中返回
		//这样可以保证每一次调用都能够有实例返回
        if (this._option.limitInstance  && 
			this._option.limitFIFO 		&&
            this._spawned.Count >= this._option.limitAmount)
        {
            UnityEngine.Object firstIn = this._spawned[0];

            if (this.logMessages)
            {
                Debug.Log(string.Format
                (
                    "SpawnPool {0} ({1}): " +
                        "LIMIT REACHED! FIFO=True. Calling despawning for {2}...",
	                    this.spawnPool.poolName,
	                    this.prefab.name,
	                    firstIn
                ));
            }

            //this.DespawnInstance(firstIn);

            this.spawnPool.Despawn( firstIn );
        }
		
		UnityEngine.Object inst;
		if( this._despawned.Count == 0)
		{
			inst = this.SpawnNew( pos, rot );
		}
		else
		{
			//处理可能因为人为的错误导致实例化出来的内容已经被删除
			while( true )
			{
				if( this._despawned.Count > 0 )
				{
					inst = this._despawned[0];
					this._despawned.RemoveAt(0);
				}
				else
				{
					inst = this.SpawnNew( pos, rot );
				}
				
				
				if ( inst == null )
				{
					string msg = string.Format("Prefab {0} instance was deleted ", this.prefabName);
					
#if UNITY_EDITOR || UNITY_DEBUG
	                //TipManager.AddTip( string.Format("缓存池资源丢失：" + 	this.prefabName));
					//Debug.LogError( msg );
					GameDebuger.Log( msg);
#else
					GameDebuger.Log( msg);
#endif
	
				}
				else
				{
					this._spawned.Add(inst);
					break;
				}
			}
			
			
            if (inst == null)
            {
                string msg = "Make sure you didn't delete a despawned instance directly. " + this.prefabName;
                throw new MissingReferenceException(msg);
            }
			
			
            if (this.logMessages)
			{
                Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.",
                                        this.spawnPool.poolName,
                                        this.prefab.name,
                                        inst.name));					
			}

		}

		if( inst is GameObject )
		{
			( inst as GameObject).transform.position = pos;
			( inst as GameObject).transform.rotation = rot;
			
			( inst as GameObject).SetActive( true );
		}	
		
        return inst;	
	}
	
	public UnityEngine.Object SpawnInstance()
	{
		return this.SpawnInstance(Vector3.zero, Quaternion.identity);
	}
	
	/// <summary>
	/// 充缓存池里创建新的实例
	/// </summary>
	/// <returns>
	/// The new.
	/// </returns>
	/// <param name='pos'>
	/// Position.
	/// </param>
	/// <param name='rot'>
	/// Rot.
	/// </param> 
	private UnityEngine.Object SpawnNew( Vector3 pos, Quaternion rot)
	{
		this.lastInstanceTime = UnityEngine.Time.time;
		
		//如果不产生实例， 则直接返回
		if( isNoInstance )
		{
			return this.prefab;
		}
			
		
        // Handle limiting if the limit was used and reached.
        if (this._option.limitInstance && this._spawned.Count >= this._option.limitAmount)
        {
            if (this.logMessages)
            {
                Debug.Log(string.Format
                (
                    "SpawnPool {0} ({1}): " +
                            "LIMIT REACHED! Not creating new instances! (Returning null)",
	                        this.spawnPool.poolName,
	                        this.prefabName
                ));
            }

            return null;
        }

        if (this.spawnPool == null)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!");
        }
		
		if( this.prefab == null )
		{
			Debug.Log(" GamePrefabPool : " + this.prefabName + " 's prefab is null");
		}
		
        UnityEngine.Object inst = UnityEngine.Object.Instantiate(this.prefab, pos, rot);
		
		this.nameInstance(inst);  // Adds the number to the end

        if (this.spawnPool.matchPoolScale && (inst is GameObject))
		{
            (inst as GameObject ).transform.localScale = Vector3.one;
		}

        if (this.spawnPool.matchPoolLayer)
            this.SetRecursively(inst, this.spawnPool.gameObject.layer);

        // Start tracking the new instance
        this._spawned.Add(inst);

        if (this.logMessages)
            Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.",
                                    this.spawnPool.poolName,
                                    this.prefabName,
                                    inst.name ));
		

		
        return inst;		
	}
	
	private UnityEngine.Object SpawnNew()
	{
		return this.SpawnNew( Vector3.zero, Quaternion.identity );
	}
	
	
	
	/// <summary>
	/// 是否正在进行裁剪
	/// </summary>
	private bool _cullingActive = false;
	
	
	/// <summary>
	/// 是否自动删除中
	/// </summary>
	private bool _autoDeleteActive = false;
	
	/// <summary>
	/// Despawn 实例， 实例资源返回到spawned 池里面
	/// </summary>
	/// <returns>
	/// The instance.
	/// </returns>
	/// <param name='inst'>
	/// If set to <c>true</c> inst.
	/// </param>
	public bool DespawnInstance( UnityEngine.Object inst )
	{
		//如果此PrefabPool 是 NoInstance， 直接返回
		if( isNoInstance ) return true;
		
		return this.DespawnInstance( inst, false );
	}
	
	public bool DespawnInstance( UnityEngine.Object inst, bool sendEventMessage )
	{
		//Set Last Instance Time
		this.lastInstanceTime = UnityEngine.Time.time;
		
		//如果此PrefabPool 是 NoInstance， 直接返回
		if( isNoInstance ) return true;
		
		if( ! this.IsSpawnedContain( inst ))
		{
	        if (this.logMessages)
			{
	            GameDebuger.Log(string.Format("SpawnPool {0}, GamePrefabPool {1} is Not Spwaned instance of {2}",
	                                   this.spawnPool.poolName,
	                                   this.prefabName,
	                                   inst.name));
			}
			return false;
		}
		
		
        if (this.logMessages)
		{
            GameDebuger.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'",
                                   this.spawnPool.poolName,
                                   this.prefabName,
                                   inst.name));			
		}
		
		
		this._spawned.Remove( inst );
		this._despawned.Add ( inst );
		
		if( sendEventMessage )
		{
			if( inst is GameObject )
			{
				( inst as GameObject ).SendMessage("OnDespawned", SendMessageOptions.DontRequireReceiver); 
			}
		}
		
		
		//隐藏实例并调整其父结构层
		if( inst is GameObject )
		{
			if( this.spawnPool.group != null )
			{
				(inst as GameObject).transform.parent = this.spawnPool.group;
			}
			(inst as GameObject).SetActive( false );
		}
		
		
		if (!this._cullingActive &&   // Cheap & Singleton. Only trigger once!
            this._option.cullDespawned &&    // Is the feature even on? Cheap too.
            this.totalCount > this._option.cullAbove)   // Criteria met?
        {
            this._cullingActive = true;
            this.spawnPool.StartCoroutine(CullDespawned());
        }
		else
		{
			if( this._option.autoDeletePrefab  &&
				this.spawnedCount == 0 &&
				!this._autoDeleteActive)
			{
				this._autoDeleteActive = true;
				this.spawnPool.StartCoroutine( AutoDelPrefab());
			}
		}
		
		
        return true;
		
	}
	
	/// <summary>
	/// 固定时间后自动删除当前GamePrefabPool
	/// </summary>
	/// <returns>
	/// The del prefab.
	/// </returns>
	internal IEnumerator AutoDelPrefab()
	{
		if( this.logMessages)
		{
            Debug.Log(string.Format("SpawnPool {0} ({1}): AUTO DELETE PREFABE! " +
                                      "Waiting {2}sec to begin checking for despawns...",
                                    this.spawnPool.poolName,
                                    this.prefabName,
                                    this._option.autoDeleteDelay));			
		}
		
		yield return new WaitForSeconds(this._option.autoDeleteDelay);
		
		if( this.spawnedCount == 0 )
		{
			this.spawnPool.RemoveGamePrefabPool( this );
		}
		
		_autoDeleteActive = false;
	}
	
	
	/// <summary>
	/// 删减 _despawned 的大小
	/// </summary>
	/// <returns>
	/// The despawned.
	/// </returns>
	internal IEnumerator CullDespawned()
	{
        if (this.logMessages)
		{
            Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING TRIGGERED! " +
                                      "Waiting {2}sec to begin checking for despawns...",
                                    this.spawnPool.poolName,
                                    this.prefab.name,
                                    this._option.cullDelay));			
		}
		
        yield return new WaitForSeconds(this._option.cullDelay);
		
        while (this.totalCount > this._option.cullAbove)
        {
            // Attempt to delete an amount == this.cullMaxPerPass
            for (int i = 0; i < this._option.cullMaxPerPass; i++)
            {
                // Break if this.cullMaxPerPass would go past this.cullAbove
                if (this.totalCount <= this._option.cullAbove)
                    break;  // The while loop will stop as well independently

                // Destroy the last item in the list
                if (this._despawned.Count > 0)
                {
                    UnityEngine.Object inst = this._despawned[0];
                    
					this._despawned.RemoveAt(0);
					
                    GameObject.Destroy(inst);

                    if (this.logMessages)
					{
                        Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                "CULLING to {2} instances. Now at {3}.",
                                            this.spawnPool.poolName,
                                            this.prefab.name,
                                            this._option.cullAbove,
                                            this.totalCount));						
					}

                }
                else if (this.logMessages)
                {
                    Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                "CULLING waiting for despawn. " + 
                                                "Checking again in {2}sec",
	                                            this.spawnPool.poolName,
	                                            this.prefab.name,
	                                            this._option.cullDelay));

                    break;
                }
            }

            // Check again later
            yield return new WaitForSeconds(this._option.cullDelay);
        }
		
	    if (this.logMessages)
		{
		    Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING FINISHED! Stopping",
		                            this.spawnPool.poolName,
		                            this.prefab.name));			
		}

		this._cullingActive = false;
		
		if( this._option.autoDeletePrefab  &&
			this.spawnedCount == 0 &&
			!this._autoDeleteActive)
		{
			this._autoDeleteActive = true;
			this.spawnPool.StartCoroutine( AutoDelPrefab());
		}
	}
	
	
	
	/// <summary>
	/// 设置具体的Layer
	/// </summary>
	/// <param name='xform'>
	/// Xform.
	/// </param>
	/// <param name='layer'>
	/// Layer.
	/// </param>
	private void SetRecursively(UnityEngine.Object inst, int layer)
    {
		if( inst is GameObject )
		{
			( inst as GameObject ).layer = layer;
			Transform trans = ( inst as GameObject ).transform;
			
			int childCount = trans.childCount;
			for( int i = 0 ; i < childCount ; i ++ )
			{
				Transform child = trans.GetChild( i );
				SetRecursively( child.gameObject, layer );
			}
		}		
    }
	
	
	/// <summary>
	/// 为实例改名
	/// </summary>
	/// <param name='instance'>
	/// Instance.
	/// </param>
	private void nameInstance(UnityEngine.Object instance)
    {
        instance.name += (this.totalCount + 1).ToString("#000");
    }
}
#endregion