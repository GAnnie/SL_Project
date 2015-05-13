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

public class AssetbundleDownLoader : AssetsLoader
{
	#region ExperiencePack
	private class ExperiencePack
	{
		//目录的引用ID
		public string folderId = string.Empty;
		
		public uint size = 0;
		
		public List< AssetPackage > assetPackageList = null;
		
		public ExperiencePack( string  folderId)
		{
			this.folderId = folderId;
			
			this.size = 0;
			
			assetPackageList = new List<AssetPackage>();
		}
	}
	#endregion
	
	//完整资源包
    private MergePackageList _mergePackageList = null;
	//体验包资源
	private Queue< ExperiencePack > _assetPrefabQueue = null;
	//体验包目录
	private const string miniPackage = "minipackage/";
	
    private const string unity3dEx = ".unity3d";
	private const string zipEX     = ".zBaoyu";
    //构造函数
	public AssetbundleDownLoader(AssetsConfig config, AssetsExistMsg existMsg,  string remoteVersionPath, string remoteResPath, string downLoadPath):
		base( config, existMsg, remoteVersionPath, remoteResPath, downLoadPath)
    {
    }

    /*******************************************************************************************************
     *   调用此接口会获取远程的一个版本好目录， 并检查资源更新情况， 然后
     *    等下下载接口的调用， 下载所需的合拼包资源。
     *   <param name="checkVersionFinishFunc">获取成功后的回调</param>
     *   <param name="errorFunc">错误的回调</param>
     *******************************************************************************************************/
    public override void CheckVersion(Action checkVersionFinishFunc, Action<ErrorType, string> errorFunc = null)
    {
        _assetbundleCheckVersionFinish = checkVersionFinishFunc;
        _assetbundleCheckVersionError  = errorFunc;

        string assetCtrlPath = null;
        //获取远程目录版本数据
        if (GameSetting.IsUpdateLocalPackage())
        {
            assetCtrlPath = _assetsTargetPath + _assetVerCtrl;
        }
        else
        {
            assetCtrlPath = _assetsTargetPath + _assetVerCtrl + "?ver=" + DateTime.Now.Ticks.ToString();
        }
        GameDebuger.Log("Asset Version Path " + assetCtrlPath);

        //HTTP 获取
        HttpController.Instance.DownLoad(assetCtrlPath, __CheckAssetbundleVersion, null,_LoadVersionDataError, false, SimpleWWW.ConnectionType.Short_Connect);
    }

    /// 错误信息回调
    private void _LoadVersionDataError( Exception e )
    {
        if (_assetbundleCheckVersionError != null) _assetbundleCheckVersionError(ErrorType.AssetsVerDataLoadError, "Load AssetBudle Version Data Error : " +  e.StackTrace + e.Message);
    }

