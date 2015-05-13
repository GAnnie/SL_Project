using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoPlayVoiceModel {



	private static readonly AutoPlayVoiceModel instance = new AutoPlayVoiceModel();
	public static AutoPlayVoiceModel Instance
	{
		get{
			return instance;
		}
	}
	
	public event System.Action<string> AutoPlayVoice;
	
	List < string > voiceList = new List < string >();

	
	public void AddVoice(string str){

		if(voiceList.Count > 10){
			voiceList.RemoveAt(0);
		}

		voiceList.Add(str);

		JudgeVoiceList();
	}
	
	
	/**
	 * 是否正在播放
	 */
	private bool _isPlayingVoice = false;
	
	public void SetIsPlaying(bool b){
		_isPlayingVoice = b;
	}
	
	/**
	 * 判断是否正在播放
	 */
	public void JudgeVoiceList()
	{
		if( voiceList.Count > 0 )
		{
			if(!_isPlayingVoice){
				PlayVoice( voiceList[ 0 ]  );
				voiceList.RemoveAt(0);
			}
		}
		else 
		{
//			TipManager.AddTip("回复背景音乐");
			AudioManager.Instance.PlayVoiceWhenFinishRecord();
			PlayVoice( null );
		}
	}


	//自动播放语音
	public void PlayVoice( string  msg )
	{
		if(string.IsNullOrEmpty(msg)) return;
		
		if( AutoPlayVoice != null )
		{
			AutoPlayVoice(msg);
		}
	}

}
