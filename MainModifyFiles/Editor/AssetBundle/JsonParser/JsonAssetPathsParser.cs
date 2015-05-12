using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;
using System.Text;
using AssetBundleEditor;

public class JsonAssetPathsParser
{
	public AssetPaths DeserializeJson_AssetPaths(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetPaths result = new AssetPaths();
		if ( jsonData_.ContainsKey ( "assetPaths" ) )
		{
			Dictionary<string,object> data = jsonData_["assetPaths"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assetPaths = new Dictionary<string,AssetsGroup>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetsGroup current = DeserializeJson_AssetsGroup(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.assetPaths.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "scenePaths" ) )
		{
			Dictionary<string,object> data = jsonData_["scenePaths"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.scenePaths = new Dictionary<string,AssetsGroup>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetsGroup current = DeserializeJson_AssetsGroup(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.scenePaths.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "commonPaths" ) )
		{
			Dictionary<string,object> data = jsonData_["commonPaths"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.commonPaths = new Dictionary<string,AssetsGroup>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetsGroup current = DeserializeJson_AssetsGroup(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.commonPaths.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "allDependencis" ) )
		{
			Dictionary<string,object> data = jsonData_["allDependencis"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.allDependencis = new Dictionary<string,AssetDependencisGroup>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetDependencisGroup current = DeserializeJson_AssetDependencisGroup(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.allDependencis.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private AssetsGroup DeserializeJson_AssetsGroup(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetsGroup result = new AssetsGroup();
		if ( jsonData_.ContainsKey ( "groupPath" ) )
		{
			result.groupPath = jsonData_["groupPath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "groupName" ) )
		{
			result.groupName = jsonData_["groupName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "exportProperty" ) )
		{
			result.exportProperty = DeserializeJson_AssetExportProperty( jsonData_["exportProperty"] as Dictionary<string,object>);
		}
	
		if ( jsonData_.ContainsKey ( "assets" ) )
		{
			Dictionary<string,object> data = jsonData_["assets"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.assets = new Dictionary<string,AssetSaveData>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetSaveData current = DeserializeJson_AssetSaveData(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.assets.Add ( keyValue.Key, current ); }
				}
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
	
		if ( jsonData_.ContainsKey ( "isCompress" ) )
		{
			result.isCompress = (bool) jsonData_["isCompress"];
		}
	
		return result;
	}
	
	
	private AssetSaveData DeserializeJson_AssetSaveData(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetSaveData result = new AssetSaveData();
		if ( jsonData_.ContainsKey ( "version" ) )
		{
			result.version = (long) jsonData_["version"];
		}
	
		if ( jsonData_.ContainsKey ( "versionTime" ) )
		{
			result.versionTime = jsonData_["versionTime"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "fileTime" ) )
		{
			result.fileTime = jsonData_["fileTime"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "path" ) )
		{
			result.path = jsonData_["path"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "exportPath" ) )
		{
			result.exportPath = jsonData_["exportPath"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "groupName" ) )
		{
			result.groupName = jsonData_["groupName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "fileName" ) )
		{
			result.fileName = jsonData_["fileName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "fileID" ) )
		{
			result.fileID = jsonData_["fileID"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "md5" ) )
		{
			result.md5 = jsonData_["md5"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "changMd5" ) )
		{
			result.changMd5 = jsonData_["changMd5"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "isDirty" ) )
		{
			result.isDirty = (bool) jsonData_["isDirty"];
		}
	
		if ( jsonData_.ContainsKey ( "isReferenceDirty" ) )
		{
			result.isReferenceDirty = (bool) jsonData_["isReferenceDirty"];
		}
	
		if ( jsonData_.ContainsKey ( "isExist" ) )
		{
			result.isExist = (bool) jsonData_["isExist"];
		}
	
		if ( jsonData_.ContainsKey ( "isExport" ) )
		{
			result.isExport = (bool) jsonData_["isExport"];
		}
	
		if ( jsonData_.ContainsKey ( "needUncompress" ) )
		{
			result.needUncompress = (bool) jsonData_["needUncompress"];
		}
	
		if ( jsonData_.ContainsKey ( "dataSize" ) )
		{
			result.dataSize = (uint)(long) jsonData_["dataSize"];
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
	
		if ( jsonData_.ContainsKey ( "type" ) )
		{
			result.type = (AssetbundleType)(long) jsonData_["type"];
		}
	
		return result;
	}
	
	
	
	
	
	private AssetDependencisGroup DeserializeJson_AssetDependencisGroup(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetDependencisGroup result = new AssetDependencisGroup();
		if ( jsonData_.ContainsKey ( "type" ) )
		{
			result.type = (AssetbundleType)(long) jsonData_["type"];
		}
	
		if ( jsonData_.ContainsKey ( "dependencisDic" ) )
		{
			Dictionary<string,object> data = jsonData_["dependencisDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.dependencisDic = new Dictionary<string,AssetDependencis>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AssetDependencis current = DeserializeJson_AssetDependencis(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.dependencisDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private AssetDependencis DeserializeJson_AssetDependencis(Dictionary<string,object> jsonData_)
	{
		if(jsonData_ == null) return null;
		AssetDependencis result = new AssetDependencis();
		if ( jsonData_.ContainsKey ( "md5" ) )
		{
			result.md5 = jsonData_["md5"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "isChang" ) )
		{
			result.isChang = (bool) jsonData_["isChang"];
		}
	
		if ( jsonData_.ContainsKey ( "path" ) )
		{
			result.path = jsonData_["path"] as string;
		}
	
		return result;
	}
	
	
	
	
	public Dictionary<string,object> SerializeJson_AssetPaths(AssetPaths objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict_AssetsGroup(objectData_.assetPaths);
			if(data!=null) jsonData.Add("assetPaths",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetsGroup(objectData_.scenePaths);
			if(data!=null) jsonData.Add("scenePaths",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetsGroup(objectData_.commonPaths);
			if(data!=null) jsonData.Add("commonPaths",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetDependencisGroup(objectData_.allDependencis);
			if(data!=null) jsonData.Add("allDependencis",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_AssetsGroup(Dictionary<string,AssetsGroup> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetsGroup> keyValue in dict_)
		{
			object current = SerializeJson_AssetsGroup(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetsGroup(AssetsGroup objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.groupPath!=null) jsonData.Add("groupPath", objectData_.groupPath);
	
		if(objectData_.groupName!=null) jsonData.Add("groupName", objectData_.groupName);
	
		{
			object data = SerializeJson_AssetExportProperty(objectData_.exportProperty);
			if(data!=null) jsonData.Add("exportProperty",data);
		}
	
		{
			object data = SerializeJson_Dict_AssetSaveData(objectData_.assets);
			if(data!=null) jsonData.Add("assets",data);
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
	
		jsonData.Add("isCompress",objectData_.isCompress);
	
		return jsonData;
	}
	
	
	private Dictionary<string,object> SerializeJson_Dict_AssetSaveData(Dictionary<string,AssetSaveData> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetSaveData> keyValue in dict_)
		{
			object current = SerializeJson_AssetSaveData(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetSaveData(AssetSaveData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("version",objectData_.version);
	
		if(objectData_.versionTime!=null) jsonData.Add("versionTime", objectData_.versionTime);
	
		if(objectData_.fileTime!=null) jsonData.Add("fileTime", objectData_.fileTime);
	
		if(objectData_.path!=null) jsonData.Add("path", objectData_.path);
	
		if(objectData_.exportPath!=null) jsonData.Add("exportPath", objectData_.exportPath);
	
		if(objectData_.groupName!=null) jsonData.Add("groupName", objectData_.groupName);
	
		if(objectData_.fileName!=null) jsonData.Add("fileName", objectData_.fileName);
	
		if(objectData_.fileID!=null) jsonData.Add("fileID", objectData_.fileID);
	
		if(objectData_.md5!=null) jsonData.Add("md5", objectData_.md5);
	
		if(objectData_.changMd5!=null) jsonData.Add("changMd5", objectData_.changMd5);
	
		jsonData.Add("isDirty",objectData_.isDirty);
	
		jsonData.Add("isReferenceDirty",objectData_.isReferenceDirty);
	
		jsonData.Add("isExist",objectData_.isExist);
	
		jsonData.Add("isExport",objectData_.isExport);
	
		jsonData.Add("needUncompress",objectData_.needUncompress);
	
		jsonData.Add("dataSize",objectData_.dataSize);
	
		if(objectData_.package!=null) jsonData.Add("package", objectData_.package);
	
		jsonData.Add("packageNum",objectData_.packageNum);
	
		jsonData.Add("packagePos",objectData_.packagePos);
	
		jsonData.Add("type",(int)objectData_.type);
	
		return jsonData;
	}
	
	
	
	
	
	private Dictionary<string,object> SerializeJson_Dict_AssetDependencisGroup(Dictionary<string,AssetDependencisGroup> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetDependencisGroup> keyValue in dict_)
		{
			object current = SerializeJson_AssetDependencisGroup(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetDependencisGroup(AssetDependencisGroup objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("type",(int)objectData_.type);
	
		{
			object data = SerializeJson_Dict_AssetDependencis(objectData_.dependencisDic);
			if(data!=null) jsonData.Add("dependencisDic",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_AssetDependencis(Dictionary<string,AssetDependencis> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AssetDependencis> keyValue in dict_)
		{
			object current = SerializeJson_AssetDependencis(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AssetDependencis(AssetDependencis objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.md5!=null) jsonData.Add("md5", objectData_.md5);
	
		jsonData.Add("isChang",objectData_.isChang);
	
		if(objectData_.path!=null) jsonData.Add("path", objectData_.path);
	
		return jsonData;
	}
	
	
	
	
}
