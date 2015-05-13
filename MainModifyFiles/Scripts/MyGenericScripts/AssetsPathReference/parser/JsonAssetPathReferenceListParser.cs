using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonAssetPathReferenceListParser
{
	public AssetPathReferenceList DeserializeJson_AssetPathReferenceList(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AssetPathReferenceList result = new AssetPathReferenceList();
		if ( jsonData_.ContainsKey ( "folderRefNumber" ) )
		{
			result.folderRefNumber = (int)(long) jsonData_["folderRefNumber"];
		}
	
		if ( jsonData_.ContainsKey ( "folderRefDict" ) )
		{
			Dictionary<string,object> data = jsonData_["folderRefDict"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.folderRefDict = new Dictionary<string,int>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					int current = (int)(long)keyValue.Value;
					if ( current != null ) { result.folderRefDict.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "assetsNumber" ) )
		{
			result.assetsNumber = (int)(long) jsonData_["assetsNumber"];
		}
	
		if ( jsonData_.ContainsKey ( "assetRefDict" ) )
		{
			Dictionary<string,object> data = jsonData_["assetRefDict"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assetRefDict = new Dictionary<string,int>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					int current = (int)(long)keyValue.Value;
					if ( current != null ) { result.assetRefDict.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	
	
	
	public Dictionary<string,object> SerializeJson_AssetPathReferenceList(AssetPathReferenceList objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("folderRefNumber", objectData_.folderRefNumber);
	
		{
			object data = SerializeJson_Dict(objectData_.folderRefDict);
			if(data!=null) jsonData.Add("folderRefDict",data);
		}
	
		jsonData.Add("assetsNumber", objectData_.assetsNumber);
	
		{
			object data = SerializeJson_Dict(objectData_.assetRefDict);
			if(data!=null) jsonData.Add("assetRefDict",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict(Dictionary<string,int> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,int> keyValue in dict_)
		{
			object current = keyValue.Value;
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	
	
	
}
