//#define Assetbundle_Debug_Message

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using MiniJSON;


public class AssetbundeMsgSyn : MonoBehaviour
{
    //this gam
    private GameObject cacheSceneObjNode = null;
    // Use this for initialization
    private SceneAssetsCacheController sceneAssetsCacheController = null;
	void Start () 
    {
        sceneAssetsCacheController = new SceneAssetsCacheController();
    }

	// Update is called once per frame
//	void Update () 
//    {
//        switch (_curState)
//        {
//            case AssetbundleSynState.INIT_FINISH_STATE:
//                {
//                    //if (_assetsPoolManager != null) _assetsPoolManager.Check();
//                    break;
//                }
//            case AssetbundleSynState.INIT_DOWNLOAD_ASSETS_STATE:
//                {
//                    break;
//                }
//            case AssetbundleSynState.INIT_VERSION_STATE:
//                {
//                    break;
//                }
//            default:
//                {
//                    break;
//                }
//        }
//	}

    void OnApplicationQuit()
    {
        if (_assetbundleDownLoader != null) _assetbundleDownLoader.Dispose();
		
		ALLShaderLoader.Instance.Dispose();
		
        _appQuit = false;
        this.Dispose();
    }

#if UNITY_STANDALONE
    private static string _downLoadPath = Application.persistentDataPath + "STANDALONE"+ "/"; //C:/Users/baoyu/AppData/LocalLow/Baoyugame/____________STANDALONE
#else
    private static string _downLoadPath = Application.persistentDataPath + "/"; //C:/Users/baoyu/AppData/LocalLow/Baoyugame/____________
#endif

    private string _assetsTargetPath     = String.Empty ;
	private string _assetsResPath        = String.Empty ;

    private Action<int> _showMsgFunc;

    public delegate void DataLoadFinish();
    private DataLoadFinish _initFinish;

    public delegate void DataUpdateMessageCallBack(bool isFirstDownload, int updateNum, uint totalSize, bool downLoadLocalPackage );
    private DataUpdateMessageCallBack _dataUpdateMessage;

    private bool _appQuit = true;

    private bool _isDownLoadFromPackage = false;

    //AssetPoolManager
    //private AssetsPoolManager _assetsPoolManager = null;

	#region AssetbundleSynState
    private enum AssetbundleSynState
    {
        NONE_STATE                 = 0,
        INIT_LOCAL_PACKAGER_STATE  = 1,
        INIT_VERSION_STATE         = 2,
        INIT_DOWNLOAD_ASSETS_STATE = 3,
        INIT_FINISH_STATE          = 4
    }
	
    private AssetbundleSynState _curState = AssetbundleSynState.NONE_STATE;	
    #endregion

	#region Error Contorl
    /// <summary>
    /// 错误信息回调 
    /// </summary>
    private bool  _isError = false;
    private void AddDownLoadErrorList( ErrorType errorType,  string errorString)
    {
        _isError = true;
        ErrorManager.LogError(errorType, errorString);
    }
	#endregion
	
	#region AssetConfig
    //保存资源信息
    private static AssetsConfig _assetConfig = null;
	
    /****************************************************************************************
     * 判断资源是否存在
     * **************************************************************************************/
    public static bool IsAssetExist(string objName , bool isCollectAsset = true )
    {
        if (_assetConfig == null)
        {
            return false;
        }
        else
        {
            int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
            if (index > 0)
            {
                objName = objName.Substring(index);
            }

            return _assetConfig.IsContainAsset(objName, isCollectAsset);
        }
    }	
	#endregion
	
	#region Assets Exist Message 
	private static AssetsExistMsg _assetExistMsg = null;
	public static Dictionary< string, AssetPrefab > GetMissingAssets( )
	{
		if( _assetExistMsg == null )
		{
			return null;
		}
		
		return _assetExistMsg.missingMessage.missingAssets;
	}
	
	public static int GetGameTotalAssets()
	{
		if( _assetExistMsg == null )
		{
			return -1;
		}
		
		return _assetExistMsg.resTotalNumber;
	}
	
	public static bool IsAssetMissing( string refId)
	{
		if( _assetExistMsg == null )
		{
			return true;
		}
		
		return _assetExistMsg.IsAssetsExist( refId );
	}
	
	#endregion
	
    //处理Assetbundle 的加载控件
    private AssetsLoader _assetbundleDownLoader = null;

    /// <summary>
    /// 初始化资源的信息， 判断资源版本信息
    /// </summary>
    /// <param name="assetsTargetPath"> 资源存放地址</param>
    /// <param name="dataUpdateMsgFunc"> 回调信息</param>
	public void InitAssetsData(string assetsTargetPath, string assetsResPath, bool localPackageUpdate = false, long resDataVer = 0,  DataUpdateMessageCallBack dataUpdateMsgFunc = null)
    {
        _curState = AssetbundleSynState.INIT_VERSION_STATE;

        _assetsTargetPath  = assetsTargetPath;
		_assetsResPath = assetsResPath;
        _dataUpdateMessage = dataUpdateMsgFunc;
		
		//获取本地ResVersionData
		bool isLoadLocalAssetError = false;
        if (_assetConfig == null)
        {
            _assetConfig = new AssetsConfig();

            try
            {
	            GameDebuger.Log(" GetLocalAssetbundleVersionData ");
	            string localAssetCtrlPath = _downLoadPath + AssetsLoader._localAssetVerCtrl;
	            _assetConfig.GetLocalAssetbundleVersionData(localAssetCtrlPath);					
            }
            catch (Exception e)
            {
				isLoadLocalAssetError = true;
                AddDownLoadErrorList(ErrorType.AssetIOError, "File Read Error : " + e.Message);
            }			
        }
		
		
		//初始化跟踪缺失的资源信息
		if( _assetExistMsg == null )
		{
			_assetExistMsg = new AssetsExistMsg();
		}
		
		//开始检查资源的版本信息
		if( !isLoadLocalAssetError )
		{
			_assetConfig.Reset();
			
			//如果当前的本地资源版本 比较旧， 则去更新版本资源
			if( _assetConfig.localAssetbundleVersionData.version < resDataVer )
			{
		        //分析整包的信息
		        if (localPackageUpdate)
		        {
					GameDebuger.Log ("初始化本地资源信息");
					_assetbundleDownLoader = new LocalAssetbundleDownLoader(_assetConfig, _assetExistMsg, assetsTargetPath, assetsResPath, _downLoadPath);
		        }
		
		        //分析远程版本信息
		        else
		        {
					GameDebuger.Log ("初始化远程资源信息");
					_assetbundleDownLoader = new AssetbundleDownLoader(_assetConfig, _assetExistMsg, assetsTargetPath, assetsResPath, _downLoadPath);
		        }
		
		        _assetbundleDownLoader.CheckVersion(CheckVersionFinish, AddDownLoadErrorList);					
			}
			else
			{
				GameDebuger.Log (string.Format( "本地版本高于远程版本, 本地： {0}, 远程 : {1}", 
											_assetConfig.localAssetbundleVersionData.version,
											resDataVer));
				
				//初始化缺失资源
				_assetExistMsg.InitMissingMessage( _downLoadPath );
				
				//初始化AssetRefList
				AssetPathReferenceManager.Instance.InitAssetPathRefData();
				
				//初始化mapAreaDataBytes
				//MapAreasDataController.Instance.Setup();
				
				CheckVersionFinish();
			}
		}
		else
		{
			GameDebuger.Log( "AssetbundleMsgSys < isLoadLocalAssetError > ");
		}
    }

