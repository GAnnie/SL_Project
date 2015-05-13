using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public abstract class AssetsLoader 
{
    //信息回调代理
    protected Action _assetbundleDownLoadFinish = null;
    protected Action _assetbundleCheckVersionFinish = null;

    protected Action<ErrorType, string> _assetbundleDownError = null;
    protected Action<ErrorType, string> _assetbundleCheckVersionError = null;

    //加载进度回调
    protected Action<int> _assetsDownLoadProgress = null;

    //资源存放目录
    protected string _downLoadPath = String.Empty;//Application.persistentDataPath + "/";
    //版本文件本地存放目录
    public const string _assetVerCtrl = "ResourceData.bytes";
	//本地的版本文件
	public const string _localAssetVerCtrl = "ResourceVersionData.bytes";
	
    //版本号的地址
    protected string _assetsTargetPath = String.Empty;
	//客户端静态资源的地址
	protected string _assetsResPath = String.Empty;

    //是否已经初始化
    protected bool _isInit = false;
    //是否正在加载中
    protected bool _isDownLoading = false;
	//是否强行停止
	protected bool _isForceStop = false;
    //是否退出程序
    public bool appQuit = false;
    //是否加载遇到错误
    protected bool _downLoadError = false;
    //参数配置
    protected AssetsConfig _assetsConfig = null;
	//缺失资源
	protected AssetsExistMsg _assetsExistMsg = null;
	
    private const string unity3dEx = ".unity3d";

    //当前版本配置文件的版本号 ，如果本地版本信息低于此版本
    protected long _curVersionDataVer = -1;

    //构造函数
	public AssetsLoader( AssetsConfig config , AssetsExistMsg existMsg,  string remoteVersionPath, string remoteResPath, string downLoadPath)
    {
        _assetsConfig     = config;
        _assetsExistMsg   = existMsg;
		_downLoadPath     = downLoadPath;
        _assetsTargetPath = remoteVersionPath;
		_assetsResPath    = remoteResPath;
    }


    /// 关闭的时候调用
    public void Dispose()
    {
        appQuit = true;

        //保存当前的资源
        if (_isDownLoading) SaveAssetbundleVersionData();
    }

    /// 获取本地版本数据
    /// <returns>返回本地资源数据</returns>
    protected void GetLocalAssetbundleVersionData()
    {
        if (_assetsConfig != null && _assetsConfig.localAssetbundleVersionData == null)
        {
            GameDebuger.Log(" GetLocalAssetbundleVersionData ");
            string localAssetCtrlPath = _downLoadPath + _localAssetVerCtrl;
            _assetsConfig.GetLocalAssetbundleVersionData(localAssetCtrlPath);
        }
    }

    /// 获取远程数据为 ResouresVersionData
    /// <param name="bytes"></param>
    /// <returns></returns>
    protected ResourcesVersionData GetRometeResourcesVersionData(byte[] bytes)
    {
		Dictionary<string, object> obj = DataHelper.GetJsonFile(bytes);
        JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
        return parser.DeserializeJson_ResourcesVersionData(obj);
    }
	
	
	private const string remoteResVerDataFile = "Set/v.bytes";
	protected ResourcesVersionData GetRometeResourcesVersionData()
	{
		byte[] bytes  = DataHelper.GetFileBytes( _downLoadPath + remoteResVerDataFile );
		return GetRometeResourcesVersionData( bytes );
	}
	
	protected void SaveRometeResourcesVersionData( byte[] bytes)
	{
		DataHelper.SaveFile( _downLoadPath + remoteResVerDataFile, bytes );
	}
	

    /// <summary>
    /// Create assign directory
    /// </summary>
    /// <param name="path"></param>
    protected void CheckAndCreateDirectory(string path)
    {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Copy Asset Prefab
    /// </summary>
    /// <param name="source"></param>
    /// <param name="desc"></param>
    protected void CopyAssetPrefab(AssetPrefab source, LocalAssetPrefab desc)
    {
        desc.needDecompress = source.needDecompress;
        desc.version = source.version;
        desc.isLocalPack = source.isLocalPack;
    }

    /// <summary>
    /// 完成加载信息
    /// </summary>
    protected void FinishAssetsBundleVersionData()
    {
        if (_assetsConfig.localAssetbundleVersionData != null)
        {
            if (_curVersionDataVer >= 0)
            {
                GameDebuger.Log(" Save Cur Version Data Ver : " + _curVersionDataVer);

                //保存当前的版本号信息
                _assetsConfig.localAssetbundleVersionData.version = _curVersionDataVer;
            }

            _assetsConfig.localAssetbundleVersionData.firstDownLoadFinish = false;
            _assetsConfig.isFirstDownLoad = false;
            SaveAssetbundleVersionData();
        }
    }


    /// 保存<LocalAssetsVersionData>信息
    /// <param name="bytes"></param>
    protected void SaveAssetbundleVersionData()
    {
        if ( _assetsConfig != null && _assetsConfig.localAssetbundleVersionData != null)
        {
            string asssetCtrlPath = _downLoadPath + _localAssetVerCtrl;

            JsonLocalAssetsVersionDataParser parser = new JsonLocalAssetsVersionDataParser();
            Dictionary<string, object> obj = parser.SerializeJson_LocalAssetsVersionData(_assetsConfig.localAssetbundleVersionData);

			DataHelper.SaveJsonFile(obj, asssetCtrlPath);

            //Utility.SaveObjectAsXML<LocalAssetsVersionData>(asssetCtrlPath, _localAssetbundleVersionData);
        }
    }

    /// 保存文件信息
    /// <param name="path"></param>
    /// <param name="stream"></param>
    protected void SaveAsset(string path, MemoryStream stream)
    {
        SaveAsset(path, stream.GetBuffer());
    }


    protected void SaveAsset(string path, byte[] bytes)
    {
        string dir = path.Substring(0, path.LastIndexOf("/"));
        CheckAndCreateDirectory(dir);
        try
        {
            if (File.Exists(path)) File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(bytes);
            writer.Close();
            fs.Close();

			iOSUtility.ExcludeFromBackupUrl(path);
        }
        catch (IOException e)
        {
            _assetbundleDownError(ErrorType.AssetIOError, "File Write Error : " + e.Message);
        }

        //GC.Collect();
    }


    //检查资源版本信息
    public abstract void CheckVersion(Action checkVersionFinishFunc, Action<ErrorType, string> errorFunc = null);


    //加载资源
    public abstract void DownLoad(Action allFinishCallBack = null, Action<int> progressFunc = null, Action<ErrorType, string> errorCallBack = null);

}
