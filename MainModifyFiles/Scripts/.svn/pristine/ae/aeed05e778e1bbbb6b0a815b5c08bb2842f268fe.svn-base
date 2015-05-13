using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonLocalAssetsVersionDataParser
{
	public LocalAssetsVersionData DeserializeJson_LocalAssetsVersionData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		LocalAssetsVersionData result = new LocalAssetsVersionData();
		if ( jsonData_.ContainsKey ( "version" ) )
		{
			result.version = (long) jsonData_["version"];
		}
	
		if ( jsonData_.ContainsKey ( "localAssetDic" ) )
		{
			Dictionary<string,object> data = jsonData_["localAssetDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.localAssetDic = new Dictionary<string,LocalAssetPrefab>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					LocalAssetPrefab current = DeserializeJson_LocalAssetPrefab(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.localAssetDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "scenes" ) )
		{
			Dictionary<string,object> data = jsonData_["scenes"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.scenes = new Dictionary<string,string>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					string current = keyValue.Value as string;
					if ( current != null ) { result.scenes.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "commonObjs" ) )
		{
			List<object> data = jsonData_["commonObjs"] as List<object>;
			result.commonObjs = new List<string>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				string current = data[i] as string;
				if ( current != null ) { result.commonObjs.Add(current); }
			}
		}
	
		if ( jsonData_.ContainsKey ( "firstDownLoadFinish" ) )
		{
			result.firstDownLoadFinish = (bool) jsonData_["firstDownLoadFinish"];
		}
	
		return result;
	}
	
	private LocalAssetPrefab DeserializeJson_LocalAssetPrefab(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		LocalAssetPrefab result = new LocalAssetPrefab();
		if ( jsonData_.ContainsKey ( "version" ) )
		{
			result.version = (long) jsonData_["version"];
		}
	
		if ( jsonData_.ContainsKey ( "needDecompress" ) )
		{
			result.needDecompress = (bool) jsonData_["needDecompress"];
		}
	
		if ( jsonData_.ContainsKey ( "isLocalPack" ) )
		{
			result.isLocalPack = (bool) jsonData_["isLocalPack"];
		}
	
		return result;
	}
	
	
	
	
	
	public Dictionary<string,object> SerializeJson_LocalAssetsVersionData(LocalAssetsVersionData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("version", objectData_.version);
	
		{
			object data = SerializeJson_Dict_LocalAssetPrefab(objectData_.localAssetDic);
			if(data!=null) jsonData.Add("localAssetDic",data);
		}
	
		{
			object data = SerializeJson_Dict(objectData_.scenes);
			if(data!=null) jsonData.Add("scenes",data);
		}
	
		{
			object data = SerializeJson_List(objectData_.commonObjs);
			if(data!=null) jsonData.Add("commonObjs",data);
		}
	
		jsonData.Add("firstDownLoadFinish", objectData_.firstDownLoadFinish);
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_LocalAssetPrefab(Dictionary<string,LocalAssetPrefab> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,LocalAssetPrefab> keyValue in dict_)
		{
			object current = SerializeJson_LocalAssetPrefab(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_LocalAssetPrefab(LocalAssetPrefab objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("version", objectData_.version);
	
		jsonData.Add("needDecompress", objectData_.needDecompress);
	
		jsonData.Add("isLocalPack", objectData_.isLocalPack);
	
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
	
	
	
}
