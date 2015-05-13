// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AssetbundleDownLoader.cs
// Author   : wenlin
// Created  : 2013/6/8 17:00:05
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using UnityEngine;


public class LocalAssetbundleDownLoader : AssetsLoader
{
    private  Queue<AssetPackage> _needToUpdatePrefebQueue = null;

    private const string unity3dEx = ".unity3d";

    //构造函数
	public LocalAssetbundleDownLoader(AssetsConfig config, AssetsExistMsg existMsg,  string remoteVersionPath, string remoteResPath, string downLoadPath) :
		base(config, existMsg, remoteVersionPath, remoteResPath, downLoadPath)
    {
    }

    /*********************************************************************************
     *    如果当前包是整包的时候， 调用这个函数来判断和更新当前<LocalAssetsVersionData>里面的信息
     *   <param name="checkVersionFinishFunc">获取成功后的回调</param>
     *   <param name="errorFunc">错误的回调</param>
     *********************************************************************************/
    public override void CheckVersion(Action checkVersionFinishFunc, Action<ErrorType, string> errorFunc = null)
    {
        _assetbundleCheckVersionFinish = checkVersionFinishFunc;
        _assetbundleCheckVersionError = errorFunc;

        string localVersionCtrlPath = _assetsTargetPath + _assetVerCtrl;

        GameDebuger.Log("Local Asset Version Path " + localVersionCtrlPath);

        //获取本地资源信息
        HttpController.Instance.DownLoad(localVersionCtrlPath, __CheckAssetbundleVersion, null, _LoadVersionDataError,false, SimpleWWW.ConnectionType.Short_Connect);
    }

    /// 错误信息回调
    /// <param name="e"></param>
    private void _LoadVersionDataError(Exception e)
    {
        if (_assetbundleCheckVersionError != null) _assetbundleCheckVersionError(ErrorType.AssetsVerDataLoadError, "Load AssetBudle Version Data Error : " + e.StackTrace + e.Message);
    }

    /// 版本数据回调
    private void __CheckAssetbundleVersion(byte[] bytes)
    {
        GameDebuger.Log("Version Data Ready ");

        if (bytes != null)
        {
            _assetsConfig.curLoadU3DNumber      = 0;
            _assetsConfig.totalU3DNumber        = 0;
            _assetsConfig.totalUpdateAssetsSize = 0;
			
			
			//分析ResourcesData
			 byte[]	resVersionDataBytes	= null;
			 byte[]	assetRefListBytes	= null;
			 byte[]	mapAreaDataBytes	= null;			
			
 
			ResourceDataManager.AnalyResourceData( bytes,
												   out resVersionDataBytes,
												   out assetRefListBytes
				);
			
			//初始化AssetRefList
			AssetPathReferenceManager.Instance.Setup( assetRefListBytes );
			AssetPathReferenceManager.Instance.UpdateAssetPathRefData( assetRefListBytes );
			
//			//初始化mapAreaDataBytes
//			MapAreasDataController.Instance.Setup( mapAreaDataBytes );
//			MapAreasDataController.Instance.UpdataMapAreasData(mapAreaDataBytes);
			
            //获取ResroucesVersionData
//			SaveRometeResourcesVersionData( resVersionDataBytes );
            ResourcesVersionData remoteassetBudleVersionData = GetRometeResourcesVersionData(resVersionDataBytes);
			
            if (remoteassetBudleVersionData != null)
            {
                Dictionary<string, LocalAssetPrefab> localAssetDic = _assetsConfig.localAssetbundleVersionData.localAssetDic;
                if (localAssetDic == null)
                {
                    localAssetDic = _assetsConfig.localAssetbundleVersionData.localAssetDic = new Dictionary<string, LocalAssetPrefab>();
                }

                _needToUpdatePrefebQueue = new Queue<AssetPackage>();

                //如果大于或则等于本地资源文件版本号的时候， 才会进行版本对比
                if (remoteassetBudleVersionData.version >= _assetsConfig.localAssetbundleVersionData.version)
                {
					_assetsExistMsg.Clear();
					
                    _CompareAssetVersion(_needToUpdatePrefebQueue, remoteassetBudleVersionData.assetPrefabs, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize, ref _assetsConfig.totalU3DNumber);
                    _CompareAssetVersion(_needToUpdatePrefebQueue, remoteassetBudleVersionData.commonObjs, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize, ref _assetsConfig.totalU3DNumber);
                    _CompareAssetVersion(_needToUpdatePrefebQueue, remoteassetBudleVersionData.scenes, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize, ref _assetsConfig.totalU3DNumber);

                    //if (_localAssetbundleVersionData.assetsList.Count > 0 )
                    //{
                    //    DelectNotExistAssets(totalRemotePrefabList, _assetsDictionary);
                    //    totalRemotePrefabList = CompareAssetVersion(totalRemotePrefabList, _assetsDictionary);
                    //}

                    //保存远程版本文件的版本号信息
                    _curVersionDataVer = remoteassetBudleVersionData.version;
					
					//保存缺失资源
					_assetsExistMsg.SaveMissingMessage(_downLoadPath);
                }
                else
                {
                    GameDebuger.Log("====**** Local Assetbundle Version Data is Out of Data ");
                }

                //获取共用资源的引用
                _assetsConfig.InitCommonObject(remoteassetBudleVersionData);

                //获取场景信息
                _assetsConfig.InitScenes(remoteassetBudleVersionData);

                _isInit = true;
                if (_assetbundleCheckVersionFinish != null) _assetbundleCheckVersionFinish();
            }
            else
            {
                GameDebuger.Log("Load Remote AssetBudle Version Data Error");
                _assetbundleCheckVersionError(ErrorType.AssetsVerDataLoadError, "Load Remote AssetBudle Version Data Error");
            }
        }
        else
        {
            GameDebuger.Log("Version Data is null ");
        }

        CheckVersionRelease();
    }

