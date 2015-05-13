using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class ExperienceData
{
	public List< string > data = null;
	
	public ExperienceData ()
	{
		
	}
}

public class JsonExperienceDataParser
{
	public ExperienceData DeserializeJson_ExperienceData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		ExperienceData result = new ExperienceData();
		if ( jsonData_.ContainsKey ( "data" ) )
		{
			List<object> data = jsonData_["data"] as List<object>;
			result.data = new List<string>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				string current = data[i] as string;
				if ( current != null ) { result.data.Add(current); }
			}
		}
	
		return result;
	}
	
	
	
	public Dictionary<string,object> SerializeJson_ExperienceData(ExperienceData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_List(objectData_.data);
			if(data!=null) jsonData.Add("data",data);
		}
	
		return jsonData;
	}
	
	private List<object> SerializeJson_List(List<string> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = list_[i];
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	
	
}
