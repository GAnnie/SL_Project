// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	SystemSettingController.cs
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

public class SystemSettingController : MonoBehaviour, IViewController {
	private SystemSettingView _view;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<SystemSettingView>();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		//	音乐
		EventDelegate.Add(_view.MusicCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.SetToggleMusic(_view.MusicCheckbox_UIToggle.value);
		});
		//	音效
		EventDelegate.Add(_view.SoundCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.SetToggleSound(_view.SoundCheckbox_UIToggle.value);
			
		});
		//	地理位置
		EventDelegate.Add(_view.LocationCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.localToggle = _view.LocationCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemLocalToggle", _view.LocationCheckbox_UIToggle.value);
		});
		//	3D镜头
		EventDelegate.Add(_view.LensCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.battleMapToggle = _view.LensCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemLensToggle", _view.LensCheckbox_UIToggle.value);
		});
		//	省流量模式
		EventDelegate.Add(_view.TrafficCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.trafficToggle = _view.TrafficCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemTrafficToggle", _view.TrafficCheckbox_UIToggle.value);
		});
        //	省电模式
        EventDelegate.Add(_view.EnergyConservingCheckbox_UIToggle.onChange, () =>
        {
            SystemDataModel.Instance.energyConserving = _view.EnergyConservingCheckbox_UIToggle.value;
            PlayerPrefsExt.SetBool("_SystemEnergyConserving", _view.EnergyConservingCheckbox_UIToggle.value);
        });
		//	帮派语音
		EventDelegate.Add(_view.FactionCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.factionToggle = _view.FactionCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemFactionToggle", _view.FactionCheckbox_UIToggle.value);
		});
		//	世界语音
		EventDelegate.Add(_view.WorldCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.worldToggle = _view.WorldCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemWorldToggle", _view.WorldCheckbox_UIToggle.value);
		});
		//	队伍语音
		EventDelegate.Add(_view.ContingentCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.contingentToggle = _view.ContingentCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemContingentToggle", _view.ContingentCheckbox_UIToggle.value);
		});
		//	好友语音
		EventDelegate.Add(_view.FriendsCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.friendsToggle = _view.FriendsCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemFriendsToggle", _view.FriendsCheckbox_UIToggle.value);
		});
		//	陌生人
		EventDelegate.Add(_view.StrangerCheckbox_UIToggle.onChange, () => {
			SystemDataModel.Instance.strangerToggle = _view.StrangerCheckbox_UIToggle.value;
			PlayerPrefsExt.SetBool("_SystemStrangerToggle", _view.StrangerCheckbox_UIToggle.value);
		});

		//	音乐Slider
		EventDelegate.Add(_view.MusicSlider_UISlider.onChange, () => {
			OnMusicCallback(_view.MusicSlider_UISlider.value);
		});
		_view.MusicSlider_UISlider.onDragFinished = () => {
			OnMusicCallback(_view.MusicSlider_UISlider.value, true);
		};
		//	音效Slider
		EventDelegate.Add(_view.SoundSlider_UISlider.onChange, () => {
			OnSoundCallback(_view.SoundSlider_UISlider.value);
		});
		_view.SoundSlider_UISlider.onDragFinished = () => {
			OnSoundCallback(_view.SoundSlider_UISlider.value, true);
		};
		//	语音Slider
		EventDelegate.Add(_view.VoiceSlider_UISlider.onChange, () => {
			int tVoiceValue = Mathf.CeilToInt(_view.VoiceSlider_UISlider.value*100);
			_view.VoiceValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tVoiceValue);
		});
		_view.VoiceSlider_UISlider.onDragFinished = () => {
			int tVoiceValue = Mathf.CeilToInt(_view.VoiceSlider_UISlider.value*100);
			SystemDataModel.Instance.SetValueVoice(tVoiceValue);
			_view.VoiceValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tVoiceValue);
		};
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		
	}
	#endregion
	
	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();

			//	系统设置
			//SystemDataModel.Instance.GetSystemDataFormLocal();
		}

		SetToggleAndValue();
	}
	
	private void OnMusicCallback(float f, bool setValue = false) {
		int tMusicValue = Mathf.CeilToInt(f*100);
		if (setValue)
			SystemDataModel.Instance.SetValueMusic(tMusicValue);
		_view.MusicValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tMusicValue);
	}

	private void OnSoundCallback(float f, bool setValue = false) {
		int tSoundValue = Mathf.CeilToInt(f*100);
		if (setValue)
			SystemDataModel.Instance.SetValueSound(tSoundValue);
		_view.SoundValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tSoundValue);
	}

	private void SetToggleAndValue() {
		SystemDataModel tData = SystemDataModel.Instance;

		_view.MusicCheckbox_UIToggle.value = tData.musicToggle;
		_view.SoundCheckbox_UIToggle.value = tData.soundToggle;

		_view.LocationCheckbox_UIToggle.value = tData.localToggle;
		_view.LensCheckbox_UIToggle.value = tData.battleMapToggle;
		_view.TrafficCheckbox_UIToggle.value = tData.trafficToggle;
        _view.EnergyConservingCheckbox_UIToggle.value = tData.energyConserving;

		_view.FactionCheckbox_UIToggle.value = tData.factionToggle;
		_view.WorldCheckbox_UIToggle.value = tData.worldToggle;
		_view.ContingentCheckbox_UIToggle.value = tData.contingentToggle;
		_view.FriendsCheckbox_UIToggle.value = tData.friendsToggle;
		_view.StrangerCheckbox_UIToggle.value = tData.strangerToggle;

		_view.MusicValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tData.musicValue);
		_view.MusicSlider_UISlider.value = tData.musicValue/100.0f;

		_view.SoundValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tData.soundValue);
		_view.SoundSlider_UISlider.value = tData.soundValue/100.0f;
		
		_view.VoiceValue_UILabel.text = string.Format("{0}{1}", "[6f3e1a]", tData.voiceValue);
		_view.VoiceSlider_UISlider.value = tData.voiceValue/100.0f;
	}
}
