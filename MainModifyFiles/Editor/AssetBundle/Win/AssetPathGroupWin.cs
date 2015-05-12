// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPathGroupWin.cs
// Author   : wenlin
// Created  : 2013/3/27 
// Purpose  : 
// **********************************************************************
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace AssetBundleEditor
{
	public class AssetPathGroupWin : EditorWindow
	{
		public AssetPathGroupWin ()
		{
		}
		
		[MenuItem( "AssetBundle/AssetPathGroupSetting")]
		public static void ShowWin()
		{
			EditorWindow.GetWindow( typeof(  AssetPathGroupWin ));
		}
		
		private Vector2 scrollPos;
		private string delGroupName = "";
        private string delConfigGroupName = "";
        private string delMovieName = "";
		private string delCommonGroupName = "";
		private string delMusicSoundGroupName = "";
        //private string delAtlasName = "";
		private void OnGUI()
		{
			EditorGUILayout.Space();
			scrollPos = EditorGUILayout.BeginScrollView( scrollPos, GUILayout.Width( position.width ), GUILayout.Height( position.height ));
			EditorGUILayout.BeginVertical();

                AssetPatGroupSelect();
				AssetCommonSelect();
                MovieSceneFileSelect();
                ExcelPathSelect();
                AssetSaveDataPathSelect();
                AssetSceneFileSelect();
                
                //AssetExportFolderSelect();


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                if (GUILayout.Button("Save", GUILayout.Height( 40 )))
                {
                    AssetPathGroupManager.Instance.SaveAssetPathGroup();
                }
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

        private void AssetPatGroupSelect()
        {
            EditorGUILayout.LabelField("Group Name", "Group Path");
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic)
            {
                ShowGroupPathMsg(item.Value);
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Add New AssetPath"))
            {
                AssetPathGroupManager.Instance.AddOnePathGroup(AddPathGroup);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            delGroupName = EditorGUILayout.TextField(delGroupName);
            if (GUILayout.Button("Del AssetPath"))
            {
                AssetPathGroupManager.Instance.DeleteOneGroup(delGroupName, AddPathGroup);
                delGroupName = "";
            }

            EditorGUILayout.EndHorizontal();
        }
 
	    private void AssetCommonSelect()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("---------------------------------", "---------------------------------");

            EditorGUILayout.LabelField("Common Group Name", "Group Path");
            foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.commonObjectPathDic)
            {
                ShowGroupPathMsg(item.Value);
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Add New Common Path"))
            {
                AssetPathGroupManager.Instance.AddOneCommonGroup(AddPathGroup);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            delCommonGroupName = EditorGUILayout.TextField(delCommonGroupName);
            if (GUILayout.Button("Del Common Path"))
            {
                AssetPathGroupManager.Instance.DeleteOneCommonGroup(delCommonGroupName, AddPathGroup);
                delCommonGroupName = "";
            }

            EditorGUILayout.EndHorizontal();
        }

        private string[] extensionArray = new string[16]
        {
            "prefab",
            "mp3",
            "wav",
            "unity",
            "txt",
            "bytes",
            "mat",
            "jpg",
            "png",
            "tga",
            "fbx",
            "anim",
            "cubemap",
            "shader",
            "cs",
			"exr"
        };


        private Dictionary<string, int> extentionDic = new Dictionary<string, int>();
        private void ShowGroupPathMsg( GroupPathMsg msg )
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(msg.groupName, msg.groupPath, GUILayout.Width(700));
                    msg.exportProperty.isPushDependencies = GameEditorUtils.ShowToggle("关联", msg.exportProperty.isPushDependencies, 50, Color.white);
                    msg.exportProperty.isPushIntoOneAssetBundle = GameEditorUtils.ShowToggle("合拼", msg.exportProperty.isPushIntoOneAssetBundle, 50, Color.white);
                    msg.exportProperty.isDeleteAnimation = GameEditorUtils.ShowToggle("删除动作", msg.exportProperty.isDeleteAnimation, 50, Color.white);
                    msg.exportProperty.isCompress = GameEditorUtils.ShowToggle("Unity压缩", msg.exportProperty.isCompress, 50, Color.white);
                    msg.exportProperty.isNativeCompress = GameEditorUtils.ShowToggle("本地压缩", msg.exportProperty.isNativeCompress, 50, Color.white);

                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField( "文件后缀　：", GUILayout.Width( 70 ));
                    if (msg.extensionList == null)
                    {
                        msg.extensionList = new List<string>();
                    }

                    foreach ( string ex in msg.extensionList )
                    {
                        EditorGUILayout.LabelField(ex + " | ", GUILayout.Width(70));
                    }


                    if (!extentionDic.ContainsKey(msg.groupName))
                    {
                        extentionDic.Add(msg.groupName, 0);
                    }

                    extentionDic[msg.groupName] = EditorGUILayout.Popup(extentionDic[msg.groupName], extensionArray, GUILayout.Width(50));

                    if( GUILayout.Button( "添加" , GUILayout.Width( 50 )))
                    {
                        if(!msg.extensionList.Contains( extensionArray[extentionDic[msg.groupName]]))
                        {
                            msg.extensionList.Add( extensionArray[extentionDic[msg.groupName]]);
                        }
                    }

                    if( GUILayout.Button( "删除" , GUILayout.Width( 50 )))
                    {
                        if( msg.extensionList.Count > 0)
                        {
                            msg.extensionList.RemoveAt( msg.extensionList.Count - 1 );
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                GameEditorUtils.Space(2);
            }
            EditorGUILayout.EndVertical();

        }


        private void ExcelPathSelect()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("---------------------------------", "---------------------------------");
            EditorGUILayout.LabelField("Excel Path : ", AssetPathGroupManager.Instance.assetPathGroup.excelPath);
            EditorGUILayout.Space();
            if (GUILayout.Button("Select Export Excel Path "))
            {
                AssetPathGroupManager.Instance.SelectExcelPath(AddPathGroup);
            }
        }

        private void AssetSaveDataPathSelect()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("---------------------------------", "---------------------------------");
			EditorGUILayout.LabelField("AssetSaveData Path : ", AssetPathGroupManager.Instance.assetPathGroup.oriAssetSaveDatePath);
			EditorGUILayout.Space();
            if (GUILayout.Button("Select AssetSaveData Path "))
            {
                AssetPathGroupManager.Instance.SelectAssetSaveDataPath(AddPathGroup);
            }
        }

        private void AssetSceneFileSelect()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("---------------------------------", "---------------------------------");
            EditorGUILayout.LabelField(" Scene Folder Path : ", AssetPathGroupManager.Instance.assetPathGroup.sceneFilePath);
            EditorGUILayout.Space();
            if (GUILayout.Button("Select Scene Folder Path "))
            {
                AssetPathGroupManager.Instance.SelectSceneFilePath(AddPathGroup);
            }
        }

        private void MovieSceneFileSelect()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("---------------------------------", "---------------------------------");

            EditorGUILayout.LabelField("Movie Scene Name ", "Path");
            foreach (KeyValuePair<string, string> item in AssetPathGroupManager.Instance.assetPathGroup.movieScenePathDic)
            {
                EditorGUILayout.LabelField(item.Key, item.Value);
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Add New Movie"))
            {
                AssetPathGroupManager.Instance.SelectOneMovieScenePath(AddPathGroup);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            delMovieName = EditorGUILayout.TextField(delMovieName);
            if (GUILayout.Button("Del Movie"))
            {
                AssetPathGroupManager.Instance.DeleteOneMovieScenePath(delMovieName, AddPathGroup);
                delMovieName = "";
            }

            EditorGUILayout.EndHorizontal();
        }

		private void AddPathGroup()
		{
			this.Repaint();
		}
	}
}

