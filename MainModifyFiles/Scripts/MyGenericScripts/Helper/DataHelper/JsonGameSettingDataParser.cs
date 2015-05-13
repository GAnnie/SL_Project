using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonGameSettingDataParser
{
	public GameSettingData DeserializeJson_GameSettingData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		GameSettingData result = new GameSettingData();

		if ( jsonData_.ContainsKey ( "resPath" ) )
		{
			result.resPath = jsonData_["resPath"] as string;
		}

		if ( jsonData_.ContainsKey ( "httpPath" ) )
		{
			result.httpPath = jsonData_["httpPath"] as string;
		}

		if ( jsonData_.ContainsKey ( "gameType" ) )
		{
			result.gameType = (int)(long) jsonData_["gameType"];
		}		
		
		if ( jsonData_.ContainsKey ( "loadMode" ) )
		{
			result.loadMode = (int)(long) jsonData_["loadMode"];
		}
	
		if ( jsonData_.ContainsKey ( "mobileMode" ) )
		{
			result.mobileMode = (bool) jsonData_["mobileMode"];
		}
	
		if ( jsonData_.ContainsKey ( "clientMode" ) )
		{
			result.clientMode = (int)(long) jsonData_["clientMode"];
		}
	
		if ( jsonData_.ContainsKey ( "unityDebugMode" ) )
		{
			result.unityDebugMode = (bool) jsonData_["unityDebugMode"];
		}
	
		if ( jsonData_.ContainsKey ( "release" ) )
		{
			result.release = (bool) jsonData_["release"];
		}
	
		if ( jsonData_.ContainsKey ( "collectDebugInfo" ) )
		{
			result.collectDebugInfo = (bool) jsonData_["collectDebugInfo"];
		}
	
		return result;
	}
	
	
	public Dictionary<string,object> SerializeJson_GameSettingData(GameSettingData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();

		if(objectData_.resPath!=null) jsonData.Add("resPath", objectData_.resPath);

		if(objectData_.httpPath!=null) jsonData.Add("httpPath", objectData_.httpPath);
	
		jsonData.Add("gameType", objectData_.gameType);
		
		jsonData.Add("loadMode", objectData_.loadMode);
	
		jsonData.Add("mobileMode", objectData_.mobileMode);
	
		jsonData.Add("clientMode", objectData_.clientMode);
	
		jsonData.Add("unityDebugMode", objectData_.unityDebugMode);
	
		jsonData.Add("release", objectData_.release);
	
		jsonData.Add("collectDebugInfo", objectData_.collectDebugInfo);
	
		return jsonData;
	}
	
	
}
