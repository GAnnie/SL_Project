using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AssetPathReferenceManager
{
	private static AssetPathReferenceManager _inst = null;
	
	public static AssetPathReferenceManager Instance
	{
		get
		{
			if( _inst == null )
			{
				_inst = new AssetPathReferenceManager();
			}
			return _inst;
		}
	}
	
	private const string assetPathRefDataName = "Set/ref.bytes";
	
	private Dictionary<int, string>    _folderDict 		=  new Dictionary<int, string>();
	private Dictionary<int, string>    _assetNameDict 	=  new Dictionary<int, string>();
	private Dictionary<string, string> _assetPathDict  	=  new Dictionary<string, string>();
	
	private Dictionary<string, int> _folderRefDict    = null;
	private Dictionary<string, int> _assetNameRefDict = null;
	
	public string GetFolderPathFormRefId( int folderId )
	{
		if( _folderDict.ContainsKey( folderId ))
		{
			return _folderDict[folderId];
		}
		
		return string.Empty;
	}
	
	public string GetAssetNameFormRefId( int assetNameid )
	{
		if( _assetNameDict.ContainsKey( assetNameid ))
		{
			return _assetNameDict[assetNameid];
		}
		
		return string.Empty;		
	}
	
	
	private bool isInit = false;
	public bool Setup( AssetPathReferenceList list )
	{
		if( list == null )
		{
			GameDebuger.Log( "AssetPathReferenceManager < _assetPathReferenceList is Null  > " );
			return false;
		}
		
		isInit = true;
		
		_folderRefDict 		= list.folderRefDict;
		_assetNameRefDict	= list.assetRefDict; 
		
		if( _assetPathDict != null ) _assetPathDict.Clear();
		if( _folderDict    != null ) _folderDict.Clear();
		if( _assetNameDict != null ) _assetNameDict.Clear();
		
		foreach( KeyValuePair< string, int > item in list.folderRefDict )
		{
			_folderDict.Add( item.Value, item.Key );
		}
		
		
		foreach( KeyValuePair< string, int > item in list.assetRefDict )
		{
			_assetNameDict.Add( item.Value, item.Key );
		}
		
		return true ;
	}
	
	
	public void Setup( byte[] assetPathRefData )
	{
		JsonAssetPathReferenceListParser parser = new JsonAssetPathReferenceListParser();
		Dictionary<string, object > obj = DataHelper.GetJsonFile( assetPathRefData, true );
		Setup ( parser.DeserializeJson_AssetPathReferenceList( obj ) );
	}
	
	
	
	public bool InitAssetPathRefData()
	{
		if( isInit ) 
			return true;
		
		string assetPath = Application.persistentDataPath + "/" + assetPathRefDataName;
		byte[] data = DataHelper.GetFileBytes( assetPath );
		if( data != null )
		{
			Dictionary<string, object > obj = DataHelper.GetJsonFile( data, true );
			JsonAssetPathReferenceListParser parser = new JsonAssetPathReferenceListParser();
			return Setup ( parser.DeserializeJson_AssetPathReferenceList( obj ) );			
		}
		else
		{
			return false;
		}
	}
	
	

	public bool UpdateAssetPathRefData( byte[] assetPathRefData )
	{
		if( assetPathRefData == null )
		{
			GameDebuger.Log( "AssetPathReferenceManager < Update AssetPathRefData is null > ");
			return false;
		}
		
		string savePath  = Application.persistentDataPath + "/" + assetPathRefDataName;
		
		//更新覆盖
		DataHelper.SaveFile( savePath, assetPathRefData );
		
		return true;
	}
	
	
	/// <summary>
	/// 根据引用Id, 获取资源的真实的地址
	/// </summary>
	public string GetAssetPath ( string refId )
	{
		if( !isInit )
		{
			GameDebuger.Log( "AssetPathReferenceManager <  AssetPathReferenceManager do not initialize > " );
			return null;			
		}
		
		if( _assetPathDict.ContainsKey( refId ) )
		{
			return _assetPathDict[refId];
		}
		else
		{
			string[] splitString = refId.Split( ':' );
			if( splitString.Length != 2 )
			{
				GameDebuger.Log( string.Format( "AssetPathReferenceManager < Receive Error Ref Id {0} >", refId ));
				return null;
			}
			
			int folderID = int.Parse( splitString[0] );
			int assetID  = int.Parse( splitString[1] );
			if( _folderDict.ContainsKey( folderID ) && _assetNameDict.ContainsKey( assetID ))
			{
				string path = _folderDict[folderID] + _assetNameDict[assetID];
				
				_assetPathDict.Add( refId, path );
				
				return path;
			}
			else
			{
				GameDebuger.Log( string.Format( "AssetPathReferenceManager < No Define Ref ID : {0} >", refId ));
				return null;
			}
		}
	}
	
	/// <summary>
	/// 根据资源目录返回引用ID
	/// </summary>
	public string GetAssetPathRefId( string assetPath )
	{
		if( !isInit )
		{
			GameDebuger.Log( "AssetPathReferenceManager <  AssetPathReferenceManager do not initialize > " );
			return null;			
		}
		
		if( string.IsNullOrEmpty( assetPath ))
		{
			return null;
		}
		
		if( !assetPath.StartsWith( PathHelper.RESOURCES_PATH ))
		{
			return	null;
		}
		
		int index = assetPath.LastIndexOf( "/" );
		if( index != -1 )
		{
			string folder = assetPath.Substring( 0, index + 1 );
			if( !_folderRefDict.ContainsKey( folder ))
			{
				return null;
			}
			
			string assetName = assetPath.Substring( index + 1 );
			if( !_assetNameRefDict.ContainsKey( assetName ))
			{
				return null;
			}
			
			return _folderRefDict[folder] + ":" + _assetNameRefDict[assetName];
		}
		
		return null;
	}
	
}
