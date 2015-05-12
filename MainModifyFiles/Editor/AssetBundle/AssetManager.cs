// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetManager.cs
// Author   : wenlin
// Created  : 2013/3/27 
// Purpose  : 
// **********************************************************************
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEditor;

namespace AssetBundleEditor
{
	public class AssetManager
	{
		private static string AssetDataPath = "Assets/Editor/AssetBundle/Data";
        private static string AssetGameResourceName = "GameResources";
		private static string AssetDataName = "Assets.assetdata";
 
		private static string assetExtension    = "prefab";
        private static string audioExtension    = "mp3";
        private static string wavExtension      = "wav";
        private static string sceneExtension    = "unity";
        private static string txtExtension      = "txt";
        private static string bytesExtension    = "bytes";
        private static string materialExtension = "mat";
        private static string jpgExtenstion     = "jpg";
        private static string pngExtenstion     = "png";
        private static string tgaExtension      = "tga";
        private static string fbxExtension      = "fbx";
        private static string animExtension     = "anim";
        private static string cubemapExtension  = "cubemap";
        private static string shaderExtension   = "shader";
        private static string csharpExtension   = "cs";

        //保存导出资源的所有shader信息
        private Dictionary<string, string>  _allShaderDic      = null;
        public Dictionary<string, string> allShader { get { return _allShaderDic; } }

        private static string dataPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets"));

        public List<string> sameNameList = new List<string>();
		private static readonly AssetManager instance = new AssetManager();
	    public static AssetManager Instance
	    {
	        get
			{
				return instance;
			}
	    }	
			
		public  bool isAutoUpdate = false;
		private AssetPaths _assetPaths = null;
		public AssetPaths assetPaths { get { return _assetPaths; }}
		
        /// <summary>
        /// param string  is the group
        /// </summary>
		private AssetManager ()
		{

            _allShaderDic = new Dictionary<string, string>();

            if (isAutoUpdate)
            {
                UpdateAssetPaths();
            }
            else 
            {
                _assetPaths = LoadAssetDatas();
            }
		}

		/// <summary>
		/// The _is getting asset path.
		/// </summary>
		private bool _isGettingAssetPath = false;
		public bool isGettingAssetPath
		{
			get { return _isGettingAssetPath;}
		}
		
		/// <summary>
		/// The _is process.
		/// </summary>
		private bool _isProcess = false;
		public bool isProcess
		{
		 	get { return _isProcess; }
		}
		
		/// <summary>
		/// The _total asset number.
		/// </summary>
		private int _totalAssetNum = 0;
		public int totalAssetNum
		{
			get {  return _totalAssetNum; }
		}
		
		/// <summary>
		/// The _cur process asset number.
		/// </summary>
		private int _curProcessAssetNum = 0;
		public int curProcessAssetNum 
		{
			get { return _curProcessAssetNum; }
		}
		

		/// <summary>
		/// Gets the asset paths.
		/// </summary>
		/// <returns>
		/// The asset paths.
        /// </returns>
        #region thread_method
        private void GetAssetPaths()
		{
			//System.Threading.Interlocked.Exchange( ref _curProcessAssetNum , 0 );
			//System.Threading.Interlocked.Exchange( ref _totalAssetNum      , 0 );

            _curProcessAssetNum = 0;
            _totalAssetNum      = 0;

			Dictionary< string , Dictionary < string , string > > allAssetsDic = new Dictionary<string, Dictionary<string, string>>();
			//Dictionary< string , string > allAssetsGroupPathDic = new Dictionary<string, string>();
			//Traveral all Asset 
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic)
			{
				int index = item.Value.groupPath.IndexOf( "/" );
				if ( index != -1 )
				{
                    //string folderPath = dataPath + item.Value.Substring( index );
                    string folderPath = dataPath + item.Value.groupPath;

                    Dictionary<string, string> assetPrefabDic = GetAssetFromGroup(folderPath,  item.Value.extensionList.ToArray() );
					
					if ( assetPrefabDic != null )
					{
						//allAssetsGroupPathDic.Add( item.Key, item.Value.groupPath );
						allAssetsDic.Add( item.Key,  assetPrefabDic );
					}
				}
			}
			
			//Traveral All Common  File
            Dictionary< string , Dictionary < string , string > > commonPathDic = new Dictionary<string, Dictionary<string, string>>();
            //Dictionary< string , string > commonGroupPathDic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.commonObjectPathDic)
            {
                string folderPath = dataPath + item.Value.groupPath;
                Dictionary<string, string> commonObjs = GetAssetFromGroup(folderPath, item.Value.extensionList.ToArray());

                if (commonObjs != null)
				{
					//commonGroupPathDic.Add( item.Key, item.Value.groupPath );
                    commonPathDic.Add(item.Key, commonObjs);
				}
            }