    /// <summary>
    /// 版本检查完毕
    /// </summary>
    private void CheckVersionFinish()
    {
        //Set Current State.
        if (_assetConfig.totalU3DNumber != 0)
        {
            _curState = AssetbundleSynState.INIT_DOWNLOAD_ASSETS_STATE;
        }
        else
        {
            _curState = AssetbundleSynState.INIT_FINISH_STATE;
        }
		
#if Assetbundle_Debug_Message		
        GameDebuger.Log("FistDownLoad :" + _assetConfig.isFirstDownLoad +
                   "totalU3DNumber :" + _assetConfig.totalU3DNumber +
                   "totalUpdateAssetsSize :" + _assetConfig.totalUpdateAssetsSize);

#endif
        _isDownLoadFromPackage = GameSetting.IsUpdateLocalPackage();

        if (_assetConfig != null && _dataUpdateMessage != null)
        {
            _dataUpdateMessage(_assetConfig.isFirstDownLoad,
                               _assetConfig.totalU3DNumber,
                               _assetConfig.totalUpdateAssetsSize,
                               _isDownLoadFromPackage
                               );
        }
    }

    /// <summary>
    /// 删除所有的引用
    /// </summary>
    public void ClearReference()
    {
        _showMsgFunc       = null;
        _initFinish        = null;
        _dataUpdateMessage = null;

//        _assetbundleDownLoader = null;
    }
	
    /// <summary>
    /// 开始加载资源
    /// </summary>
    /// <param name="showMsgFunc"></param>
    /// <param name="initFinish"></param>
    public void StartDownLoad(Action<int> showMsgFunc = null, DataLoadFinish initFinish = null)
    {
        if (!( _curState == AssetbundleSynState.INIT_DOWNLOAD_ASSETS_STATE ) )
        {
            Debug.LogWarning("Assetbundles Version Date is not Initialization!!!!");
            return;
        }
		
		Debug.Log("StartDownLoad!!!!");
		
        _showMsgFunc = showMsgFunc;
        _initFinish  = initFinish;

        if (_assetbundleDownLoader != null)
        {
            try
            {
                _assetbundleDownLoader.DownLoad(__DownloadFinish,  _showMsgFunc, AddDownLoadErrorList);
            }
            catch (Exception e)
            {
                AddDownLoadErrorList(ErrorType.AssetsDownLoadError, "DownLoad Asste Error : " + e.Message);
            }
        }
    }
	
	
	#region Free Assetbundle laoder 
	/// <summary>
	/// 开始后台加载
	/// </summary>
	public void InitFreeDownloalder( string assetsTargetPath, string assetsResPath )
	{
		_assetbundleDownLoader = new FreeAssetbundleDownLoader( _assetConfig, _assetExistMsg, assetsTargetPath, assetsResPath, _downLoadPath );
		if( _assetbundleDownLoader != null )
		{
			(_assetbundleDownLoader as FreeAssetbundleDownLoader).StartBackStageLoader();
		}
	}
	
	/// <summary>
	/// 收集缺失资源 
	/// </summary>
	public void CollectMissingAsset( string assetName )
	{
		if( _assetbundleDownLoader == null || !(_assetbundleDownLoader is FreeAssetbundleDownLoader) )
		{
			GameDebuger.Log( "AssetbundeMsgSyn < Collect Missing Assets Error >" );
			return ;
		}
		
		(_assetbundleDownLoader as FreeAssetbundleDownLoader).CollectMissingAssets( assetName );
	}
	
	
	/// <summary>
	/// 现在指定的资源
	/// </summary>
	public void StartDownLoadAssetList( List< string > assetRefList ,
										 Action<int> showMsgFunc = null, DataLoadFinish initFinish = null )
	{
        if (_assetConfig == null)
		{
			AddDownLoadErrorList(ErrorType.AssetsDownLoadError, "DownLoad Asste Error , _assetConfig is null" );
			if( initFinish != null ) initFinish();
			return ;
		}
		
		if( _assetbundleDownLoader == null || !(_assetbundleDownLoader is FreeAssetbundleDownLoader) )
		{
			GameDebuger.Log( "AssetbundeMsgSyn < Start Free Assetbundle Error >" );
			return ;
		}
		
		_showMsgFunc = showMsgFunc;
        _initFinish  = initFinish;
		
//		_assetbundleDownLoader = new FreeAssetbundleDownLoader( _assetConfig, _assetExistMsg, assetsTargetPath, _downLoadPath );
		
		if( _assetbundleDownLoader != null )
		{
			( _assetbundleDownLoader as FreeAssetbundleDownLoader ).DownLoadAssets( 
																					assetRefList,
																					__DownloadFinish,  
																					_showMsgFunc, 
																					AddDownLoadErrorList
																					);
		}
	}

	public void StopDownLoadAssetList()
	{
		if( _assetbundleDownLoader != null && (_assetbundleDownLoader is FreeAssetbundleDownLoader))
		{
			( _assetbundleDownLoader as FreeAssetbundleDownLoader ).StopDownLoad();
		}
	}
			
	#endregion
	

    /// <summary>
    /// 加载完成回调
    /// </summary>
    private void __DownloadFinish()
    {
		Debug.Log("__DownloadFinish");
		
#if Assetbundle_Debug_Message		
		GameDebuger.Log( " ---------------- DownLoad Resources Finish ---------------------" );   
#endif		
        if (_initFinish != null) _initFinish();

        if (!_isDownLoadFromPackage)
        {
            _curState = AssetbundleSynState.INIT_FINISH_STATE;
        }
		
//        ClearReference();
    }

    /// <summary>
    /// 开始游戏的调用
    /// </summary>
    public void BeignGame()
    {
        _curState = AssetbundleSynState.INIT_FINISH_STATE;
		
//#if Assetbundle_Debug_Message		
//        GameDebuger.Log("Create AssetPoolManager");
//#endif		
//        _assetsPoolManager = new AssetsPoolManager();
    }

    #region AfterLoadLevelFinishBehavior
    public enum AfterLoadLevelFinishBehavior
    {
        Immediately_Init_SceneObj,
        Manualy_Init_SceneObj
    }
    #endregion


    //加载场景的调用
    private delegate AsyncOperation LoadLevelFunc(string level);
    //场景结束后的回调函数    
    public delegate void SceneLoadFinishCallBack();
    //场景加载中的回调函数
    public delegate void SceneProgressCallBack(AsyncOperation asy);

