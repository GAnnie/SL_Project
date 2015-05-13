using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonServerMessageListParser
{
	public ServerMessageList DeserializeJson_ServerMessageList(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ServerMessageList result = new ServerMessageList();
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
			result.list = new List<ServerInfo>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				ServerInfo current = DeserializeJson_ServerInfo(data[i]  as Dictionary<string,object>);

				current.gservice = result.gservice.Replace("[targetServiceId]", current.targetServiceId.ToString());
				if (string.IsNullOrEmpty(current.host))
				{
					current.host = result.host.Replace("[targetServiceId]", current.targetServiceId.ToString());
				}
				current.loginUrl = result.loginUrl.Replace("[targetServiceId]", current.targetServiceId.ToString());
				current.payUrl = result.payUrl.Replace("[targetServiceId]", current.targetServiceId.ToString());

				if ( current != null ) { result.list.Add(current); }
			}
		}
	
		return result;
	}
	
	private ServerInfo DeserializeJson_ServerInfo(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ServerInfo result = new ServerInfo();
		if ( jsonData_.ContainsKey ( "name" ) )
		{
			result.name = jsonData_["name"] as string;
		}

		if ( jsonData_.ContainsKey ( "host" ) )
		{
			result.host = jsonData_["host"] as string;
		}

		if ( jsonData_.ContainsKey ( "targetServiceId" ) )
		{
			result.targetServiceId = (int)(long) jsonData_["targetServiceId"];
		}

		if ( jsonData_.ContainsKey ( "accessId" ) )
		{
			result.accessId = (int)(long) jsonData_["accessId"];
		}
	
		if ( jsonData_.ContainsKey ( "serviceId" ) )
		{
			result.serviceId = (int)(long) jsonData_["serviceId"];
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
	
		if ( jsonData_.ContainsKey ( "port" ) )
		{
			result.port = (int)(long) jsonData_["port"];
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
	
	
	
	public Dictionary<string,object> SerializeJson_ServerMessageList(ServerMessageList objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.gservice!=null)
		{
			jsonData.Add("gservice", objectData_.gservice);
			jsonData.Add("host", objectData_.host);
			jsonData.Add("loginUrl", objectData_.loginUrl);
			jsonData.Add("payUrl", objectData_.payUrl);
		}
	
		{
			object data = SerializeJson_List_ServerInfo(objectData_.list);
			if(data!=null) jsonData.Add("list",data);
		}
	
		return jsonData;
	}
	
	private List<object> SerializeJson_List_ServerInfo(List<ServerInfo> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = SerializeJson_ServerInfo(list_[i]);
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_ServerInfo(ServerInfo objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.name!=null) jsonData.Add("name", objectData_.name);
	
		jsonData.Add("host", objectData_.host);

		jsonData.Add("accessId", objectData_.accessId);
	
		jsonData.Add("serviceId", objectData_.serviceId);

		jsonData.Add("targetServiceId", objectData_.targetServiceId);
	
		jsonData.Add("runState", objectData_.runState);
		
		jsonData.Add("dboState", objectData_.dboState);
	
		jsonData.Add("recommend", objectData_.recommend);
	
		jsonData.Add("port", objectData_.port);
	
		jsonData.Add("limitVer", objectData_.limitVer);
		
		jsonData.Add("limitMaxVer", objectData_.limitMaxVer);
		
		return jsonData;
	}
	
	
	
}
