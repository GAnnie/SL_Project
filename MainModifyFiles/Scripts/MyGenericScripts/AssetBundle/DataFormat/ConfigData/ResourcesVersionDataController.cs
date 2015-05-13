using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#region class Of AssetPackage
public class AssetPackage
{
    public string name;

    public AssetPrefab assetPrefab;

    public AssetPackage(string name, AssetPrefab assetPrefab)
    {
        this.name = name;

        this.assetPrefab = assetPrefab;
    }
}
#endregion

#region Class Of MergePackage
public class MergePackage
{
    public string packageName;

    public List< AssetPackage > assetPackageList;

    public MergePackage( string packageName )
    {
        this.packageName = packageName;

        this.assetPackageList = new List<AssetPackage>();
    }
	
	public uint size
	{
		get
		{
			uint _totalSize = 0;
			
			if( assetPackageList != null )
			{
				for( int i = 0 ; i < assetPackageList.Count; i ++ )
				{
					_totalSize += assetPackageList[i].assetPrefab.size;
				}
			}
			
			return _totalSize;
		}
	}
}

#endregion

public class MergePackageList
{
    public MergePackageList()
    {
    }
    
    Dictionary<string, MergePackage> meragePackageDic = null;

    /// <summary>
    /// 添加需要更新的资源信息
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="assetPrefab"></param>
    public void AddUpdateAsset( string assetName, AssetPrefab assetPrefab )
    {
        if (meragePackageDic == null)
        {
            meragePackageDic = new Dictionary<string, MergePackage>();
        }


        string packageName = assetPrefab.package + "/" + assetPrefab.packageNum ;
        if (!meragePackageDic.ContainsKey(packageName))
        {
            meragePackageDic.Add(packageName, new MergePackage(packageName));
        }

        _isAnaly = false;
        meragePackageDic[packageName].assetPackageList.Add(new AssetPackage(assetName, assetPrefab));
    }


    //是否已经尽心分析
    private bool _isAnaly = false;
    //保存更新队列
    Queue<MergePackage> meragePackageQueue = null;

    /// <summary>
    /// 分析包
    /// </summary>
    public int AnalyMessage()
    {
        if (meragePackageDic != null)
        {
            if (meragePackageQueue != null)
            {
                meragePackageQueue.Clear();
            }

            _isAnaly = true;
            meragePackageQueue = new Queue<MergePackage>();

            foreach (KeyValuePair<string, MergePackage> item in meragePackageDic)
            {
                meragePackageQueue.Enqueue(item.Value);
            }

            meragePackageDic.Clear();

            return meragePackageQueue.Count;
        }
        return 0;
    }


    public int GetPackageNumber()
    {
        if (!_isAnaly)
        {
            AnalyMessage();
        }

        return meragePackageQueue.Count;
    }

    /// <summary>
    /// 获取下一个资源包
    /// </summary>
    /// <returns></returns>
    public MergePackage NextPackage()
    {
        if (!_isAnaly)
        {
            AnalyMessage();
        }

        return meragePackageQueue.Dequeue();
    }

    /// <summary>
    /// 是否已经没有数据
    /// </summary>
    /// <returns></returns>
    public bool IsEnd()
    {
		if( meragePackageDic != null )
		{
			return (meragePackageQueue.Count == 0);
		}
        else
		{
			return true;
		}
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
		if( meragePackageDic != null )
		{
			meragePackageQueue.Clear();
		}
    }
 
}
