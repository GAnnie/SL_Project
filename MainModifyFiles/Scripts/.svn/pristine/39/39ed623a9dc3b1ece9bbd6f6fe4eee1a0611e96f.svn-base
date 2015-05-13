using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class AssetsConfig 
{
    //是否第一次更新资源
    public bool isFirstDownLoad = false;

    //当前加载的资源数
    public int curLoadU3DNumber = 0;

    //总共要加载的资源数量
    public int totalU3DNumber = 0;

    //总共加载资源的大小
    public uint totalUpdateAssetsSize = 0;

    //记录COMMON OBJECT 目录
   // private List<string> _commonObjectList = null;

    //记录场景信息
    //private Dictionary<string, string> _scenesDic = null;

    //本地版本号信息
    public LocalAssetsVersionData localAssetbundleVersionData = null;

    public AssetsConfig()
    {
        //localAssetbundleVersionData = new LocalAssetsVersionData();
    }
	
	public void Reset()
	{
	    //当前加载的资源数
	    curLoadU3DNumber = 0;
	
	    //总共要加载的资源数量
	    totalU3DNumber = 0;
	
	    //总共加载资源的大小
	    totalUpdateAssetsSize = 0;		
	}
	
    /// 获取AssetPrefab资源
    /// <param name="name"> 资源名称</param>
    /// <returns></returns>
    public LocalAssetPrefab GetAsset(string name)
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("GetAsset localAssetbundleVersionData is NULL ");
            return null;
        }

        if (IsContainAsset(name))
        {
            return localAssetbundleVersionData.localAssetDic[name];
        }
		else
		{
        	return null;
		}
    }

    /// 获取Common Object 列表
    /// <returns></returns>
    public List<string> GetCommonObjects()
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("GetCommonObjects localAssetbundleVersionData is NULL ");
            return null;
        }

        return localAssetbundleVersionData.commonObjs;
    }

    /// 获取场景 地址
    /// <param name="sceneID"></param>
    /// <returns></returns>
    public string GetScensPath(string sceneID)
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("GetScensPath localAssetbundleVersionData is NULL ");
            return string.Empty;
        }

        if (localAssetbundleVersionData.scenes != null && localAssetbundleVersionData.scenes.ContainsKey(sceneID))
        {
            return localAssetbundleVersionData.scenes[sceneID];
        }

        return string.Empty;
    }

    /// 是否包含资源
    /// <param name="name"></param>
    /// <returns></returns>
	/// 
    public bool IsContainAsset(string name, bool isCollectAssets = true)
    {
        if (localAssetbundleVersionData == null)
        {
            //GameDebuger.Log("IsContainAsset localAssetbundleVersionData is NULL ", false);
            return false;
        }

         if (localAssetbundleVersionData.localAssetDic.ContainsKey(name))
        {
            return true;
        }
        else
        {
			if( isCollectAssets )
			{
				//收集缺失的资源
				AssetbundleManager.Instance.Collect( name );
			}
			
	        //GameDebuger.Log( "IsContainAsset :" + name + " not found ", false);
            return false;
        }
    }

    /// 是否包含场景
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public bool IsContainScene(string sceneName)
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("IsContainScene localAssetbundleVersionData is NULL ");
            return false;
        }

        if (localAssetbundleVersionData.scenes.ContainsKey(sceneName))
        {
            return true;
        }
        else
        {
            GameDebuger.Log( "IsContainScene :" + sceneName + "not found ");
            return false;
        }
    }


    /// 获取公用资源的引用
    /// <param name="remoteassetBudleVersionData"></param>
    public void InitCommonObject(ResourcesVersionData versionData)
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("InitCommonObject localAssetbundleVersionData is NULL ");
            return;
        }

        //Get Atlas Name
        if (localAssetbundleVersionData.commonObjs == null)
        {
            localAssetbundleVersionData.commonObjs = new List<string>();
        }

        foreach (KeyValuePair<string, AssetPrefab> item in versionData.commonObjs)
        {
			string path = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );
            if (!localAssetbundleVersionData.commonObjs.Contains(path))
            {
                localAssetbundleVersionData.commonObjs.Add(path);
            }
        }
    }


    /// 记录共用场景信息
    /// <param name="versionData"></param>
    public void InitScenes(ResourcesVersionData versionData)
    {
        if (localAssetbundleVersionData == null)
        {
            GameDebuger.Log("InitScenes localAssetbundleVersionData is NULL ");
            return;
        }

        if (localAssetbundleVersionData.scenes == null)
        {
            localAssetbundleVersionData.scenes = new Dictionary<string, string>();
        }

        foreach (KeyValuePair<string, AssetPrefab> item in versionData.scenes)
        {
            string scenePath = AssetPathReferenceManager.Instance.GetAssetPath( item.Key );
			string sceneID = scenePath;
            int index = sceneID.LastIndexOf("/");
            if (index != -1)
            {
                sceneID = sceneID.Substring(sceneID.LastIndexOf("/") + 1);

                if (!localAssetbundleVersionData.scenes.ContainsKey(sceneID))
                {
                    localAssetbundleVersionData.scenes.Add(sceneID, scenePath);
                }
            }
            else
            {
                if (!localAssetbundleVersionData.scenes.ContainsKey(scenePath))
                {
                    localAssetbundleVersionData.scenes.Add(scenePath, scenePath);
                }
            }
        }
    }

    /// 获取本地版本数据
    /// <returns>返回本地资源数据</returns>
    public void GetLocalAssetbundleVersionData(string localAssetCtrlPath )
    {
		if( localAssetbundleVersionData != null )
		{
			return ;
		}
		
        if (!File.Exists(localAssetCtrlPath))
        {
            localAssetbundleVersionData = new LocalAssetsVersionData(1);
        }
        else
        {
            try
            {
                FileStream fs = new FileStream(localAssetCtrlPath, FileMode.Open);
                BinaryReader reader = new BinaryReader(fs);
                byte[] bytes = reader.ReadBytes((int)fs.Length);

                reader.Close();
                fs.Close();

				Dictionary<string, object> obj = DataHelper.GetJsonFile(bytes);
                JsonLocalAssetsVersionDataParser parser = new JsonLocalAssetsVersionDataParser();
                localAssetbundleVersionData = parser.DeserializeJson_LocalAssetsVersionData(obj);
                parser = null;


                if (localAssetbundleVersionData != null && localAssetbundleVersionData.localAssetDic == null)
                {
                    localAssetbundleVersionData.localAssetDic = new Dictionary<string, LocalAssetPrefab>();
                }
            }
            catch (IOException e)
            {
                throw e;
            } 
        }

        if (localAssetbundleVersionData != null)
        {
            isFirstDownLoad = localAssetbundleVersionData.firstDownLoadFinish;
        }
    }
}
