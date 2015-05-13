// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  ResourceLoader.cs
// Author   : wenlin
// Created  : 2013/4/12 16:09:35
// Purpose  : 
// **********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceLoader
{
	public static string ResourcesHead( bool hasResourceHead )
	{
		return (!hasResourceHead) ? PathHelper.RESOURCES_PATH : string.Empty;
	}
	
	private const string prefabsString = "Prefabs"; 
	private static string[] extensionArray = new string[8] { "anim", "txt", "bytes", "png", "psd", "mat", "wav", "controller"  };

    /******************************************************************************************
     * 读取资源的同步接口
     * ***************************************************************************************/
    public static UnityEngine.Object Load(string path, string extension = "prefab", bool hasResourceHead = false )
    {
        UnityEngine.Object obj = null;

        //如果是空的路径， 直接放回NULL
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        //读取Assetbundle资源, 如果目录是Prefabs开头， 则表示此目录是Resources里面的目录，可以直接过滤
        if (!GameSetting.IsLoadLocalAssets() && !path.StartsWith(prefabsString) )
        {
            AssetBase assetBase = AssetbundleManager.Instance.LoadAsset( ResourcesHead(hasResourceHead) + path );
            if ( assetBase != null )
            {
                if (assetBase.asset == null)
                {
                    GameDebuger.Log("Get Remote Assets Success :  " + path + "but Asset is NULL");
                }

                obj = assetBase.asset;
            }
        }

#if UNITY_EDITOR
        //如果是模拟游戏模式
        if ((!GameSetting.IsLoadLocalAssets() && !GameSetting.mobileModel()) || GameSetting.IsLoadLocalAssets())
        {
            if (obj == null)
                obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);

            if (obj == null)
            {
                for (int i = 0; i < extensionArray.Length; i++)
                {
                    extension = extensionArray[i];
                    obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);
                    if (obj != null) break;
                }
            } 
        }

        if (obj == null)
        {
            obj = Resources.Load(path);
            if (!GameSetting.IsLoadLocalAssets() && GameSetting.mobileModel() ) { GameDebuger.Log(" Load Local Asset : " + PathHelper.ASSET_RESOURCES_PATH + path); };
        }

        return obj;
#else
        if ( obj != null )
        {
            return obj;
        }
        else
        {
            //GameDebuger.Log(" Load Local Asset : " + ResourcesHead(hasResourceHead) + path);
            return Resources.Load(path);
        }
#endif
    }

    /**********************************************************************************************
     * 异步方式获取资源
     * ********************************************************************************************/
    public static void LoadAsync(string path, Action<AssetBase> getter, string extension = "prefab", bool hasResourceHead = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            if (getter != null) getter(null);
            return;
        }

        //if( GameSetting.IsLoadLocalAssets())
        //{
#if UNITY_EDITOR
            if (!GameSetting.IsLoadLocalAssets())
            {
                AssetbundleManager.Instance.LoadAsset(ResourcesHead(hasResourceHead) + path, getter);
            }
            else
            {
                UnityEngine.Object obj = null;
                if ((!GameSetting.IsLoadLocalAssets() && !GameSetting.mobileModel()) || GameSetting.IsLoadLocalAssets())
                {

                    if (obj == null)
                        obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);

                    if (obj == null)
                    {
                        for (int i = 0; i < extensionArray.Length; i++)
                        {
                            extension = extensionArray[i];
                            obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);
                            if (obj != null) break;
                        }
                    } 
                }


                if (obj == null)
                {
                    obj = Resources.Load(path);
                    if (!GameSetting.IsLoadLocalAssets() && GameSetting.mobileModel()) { GameDebuger.Log(" Load Local Asset : " + PathHelper.ASSET_RESOURCES_PATH + path); };
                }

                if (getter != null)
                {
                    if (obj != null)
                    {
                        getter(new AssetBase(obj));   
                    }
                    else
                    {
                        getter(null);    
                    }
                }

            }
#else
//		UnityEngine.Object obj = null;
//		if (obj == null)
//		{
//			obj = Resources.Load(path);
//			if (!GameSetting.IsLoadLocalAssets() && GameSetting.mobileModel()) { GameDebuger.Log(" Load Local Asset : " + PathHelper.ASSET_RESOURCES_PATH + path); };
//		}
//		
//		if (getter != null)
//		{
//			if (obj != null)
//			{
//				getter(new AssetBase(obj));   
//			}
//			else
//			{
//				getter(null);    
//			}
//		}
       AssetbundleManager.Instance.LoadAsset(ResourcesHead(hasResourceHead) + path, getter);
