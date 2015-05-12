// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPathGroup.cs
// Author   : wenlin
// Created  : 2013/3/26 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;

namespace AssetBundleEditor
{

    //public class AssetPathGroup
    //{
    //    public Dictionary< string, string > groupPathDic = null;

    //    public Dictionary< string, string > configPathDic = null;
		
    //    public Dictionary< string, string > commonObjectPathDic = null;
		
    //    public Dictionary<string, string> movieScenePathDic = null;

    //    public string                     gameAtlasPathDic = null;
    //    public string                     uiFolderPath = null;
		
		
    //    public string scenePrefabFilePath = "";
    //    public string sceneFilePath = "";

    //    public string excelPath = "";

    //    public string assetSaveDatePath = "";

    //    //public string assetBundleXMLDataPath = "";

    //    //public string assetBundleExportPath = "";

    //    public string assetShaderPath = "";

    //    public string assetShaderExportPath = "";

    //    public AssetPathGroup()
    //    {
    //        groupPathDic        = new Dictionary<string, string>();
    //        configPathDic       = new Dictionary<string, string>();
    //        movieScenePathDic   = new Dictionary<string, string>();
    //        commonObjectPathDic = new Dictionary<string, string>();
    //    }
    //}

    public class ExportAssetBundleConfig
    {
        public Dictionary<string, GroupPathMsg> assetGroupPathDic = null;

        public Dictionary<string, GroupPathMsg> commonObjectPathDic = null;

        public Dictionary<string, string> movieScenePathDic = null;

        public string scenePrefabFilePath = "";
        public string sceneFilePath = "";

        public string excelPath = "";

        private string _assetSaveDatePath = "";

		public string platformAssetSaveDatePath = "";
		
		public ExportAssetBundleConfig()
        {
            assetGroupPathDic   = new Dictionary<string, GroupPathMsg>();
            commonObjectPathDic = new Dictionary<string, GroupPathMsg>();
            movieScenePathDic   = new Dictionary<string, string>();
        }

		public string oriAssetSaveDatePath
		{
			get
			{
				return _assetSaveDatePath;
			}
		}

		public string assetSaveDatePath
		{
			get
			{
				if (string.IsNullOrEmpty(_assetSaveDatePath))
				{
					return _assetSaveDatePath;
				}
				else
				{
					#if UNITY_IOS
					return _assetSaveDatePath + "/ios/";
					#elif UNITY_ANDROID
					return _assetSaveDatePath + "/android/";
					#elif UNITY_STANDALONE
					return _assetSaveDatePath + "/pc/";
					#else
					return _assetSaveDatePath;
					#endif
				}
			}
			set
			{
				_assetSaveDatePath = value;
			}
		}
    }

    public class GroupPathMsg
    {
        //组名
        public string groupName ;
        
        //组路径
        public string groupPath;

        ////是否进行数据关联
        //public bool isPushDependencies = true;

        ////是否进行合拼整包
        //public bool isPushIntoOneAssetBundle = false;

        ////是否删除动作模型
        //public bool isDeleteAnimation = false;

        ////是否进行压缩( 默认进行压缩 )
        //public bool isCompress        = true;

        //组的属性
        public AssetExportProperty exportProperty = null;

        //搜索的后缀名
        public List< string > extensionList = null;

        public GroupPathMsg()
        {

        }

        public GroupPathMsg(string groupName, string groupPath)
        {
            this.groupName = groupName;
            this.groupPath = groupPath;

            this.exportProperty = new AssetExportProperty();
            this.extensionList = new List<string>();
        }
    }
}






