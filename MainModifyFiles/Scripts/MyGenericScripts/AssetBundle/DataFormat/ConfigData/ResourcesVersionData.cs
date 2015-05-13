using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesVersionData 
{
    //版本号
    public long version = 0;

    /*****************************************************************
     * 资源信息保存使用Dictionary来保存， 其中key : String 格式为guid
     * 其中数据需要读取指定名字列表来完成
     * 
     *****************************************************************/
    public Dictionary< string , AssetPrefab > assetPrefabs = null;
    
    public Dictionary< string , AssetPrefab > scenes       = null;

    public Dictionary< string , AssetPrefab > commonObjs   = null;
	
	
    public AssetsPathTreeRoot root;

    public ResourcesVersionData()
    { }


    public ResourcesVersionData( int date )
    {
        assetPrefabs = new Dictionary<string, AssetPrefab>();

        scenes       = new Dictionary<string, AssetPrefab>();

        commonObjs   = new Dictionary<string, AssetPrefab>();
    }
}

public class ResourcesMissingMessage
{
	//总共的资源
	public int totalGameResNumber;
	
	//丢失的资源
	public Dictionary< string , AssetPrefab > missingAssets = null;
	
	public ResourcesMissingMessage()
	{
		
	}
	
	public ResourcesMissingMessage( int data )
	{
		missingAssets = new Dictionary<string, AssetPrefab>();
	}
}


public class AssetPrefab
{
    //资源版本号
    public long    version = 0;

    //////资源的地址， 格式用1:3:4表示， 读取地址类来生成
    //public string path;

    //////资源的名字
    //public string name;

    //大包目录
    public string package = string.Empty;

    //大包编号
    public int packageNum = 0;

    //在大包中的位置
    public int packagePos = 0; 

    //是否进行需要压缩
    public bool needDecompress = false;

    //是否读取本地的资源
    public bool isLocalPack = false;
	
	//是否体验包资源
	public byte isEp = 0;
		
    //文件大小
    public uint size = 0;

    public AssetPrefab()
    {}
}
