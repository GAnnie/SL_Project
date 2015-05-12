// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetManagerWin.cs
// Author   : wenlin
// Created  : 2013/3/27 
// Purpose  : 
// **********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace AssetBundleEditor
{
	public class AssetManagerWin : EditorWindow
	{
		//[MenuItem( "AssetBundle/Show Assets List" )]
		public static void ShowWin()
		{
			EditorWindow.GetWindow( typeof( AssetManagerWin ));
		}
		
		public AssetManagerWin ()
		{
			
		}


#if UNITY_ANDROID
        private BuildTarget buildTarget = BuildTarget.Android;
#elif UNITY_STANDALONE
        private BuildTarget buildTarget = BuildTarget.StandaloneWindows;
#elif UNITY_IPHONE
        private BuildTarget buildTarget = BuildTarget.iPhone;
#endif
        private int totoalDirtyNumber = 0;
        private bool exportFlag = false; 
        private void Update()
        {
            if (exportFlag)
            {
                exportFlag = false;

                if (exportName == "")
                {
                    EditorUtility.DisplayDialog("提示", "请输入导出者名字", "OK");
                    return;
                }

                string tempYes = exportYes.ToLower();
                if (tempYes != "yes")
                {
                    EditorUtility.DisplayDialog("提示", "导出请输入YES", "OK");
                    return;
                }

				PlayerSettingTool.ShowWinWithExport(GameSettingCallBack);
            }

            if ( readToExport )
            {
                readToExport = false;
                string errorMessage = AssetManager.Instance.ExportAssetBundle(exportName, buildTarget, MessageFunction);
                if (errorMessage != "")
                {
                    //EditorUtility.DisplayDialog("ERROR", errorMessage, "OK");
                    exportName = "";
                    exportYes = "";
                    AssetBundleExportLogManager.Instance.End();
                    return;
                }
                else
                {
                    exportName = "";
                    exportYes = "";
                }
            }
        }

        private bool readToExport = false;
        private void GameSettingCallBack()
        {
            readToExport = true;
        }

        private void MessageFunction(string message)
        {
            EditorUtility.DisplayDialog("Message", message, "OK");
        }
		
		private Vector2 scrollPos;
		private Vector2 scrollPos2;
        private string exportName = "";
        private string exportYes = "";
		
		private class flagObj
		{
			public bool flag = false;
			public Vector2 sco;
		}
		
        private Dictionary<string, flagObj> _foldOutFlag = new Dictionary<string, flagObj>();
		private void OnGUI()
		{
            totoalDirtyNumber = 0;
			EditorGUILayout.Space();
			scrollPos = EditorGUILayout.BeginScrollView( scrollPos, GUILayout.Width( position.width ), GUILayout.Height( position.height ));
			EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(" ApplicactionDataPath :" , Application.dataPath);
                Head();

				if ( AssetManager.Instance.isGettingAssetPath && AssetManager.Instance.isProcess )
				{
                    //Repaint();
                //}
                //else if ( AssetManager.Instance.isProcess )
                //{
					EditorUtility.DisplayProgressBar( " Update Asset Data ", 
													  " Progress : " + AssetManager.Instance.curProcessAssetNum.ToString() + " / " + AssetManager.Instance.totalAssetNum.ToString() ,
													  ( float )AssetManager.Instance.curProcessAssetNum / AssetManager.Instance.totalAssetNum );
                    //Repaint();
				}
				else
				{

					EditorUtility.ClearProgressBar();
                    if (ShowSameDataName())
                    {
                        if (AssetManager.Instance.isUpdateAssetPaths)
                        {
                            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Width(position.width ), GUILayout.Height(600));
                            {
                                ShowAssetSaveData();
                                //ShowConfigData();
                                ShowCommonOBJData();
                                //ShowUIData();
                                ShowSceneData();

                                ShowAllShader();
                            }
                            EditorGUILayout.EndScrollView();
                        }
                        
						EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (GUILayout.Button(" Update Assets Data "))
                        {
                            AssetManager.Instance.UpdateAssetPaths();
                        }
					
						if (GUILayout.Button(" Change Asset Data Format with Reference List "))
						{
							AssetManager.Instance.ExportResourceVersionDataFormat();
						}
                        
                        ExportView(); 
                    }
                }
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}


        private void Head()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ---Assets Path List---");
            if (GUILayout.Button("缩回", GUILayout.Width(50)))
            {
                if (_foldOutFlag != null)
                {
                    List<string> keys = new List<string>();
                    foreach (string key in _foldOutFlag.Keys)
                    {
                        keys.Add(key);
                    }

                    foreach (string key in keys)
                    {
                        _foldOutFlag[key].flag = false;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ShowAllShader()
        {
            //EditorGUILayout.BeginVertical();
            //{
                GameEditorUtils.Space(1);
                GameEditorUtils.PartitionLine();
                EditorGUILayout.LabelField("所有资源引用的Shader ：");
                foreach (KeyValuePair<string, string> item in AssetManager.Instance.allShader)
                {
                    EditorGUILayout.LabelField(item.Key, GUILayout.Width(600));
                }
                GameEditorUtils.Space(1);
                GameEditorUtils.PartitionLine();
            //}
           // EditorGUILayout.EndVertical();
        }

       
        private void ExportView()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("------------------------------", "---------------------------------");
            EditorGUILayout.LabelField("新添加或则已修改资源总数：", totoalDirtyNumber.ToString());

            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("资源导出平台 :", buildTarget);
            exportName = EditorGUILayout.TextField("导出人名字：", exportName, GUILayout.Width(300));

            EditorGUILayout.BeginHorizontal();
            exportYes = EditorGUILayout.TextField("确定导出请输入 YES：", exportYes, GUILayout.Width(300));
            if (GUILayout.Button(" Export Asset Data"))
            {
                exportFlag = true;
            }
            EditorGUILayout.EndHorizontal();
        }

        private bool ShowSameDataName()
        {
            if (AssetManager.Instance.sameNameList.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Warning--------------------------", "-------------------Warning");
                EditorGUILayout.LabelField("Same Assets Name ");
                foreach (string name in AssetManager.Instance.sameNameList)
                {
                    EditorGUILayout.LabelField(name);
                }
                EditorGUILayout.LabelField("Warning--------------------------", "-------------------Warning");
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ShowAssetSaveData( )
        {
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic)
            {
				if ( AssetManager.Instance.assetPaths.assetPaths.ContainsKey( item.Key ) )
				{
					FoldOut(item.Key, AssetManager.Instance.assetPaths.assetPaths[item.Key]);
				}
            }
        }

        //private void ShowConfigData()
        //{

        //    EditorGUILayout.Space();
        //    EditorGUILayout.Space();
        //    EditorGUILayout.LabelField("------------------------------------", "----------------------------");
        //    foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.configPathDic)
        //    {
        //        if ( AssetManager.Instance.assetPaths.configPaths.ContainsKey( item.Key ) )
        //        {
        //            FoldOut(item.Key, AssetManager.Instance.assetPaths.configPaths[item.Key]);
        //        }
        //    }
        //}
		
		private void ShowCommonOBJData()
        {

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("------------------------------------", "----------------------------");
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.commonObjectPathDic)
            {
				if ( AssetManager.Instance.assetPaths.commonPaths.ContainsKey( item.Key ) )
				{
					FoldOut(item.Key, AssetManager.Instance.assetPaths.commonPaths[item.Key]);
				}
            }
        }
		
        private bool uiFlag = false;
//        private void ShowUIData()
//        {
//            EditorGUILayout.Space();
//            EditorGUILayout.Space();
//            EditorGUILayout.LabelField("------------------------------------", "----------------------------");
			
			
			
////            foreach (KeyValuePair<string, AssetsGroup> groups in AssetManager.Instance.assetPaths.uiAssetsPaths)
////            { 
////				foreach( KeyValuePair< string, AssetSaveData > item in groups.Value.assets )
////				{
////					if (item.Value.isDirty) totoalDirtyNumber++;	
////				}
////			}
////
////            foreach (KeyValuePair<string, AssetsGroup> groups in AssetManager.Instance.assetPaths.atlasAssetsPaths)
////            { 
////				foreach( KeyValuePair< string, AssetSaveData > item in groups.Value.assets )
////				{
////					if (item.Value.isDirty) totoalDirtyNumber++;	
////				}
////			}

//            EditorGUILayout.PrefixLabel("-----All Atlas Files ---");
//            if ( AssetManager.Instance.assetPaths.atlasAssetsPaths.ContainsKey( "Atlas" ) )
//            {
//                FoldOut( "Atlas", AssetManager.Instance.assetPaths.atlasAssetsPaths["Atlas"]);
//            }
			

//            /*
//            uiFlag = EditorGUILayout.Foldout(uiFlag, " Atlas_UI Panel");
//            if (uiFlag)
//            {
				
				
//                foreach (KeyValuePair<string, AssetsGroup> groups in AssetManager.Instance.assetPaths.uiAssetsPaths)
//                { 
					
//                    foreach( KeyValuePair< string, AssetSaveData > item in groups.Value.assets )
//                    {
//                        if (item.Value.isDirty) totoalDirtyNumber++;	
//                    }
//                }
				
				
//                foreach (KeyValuePair<string, AssetSaveData> item in AssetManager.Instance.GetAtlasList)
//                {
//                    string word = "altas_" + item.Key;
//                    AssetSaveData data = item.Value;
//                    //if (data.isDirty) totoalDirtyNumber++;

//                    if (!_foldOutFlag.ContainsKey(word))
//                    {
//                        _foldOutFlag.Add(word, false);
//                    }

//                    if (!data.isExist)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField("   ", GUILayout.Width(20f));
//                        _foldOutFlag[word] = EditorGUILayout.Foldout(_foldOutFlag[word], word + " --No Exist");
//                        EditorGUILayout.EndHorizontal();
//                    }
//                    else
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        data.isDirty = EditorGUILayout.Toggle(data.isDirty, GUILayout.Width(20f));
//                        _foldOutFlag[word] = EditorGUILayout.Foldout(_foldOutFlag[word], word);
//                        EditorGUILayout.EndHorizontal();
//                    }

//                    if (_foldOutFlag[word])
//                    {
//                        FoldOutAssetData(data);
//                    }
//                }
				
//                */
				
//                EditorGUILayout.Space();
//                EditorGUILayout.Space();
//                EditorGUILayout.PrefixLabel("----All UI Files ----");
//                if ( AssetManager.Instance.assetPaths.uiAssetsPaths.ContainsKey( "AllUI" ) )
//                {
//                    FoldOut( "AllUI", AssetManager.Instance.assetPaths.uiAssetsPaths["AllUI"]);
//                }
			
//                /*
//                foreach (KeyValuePair<string, AssetSaveData> item in AssetManager.Instance.GetUIList)
//                {
//                    string word = "UI_" + item.Key;
//                    AssetSaveData data = item.Value;

//                    //if (data.isDirty) totoalDirtyNumber++;

//                    if (!_foldOutFlag.ContainsKey(word))
//                    {
//                        _foldOutFlag.Add(word, false);
//                    }

//                    if (!data.isExist)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        EditorGUILayout.LabelField("   ", GUILayout.Width(20f));
//                        _foldOutFlag[word] = EditorGUILayout.Foldout(_foldOutFlag[word], word + " --No Exist");
//                        EditorGUILayout.EndHorizontal();
//                    }
//                    else
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        data.isDirty = EditorGUILayout.Toggle(data.isDirty, GUILayout.Width(20f));
//                        _foldOutFlag[word] = EditorGUILayout.Foldout(_foldOutFlag[word], word);
//                        EditorGUILayout.EndHorizontal();
//                    }

//                    if (_foldOutFlag[word])
//                    {
//                        FoldOutAssetData(data);
//                    }
//                } 
                
//                */
            
//        }

        private void ShowSceneData()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("------------------------------------", "----------------------------");
            EditorGUILayout.PrefixLabel("----All Scene Files ---");
		
			if ( AssetManager.Instance.assetPaths.scenePaths.ContainsKey( "Scenes" ) )
			{
				FoldOut( "Scenes", AssetManager.Instance.assetPaths.scenePaths["Scenes"]);
			}

        }


		private void FoldOut( string groupName ,  AssetsGroup assetGroup )
		{
			if ( !_foldOutFlag.ContainsKey( groupName ))
			{
				_foldOutFlag.Add( groupName, new flagObj() );
			}
			
			Dictionary< string , AssetSaveData > dic = assetGroup.assets;

            int totalNum = 0;
            int groupNum = 0;
			foreach( KeyValuePair< string, AssetSaveData > data in dic )
			{
                if (data.Value.isDirty)
                {
                    groupNum++;
                    totoalDirtyNumber++; 
                }

                totalNum++;
			}

			GUI.color = Color.yellow;
            _foldOutFlag[groupName].flag = EditorGUILayout.Foldout(_foldOutFlag[groupName].flag, groupName + "(" + groupNum + "/" + totalNum +  ")");
			    
            GUI.color = Color.green;
			//assetGroup.isPushDependencies 		= EditorGUILayout.Toggle( "关联", assetGroup.isPushDependencies);
			//assetGroup.isPushIntoOneAssetbundle = EditorGUILayout.Toggle( "合拼", assetGroup.isPushIntoOneAssetbundle);

            EditorGUILayout.BeginHorizontal();
            {			
				if ( GUILayout.Button( "全选", GUILayout.Width(50) )){ assetGroup.SelectAllDirty( true );}
				if ( GUILayout.Button( "全否", GUILayout.Width(50) )){ assetGroup.SelectAllDirty( false );}
			}
			EditorGUILayout.EndHorizontal();
			GUI.color = Color.white;
			
			if ( _foldOutFlag[groupName].flag )
			{
				
				EditorGUILayout.Space();
				
	                //foreach( KeyValuePair< string , AssetSaveData > item in dic )
	                foreach( KeyValuePair<string ,AssetSaveData >  item in dic )
					{
						AssetSaveData data = item.Value;
						
	                    string groupFileName = data.path; //+ "/" + data.fileName;
	                    if (!_foldOutFlag.ContainsKey(groupFileName))
	                    {
	                        _foldOutFlag.Add(groupFileName, new flagObj() );
	                    }
	
	                    if (data.isExist)
	                    {
	                        //string groupFileName = groupName + "/" + data.fileName;
	                        //if (!_foldOutFlag.ContainsKey(groupFileName))
	                        //{
	                        //    _foldOutFlag.Add(groupFileName, false);
	                        //}
	                        EditorGUILayout.BeginHorizontal();
	                        data.isDirty = EditorGUILayout.Toggle(data.isDirty, GUILayout.Width(20f));
	
	                        _foldOutFlag[groupFileName].flag = EditorGUILayout.Foldout(_foldOutFlag[groupFileName].flag, /*data.fileName*/groupFileName);
	                        if (_foldOutFlag[groupFileName].flag)
	                        {
	                            FoldOutAssetData(data);
	                        }
	
	                        //					EditorGUILayout.LabelField( item.Key , item.Value.path );
	                        EditorGUILayout.EndHorizontal();
	                    }
	                    else
	                    {
	                        EditorGUILayout.BeginHorizontal();
	                        EditorGUILayout.LabelField("   ", GUILayout.Width(20f));
	                        _foldOutFlag[groupFileName].flag = EditorGUILayout.Foldout(_foldOutFlag[groupFileName].flag, data.fileName + " --No Exist");
	                        if (_foldOutFlag[groupFileName].flag)
	                        {
	                            FoldOutAssetData(data);
	                        }
	                        EditorGUILayout.EndHorizontal();
	                        totoalDirtyNumber++;
	                    }
					}
				EditorGUILayout.Space();
			}
		}
		
		private void FoldOutAssetData( AssetSaveData data )
		{
			EditorGUILayout.BeginVertical();
				EditorGUILayout.Space();	
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Version"  , data.version.ToString());
				EditorGUILayout.LabelField( "VerTime"  , data.versionTime 	    );
				EditorGUILayout.LabelField( "MD5"      , data.md5 				);
				EditorGUILayout.LabelField( "ChangMD5" , data.changMd5 			);
				EditorGUILayout.LabelField( "FileTime" , data.fileTime			);
				EditorGUILayout.LabelField( "Path"     , data.path    			);
				EditorGUILayout.LabelField( "GUID"     , data.guid    			);
				EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}


        private void OnInspectorUpdate()
        {
            Repaint();
        }
	}

}