    //场景组件的数据
    #region BuildSceneData
    public class BuildSceneData
    {
        public SceneAssetsCacheController cacheController = null;
        public SceneLoadFinishCallBack    sceneLoadFinishCallBack = null;
        public SceneLoadFinishCallBack    systemSceneLoadFinishCallBack = null;
        public SceneObjectsInfoSet        sceneObjectsSet = null;
    }
    #endregion

    //场景加载后，如果 调用的行为为AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj, 
    //则会回调此函数通知调用者场景的地表已经加载完成。
    //参数AssetsHandlerFunc handler 是给回调者调用此函数用于开始初始化的操作
    public delegate void AssetsHandlerFunc(BuildSceneData data);
    public delegate void SceneObjectsHanderFunc(AssetsHandlerFunc handler, BuildSceneData data);


    /******************************************************************************************
     * 加载场景
     * level             : 场景的名称
     * progreassCallBack : 场景加载中的回调
     * sceneLoadFinishCallBack : 场景加载完成的回调
     * behavior ： 【Immediately_Init_SceneObj】 程序会立马去加载场景的额外资源， 加载完成后回调sceneLoadFinishCallBack
     *             【Manualy_Init_SceneObj】     加载完地表后， 程序会交回使用权给上层， 等上层完成所需操作， 再继续加载场景额外的资源
     * sceneObjectsHanderFunction ：　当behavior == Manualy_Init_SceneObj时才会调用
     * *****************************************************************************************/
    public void LoadLevelAsync(string level, 
                               SceneProgressCallBack progreassCallBack, 
                               SceneLoadFinishCallBack sceneLoadFinishCallBack, 
                               AfterLoadLevelFinishBehavior behavior = AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                               SceneObjectsHanderFunc sceneObjectsHanderFunction = null)
    {
        //统计场景加载时间
        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.COMMON_SCENE_TIME_TYPE);
		
		//清空缓存池的信息
		ResourcePoolManager.Instance.DespawnChangeScene();
		
        StartCoroutine(_LoadLevel(level, 
                                  DealWithCommonScene, 
                                  _LoadLevelAsync,
                                  _LoadLevelFinish,
                                  sceneAssetsCacheController, 
                                  progreassCallBack, 
                                  sceneLoadFinishCallBack, 
                                  behavior,
                                  sceneObjectsHanderFunction));
    }


    private AsyncOperation _LoadLevelAsync(string level)
    {
#if Assetbundle_Debug_Message		
		GameDebuger.Log("-------------------Begin Load Level -----------------------" );
#endif

        return Application.LoadLevelAsync(level);
    }

    private void _LoadLevelFinish()
    {
        GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.COMMON_SCENE_TIME_TYPE);
		
#if Assetbundle_Debug_Message		
        GameDebuger.Log(" ---------------------Resources.UnloadUnusedAssets -----------------------");
#endif
		
#if UNITY_EDITOR
		if( GameSetting.IsLoadLocalAssets())
		{		
			GameObject skybox = GameObject.Find("Skybox");
			GameObject layer  = GameObject.Find("SceneLayer");
			if( skybox != null && layer != null )
			{
				skybox.transform.parent = layer.transform;
			}
		}
#endif
		
        Resources.UnloadUnusedAssets();
        //GC.Collect();
    }

    /// <summary>
    /// Add level to current level
    /// </summary>
    /// <param name="level"></param>
    /// <param name="progreassCallBack"></param>
    /// <param name="sceneLoadFinishCallBack"></param>
    public void LoadLevelAdditiveAsync(string level, 
                                       SceneProgressCallBack progreassCallBack, 
                                       SceneLoadFinishCallBack sceneLoadFinishCallBack, 
                                       AfterLoadLevelFinishBehavior behavior = AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                                       SceneObjectsHanderFunc sceneObjectsHanderFunction = null)
    {

        //统计战斗场景加载时间
        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.BATTLE_SCENE_TIME_TYPE);

        StartCoroutine(_LoadLevel(level, 
                                  DealWithBattleScene, 
                                  _LoadLevelAdditiveAsync,
                                  _LoadLevelAdditiveFinish,
                                  sceneAssetsCacheController, 
                                  progreassCallBack, 
                                  sceneLoadFinishCallBack, 
                                  behavior,
                                  sceneObjectsHanderFunction,
                                  false,
                                  false));
    }

    private AsyncOperation _LoadLevelAdditiveAsync(string level)
    {
#if Assetbundle_Debug_Message
		GameDebuger.Log( " --------------------------Begin Load Level Additive -------------------" );
#endif		
		
        return Application.LoadLevelAdditiveAsync(level);
    }

    private void _LoadLevelAdditiveFinish()
    {
        GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.BATTLE_SCENE_TIME_TYPE);
		
#if Assetbundle_Debug_Message		
        GameDebuger.Log("---------------------Resources.UnloadUnusedAssets -----------------------");
#endif

#if UNITY_EDITOR
		
		if( GameSetting.IsLoadLocalAssets())
		{
			GameObject skybox = GameObject.Find("Skybox");
			GameObject layer  = GameObject.Find("BattleStage");
			if( skybox != null && layer != null)
			{
				skybox.transform.parent = layer.transform;
			}
		}
