using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonServerPlayerMessageListParser
{
	public ServerPlayerMessageList DeserializeJson_ServerPlayerMessageList(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ServerPlayerMessageList result = new ServerPlayerMessageList();
		if ( jsonData_.ContainsKey ( "players" ) )
		{
			List<object> data = jsonData_["players"] as List<object>;
			result.players = new List<ServerPlayerMessage>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				ServerPlayerMessage current = DeserializeJson_ServerPlayerMessage(data[i]  as Dictionary<string,object>);
				if ( current != null ) { result.players.Add(current); }
			}
		}
	
		return result;
	}
	
	private ServerPlayerMessage DeserializeJson_ServerPlayerMessage(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ServerPlayerMessage result = new ServerPlayerMessage();
		if ( jsonData_.ContainsKey ( "id" ) )
		{
			result.id = jsonData_["id"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "nickname" ) )
		{
			result.nickname = jsonData_["nickname"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "grade" ) )
		{
			result.grade = (int)(long) jsonData_["grade"];
		}
	
		if ( jsonData_.ContainsKey ( "gender" ) )
		{
			result.gender = (int)(long) jsonData_["gender"];
		}
	
		if ( jsonData_.ContainsKey ( "serviceId" ) )
		{
			result.serviceId = (int)(long) jsonData_["serviceId"];
		}
	
		return result;
	}
	
	
	
	public Dictionary<string,object> SerializeJson_ServerPlayerMessageList(ServerPlayerMessageList objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_List_ServerPlayerMessage(objectData_.players);
			if(data!=null) jsonData.Add("players",data);
		}
	
		return jsonData;
	}
	
	private List<object> SerializeJson_List_ServerPlayerMessage(List<ServerPlayerMessage> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = SerializeJson_ServerPlayerMessage(list_[i]);
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_ServerPlayerMessage(ServerPlayerMessage objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.id!=null) jsonData.Add("id", objectData_.id);
	
		if(objectData_.nickname!=null) jsonData.Add("nickname", objectData_.nickname);
	
		jsonData.Add("grade", objectData_.grade);
	
		jsonData.Add("gender", objectData_.gender);
	
		jsonData.Add("serviceId", objectData_.serviceId);
	
		return jsonData;
	}
	
	
	
}
