using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace AssetBundleEditor
{
	public class AssetBundleExport
	{
        //private string defaultExportPath = "";
        private static string beignStringName = "Assetbundles";

		#if UNITY_IOS
		private static string platformDir = "/ios";
		#elif UNITY_ANDROID
		private static string platformDir = "/android";
		#elif UNITY_STANDALONE
		private static string platformDir = "/pc";
		#else
		private static string platformDir = "";
		#endif
		
		public static string  exportPath      = "Export" + platformDir;
        public static string exportData       = String.Empty;
        private BuildTarget   buidleTarget    = BuildTarget.Android;
        private static string streamPath      = "Assets/StreamingAssets";
        private static string unusePath       = "A_non";
        

        //private static string AssetBundleXMLPath = "Assets/Editor/AssetBundle/Data";
        private static string AssetBudnleXMLName = "ResourceVersionData.bytes";
		private static string AssetBudnleXMLNameWithRefList = "ResourceData.bytes";
		
        private List<AssetSaveData> _compressAssetsList = null;
        //private static Dictionary< string , string > _prefabExportList;
        //private static Dictionary< string , bool   > _prefabExportFlag;

        private static string dataPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets"));
		private static BuildAssetBundleOptions compressOptions = BuildAssetBundleOptions.CollectDependencies |
                                                         BuildAssetBundleOptions.CompleteAssets |
                                                         BuildAssetBundleOptions.DeterministicAssetBundle;

        private static BuildAssetBundleOptions uncompressOptions = BuildAssetBundleOptions.CollectDependencies |
                                                         BuildAssetBundleOptions.CompleteAssets |
                                                         BuildAssetBundleOptions.UncompressedAssetBundle |
                                                         BuildAssetBundleOptions.DeterministicAssetBundle;

        private static readonly AssetBundleExport instance = new AssetBundleExport();
	    public static AssetBundleExport Instance
	    {
	        get
			{
				return instance;
			}
	    }

		private AssetBundleExport ()
		{
		}

        public void ResetExportMsg( DateTime time, BuildTarget target = BuildTarget.Android)
        {
            buidleTarget = target;
            exportData   = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
        }

        //public GameSettingData data = null;
        //private string gamesettingDataPath = Application.dataPath + "/Resources/Setting/GameSettingData.xml.txt";
        private string defaultExportPath
        {
            get 
            {
                //if ( data == null )
                //{
                //    if (File.Exists(gamesettingDataPath))
                //    {
                //        data = Utility.LoadObjectAsXML<GameSettingData>(gamesettingDataPath);
                //    }
                //}

                //if (data != null)
                //{
                //    if (data.loadMode == 2) return streamPath + "/" + exportData + "/" + beignStringName;
                //}

                return exportPath + "/" + exportData;
            }
        }

        private string backupExportPath
        {
            get
            {
                return exportPath + "/VersionControl-" + exportData + "/";
            }
        }

        public ResourcesVersionData LoadResourcesVersionData()
        {
            //CheckAndCreateDirectory(exportPath);
            ResourcesVersionData versionData = null;
            if (File.Exists(exportPath + "/" + AssetBudnleXMLName))
            {
				Dictionary< string, object > obj = DataHelper.GetJsonFile(exportPath + "/" + AssetBudnleXMLName);
                JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
                versionData = parser.DeserializeJson_ResourcesVersionData(obj );//Utility.LoadObjectAsXML<ResourcesVersionData>(exportPath + "/" + AssetBudnleXMLName, true);
            }
            if (versionData == null)
            {
                versionData = new ResourcesVersionData(1);
            }

            return versionData;
        }

        public void SaveResourcesVersionData( ResourcesVersionData data )
        {
            if (data != null)
            {
                //Utility.SaveObjectAsXML<AssetBundleXML>(AssetPathGroupManager.Instance.assetPathGroup.assetBundleXMLDataPath + "/" + AssetBudnleXMLName, xml);
                
                CheckAndCreateDirectory(exportPath + "/VersionControl-" + exportData + "/");

                JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
                Dictionary<string, object> obj = parser.SerializeJson_ResourcesVersionData(data);

                DataHelper.SaveJsonFile(obj, exportPath + "/VersionControl-" + exportData + "/" + AssetBudnleXMLName);
				DataHelper.SaveJsonFile(obj, exportPath + "/" + AssetBudnleXMLName);
                
                //Utility.SaveObjectAsXMLWithCompress<AssetBundleXML>(exportPath + "/VersionControl-" + exportData + "/" + AssetBudnleXMLName, xml);
                //Utility.SaveObjectAsXMLWithCompress<AssetBundleXML>(exportPath + "/" + AssetBudnleXMLName, xml);
            }
        }
		
		
		/// <summary>
		/// 最终生成ResourceData信息
		/// 	此表包括：
		/// 		1】 ResourceVersionData 资源的保本信息
		/// 		2]	AssetReferenceList  资源的目录引用表
		/// 		3]  MapAreasData	    区域资源信息	 
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void SaveResourcesVersionDataRefList( ResourcesVersionData data  )
		{
            if (data != null)
            {
				//获取ResourceVerData
                JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
                Dictionary<string, object> obj = parser.SerializeJson_ResourcesVersionData(data);
				byte[] resourceVerData = DataHelper.GetJsonData( obj, true );
//				uint resourceVerDataLen = (uint)resourceVerData.Length;
				
				//获取AssetReferenceList
				byte[] assetReferenceList = AssetPathReferenceGenator.LoadAssetPathReferenceListBytes( false );				
//				uint assetReferenceListLen = (uint)assetReferenceList.Length;
				
				//获取MapAreasData 信息
//				MapAreasDataController	
//				byte[] mapAssetData = MapAreasGenerator.GetMapAreasDataWithBytes(false);
//				uint mapAssetDataLen = (uint)mapAssetData.Length;				
//				
//				//合拼资源
//				List<byte> bytesList = new List<byte>();				
//				
//				//文件头
//				bytesList.AddRange( BitConverter.GetBytes( 0x56ae45 )  );
//				
//				//ResourceVersionData长度
//				bytesList.AddRange( BitConverter.GetBytes( resourceVerDataLen ) );
//				
//				//MapAreasData 的长度
//				bytesList.AddRange( BitConverter.GetBytes( assetReferenceListLen ) );
//				
//				//MapAreasData 的长度
//				bytesList.AddRange( BitConverter.GetBytes( mapAssetDataLen) );
//				
//				//ResourceVersionData数据体
//				bytesList.AddRange( resourceVerData );
//					
//				//AssetReferenceList数据体
//				bytesList.AddRange( assetReferenceList );
//				
//				//MapAreasData数据体
//				bytesList.AddRange( mapAssetData );
//				
//				bytesList.AddRange( BitConverter.GetBytes( 0x32) );
				
				byte[] exportData = ResourceDataManager.AssembleResourceData( 
																		resourceVerData,
																		assetReferenceList
																		);	
				
				//GameEditorUtils.SaveJsonFile(obj, exportPath + "/" + AssetBudnleXMLNameWithRefList);
				DataHelper.SaveFile( exportPath + "/" + AssetBudnleXMLNameWithRefList, exportData );
            }			
		}
		

        /// <summary>
        /// µŒ³öµ¥žöÎÄŒþ×ÊÔŽ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="property"></param>
        /// <param name="isMustSetDependencis"></param>
        /// <returns></returns>
        public bool ExportOneAsset(AssetSaveData data, AssetExportProperty property , bool isMustSetDependencis = false)
        {
            if (data == null)
            {
                Debug.LogError(" AssetSaveData is null !! ");
                return false;
            }

            data.version = long.Parse(DateTime.Now.ToString("yyMMddHHmm"));
            data.versionTime = DateTime.Now.ToString();
            data.md5 = data.changMd5;
            //data.changMd5 = "";
            
            string path = dataPath + data.path;
            data.fileTime = File.GetLastWriteTime(path).ToString();

            string assetBundlePath = GetExportFolder(data);
            CheckAndCreateDirectory(unusePath);
            string assetExportPath = assetBundlePath + "/" + data.fileName + ".unity3d";
            string nonExportPath = unusePath + "/" + data.fileName + ".unity3d";

            //ÊÇ·ñÉŸ³ýÄ£ÐÍ¶¯×÷
            string fbxFile = null; 
            ModelImporter importer = null; 
            ModelImporterClipAnimation[] clipAnimationArray = null;
            if (property.isDeleteAnimation)
            {
                importer = ModelImporter.GetAtPath(data.path) as ModelImporter;
                fbxFile = data.path; 
                
                if (importer == null)
                {
                    string[] dependencies = AssetDatabase.GetDependencies(new string[1] { data.path });
                    fbxFile = null;
                    foreach (string objPath in dependencies)
                    {
                        if (objPath.ToLower().EndsWith("fbx"))
                        {
                            fbxFile = objPath;
                            break;
                        }
                    }

                    if (fbxFile != null)
                    {
                        importer = ModelImporter.GetAtPath(fbxFile) as ModelImporter; 
                    }
                }

                //Èç¹ûÕÒµœModelImporter, Ôò¿ªÊŒÉŸ³ý¶¯×÷
                if (importer != null)
                {
                    ModelImporterClipAnimation clipAnimation = null;
                    clipAnimationArray = importer.clipAnimations;
                    for (int j = 0; j < importer.clipAnimations.Length; j++)
                    {
                        clipAnimation = importer.clipAnimations[j];
                        if (clipAnimation.name == "idle")
                        {
                            break;
                        }
                    }

                    if (clipAnimation != null)
                    {
                        importer.clipAnimations = new ModelImporterClipAnimation[1] { clipAnimation };
                    }
                    else
                    {
                        importer.clipAnimations = new ModelImporterClipAnimation[0];
                    }
                    AssetDatabase.ImportAsset(fbxFile);
                }
            }


            //ÊÇ·ñºÍCommonObjŽæÔÚ¹ØÁª¹ØÏµ
            if (isMustSetDependencis && property.isPushDependencies)
            {
				BuildPipeline.PushAssetDependencies();
			}

            UnityEngine.Object exportObj = null;
            exportObj = AssetDatabase.LoadMainAssetAtPath(data.path);

            //ÊÇ·ñœøÐÐÑ¹Ëõ
            if (property.isCompress)
            {
                BuildPipeline.BuildAssetBundle(exportObj,
                                                null,
                                                assetExportPath,
                                                compressOptions,
                                                buidleTarget);
            }
            else
            {
                BuildPipeline.BuildAssetBundle(exportObj,
                                               null,
                                               nonExportPath,
                                               uncompressOptions,
                                               buidleTarget);

                ZipLibUtils.CompressFile(nonExportPath, assetExportPath);
            }

            if (isMustSetDependencis && property.isPushDependencies)
            {
				BuildPipeline.PopAssetDependencies();
			}

            //»ÖžŽFBXÎÄŒþ¶¯×÷ÐÅÏ¢
            if (property.isDeleteAnimation && importer != null && clipAnimationArray != null && fbxFile != null )
            {
                importer.clipAnimations = clipAnimationArray;

                AssetDatabase.ImportAsset(fbxFile);
            }


            //Compress Assetbundle
            //LZMA_Util.CompressFileLZMA(unusePath + "/" + data.fileName + ".unity3d", assetBundlePath + "/" + data.fileName + ".unity3d");
             
            data.needUncompress = property.isCompress;
            data.isExport       = true;
            data.exportPath     = assetExportPath;

            //data.exportPath = exportData + "/" + data.exportPath.Substring(data.exportPath.IndexOf(beignStringName));
            data.dataSize = GetFileSize(assetExportPath);
            return true;
        }
		/*
		public bool ExportAssetPackage( AssetsGroup assetGroup , bool isSetDependencis = false )
		{
			if ( assetGroup == null ) 
			{
				Debug.Log( "Assets Group is NULL " );
				return false;
			}

		    string exportPath = defaultExportPath; // AssetPathGroupManager.Instance.assetPathGroup.assetBundleExportPath;
            int beginIndex = assetGroup.groupPath.IndexOf("/");
//            int endIndex = data.path.LastIndexOf("/");
            string assetBundlePath = exportPath + assetGroup.groupPath.Substring(beginIndex);
            CheckAndCreateDirectory(assetBundlePath);
			
			string tmpExportPath = assetBundlePath + "/" + assetGroup.groupName + ".unity3d";
			string assetExportPath = exportData + "/" + tmpExportPath.Substring(tmpExportPath.IndexOf(beignStringName));
			
			List< string > objNameList = new List<string>();
			List< UnityEngine.Object > objList = new List<UnityEngine.Object>();
			AssetSaveData firstData = null;
			foreach( KeyValuePair< string, AssetSaveData> item in assetGroup.assets )
			{
				AssetSaveData data = item.Value;
				
				if ( firstData == null ) firstData = item.Value;
				
				data.version = long.Parse(DateTime.Now.ToString("yyMMddHHmm"));
            	data.versionTime = DateTime.Now.ToString();
            	data.md5 = data.changMd5;
				
				string path = dataPath + data.path;
            	data.fileTime = File.GetLastWriteTime(path).ToString();
				
            	
				
				data.needUncompress = true;
            	data.isExport       = true;
				
				data.dataSize       = 0;
				data.exportPath     = assetExportPath;
				objNameList.Add( data.fileName );
				objList.Add( AssetDatabase.LoadMainAssetAtPath(data.path) );
			}
			
			CheckAndCreateDirectory(unusePath);
			
			if ( objNameList.Count > 0)
			{
				if ( isSetDependencis ) 
				{
					BuildPipeline.PushAssetDependencies();
				}
				
				
	            BuildPipeline.BuildAssetBundleExplicitAssetNames( 
												objList.ToArray(),
	                                            objNameList.ToArray(),
	                                            unusePath + "/" + assetGroup.groupName + ".unity3d",
	                                            compressOptions,
	                                            buidleTarget);
				
				if ( isSetDependencis ) 
				{
					BuildPipeline.PopAssetDependencies();
				}			
				
		
				if ( firstData != null )
				{
					firstData.exportPath = tmpExportPath;
					//AddCompressPath(firstData);	
				}
							
			}

            return true;
		}
		*/
		
		
        //private void AddCompressPath(AssetSaveData data )
        //{
        //    if (_compressAssetsList == null)
        //    {
        //        _compressAssetsList = new List<AssetSaveData>();
        //    }

        //    _compressAssetsList.Add(data);
        //}

        private uint GetFileSize(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    uint size = (uint)fs.Length / 1024;

                    fs.Close();
                    return size;
                }
            }
            else
            {
                return 0;
            }
        }

        private string GetExportFolder( AssetSaveData data )
        {
            //if (AssetPathGroupManager.Instance.assetPathGroup.assetBundleExportPath.Length == 0)
            //{
            //    AssetPathGroupManager.Instance.assetPathGroup.assetBundleExportPath = defaultExportPath;
            //    AssetPathGroupManager.Instance.SaveAssetPathGroup();
            //}

            string exportPath = backupExportPath;//defaultExportPath; // AssetPathGroupManager.Instance.assetPathGroup.assetBundleExportPath;
            int beginIndex = data.path.IndexOf("/");
            int endIndex = data.path.LastIndexOf("/");
            string assetBundlePath = exportPath + data.path.Substring(beginIndex + 1, (endIndex - beginIndex-1));
            CheckAndCreateDirectory(assetBundlePath);

            return assetBundlePath;
        }

        private void CheckAndCreateDirectory(string fullpath)
        { 
            if( Directory.Exists( fullpath )) return;

            Directory.CreateDirectory(fullpath);
        }

        public void DeleteOneAssetBundle( string path )
        {
            if (File.Exists(path))
            {
                File.Delete( path );
            }
        }
		
        /// <summary>
        /// Žò°üËùÓÐËùÓÐÊýŸÝ
        /// </summary>
        /// <param name="assetPaths"></param>
        /// <returns>·µ»ØÓÐÒÑŽò°üÊýŸÝ</returns>
		public List< AssetSaveData > ExportAllAssets( AssetPaths assetPaths )
		{
            List<AssetSaveData> buildAssetList = new List<AssetSaveData>();

            //·ÖÀàÐÅÏ¢
            List<AssetsGroup> dependencisList = new List<AssetsGroup>();
            List<AssetsGroup> dontDependencisList = new List<AssetsGroup>();
            foreach (KeyValuePair<string, AssetsGroup> item in assetPaths.assetPaths)
            {
                if (item.Value.exportProperty.isPushDependencies)
                {
                    dependencisList.Add(item.Value);
                }
                else
                {
                    dontDependencisList.Add(item.Value);
                }
            }

			BuildPipeline.PushAssetDependencies();
			{
                buildAssetList.AddRange(ExportSaveData(assetPaths.commonPaths, true, false, false));
				buildAssetList.AddRange(ExportWithScene( assetPaths ));
                buildAssetList.AddRange(ExportAssetsGroup(dependencisList));
        		//buildAssetList.AddRange(ExportSaveData(assetPaths.assetPaths));
			}
			BuildPipeline.PopAssetDependencies();

            buildAssetList.AddRange(ExportAssetsGroup(dontDependencisList));

            dependencisList.Clear();
            dontDependencisList.Clear();

            dependencisList = null;
            dontDependencisList = null;

            return buildAssetList;
		}
		

        /// <summary>
        /// Žò°üAssetGroupÀïÃæÎÄŒþ
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="mustExport"></param>
        /// <param name="TotalPackFlag"></param>
        /// <param name="TotalDependencisFlag"></param>
        /// <returns>·µ»ØÒÑŽò°üµÄÎÄŒþ</returns>
		private List<AssetSaveData> ExportSaveData( Dictionary< string, AssetsGroup > dic, bool mustExport = false, bool TotalPackFlag = true,  bool TotalDependencisFlag = true )
        {
            List<AssetSaveData> buildAssetList = new List<AssetSaveData>();

			foreach( KeyValuePair< string, AssetsGroup > group in dic )
			{
                //if ( groups.Value.isPushIntoOneAssetbundle && TotalPackFlag )
                //{
                //    ExportAssetPackage( groups.Value, groups.Value.isPushDependencies && TotalDependencisFlag );
                //}
                //else
                //{
		            foreach (KeyValuePair<string, AssetSaveData> item in group.Value.assets)
		            {
                        if (item.Value.isDirty || mustExport)
		                {
                            if ( mustExport ) item.Value.isDirty = true;
                            
                            //Èç¹ûÊÇÓŠÓÃÒÑŸ­ÐÞžÄ£¬ Ôò±ØÐëÖØÐÂŽò°ü
                            if (item.Value.isReferenceDirty) item.Value.isDirty = true;

		                    AssetSaveData data = item.Value;
                            buildAssetList.Add(data);
		                    AssetBundleExport.Instance.ExportOneAsset(data , group.Value.exportProperty, TotalDependencisFlag);
		                }
		            }							
				//}
			}

            return buildAssetList;
        }

        private List<AssetSaveData> ExportAssetsGroup(List< AssetsGroup >  assetGroupList, bool mustExport = false, bool TotalPackFlag = true, bool TotalDependencisFlag = true)
        {
            List<AssetSaveData> buildAssetList = new List<AssetSaveData>();

            foreach (AssetsGroup assetGroup in assetGroupList)
            {
                foreach (KeyValuePair<string, AssetSaveData> item in assetGroup.assets)
                {
                    if (item.Value.isDirty || mustExport)
                    {
                        if (mustExport) item.Value.isDirty = true;

                        //Èç¹ûÊÇÓŠÓÃÒÑŸ­ÐÞžÄ£¬ Ôò±ØÐëÖØÐÂŽò°ü
                        if (item.Value.isReferenceDirty) item.Value.isDirty = true;

                        AssetSaveData data = item.Value;
                        buildAssetList.Add(data);
                        AssetBundleExport.Instance.ExportOneAsset(data, assetGroup.exportProperty, TotalDependencisFlag);
                    }
                } 
            }
            return buildAssetList;
        }
		
		
        /// <summary>
        /// Žò°üÒÑ±êŒÇµÄ³¡Ÿ°
        /// </summary>
        /// <param name="assetPaths"></param>
		public List<AssetSaveData> ExportWithScene( AssetPaths assetPaths )
		{
            List<AssetSaveData> buildAssetList = new List<AssetSaveData>();
			
			Dictionary< string , AssetSaveData > scenePath = assetPaths.scenePaths["Scenes"].assets;
			
            foreach (KeyValuePair<string, AssetSaveData> item in scenePath)
            {
                BuildPipeline.PushAssetDependencies();
				{
                    AssetSaveData data = item.Value;//assetPaths.scenePaths[item.Key];
                    if (data.isDirty && data.isExist)
                    {
                        buildAssetList.Add(data);

                        string[] levels = new string[1] { data.path };
                        string exportPath = GetExportFolder(data);

                        data.version = long.Parse( DateTime.Now.ToString("yyMMddHHmm"));
                        data.versionTime = DateTime.Now.ToString();
                        data.md5 = data.changMd5;
                        //data.changMd5 = "";
                        data.isExport = true;

                        string path = dataPath + data.path;
                        data.fileTime = File.GetLastWriteTime(path).ToString();

                        string assetExportPath = exportPath + "/" + data.fileName + ".unity3d";
                        Debug.Log("AssetExportPath : " + assetExportPath); 

                        BuildPipeline.BuildStreamedSceneAssetBundle(levels,
                                                                    assetExportPath,
                                                                    buidleTarget);

                        data.needUncompress = true;
                        data.exportPath = assetExportPath;
                        //data.exportPath = exportData + "/" + data.exportPath.Substring(data.exportPath.IndexOf(beignStringName));
                        data.dataSize = GetFileSize(assetExportPath);
                    }
				}
				BuildPipeline.PopAssetDependencies();
            }

            return buildAssetList;
		}


        /// <summary>
        /// µŒ³öºÏÆŽ°ü
        /// </summary>
        /// <param name="assetSaveDataList"></param>
        /// <param name="packageMinSize"> ºÏÆŽ°ü×îÐ¡ŽóÐ¡( µ¥Î» £ºKB)</param>
        public void ExportMergedPackage(List<AssetSaveData> assetSaveDataList, int packageMinSize )
        {
            int index = 1;
            uint accumulativeLength = 0;
            List<AssetSaveData> mergedPackageData = new List<AssetSaveData>();
            for ( int i = 0 ; i <  assetSaveDataList.Count ; i ++)
            {
                AssetSaveData saveData = assetSaveDataList[i];
                EditorUtility.DisplayProgressBar("ÕýÔÚœøÐÐ×ÊÔŽºÏÆŽ....",
                                saveData.fileName + "(" + i + " / " + assetSaveDataList.Count + ")",
                                ((float)i) / assetSaveDataList.Count);

                if (accumulativeLength > packageMinSize)
                {
                    _Merge(mergedPackageData, index);

                    index++;

                    mergedPackageData.Clear();
                    accumulativeLength = 0;
                }

                mergedPackageData.Add(saveData);
                accumulativeLength += saveData.dataSize;
            }

            if (mergedPackageData.Count > 0)
            {
                _Merge(mergedPackageData, index);
            }
            index++;
            mergedPackageData.Clear();
            accumulativeLength = 0;


            EditorUtility.ClearProgressBar();
        }

        private struct AssetFile
        {
            public byte[] fileBytes;

            public long fileLen;

            public AssetFile(byte[] fileBytes, long fileLen)
            {
                this.fileBytes = fileBytes;

                this.fileLen = fileLen;
            }
        }

        /// <summary>
        /// ºÏÆŽ×ÊÔŽ
        /// </summary>
        /// <param name="assetSaveData"></param>
        private void _Merge(List<AssetSaveData> assetSaveDataList, int index)
        {

            string exportPath = defaultExportPath;
            CheckAndCreateDirectory(exportPath);
            string mergePackageFilePath = exportPath + "/" + index + ".baoyu";

            AssetByteArray byteArray = new AssetByteArray();
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                fs = new FileStream(mergePackageFilePath, FileMode.Create);
                bw = new BinaryWriter(fs);
            }
            catch (Exception e)
            {
                Debug.Log("Merge Func : index" + index + "-" + e.Message);
            }

            Debug.Log(" ============== Merge Package  : " + index + " =================== "); 
            List< AssetFile > assetFileList = new List<AssetFile>();
            for (int i = 0 ; i <  assetSaveDataList.Count ; i ++ )
            {
                AssetSaveData saveData = assetSaveDataList[i];
                saveData.package    = exportData;
                saveData.packageNum = index;
                saveData.packagePos = i;

                Debug.Log(" ---> Merge Asset : " + saveData.fileName);
                try
                {
                    FileStream fileStream = new FileStream(saveData.exportPath, FileMode.Open);
                    BinaryReader reader = new BinaryReader(fileStream);

                    byte[] bytes = reader.ReadBytes((int)fileStream.Length);

                    assetFileList.Add( new AssetFile( bytes, fileStream.Length ));

                    reader.Close();
                    fileStream.Close();
                }
                catch (Exception e)
                { 
                    Debug.LogError("Merge Func : Get Save Date " + saveData.fileName  + "-" + e.Message); 
                }
            }


            //¿ªÊŒÐŽÈëÕû°ü
            uint offset = 0;

            //ÎÄŒþÍ·ÐÅÏ¢
            //bw.Write("BAOYU");
            byteArray.WriteUTFTiny("BAOYU");

            //ÎÄŒþ±êÊŸ
            //bw.Write(0x23AC);
            byteArray.WriteUnsignedInt(0x23AC);

            //ÎÄŒþºÏÆŽµÄÊýÁ¿
            //bw.Write(assetSaveDataList.Count);
            byteArray.WriteUnsignedInt( (uint)assetSaveDataList.Count);

            offset = byteArray.Length + (uint)assetSaveDataList.Count * 8;

            foreach ( AssetFile assetFile in assetFileList)
            {
                byteArray.WriteUnsignedInt( offset );                   // ---4byte
                byteArray.WriteUnsignedInt((uint)assetFile.fileLen);   // ---4byte
                offset += (uint)assetFile.fileLen;
            }

            foreach (AssetFile assetFile in assetFileList)
            {
                byteArray.WriteBytes(assetFile.fileBytes);
                //bw.Write(assetFile.fileBytes);     // --assetFile.len
                //offset += (int)assetFile.fileLen;
            }

            byteArray.WriteShort(0x4A);

            bw.Write(byteArray.ToArray());
            bw.Close();
            fs.Close();

            assetFileList.Clear();

        }

        /// <summary>
        /// Export All UI Assets
        /// </summary>
        /// <param name="assetPaths"></param>
