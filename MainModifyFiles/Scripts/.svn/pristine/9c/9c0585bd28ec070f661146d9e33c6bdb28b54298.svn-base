// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FreeAssetbundleDownLoader.cs
// Author   : wenlin
// Created  : 2013/12/24 
// Purpose  : 
// **********************************************************************

#define NEED_IMMEDIATE_STOP
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;


public class FreeAssetbundleDownLoader : AssetsLoader
{
	#region Enum of Memory State	
	private enum MemoryState
	{
		STATE_RESTRICT_DOWNLOAD = 0, //限制后台下载
		STATE_NORMAL_DOWNLOAD   = 1, //正常的下载状态
		STATE_LOW_MEMORY_DOWNLOAD = 2 //低内存的状态时的下载状态
	}
	
	//当前的下载状态
	private MemoryState _curBackStageDownLoadState = MemoryState.STATE_NORMAL_DOWNLOAD;

	#if UNITY_IPHONE
	//正常的内存情况
	private int normal_memory = 30;
	//低内存情况
	private int low_memory    = 20;
	#else
	//正常的内存情况
	private int normal_memory = 70;
	//低内存情况
	private int low_memory    = 45;
	#endif

	
	#endregion
		
	
	//收集资源的过滤器
	private CollectMissingAssetsFilter _missingFilter = null;
	
	
	public FreeAssetbundleDownLoader (AssetsConfig config, AssetsExistMsg existMsg, string remoteVersionPath, string remoteResPath, string downLoadPath) : 
		base( config, existMsg, remoteVersionPath, remoteResPath, downLoadPath)
	{
		_missingFilter = new CollectMissingAssetsFilter( config, existMsg );
		_missingAssetsList = new List<string>();

//		List< string > exportList = ExperiencePathManager.ImportPrestrainExperienceData( ExperiencePathManager.exportPrestrainPath );
//		if( exportList != null && exportList.Count > 0 )
//		{
//
//			for( int index = 0 , imax = exportList.Count ; index < imax ; index ++ )
//			{
//				CollectMissingAssets( exportList [ index ]   );
//			}
//		}
	}
	
    //检查资源版本信息
    public override void CheckVersion(Action checkVersionFinishFunc, Action<ErrorType, string> errorFunc = null){}
	
	
	
	#region BackStage Download	
	//是否当前已启动后台加载器
	private bool _isBackStageLoaderStart = false;
	private bool _isBackStageDownloading = false;
	
	//存储当前丢失资源的下载队列
	private List< string > _missingAssetsList = null;
	
	//后台定时器
	private MonoTimer _backStageMonoTimer = null;
	
	//后台加载数据的备份
	private byte[] _backStageBackUpByte	= null;
	private object _backStageBackUpObj		= null;	
	
	/// <summary>
	/// 开启后台加载器
	/// </summary>
	public bool StartBackStageLoader()
	{
		if( _isBackStageLoaderStart )
		{
			return true;
		}

		_isDownLoading = true;

		_isBackStageLoaderStart = true;
		
		//如果上一次停止加载的数据存在备份， 则先处理备份的信息
		if( _backStageBackUpByte != null && _backStageBackUpObj != null )
		{
			_BackStageDownloadFinish( _backStageBackUpByte, _backStageBackUpObj );
		}
		
		if( _backStageMonoTimer == null )
		{
			_backStageMonoTimer = TimerManager.GetTimer( "BackStageLoader" );
		}
		
		_backStageMonoTimer.Setup2Time(10, _DownLoadMissingAssets );
		_backStageMonoTimer.Play();
		
		_curBackStageDownLoadState = MemoryState.STATE_NORMAL_DOWNLOAD;
		
		return true;
	}
	