    /// 释放版本检测的引用
    private void CheckVersionRelease()
    {
        _assetbundleCheckVersionFinish = null;
        _assetbundleCheckVersionError = null;
    }

    /// 资源版本的对比
    /// <param name="assetPrefabList"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    private void _CompareAssetVersion(Queue<AssetPackage> needToUpdateQueue,
                                      Dictionary<string, AssetPrefab> remoteAssetVersionDic,
                                      Dictionary<string, LocalAssetPrefab> localAssetsVersionDic,
                                      ref uint totalSize,
                                      ref int  totalNum)
    {
        foreach (KeyValuePair<string, AssetPrefab> item in remoteAssetVersionDic)
        {
			++_assetsExistMsg.resTotalNumber;
			
			string path = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );
            //如果不存在， 则加入更新列表里面
            if (!localAssetsVersionDic.ContainsKey(path))
            {
                totalNum++;
                totalSize += item.Value.size;
                needToUpdateQueue.Enqueue(new AssetPackage(path, item.Value));
            }
            else
            {
                if (item.Value.version > localAssetsVersionDic[path].version)
                {
                    totalNum++;
                    totalSize += item.Value.size;
                    needToUpdateQueue.Enqueue(new AssetPackage(path, item.Value));
                }
            }
        }
    }

    /// 删除不存在的资源文件
    /// <param name="remoteAssetsList">远程资源数据目录</param>
    /// <param name="localTotalAssetsDic">本地资源数据目录</param>
    private void DelectNotExistAssets(List<AssetPrefab> remoteAssetsList, Dictionary<string, AssetPrefab> localTotalAssetsDic)
    {
        //Dictionary<string, bool> existAssets = new Dictionary<string, bool>();
        //foreach (KeyValuePair<string, AssetPrefab> item in localTotalAssetsDic)
        //{
        //    existAssets.Add(item.Key, false);
        //}

        //foreach (AssetPrefab assetPrefab in remoteAssetsList)
        //{
        //    if (existAssets.ContainsKey(assetPrefab.name))
        //    {
        //        existAssets[assetPrefab.name] = true;
        //    }
        //}

        //foreach (KeyValuePair<string, bool> item in existAssets)
        //{
        //    // Is not  Exist
        //    if (localTotalAssetsDic.ContainsKey(item.Key) && !item.Value)
        //    {
        //        string AssetsPath = _downLoadPath + localTotalAssetsDic[item.Key].path;
        //        if (File.Exists(AssetsPath)) File.Delete(AssetsPath);
        //        localTotalAssetsDic.Remove(item.Key);
        //    }
        //}
    }


    /********************************************************************************************************
     * 加载需要更新的资源， 并且修改<LocalAssetsVersionData>数据信息
     ********************************************************************************************************/
	public override void DownLoad(Action allFinishCallBack = null, Action<int> progressFunc = null, Action<ErrorType, string> errorCallBack = null)
    {
        _assetbundleDownLoadFinish = allFinishCallBack;
        _assetbundleDownError      = errorCallBack;
        _assetsDownLoadProgress    = progressFunc;

        if (!_isInit)
        {
            if (_assetbundleDownError != null) _assetbundleDownError(ErrorType.AssetsVerDataLoadError, "Assetbundles Version Date is not Initialization!!!!");
            return;
        }
        //_curDownloadingNum = 0;

        BeginDownLoad();

        //下载全部更新资源
        _DownLoad();
    }

    /// <summary>
    /// 开始加载
    /// </summary>
    private GameObject _localLoadObj = null;
    private LocalLoadCom _localLoadCom = null;
	private int _totalDownLoadBytes = 0;
    private void BeginDownLoad()
    {
		_totalDownLoadBytes = 0;
		
        _isDownLoading = true;

        _localLoadObj = new GameObject("LocalLoadObj");
        _localLoadCom = _localLoadObj.AddComponent<LocalLoadCom>();
        _localLoadCom.Setup(_assetsTargetPath, _downLoadPath);

        JudgeDownLoadNumber();
    }

    /// <summary>
    /// 结束加载
    /// </summary>
    private void EndDownLoad()
    {
        _isDownLoading = false;

        if (_needToUpdatePrefebQueue != null)
        {
            _needToUpdatePrefebQueue.Clear();
            _needToUpdatePrefebQueue = null;
        }

        if (_localLoadObj != null)
        {
            GameObject.DestroyImmediate(_localLoadObj);
        }

        //设置标记为， 如果是整包， 此位来标记整包是否已经更新完毕
        GameSetting.SetUpdateLocalPackageFlag();
    }



    //最大同时下载的数量