            //Traveral All Scene
           // Dictionary<string, string> sceneDic = null;
			Dictionary< string , Dictionary < string , string > > allSceneDic = new Dictionary<string,Dictionary<string,string>>();
            Dictionary<string, GroupPathMsg> allSceneGroupPathDic = new Dictionary<string, GroupPathMsg>();
			if (AssetPathGroupManager.Instance.assetPathGroup.sceneFilePath.Length > 0)
            {
                string sceneFolderPath = dataPath + AssetPathGroupManager.Instance.assetPathGroup.sceneFilePath;
                Dictionary < string , string > sceneDic = GetAssetFromGroup(sceneFolderPath, sceneExtension);

                foreach (KeyValuePair<string, string> item in AssetPathGroupManager.Instance.assetPathGroup.movieScenePathDic)
                {
                    _totalAssetNum++;
                    string str = item.Value.Substring(item.Value.IndexOf("Assets"));
                    string key = str.Substring(0, str.LastIndexOf(".")); 

                    sceneDic.Add(key, str);
                }
				
				allSceneGroupPathDic.Add( "Scenes",  new GroupPathMsg( "Scenes", AssetPathGroupManager.Instance.assetPathGroup.sceneFilePath ));
				allSceneDic.Add( "Scenes",  sceneDic );
            }

			_isGettingAssetPath = false;

            //更新关联信息
            UpdateAllDependencisMessage();

			//Compare Data 
            CompareAssetsGroup(allAssetsDic, _assetPaths.assetPaths,        AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic);
          	CompareAssetsGroup(commonPathDic,  _assetPaths.commonPaths,     AssetPathGroupManager.Instance.assetPathGroup.commonObjectPathDic);
			CompareAssetsGroup(allSceneDic,    _assetPaths.scenePaths, 		allSceneGroupPathDic );
			_isProcess = false;
		}
		#endregion
		
		
		/// <summary>
		/// Gets the asset from group.
		/// </summary>
		/// <returns>
		/// The asset from group.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='extension'>
		/// Extension.
		/// </param>
        private Dictionary<string, string> GetAssetFromGroup(string path, params string[] extension)
		{		
			if ( !Directory.Exists( path ))
			{
				Debug.Log( "Directory  : " + path + " is not Exist" );
				return null;
			}
			
			Dictionary< string , string > assetDic = new Dictionary<string, string>();
			DirectoryInfo info = new DirectoryInfo( path );
            ListFiles(info, assetDic, extension);
			return assetDic;
		}
		