#endif		
		
        Resources.UnloadUnusedAssets();
        //GC.Collect();
    }

    /// <summary>
    /// Load Null Level
    /// </summary>
    /// <returns></returns>
    private AsyncOperation _LoadNullLevel()
    {
        return Application.LoadLevelAsync("NullScene");
    }
 
    /// <summary>
    /// load one level
    /// </summary>
    /// <param name="level"></param>
    /// <param name="loadLevelFunc"></param>
    /// <param name="progreassCallBack"></param>
    /// <param name="sceneLoadFinishCallBack"></param>
    /// <returns></returns>
    private float nullLevelStayTime = 0.5f;
    private const string TYPE_INFO = "_sceneMsg";
    private IEnumerator _LoadLevel(string level, 
                                   AssetsHandlerFunc handleFunc, LoadLevelFunc loadLevelFunc, SceneLoadFinishCallBack sysLoadFinishClassBack,
                                   SceneAssetsCacheController cacheController,
                                   SceneProgressCallBack progreassCallBack, SceneLoadFinishCallBack sceneLoadFinishCallBack, 
                                   AfterLoadLevelFinishBehavior behavior = AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                                   SceneObjectsHanderFunc sceneObjectsHandlerFunc = null,
                                   bool isCache = true, 
                                   bool isLoadNullScene = true)
    {
		//AppUtils.SetLightMap( null );
		
        //判断是否加载内部的场景， 还是加载Assetbundle 场景
        bool loadInternalScene = GameSetting.IsLoadLocalAssets()   ||
                                 (_assetConfig == null) ||
                                 (_assetConfig != null && !_assetConfig.IsContainScene(level));


        SceneObjectsInfoSet sceneObjectsSet = null;
        //如果是加载内部的场景。 则跳过资源缓存的步骤
        if (!loadInternalScene )
        {
                //1、 获取场景物件配置数据
                //AssetBundleCreateRequest request = _GetAssetRequest( SystemHelper.RESOURCES_PATH + SystemHelper.SCENE_DATA_PATH + level + TYPE_INFO );
                //if( request != null )
                //{
                    //yield return request;

                    //if (request.isDone)
                    //{
                       // AssetBundle bundle = request.assetBundle;
                        //TextAsset text = bundle.mainAsset as TextAsset;
                        
                        //bundle.Unload(false);
                        //bundle = null;

                        //request = null;

						AssetBase assetBase = _LoadAssetbundle(PathHelper.RESOURCES_PATH + PathHelper.SCENE_DATA_PATH + level + TYPE_INFO);
                        if (assetBase != null)
                        {
                            TextAsset text = assetBase.asset as TextAsset;
                            if (text != null)
                            {
                                Dictionary<string, object> obj = DataHelper.GetJsonFile(text.bytes, false);
                                JsonSceneObjectsInfoSetParser parser = new JsonSceneObjectsInfoSetParser();
                                sceneObjectsSet = parser.DeserializeJson_SceneObjectsInfoSet(obj);
                                parser = null;
                            }
                            else
                            {
                                Debug.LogError("JsonSceneObjectsInfoSetParser is null ");
                            } 
                        }
                   // }
                //}

                if (sceneObjectsSet != null && cacheController != null && isCache)
                {

                    cacheController.CleanStaticBatchingCache();

                    //2、进行重复资源的缓冲使用
                    cacheController.CacheSceneObject(sceneObjectsSet);
                }
        }

        //加载空的资源场景， 以清空上一个场景数据
//        if (isLoadNullScene && !loadInternalScene && !WorldManager.KeepSceneMode)
//        {
//            //Load Null Scene
//            AsyncOperation nullLevelOpreation = _LoadNullLevel();
//            yield return nullLevelOpreation;
//
//            GameDebuger.Log("New Null Scene");
//            yield return new WaitForSeconds(nullLevelStayTime); 
//        }

		
		bool initSceneError = false;
        //如果不存在把Scene或者没有打包为Assetbundle
        if (loadInternalScene )
        {
            AsyncOperation opreation = loadLevelFunc(level);
            if (progreassCallBack != null) progreassCallBack(opreation);
            yield return opreation;

            GameDebuger.Log("Load Scene : " + level.ToString() + " - Finish");
			
            yield return new WaitForEndOfFrame();
			
            if (sysLoadFinishClassBack != null) sysLoadFinishClassBack();
            if (sceneLoadFinishCallBack != null) sceneLoadFinishCallBack();
        }
        else
        {
            AssetBundle bundle = null;
            string scenePath = _assetConfig.GetScensPath(level);
            GameDebuger.Log("ScenePath : " + scenePath);
            //AssetBundleCreateRequest request = _GetAssetRequest(scenePath);
            WWW request = _GetAssetRequestWWW(scenePath);

            if (request != null)
            {
                yield return request;

                if (request.isDone && request.error == null )
                {
                    bundle = request.assetBundle;
                    request.Dispose();
                    request = null;
                }
                else
                {
					initSceneError = true;
					initSceneError = true;
                    GameDebuger.Log(" Assetbundle request error : " + scenePath + " requestError" );
                }
            }
            else
            {
//                initSceneError = true;
//				Debug.LogError("Can not find scene of : " + level + " message ");
				if( scenePath.IndexOf( "GameResources/ArtResources/Scenes_All/BattleScene_" ) != -1 )
				{
					scenePath = "GameResources/ArtResources/Scenes_All/BattleScene_2001";
					level = "BattleScene_2001";
					request = _GetAssetRequestWWW( scenePath );
					if( request != null )
					{
						yield return request;
						if (request.isDone && request.error == null )
						{
							bundle = request.assetBundle;
							request.Dispose();
							request = null;
						}
						else
						{
							initSceneError = true;
							initSceneError = true;
							//GameDebuger.Log(" Assetbundle request error : " + scenePath + " requestError" );
						}
					}
					else
					{
						initSceneError = true;
						//Debug.LogError("Can not find scene of : " + level + " message ");
					}

				}
				if( scenePath.IndexOf( "GameResources/ArtResources/Scenes_All/BattleScene_" ) != -1 )
				{
					level = "BattleScene_2001";
					_LoadLevel( level , handleFunc ,loadLevelFunc , sysLoadFinishClassBack , cacheController ,progreassCallBack ,sceneLoadFinishCallBack ,behavior ,sceneObjectsHandlerFunc );
				}
               
            }

			if ( !initSceneError )
			{
	            AsyncOperation opreation = loadLevelFunc(level);
	            if (progreassCallBack != null) progreassCallBack(opreation);
	            yield return opreation;
	
                bundle.Unload(false);
	            GameDebuger.Log("Load Scene : " + level.ToString() + " - Finish");
			}
        }
		
		if ( !loadInternalScene && !initSceneError )
		{
//            StartCoroutine(DealWithCommonSceneOfColliderObj(sceneObjectsSet, cacheController, delegate() 
//            {
//                //进行回调操作
//                BuildSceneData data = new BuildSceneData();
//                data.cacheController = cacheController;
//                data.sceneLoadFinishCallBack = sceneLoadFinishCallBack;
//                data.sceneObjectsSet = sceneObjectsSet;
//                data.systemSceneLoadFinishCallBack = sysLoadFinishClassBack;
//
//                //立即初始化场景信息
//                if (behavior == AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj)
//                {
//                    if (handleFunc != null) handleFunc(data);
//                }
//
//                //等待游戏必须资源加载完毕后再开启初始化
//                else if (behavior == AfterLoadLevelFinishBehavior.Manualy_Init_SceneObj)
//                {
//                    if (sceneObjectsHandlerFunc != null && handleFunc != null)
//                        sceneObjectsHandlerFunc(handleFunc, data);
//                }	            
//            }));

			//进行回调操作
			BuildSceneData data = new BuildSceneData();
			data.cacheController = cacheController;
			data.sceneLoadFinishCallBack = sceneLoadFinishCallBack;
			data.sceneObjectsSet = sceneObjectsSet;
			data.systemSceneLoadFinishCallBack = sysLoadFinishClassBack;
			
			//立即初始化场景信息
			if (behavior == AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj)
			{
				if (handleFunc != null) handleFunc(data);
			}
			
			//等待游戏必须资源加载完毕后再开启初始化
			else if (behavior == AfterLoadLevelFinishBehavior.Manualy_Init_SceneObj)
			{
				if (sceneObjectsHandlerFunc != null && handleFunc != null)
					sceneObjectsHandlerFunc(handleFunc, data);
			}
		}
    }

    //处理普通场景资源的组件
    private void DealWithCommonScene(BuildSceneData data)
    {
        GameObject sceneLayer = GameObject.Find("WorldStage");
        if (sceneLayer == null)
        {
            sceneLayer = GameObject.Find("BattleStage");
        }

        if (sceneLayer != null)
        {
            Transform sceneNode = sceneLayer.transform.GetChild(0);
            if (sceneNode != null)
            {
				if (data.sceneLoadFinishCallBack != null) data.sceneLoadFinishCallBack();
				if (data.systemSceneLoadFinishCallBack != null) data.systemSceneLoadFinishCallBack();

                //StartCoroutine(_SceneBuidler(data, sceneNode));
            }
        }
    }


    /// <summary>
    /// 预先加载一些存在碰撞体的场景物体
    /// </summary>
    /// <param name="set"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