#if UNITY_EDITOR
    private int MAX_DOWM_NUM_ONCE = 3;
#else
	private int MAX_DOWM_NUM_ONCE = 2;
#endif
	


    /// <summary>
    /// 判断游戏线程数量
    /// </summary>
    private void JudgeDownLoadNumber()
    {
#if UNITY_ANDROID
			if (SystemSetting.IsMobileRuntime)
        {
            //如果超过512内存， 则显示使用4个线程
            int memorySize = (int)(BaoyugameSdk.getTotalMemory() / 1024);
            if (memorySize > 512)
            {
                MAX_DOWM_NUM_ONCE = 4;
            }
            else
            {
                MAX_DOWM_NUM_ONCE = 2;
            }

            //是否整包加载
            if (GameSetting.IsUpdateLocalPackage())
            {
                MAX_DOWM_NUM_ONCE /= 2;
            }
        }
#endif

        GameDebuger.Log("=======Assetbundle DownLoader : Once DownLoad num : " + MAX_DOWM_NUM_ONCE + "==========");
    }

    //当前下载的数量
    private int _curDownloadingNum = 0;

    /// <summary>
    /// 现在资源
    /// </summary>
    private void _DownLoad()
    {
        if (_localLoadCom != null && _needToUpdatePrefebQueue != null)
        {
			
//#if !UNITY_IPHONE		               
            _localLoadCom.LoadLocalAssets(_needToUpdatePrefebQueue,
                                           _LoadOneUpdatePrefab,
                                           _LoadAssetsError,
                                           _LoadLocalAssetsFinish);
			                       
//#else
//
//
//			while(  _needToUpdatePrefebQueue.Count > 0)
//			{
//				AssetPackage updatePrefab = _needToUpdatePrefebQueue.Dequeue();
//				_LoadOneUpdatePrefab( updatePrefab );
//			}
//			
//			_LoadLocalAssetsFinish();
//#endif

                                           
        }
        //if (_needToUpdatePrefebQueue != null)
        //{
        //    while ((_curDownloadingNum < MAX_DOWM_NUM_ONCE) && (_needToUpdatePrefebQueue.Count > 0) && !appQuit)
        //    {
        //        _curDownloadingNum++;

        //        LocalUpdatePrefab updatePrefab = _needToUpdatePrefebQueue.Dequeue();
        //        string url = _assetsTargetPath + updatePrefab.assetName + unity3dEx;
        //        //GameDebuger.Log(" url : " + url);
        //        HttpController.Instance.DownLoad(url, __LoadAssetFinish, updatePrefab, null, _LoadAssetsError, false);
        //    }
        //}
    }
	
	
	
    /// 每次转移一个数据， 就会调用一次这个函数
    /// <param name="updatePrefab"></param>
    private void _LoadOneUpdatePrefab(AssetPackage updatePrefab)
    {
        if (updatePrefab != null)
        {
            //string downPath = _downLoadPath + updatePrefab.assetName + unity3dEx;
            //SaveAsset(downPath, bytes);
            AddOneAssetsMessage(updatePrefab);
        }
        _assetsConfig.curLoadU3DNumber++;

        //显示回调信息
        if (_assetsDownLoadProgress != null)
        {
//            int percent = ((_assetsConfig.curLoadU3DNumber) * 100 / _assetsConfig.totalU3DNumber);
//            if (percent > 100) percent = 100;
			_totalDownLoadBytes += (int)updatePrefab.assetPrefab.size;
			_assetsDownLoadProgress(_totalDownLoadBytes);
        }
    }

    private void _LoadAssetsError(string errorMessage)
    {
        if (_assetbundleDownError != null) _assetbundleDownError(ErrorType.AssetsDownLoadError, errorMessage);
    }

    //转移完成之后的调用
    private void _LoadLocalAssetsFinish()
    {
        //if (_assetsConfig.curLoadU3DNumber >= _assetsConfig.totalU3DNumber)
        {
            GameDebuger.Log(" Current Local Load Assts Number : " + _assetsConfig.curLoadU3DNumber);
            GameDebuger.Log(" Total   Local Load Assts Number : " + _assetsConfig.totalU3DNumber);

            //结束加载
            EndDownLoad();

            //完成加载版本
            FinishAssetsBundleVersionData();

            //完成所有资源下载
            if (_assetbundleDownLoadFinish != null) _assetbundleDownLoadFinish();

            //释放引用
            DownLoadRelease();

        }
    }

    //释放下载中的引用
    private void DownLoadRelease()
    {
        _assetbundleDownLoadFinish = null;
        _assetbundleDownError = null;
        _assetsDownLoadProgress = null;
    }


    /// <summary>
    /// 加载资源完成
    /// </summary>
    /// <param name="bytes"></param>
    //private void __LoadAssetFinish(byte[] bytes, object obj)
    //{
    //    if (_curDownloadingNum > 0)
    //        _curDownloadingNum--;
    //    else
    //    {
    //        GameDebuger.Log("=========== LocalAssetbundleDownLoader Load number error ============");
    //        return;
    //    }

    //    LocalUpdatePrefab updatePrefab = obj as LocalUpdatePrefab;
    //    if (updatePrefab != null)
    //    {
    //        string downPath = _downLoadPath + updatePrefab.assetName + unity3dEx;
    //        SaveAsset(downPath, bytes);
    //        AddOneAssetsMessage(updatePrefab);
    //    }

    //    _assetsConfig.curLoadU3DNumber++;

    //    GC.Collect();
    //    _DownLoad();
    //}

    private void AddOneAssetsMessage(AssetPackage updateAssetPrefab)
    {
        if (_assetsConfig.localAssetbundleVersionData != null)
        {
            if (!_assetsConfig.localAssetbundleVersionData.localAssetDic.ContainsKey(updateAssetPrefab.name))
            {
                _assetsConfig.localAssetbundleVersionData.localAssetDic.Add(updateAssetPrefab.name, new LocalAssetPrefab());
            }
            CopyAssetPrefab(updateAssetPrefab.assetPrefab, _assetsConfig.localAssetbundleVersionData.localAssetDic[updateAssetPrefab.name]);
        }
    }
}
