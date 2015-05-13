// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionTypeCellController.cs
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
using com.nucleus.h1.logic.core.modules.mission.data;

public class MissionTypeCellController : MonoBehaviour {

	private TypeCellPrefabView _view = null;
	
	private MissionType _missionType = null;
	private Mission _mission = null;
	private bool _isMainMenu = false;

	public System.Action<MissionTypeCellController> _OnTypeItemSelectCallback = null;
	
	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<TypeCellPrefabView>();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView后调用
	/// </summary>
	public void RegisterEvent() {
		
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		
	}
	#endregion

	//	===============================	Get	=============================
	public MissionType GetMissionType() {
		return _isMainMenu? _missionType : _mission.missionType;
	}

	public Mission GetMission() {
		return _mission;
	}
	
	public bool GetIsMainMenu() {
		return _isMainMenu;
	}
	
	//	===============================	Set	=============================
	public void Selected(bool select) {
		if (_isMainMenu) {
			_view.TypeBgSprite_UISprite.spriteName = select? "green-little-bone" : "small-bone-under-lines";
		} else {
			_view.TypeSelectSprite_UISprite.gameObject.SetActive (select);
		}
	}

	/// <summary>
	/// 一级菜单 -- Sets the mission type data.
	/// </summary>
	/// <param name="missionType">Mission type.</param>
	/// <param name="onTypeItemSelect">On type item select.</param>
	public void SetMissionTypeData(MissionType missionType, System.Action<MissionTypeCellController> onTypeItemSelect) {
		_missionType = missionType;
		_OnTypeItemSelectCallback = onTypeItemSelect;

		StartController();
	}

	/// <summary>
	/// 二级菜单 -- Sets the mission type data.
	/// </summary>
	/// <param name="mission">Mission.</param>
	/// <param name="onTypeItemSelect">On type item select.</param>
	public void SetMissionTypeData(Mission mission, System.Action<MissionTypeCellController> onTypeItemSelect) {
		_mission = mission;
		_OnTypeItemSelectCallback = onTypeItemSelect;

		StartController();
	}

	private void StartController() {
		_isMainMenu = _missionType != null;
		
		InitView ();
		RegisterEvent ();

		_view.TypeBgSprite_UISprite.alpha = _isMainMenu? 1 : 0;
		_view.SubSprite_Transform.gameObject.SetActive(!_isMainMenu);
		
		_view.TypeNameLabel_UILabel.color = Color.white;
		
		_view.TypeNameLabel_UILabel.effectStyle = _isMainMenu? UILabel.Effect.Outline8 : UILabel.Effect.None;
		//	是否主菜单 -> 是否主线\支线
		_view.TypeNameLabel_UILabel.text = _isMainMenu?
			string.Format("[fff9e3]{0}[-]", _missionType.id > 2? "日常任务" : _missionType.name)
			: string.Format("[6f3e1a]{0}[-]", MissionDataModel.Instance.IsMainOrExtension(_mission)? _mission.name : _mission.missionType.name);
		
		_view.TypeSelectSprite_UISprite.gameObject.SetActive(false);
		EventDelegate.Set(_view.TypeCellPrefabView_UIButton.onClick, OnBtnCellClick);
	}
	
	private void OnBtnCellClick() {
		if (_OnTypeItemSelectCallback != null) {
			_OnTypeItemSelectCallback(this);
		}
	}
}
