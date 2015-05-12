 using UnityEngine;
using UnityEditor;
using System.Collections;
using AssetBundleEditor;
using System.Collections.Generic;


public class ChangAssetsMD5 : EditorWindow
{
	private static AssetPaths _assetPaths = null; 
	private static string _animationFolderPath = "Assets/GameResources/ArtResources/Animation";
	
	private static ChangAssetsMD5 _win = null;
	
	[MenuItem( "AssetBundle/Change Assets MD5" )]
	public static void ShowWin()
	{
		if( _win == null )
		{
			_win = (ChangAssetsMD5)EditorWindow.GetWindow( typeof( ChangAssetsMD5 ));
		}
		
		_win.minSize = new  Vector2(443,124);
		_win.maxSize = new  Vector2(4434,125);		
	}
	
	private string groupName = string.Empty;
	private void OnGUI()
	{
		EditorGUILayout.LabelField( "输入需要更新个资源目录位置" );
		groupName = EditorGUILayout.TextField( "目录 ：", groupName );
		
		if( GUILayout.Button( " 修改"))
		{
			Change( groupName );
			//groupName = string.Empty;
		}
	}
	
	public void Change( string groupName )
	{
		groupName = groupName.Replace("\\", "/" );
		_assetPaths = LoadAssetDatas();
		
		if( _assetPaths != null )
		{
			if( _assetPaths.assetPaths.ContainsKey( groupName ))
			{
				AssetsGroup group = _assetPaths.assetPaths[groupName];
				
				List<string> list = AssetPathGroupManager.Instance.assetPathGroup.assetGroupPathDic[groupName].extensionList;
				ChangeGroup( group, list );
				
				if( EditorUtility.DisplayDialog( "提示", "是否保存更改信息！！！" , "YES", "NO"  ))
				{
					SaveAssetDatas( _assetPaths );
				}
			}
			else
			{
				EditorUtility.DisplayDialog( "提示", "不存在当前资源目录", "OK" );
			}
			
		}
	}
	
	
	
	/// <summary>
	/// Loads the asset datas.
	/// </summary>
	/// <returns>
	/// The asset datas.
	/// </returns>
	private static AssetPaths LoadAssetDatas()
	{
        if (AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath == "")
        {
            AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath = "Assets/Editor/AssetBundle/Data"; 
            AssetPathGroupManager.Instance.SaveAssetPathGroup();
        }
		
        Dictionary<string, object> obj = DataHelper.GetJsonFile(AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + "Assets.assetdata");
        JsonAssetPathsParser parser = new JsonAssetPathsParser();
        AssetPaths assetPaths = parser.DeserializeJson_AssetPaths(obj);

		return assetPaths;		
	}	
	
		/// <summary>
		/// Saves the asset datas.
		/// </summary>
		public void SaveAssetDatas( AssetPaths _assetPaths)
		{
			if ( _assetPaths != null )
			{
                JsonAssetPathsParser parser = new JsonAssetPathsParser();
                Dictionary<string, object> obj = parser.SerializeJson_AssetPaths(_assetPaths);

				DataHelper.SaveJsonFile(obj, AssetPathGroupManager.Instance.assetPathGroup.assetSaveDatePath + "/" + "Assets.assetdata");
			}				
		}	
	
	
	private void ChangeGroup( AssetsGroup group, List<string> extension )
	{
		if( group == null )
		{
			EditorUtility.DisplayDialog( "提示", "资源目录为空", "OK" );
			return ;
		}
		
		
		
		int index  = 0 ;
		foreach( KeyValuePair < string,  AssetSaveData> item in group.assets )
		{
			index++;
			
			bool updataSuccess = false;
			foreach( string ext in extension )
			{
				string md5 = MD5Hashing.HashString( item.Key + "." + ext);
				
				if( !string.IsNullOrEmpty( md5 ))
				{
					item.Value.md5 = md5;
					updataSuccess = true;
					break;
				}				
			}

			if ( !updataSuccess )
			{
				Debug.LogError( "Get MD5 Error : " + item.Key );
			}
		
			EditorUtility.DisplayProgressBar( "更新资源MD5", string.Format( " {0} / {1} ", index , group.assets.Count ) ,
												(float)(index) /  group.assets.Count );
		}
		
		EditorUtility.ClearProgressBar();
		
		EditorUtility.DisplayDialog( "提示", "转换完成！！！" , "OK"  );
	}
}