	/// <summary>
	/// 关闭后台加载器
	/// </summary>
	public bool StopBackStageLoader()
	{
		if( !_isBackStageLoaderStart )
		{
			return true;
		}
		
		_isBackStageLoaderStart = false;
		
		//停止后台加载
		if( _backStageMonoTimer != null )
		{
			_backStageMonoTimer.Stop();
		}

		return true;
	}
	
	
	/// <summary>
	/// Collects the missing assets.
	/// </summary>
	public void CollectMissingAssets( string assetName )
	{
		//string refId = string.Empty;
		//GameDebuger.Log (  "collectMissingAssets ............................. :" + assetName );
		string refId = _missingFilter.Filter( assetName );
		if( string.IsNullOrEmpty( refId ) )	return;
		
		for( int index = _missingAssetsList.Count - 1; index >= 0 ; index -- )
		{
			if( _missingAssetsList[ index ] == refId )
			{
				_missingAssetsList.RemoveAt( index );
			}
		}
		_missingAssetsList.Add( refId );

	}
	

	//检查当前内容的情况而做判断
	private int delaynum = 0;
	private bool CheckMemory()
	{
		if (SystemSetting.IsMobileRuntime)
		{
			long memory = BaoyugameSdk.getFreeMemory() / 1024;	
			
			//如果当前内存达到70m， 则开始正常的后台下载状态
			if (memory >= normal_memory)
			{
				if( _curBackStageDownLoadState != MemoryState.STATE_NORMAL_DOWNLOAD )
				{
					delaynum = 0;
					_curBackStageDownLoadState = MemoryState.STATE_NORMAL_DOWNLOAD;
					
#if UNITY_EDITOR || UNITY_DEBUG
					TipManager.AddTip( "开启正常后台下载模式" );				
#endif
				}
			}
			
			//如果是低内存的模式下， 后台下载时间延长， 避免密集的下载到时游戏闪退
			else if( memory >= low_memory )
			{
				if( _curBackStageDownLoadState != MemoryState.STATE_LOW_MEMORY_DOWNLOAD )
				{
					delaynum = 0;
					_curBackStageDownLoadState = MemoryState.STATE_LOW_MEMORY_DOWNLOAD;
					
#if UNITY_EDITOR || UNITY_DEBUG
					TipManager.AddTip( "开启低内存后台下载模式" );				
#endif					
				}
				
				delaynum ++;
				if( delaynum <= 3 )
				{
					return false;
				}
				else
				{
					delaynum = 0;
					return true;
				}
			}
			
			//如果低于限制， 直接返回， 不进行任何的加载
			else
			{
				_curBackStageDownLoadState = MemoryState.STATE_RESTRICT_DOWNLOAD;
#if UNITY_EDITOR || UNITY_DEBUG
					TipManager.AddTip( "内存过低， 停止后台下载" );				
#endif				
				return false;
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// 定时下载缺失的资源
	/// </summary>
	private void _DownLoadMissingAssets()
	{
		//如果非wifi环境下， 则直接跳过
//		if( !Utility.IsWifiState())
//		{
//			return;
//		}

		//TipManager.AddWhiteTip( "只有是wifi环境下才会执行" );

		//如果当前没有资源下载， 则直接返回
		if( _missingAssetsList.Count <= 0 )
		{
			return;
		}
		
		//如果后台正在下载中
		if( _isBackStageDownloading )
		{
			return;
		}

		
		//如果MapData正在检查资源中， 则直接返回
//		if( MapAreasDataController.Instance.isChecking )
//		{
//			return;
//		}
		
		
		//检查当前内存情况
		if( !CheckMemory())
		{
			return;
		}
		
#if UNITY_EDITOR
		if( _curBackStageDownLoadState == MemoryState.STATE_LOW_MEMORY_DOWNLOAD )
		{
			TipManager.AddTip( " 开始后台下载(low memory)" );	
		}
		else
		{
			TipManager.AddTip( " 开始后台下载(Normal)" );	
		}	
#endif
		//寻找满足条件的下载
		while( _missingAssetsList.Count > 0 )
		{
			string refId = _missingAssetsList[ 0 ];
			
			Dictionary< string, AssetPrefab > assetPrefabDict = _CollectSamePacageAssets( refId );

			_missingAssetsList.RemoveAt( 0 );

			GameDebuger.Log( "_missingAssetsLis>>>>>>>>>>>>>>>Count: " + _missingAssetsList.Count.ToString() );
			GameDebuger.Log( "refid................................"+ refId );

			if( assetPrefabDict == null )
			{
				continue;
			}
			else
			{
				MergePackageList backstageMergePackageList = new MergePackageList();
				
				foreach( KeyValuePair< string, AssetPrefab> item in assetPrefabDict )
				{
					string assetPath = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );
					if( string.IsNullOrEmpty( assetPath ))
					{
						GameDebuger.Log( string.Format( "FreeAssetbundleDownLoader < Get refid : {0} error > ", item.Key ));
						continue;
					}

					backstageMergePackageList.AddUpdateAsset(assetPath, item.Value );					
				}
				
				int downloadNum =  backstageMergePackageList.AnalyMessage();
				if( downloadNum != 0 )
				{
					_SetBackStageStart();
					DownLoadBackstage( backstageMergePackageList );
				}
				
				break;
			}
		}
	}
	
	/// <summary>
	/// 设置开始后台下载
	/// </summary>
	private void _SetBackStageStart()
	{
		_isBackStageDownloading = true;
	}
	
	/// <summary>
	/// 设置停止后台加载
	/// </summary>
	private void _SetBackStageStop()
	{
		_isBackStageDownloading = false;
	}
	
	/// <summary>
	/// 开始后台下载资源
	/// </summary>
	private void DownLoadBackstage(MergePackageList backstageMergePackageList )
	{
        if ( backstageMergePackageList != null)
        {
            MergePackage meragePackage = backstageMergePackageList.NextPackage();
			string url = _assetsResPath + meragePackage.packageName + ".baoyu";
			GameDebuger.Log( "FreeAssetbundleDownLoader---------------------------" + meragePackage.packageName );
			uint packSize = meragePackage.size;
            HttpController.Instance.DownLoad(url, _BackStageDownloadFinish, meragePackage, null, _LoadAssetsError, false);
        }
	}
	
	/// <summary>
	/// 下载完成
	/// </summary>
	private void _BackStageDownloadFinish( byte[] bytes, object obj )
	{
		//如果当前后台已经停止， 则备份下来， 等一下开启的时候进行处理
		if( !_isBackStageLoaderStart )
		{
			_backStageBackUpByte = bytes;
			_backStageBackUpObj  = obj;
		}
		else
		{
			_backStageBackUpByte = null;
			_backStageBackUpObj  = null;
			
	        MergePackage meragePackage = obj as MergePackage;
	        if (meragePackage != null)
	        {
	            _UpdateAsset(bytes, meragePackage, false);
	        }		
		}
		
		_SetBackStageStop();
	}
	
	#endregion
	
	
	#region FrontStage DownLoad
	private bool isInit = false;
	private MergePackageList _mergePackageList = null;
	/// <summary>
	/// 从服务器里面下载制定的资源
	/// </summary>
	/// <param name='assetsRefIdList'>
	/// 制定资源的列表， 里面保存的是资源的引用ID
	/// </param>
	/// <param name='allFinishCallBack'>
	/// All finish call back.
	/// </param>
	/// <param name='progressFunc'>
	/// Progress func.
	/// </param>
	/// <param name='errorCallBack'>
	/// Error call back.
	/// </param>
	public void DownLoadAssets( List< string > assetsRefIdList, 
								 Action allFinishCallBack = null, 
								 Action<int> progressFunc = null, 
								 Action<ErrorType, string> errorCallBack = null)
	{
		if( _assetsExistMsg == null )
		{
			errorCallBack( ErrorType.AssetIOError, string.Format( "FreeAssetbundleDownLoader < AssetExistMessage is null  >" ));
			return;			
		}
		
		//关闭后台下载
		StopBackStageLoader();	
		
		List< string > doneRefIDList = new List<string>();
		
		_mergePackageList = new MergePackageList();
		
		Dictionary< string, AssetPrefab > assetPrefabDict = new Dictionary<string, AssetPrefab>();
		foreach( string refId in assetsRefIdList )
		{
			//如果已经处理过， 则直接跳过
			if( doneRefIDList.Contains( refId ))
			{
				continue;
			}
			doneRefIDList.Add( refId );
			
			if( _missingFilter.CheckRefIdValid( refId ) )
			{
				assetPrefabDict.Add( refId, _assetsExistMsg.missingMessage.missingAssets[refId]);
			}
			else
			{
				GameDebuger.Log( string.Format(" FreeAssetbundleDownLoader < Can not get Asset ref id {0}, path : {1} >" , 
												refId,
												AssetPathReferenceManager.Instance.GetAssetPath(refId) )); 
			}
		}
		
		//收集相同资源内容
		_CollectSamePacageAssets( assetPrefabDict );
		
		//开始分析资源
		_assetsConfig.curLoadU3DNumber = 0;
		_assetsConfig.totalU3DNumber  = 0;
		foreach( KeyValuePair< string, AssetPrefab > item in assetPrefabDict )
		{
			string assetPath = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );
			if( string.IsNullOrEmpty( assetPath ))
			{
				GameDebuger.Log( string.Format( "FreeAssetbundleDownLoader < Get refid : {0} error > ", item.Key ));
				continue;
			}
			
			_mergePackageList.AddUpdateAsset(assetPath, item.Value );
			
			_assetsConfig.totalU3DNumber ++;
		}
		_mergePackageList.AnalyMessage();
		
		isInit = true;
		
		DownLoad( allFinishCallBack, progressFunc, errorCallBack );
	}
	
	
    //加载资源
    public override void DownLoad(Action allFinishCallBack = null, Action<int> progressFunc = null, Action<ErrorType, string> errorCallBack = null)
	{
		if( !isInit || _mergePackageList == null)
		{
			GameDebuger.Log( string.Format("FreeAssetbundleDownLoader < Loader is not initailization >"));
			return ;
		}
		
		_assetbundleDownLoadFinish = allFinishCallBack;
        _assetbundleDownError      = errorCallBack;
        _assetsDownLoadProgress    = progressFunc;
		
		BeginDownLoad();
		
		_DownLoad();
	}

	
    //最大同时下载的数量
#if UNITY_EDITOR
    private int MAX_DOWM_NUM_ONCE = 3;
#else
	private int MAX_DOWM_NUM_ONCE = 2;
#endif	

    //当前下载的数量
    private int _curDownloadingNum = 0;
    
    private const string unity3dEx = ".unity3d";
	
    /// <summary>
    /// 现在资源
    /// </summary>
    private void _DownLoad()
    {
		
#if NEED_IMMEDIATE_STOP			
		//强制停止
		if( _isForceStop )
		{
			return;
		}
#endif		
		
        //显示回调信息
        if (_assetsDownLoadProgress != null)
        {
            int percent = ((_assetsConfig.curLoadU3DNumber) * 100 / _assetsConfig.totalU3DNumber);
            if (percent > 100) percent = 100;
			
            _assetsDownLoadProgress(percent);
        }

        if (_assetsConfig.curLoadU3DNumber >= _assetsConfig.totalU3DNumber || ( _isForceStop && _curDownloadingNum <= 0 ) )
        {
			Finish();
        }

        if ( _mergePackageList != null)
        {
            while ((_curDownloadingNum < MAX_DOWM_NUM_ONCE) && !_mergePackageList.IsEnd() && !_isForceStop)
            {
                _curDownloadingNum++;

                MergePackage meragePackage = _mergePackageList.NextPackage();
				string url = _assetsResPath + meragePackage.packageName + ".baoyu";
				uint packSize = meragePackage.size;
				float oldProgress = 0.0f;
                HttpController.Instance.DownLoad(url, __LoadAssetFinish, meragePackage, null, _LoadAssetsError, false);
            }
        }
    }
	
	
	private MonoTimer _timer = null;
	public void StopDownLoad()
	{
		_isForceStop = true;
		
#if NEED_IMMEDIATE_STOP		
		//立马停止
		ForceFinish();
		
#else
		//强制停止的时候，此时可能会出现一个问题， 如果网络比较繁忙， 这是可能资源一直无法下载成功， 这是需要一个超时强制停止
		//的机制， 如果超过 2分钟都没有下载完成 ， 会强制停止并调用放回
		
		if( _timer == null )
		{
			_timer = TimerManager.GetTimer( "FreeDownloader" );
			_timer.Setup2Time( 2 * 60 , ForceFinish );
			_timer.Play();
		}
#endif
	}
	
	//强制停止			
	private void ForceFinish()
	{
		Finish();
	}
	
	public void Finish()
	{
		if( !_isDownLoading )
			return ;
		
		//停止强制退出
		if( _timer != null )
		{
			_timer.Stop();
			_timer = null;
		}
		
        //结束加载
        EndDownLoad();

        //完成加载版本
        SaveAssetbundleVersionData();

        //完成所有资源下载
		if (_assetbundleDownLoadFinish != null) _assetbundleDownLoadFinish();
		
        //释放引用
        DownLoadRelease();		
		
		//开启后台下载
		StartBackStageLoader();
	}
	
	
    private void BeginDownLoad()
    {
		_isForceStop = false;
		_isDownLoading = true;
    }

    /// <summary>
    /// 结束加载
    /// </summary>
    private void EndDownLoad()
    {
		_isDownLoading = false;
        _mergePackageList.Dispose();
        _mergePackageList = null;
    }	
	
    //释放下载中的引用
    private void DownLoadRelease()
    {
        _assetbundleDownLoadFinish = null;
        _assetbundleDownError      = null;
        _assetsDownLoadProgress    = null;
    }	
	
	
	
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
		
#if NEED_IMMEDIATE_STOP			
		//强制停止
		if( _isForceStop )
		{
			return;
		}
#endif		
		
        MergePackage meragePackage = obj as MergePackage;

        //AssetPrefab assetPrefab = obj as AssetPrefab;
        if (meragePackage != null)
        {
            _UpdateAsset(bytes, meragePackage);
        }
		
        
        _DownLoad();
    }

