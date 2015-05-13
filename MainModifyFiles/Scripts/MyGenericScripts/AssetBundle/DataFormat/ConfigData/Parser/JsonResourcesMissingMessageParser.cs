using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonResourcesMissingMessageParser
{
	public ResourcesMissingMessage DeserializeJson_ResourcesMissingMessage(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ResourcesMissingMessage result = new ResourcesMissingMessage();
		if ( jsonData_.ContainsKey ( "totalGameResNumber" ) )
		{
			result.totalGameResNumber = (int)(long) jsonData_["totalGameResNumber"];
		}
	
		if ( jsonData_.ContainsKey ( "missingAssets" ) )
		{
			Dictionary<string,object> data = jsonData_["missingAssets"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.missingAssets = new Dictionary<string,AssetPrefab>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetPrefab current = DeserializeJson_AssetPrefab(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.missingAssets.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private AssetPrefab DeserializeJson_AssetPrefab(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AssetPrefab result = new AssetPrefab();
		if ( jsonData_.ContainsKey ( "version" ) )
		{
			result.version = (long) jsonData_["version"];
		}
	
		if ( jsonData_.ContainsKey ( "package" ) )
		{
			result.package = jsonData_["package"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "packageNum" ) )
		{
			result.packageNum = (int)(long) jsonData_["packageNum"];
		}
	
		if ( jsonData_.ContainsKey ( "packagePos" ) )
		{
			result.packagePos = (int)(long) jsonData_["packagePos"];
		}
	
		if ( jsonData_.ContainsKey ( "needDecompress" ) )
		{
			result.needDecompress = (bool) jsonData_["needDecompress"];
		}
	
		if ( jsonData_.ContainsKey ( "isLocalPack" ) )
		{
			result.isLocalPack = (bool) jsonData_["isLocalPack"];
		}
	
		if ( jsonData_.ContainsKey ( "isEp" ) )
		{
			result.isEp = (byte)(long) jsonData_["isEp"];
		}
	
		if ( jsonData_.ContainsKey ( "size" ) )
		{
			result.size = (uint)(long) jsonData_["size"];
		}
	
		return result;
	}
	
	
	
	public Dictionary<string,object> SerializeJson_ResourcesMissingMessage(ResourcesMissingMessage objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("totalGameResNumber", objectData_.totalGameResNumber);
	
		{
			object data = SerializeJson_Dict_AssetPrefab(objectData_.missingAssets);
			if(data!=null) jsonData.Add("missingAssets",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_AssetPrefab(Dictionary<string,AssetPrefab> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetPrefab> keyValue in dict_)
		{
			object current = SerializeJson_AssetPrefab(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetPrefab(AssetPrefab objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("version", objectData_.version);
	
		if(objectData_.package!=null) jsonData.Add("package", objectData_.package);
	
		jsonData.Add("packageNum", objectData_.packageNum);
	
		jsonData.Add("packagePos", objectData_.packagePos);
	
		jsonData.Add("needDecompress", objectData_.needDecompress);
	
		jsonData.Add("isLocalPack", objectData_.isLocalPack);
	
		jsonData.Add("isEp", objectData_.isEp);
	
		jsonData.Add("size", objectData_.size);
	
		return jsonData;
	}
	
	
	
}
