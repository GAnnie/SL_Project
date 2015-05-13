using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetbundleManager
{
    private static AssetbundleManager _instance = null;
    public static AssetbundleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AssetbundleManager();
            }
            return _instance;
        }
    }

    private AssetbundleManager() 
    {

    }

    private AssetbundeMsgSyn assetbundleMsgSyn = null;
    public void Setup()
    {
        GameObject go = new GameObject();
        go.name = "AssetbundeMsgSyn";
        GameObject.DontDestroyOnLoad(go);
        assetbundleMsgSyn = go.AddComponent<AssetbundeMsgSyn>();
    }

    /// 准备加载本地整包资源 
    /// <param name="finishMessageFunc"></param>
    public void InitializeLocalPackage( long resDataVer, AssetbundeMsgSyn.DataUpdateMessageCallBack dataUpdateMessageFunc)
    {
        if (assetbundleMsgSyn != null)
        {
			assetbundleMsgSyn.InitAssetsData(GetStreamAssetPath(), GetStreamAssetPath(), true, resDataVer, dataUpdateMessageFunc);
        }
    }

    public void Initialize( long resDataVer, AssetbundeMsgSyn.DataUpdateMessageCallBack dataUpdateMessageFunc )
    {
        if (assetbundleMsgSyn != null)
        {
			assetbundleMsgSyn.InitAssetsData(GetAssetTargetPath(), GetAssetResPath(), false, resDataVer, dataUpdateMessageFunc);
        }
    }
	
	public void InitializeFreeDownloader()
	{
        if (assetbundleMsgSyn != null)
        {
			assetbundleMsgSyn.InitFreeDownloalder(GetAssetTargetPath(), GetAssetResPath());
        }
	}

    public void StartLoad(Action<int> dataLoadingMsgFunc, AssetbundeMsgSyn.DataLoadFinish dataLoadFinishFunc)
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.StartDownLoad(dataLoadingMsgFunc, dataLoadFinishFunc);
        }
    }

	/// <summary>
	/// 现在指定资源
	/// </summary>
	public void DownLoadAssets( List< string > assetRefList , Action<int> dataLoadingMsgFunc, AssetbundeMsgSyn.DataLoadFinish dataLoadFinishFunc )
	{
        if (assetbundleMsgSyn != null)
        {
			assetbundleMsgSyn.StartDownLoadAssetList( assetRefList, dataLoadingMsgFunc, dataLoadFinishFunc);
        }		
	}
	
	public void StopDownLoadAssets()
	{
        if (assetbundleMsgSyn != null)
        {
			assetbundleMsgSyn.StopDownLoadAssetList();
        }			
	}
	
	
    public void BeginGame()
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.BeignGame();
        }
        else
        {
            Debug.LogError("assetbundleMsgSyn is null ");
        }
    }


    public void CleanCache()
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.ReloadAllAssets();
        }
    }


    private string GetStreamAssetPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return Application.streamingAssetsPath + "/";
            //return "file:///" + Application.streamingAssetsPath + "/";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            return string.Empty;
            //return Application.streamingAssetsPath + "/";
        }
        else
        {
            return Application.streamingAssetsPath + "/";
            //return "file:///" + Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// Get Target asset's path 
    /// </summary>
    /// <returns></returns>
    private string GetAssetTargetPath()
    {
        return GameSetting.httpStaticPath;
    }

	private string GetAssetResPath()
	{
		return GameSetting.httpResPath;
	}

    //Load Scene 
    public void LoadLevelAsync(string sceneName, 
                               AssetbundeMsgSyn.SceneProgressCallBack sceneProgressCallBack = null, AssetbundeMsgSyn.SceneLoadFinishCallBack sceneLoadFinishCallBack = null, 
                               AssetbundeMsgSyn.AfterLoadLevelFinishBehavior behavior = AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                               AssetbundeMsgSyn.SceneObjectsHanderFunc sceneObjectHanderFunc = null)
    {
        if( assetbundleMsgSyn != null )
        {
            assetbundleMsgSyn.LoadLevelAsync(sceneName, sceneProgressCallBack, sceneLoadFinishCallBack, behavior, sceneObjectHanderFunc);
        }
    }


    public void LoadLevelAdditiveAsync(string sceneName, AssetbundeMsgSyn.SceneProgressCallBack sceneProgressCallBack = null, AssetbundeMsgSyn.SceneLoadFinishCallBack sceneLoadFinishCallBack = null,
                                       AssetbundeMsgSyn.AfterLoadLevelFinishBehavior behavior = AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                                       AssetbundeMsgSyn.SceneObjectsHanderFunc sceneObjectHanderFunc = null)
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.LoadLevelAdditiveAsync(sceneName, sceneProgressCallBack, sceneLoadFinishCallBack, behavior, sceneObjectHanderFunc);
        }
    }

    public AssetBase LoadAsset(string objName)
    {
        if (assetbundleMsgSyn != null)
        {
            return assetbundleMsgSyn.LoadObject(objName);
        }
        return null;
    }

	public void LoadAsset( string objName,Action<AssetBase> callBackFun)
	{
		if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.LoadObjectAsync(objName, callBackFun);
        }
	}

    public void LoadAssetList(List<string> objNameList, Action<Dictionary<string, AssetBase>, List<string>> getter, Action< int > progresser = null)
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.LoadObjectListAsync(objNameList, getter, progresser);
        } 
    }

    public AssetBase LoadAtlas(string atlasName)
    {
        if (assetbundleMsgSyn != null)
        {
            return assetbundleMsgSyn.LoadAtlas(atlasName);
        }
        return null;        
    }

    public long GetAssetVersion(string objName, bool isScene = false)
    {
        if (assetbundleMsgSyn != null)
        {
            return assetbundleMsgSyn.GetAssetsVersion(objName, isScene);
        }
        return 0;
    }

    public List<string> SearchAssets(string rootPath)
    {
        if (assetbundleMsgSyn != null)
        {
            return assetbundleMsgSyn.SearchAssets(rootPath);
        }
        return null;
    }


    public bool IsAssetExist(string objName, bool isCollectAsset = true )
    {
//        if (assetbundleMsgSyn != null)
//        {
//            return assetbundleMsgSyn.IsAssetExist(objName);
//        }
//        return false;
		
		return AssetbundeMsgSyn.IsAssetExist( objName, isCollectAsset );
    }
	
	
	//收集缺失资源
	public void Collect( string objName )
	{
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.CollectMissingAsset( objName );
        }
	}
	
	
	public Dictionary< string, AssetPrefab > GetMissingAssets()
	{
		return AssetbundeMsgSyn.GetMissingAssets();
	}
	
	public int GetTotalResourceNumber()
	{
		return AssetbundeMsgSyn.GetGameTotalAssets();
	}
	
	public bool IsAssetMissing( string refId )
	{
		return AssetbundeMsgSyn.IsAssetMissing( refId );
	}
	
	public void Clear()
	{
		if( assetbundleMsgSyn != null )
		{
			assetbundleMsgSyn.ClearReference();
		}
	}
	
    public void UnLoadAtlas(string atlasName)
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.UnLoadAtlas(atlasName);
        } 
    }

    public void DownLoadCommonAtlas()
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.LoadCommonAtlasAssetbundle();
        }
    }


    public void Dispose()
    {
        if (assetbundleMsgSyn != null)
        {
            assetbundleMsgSyn.Dispose();
        }
    }
}
