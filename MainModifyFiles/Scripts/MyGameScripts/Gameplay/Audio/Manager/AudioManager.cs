	// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AudioManager.cs
// Author   : wenlin
// Created  : 2013/4/16 14:48:34
// Purpose  : 
// **********************************************************************

using UnityEngine;
using System;
using System.Collections.Generic;
//using LitJson;

public class AudioManager
{
	private static AudioManager _instance = null;
	public static AudioManager Instance
	{
		get 
		{
			if (_instance == null)
			{
				_instance = new AudioManager();
			}
			return _instance;
		}
	}

	private AudioCategory _musicCategory = null;
	private AudioCategory _soundCategory = null;
	private Transform _musicController = null;
	private Transform _soundController = null;
	private Dictionary<string, GameAudioItem> _musicItemDic = null;
	private Dictionary<string, GameAudioItem> _soundItemDic = null;
	private AudioManager() 
	{
		_musicCategory = GetAudioCategory("Music");
		_soundCategory = GetAudioCategory("Sound");

		GameObject musicCtl = GameObject.Find("MusicController");
		GameObject soundCtl = GameObject.Find("SoundController");

		if (musicCtl != null) _musicController = musicCtl.transform;
		if (soundCtl != null) _soundController = soundCtl.transform;

		_musicItemDic = new Dictionary<string, GameAudioItem>();
		_soundItemDic = new Dictionary<string, GameAudioItem>();
	}

	//SoundConfigData
	private SoundConfigData _soundConfigData = null;
	//SoundConfigData Path
	private const string soundConfigFile = "SoundConfigFile";
	//private const string soundConfigFilePath = SystemHelper.BATTLE_CONFIG_PATH + soundConfigFile;
	
	/// <summary>
	/// Setup AudioManager
	/// </summary>
	public void Setup()
	{
//        TextAsset textAsset = ResourceLoader.Load(soundConfigFilePath, "txt") as TextAsset;
//        if (textAsset != null)
//        {
//            Dictionary<string, object> obj = DataHelper.GetJsonFile(textAsset.bytes, false);
//            JsonSoundConfigDataParser parser = new JsonSoundConfigDataParser();
//            _soundConfigData = parser.DeserializeJson_SoundConfigData(obj);
//        }
//        else
//        {
//            GameDebuger.Log(" Error : Init SoundConfigData Error !!!!!!");
//        }
	}

	/// <summary>
	/// Get assgin AudioCategory , if no Exsic , then create it
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	private AudioCategory GetAudioCategory(string name)
	{
		AudioCategory category = AudioController.GetCategory(name);
		if (category == null)
		{
			category = AudioController.NewCategory(name);
		}
		return category;
	}



	/// <summary>
	/// Play Music 
	/// </summary>
	/// <param name="musicName"></param>
	/// <param name="option"></param>
	/// 
	private AudioObject _curAudioObject = null;   //GET WARNING
	private string _curMusicName = "";
	public bool toggleMusic = true;
	public bool PlayMusic(string musicName, AudioOption option = null )
	{
		if (!toggleMusic) {
			GameDebuger.Log("toggleMusic is false");
			return false;
		}

		if ( string.IsNullOrEmpty( musicName ) )
		{
			GameDebuger.Log("Music name is null");
			return false;             
		}

		if (_curMusicName == musicName)
		{
			GameDebuger.Log( "Music : " + musicName + "is Playing ") ;
			return false;
		}

		GameDebuger.Log("Play music : " + musicName );


		GameAudioItem gameItem = null;
		if (!_musicItemDic.ContainsKey(musicName))
		{
			AudioClip audioClip = ResourceLoader.Load(PathHelper.AUDIO_MUSIC_PATH + musicName, "mp3") as AudioClip;
			if (audioClip == null)
			{
				GameDebuger.Log("Can not find the music of " + musicName);
				return false;
			}

			AudioItem item = AudioController.AddToCategory(_musicCategory, audioClip, musicName);
			item.MaxInstanceCount = 1;

			item.DestroyOnLoad = false;
			gameItem = new GameAudioItem(musicName, item, 2);
			_musicItemDic.Add(musicName, gameItem);
		}
		else
		{
			gameItem = _musicItemDic[musicName];
		}

		gameItem.Reset();
		RemoveOutDataItem(_musicItemDic);

		if (option != null)
		{
			AudioController.Instance.specifyCrossFadeInAndOutSeperately = true;
			AudioController.Instance.musicCrossFadeTime_In = option.fadeInTime;
			AudioController.Instance.musicCrossFadeTime_Out = option.fadeOutTime;
			SetAudioOption(gameItem.Item, option);
		}
		else
		{
			AudioController.Instance.specifyCrossFadeInAndOutSeperately = false;
			AudioController.Instance.musicCrossFadeTime = 1;
		}

		_curMusicName = musicName;
		_curAudioObject = AudioController.PlayMusic(musicName);   //GET WARNING

		if (_musicController != null && _curAudioObject != null )
		{
			_curAudioObject.gameObject.transform.parent = _musicController;
			_curAudioObject.gameObject.audio.loop = true;
		}
		
		return true;
	}