//    private IEnumerator DealWithCommonSceneOfColliderObj(SceneObjectsInfoSet set, SceneAssetsCacheController controller, Action callBackFunction )
//    {
//        GameObject sceneLayer = GameObject.Find("WorldStage");
//
//        if (sceneLayer != null)
//        {
//            //Utility.SetPetShader2(sceneLayer);
//
//            Transform sceneNode = sceneLayer.transform.GetChild(0);
//            if (sceneNode != null)
//            {
//                if (set != null && controller != null)
//                {
//                    //List<MeshRenderer> childMeshRendererList = new List<MeshRenderer>();
//                    List<GameObject> childObjectList = new List<GameObject>();
//                    foreach (KeyValuePair<string, SceneNode> item in set.sceneNodeDic)
//                    {
//                        SceneNode nodeMessage = item.Value;
//                        if (nodeMessage.colliderObjectDataDic == null) continue;
//
//                        string nodeName = item.Key;
//                        Transform childNode = sceneNode.FindChild(nodeName);
//                        if (childNode != null)
//                        {
//                            foreach (KeyValuePair<string, SceneObjectData> nodeItem in nodeMessage.colliderObjectDataDic)
//                            {
//                                SceneObjectData sceneObjectData = nodeItem.Value;
//
//                                //获取 Asset 资源
//                                UnityEngine.Object go = null;
//                                go = controller.GetCacheGameObject(sceneObjectData.objectName);
//                                
//                                if( go == null )
//                                {
//                                    //AssetBundleCreateRequest request = _GetAssetRequest(sceneObjectData.objectPathName);
//                                    WWW request = _GetAssetRequestWWW(sceneObjectData.objectPathName);
//									
//									if( request != null )
//									{
//	                                    yield return request;
//	
//	                                    if (request.isDone && request.error == null)
//	                                    {
//	                                        AssetBundle assetbundle = request.assetBundle;
//	                                        request.Dispose();
//	                                        request = null;
//	
//	                                        go = assetbundle.mainAsset;
//	                                        assetbundle.Unload(false);
//	
//	                                        controller.AddCacheObjectReference(sceneObjectData.objectName, go);
//	                                    }										
//									}
//                                }
//								
//                                if (go != null)
//                                {
//                                    _BuildObject(childNode, go, childObjectList, sceneObjectData);
//                                }
//                            }
//                            //StaticBatchingUtility.Combine(childObjectList.ToArray(), childNode.gameObject);
//                            //childObjectList.Clear();
//                        }
//                    }
//                }
//            }
//
//        }
//
//        if (callBackFunction != null) callBackFunction();
//    }

	
    /// <summary>
    /// 处理战斗场景的组件
    /// </summary>
    /// <param name="data"></param>
    private void DealWithBattleScene(BuildSceneData data)
    {
        GameObject battleLayer = GameObject.Find("BattleStage");
        if (battleLayer != null)
        {
            //Utility.SetPetShader2(battleLayer);

            Transform sceneNode = battleLayer.transform.GetChild(0);
            if (sceneNode != null)
            {
				if (data.sceneLoadFinishCallBack != null) data.sceneLoadFinishCallBack();
				if (data.systemSceneLoadFinishCallBack != null) data.systemSceneLoadFinishCallBack();

                //StartCoroutine(_SceneBuidler(data, sceneNode, false, false));
            }
        }
    }	
	

    /// <summary>
    /// 负责组件场景物品
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scenenNode"></param>
    /// <returns></returns>
