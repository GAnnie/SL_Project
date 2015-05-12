// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPathGroupManager.cs
// Author   : wenlin
// Created  : 2013/3/27 
// Purpose  : 
// **********************************************************************
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

namespace AssetBundleEditor
{
    public class AssetPathGroupManager
	{		
		public static string NewAssetResGroupPath = Application.dataPath + "/GameResources";
		public static string AssetGroupDataPath = "Assets/Editor/AssetBundle/Data";
        public static string AssetGroupDataName = "AssetGroupName.assetgroup";
        public static string AssetGroupConfigName = "ExportGroupConfig.assetgroup";
        public static string AssetBundleExportConfig = "AssetConfigData.json";
        private static string AssetRootNode 	= "Assets";
		public delegate void AssetPathCallBack();

		private static readonly AssetPathGroupManager instance = new AssetPathGroupManager();
	    public static AssetPathGroupManager Instance
	    {
	        get
			{
				return instance;
			}
	    }	
		
		private ExportAssetBundleConfig _assetPathGroup = null;
		private AssetPathGroupManager ()
		{		
	
			_assetPathGroup = LoadAssetPathGroup();
		}
		
		
		/// <summary>
		/// Gets the asset path group.
		/// </summary>
		/// <value>
		/// The asset path group.
		/// </value>
        public ExportAssetBundleConfig assetPathGroup
		{
			get
			{
				return _assetPathGroup;
			}
		}
		
		/// <summary>
		/// Loads the asset path group.
		/// </summary>
		/// <returns>
		/// The asset path group.
		/// </returns>
        public ExportAssetBundleConfig LoadAssetPathGroup()
		{

            //ExportAssetBundleConfig assetPathGroup = (ExportAssetBundleConfig)(AMF3Utils.LoadAmf3FromFile(AssetGroupDataPath + "/" + AssetGroupDataName));
            //if ( assetPathGroup == null )
            //{
            //    assetPathGroup = new ExportAssetBundleConfig();
            //}


            //Assets/Editor/AssetBundle/Data/AssetGroupName.assetgroup
			Dictionary<string, object> obj = DataHelper.GetJsonFile(AssetGroupDataPath + "/" + AssetGroupDataName);
            JsonExportAssetBundleConfigParser parser = new JsonExportAssetBundleConfigParser();
            ExportAssetBundleConfig assetPathGroup = parser.DeserializeJson_ExportAssetBundleConfig(obj);

			Debug.Log("ExportAssetBundleConfig1");

            if (assetPathGroup == null)
            {
                assetPathGroup = new ExportAssetBundleConfig();
            }

            /*
            foreach (KeyValuePair<string, GroupPathMsg> item in assetPathGroup.assetGroupPathDic)
            { 
                if( item.Value.exportProperty == null )
                {
                    item.Value.exportProperty = new AssetExportProperty();
                }


                item.Value.exportProperty.isPushDependencies        = item.Value.isPushDependencies;
                item.Value.exportProperty.isPushIntoOneAssetBundle  = item.Value.isPushIntoOneAssetBundle;
                item.Value.exportProperty.isDeleteAnimation         = item.Value.isDeleteAnimation;
                item.Value.exportProperty.isCompress                = item.Value.isCompress;

            }


            foreach (KeyValuePair<string, GroupPathMsg> item in assetPathGroup.commonObjectPathDic)
            {
                if (item.Value.exportProperty == null)
                {
                    item.Value.exportProperty = new AssetExportProperty();
                }


                item.Value.exportProperty.isPushDependencies = item.Value.isPushDependencies;
                item.Value.exportProperty.isPushIntoOneAssetBundle = item.Value.isPushIntoOneAssetBundle;
                item.Value.exportProperty.isDeleteAnimation = item.Value.isDeleteAnimation;
                item.Value.exportProperty.isCompress = item.Value.isCompress;

            }
            */
			return assetPathGroup;
		}

		/// <summary>
		/// Saves the asset path group.
		/// </summary>
		public void SaveAssetPathGroup()
		{
			if ( _assetPathGroup != null )
			{
                //AMF3Utils.SaveAmf3File(_assetPathGroup, AssetGroupDataPath + "/" + AssetGroupDataName);

                JsonExportAssetBundleConfigParser parser = new JsonExportAssetBundleConfigParser();
                Dictionary<string, object> obj = parser.SerializeJson_ExportAssetBundleConfig(_assetPathGroup);

                DataHelper.SaveJsonFile(obj, AssetGroupDataPath + "/" + AssetGroupDataName);
                //ExportAssetBundleParser parser = new ExportAssetBundleParser();
                //Utility.SaveJsonFile(parser.SerializeExportAssetBundleConfigToJsonData( config ), AssetGroupDataPath + "/" + AssetBundleExportConfig);
			}
		}
		