#endif
    }


    /**********************************************************************************************
     * 异步读取一个资源数组列表
     * 回调函数getter : Dictionary<string, AssetBase> - 返回加载后的资源数据
     *                 List<string>                  -  返回不存在的资源名称
     * *******************************************************************************************/
    public static void LoadAssetListAsync(List<string> pathList, Action<Dictionary<string, AssetBase>, List<string> > getter, Action< int > progresser = null, string extension = "prefab")
    {
#if UNITY_EDITOR
            if (!GameSetting.IsLoadLocalAssets())
            {
                AssetbundleManager.Instance.LoadAssetList(pathList, getter, progresser);
            }
            else
            {
                UnityEngine.Object obj = null;
                Dictionary< string, AssetBase > assetBaseDic = new Dictionary<string,AssetBase>();
                List<string> donotExitsAssets = new List<string>();
                for( int i = 0 ; i < pathList.Count; i++ )
                {
                    string path = pathList[i];
                    if ((!GameSetting.IsLoadLocalAssets() && !GameSetting.mobileModel()) || GameSetting.IsLoadLocalAssets())
                    {

                        if (obj == null)
                            obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);

                        if (obj == null)
                        {
                            for (int j = 0; j < extensionArray.Length; j++)
                            {
                                extension = extensionArray[j];
                                obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);
                                if (obj != null) break;
                            }
                        } 
                    }

                    if (obj == null)
                    {
                        obj = Resources.Load(path);
                        if (!GameSetting.IsLoadLocalAssets() && GameSetting.mobileModel()) { GameDebuger.Log(" Load Local Asset : " + PathHelper.ASSET_RESOURCES_PATH + path); };
                    }

                    if (obj != null)
                    {
                        if (assetBaseDic.ContainsKey(path))
                        {
                            assetBaseDic.Add(path, new AssetBase(obj));
                        }
                    }
                    else
                    {
                        donotExitsAssets.Add(path);
                    }

                    if (progresser != null) progresser((i * 100 ) / pathList.Count);
                }

                if (getter != null && obj != null) getter(assetBaseDic, donotExitsAssets);   
            }
#else
                AssetbundleManager.Instance.LoadAssetList(pathList, getter, progresser);
#endif


    }

    public static UnityEngine.Object Load(string path, Type systemTypeInstance, bool hasResourceHead = false )
    {
#if UNITY_EDITOR
        return Resources.Load(path, systemTypeInstance);
        //return AssetDatabase.LoadAssetAtPath(ResourcesHead(hasResourceHead) + path, systemTypeInstance);
#else
        return Resources.Load(path, systemTypeInstance);
#endif
    }


    /***********************************************************************************************
     * 判断资源是否存在
     * *********************************************************************************************/
    public static bool IsAssetExist(string path, string extension = "prefab", bool hasResourceHead = false, bool isCollectAsset = true)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        if (!GameSetting.IsLoadLocalAssets())
        {
            bool isExist = false;
            isExist = AssetbundleManager.Instance.IsAssetExist(ResourcesHead(hasResourceHead) + path, isCollectAsset);
            if (isExist) return true;
        }

        UnityEngine.Object obj = null;
#if UNITY_EDITOR
		
		if( !GameSetting.mobileModel())
		{
			if (obj == null)
                obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);

            if (obj == null)
            {
                for (int i = 0; i < extensionArray.Length; i++)
                {
                    extension = extensionArray[i];
                    obj = AssetDatabase.LoadMainAssetAtPath(PathHelper.ASSET_RESOURCES_PATH + path + "." + extension);
                    if (obj != null) break;
                }
            }
		}

        if (obj == null) { obj = Resources.Load(path); }
        return (obj != null);
#else
        obj = Resources.Load(path);
        return (obj != null);
#endif
    }
	
	
    public static bool IsGameResAssetExist(string path, string extension = "prefab", bool hasResourceHead = false, bool isCollectAsset = true)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        if (!GameSetting.IsLoadLocalAssets())
        {
            bool isExist = false;
            isExist = AssetbundleManager.Instance.IsAssetExist(ResourcesHead(hasResourceHead) + path, isCollectAsset);
            if (isExist) return true;
        }
		
		return false;
    } 
	

    public static long GetAssetVersion(string path, bool isScene = false, bool hasResourceHead = false)
    {
        if (isScene)
        {
            return AssetbundleManager.Instance.GetAssetVersion( path, true);
        }
        else
        {
            return AssetbundleManager.Instance.GetAssetVersion(ResourcesHead(hasResourceHead) + path, false);
        }
    }


    /***************************************************************************************************
     * 异步读取场景
     * ************************************************************************************************/
    public static void LoadLevelAsync(string sceneName, 
                                      AssetbundeMsgSyn.SceneProgressCallBack sceneProgressCallBack = null, AssetbundeMsgSyn.SceneLoadFinishCallBack sceneLoadFinishCallBack = null,
                                      AssetbundeMsgSyn.AfterLoadLevelFinishBehavior behavior = AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                                      AssetbundeMsgSyn.SceneObjectsHanderFunc sceneObjectHanderFunc = null)
    {
        AssetbundleManager.Instance.LoadLevelAsync(sceneName, sceneProgressCallBack, sceneLoadFinishCallBack, behavior, sceneObjectHanderFunc);
    }


    public static void LoadLevelAdditiveAsync(string sceneName, 
                                              AssetbundeMsgSyn.SceneProgressCallBack sceneProgressCallBack = null, AssetbundeMsgSyn.SceneLoadFinishCallBack sceneLoadFinishCallBack = null,
                                              AssetbundeMsgSyn.AfterLoadLevelFinishBehavior behavior = AssetbundeMsgSyn.AfterLoadLevelFinishBehavior.Immediately_Init_SceneObj,
                                              AssetbundeMsgSyn.SceneObjectsHanderFunc sceneObjectHanderFunc = null)
    {
        AssetbundleManager.Instance.LoadLevelAdditiveAsync(sceneName, sceneProgressCallBack, sceneLoadFinishCallBack, behavior, sceneObjectHanderFunc);
    }

}
