using System;
using System.Collections.Generic;

public class AssetBundleXML
{
    public List<AssetPrefab> assetPrefabList = null;
    public List<AssetPrefab> configList      = null;
    public List<AssetPrefab> sceneList       = null;
	public List<AssetPrefab> commonObjList   = null;
	
    public List<AssetPrefab> dependenceList  = null;

    public List<AssetPrefab> uiList          = null;
    public List<AssetPrefab> commonAtlasList = null;

    public AssetBundleXML()
    {
        assetPrefabList = new List<AssetPrefab>();
        configList      = new List<AssetPrefab>();
        sceneList       = new List<AssetPrefab>();
        uiList          = new List<AssetPrefab>();
        commonAtlasList = new List<AssetPrefab>();
		commonObjList   = new List<AssetPrefab>();

        dependenceList  = new List<AssetPrefab>();
    }
}