		/// <summary>
		/// Updates the asset path group.
		/// </summary>
		public void UpdateAssetPathGroup()
		{
			_assetPathGroup = LoadAssetPathGroup();
		}
		
		/// <summary>
		/// Adds the one path group.
		/// </summary>
		/// <param name='function'>
		/// Function.
		/// </param>
		public void AddOnePathGroup( AssetPathCallBack function = null )
		{
			if ( _assetPathGroup == null )
			{
				_assetPathGroup = LoadAssetPathGroup();
				if ( _assetPathGroup == null ) 
				{
					return;
				}
			}
			_SelectOneGroup( _assetPathGroup.assetGroupPathDic, function, "选择资源目录", NewAssetResGroupPath);
		}
		
		
		/// <summary>
		/// Deletes the one group.
		/// </summary>
		/// <param name='groupName'>
		/// Group name.
		/// </param>
		/// <param name='function'>
		/// Function.
		/// </param>
		public void DeleteOneGroup( string groupName, AssetPathCallBack function = null )
		{
			if ( _assetPathGroup == null ) return;
			
			_DelectOneGroup( _assetPathGroup.assetGroupPathDic, groupName, function );
		}

		/// <summary>
		/// Adds the one common group.
		/// </summary>
		/// <param name='function'>
		/// Function.
		/// </param>
        public void AddOneCommonGroup(AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }
			
			_SelectOneGroup( _assetPathGroup.commonObjectPathDic, function, "选择公共文件目录", NewAssetResGroupPath );
        }

		
		/// <summary>
		/// Deletes the one common group.
		/// </summary>
		/// <param name='configGroupName'>
		/// Config group name.
		/// </param>
		/// <param name='function'>
		/// Function.
		/// </param>
        public void DeleteOneCommonGroup(string configGroupName, AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null) return;
			
