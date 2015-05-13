using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonResourcesVersionDataParser
{
	public ResourcesVersionData DeserializeJson_ResourcesVersionData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ResourcesVersionData result = new ResourcesVersionData();
		if ( jsonData_.ContainsKey ( "version" ) )
		{
			result.version = (long) jsonData_["version"];
		}
	
		if ( jsonData_.ContainsKey ( "assetPrefabs" ) )
		{
			Dictionary<string,object> data = jsonData_["assetPrefabs"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assetPrefabs = new Dictionary<string,AssetPrefab>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetPrefab current = DeserializeJson_AssetPrefab(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.assetPrefabs.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "scenes" ) )
		{
			Dictionary<string,object> data = jsonData_["scenes"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.scenes = new Dictionary<string,AssetPrefab>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetPrefab current = DeserializeJson_AssetPrefab(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.scenes.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "commonObjs" ) )
		{
			Dictionary<string,object> data = jsonData_["commonObjs"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.commonObjs = new Dictionary<string,AssetPrefab>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetPrefab current = DeserializeJson_AssetPrefab(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.commonObjs.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "root" ) )
		{
			result.root = DeserializeJson_AssetsPathTreeRoot( jsonData_["root"] as Dictionary<string,object>);
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
	
	
	
	
	private AssetsPathTreeRoot DeserializeJson_AssetsPathTreeRoot(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AssetsPathTreeRoot result = new AssetsPathTreeRoot();
		if ( jsonData_.ContainsKey ( "nodes" ) )
		{
			Dictionary<string,object> data = jsonData_["nodes"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.nodes = new Dictionary<string,AssetsPathNode>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetsPathNode current = DeserializeJson_AssetsPathNode(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.nodes.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "assets" ) )
		{
			Dictionary<string,object> data = jsonData_["assets"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assets = new Dictionary<string,string>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					string current = keyValue.Value as string;
					if ( current != null ) { result.assets.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private AssetsPathNode DeserializeJson_AssetsPathNode(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AssetsPathNode result = new AssetsPathNode();
		if ( jsonData_.ContainsKey ( "nodes" ) )
		{
			Dictionary<string,object> data = jsonData_["nodes"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.nodes = new Dictionary<string,AssetsPathNode>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetsPathNode current = DeserializeJson_AssetsPathNode(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.nodes.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "assets" ) )
		{
			Dictionary<string,object> data = jsonData_["assets"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assets = new Dictionary<string,string>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					string current = keyValue.Value as string;
					if ( current != null ) { result.assets.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	
	
	
	
	
	
	public Dictionary<string,object> SerializeJson_ResourcesVersionData(ResourcesVersionData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("version", objectData_.version);
	
		{
			object data = SerializeJson_Dict_AssetPrefab(objectData_.assetPrefabs);
			if(data!=null) jsonData.Add("assetPrefabs",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetPrefab(objectData_.scenes);
			if(data!=null) jsonData.Add("scenes",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetPrefab(objectData_.commonObjs);
			if(data!=null) jsonData.Add("commonObjs",data);
		}
	
		{
			object data = SerializeJson_AssetsPathTreeRoot(objectData_.root);
			if(data!=null) jsonData.Add("root",data);
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
	
	
	
	
	private Dictionary<string,object> SerializeJson_AssetsPathTreeRoot(AssetsPathTreeRoot objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict_AssetsPathNode(objectData_.nodes);
			if(data!=null) jsonData.Add("nodes",data);
		}
	
		{
			object data = SerializeJson_Dict(objectData_.assets);
			if(data!=null) jsonData.Add("assets",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_AssetsPathNode(Dictionary<string,AssetsPathNode> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetsPathNode> keyValue in dict_)
		{
			object current = SerializeJson_AssetsPathNode(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetsPathNode(AssetsPathNode objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict_AssetsPathNode(objectData_.nodes);
			if(data!=null) jsonData.Add("nodes",data);
		}
	
		{
			object data = SerializeJson_Dict(objectData_.assets);
			if(data!=null) jsonData.Add("assets",data);
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
