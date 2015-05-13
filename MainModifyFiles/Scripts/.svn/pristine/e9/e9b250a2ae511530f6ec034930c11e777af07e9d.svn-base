using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonKDTSServerMessageListParser
{
	public KDTSServerMessageList DeserializeJson_KDTSServerMessageList(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		KDTSServerMessageList result = new KDTSServerMessageList();
		if ( jsonData_.ContainsKey ( "gservice" ) )
		{
			result.gservice = jsonData_["gservice"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "host" ) )
		{
			result.host = jsonData_["host"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "loginUrl" ) )
		{
			result.loginUrl = jsonData_["loginUrl"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "payUrl" ) )
		{
			result.payUrl = jsonData_["payUrl"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "list" ) )
		{
			List<object> data = jsonData_["list"] as List<object>;
			result.list = new List<KDTSServerInfo>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				KDTSServerInfo current = DeserializeJson_KDTSServerInfo(data[i]  as Dictionary<string,object>);
				if ( current != null ) { result.list.Add(current); }
			}
		}
	
		return result;
	}
	
	private KDTSServerInfo DeserializeJson_KDTSServerInfo(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		KDTSServerInfo result = new KDTSServerInfo();
		if ( jsonData_.ContainsKey ( "targetServiceId" ) )
		{
			result.targetServiceId = (int)(long) jsonData_["targetServiceId"];
		}
	
		if ( jsonData_.ContainsKey ( "port" ) )
		{
			result.port = (int)(long) jsonData_["port"];
		}
	
		if ( jsonData_.ContainsKey ( "serviceId" ) )
		{
			result.serviceId = (int)(long) jsonData_["serviceId"];
		}
	
		if ( jsonData_.ContainsKey ( "needPayUrl" ) )
		{
			result.needPayUrl = (bool) jsonData_["needPayUrl"];
		}
	
		if ( jsonData_.ContainsKey ( "accessId" ) )
		{
			result.accessId = (int)(long) jsonData_["accessId"];
		}
	
		if ( jsonData_.ContainsKey ( "name" ) )
		{
			result.name = jsonData_["name"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "runState" ) )
		{
			result.runState = (int)(long) jsonData_["runState"];
		}
	
		if ( jsonData_.ContainsKey ( "dboState" ) )
		{
			result.dboState = (int)(long) jsonData_["dboState"];
		}
	
		if ( jsonData_.ContainsKey ( "recommend" ) )
		{
			result.recommend = (int)(long) jsonData_["recommend"];
		}
	
		if ( jsonData_.ContainsKey ( "limitVer" ) )
		{
			result.limitVer = (int)(long) jsonData_["limitVer"];
		}
	
		if ( jsonData_.ContainsKey ( "limitMaxVer" ) )
		{
			result.limitMaxVer = (int)(long) jsonData_["limitMaxVer"];
		}
	
		return result;
	}
	
	
	
	public Dictionary<string,object> SerializeJson_KDTSServerMessageList(KDTSServerMessageList objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.gservice!=null) jsonData.Add("gservice", objectData_.gservice);
	
		if(objectData_.host!=null) jsonData.Add("host", objectData_.host);
	
		if(objectData_.loginUrl!=null) jsonData.Add("loginUrl", objectData_.loginUrl);
	
		if(objectData_.payUrl!=null) jsonData.Add("payUrl", objectData_.payUrl);
	
		{
			object data = SerializeJson_List_KDTSServerInfo(objectData_.list);
			if(data!=null) jsonData.Add("list",data);
		}
	
		return jsonData;
	}
	
	private List<object> SerializeJson_List_KDTSServerInfo(List<KDTSServerInfo> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = SerializeJson_KDTSServerInfo(list_[i]);
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_KDTSServerInfo(KDTSServerInfo objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("targetServiceId", objectData_.targetServiceId);
	
		jsonData.Add("port", objectData_.port);
	
		jsonData.Add("serviceId", objectData_.serviceId);
	
		jsonData.Add("needPayUrl", objectData_.needPayUrl);
	
		jsonData.Add("accessId", objectData_.accessId);
	
		if(objectData_.name!=null) jsonData.Add("name", objectData_.name);
	
		jsonData.Add("runState", objectData_.runState);
	
		jsonData.Add("dboState", objectData_.dboState);
	
		jsonData.Add("recommend", objectData_.recommend);
	
		jsonData.Add("limitVer", objectData_.limitVer);
	
		jsonData.Add("limitMaxVer", objectData_.limitMaxVer);
	
		return jsonData;
	}
	
	
	
}