//    private IEnumerator _SceneBuidler(BuildSceneData data, Transform scenenNode, bool isGetFormCache = true, bool isAddToCacheController = true)
//    {
//        if (data != null && scenenNode != null)
//        {
//            SceneObjectsInfoSet         set        = data.sceneObjectsSet;
//            SceneAssetsCacheController  controller = data.cacheController;
//
//            if (set != null && controller != null)
//            {
//				//Set Skybox Infomation
////				AppUtils.SetSkyboxBasicInfo( set.skyboxData );
//				
//                //List<MeshRenderer> childMeshRendererList = new List<MeshRenderer>();
//                List<GameObject> childObjectList = new List<GameObject>();
//                foreach (KeyValuePair<string, SceneNode> item in set.sceneNodeDic)
//                {
//                    string nodeName = item.Key;
//                    Transform childNode = scenenNode.FindChild(nodeName);
//                    if (childNode != null)
//                    {
//                        SceneNode nodeMessage = item.Value;
//                        foreach (KeyValuePair<string, SceneObjectData> nodeItem in nodeMessage.sceneObjectDataDic)
//                        {
//                            SceneObjectData sceneObjectData = nodeItem.Value;
//
//                            //获取 Asset 资源
//                            UnityEngine.Object go = null;
//                            go = controller.GetCacheGameObject(sceneObjectData.objectName);
//
//                            if (go == null)
//                            {
//                                //AssetBundleCreateRequest request = _GetAssetRequest(sceneObjectData.objectPathName);
//                                WWW request = _GetAssetRequestWWW(sceneObjectData.objectPathName);
//								
//								if( request != null )
//								{
//	                                yield return request;
//	
//	                                if (request.isDone && request.error == null)
//	                                {
//	                                    AssetBundle assetbundle = request.assetBundle;
//	                                    request.Dispose();
//	                                    request = null;
//	
//	                                    go = assetbundle.mainAsset;
//	                                    assetbundle.Unload(false);
//	
//	                                    controller.AddCacheObjectReference(sceneObjectData.objectName, go);
//	                                }
//	                                else
//	                                {
//	                                    Debug.LogError("AssetBundleCreateRequest request ");
//	                                }									
//								}
//                            }
//
//                            if (go != null)
//                            {
//                                _BuildObject(childNode, go, childObjectList, sceneObjectData);
//                            }
//                            yield return new WaitForEndOfFrame();
//                        }
//                        //StaticBatchingUtility.Combine(childObjectList.ToArray(), childNode.gameObject);
//                        //childObjectList.Clear();
//                    }
//                }
//                //controller.StaticBatchingCombine(scenenNode);
//            }
//            //Utility.SetPetShader2(scenenNode.gameObject);
//
//
//
//            if (data.sceneLoadFinishCallBack != null) data.sceneLoadFinishCallBack();
//            if (data.systemSceneLoadFinishCallBack != null) data.systemSceneLoadFinishCallBack();
//
//            data.sceneLoadFinishCallBack = null;
//            data.sceneObjectsSet         = null;
//            data.cacheController         = null;
//        }
//
//
//    }

    private void _BuildObject(Transform childNode, UnityEngine.Object asset, List<GameObject> childObjectList, SceneObjectData sceneObjectData)
    {
        //如果asset == null 
        if (asset == null) return;

        for (int i = 0; i < sceneObjectData.objectTransFormDataList.Count; i++)
        {
            SceneObjectTransFormData transFormData = sceneObjectData.objectTransFormDataList[i];

            GameObject go = GameObject.Instantiate( asset ) as GameObject;

            //初始化物品的信息
            Transform goTransform = go.transform;
            goTransform.parent = childNode;
            goTransform.localPosition = new Vector3((float)transFormData.positionX, (float)transFormData.positionY, (float)transFormData.positionZ);
            goTransform.localEulerAngles = new Vector3((float)transFormData.rotationX, (float)transFormData.rotationY, (float)transFormData.rotationZ);
            goTransform.localScale = new Vector3((float)transFormData.scaleX, (float)transFormData.scaleY, (float)transFormData.scaleZ);

			if( transFormData.lightmapInfo != null )
			{
				Renderer goRender = go.renderer;
				if( goRender != null )
				{
					SceneObjectLightmapInfo lightmapInfo = transFormData.lightmapInfo;
					
					goRender.lightmapIndex 		  = lightmapInfo.lightmapIndex;
					goRender.lightmapTilingOffset = new Vector4( (float)lightmapInfo.x, (float)lightmapInfo.y, (float)lightmapInfo.z, (float)lightmapInfo.w );
				}
			}
			
			
            //如果存在子节点的TransForm变动。 则改变
            int childCount = goTransform.childCount;
			
            for (int j = 0; j < childCount; j++)
            {
                Transform childTransForm = goTransform.GetChild(j);
				
                if (transFormData.subObjects.ContainsKey(childTransForm.name))
                {
                    SubObject subData = transFormData.subObjects[childTransForm.name];
					
					if( subData.lightMapInfo != null )
					{
						Renderer childRender = childTransForm.gameObject.renderer;
						SceneObjectLightmapInfo lightmapInfo = subData.lightMapInfo;
						
						childRender.lightmapIndex = lightmapInfo.lightmapIndex;
						childRender.lightmapTilingOffset = new Vector4( (float)lightmapInfo.x, (float)lightmapInfo.y, (float)lightmapInfo.z, (float)lightmapInfo.w );
					}
					
					if( subData.transformInfo != null )
					{
	                    childTransForm.localPosition     = new Vector3((float)subData.transformInfo.positionX, (float)subData.transformInfo.positionY, (float)subData.transformInfo.positionZ);
	                    childTransForm.localEulerAngles  = new Vector3((float)subData.transformInfo.rotationX, (float)subData.transformInfo.rotationY, (float)subData.transformInfo.rotationZ);
	                    childTransForm.localScale        = new Vector3((float)subData.transformInfo.scaleX,    (float)subData.transformInfo.scaleY,    (float)subData.transformInfo.scaleZ);
					}
					
                }
                else
                {
                    childTransForm.localPosition = new Vector3();
                    childTransForm.localEulerAngles = new Vector3();
                    childTransForm.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }


                MeshRenderer meshRenderer = childTransForm.GetComponent<MeshRenderer>();
                if (meshRenderer != null && meshRenderer.sharedMaterial != null)
                {
                    if (
                         Mathf.Abs(goTransform.localScale.x - goTransform.localScale.y) < 0.01 &&
                         Mathf.Abs(goTransform.localScale.y - goTransform.localScale.z) < 0.01 &&
                         Mathf.Abs(childTransForm.localScale.x - childTransForm.localScale.y) < 0.01 &&
                         Mathf.Abs(childTransForm.localScale.y - childTransForm.localScale.z) < 0.01
                        )
                    {
                        childObjectList.Add(childTransForm.gameObject);
                    }
                }
            }
            
        }
        StaticBatchingUtility.Combine(childObjectList.ToArray(), childNode.gameObject);
        childObjectList.Clear();
    }

    /// <summary>
    /// 获取Atlas
    /// </summary>
    /// <param name="atlasName"></param>
    /// <returns></returns>
    public AssetBase LoadAtlas(string atlasName)
    {
        //AssetBase assetBase = null;
        //assetBase = _LoadAssetbundle(atlasName);
        //if (assetBase != null)
        //{
        //    assetBase.AddReferenceIndex();
        //}

        //return assetBase;
        return null;
    }

    /// <summary>
    /// 删除Atlas的引用
    /// </summary>
    /// <param name="atlasName"></param>
    public void UnLoadAtlas(string atlasName)
    {
        //if (_assetsPoolManager != null && _assetsPoolManager.IsU3DExist(atlasName))
        //{
        //    AssetU3D assetU3D = (_assetsPoolManager.GetU3DData(atlasName) as AssetU3D);
        //    AssetBase assetBase = assetU3D.asset;
        //    assetBase.RemoveReferenceIndex();

        //    if (assetBase.IsNoneReference())
        //    {
        //        _assetsPoolManager.Delete(atlasName);
        //        assetU3D = null;
        //        assetBase = null;

        //        Resources.UnloadUnusedAssets();
        //    }
        //}
    }

    // ============================ 资源加载操作 ==============================================

    private const string unity3dEx = ".unity3d";

    /***************************************************************************************
     * 字典用于保存异步获取资源的回调信息， 如果同时请求相同的资源，会保持相同的回调函数。
     * *************************************************************************************/
    private Dictionary<string, List< Action<AssetBase> >> _asyncAssetCallBackDic = null;

    /****************************************************************************************
     * 判断资源是否存在
     * **************************************************************************************/