	/// <summary>
	/// Play aniamtion sound
	/// </summary>
	/// <param name="model"></param>
	/// <param name="animation"></param>
	/// <returns></returns>
	private  const string petHeadString = "pet_";
	public bool PlayAnimationSound(string model, string animation)
	{
		if (_soundConfigData == null)
		{
			GameDebuger.Log(" SoundConfigData is NULL !!! ");
			return false;
		}

		string modelName = petHeadString + model;
		if (_soundConfigData.modelSounds.ContainsKey(modelName))
		{
			AnimationSoundConfigData data = _soundConfigData.modelSounds[modelName];
			if (data.animationSoundDic.ContainsKey(animation))
			{
				PlaySoundFormSoundList(data.animationSoundDic[animation]);
				return true;
			}
			else
			{
				//GameDebuger.Log("Model : " + modelName + " Animation : " + animation + " 's AnimationSound is not Config ");
			}
		}
		else
		{
			//GameDebuger.Log("Model : " + modelName + " 's AnimationSound is not Config ");
		}

		return false;
	}


	/// <summary>
	/// Playe Effect sound
	/// </summary>
	/// <param name="effect"></param>
	/// <returns></returns>
	public bool PlayEffectSound(string effect)
	{
		if (_soundConfigData == null)
		{
			GameDebuger.Log(" SoundConfigData is NULL !!! ");
			return false;
		}

		if (_soundConfigData.effectSounds.ContainsKey(effect))
		{
			EffectSoundConfigData data = _soundConfigData.effectSounds[effect];
			PlaySoundFormSoundList(data.audioInfoList);

			return true;
		}
		else
		{
			GameDebuger.Log("Effect : " + effect + " 's EffectSound is not Config ");
		}
		return false;
	}


	/// <summary>
	/// Play form animation sound data 
	/// </summary>
	/// <param name="data"></param>
	public void PlaySoundFormSoundList(List<AudioInfo> audioInfoList)
	{
		foreach (AudioInfo info in audioInfoList)
		{
			PlaySoundFromAudioInfo(info);
		}
	}

	/// <summary>
	/// 播放音效
	/// </summary>
	/// <param name="info"></param>
	public void PlaySoundFromAudioInfo(AudioInfo info)
	{
		if (info == null) return;

		AudioOption option = new AudioOption();
		option.fadeInTime = (float)(info.fadeInTime);
		option.fadeOutTime = (float)(info.fadeOutTime);
		option.stayTime = (float)(info.stayTime);
		option.delayTime = (float)(info.delayTime);
		option.isLoop = info.isLoop;

		PlaySound(info.audioName, option);
	}

	/// <summary>
	/// Play Sound
	/// </summary>
	/// <param name="soundName"></param>
	/// <param name="option"></param>
	private bool toggleSound = true;
	public AudioObject PlaySound(string soundName, AudioOption option = null)
	{
		if (!toggleSound) {
			GameDebuger.Log("toggleSound is false");
			return null;
		}

		if (string.IsNullOrEmpty(soundName))
		{
			GameDebuger.Log("Sound name is null");
			return null;
		}

		GameAudioItem gameItem = null;
		if (!_soundItemDic.ContainsKey(soundName))
		{
			AudioClip audioClip = ResourceLoader.Load(PathHelper.AUDIO_SOUND_PATH + soundName, "mp3") as AudioClip;
			if (audioClip == null)
			{
				GameDebuger.Log("Can not find the sound of " + soundName);
				return null;
			}

			AudioItem item = AudioController.AddToCategory(_soundCategory, audioClip, soundName);
			gameItem = new GameAudioItem(soundName, item);
			item.DestroyOnLoad = false;
			_soundItemDic.Add(soundName, gameItem);
		}
		else
		{
			gameItem = _soundItemDic[soundName];
		}

		gameItem.Reset();
		RemoveOutDataItem(_soundItemDic);

		if (option == null) option = new AudioOption();

		if (option != null)
		{
			SetAudioOption(gameItem.Item, option);
		}

		AudioObject audioObject = AudioController.Play(soundName);
		if (_soundController != null && audioObject != null)
		{
			audioObject.gameObject.transform.parent = _soundController;
		}

		if (option != null)
		{
			SetAudioObject(audioObject, option);
		}

		return audioObject;
	}

	/// <summary>
	/// Play the list of music 
	/// </summary>
	/// <param name="audioList"></param>
	//public void PlayMusicList(List<WeatherAudio> audioList)
	//{
	//    List<string> musicList = new List<string>();
	//    foreach (WeatherAudio audio in audioList)
	//    {
	//        musicList.Add(audio.name);
	//    }

	//    PlayMusicList(musicList);
	//}

	//public void PlayMusicList(List<string> audioList)
	//{
	//    if (audioList == null) return;

	//    GameAudioItem gameItem = null;
	//    foreach (string audioName in audioList)
	//    {
	//        if ( !_musicItemDic.ContainsKey(audioName) )
	//        {
	//            AudioClip audioClip = ResourceLoader.Load(SystemHelper.AUDIO_MUSIC_PATH + audioName, "mp3") as AudioClip;
	//            if (audioClip == null)
	//            {
	//                GameDebuger.Log("Can not find the music of " + audioName);
	//                return;
	//            }