			_DelectOneGroup( _assetPathGroup.commonObjectPathDic, configGroupName, function );
        }


        /// <summary>
        /// selete excel path
        /// </summary>
        /// <param name="function"></param>
        public void SelectExcelPath( AssetPathCallBack function = null )
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }
			
			_assetPathGroup.excelPath = _SelectOneFile( "选择Excel文件目录", Application.dataPath, "xls" );
			
            if (function != null)
            {
                function();
            }
        }
		
        /// <summary>
        /// selete movie paths
        /// </summary>
        /// <param name="function"></param>
        public void SelectOneMovieScenePath(AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }

            string movieScenePath = EditorUtility.OpenFilePanel("选择动画场景文件", Application.dataPath, "unity");
            if (movieScenePath.Length > 0)
            {
                movieScenePath = movieScenePath.Replace("\\", "/");
                int assetIndex = movieScenePath.IndexOf(AssetRootNode);
                if (assetIndex == -1)
                {
                    Debug.Log(" Wrong Asset Group Path");
                    return;
                }

                movieScenePath = movieScenePath.Substring(assetIndex);

                string movieSceneName = movieScenePath.Substring(movieScenePath.LastIndexOf("/") + 1);
                movieSceneName = movieSceneName.Substring( 0 ,movieSceneName.LastIndexOf("."));

                if (!_assetPathGroup.movieScenePathDic.ContainsKey(movieSceneName))
                {
                    _assetPathGroup.movieScenePathDic.Add(movieSceneName, movieScenePath);
                    if (function != null)
                    {
                        function();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Tips", movieSceneName + " is Exits", "OK");
                }

            }

        }
		
		/// <summary>
		/// Deletes the one movie scene path.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='function'>
		/// Function.
		/// </param>
        public void DeleteOneMovieScenePath(string name, AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }

            if (_assetPathGroup.movieScenePathDic.ContainsKey(name))
            {
                _assetPathGroup.movieScenePathDic.Remove(name);
            }
            else
            {
                EditorUtility.DisplayDialog("Tips", name + "is not Exits", "OK");
            }

            if (function != null)
            {
                function();
            }
        }

        /// <summary>
        /// select assetSaveData Path
        /// </summary>
        /// <param name="function"></param>
        public void SelectAssetSaveDataPath(AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }
			
			_assetPathGroup.assetSaveDatePath = _SelectOneFolerPanel( "选择AssetSaveData保存目录", Application.dataPath, "assetdata");
            if (function != null)
            {
                function();
            }
        }

        /// <summary>
        /// select scene file path
        /// </summary>
        /// <param name="function"></param>
        public void SelectSceneFilePath(AssetPathCallBack function = null)
        {
            if (_assetPathGroup == null)
            {
                _assetPathGroup = LoadAssetPathGroup();
                if (_assetPathGroup == null)
                {
                    return;
                }
            }
			
			
			_assetPathGroup.sceneFilePath = _SelectOneFolerPanel( "选择SCENE文件目录", NewAssetResGroupPath );

            if (function != null)
            {
                function();
            } 
        }
		
		//=======================================Common Function ================================================================
		//=======================================================================================================================
		
		/// <summary>
		/// _s the select one foler panel.
		/// </summary>
		/// <returns>
		/// The select one foler panel.
		/// </returns>
		/// <param name='folderPanelTips'>
		/// Folder panel tips.
		/// </param>
		/// <param name='folderPath'>
		/// Folder path.
		/// </param>
		/// <param name='folderExtension'>
		/// Folder extension.
		/// </param>
		private string _SelectOneFolerPanel( string folderPanelTips = "", string folderPath = "", string folderExtension = "")
		{
			string path = EditorUtility.OpenFolderPanel(folderPanelTips, folderPath, folderExtension);
            if (path.Length > 0)
            {
                path = path.Replace("\\", "/");
                int assetIndex = path.IndexOf(AssetRootNode);
                if (assetIndex == -1)
                {
                    Debug.Log(" Wrong Atlas Path");
                    return string.Empty;
                }

                path = path.Substring(assetIndex);
				return path;
            }
			
			return string.Empty;
		}

		
		/// <summary>
		/// _s the select one file.
		/// </summary>
		/// <returns>
		/// The select one file.
		/// </returns>
		/// <param name='filePanelTips'>
		/// File panel tips.
		/// </param>
		/// <param name='filePanelPath'>
		/// File panel path.
		/// </param>
		/// <param name='filePanelExtension'>
		/// File panel extension.
		/// </param>
		private string _SelectOneFile( string filePanelTips = "",  string filePanelPath = "",  string filePanelExtension = "" )
		{
            string path = EditorUtility.OpenFilePanel(filePanelTips, filePanelPath, filePanelExtension);
            if ( path.Length > 0)
            {
                path = path.Replace("\\", "/");
                int assetIndex = path.IndexOf(AssetRootNode);
                if (assetIndex == -1)
                {
                    Debug.Log(" Wrong Asset Path");
                    return String.Empty;
                }

                path = path.Substring(assetIndex);
				return path;
            }
			
			return String.Empty;
		}
		
		/// <summary>
		/// Select One Folder
		/// </summary>
		/// <param name='dic'>
		/// Dic.
		/// </param>
		/// <param name='function'>
		/// Function.
		/// </param>
		/// <param name='FolderTips'>
		/// Folder tips.
		/// </param>
        private void _SelectOneGroup(Dictionary<string, GroupPathMsg> dic, AssetPathCallBack function = null, string FolderTips = "", string SearcherPath = "")
		{
			
			if ( dic == null ) return;
			string assetGroup = EditorUtility.OpenFolderPanel( FolderTips, SearcherPath, "");
			if( assetGroup.Length > 0 )
			{
				assetGroup = assetGroup.Replace( "\\" , "/" ); 
				int assetIndex = assetGroup.IndexOf( AssetRootNode );
				if ( assetIndex == -1 )
				{
					Debug.Log( " Wrong Asset Group Path" );	
					return;
				}
				
				int index = assetGroup.LastIndexOf( "/" );
				if ( index != -1 )
				{
					string groupPath = assetGroup.Substring( assetIndex );
					string groupName = assetGroup.Substring( index + 1  );
					if( _assetPathGroup.assetGroupPathDic.ContainsKey( groupName ) )
					{
                        groupName = GetNewgroupName(groupName);
					}
					
					dic.Add( groupName,  new GroupPathMsg( groupName, groupPath) );
				}
				
				if ( function != null )
				{
					function();
				}
			}
		}
		
		
		/// <summary>
		/// Delete One Group
		/// </summary>
		/// <param name='dic'>
		/// Dic.
		/// </param>
		/// <param name='groupName'>
		/// Group name.
		/// </param>
		/// <param name='function'>
		/// Function.
		/// </param>
        private void _DelectOneGroup(Dictionary<string, GroupPathMsg> dic, string groupName, AssetPathCallBack function = null)
		{
            if (!dic.ContainsKey(groupName))
            {
                Debug.Log(" the group of " + groupName + " is Not Exist!!");
                return;
            }

            dic.Remove(groupName);
            if (function != null)
            {
                function();
            }			
		}
		
		/// <summary>
		/// Gets the name of the newgroup.
		/// </summary>
		/// <returns>
		/// The newgroup name.
		/// </returns>
		/// <param name='groupName'>
		/// Group name.
		/// </param>
        private string GetNewgroupName(string groupName)
        {
            if (_assetPathGroup == null) return String.Empty;

            int index = 1;
            string newName = groupName  + index;
            while (_assetPathGroup.assetGroupPathDic.ContainsKey(newName))
            {
                index++;
                newName = groupName + index;
            }

            return newName;
        }
	}
}