//    public bool IsAssetExist(string objName)
//    {
//        if (_assetConfig == null)
//        {
//            return false;
//        }
//        else
//        {
//            int index = objName.IndexOf(SystemHelper.RESOURCES_PATH);
//            if (index > 0)
//            {
//                objName = objName.Substring(index);
//            }
//
//            return _assetConfig.IsContainAsset(objName);
//        }
//    }


    /**************************************************************************************
     * 返回当前资源的版本号
     * ************************************************************************************/
    public long GetAssetsVersion(string objName, bool isSceneAsset = false)
    {
        if (isSceneAsset)
        {
            objName = _assetConfig.GetScensPath(objName);
        }

        LocalAssetPrefab localAssetPrefab = _assetConfig.GetAsset(objName);
        if (localAssetPrefab != null)
        {
            return localAssetPrefab.version;
        }

        return 0;
    }

    /***************************************************************************************
     * 搜索指定目录下的资源
     * *************************************************************************************/
    public List<string> SearchAssets(string rootPath)
    {
        List<string> result = new List<string>();
        foreach (KeyValuePair<string, LocalAssetPrefab> item in _assetConfig.localAssetbundleVersionData.localAssetDic)
        {
            if (item.Key.StartsWith(rootPath))
            {
                result.Add(item.Key);
            }
        }
        return result;
    }

    /****************************************************************************************
     * 函数LoadObjectListAsync 是异步获取资源数据的函数
     * 回调函数getter : Dictionary<string, AssetBase> - 返回加载后的资源数据
     *                 List<string>                  -  返回不存在的资源数据
     ****************************************************************************************/
    public void LoadObjectListAsync(List<string> objNameList, Action<Dictionary<string, AssetBase>, List<string>> getter, Action< int > progresser)
    {
        StartCoroutine(_LoadAssetsListAsync(objNameList, getter, progresser));
    }

    private IEnumerator _LoadAssetsListAsync(List<string> objNameList, Action<Dictionary<string, AssetBase>, List<string>> getter, Action<int> progresser )
    {
        AssetBundle bundle = null;
        Dictionary<string, AssetBase> assetBaseDic = new Dictionary<string, AssetBase>();
        List<string> donotExitAssets = new List<string>();

        for (int i = 0; i < objNameList.Count; i ++)
        {
            string name    = objNameList[i];
            string objName =  name;

			int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
            if (index == -1)
            {
				objName = PathHelper.RESOURCES_PATH + objName;
            }
            else if (index > 0)
            {
                objName = objName.Substring(index);
            }

            //AssetBundleCreateRequest request = _GetAssetRequest(objName);
            WWW request = _GetAssetRequestWWW(objName);

            if (request != null)
            {
                yield return request;

                if (request.isDone && request.error == null)
                {
                    bundle = request.assetBundle;
                    request.Dispose();
                    request = null;
                }
            }

            //生成AssetBase
            if (bundle != null)
            {
                AssetBase assetBase = new AssetBase(bundle.mainAsset);
                assetBaseDic.Add(name, assetBase);
                bundle.Unload(false);
            }
            else
            {
                donotExitAssets.Add(name);
            }

            if (progresser != null) progresser((i * 100) / objNameList.Count);
        }

        if (getter != null) getter(assetBaseDic, donotExitAssets);
    }


    /****************************************************************************************
     * 函数 LoadObjectAsync 是异步获取资源的函数， 主要是对于某些获取已经压缩的Assetbundle进行操作
     * 如果资源是未压缩资源， 则会直接调用LoadObject来获取资源
     * **************************************************************************************/
    public void LoadObjectAsync(string objName, Action<AssetBase> callBackFun)
    {
        StartCoroutine(_LoadAsync(objName, callBackFun));
    }

    private IEnumerator _LoadAsync(string objName, Action<AssetBase> callBackFun)
    {
        AssetBase assetBase = null;
        AssetBundle bundle = null;

        //如果AssetbundleDownLoader没有初始化， 则直接返回NULL
        if (_assetConfig == null)
        {
            GameDebuger.Log("_assetConfig is null ");

            if (callBackFun != null)
            {
                callBackFun(null);
            }
        }
        else
        {
            if( callBackFun != null )
            {
                //保存回调信息
                if (_asyncAssetCallBackDic == null)
                {
                    _asyncAssetCallBackDic = new Dictionary<string,List<Action<AssetBase>>>();
                }

                if (_asyncAssetCallBackDic.ContainsKey(objName))
                {
                    _asyncAssetCallBackDic[objName].Add(callBackFun);
                }
                else
                {
                    LocalAssetPrefab localPrefab = _GetLocalAssetPrefab( objName);
                    if (localPrefab != null)
                    {
                        //如果是本地， 且没有压缩的， 直接调用同步接口返回
                        if (localPrefab.isLocalPack && !localPrefab.needDecompress)
                        {
                            if (callBackFun != null) callBackFun(LoadObject(objName));
                        }
                        else
                        {
                            _asyncAssetCallBackDic.Add(objName, new List<Action<AssetBase>>());
                            _asyncAssetCallBackDic[objName].Add(callBackFun);

                            //AssetBundleCreateRequest request = _GetAssetRequest(objName);
                            WWW request = _GetAssetRequestWWW(objName);
                            if (request != null)
                            {
                                yield return request;

                                if (request.isDone && request.error == null)
                                {
                                    bundle = request.assetBundle;
                                    request.Dispose();
                                    request = null;
                                }
                                else
                                {
                                    GameDebuger.Log(" request load error :  " + objName);
                                }
                            }
                            else
                            {
                                GameDebuger.Log("_LoadAsync : Get AssetbundleCreateRequest is NULL :  " + objName);
                            }

                            //生成AssetBase
                            if (bundle != null)
                            {
                                assetBase = new AssetBase(bundle.mainAsset);
                                bundle.Unload(false);
                            }

                            //回调信息
                            if (_asyncAssetCallBackDic.ContainsKey(objName))
                            {
                                List<Action<AssetBase>> callBackList = _asyncAssetCallBackDic[objName];
                                for (int i = 0; i < callBackList.Count; i++)
                                {
                                    callBackList[i](assetBase);
                                }

                                _asyncAssetCallBackDic.Remove(objName);
                            }
                            else
                            {
                                if (callBackFun != null) { callBackFun(null); }                                
                            }
                        }
                    }
                    else
                    {
                        if (callBackFun != null) { callBackFun(null);}
                    }
                }
            }
        }
    }

    private LocalAssetPrefab _GetLocalAssetPrefab( string objName)
    {
        if (_assetConfig == null)
        {
            return null;
        }

        //检测地址是否合服规格
		int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
        if (index > 0)
        {
            objName = objName.Substring(index);
        }

        if (_assetConfig.IsContainAsset(objName))
        {
            return _assetConfig.GetAsset(objName);
        }
        else
        {
            GameDebuger.Log(" Can not find LocalAssetPrefab " + objName); 
            return null;
        }
    }

    /*******************************************************************************************
     * 获取异步加载的AssetBundleCreateRequest
     * *****************************************************************************************/
    private AssetBundleCreateRequest _GetAssetRequest( string objName )
    {
        if (_assetConfig == null)
        {
            return null;
        }

        //检测地址是否合服规格
        int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
        if (index > 0)
        {
            objName = objName.Substring(index);
        }

        if (_assetConfig.IsContainAsset(objName))
        {
            LocalAssetPrefab assetPrefab = _assetConfig.GetAsset(objName);

            byte[] assetBuffer = null;
            string assetPath;
			

#if UNITY_EDITOR
				//ExperiencePathManager.AddResPath( objName );
#endif			
			
			
//#if UNITY_IPHONE
//	            //如果是整包，则读取整包里面的内容
//	           	if (assetPrefab.isLocalPack)
//	            {
//	                assetPath  =  Application.streamingAssetsPath + "/" + objName + unity3dEx;
//	                assetBuffer = GetAssetData(assetPath);
//	           }
//	            else
//	            {
//	                //assetPath = "file:///" + _downLoadPath + objName + unity3dEx;// assetPrefab.path;
//	                assetPath = _downLoadPath + objName + unity3dEx;
//	                assetBuffer = GetAssetData(assetPath);
//	            }
//			
//#else
			assetPath = _downLoadPath + objName + unity3dEx;
			assetBuffer = GetAssetData(assetPath);
//#endif

            //异步调用获取Assetbundle
            if (assetBuffer != null)
            {
                AssetBundleCreateRequest request = AssetBundle.CreateFromMemory(assetBuffer);
                return request;
            }
        }
        else
        {
            GameDebuger.Log(" Can not find Assetbundle of " + objName);
        }

        return null;
    }

    private WWW _GetAssetRequestWWW(string objName)
    {
        if (_assetConfig == null)
        {
            return null;
        }

        //检测地址是否合服规格
        int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
        if (index > 0)
        {
            objName = objName.Substring(index);
        }

        if (_assetConfig.IsContainAsset(objName))
        {
            LocalAssetPrefab assetPrefab = _assetConfig.GetAsset(objName);

            byte[] assetBuffer = null;
            string assetPath;
			
#if UNITY_EDITOR
				//ExperiencePathManager.AddResPath( objName );
#endif			
			
			//#if UNITY_IPHONE
			//			if( assetPrefab.isLocalPack )
			//			{
			//				assetPath = "file:///" + Application.streamingAssetsPath + "/" + objName +  unity3dEx;	
			//			}
			//			else
			//			{
			//				assetPath = "file:///" + _downLoadPath + objName + unity3dEx;// assetPrefab.path;						
			//			}
			//#else
			assetPath = "file:///" + _downLoadPath + objName + unity3dEx;// assetPrefab.path;			
			//#endif
            WWW request = new WWW(assetPath);
            return request;
        }
        else
        {
            GameDebuger.Log(" Can not find Assetbundle of " + objName);
        }

        return null;
    }

    /****************************************************************************************
     * 函数 LoadObject 是同步获取资源的函数， 主要是对于某些没有进行压缩的Assetbundle进行操作。
     * 如果资源已进入打包出来之后，就无法调用此函数。调用的时候请注意
     * **************************************************************************************/
    public AssetBase LoadObject(string objName)
    {
        AssetBase assetBase = _LoadAssetbundle(objName);
        return assetBase;
    }

    private AssetBundle _synchronousBundle = null;
    private string _synchronousAssetName   = string.Empty;
    private AssetBase _LoadAssetbundle(string objName)
    {
        if (_assetConfig == null)
        {
            GameDebuger.Log("_assetConfig is null ");
            return null;
        }

        //检测地址是否合服规格
        int index = objName.IndexOf(PathHelper.RESOURCES_PATH);
        if (index > 0)
        {
            objName = objName.Substring(index);
        }

        if (_assetConfig.IsContainAsset(objName))
        {
            LocalAssetPrefab assetPrefab = _assetConfig.GetAsset(objName);
            
            //如果是非压缩的AssetBundle才可以用到这个函数
            if (!assetPrefab.needDecompress)
            {
                string assetPath;

                if (_synchronousAssetName == objName && _synchronousBundle != null)
                {
                    AssetBase assetBase = new AssetBase(_synchronousBundle.mainAsset);
                    return assetBase;
                }
                else
                {
                    if (_synchronousBundle != null)
                    {
                        _synchronousBundle.Unload(false);
                        _synchronousBundle = null;
                    }
                }
				
#if UNITY_EDITOR
				//ExperiencePathManager.AddResPath( objName );
#endif
				
//#if UNITY_IPHONE
//                //如果是整包，则读取整包里面的内容
//                if (assetPrefab.isLocalPack)
//                {
//                    //if (Application.platform == RuntimePlatform.Android)
//                    //{
//                    //    objName = SystemHelper.LOCALASSETPATH + objName; 
//                    //   AssetBase assetBase = new AssetBase(Resources.Load(objName));
//                    //    return assetBase;
//                    //}
//                    //else
//                    //{
//                       assetPath = Application.streamingAssetsPath + "/" +  objName + unity3dEx; 
//                    //}
//					
//					if( File.Exists( assetPath ))
//					{
//						_synchronousBundle = AssetBundle.CreateFromFile(assetPath);
//					}
//					else
//					{
//						GameDebuger.Log("Can not find File of " +  assetPath );
//					}
//				}
//                else
//                {
//#endif
					
					
                    assetPath = _downLoadPath + objName + unity3dEx;// assetPrefab.path;
                    if (File.Exists(assetPath))
                    {
                        _synchronousBundle = AssetBundle.CreateFromFile(assetPath);
                    }
                    else
                    {
                        GameDebuger.Log(" Can not find File of " + assetPath);
                    }
					
					
//#if UNITY_IPHONE
//                }
//#endif				
				
	

				
                 if (_synchronousBundle != null)
                {
                    UnityEngine.Object obj = _synchronousBundle.mainAsset;
                    if (obj == null)
                    {
                        GameDebuger.Log("Assetbundel Object is Null : " + assetPath);
                    }
                    _synchronousAssetName = objName;
                    AssetBase assetBase = new AssetBase(obj);
                    return assetBase;
                }
                else
                {
                    GameDebuger.Log( "Create Assetbundle Error : " + assetPath );
                }
            }
            else
            {
                GameDebuger.Log("== Obj Paht : " + objName + " is compress assetbundle ==");
            }
        }
        else
        {
            GameDebuger.Log(" Can not find Assetbundle of " + objName);
        }
        return null;
    }

    /// <summary>
    /// 获取资源数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private byte[] GetAssetData(string path)
    {
        byte[] assetData = null;
        if (File.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryReader reader = new BinaryReader(stream);
                assetData = reader.ReadBytes((int)stream.Length);

                reader.Close();
                stream.Close();
            }
            catch (IOException e)
            {
                Debug.LogException(e);

            }
        }

        return assetData;
    }


    /// <summary>
    /// Load Commone Atlas Assetbundle
    /// </summary>
    //private GameObject allShaderObjMainAsset = null;
    public void LoadCommonAtlasAssetbundle()
    {
        if (!( _curState == AssetbundleSynState.INIT_FINISH_STATE ) )
        {
            Debug.LogWarning("Assetbundles Version Date is not Initialization!!!!");
            return;
        }

        StartCoroutine(_LoadCommonAsset());

    }

    private IEnumerator _LoadCommonAsset()
    {
        if (_assetConfig != null)
        {
            List<string> commonObjList = _assetConfig.GetCommonObjects();
            foreach (string commonObjName in commonObjList)
            {
                if (_assetConfig.IsContainAsset(commonObjName))
                {
                    //AssetBundleCreateRequest request = _GetAssetRequest(commonObjName);
                    WWW request = _GetAssetRequestWWW(commonObjName);
                    if (request != null)
                    {
                        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.SHADER_INIT_TIME_TYPE);
                        yield return request;

                        if (request.isDone && request.error == null)
                        {
                            AssetBundle allShaderObject = request.assetBundle;
                            request.Dispose();
                            request = null;

                            if (allShaderObject != null)
                            {
                                ALLShaderLoader.Instance.Setup(allShaderObject);
                            }
                        }
                        else
                        {
                            GameDebuger.Log("Load Common Asset : " + commonObjName + " Error !!!");
                        }

                        GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.SHADER_INIT_TIME_TYPE);
                    }
                }
            }
        }
    }


    public void Dispose()
    {
        //if ( _assetsPoolManager != null ) _assetsPoolManager.Dispose();
    }

    public void ReloadAllAssets()
    {
        if (Directory.Exists(_downLoadPath ))
        {
            Directory.Delete(_downLoadPath, true);
        }
        this.Dispose();
    }
}
