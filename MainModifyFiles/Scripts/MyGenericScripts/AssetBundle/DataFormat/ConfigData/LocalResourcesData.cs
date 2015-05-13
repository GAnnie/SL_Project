using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LocalAssetsVersionData
{
    //本地资源文件版本号
    public long version = 0;

    public Dictionary< string , LocalAssetPrefab> localAssetDic = null;

    public Dictionary<string, string> scenes = null;

    public List< string > commonObjs = null;

    public bool firstDownLoadFinish = true;

    public LocalAssetsVersionData()
    {

    }

    public LocalAssetsVersionData( int newOne)
    {
        scenes = new Dictionary<string, string>();

        commonObjs = new List<string>();

        localAssetDic = new Dictionary<string, LocalAssetPrefab>();
    }
}

public class LocalAssetPrefab
{
    //资源版本号
    public long version = 0;

    //是否压缩
    public bool needDecompress = false;

    //是否读取本地的资源
    public bool isLocalPack = false;

    public LocalAssetPrefab()
    {}

    public LocalAssetPrefab( long version , bool needDecompress, bool isLocalPack)
    {
        this.version        = version ;

        this.needDecompress = needDecompress;

        this.isLocalPack = isLocalPack;
    }
}