// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	SystemDataModel.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;

public class SystemDataModel {

	private static readonly SystemDataModel instance = new SystemDataModel();
	public static SystemDataModel Instance {
		get { return instance; }
	}

	//	System分类选项卡
	public readonly int systemBaseTabNum = 0;//2;

	private bool _musicToggle = true;
	public bool musicToggle {
		get { return _musicToggle; }
	}

	private bool _soundToggle = true;
	public bool soundToggle {
		get { return _soundToggle; }
	}

	private int _musicValue = 75;
	public int musicValue {
		get { return _musicValue; }
	}

	private int _soundValue = 75;
	public int soundValue {
		get { return _soundValue; }
	}

	public int _voiceValue = 75;
	public int voiceValue {
		get { return _voiceValue; }
	}

	public bool localToggle = true;
	public bool battleMapToggle = true;
	public bool trafficToggle = true;
    public bool energyConserving = true;

	public bool factionToggle = true;
	public bool worldToggle = true;
	public bool contingentToggle = true;
	public bool friendsToggle = true;
	public bool strangerToggle = true;

	public void SetToggleMusic(bool b) {
		_musicToggle = b;
		AudioManager.Instance.toggleMusic = b;
		PlayerPrefsExt.SetBool("_SystemMusicToggle", b);

		AudioManager.Instance.MusicVolume = _musicToggle? musicValue/100.0f : 0;
	}
	
	public void SetToggleSound(bool b) {
		_soundToggle = b;
		AudioManager.Instance.toggleMusic = b;
		PlayerPrefsExt.SetBool("_SystemSoundToggle", b);

		AudioManager.Instance.SoundVolume = _soundToggle? soundValue/100.0f : 0;
	}

	public void SetValueMusic(int v, bool save = true) {
		_musicValue = v;
		if (save)
			PlayerPrefsExt.SetLocalInt("_SystemMusicValue", v);

		AudioManager.Instance.MusicVolume = _musicToggle? v/100.0f : 0;
	}
	
	public void SetValueSound(int v, bool save = true) {
		_soundValue = v;
		if (save)
			PlayerPrefsExt.SetLocalInt("_SystemSoundValue", v);

		AudioManager.Instance.SoundVolume = _soundToggle? v/100.0f : 0;
	}
	
	public void SetValueVoice(int v, bool save = true) {
		_voiceValue = v;
		if (save)
			AudioManager.Instance.SoundVolume = v;

		PlayerPrefsExt.SetLocalInt("_SystemVoiceValue", v);
	}

	//	获取本地数据
	public void GetSystemDataFormLocal() {
		_musicToggle = PlayerPrefsExt.GetBool("_SystemMusicToggle", true);
		_soundToggle = PlayerPrefsExt.GetBool("_SystemSoundToggle", true);

		_musicValue = PlayerPrefsExt.GetLocalInt("_SystemMusicValue", 75);
		_soundValue = PlayerPrefsExt.GetLocalInt("_SystemSoundValue", 75);
		_voiceValue = PlayerPrefsExt.GetLocalInt("_SystemVoiceValue", 75);
		
		localToggle = PlayerPrefsExt.GetBool("_SystemLocalToggle", true);
		battleMapToggle = PlayerPrefsExt.GetBool("_SystemBattleMapToggle", true);
		trafficToggle = PlayerPrefsExt.GetBool("_SystemTrafficToggle", true);
        energyConserving = PlayerPrefsExt.GetBool("_SystemEnergyConserving", true);

		factionToggle = PlayerPrefsExt.GetBool("_SystemFactionToggle", true);
		worldToggle = PlayerPrefsExt.GetBool("_SystemWorldToggle", true);
		contingentToggle = PlayerPrefsExt.GetBool("_SystemContingentToggle", true);
		friendsToggle = PlayerPrefsExt.GetBool("_SystemFriendsToggle", true);
		strangerToggle = PlayerPrefsExt.GetBool("_SystemStrangerToggle", true);

		SetValueMusic(_musicValue, false);
		SetValueSound(_soundValue, false);
		SetValueVoice(_voiceValue, false);

        InitBrightness();
	}

    private bool _isAutoBrightness;
    private int _brightness;
    private void InitBrightness()
    {
        SystemTimeManager.Instance.OnIdleChange += OnIdleChange;

        _isAutoBrightness = BaoyugameSdk.isAutoBrightness();
        if(_isAutoBrightness)
        {
            BaoyugameSdk.stopAutoBrightness();
        }

        _brightness = BaoyugameSdk.getScreenBrightness();
    }

    private void RecoveryBrightness()
    {
        SystemTimeManager.Instance.OnIdleChange -= OnIdleChange;
        BaoyugameSdk.setBrightness(_brightness);
        if (_isAutoBrightness)
        {
            BaoyugameSdk.startAutoBrightness();
        }
    }

    private void OnIdleChange(bool isIdle)
    {
        if (isIdle)
        {
            if (energyConserving)
            {
                //TipManager.AddTip("****** 省电模式开启 ******");
                BaoyugameSdk.setBrightness(70);
            }
        }
        else
        {
            //TipManager.AddTip("关闭省电模式");
            BaoyugameSdk.setBrightness(_brightness);
        }
    }

    public void Dispose()
    {
        RecoveryBrightness();
    }
}