	//            AudioItem item = AudioController.AddToCategory(_musicCategory, audioClip, audioName);
	//            item.MaxInstanceCount = 1;

	//            item.DestroyOnLoad = false;
	//            gameItem = new GameAudioItem(audioName, item);
	//            _musicItemDic.Add(audioName, gameItem);
	//        }
	//    }

	//    RemoveOutDataItem(_musicItemDic);

	//    AudioController.Instance.specifyCrossFadeInAndOutSeperately = false;
	//    AudioController.Instance.musicCrossFadeTime = 1;

	//    AudioController.ClearPlaylist();
	//    AudioController.SetMusicPlaylist(audioList.ToArray());

	//    AudioController.PlayMusicPlaylist();
	//}

	public void PauseMusic(string musicName){}

	public void PauseAllMusic(){ }

	public void PauseSound(string soundName){}

	public void PauseAllSound(){}

	public void StopMusic()
	{
		if (_curMusicName != string.Empty)
		{
			if (_musicItemDic.ContainsKey(_curMusicName))
			{
				_soundItemDic.Remove(_curMusicName);
			}
			AudioController.Stop(_curMusicName);

			_curMusicName = "";
		}
	}

	public void StopSound(string soundName)
	{
		if (_soundItemDic.ContainsKey(soundName))
		{
			GameAudioItem item = _soundItemDic[soundName];

			if (item != null)
			{
				_soundItemDic.Remove(soundName);
				AudioController.Stop(soundName);
				AudioController.RemoveAudioItem(soundName);
			}

		}
	}

	/// <summary>
	/// 关闭所有Loop
	/// </summary>
	/// <returns></returns>
	public void StopAllLoopSound()
	{
		if (_soundItemDic == null) return;

		foreach (KeyValuePair<string, GameAudioItem> item in _soundItemDic)
		{
			if (item.Value.Item.Loop == AudioItem.LoopMode.LoopSubitem)
			{
				item.Value.SetOutTime();
			}
		}

		RemoveOutDataItem(_soundItemDic, false);

	}


	public void StopAll()
	{
		AudioController.StopAll();
	}

	private void SetAudioOption(AudioItem item, AudioOption option)
	{
		if (item == null || option == null) return;

		//item.subItems[0].ClipStartTime = option.startClipTime;
		//item.subItems[0].SubItemType = AudioSubItemType.Item;
		//item.subItems[0].ClipStopTime = option.stayTime > 0 ? option.stayTime : item.subItems[0].Clip.length;
		item.subItems[0].Delay = option.delayTime;
		if (option.isLoop)
		{
			item.Loop = AudioItem.LoopMode.LoopSubitem;
		}
		else
		{
			item.Loop = AudioItem.LoopMode.DoNotLoop;
		}
	}

	private void SetAudioObject(AudioObject audioObject, AudioOption option)
	{
		if (audioObject == null) return;

		if ( option.stayTime != 0 )
		{
			audioObject.Stop(option.fadeOutTime, option.stayTime);
		}
	}

	private void RemoveOutDataItem( Dictionary< string , GameAudioItem > dic, bool isUpdate = true )
	{
		if (dic == null) return;

		List<GameAudioItem> deleteItem = new List<GameAudioItem>();
		foreach (KeyValuePair<string, GameAudioItem> item in dic)
		{
			if (item.Value.IsOvertake)
			{
				if ( isUpdate ) deleteItem.Add(item.Value);
			}
			else
			{
				item.Value.AddAccNumber();
			}
		}

		foreach (GameAudioItem item in deleteItem)
		{
			dic.Remove(item.ItemName);
			AudioController.RemoveAudioItem(item.ItemName);
		}
	}

	public float MusicVolume
	{
		get { return _musicCategory.Volume; }
		set { _musicCategory.Volume = value; }
	}

	public float SoundVolume
	{
		get { return _soundCategory.Volume; }
		set { _soundCategory.Volume = value; }
	}

	public void SetMusicActive()
	{
		_musicController.gameObject.SetActive(!_musicController.gameObject.activeSelf);
	}

	/*
	 * 当玩家在使用语音的时候，将当前场景音量放最小
	 */
	private float _tempMusicVolume = 0;
	private float _tempSoundVolume = 0;
	public void StopVolumeWhenRecordVoice()
	{
		_tempMusicVolume = MusicVolume;
		_tempSoundVolume = SoundVolume;
		MusicVolume = 0;
		SoundVolume = 0;
	}
	
	/*
	 * 停止当前正在播放语音
	 */
	public void PlayVoiceWhenFinishRecord()
	{
		MusicVolume = _tempMusicVolume;
		SoundVolume = _tempSoundVolume;
	}
	
	public void SetMainAudioListernerActive( bool b )
	{
		/*
		AudioListener listener = LayerManager.Instance.GetMainAudioListener();
		if( listener != null )
		{
			listener.enabled = b;
		}
		*/
	}
}
