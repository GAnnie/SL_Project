// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetPathReferenceGenator.cs
// Author   : wenlin
// Created  : 2013/12/9 
// Purpose  : 
// **********************************************************************


using System;
using UnityEditor;
using UnityEngine;
using AssetBundleEditor;

using System.Collections.Generic;

public class AssetPathReferenceGenator
{
	[MenuItem("Tools/Build reference list of Assets' Path ")]
	public static void  GenatorAssetRefList()
	{
		List< string > totoalAssetsPathList = new List<string>();
		
		//Asset Path
        foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic)
		{
			List< string > assets = GameEditorUtils.GetAssetsList( item.Value.groupPath, item.Value.extensionList.ToArray());
			totoalAssetsPathList.AddRange( assets );
		}
		
        foreach (KeyValuePair<string, GroupPathMsg> item in AssetPathGroupManager.Instance.assetPathGroup.commonObjectPathDic)
		{
			List< string > assets = GameEditorUtils.GetAssetsList( item.Value.groupPath, item.Value.extensionList.ToArray());
			totoalAssetsPathList.AddRange( assets );
		}

        foreach (KeyValuePair<string, string> item in AssetPathGroupManager.Instance.assetPathGroup.movieScenePathDic)
		{
			totoalAssetsPathList.Add( item.Value );
		}
		
		Debug.Log( string.Format( "Total Assets Number : {0}", totoalAssetsPathList.Count ));
		
		
		AssetPathReferenceList list = LoadAssetPathReferenceList();
		if( list != null )
		{
			int progressNum = 0;
			foreach(string path in totoalAssetsPathList )
			{
				progressNum ++;
				
				int gameResIndex = path.IndexOf("GameResource");
				if(gameResIndex != -1)
				{
					string subpath = path.Substring( gameResIndex, path.LastIndexOf(".") - gameResIndex );
					
					int index = subpath.LastIndexOf("/");
					if( index != -1 )
					{
						string folder = subpath.Substring( 0, index+1 );
						string assetName = subpath.Substring( index+1 );
						
						if( !list.folderRefDict.ContainsKey( folder ))
						{
							list.folderRefDict.Add( folder, ++list.folderRefNumber);
						}
						
						if( !list.assetRefDict.ContainsKey( assetName ))
						{
							list.assetRefDict.Add( assetName, ++list.assetsNumber );
						}
					}
					
					EditorUtility.DisplayProgressBar( " Progressing ..... ", 
													string.Format(" {0} / {1} ", progressNum.ToString(), totoalAssetsPathList.Count.ToString()),
													(float)progressNum / totoalAssetsPathList.Count );					
				}

			}
			
			EditorUtility.ClearProgressBar();
			
			SaveAssetPathReferenceList( list );
			
			AssetDatabase.Refresh();
		}
	}
	
	private static bool compress_flag = true;
	
	public static readonly string AssetPathRefListPath = "Assets/GameResources/AssetsReferenceList/";
	public static readonly string AssetPathRefListFileName = "AssetsRefList";
	public static AssetPathReferenceList LoadAssetPathReferenceList(bool isAutoCreateNewList = true )
	{
		Dictionary<string, object> objs = GameEditorUtils.GetJsonFile( AssetPathRefListPath + AssetPathRefListFileName +".txt", compress_flag);
		AssetPathReferenceList list = null;
		if( objs == null)
		{
			if( !isAutoCreateNewList )
			{
				return null;	
			}
			
			list = new AssetPathReferenceList();			
		}
		else
		{
			JsonAssetPathReferenceListParser parser = new JsonAssetPathReferenceListParser();
			list = parser.DeserializeJson_AssetPathReferenceList( objs );
		}
		
		return list;
	}
	
	public static byte[] LoadAssetPathReferenceListBytes( bool isCompress )
	{
		return GameEditorUtils.GetFileBytes( AssetPathRefListPath + AssetPathRefListFileName +".txt", isCompress);
	}
	
	public static void SaveAssetPathReferenceList( AssetPathReferenceList list )
	{
		if( list == null ) return;
		
		JsonAssetPathReferenceListParser parser = new JsonAssetPathReferenceListParser();
		Dictionary<string, object> obj = parser.SerializeJson_AssetPathReferenceList( list );
		GameEditorUtils.SaveJsonFile( obj, AssetPathRefListPath + AssetPathRefListFileName +".txt", compress_flag);
	}
}