    /// 版本数据回调
    private void __CheckAssetbundleVersion( byte[] bytes )
    {
        GameDebuger.Log("Version Data Ready ");

        if (bytes != null)
        {
            _assetsConfig.curLoadU3DNumber = 0;
            _assetsConfig.totalU3DNumber = 0;
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
			SaveRometeResourcesVersionData( resVersionDataBytes );
            ResourcesVersionData remoteassetBudleVersionData = GetRometeResourcesVersionData(resVersionDataBytes);

            if (remoteassetBudleVersionData != null)
            {
                Dictionary<string, LocalAssetPrefab> localAssetDic = _assetsConfig.localAssetbundleVersionData.localAssetDic;
                if (localAssetDic == null)
                {
                    localAssetDic = _assetsConfig.localAssetbundleVersionData.localAssetDic = new Dictionary<string, LocalAssetPrefab>();
                }

                _mergePackageList = new MergePackageList();
				_assetPrefabQueue = new Queue<ExperiencePack>();
				Dictionary< string, ExperiencePack > expriencePackDic = new Dictionary<string, ExperiencePack>();
                //如果大于或则等于本地资源文件版本号的时候， 才会进行版本对比
                if (remoteassetBudleVersionData.version >= _assetsConfig.localAssetbundleVersionData.version)
                {
					_assetsExistMsg.Clear();
					
                    _CompareAssetVersion(_mergePackageList, expriencePackDic, remoteassetBudleVersionData.assetPrefabs, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize);
                    _CompareAssetVersion(_mergePackageList, expriencePackDic, remoteassetBudleVersionData.commonObjs, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize);
                    _CompareAssetVersion(_mergePackageList, expriencePackDic, remoteassetBudleVersionData.scenes, localAssetDic, ref _assetsConfig.totalUpdateAssetsSize);

                    //if (_localAssetbundleVersionData.assetsList.Count > 0 )
                    //{
                    //    DelectNotExistAssets(totalRemotePrefabList, _assetsDictionary);
                    //    totalRemotePrefabList = CompareAssetVersion(totalRemotePrefabList, _assetsDictionary);
                    //}

                    //保存远程版本文件的版本号信息
                    _curVersionDataVer = remoteassetBudleVersionData.version;
					
                    _assetsConfig.totalU3DNumber += _mergePackageList.AnalyMessage();
					
					//处理体验包队列
					foreach( KeyValuePair< string, ExperiencePack > item in expriencePackDic ) 
					{ 
						_assetPrefabQueue.Enqueue( item.Value ); 
						_assetsConfig.totalU3DNumber++;
					}
					
					//保存缺失资源
					_assetsExistMsg.SaveMissingMessage( _downLoadPath );
                }
                else
                {
                    GameDebuger.Log("====**** Remote Assetbundle Version Data is Out of Data  - local.version :" + _assetsConfig.localAssetbundleVersionData.version + 
                                        "Remote.version : " + remoteassetBudleVersionData.version);
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
        _assetbundleCheckVersionError  = null;
    }
	
	
    /// 资源版本的对比
    /// <param name="assetPrefabList"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    private void _CompareAssetVersion( MergePackageList mergePackList,
									  Dictionary< string, ExperiencePack > expriencePackDic,
                                      Dictionary< string, AssetPrefab > remoteAssetVersionDic, 
                                      Dictionary< string, LocalAssetPrefab> localAssetsVersionDic,
                                      ref uint totalSize )
    {
		//更新一下规则， 
		// 1】 如果资源是体验包资源， 则单独保存出来		
		// 2】 如果资源是非体验包资源， 则做一下更新条件
		//		如果local 里面是存在此资源， 但是Local的版本比较低
		foreach (KeyValuePair<string, AssetPrefab> item in remoteAssetVersionDic)
        {
			++_assetsExistMsg.resTotalNumber;
			
			string path = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );

			//TODO,更新包修改为需要加载所有资源才能进入游戏,所以强制修改为非体验包资源
			//item.Value.isEp = 0;

			//如果当前资源是体验包资源
			if( item.Value.isEp == 1 )
			{
				string foldId = item.Key.Split(':')[0];
			
				
	            //如果不存在， 则加入更新列表里面
	            if (!localAssetsVersionDic.ContainsKey(path))
	            {
					totalSize += item.Value.size;
					AssetPackage assetPackage = new AssetPackage( path, item.Value );
					
					if( !expriencePackDic.ContainsKey( foldId ) )
					{
						expriencePackDic.Add( foldId, new ExperiencePack(foldId));
					}						
					expriencePackDic[foldId].assetPackageList.Add( assetPackage );
					expriencePackDic[foldId].size += item.Value.size;
	            }
				
	            else
	            {
	                if (item.Value.version > localAssetsVersionDic[path].version)
	                {
						totalSize += item.Value.size;
						AssetPackage assetPackage = new AssetPackage( path, item.Value );
						
						if( !expriencePackDic.ContainsKey( foldId ) )
						{
							expriencePackDic.Add( foldId, new ExperiencePack(foldId));
						}
						expriencePackDic[foldId].assetPackageList.Add( assetPackage );
						expriencePackDic[foldId].size += item.Value.size;
	                }
	            }				
			}
			
			//如果非体验包
			else
			{
	            //如果是非体验包资源， 而且资源不在本地， 则不作处理
	            if (!localAssetsVersionDic.ContainsKey(path))
	            {
					//如果当前资源不存在，则保存
					_assetsExistMsg.SetMissingResource( item.Key,  item.Value );
					continue;

//					//TODO,更新包修改为需要加载所有资源才能进入游戏
//					totalSize += item.Value.size;
//					mergePackList.AddUpdateAsset(path, item.Value);		
	            }
				
	            else
	            {
	                if (item.Value.version > localAssetsVersionDic[path].version)
	                {
						totalSize += item.Value.size;
						mergePackList.AddUpdateAsset(path, item.Value);						
						
	                }
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
    public override void DownLoad( Action allFinishCallBack = null, Action<int> progressFunc = null, Action<ErrorType, string> errorCallBack = null)
    {
        _assetbundleDownLoadFinish = allFinishCallBack;
        _assetbundleDownError      = errorCallBack;
        _assetsDownLoadProgress    = progressFunc;

        if (!_isInit)
        {
            if (_assetbundleDownError != null) _assetbundleDownError(ErrorType.AssetsVerDataLoadError, "Assetbundles Version Date is not Initialization!!!!");
            return;
        }

        _curDownloadingNum = 0;

        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.LOADING_TIME_TYPE);

        BeginDownLoad();

        //下载全部更新资源
        _DownLoad();
    }

    /// <summary>
    /// 开始加载
    /// </summary>
    
	private int _totalDownLoadBytes = 0;
    private void BeginDownLoad()
    {
        _isDownLoading = true;
		_totalDownLoadBytes = 0;
        JudgeDownLoadNumber();
    }

    /// <summary>
    /// 结束加载
    /// </summary>
    private void EndDownLoad()
    {
        _isDownLoading = false;

        _mergePackageList.Dispose();
        _mergePackageList = null;

        //设置标记为， 如果是整包， 此位来标记整包是否已经更新完毕
        //GameSetting.SetUpdateLocalPackageFlag();
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
		if( SystemSetting.IsMobileRuntime)
        {
            ////如果超过512内存， 则显示使用4个线程
            //int memorySize = (int)(BaoyugameSdk.getTotalMemory()/1024);
            //if (memorySize > 512)
            //{
            //    MAX_DOWM_NUM_ONCE = 4;
            //}
            //else
            //{
            //    MAX_DOWM_NUM_ONCE = 2;
            //}

            ////是否整包加载
            //if (GameSetting.IsUpdateLocalPackage())
            //{
            //    MAX_DOWM_NUM_ONCE /= 2;
            //}

            //单线程下载资源
           // MAX_DOWM_NUM_ONCE = 2;
        }
#endif
    }

    //当前下载的数量
    private int _curDownloadingNum = 0;
    
    /// <summary>
    /// 现在资源
    /// </summary>
    private void _DownLoad()
    {
        //显示回调信息
        if (_assetsDownLoadProgress != null)
        {
//            int percent = ((_assetsConfig.curLoadU3DNumber) * 100 / _assetsConfig.totalU3DNumber);
//            if (percent > 100) percent = 100;
//			
//            _assetsDownLoadProgress(percent);
        }		
	

		if (_assetsConfig.curLoadU3DNumber >= _assetsConfig.totalU3DNumber || 
			 (_mergePackageList == null && _assetPrefabQueue == null))
        {
            GameDebuger.Log(" Current Load Assts Number : " + _assetsConfig.curLoadU3DNumber);
            GameDebuger.Log(" Total Load Assts Number : "   + _assetsConfig.totalU3DNumber);

            //结束加载
            EndDownLoad();

            //记录时间
            GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.LOADING_TIME_TYPE);

            //完成加载版本
            FinishAssetsBundleVersionData();

            //完成所有资源下载
			if (_assetbundleDownLoadFinish != null) _assetbundleDownLoadFinish();
			
            //释放引用
            DownLoadRelease();
            return;
        }
		
		//普通资源的加载
        if ( _mergePackageList != null)
        {
            while ((_curDownloadingNum < MAX_DOWM_NUM_ONCE) && !_mergePackageList.IsEnd() && !appQuit)
            {
                _curDownloadingNum++;

                MergePackage meragePackage = _mergePackageList.NextPackage();
				string url = _assetsResPath + meragePackage.packageName + ".baoyu";
				uint packSize = meragePackage.size;
				float oldProgress = 0.0f;
                //GameDebuger.Log(" url : " + url);
                HttpController.Instance.DownLoad(url, __LoadAssetFinish, meragePackage, delegate(float progress )
				{
					float curProgress = progress - oldProgress;
					oldProgress = progress;
					
					_totalDownLoadBytes += (int)(curProgress * packSize);
					if( _assetsDownLoadProgress != null ) _assetsDownLoadProgress(_totalDownLoadBytes);
				},
				_LoadAssetsError, 
				false);
            }
        }
		
		
		//体验包资源的下载
		if ( _assetPrefabQueue != null )
		{
			while( (_curDownloadingNum < MAX_DOWM_NUM_ONCE) && (_assetPrefabQueue.Count > 0 ) && !appQuit )
			{
				_curDownloadingNum++;
				ExperiencePack assetPackage = _assetPrefabQueue.Dequeue();
				if( assetPackage != null )
				{
					//使用本地的资源版本号作为数据版本
	                string url = _assetsTargetPath + miniPackage +  assetPackage.folderId + zipEX + "?ver=" + _assetsConfig.localAssetbundleVersionData.version ;
					
					uint packSize = assetPackage.size;
					//Debug.Log( url );
					float oldProgress = 0.0f;
	                HttpController.Instance.DownLoad(url, __LoadExperienceAssetFinshi, assetPackage, delegate(float progress )
					{
						float curProgress = progress - oldProgress;
						oldProgress = progress;
						
						_totalDownLoadBytes += (int)(curProgress * packSize);
						if( _assetsDownLoadProgress != null ) _assetsDownLoadProgress(_totalDownLoadBytes);
					},
					_LoadAssetsError, 
					false);					
				}
			}
		}
    }

    private void _LoadAssetsError( Exception e )
    {
        if (_assetbundleDownError != null) _assetbundleDownError(ErrorType.AssetsDownLoadError, e.StackTrace +  e.Message);
    }
	
	
    //释放下载中的引用
    private void DownLoadRelease()
    {
        _assetbundleDownLoadFinish = null;
        _assetbundleDownError      = null;
        _assetsDownLoadProgress    = null;
    }
	
	
	
	#region Common Asset Control	
    /// <summary>
    /// 加载资源完成
    /// </summary>
    /// <param name="bytes"></param>
    private void __LoadAssetFinish(byte[] bytes, object obj)
    {
        if ( _curDownloadingNum > 0 ) 
			_curDownloadingNum--;
		else
		{
			GameDebuger.Log( "=========== AssetbundleDownLoader Load number error ============" );
            return;
		}

        MergePackage meragePackage = obj as MergePackage;

        //AssetPrefab assetPrefab = obj as AssetPrefab;
        if (meragePackage != null)
        {
            _UpdateAsset(bytes, meragePackage);
        }
		
        _assetsConfig.curLoadU3DNumber++;

        GC.Collect();
        _DownLoad();
    }

    private void _UpdateAsset( byte[] bytes ,  MergePackage mergePackage )
    {
        MergeAssetParser parser = new MergeAssetParser(bytes);
        if (!parser.isCorrect)
        {
            Debug.LogError("Error : package - " + mergePackage.packageName + " is wrong file format ");
            return;
        }
        else
        {
            foreach (AssetPackage assetPackage in mergePackage.assetPackageList)
            {
                byte[] assetBuff = parser.GetAssetBuffs(assetPackage.assetPrefab.packagePos);

                string downPath = _downLoadPath + assetPackage.name + unity3dEx;

                //如果是不压缩， 进行解压
                if (!assetPackage.assetPrefab.needDecompress)
                {
                    assetBuff = ZipLibUtils.Uncompress(assetBuff);
                }
				
                SaveAsset(downPath, assetBuff);

                AddOneAssetsMessage(assetPackage);
            }
        }
    }

    /// <summary>
    /// Update One Asset Message
    /// </summary>
    /// <param name="assetPrefab"></param>
    private void AddOneAssetsMessage(AssetPackage assetPackage)
    {
        if (_assetsConfig.localAssetbundleVersionData != null)
        {
            if (!_assetsConfig.localAssetbundleVersionData.localAssetDic.ContainsKey(assetPackage.name))
            {
                _assetsConfig.localAssetbundleVersionData.localAssetDic.Add(assetPackage.name, new LocalAssetPrefab());
            }
            CopyAssetPrefab(assetPackage.assetPrefab, _assetsConfig.localAssetbundleVersionData.localAssetDic[assetPackage.name]);
        }
    }
	#endregion
	
	
	#region Experience Asset Control
	private void __LoadExperienceAssetFinshi(byte[] bytes, object obj)
	{
        if ( _curDownloadingNum > 0 ) 
			_curDownloadingNum--;
		else
		{
			GameDebuger.Log( "=========== AssetbundleDownLoader Load number error ============" );
            return;
		}

        ExperiencePack experiencePackage = obj as ExperiencePack;
		
		if( experiencePackage != null )
		{
			string assetsRootPath = AssetPathReferenceManager.Instance.GetFolderPathFormRefId( int.Parse( experiencePackage.folderId ));
			Dictionary< string , MemoryStream > mmsDitc = ZipLibUtils.UnZipFromByte( bytes,assetsRootPath, string.Empty );
			
			for( int i = 0 ; i < experiencePackage.assetPackageList.Count ; i ++ )
			{
				AssetPackage assetPackage = experiencePackage.assetPackageList[i];
				string assetPath =  assetPackage.name + unity3dEx;
				if( mmsDitc.ContainsKey( assetPath ))
				{
					_UpdateExpAsset(mmsDitc[assetPath].ToArray(), assetPackage );
				}
				else
				{
					Debug.LogError( string.Format( "AssetbundleDownloader < Can not get Asset's bytes : {0} > ", assetPackage.name ));
				}
			}
			
			//清空所有内存
			foreach( KeyValuePair<string, MemoryStream > item in mmsDitc )
			{
				item.Value.Close();
			}
		}
        _assetsConfig.curLoadU3DNumber++;

        GC.Collect();
        _DownLoad();		
	}
	
    private void _UpdateExpAsset( byte[] assetBuff ,  AssetPackage assetPackage )
    {
        string downPath = _downLoadPath + assetPackage.name + unity3dEx;

        //如果是不压缩， 进行解压
        if (!assetPackage.assetPrefab.needDecompress)
        {
            assetBuff = ZipLibUtils.Uncompress(assetBuff);
        }
		
        SaveAsset(downPath, assetBuff);

        AddOneAssetsMessage(assetPackage);
    }	
	
	
	#endregion
	
	
}