    private void _UpdateAsset( byte[] bytes ,  MergePackage mergePackage, bool updateloadNumber = true )
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
				
				if( updateloadNumber )
				{
					_assetsConfig.curLoadU3DNumber++;
				}
				else
				{

#if UNITY_EDITOR
					GameDebuger.Log( string.Format(" 后台下载完成 : {0}", assetPackage.name ) );
					TipManager.AddTip( string.Format(" 后台下载完成 : {0}", assetPackage.name ));
#endif
				}
            }
        }
    }
	
	#endregion
	
	/// <summary>
	/// 收集RefId所在的同一个包里面的内容
	/// </summary>
	/// <returns>
	/// 如果当前refid所在的资源已经存在，则直接放回null, 如果不存在， 则把和refid同属同一个资源包里面的所有资源都收集起来
	/// Key为RefId
	/// </returns>	
	private Dictionary< string, AssetPrefab > _CollectSamePacageAssets( string refId )
	{
		if( !_missingFilter.CheckRefIdValid( refId ))
		{
			return null;
		}	
		
		AssetPrefab assetPrefab = _assetsExistMsg.missingMessage.missingAssets[refId];
		if( assetPrefab == null )
		{
			return null;
		}				
		
		Dictionary< string, AssetPrefab > assetPrefabDict = new Dictionary<string, AssetPrefab>();
		assetPrefabDict.Add( refId, assetPrefab );
		
		_CollectSamePacageAssets( assetPrefabDict );		
	
		return assetPrefabDict;
	}
	
	/// <summary>
	/// 收集RefId所在的同一个包里面的内容
	/// </summary>
	/// <returns>
	/// 如果当前refid所在的资源已经存在，则直接放回null, 如果不存在， 则把和refid同属同一个资源包里面的所有资源都收集起来
	/// Key为RefId
	/// </returns>
	private void _CollectSamePacageAssets( Dictionary< string, AssetPrefab > refDict )
	{
		if( refDict == null )
		{
			return ;
		}
		
		List< string > deleteList = new List<string>();
		List< string > exitsPackList = new List<string>();
		Dictionary< string, AssetPrefab > newAddPrefab = new Dictionary<string, AssetPrefab>();
		foreach( KeyValuePair< string, AssetPrefab > refItem in refDict )
		{
			string refId = refItem.Key;
			
			//如果当前Id并不是缺失资源， 则直接返回
			if( !_missingFilter.CheckRefIdValid( refId ))
			{
				deleteList.Add( refId );
				continue;
			}
			
			AssetPrefab assetPrefab = refItem.Value;
			if( assetPrefab == null )
			{
				continue;
			}			
			
			//过滤相同Pack的资源
			string pack = assetPrefab.package + assetPrefab.packageNum;
			if( exitsPackList.Contains( pack ))
			{
				continue;
			}
			exitsPackList.Add( pack );
			
			
			//查找存在于相同包的资源
			foreach( KeyValuePair< string, AssetPrefab > item in _assetsExistMsg.missingMessage.missingAssets )
			{
				if( refId == item.Key )
				{
					continue;
				}
				
				if( item.Value.package == assetPrefab.package && item.Value.packageNum == assetPrefab.packageNum )
				{
					if( !refDict.ContainsKey( item.Key ))
					{
						newAddPrefab.Add( item.Key, item.Value );
					}
				}
			}
		}
		
		exitsPackList.Clear();
		if( deleteList.Count > 0 )
		{
			foreach( string deleteRefId in deleteList )
			{
				refDict.Remove( deleteRefId );
			}
		}
		
		foreach( KeyValuePair< string, AssetPrefab > item in newAddPrefab )
		{
			refDict.Add( item.Key, item.Value );
		}

		
		//AssetPrefab assetPrefab = _assetsExistMsg.missingMessage.missingAssets[refId];
//		if( assetPrefab == null )
//		{
//			return null;
//		}
		
		//加入下载列表
//		Dictionary< string, AssetPrefab > downloadDict = new Dictionary<string, AssetPrefab>();
//		downloadDict.Add( refId, assetPrefab );
		
		//查找存在于相同包的资源
//		foreach( KeyValuePair< string, AssetPrefab > item in _assetsExistMsg.missingMessage.missingAssets )
//		{
//			if( refId == item.Key )
//			{
//				continue;
//			}
//			
//			if( item.Value.package == assetPrefab.package && item.Value.packageNum == assetPrefab.packageNum )
//			{
//				downloadDict.Add( item.Key, item.Value );
//			}
//		}
//		return refDict;
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
			
			string refID = AssetPathReferenceManager.Instance.GetAssetPathRefId( assetPackage.name );
			
			if( string.IsNullOrEmpty( refID ))
			{
				GameDebuger.Log( string.Format("FreeAssetbundleDownLoader < Can not Get RefID : {0} >", assetPackage.name ));
			}
			
			if( _assetsExistMsg.missingMessage.missingAssets.ContainsKey( refID ))
			{
				_assetsExistMsg.missingMessage.missingAssets.Remove( refID );
			}
        }
    }	
	
    private void _LoadAssetsError( Exception e )
    {
        if (_assetbundleDownError != null) _assetbundleDownError(ErrorType.AssetsDownLoadError, e.StackTrace +  e.Message);
    }
	
}