		/// <summary>
		/// Lists the files.
		/// </summary>
		/// <param name='info'>
		/// Info.
		/// </param>
		/// <param name='assetDic'>
		/// Asset dic.
		/// </param>
        /// 
        private void ListFiles(FileSystemInfo info, Dictionary<string, string> assetDic, params string[] extensions)
		{

			if (!info.Exists) return;
			
			DirectoryInfo dir = info as DirectoryInfo;
			if (dir == null) return;
			
			FileSystemInfo[] files = dir.GetFileSystemInfos();
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    //is file
                    if (files[i] is FileInfo)
                    {
                        FileInfo file = files[i] as FileInfo;

                        string fileFullName = file.FullName.Replace("\\", "/");
                        int dotIndex = fileFullName.LastIndexOf(".");

                        string extension = fileFullName.Substring(dotIndex + 1).ToLower(); ;

                        foreach (string ext in extensions)
                        {
                            if (extension == ext)
                            {
                                string str = fileFullName.Substring(fileFullName.IndexOf("Assets"));
                                string key = str.Substring(0, str.LastIndexOf("."));

								if (ext == materialExtension)
								{
									if (key.EndsWith("_mask") == false && key.EndsWith("_mutate") == false)
									{
										break;
									}
								}

                                if (!assetDic.ContainsKey(key))
                                {
                                    //System.Threading.Interlocked.Increment(ref _totalAssetNum);

                                    _totalAssetNum++;

                                    
                                    //assetDic.Add(
                                    //        file.Name.Substring(0, file.Name.IndexOf(".")),
                                    //        fileFullName.Substring(fileFullName.IndexOf("Assets"))
                                    //        );

                                    //assetDic.Add(
                                    //             str.Substring(0, str.LastIndexOf(".")),
                                    //             str 
                                    //             );

                                    assetDic.Add(
                                                 key,
                                                 str
                                                 );

                                    //string filepath = fileFullName.Substring(fileFullName.IndexOf("Assets") );
                                    //string filename = filepath.Substring(0, filepath.LastIndexOf("."));
                                    //assetDic.Add(filename, filepath);
                                }
                                else
                                {
                                    Debug.Log("Same File :" + fileFullName);
                                    //sameNameList.Add(fileFullName);
                                }
                            }
                        }
                    }

                    // is Directory
                    else
                    {
                        ListFiles(files[i], assetDic, extensions);
                    }
                } 
            }

		}

		
		/// <summary>
		/// Creates the new asset save data.
		/// </summary>
		/// <returns>
		/// The new asset save data.
		/// </returns>
		/// <param name='groupName'>
		/// Group name.
		/// </param>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='filePath'>
		/// File path.
		/// </param>
		private AssetSaveData CreateNewAssetSaveData(  string groupName , string fileName, string filePath )
		{
            string extension = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();
            AssetbundleType type = AssetbundleType.ASSETBUNDLE_TYPE_OTHER;
            if (extension == jpgExtenstion || extension == pngExtenstion || extension == tgaExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_TEXTURE;
            }
            else if (extension == materialExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_MATERIAL;
            }
            else if (extension == assetExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_PREFAB;
            }
            else if (extension == sceneExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_SCENE;
            }
            else if (extension == audioExtension || extension == wavExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_AUDIO;
            }
            else if (extension == bytesExtension || extension == txtExtension)
            {
                type = AssetbundleType.ASSETBUNDLE_TYPE_DATA;
            }


			AssetSaveData saveData = new AssetSaveData( type );
			saveData.groupName = groupName;
			saveData.fileName  = fileName.Substring(fileName.LastIndexOf("/") + 1);
            saveData.fileID    = fileName;
            saveData.guid = AssetDatabase.AssetPathToGUID(filePath);

			saveData.isDirty   = true;
			saveData.isExist   = true;
            saveData.isReferenceDirty = false;
			saveData.path 	   = filePath;

            saveData.version = long.Parse(DateTime.Now.ToString("yyMMddHHmm"));
			saveData.versionTime = DateTime.Now.ToString();
			
            //string path = dataPath + filePath.Substring( filePath.IndexOf( "/" ));
            string path = dataPath + filePath;
			saveData.fileTime 	 = File.GetLastWriteTime( path ).ToString();
			saveData.changMd5    = MD5Hashing.HashString( path );
            //saveData.md5         = string.Empty; 

			return saveData;
		}

		/// <summary>
		/// Compares the assets group.
		/// </summary>
		/// <param name='pathDic'>
		/// Path dic.
		/// </param>
		/// <param name='assetSaveData'>
		/// Asset save data.
		/// </param>
		/// <param name='returnSaveData'>
		/// Return save data.
		/// </param>
        private void CompareAssetsGroup(Dictionary<string, Dictionary<string, string>> pathDic,
                                        Dictionary<string, AssetsGroup> assetSaveData,
                                        Dictionary<string, GroupPathMsg> groupPathDic)
        {
            if (pathDic == null) return;
            if (assetSaveData == null) return;

            //set all <AssetSaveData > isExist to false
			foreach(KeyValuePair<string, AssetsGroup > groups in assetSaveData )
			{
				foreach (KeyValuePair<string, AssetSaveData> item in groups.Value.assets)
	            {
                    item.Value.isReferenceDirty = false;
	                item.Value.isExport = false;
	                item.Value.isExist = false;
	            }				
			}
			
			foreach (KeyValuePair<string, Dictionary<string, string> > item in pathDic)
            {
				if ( !assetSaveData.ContainsKey( item.Key ))
				{
					assetSaveData.Add( item.Key, new AssetsGroup(item.Key, groupPathDic[item.Key].groupPath));
				}

                GroupPathMsg groupPathMsg = groupPathDic[item.Key];
                AssetsGroup assetGroup  = assetSaveData[item.Key];

                if (assetGroup.exportProperty == null)
                {
                    assetGroup.exportProperty = new AssetExportProperty();
                }

               // assetGroup.exportProperty.isPushDependencies = groupPathMsg.exportProperty.isPushDependencies;
                //assetGroup.exportProperty.isPushIntoOneAssetBundle = groupPathMsg.exportProperty.isPushIntoOneAssetBundle;
               // assetGroup.exportProperty.isDeleteAnimation = groupPathMsg.exportProperty.isDeleteAnimation;
                //assetGroup.exportProperty.isCompress = groupPathMsg.exportProperty.isCompress;

                assetGroup.exportProperty = groupPathMsg.exportProperty;

				JudgeAssetsGroup(item.Key, item.Value, assetSaveData[item.Key].assets);
				
                //JudgeAssets(fileGroupItem.Key, fileGroupItem.Value, assetSaveData[fileGroupItem.Key]);
            }
        }
		
		/// <summary>
		/// Judges the assets group.
		/// </summary>
		/// <param name='groupName'>
		/// Group name.
		/// </param>
		/// <param name='pathDic'>
		/// Path dic.
		/// </param>
		/// <param name='assetSaveData'>
		/// Asset save data.
		/// </param>
		/// <param name='returnSaveData'>
		/// Return save data.
		/// </param>
        private void JudgeAssetsGroup(string groupName, 
                                      Dictionary<string, string> pathDic,
                                      Dictionary< string , AssetSaveData > assetSaveData)
        {
            if (pathDic == null) return;
            if (assetSaveData == null) return;

            AssetSaveData saveData = null;

            // Key is fileName , Value is filePath
            foreach (KeyValuePair<string, string> item in pathDic)
            {
                if (assetSaveData.ContainsKey(item.Key))
                {
                    saveData = assetSaveData[item.Key];

                    //string path = dataPath + item.Value;
                    //string fileMd5 = MD5.Md5_hash(path);
					
					saveData.isExist  = true;

                    //if (saveData.md5 == fileMd5)
                    //{
                    //    saveData.isDirty = false;
                    //}
                    //else
                    //{
                    //    saveData.isDirty = true;
                    //    saveData.changMd5 = fileMd5;

                    //    //						saveData.version ++;
                    //    //						saveData.versionTime = DateTime.Now.ToString();
                    //    saveData.fileTime = File.GetLastWriteTime(path).ToString();
                    //}
                }
                else
                {
                    saveData = CreateNewAssetSaveData(groupName, item.Key, item.Value);
                    assetSaveData.Add(item.Key, saveData);
                }

                if (_JudgeAssetChange(saveData))
                {
                    saveData.isDirty = true;
                    saveData.fileTime = File.GetLastWriteTime(dataPath + saveData.path).ToString();
                }
                else
                {
                    saveData.isDirty = false;
                }

                _curProcessAssetNum++;
                EditorUtility.DisplayProgressBar(" Update Asset Data ",
                                  " Progress : " + AssetManager.Instance.curProcessAssetNum.ToString() + " / " + AssetManager.Instance.totalAssetNum.ToString(),
                                  (float)AssetManager.Instance.curProcessAssetNum / AssetManager.Instance.totalAssetNum);



//                returnSaveData[groupName].Add(saveData);

                //System.Threading.Interlocked.Increment(ref _curProcessAssetNum);
            }
        }

        /// <summary>
        /// 判断是否有修改， 这个判断还会包括其引用的修改
        /// </summary>
        /// <param name="saveData"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        private bool _JudgeAssetChange(AssetSaveData saveData )
        {
            if (_assetPaths == null) return false;
            if (saveData == null) return false;

            string path = dataPath + saveData.path;
			string fileMd5 = MD5Hashing.HashString(path);
            bool isChange = false;
            
            //如果当前文件
            if( fileMd5 != saveData.md5 )
            {
                isChange = true;
                saveData.changMd5 = fileMd5;
                //saveData.md5 = fileMd5;
            }

            //如果文件不是Material类型 或者不是Prefab类型 或者Texture， 则直接放回
            if ( saveData.type != AssetbundleType.ASSETBUNDLE_TYPE_MATERIAL &&
                saveData.type != AssetbundleType.ASSETBUNDLE_TYPE_PREFAB && 
                saveData.type != AssetbundleType.ASSETBUNDLE_TYPE_TEXTURE && 
				saveData.type != AssetbundleType.ASSETBUNDLE_TYPE_SCENE)
            {
                return isChange;
            }

            string[] assetPath = new string[1] { saveData.path };
            string[] dependecis = AssetDatabase.GetDependencies(assetPath);
            foreach (string objPath in dependecis)
            {
                bool dependencisChange = false;
                string lowPath = objPath.ToLower();
                string fileName = objPath.Substring(0, objPath.LastIndexOf("."));
                //如果是贴图, 贴图的改变是判断其META文件是否修改了~！
                if (lowPath.EndsWith(pngExtenstion) || lowPath.EndsWith(jpgExtenstion) || lowPath.EndsWith(tgaExtension))
                {
                    dependencisChange = AddOrGetDependencisMessage(AssetbundleType.ASSETBUNDLE_TYPE_OTHER, fileName, objPath + ".meta");
                }

                //材质 判断材质是否修改
                else if (lowPath.EndsWith(materialExtension))
                {
                    dependencisChange = AddOrGetDependencisMessage(AssetbundleType.ASSETBUNDLE_TYPE_MATERIAL, fileName, objPath);
                }

                //天空盒的判断
                else if (lowPath.EndsWith(cubemapExtension))
                {
                    dependencisChange = AddOrGetDependencisMessage(AssetbundleType.ASSETBUNDLE_TYPE_OTHER, fileName, objPath);
                }

                //如果是FBX文件， FBX文件改变是判断其META文件是否有修改
                else if (lowPath.EndsWith(fbxExtension))
                {
                    dependencisChange = AddOrGetDependencisMessage(AssetbundleType.ASSETBUNDLE_TYPE_OTHER, fileName, objPath + ".meta");
                }

                else if (lowPath.EndsWith(csharpExtension))
                {
					if( Path.GetFileName( objPath ) != "EffectTime.cs" )
					{
						dependencisChange = AddOrGetDependencisMessage(AssetbundleType.ASSETBUNDLE_TYPE_OTHER, fileName, objPath);
					}					
                }

                //收集shader信息
                else if (lowPath.EndsWith(shaderExtension))
                {
                    string shaderName = fileName.Substring(fileName.LastIndexOf("/") + 1);
                    if (!_allShaderDic.ContainsKey(shaderName))
                    {
                        _allShaderDic.Add(shaderName, objPath);
                    }

                }

                if (dependencisChange)
                {
                    isChange = true;
                    saveData.isReferenceDirty = true;
                }
            }

            return isChange;
        }


        /// <summary>
        /// 添加和放回依赖项的信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns>返回依赖项是否改变</returns>
        private bool AddOrGetDependencisMessage(AssetbundleType type, string fileName, string path)
        {
            if (_assetPaths.allDependencis == null)
            {
                _assetPaths.allDependencis = new Dictionary<string , AssetDependencisGroup>();
            }

            if (!_assetPaths.allDependencis.ContainsKey(type.ToString()))
            {
                _assetPaths.allDependencis.Add(type.ToString(), new AssetDependencisGroup(type));
            }

            AssetDependencisGroup group = _assetPaths.allDependencis[type.ToString()];
            AssetDependencis dependencis = null;
            if (!group.dependencisDic.ContainsKey(fileName))
            {
                dependencis = new AssetDependencis();
                string fileMd5 = MD5Hashing.HashString(dataPath + path);

                dependencis.md5 = fileMd5;
                dependencis.isChang = true;
                dependencis.path = path;

                group.dependencisDic.Add(fileName, dependencis);
            }
            else
            {
                dependencis = group.dependencisDic[fileName];
            }

            return dependencis.isChang;
        }


        /// <summary>
        /// 更新所有关联信息的修改情况
        /// </summary>
        private void UpdateAllDependencisMessage()
        {
            if (_assetPaths == null)
            {
                return;
            }

            foreach (KeyValuePair<string, AssetDependencisGroup> item in _assetPaths.allDependencis)
            {
                foreach (KeyValuePair<string, AssetDependencis> dep in item.Value.dependencisDic)
                {
                    string path    = dataPath + dep.Value.path;
					string fileMd5 = MD5Hashing.HashString( path);
                    if (dep.Value.md5 != fileMd5)
                    {
                        dep.Value.isChang = true;
                        dep.Value.md5 = fileMd5;
                    }
                    else
                    {
                        dep.Value.isChang = false;
                    }
                }
            }
        }

        //设置重新AllShaderObject
        private string allShaderObjectpath = "Assets/GameResources/ShaderObj/ALLShaderObj.prefab";
        private void UpdateAllShaderObj()
        {
            GameObject prefab = AssetDatabase.LoadMainAssetAtPath(allShaderObjectpath) as GameObject;
            ShaderReferenceObj shaderReferenceObj = prefab.GetComponent<ShaderReferenceObj>();

            if (shaderReferenceObj != null)
            {
                List<Shader> list = new List<Shader>();
                foreach (KeyValuePair<string, string> item in _allShaderDic)
                {
                    Shader shader = AssetDatabase.LoadMainAssetAtPath(item.Value) as Shader;
                    list.Add(shader);
                }

                shaderReferenceObj.shaders = list.ToArray();

                Debug.Log(" =============== Rest All Shader Obj =================");
                EditorUtility.SetDirty(shaderReferenceObj);
                //GameObject.DestroyImmediate(prefab);
                prefab = null;
                shaderReferenceObj = null;

               AssetDatabase.ImportAsset(allShaderObjectpath, ImportAssetOptions.ForceUpdate);
            }
        }
		
		/// <summary>
		/// Updates the asset paths.
		/// </summary>
		public void UpdateAssetPaths( /*bool updataAssetPaths = true*/)
		{
			if ( _isGettingAssetPath || isProcess ) return;


			_isGettingAssetPath = true;
			_isProcess          = true;
			
            sameNameList.Clear();

            //if (updataAssetPaths)
            //{
            isUpdateAssetPaths = true;
            _assetPaths = LoadAssetDatas();
            //}

            _allShaderDic.Clear();

			//Create one thread to traversal all prefab from special group
			//Thread thread = new Thread( new ThreadStart( GetAssetPaths )); 
			//thread.Start();

            //改为主线程调用
            GetAssetPaths();

            //修改AllShaderObj
            UpdateAllShaderObj();
		}
		
		/// <summary>
		/// Loads the asset datas.
		/// </summary>
		/// <returns>
		/// The asset datas.
		/// </returns>
		private AssetPaths LoadAssetDatas()
		{
            if (AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath == "")
            {
                AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath = AssetDataPath;
                AssetPathGroupManager.Instance.SaveAssetPathGroup();
            }

            //AssetPaths assetPaths = (AssetPaths)(AMF3Utils.LoadAmf3FromFile( AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + AssetDataName));

            Dictionary<string, object> obj = DataHelper.GetJsonFile(AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + AssetDataName);
            JsonAssetPathsParser parser = new JsonAssetPathsParser();
            AssetPaths assetPaths = parser.DeserializeJson_AssetPaths(obj);

            if ( assetPaths == null )
			{
                Debug.Log("Create New One ");
				assetPaths  = new AssetPaths();
			}
			return assetPaths;		
		}
		
		/// <summary>
		/// Saves the asset datas.
		/// </summary>
		public void SaveAssetDatas( DateTime time)
		{
			if ( _assetPaths != null )
			{
                JsonAssetPathsParser parser = new JsonAssetPathsParser();
                Dictionary<string, object> obj = parser.SerializeJson_AssetPaths(_assetPaths);

                //AMF3Utils.SaveAmf3File(_assetPaths, AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + AssetDataName);
                DataHelper.SaveJsonFile(obj, AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + AssetDataName);


                string data = time.ToString("yyyy-MM-dd-HH-mm");
                string backupPath = AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + data;
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                //AMF3Utils.SaveAmf3File(_assetPaths, backupPath + "/" + AssetDataName);
                DataHelper.SaveJsonFile(obj, backupPath + "/" + AssetDataName);


			}				
		}

        /// <summary>
        /// Load AssetBundle XML
        /// </summary>
        /// <returns></returns>
        //private AssetBundleXML LoadAssetBundleXML()
        //{
        //    return AssetBundleExport.Instance.LoadAssetBundleXML();
        //}

        private ResourcesVersionData LoadResourceVersionData()
        {
            return AssetBundleExport.Instance.LoadResourcesVersionData();
        }

        /// <summary>
        /// Save AssetBundle XML
        /// </summary>
        /// <param name="xml"></param>
        //private void SaveAssetBundleXML( AssetBundleXML xml )
        //{
        //    AssetBundleExport.Instance.SaveAssetBundleXML(xml);
        //}
        private void SaveResourceVesionData( ResourcesVersionData data )
        {
            AssetBundleExport.Instance.SaveResourcesVersionData( data );
        }
		
		private void SaveResourceVesionDataRefList(ResourcesVersionData data)
		{
			AssetBundleExport.Instance.SaveResourcesVersionDataRefList( data );
			
		}

        /// <summary>
        /// 打包资源的总接口
        /// </summary>
        public bool isUpdateAssetPaths = false; 
        public delegate void MessageFunction(string message);
		
		private DateTime dateTime ;
        public string ExportAssetBundle(string exporerName = "", BuildTarget buildTarget = BuildTarget.Android, MessageFunction messageFunc = null)
        {
            if (!isUpdateAssetPaths  )
            {
                //0、 如果没有更新资源信息， 则自动更新
                this.UpdateAssetPaths();

                //等待资源更新完毕
                while (_isGettingAssetPath || _isProcess) { }
            }
			
			bool exportResourceData = EditorUtility.DisplayDialog( "提示", "是否直接导出ResourceData?", "是","否" );
			
            EditorApplication.SaveAssets();

            if ( _assetPaths == null )
            {
                Debug.Log("Initialization Error !!");
                if (messageFunc != null) messageFunc(AssetBundleError.AssetPaths_ERROR);
                return AssetBundleError.AssetPaths_ERROR;
            }
			
			//1、 获取当前打包的时间
			dateTime  = DateTime.Now;

            //2、重置AssetBundleExport的基本信息
            AssetBundleExport.Instance.ResetExportMsg(dateTime, buildTarget);

            //3、开始Export Output Log 的写入
            string error = AssetBundleExportLogManager.Instance.Begin(AssetBundleExport.exportData);
            if ( error == "" )
            {
                Debug.Log("Load Excel Success !!");
                AssetBundleExportLogManager.Instance.WriteExportHeadMessage(exporerName, DateTime.Now.ToString());
            }
            else
            {
                Debug.Log(error);
                if (messageFunc != null) messageFunc(error);
                return error;
            }

            //4、读取ResourcesVesionData
            ResourcesVersionData resourceData = LoadResourceVersionData();

            //5: 删除 不存在的AssetSaveData数据， 并记录进去Excel表里面
            _DeleteNotExist( resourceData.assetPrefabs, _assetPaths.assetPaths,  AssetBundleExportLogManager.Instance.WriteAssetDelMessage);
            _DeleteNotExist( resourceData.commonObjs,   _assetPaths.commonPaths, AssetBundleExportLogManager.Instance.WriteAssetDelCommonObjMessage);
            _DeleteNotExist( resourceData.scenes,       _assetPaths.scenePaths,  AssetBundleExportLogManager.Instance.WriteAssetDelSceneMessage, true);
            
            //6: 导出所有的已修改的资源的Assebundle
			List< AssetSaveData > exportAssetSaveList = AssetBundleExport.Instance.ExportAllAssets( _assetPaths );

            //7、1M包整理
            AssetBundleExport.Instance.ExportMergedPackage( exportAssetSaveList, 1000 );


            //8: 压缩文件
            //string compressErrorMsg = AssetBundleExport.Instance.CompressAllAssets();
            //if (compressErrorMsg != String.Empty)
            //{
            //    if (messageFunc != null) messageFunc(compressErrorMsg);
            //    return compressErrorMsg;
            //}
            //else
            //{
                //8: 保存文件数据
                SaveMsg( resourceData.assetPrefabs, _assetPaths.assetPaths,   AssetBundleExportLogManager.Instance.WriteAssetNewMessage,                AssetBundleExportLogManager.Instance.WriteAssetChangeMessage);
				SaveMsg( resourceData.commonObjs  , _assetPaths.commonPaths,  AssetBundleExportLogManager.Instance.WriteAssetNewCommoneObjectMessage,   AssetBundleExportLogManager.Instance.WriteAssetChangeCommonObjMessage);
                SaveMsg( resourceData.scenes      , _assetPaths.scenePaths,   AssetBundleExportLogManager.Instance.WriteAssetNewSceneMessage,           AssetBundleExportLogManager.Instance.WriteAssetChangeSceneMessage, true);

                //8.1 生成树
                //AssetsTreeAssembler assemble = new AssetsTreeAssembler();
                //assemble.AddAssetPath(assetPaths);
                resourceData.root = null;//assemble.root;


                //9: 保存< ResourceVesionData >
                if (resourceData != null)
                {
                    //设置资源文件的版本好资源
                    resourceData.version = long.Parse(dateTime.ToString("yyMMddHHmm"));
                    SaveResourceVesionData(resourceData);
                }

                //10: 保存 < AssetPaths>
                SaveAssetDatas(dateTime);
			
                //11: 保存Log 数据
                AssetBundleExportLogManager.Instance.End();
			
			
				//12: 导出ResourceData
				if( exportResourceData )
				{
					ExportResourceVersionDataFormat( resourceData );
				}
			
				TimeSpan span = DateTime.Now.Subtract( dateTime ) ;
				
                //12: 弹出打包完成提示框
                if (messageFunc != null) messageFunc("Message : Export Finish . Spend Time : " + span.Hours.ToString() + "h:" + span.Minutes.ToString() + "m:" + span.Seconds.ToString() + "s");

                isUpdateAssetPaths = false;

                return "";               
            //}
        }
		
		
		
		public void ExportResourceVersionDataFormat( ResourcesVersionData resourceData = null )
		{
			//如果传进来的resourceData为空， 则从新加载
			if( resourceData == null )
			{
            	resourceData = LoadResourceVersionData();
			}
			
			//读取资源引用表
			AssetPathReferenceList list = AssetPathReferenceGenator.LoadAssetPathReferenceList();
			
			if( list == null )
			{
				Debug.LogError( " Can not get the <AssetPathReferenceList> Data !!!" );
				return;
			}			
			
			//读取体验包信息
			//List< string > experiencePakageList = SaveExperienceData.ImportExperienceData( SaveExperienceData.exportDataPath );
			//问情项目不采用体验包方式(2014/12/05开始需要才有体验包方式)
			List< string > experiencePakageList = null;
			
			ResourcesVersionData resourceDataWithRefList = new ResourcesVersionData(1);
			resourceDataWithRefList.version = resourceData.version;
			
			_DealWithResourceVersionData( resourceData.assetPrefabs,  resourceDataWithRefList.assetPrefabs ,list, experiencePakageList );
			_DealWithResourceVersionData( resourceData.scenes,  	  resourceDataWithRefList.scenes	   ,list, experiencePakageList );
			_DealWithResourceVersionData( resourceData.commonObjs,    resourceDataWithRefList.commonObjs   ,list, experiencePakageList );
			
			//保存SaveAssetPathReferenceList
			AssetPathReferenceGenator.SaveAssetPathReferenceList( list );
			
			//生成ResourceAssetData
			SaveResourceVesionDataRefList( resourceDataWithRefList );
			
			EditorUtility.DisplayDialog( "Tips", "Export Resource Data Success !!!" , "ok" );
		}
		
	
		private void _DealWithResourceVersionData(Dictionary< string , AssetPrefab > source, 
												    Dictionary< string , AssetPrefab > desc,
													AssetPathReferenceList list,
		                                          	List<string> experiencePakageList
													)
		{
			foreach( KeyValuePair< string,  AssetPrefab > item in source )
			{
				string subpath = item.Key;
				int index = subpath.LastIndexOf("/");
				if( index != -1 )
				{
					string folder 	  = subpath.Substring( 0, index+1 );
					string assetName = subpath.Substring( index+1 );

					if( !list.folderRefDict.ContainsKey( folder))
					{
						list.folderRefDict.Add( folder, ++list.folderRefNumber );
					}
					
					if( !list.assetRefDict.ContainsKey( assetName))
					{
						list.assetRefDict.Add( assetName, ++list.assetsNumber );
					}

					//分析体验包信息
					if( experiencePakageList != null && experiencePakageList.Contains( item.Key ))
					{
						item.Value.isEp = 1;
					}
					else
					{
						item.Value.isEp = 0;
					}


//					if (experiencePakageList == null)
//					{
//						item.Value.isEp = 0;
//					}
//					else{
//						//分析体验包信息
//						if( experiencePakageList.Contains( item.Key ))
//						{
//							item.Value.isEp = 1;
//						}
//						else
//						{
//							//体验包只判断怪物模型资源
//							if (subpath.IndexOf("GameResources/ArtResources/Prefabs/Model/Character/Pet/") == -1)
//							{
//								item.Value.isEp = 1;
//							}
//							else
//							{
//								item.Value.isEp = 0;
//							}
//						}
//					}
					
					desc.Add( 
							list.folderRefDict[folder]+ ":" + list.assetRefDict[assetName],
							item.Value
							);
					
				}				
			}
		}		
		
		
		

        private void _DeleteNotExist(
                                    Dictionary<string,  AssetPrefab > versionDataDic,  
                                    Dictionary< string, AssetsGroup > groupDic,
                                    Action< AssetSaveData >           delMessage = null, 
                                    bool originName = false
            )
        {
            List<string> deleteList = new List<string>();
			foreach ( KeyValuePair< string, AssetsGroup > groupItem in groupDic )
			{
				deleteList.Clear();
				
	            foreach (KeyValuePair<string, AssetSaveData> item in groupItem.Value.assets)
	            {
	                if (!item.Value.isExist) 
					{
						deleteList.Add(item.Key); 
					}
	            }
				
	            foreach (string fileName in deleteList)
	            {
	                AssetSaveData data = groupItem.Value.assets[fileName];
	                groupItem.Value.assets.Remove(fileName);

                    string fileId = null;//data.guid;
                    int index = data.fileID.IndexOf(AssetGameResourceName);
                    if (index != -1)
                    {
                        fileId = data.fileID.Substring(index);
                    }
                    else
                    {
                        fileId = data.fileID;
                    }

                    //if (!originName) fileId = data.fileID;//data.path.Substring(0, data.path.LastIndexOf("."));
                    //else             fileId = data.fileName;
	
	                //remove
                    if (versionDataDic.ContainsKey(fileId))
	                {
                        AssetPrefab prefab = versionDataDic[fileId];
                        versionDataDic.Remove(fileId);
	                }
	
	                if ( delMessage != null )
	                {
	                    delMessage(data);
	                }
	
	                AssetBundleExport.Instance.DeleteOneAssetBundle(data.exportPath);
	                data = null;
	            }				
			}

        }

        private void SaveMsg(
                              Dictionary<string, AssetPrefab> versionDateDic,
                              Dictionary<string, AssetsGroup> groupDic,
                              Action< AssetSaveData > NewMessage    = null, 
                              Action< AssetSaveData > ChangeMessage = null, 
                              bool originName = false)
        {
			foreach( KeyValuePair< string , AssetsGroup > groupItem in groupDic )
			{
	            foreach (KeyValuePair<string, AssetSaveData> item in groupItem.Value.assets)
	            {
	                if (item.Value.isDirty)
	                {
	                    AssetSaveData data = item.Value;
	                    data.isDirty = false;
                        data.isExist = true;
                        data.isReferenceDirty = false;

	                    AssetPrefab exportPrefab = null;
                        //string fileId = data.fileID;
                        string fileId = null;// data.guid;
                        int index = data.fileID.IndexOf(AssetGameResourceName);
                        if (index != -1)
                        {
                            fileId = data.fileID.Substring(index);
                        }
                        else
                        {
                            fileId = data.fileID;
                        }

                        //if (!originName) fileId = data.fileID;//data.path.Substring(0, data.path.LastIndexOf("."));
	                    //else             fileId = data.fileName;//.Substring( data.fileName.LastIndexOf("/"));

                        Debug.Log("data.fileID : " + data.fileID);
                        Debug.Log("FileID : " + fileId);

                        //如果不存在此数据， 则创建
                        if (!versionDateDic.ContainsKey(fileId))
	                    {
	                        exportPrefab = new AssetPrefab();
	                        //exportPrefab.name = fileId;
	                        //exportPrefab.path = data.exportPath;

	                        exportPrefab.needDecompress = data.needUncompress;
	                        exportPrefab.version        = data.version;
	                        exportPrefab.size           = data.dataSize;
                            exportPrefab.package        = data.package;
                            exportPrefab.packageNum     = data.packageNum;
                            exportPrefab.packagePos     = data.packagePos;

                            versionDateDic.Add(fileId, exportPrefab);
	                        if (NewMessage != null) NewMessage(data);
	                    }
	                    else
	                    {
                            //更新数据
                            exportPrefab = versionDateDic[fileId];
	                        exportPrefab.needDecompress = data.needUncompress;
	                        exportPrefab.version        = data.version;
	                        exportPrefab.size           = data.dataSize;
                            exportPrefab.package        = data.package;
                            exportPrefab.packageNum     = data.packageNum;
                            exportPrefab.packagePos     = data.packagePos;

	                        if (ChangeMessage != null) ChangeMessage(data);
	                    }
	                }
	            }				
			}
        }
	}
}

