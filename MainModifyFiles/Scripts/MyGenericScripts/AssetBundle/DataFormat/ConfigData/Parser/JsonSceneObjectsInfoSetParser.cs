using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonSceneObjectsInfoSetParser
{
	public SceneObjectsInfoSet DeserializeJson_SceneObjectsInfoSet(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneObjectsInfoSet result = new SceneObjectsInfoSet();
		if ( jsonData_.ContainsKey ( "sceneID" ) )
		{
			result.sceneID = jsonData_["sceneID"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "sceneNodeDic" ) )
		{
			Dictionary<string,object> data = jsonData_["sceneNodeDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.sceneNodeDic = new Dictionary<string,SceneNode>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					SceneNode current = DeserializeJson_SceneNode(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.sceneNodeDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "skyboxData" ) )
		{
			result.skyboxData = DeserializeJson_SceneSkyboxData( jsonData_["skyboxData"] as Dictionary<string,object>);
		}
	
		return result;
	}
	
	private SceneNode DeserializeJson_SceneNode(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneNode result = new SceneNode();
		if ( jsonData_.ContainsKey ( "nodeName" ) )
		{
			result.nodeName = jsonData_["nodeName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "sceneObjectDataDic" ) )
		{
			Dictionary<string,object> data = jsonData_["sceneObjectDataDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.sceneObjectDataDic = new Dictionary<string,SceneObjectData>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					SceneObjectData current = DeserializeJson_SceneObjectData(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.sceneObjectDataDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "colliderObjectDataDic" ) )
		{
			Dictionary<string,object> data = jsonData_["colliderObjectDataDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.colliderObjectDataDic = new Dictionary<string,SceneObjectData>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					SceneObjectData current = DeserializeJson_SceneObjectData(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.colliderObjectDataDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private SceneObjectData DeserializeJson_SceneObjectData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneObjectData result = new SceneObjectData();
		if ( jsonData_.ContainsKey ( "objectName" ) )
		{
			result.objectName = jsonData_["objectName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "objectPathName" ) )
		{
			result.objectPathName = jsonData_["objectPathName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "parentName" ) )
		{
			result.parentName = jsonData_["parentName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "objectTransFormDataList" ) )
		{
			List<object> data = jsonData_["objectTransFormDataList"] as List<object>;
			result.objectTransFormDataList = new List<SceneObjectTransFormData>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				SceneObjectTransFormData current = DeserializeJson_SceneObjectTransFormData(data[i]  as Dictionary<string,object>);
				if ( current != null ) { result.objectTransFormDataList.Add(current); }
			}
		}
	
		return result;
	}
	
	private SceneObjectTransFormData DeserializeJson_SceneObjectTransFormData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneObjectTransFormData result = new SceneObjectTransFormData();
		if ( jsonData_.ContainsKey ( "positionX" ) )
		{
			result.positionX = ( jsonData_["positionX"] is long) ? (double)(long) jsonData_["positionX"] : (double) jsonData_["positionX"];
		}
	
		if ( jsonData_.ContainsKey ( "positionY" ) )
		{
			result.positionY = ( jsonData_["positionY"] is long) ? (double)(long) jsonData_["positionY"] : (double) jsonData_["positionY"];
		}
	
		if ( jsonData_.ContainsKey ( "positionZ" ) )
		{
			result.positionZ = ( jsonData_["positionZ"] is long) ? (double)(long) jsonData_["positionZ"] : (double) jsonData_["positionZ"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationX" ) )
		{
			result.rotationX = ( jsonData_["rotationX"] is long) ? (double)(long) jsonData_["rotationX"] : (double) jsonData_["rotationX"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationY" ) )
		{
			result.rotationY = ( jsonData_["rotationY"] is long) ? (double)(long) jsonData_["rotationY"] : (double) jsonData_["rotationY"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationZ" ) )
		{
			result.rotationZ = ( jsonData_["rotationZ"] is long) ? (double)(long) jsonData_["rotationZ"] : (double) jsonData_["rotationZ"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleX" ) )
		{
			result.scaleX = ( jsonData_["scaleX"] is long) ? (double)(long) jsonData_["scaleX"] : (double) jsonData_["scaleX"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleY" ) )
		{
			result.scaleY = ( jsonData_["scaleY"] is long) ? (double)(long) jsonData_["scaleY"] : (double) jsonData_["scaleY"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleZ" ) )
		{
			result.scaleZ = ( jsonData_["scaleZ"] is long) ? (double)(long) jsonData_["scaleZ"] : (double) jsonData_["scaleZ"];
		}
	
		if ( jsonData_.ContainsKey ( "subObjects" ) )
		{
			Dictionary<string,object> data = jsonData_["subObjects"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.subObjects = new Dictionary<string,SubObject>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					SubObject current = DeserializeJson_SubObject(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.subObjects.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "lightmapInfo" ) )
		{
			result.lightmapInfo = DeserializeJson_SceneObjectLightmapInfo( jsonData_["lightmapInfo"] as Dictionary<string,object>);
		}
	
		return result;
	}
	
	private SubObject DeserializeJson_SubObject(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SubObject result = new SubObject();
		if ( jsonData_.ContainsKey ( "objectName" ) )
		{
			result.objectName = jsonData_["objectName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "lightMapInfo" ) )
		{
			result.lightMapInfo = DeserializeJson_SceneObjectLightmapInfo( jsonData_["lightMapInfo"] as Dictionary<string,object>);
		}
	
		if ( jsonData_.ContainsKey ( "transformInfo" ) )
		{
			result.transformInfo = DeserializeJson_SubObjectTransformInfo( jsonData_["transformInfo"] as Dictionary<string,object>);
		}
	
		return result;
	}
	
	private SceneObjectLightmapInfo DeserializeJson_SceneObjectLightmapInfo(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneObjectLightmapInfo result = new SceneObjectLightmapInfo();
		if ( jsonData_.ContainsKey ( "lightmapIndex" ) )
		{
			result.lightmapIndex = (int)(long) jsonData_["lightmapIndex"];
		}
	
		if ( jsonData_.ContainsKey ( "x" ) )
		{
			result.x = ( jsonData_["x"] is long) ? (double)(long) jsonData_["x"] : (double) jsonData_["x"];
		}
	
		if ( jsonData_.ContainsKey ( "y" ) )
		{
			result.y = ( jsonData_["y"] is long) ? (double)(long) jsonData_["y"] : (double) jsonData_["y"];
		}
	
		if ( jsonData_.ContainsKey ( "z" ) )
		{
			result.z = ( jsonData_["z"] is long) ? (double)(long) jsonData_["z"] : (double) jsonData_["z"];
		}
	
		if ( jsonData_.ContainsKey ( "w" ) )
		{
			result.w = ( jsonData_["w"] is long) ? (double)(long) jsonData_["w"] : (double) jsonData_["w"];
		}
	
		return result;
	}
	
	
	private SubObjectTransformInfo DeserializeJson_SubObjectTransformInfo(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SubObjectTransformInfo result = new SubObjectTransformInfo();
		if ( jsonData_.ContainsKey ( "positionX" ) )
		{
			result.positionX = ( jsonData_["positionX"] is long) ? (double)(long) jsonData_["positionX"] : (double) jsonData_["positionX"];
		}
	
		if ( jsonData_.ContainsKey ( "positionY" ) )
		{
			result.positionY = ( jsonData_["positionY"] is long) ? (double)(long) jsonData_["positionY"] : (double) jsonData_["positionY"];
		}
	
		if ( jsonData_.ContainsKey ( "positionZ" ) )
		{
			result.positionZ = ( jsonData_["positionZ"] is long) ? (double)(long) jsonData_["positionZ"] : (double) jsonData_["positionZ"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationX" ) )
		{
			result.rotationX = ( jsonData_["rotationX"] is long) ? (double)(long) jsonData_["rotationX"] : (double) jsonData_["rotationX"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationY" ) )
		{
			result.rotationY = ( jsonData_["rotationY"] is long) ? (double)(long) jsonData_["rotationY"] : (double) jsonData_["rotationY"];
		}
	
		if ( jsonData_.ContainsKey ( "rotationZ" ) )
		{
			result.rotationZ = ( jsonData_["rotationZ"] is long) ? (double)(long) jsonData_["rotationZ"] : (double) jsonData_["rotationZ"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleX" ) )
		{
			result.scaleX = ( jsonData_["scaleX"] is long) ? (double)(long) jsonData_["scaleX"] : (double) jsonData_["scaleX"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleY" ) )
		{
			result.scaleY = ( jsonData_["scaleY"] is long) ? (double)(long) jsonData_["scaleY"] : (double) jsonData_["scaleY"];
		}
	
		if ( jsonData_.ContainsKey ( "scaleZ" ) )
		{
			result.scaleZ = ( jsonData_["scaleZ"] is long) ? (double)(long) jsonData_["scaleZ"] : (double) jsonData_["scaleZ"];
		}
	
		return result;
	}
	
	
	
	
	
	
	
	
	private SceneSkyboxData DeserializeJson_SceneSkyboxData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SceneSkyboxData result = new SceneSkyboxData();
		if ( jsonData_.ContainsKey ( "x" ) )
		{
			result.x = ( jsonData_["x"] is long) ? (double)(long) jsonData_["x"] : (double) jsonData_["x"];
		}
	
		if ( jsonData_.ContainsKey ( "y" ) )
		{
			result.y = ( jsonData_["y"] is long) ? (double)(long) jsonData_["y"] : (double) jsonData_["y"];
		}
	
		if ( jsonData_.ContainsKey ( "z" ) )
		{
			result.z = ( jsonData_["z"] is long) ? (double)(long) jsonData_["z"] : (double) jsonData_["z"];
		}
	
		if ( jsonData_.ContainsKey ( "far" ) )
		{
			result.far = DeserializeJson_SkyboxItem( jsonData_["far"] as Dictionary<string,object>);
		}
	
		if ( jsonData_.ContainsKey ( "near" ) )
		{
			result.near = DeserializeJson_SkyboxItem( jsonData_["near"] as Dictionary<string,object>);
		}
	
		return result;
	}
	
	private SkyboxItem DeserializeJson_SkyboxItem(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SkyboxItem result = new SkyboxItem();
		if ( jsonData_.ContainsKey ( "local_p_x" ) )
		{
			result.local_p_x = ( jsonData_["local_p_x"] is long) ? (double)(long) jsonData_["local_p_x"] : (double) jsonData_["local_p_x"];
		}
	
		if ( jsonData_.ContainsKey ( "local_p_y" ) )
		{
			result.local_p_y = ( jsonData_["local_p_y"] is long) ? (double)(long) jsonData_["local_p_y"] : (double) jsonData_["local_p_y"];
		}
	
		if ( jsonData_.ContainsKey ( "local_p_z" ) )
		{
			result.local_p_z = ( jsonData_["local_p_z"] is long) ? (double)(long) jsonData_["local_p_z"] : (double) jsonData_["local_p_z"];
		}
	
		if ( jsonData_.ContainsKey ( "local_r_x" ) )
		{
			result.local_r_x = ( jsonData_["local_r_x"] is long) ? (double)(long) jsonData_["local_r_x"] : (double) jsonData_["local_r_x"];
		}
	
		if ( jsonData_.ContainsKey ( "local_r_y" ) )
		{
			result.local_r_y = ( jsonData_["local_r_y"] is long) ? (double)(long) jsonData_["local_r_y"] : (double) jsonData_["local_r_y"];
		}
	
		if ( jsonData_.ContainsKey ( "local_r_z" ) )
		{
			result.local_r_z = ( jsonData_["local_r_z"] is long) ? (double)(long) jsonData_["local_r_z"] : (double) jsonData_["local_r_z"];
		}
	
		if ( jsonData_.ContainsKey ( "local_s_x" ) )
		{
			result.local_s_x = ( jsonData_["local_s_x"] is long) ? (double)(long) jsonData_["local_s_x"] : (double) jsonData_["local_s_x"];
		}
	
		if ( jsonData_.ContainsKey ( "local_s_y" ) )
		{
			result.local_s_y = ( jsonData_["local_s_y"] is long) ? (double)(long) jsonData_["local_s_y"] : (double) jsonData_["local_s_y"];
		}
	
		if ( jsonData_.ContainsKey ( "local_s_z" ) )
		{
			result.local_s_z = ( jsonData_["local_s_z"] is long) ? (double)(long) jsonData_["local_s_z"] : (double) jsonData_["local_s_z"];
		}
	
		return result;
	}
	
	
	
	
	
	public Dictionary<string,object> SerializeJson_SceneObjectsInfoSet(SceneObjectsInfoSet objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.sceneID!=null) jsonData.Add("sceneID", objectData_.sceneID);
	
		{
			object data = SerializeJson_Dict_SceneNode(objectData_.sceneNodeDic);
			if(data!=null) jsonData.Add("sceneNodeDic",data);
		}
	
		{
			object data = SerializeJson_SceneSkyboxData(objectData_.skyboxData);
			if(data!=null) jsonData.Add("skyboxData",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_SceneNode(Dictionary<string,SceneNode> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,SceneNode> keyValue in dict_)
		{
			object current = SerializeJson_SceneNode(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SceneNode(SceneNode objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.nodeName!=null) jsonData.Add("nodeName", objectData_.nodeName);
	
		{
			object data = SerializeJson_Dict_SceneObjectData(objectData_.sceneObjectDataDic);
			if(data!=null) jsonData.Add("sceneObjectDataDic",data);
		}
	
		{
			object data = SerializeJson_Dict_SceneObjectData(objectData_.colliderObjectDataDic);
			if(data!=null) jsonData.Add("colliderObjectDataDic",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_SceneObjectData(Dictionary<string,SceneObjectData> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,SceneObjectData> keyValue in dict_)
		{
			object current = SerializeJson_SceneObjectData(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SceneObjectData(SceneObjectData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.objectName!=null) jsonData.Add("objectName", objectData_.objectName);
	
		if(objectData_.objectPathName!=null) jsonData.Add("objectPathName", objectData_.objectPathName);
	
		if(objectData_.parentName!=null) jsonData.Add("parentName", objectData_.parentName);
	
		{
			object data = SerializeJson_List_SceneObjectTransFormData(objectData_.objectTransFormDataList);
			if(data!=null) jsonData.Add("objectTransFormDataList",data);
		}
	
		return jsonData;
	}
	
	private List<object> SerializeJson_List_SceneObjectTransFormData(List<SceneObjectTransFormData> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = SerializeJson_SceneObjectTransFormData(list_[i]);
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SceneObjectTransFormData(SceneObjectTransFormData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("positionX", objectData_.positionX);
	
		jsonData.Add("positionY", objectData_.positionY);
	
		jsonData.Add("positionZ", objectData_.positionZ);
	
		jsonData.Add("rotationX", objectData_.rotationX);
	
		jsonData.Add("rotationY", objectData_.rotationY);
	
		jsonData.Add("rotationZ", objectData_.rotationZ);
	
		jsonData.Add("scaleX", objectData_.scaleX);
	
		jsonData.Add("scaleY", objectData_.scaleY);
	
		jsonData.Add("scaleZ", objectData_.scaleZ);
	
		{
			object data = SerializeJson_Dict_SubObject(objectData_.subObjects);
			if(data!=null) jsonData.Add("subObjects",data);
		}
	
		{
			object data = SerializeJson_SceneObjectLightmapInfo(objectData_.lightmapInfo);
			if(data!=null) jsonData.Add("lightmapInfo",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_SubObject(Dictionary<string,SubObject> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,SubObject> keyValue in dict_)
		{
			object current = SerializeJson_SubObject(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SubObject(SubObject objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.objectName!=null) jsonData.Add("objectName", objectData_.objectName);
	
		{
			object data = SerializeJson_SceneObjectLightmapInfo(objectData_.lightMapInfo);
			if(data!=null) jsonData.Add("lightMapInfo",data);
		}
	
		{
			object data = SerializeJson_SubObjectTransformInfo(objectData_.transformInfo);
			if(data!=null) jsonData.Add("transformInfo",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SceneObjectLightmapInfo(SceneObjectLightmapInfo objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("lightmapIndex", objectData_.lightmapIndex);
	
		jsonData.Add("x", objectData_.x);
	
		jsonData.Add("y", objectData_.y);
	
		jsonData.Add("z", objectData_.z);
	
		jsonData.Add("w", objectData_.w);
	
		return jsonData;
	}
	
	
	private Dictionary<string,object> SerializeJson_SubObjectTransformInfo(SubObjectTransformInfo objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("positionX", objectData_.positionX);
	
		jsonData.Add("positionY", objectData_.positionY);
	
		jsonData.Add("positionZ", objectData_.positionZ);
	
		jsonData.Add("rotationX", objectData_.rotationX);
	
		jsonData.Add("rotationY", objectData_.rotationY);
	
		jsonData.Add("rotationZ", objectData_.rotationZ);
	
		jsonData.Add("scaleX", objectData_.scaleX);
	
		jsonData.Add("scaleY", objectData_.scaleY);
	
		jsonData.Add("scaleZ", objectData_.scaleZ);
	
		return jsonData;
	}
	
	
	
	
	
	
	
	
	private Dictionary<string,object> SerializeJson_SceneSkyboxData(SceneSkyboxData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("x", objectData_.x);
	
		jsonData.Add("y", objectData_.y);
	
		jsonData.Add("z", objectData_.z);
	
		{
			object data = SerializeJson_SkyboxItem(objectData_.far);
			if(data!=null) jsonData.Add("far",data);
		}
	
		{
			object data = SerializeJson_SkyboxItem(objectData_.near);
			if(data!=null) jsonData.Add("near",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_SkyboxItem(SkyboxItem objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("local_p_x", objectData_.local_p_x);
	
		jsonData.Add("local_p_y", objectData_.local_p_y);
	
		jsonData.Add("local_p_z", objectData_.local_p_z);
	
		jsonData.Add("local_r_x", objectData_.local_r_x);
	
		jsonData.Add("local_r_y", objectData_.local_r_y);
	
		jsonData.Add("local_r_z", objectData_.local_r_z);
	
		jsonData.Add("local_s_x", objectData_.local_s_x);
	
		jsonData.Add("local_s_y", objectData_.local_s_y);
	
		jsonData.Add("local_s_z", objectData_.local_s_z);
	
		return jsonData;
	}
	
	
	
	
	
}