//        public void ExportWithUI(AssetPaths assetPaths)
//        {
//            Dictionary<string, AssetSaveData> pathSaveDataDic = new Dictionary<string, AssetSaveData>();
//
//            //Export Commone Atlas 
//            foreach (KeyValuePair<string, AssetSaveData> atlasItem in assetPaths.atlasAssetsPaths)
//            {
//                 AssetSaveData atlasData = atlasItem.Value;
//                if (atlasData.isDirty)
//                {
//                    ExportOneAsset(atlasData);
//                }
//
//                pathSaveDataDic.Add(atlasData.path, atlasData);
//            }
//
//
//
//            AssetSaveData data = null;
//            foreach (KeyValuePair<string, AssetSaveData> uiItem in assetPaths.uiAssetsPaths)
//            {
//                data = uiItem.Value;
//                if (data.isDirty && data.isExist)
//                {
//                    List<AssetSaveData> assetSaveDataList = new List<AssetSaveData>();
//                    string[] dependencises  = AssetDatabase.GetDependencies( new string[1] { data.path } );
//                    foreach (string dependence in dependencises)
//                    {
//                        if (pathSaveDataDic.ContainsKey(dependence))
//                        {
//                            assetSaveDataList.Add(pathSaveDataDic[dependence]);
//                        }
//                    }
//
//
//                    BuildPipeline.PushAssetDependencies();
//                        //Export Commone Atlas 
//                        foreach (AssetSaveData atlasData in assetSaveDataList)
//                        {
//                            CheckAndCreateDirectory(unusePath);
//                            BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath(atlasData.path),
//                                                            null,
//                                                            unusePath + "/" + atlasData.fileName + ".unity3d",
//                                                            options,
//                                                            buidleTarget);
//                        }
//
//                        BuildPipeline.PushAssetDependencies();
//                            ExportOneAsset(data);
//                        BuildPipeline.PopAssetDependencies();
//
//                    BuildPipeline.PopAssetDependencies();
//                }
//            }
//        }

        ///// <summary>
        ///// Compress all assetbundle with lzma
        ///// </summary>
        //private int _curCompressNum = 0; 
        //private int _totalCompressNum = 0;
        //private string _compressMessage;
        //private bool _isCompressError = false;
        //public string CompressAllAssets()
        //{
        //    if (_compressAssetsList == null || (_compressAssetsList != null && _compressAssetsList.Count <= 0))
        //    {
        //        return String.Empty;
        //    }

        //    _compressMessage  = String.Empty;
        //    _isCompressError  = false;
        //    _curCompressNum   = 0;
        //    _totalCompressNum = _compressAssetsList.Count;

        //    //Thread thread = new Thread(new ThreadStart(_Compress));
        //    //thread.Start();

        //    //while (_curCompressNum < _totalCompressNum || _isCompressError)
        //    //{
        //    //    EditorUtility.DisplayProgressBar(
        //    //                      " Compress Asset Data ",
        //    //                      " Progress : " + _curCompressNum + " / " + _totalCompressNum,
        //    //                      (float)_curCompressNum / _totalCompressNum
        //    //                      );
        //    //}

        //    _Compress();


        //    _compressAssetsList.Clear();
        //    EditorUtility.ClearProgressBar();

        //    if (_isCompressError)
        //    {
        //        Debug.Log(_compressMessage);
        //    }
        //    return _compressMessage;
        //}

        //private void _Compress()
        //{
        //    foreach (AssetSaveData data in _compressAssetsList)
        //    {
        //        string exportPath = data.exportPath;
				
        //        string []pathString = exportPath.Split( new char[2]{'.', '/'});
        //        string fileName = pathString[pathString.Length - 2 ];
				
        //        try
        //        {
        //            //Compress Assetbundle
        //            LZMA_Util.CompressFileLZMA(unusePath + "/" + fileName + ".unity3d", exportPath);

        //            data.needUncompress = true;
        //            data.isExport = true;
        //            data.exportPath = exportData + "/" + exportPath.Substring(exportPath.IndexOf(beignStringName));
        //            data.dataSize = GetFileSize(exportPath);
        //        }
        //        catch(Exception exception )
        //        {
        //            _compressMessage = ("Compress Error :" +  exception.Message);
        //            _isCompressError = true;
        //            break;
        //        }

        //        Debug.Log("exportPath : " + exportPath);
        //        EditorUtility.DisplayProgressBar(
        //                          " Compress Asset Data ",
        //                          " Progress : " + _curCompressNum + " / " + _totalCompressNum,
        //                          (float)_curCompressNum / _totalCompressNum
        //                          );

        //        _curCompressNum++;
        //        //System.Threading.Interlocked.Increment(ref _curCompressNum);
        //    }
        //}
	}	
}

