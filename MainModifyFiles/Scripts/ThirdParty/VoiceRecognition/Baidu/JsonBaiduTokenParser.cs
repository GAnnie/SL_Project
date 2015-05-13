using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonBaiduTokenParser
{
	public BaiduToken DeserializeJson_BaiduToken(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		BaiduToken result = new BaiduToken();
		if ( jsonData_.ContainsKey ( "access_token" ) )
		{
			result.access_token = jsonData_["access_token"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "session_key" ) )
		{
			result.session_key = jsonData_["session_key"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "scope" ) )
		{
			result.scope = jsonData_["scope"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "refresh_token" ) )
		{
			result.refresh_token = jsonData_["refresh_token"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "session_secret" ) )
		{
			result.session_secret = jsonData_["session_secret"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "expires_in" ) )
		{
			result.expires_in = jsonData_["expires_in"] as string;
		}
	
		return result;
	}
	
	
	public Dictionary<string,object> SerializeJson_BaiduToken(BaiduToken objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.access_token!=null) jsonData.Add("access_token", objectData_.access_token);
	
		if(objectData_.session_key!=null) jsonData.Add("session_key", objectData_.session_key);
	
		if(objectData_.scope!=null) jsonData.Add("scope", objectData_.scope);
	
		if(objectData_.refresh_token!=null) jsonData.Add("refresh_token", objectData_.refresh_token);
	
		if(objectData_.session_secret!=null) jsonData.Add("session_secret", objectData_.session_secret);
	
		if(objectData_.expires_in!=null) jsonData.Add("expires_in", objectData_.expires_in);
	
		return jsonData;
	}
	
	
}
