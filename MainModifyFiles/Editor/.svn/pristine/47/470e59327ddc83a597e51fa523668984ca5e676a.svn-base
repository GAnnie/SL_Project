using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;
using System.Text;
using AssetBundleEditor;

public class JsonExportAssetBundleConfigParser
{
	public ExportAssetBundleConfig DeserializeJson_ExportAssetBundleConfig(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		ExportAssetBundleConfig result = new ExportAssetBundleConfig();
		if ( jsonData_.ContainsKey ( "assetGroupPathDic" ) )
		{
			Dictionary<string,object> data = jsonData_["assetGroupPathDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assetGroupPathDic = new Dictionary<string,GroupPathMsg>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					GroupPathMsg current = DeserializeJson_GroupPathMsg(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.assetGroupPathDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "commonObjectPathDic" ) )
		{
			Dictionary<string,object> data = jsonData_["commonObjectPathDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.commonObjectPathDic = new Dictionary<string,GroupPathMsg>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					GroupPathMsg current = DeserializeJson_GroupPathMsg(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.commonObjectPathDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "movieScenePathDic" ) )
		{
			Dictionary<string,object> data = jsonData_["movieScenePathDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.movieScenePathDic = new Dictionary<string,string>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					string current = keyValue.Value as string;
					if ( current != null ) { result.movieScenePathDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "scenePrefabFilePath" ) )
		{
			result.scenePrefabFilePath = jsonData_["scenePrefabFilePath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "sceneFilePath" ) )
		{
			result.sceneFilePath = jsonData_["sceneFilePath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "excelPath" ) )
		{
			result.excelPath = jsonData_["excelPath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "assetSaveDatePath" ) )
		{
			result.assetSaveDatePath = jsonData_["assetSaveDatePath"] as string;
		}
	
		return result;
	}
	
	private GroupPathMsg DeserializeJson_GroupPathMsg(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		GroupPathMsg result = new GroupPathMsg();
		if ( jsonData_.ContainsKey ( "groupName" ) )
		{
			result.groupName = jsonData_["groupName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "groupPath" ) )
		{
			result.groupPath = jsonData_["groupPath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "exportProperty" ) )
		{
			result.exportProperty = DeserializeJson_AssetExportProperty( jsonData_["exportProperty"] as Dictionary<string,object>);
		}
	
		if ( jsonData_.ContainsKey ( "extensionList" ) )
		{
			List<object> data = jsonData_["extensionList"] as List<object>;
			result.extensionList = new List<string>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				string current = data[i] as string;
				if ( current != null ) { result.extensionList.Add(current); }
			}
		}
	
		return result;
	}
	
	private AssetExportProperty DeserializeJson_AssetExportProperty(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetExportProperty result = new AssetExportProperty();
		if ( jsonData_.ContainsKey ( "isPushDependencies" ) )
		{
			result.isPushDependencies = (bool) jsonData_["isPushDependencies"];
		}
	
		if ( jsonData_.ContainsKey ( "isPushIntoOneAssetBundle" ) )
		{
			result.isPushIntoOneAssetBundle = (bool) jsonData_["isPushIntoOneAssetBundle"];
		}
	
		if ( jsonData_.ContainsKey ( "isDeleteAnimation" ) )
		{
			result.isDeleteAnimation = (bool) jsonData_["isDeleteAnimation"];
		}
	
		if ( jsonData_.ContainsKey ( "isNativeCompress" ) )
		{
			result.isNativeCompress = (bool) jsonData_["isNativeCompress"];
		}
	
		if ( jsonData_.ContainsKey ( "isCompress" ) )
		{
			result.isCompress = (bool) jsonData_["isCompress"];
		}
	
		return result;
	}
	
	
	
	
	
	
	
	public Dictionary<string,object> SerializeJson_ExportAssetBundleConfig(ExportAssetBundleConfig objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict_GroupPathMsg(objectData_.assetGroupPathDic);
			if(data!=null) jsonData.Add("assetGroupPathDic",data);
		}
	
		{
			object data = SerializeJson_Dict_GroupPathMsg(objectData_.commonObjectPathDic);
			if(data!=null) jsonData.Add("commonObjectPathDic",data);
		}
	
		{
			object data = SerializeJson_Dict(objectData_.movieScenePathDic);
			if(data!=null) jsonData.Add("movieScenePathDic",data);
		}
	
		if(objectData_.scenePrefabFilePath!=null) jsonData.Add("scenePrefabFilePath", objectData_.scenePrefabFilePath);
	
		if(objectData_.sceneFilePath!=null) jsonData.Add("sceneFilePath", objectData_.sceneFilePath);
	
		if(objectData_.excelPath!=null) jsonData.Add("excelPath", objectData_.excelPath);
	
		if(objectData_.assetSaveDatePath!=null) jsonData.Add("assetSaveDatePath", objectData_.oriAssetSaveDatePath);
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_GroupPathMsg(Dictionary<string,GroupPathMsg> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,GroupPathMsg> keyValue in dict_)
		{
			object current = SerializeJson_GroupPathMsg(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_GroupPathMsg(GroupPathMsg objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.groupName!=null) jsonData.Add("groupName", objectData_.groupName);
	
		if(objectData_.groupPath!=null) jsonData.Add("groupPath", objectData_.groupPath);
	
		{
			object data = SerializeJson_AssetExportProperty(objectData_.exportProperty);
			if(data!=null) jsonData.Add("exportProperty",data);
		}
	
		{
			object data = SerializeJson_List(objectData_.extensionList);
			if(data!=null) jsonData.Add("extensionList",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetExportProperty(AssetExportProperty objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("isPushDependencies",objectData_.isPushDependencies);
	
		jsonData.Add("isPushIntoOneAssetBundle",objectData_.isPushIntoOneAssetBundle);
	
		jsonData.Add("isDeleteAnimation",objectData_.isDeleteAnimation);
	
		jsonData.Add("isNativeCompress",objectData_.isNativeCompress);
	
		jsonData.Add("isCompress",objectData_.isCompress);
	
		return jsonData;
	}
	
	
	private List<object> SerializeJson_List(List<string> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = list_[i];
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	
	
	
	private Dictionary<string,object> SerializeJson_Dict(Dictionary<string,string> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,string> keyValue in dict_)
		{
			object current = keyValue.Value;
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	
	
}
