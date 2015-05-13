using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using System;
using System.Text;

public class JsonSoundConfigDataParser
{
	public SoundConfigData DeserializeJson_SoundConfigData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		SoundConfigData result = new SoundConfigData();
		if ( jsonData_.ContainsKey ( "modelSounds" ) )
		{
			Dictionary<string,object> data = jsonData_["modelSounds"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.modelSounds = new Dictionary<string,AnimationSoundConfigData>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					AnimationSoundConfigData current = DeserializeJson_AnimationSoundConfigData(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.modelSounds.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		if ( jsonData_.ContainsKey ( "effectSounds" ) )
		{
			Dictionary<string,object> data = jsonData_["effectSounds"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.effectSounds = new Dictionary<string,EffectSoundConfigData>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					EffectSoundConfigData current = DeserializeJson_EffectSoundConfigData(keyValue.Value as Dictionary<string,object>);
					if ( current != null ) { result.effectSounds.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private AnimationSoundConfigData DeserializeJson_AnimationSoundConfigData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AnimationSoundConfigData result = new AnimationSoundConfigData();
		if ( jsonData_.ContainsKey ( "sourceID" ) )
		{
			result.sourceID = jsonData_["sourceID"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "animationSoundDic" ) )
		{
			Dictionary<string,object> data = jsonData_["animationSoundDic"] as Dictionary<string,object>;
			if ( data != null )
			{
				result.animationSoundDic = new Dictionary<string,List<AudioInfo>>();
				foreach(KeyValuePair<string,object> keyValue in data)
				{
					List<AudioInfo> current = DeserializeJson_List_AudioInfo(keyValue.Value as List<object>);
					if ( current != null ) { result.animationSoundDic.Add ( keyValue.Key, current ); }
				}
			}
		}
	
		return result;
	}
	
	private List<AudioInfo> DeserializeJson_List_AudioInfo ( List<object> jsonData_ )
	{
		if(jsonData_ == null) return null;
		List<AudioInfo> resultList = new List<AudioInfo>();
		for(int i = 0 , imax = jsonData_.Count ; i < imax ; ++i )
		{
			AudioInfo current = DeserializeJson_AudioInfo(jsonData_[i]  as Dictionary<string,object>);
			if(current != null ) resultList.Add(current);
		}
		return resultList;
	}
	
	private AudioInfo DeserializeJson_AudioInfo(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		AudioInfo result = new AudioInfo();
		if ( jsonData_.ContainsKey ( "audioType" ) )
		{
			result.audioType = (int)(long) jsonData_["audioType"];
		}
	
		if ( jsonData_.ContainsKey ( "audioName" ) )
		{
			result.audioName = jsonData_["audioName"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "isLoop" ) )
		{
			result.isLoop = (bool) jsonData_["isLoop"];
		}
	
		if ( jsonData_.ContainsKey ( "delayTime" ) )
		{
			result.delayTime = ( jsonData_["delayTime"] is long) ? (double)(long) jsonData_["delayTime"] : (double) jsonData_["delayTime"];
		}
	
		if ( jsonData_.ContainsKey ( "stayTime" ) )
		{
			result.stayTime = ( jsonData_["stayTime"] is long) ? (double)(long) jsonData_["stayTime"] : (double) jsonData_["stayTime"];
		}
	
		if ( jsonData_.ContainsKey ( "fadeInTime" ) )
		{
			result.fadeInTime = ( jsonData_["fadeInTime"] is long) ? (double)(long) jsonData_["fadeInTime"] : (double) jsonData_["fadeInTime"];
		}
	
		if ( jsonData_.ContainsKey ( "fadeOutTime" ) )
		{
			result.fadeOutTime = ( jsonData_["fadeOutTime"] is long) ? (double)(long) jsonData_["fadeOutTime"] : (double) jsonData_["fadeOutTime"];
		}
	
		return result;
	}
	
	
	
	private EffectSoundConfigData DeserializeJson_EffectSoundConfigData(Dictionary<string,object> jsonData_)
	{
		if (jsonData_ == null) return null;
		EffectSoundConfigData result = new EffectSoundConfigData();
		if ( jsonData_.ContainsKey ( "effect" ) )
		{
			result.effect = jsonData_["effect"] as string;
		}
	
		if ( jsonData_.ContainsKey ( "audioInfoList" ) )
		{
			List<object> data = jsonData_["audioInfoList"] as List<object>;
			result.audioInfoList = new List<AudioInfo>();
			for ( int i = 0 , imax = data.Count; i < imax ; ++i )
			{
				AudioInfo current = DeserializeJson_AudioInfo(data[i]  as Dictionary<string,object>);
				if ( current != null ) { result.audioInfoList.Add(current); }
			}
		}
	
		return result;
	}
	
	
	
	
	public Dictionary<string,object> SerializeJson_SoundConfigData(SoundConfigData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		{
			object data = SerializeJson_Dict_AnimationSoundConfigData(objectData_.modelSounds);
			if(data!=null) jsonData.Add("modelSounds",data);
		}
	
		{
			object data = SerializeJson_Dict_EffectSoundConfigData(objectData_.effectSounds);
			if(data!=null) jsonData.Add("effectSounds",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_AnimationSoundConfigData(Dictionary<string,AnimationSoundConfigData> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,AnimationSoundConfigData> keyValue in dict_)
		{
			object current = SerializeJson_AnimationSoundConfigData(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AnimationSoundConfigData(AnimationSoundConfigData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.sourceID!=null) jsonData.Add("sourceID", objectData_.sourceID);
	
		{
			object data = SerializeJson_Dict_List_AudioInfo(objectData_.animationSoundDic);
			if(data!=null) jsonData.Add("animationSoundDic",data);
		}
	
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_Dict_List_AudioInfo(Dictionary<string,List<AudioInfo>> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,List<AudioInfo>> keyValue in dict_)
		{
			object current = SerializeJson_List_AudioInfo(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private List<object> SerializeJson_List_AudioInfo(List<AudioInfo> list_)
	{
		List<object> jsonData = new List<object>();
		for(int i = 0 , imax = list_.Count; i < imax ; ++i )
		{
			object current = SerializeJson_AudioInfo(list_[i]);
			if(current!=null) jsonData.Add(current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_AudioInfo(AudioInfo objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		jsonData.Add("audioType",objectData_.audioType);
	
		if(objectData_.audioName!=null) jsonData.Add("audioName", objectData_.audioName);
	
		jsonData.Add("isLoop",objectData_.isLoop);
	
		jsonData.Add("delayTime",objectData_.delayTime);
	
		jsonData.Add("stayTime",objectData_.stayTime);
	
		jsonData.Add("fadeInTime",objectData_.fadeInTime);
	
		jsonData.Add("fadeOutTime",objectData_.fadeOutTime);
	
		return jsonData;
	}
	
	
	
	private Dictionary<string,object> SerializeJson_Dict_EffectSoundConfigData(Dictionary<string,EffectSoundConfigData> dict_)
	{
		if(dict_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		foreach(KeyValuePair<string,EffectSoundConfigData> keyValue in dict_)
		{
			object current = SerializeJson_EffectSoundConfigData(keyValue.Value);
			if(current!=null) jsonData.Add(keyValue.Key,current);
		}
		return jsonData;
	}
	
	private Dictionary<string,object> SerializeJson_EffectSoundConfigData(EffectSoundConfigData objectData_)
	{
		if(objectData_==null) return null;
		Dictionary<string,object> jsonData = new Dictionary<string,object>();
		if(objectData_.effect!=null) jsonData.Add("effect", objectData_.effect);
	
		{
			object data = SerializeJson_List_AudioInfo(objectData_.audioInfoList);
			if(data!=null) jsonData.Add("audioInfoList",data);
		}
	
		return jsonData;
	}
	
	
	
	
}
