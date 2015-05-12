using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonNGUIAtlasSetParser
{
	public NGUIAtlasSet DeserializeJson_NGUIAtlasSet(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		NGUIAtlasSet result = new NGUIAtlasSet();
		if ( jsonData_.ContainsKey ( "nguiAtlasSet" ) )
		{
			Dictionary<string,object> data = jsonData_["nguiAtlasSet"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.nguiAtlasSet = new Dictionary<string,string>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					string current = keyValue.Value as string;
					if ( current != null ) { result.nguiAtlasSet.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	
	
	public Dictionary<string,object> SerializeJson_NGUIAtlasSet(NGUIAtlasSet objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict(objectData_.nguiAtlasSet);
			if(data!=null) jsonData.Add("nguiAtlasSet",data);
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
