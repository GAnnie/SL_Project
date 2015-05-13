// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	TypeCellPrefabController.cs
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
using com.nucleus.h1.logic.whole.modules.trade.data;
using System;

public class TypeCellPrefabController : MonoBehaviour,IViewController {

	private TypeCellPrefabView _view;
	
	private TradeMenu _tradeMenu;
	public event Action<TypeCellPrefabController> _OnTypeItemSelectCallback;
	
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
	
	public void Selected(bool select) {
		if (_tradeMenu.parentId == 0) {
			_view.TypeBgSprite_UISprite.spriteName = select? "green-little-bone" : "small-bone-under-lines";
		} else {
			_view.TypeSelectSprite_UISprite.gameObject.SetActive (select);
		}
	}
	
	public TradeMenu GetTradeMenu() {
		return _tradeMenu;
	}

	/// <summary>
	/// Starts the controller.是否适用
	/// </summary>
	/// <param name="tradeMenu">Trade menu.</param>
	/// <param name="OnTypeItemSelect">On type item select.</param>
	/// <param name="suitableSta">If set to <c>true</c> suitable sta.</param>
	public void StartController(TradeMenu tradeMenu, Action<TypeCellPrefabController> OnTypeItemSelect, bool suitableSta = false) {
		_tradeMenu = tradeMenu;
		_OnTypeItemSelectCallback = OnTypeItemSelect;

		InitView ();
		RegisterEvent ();

		bool tIsMainMenu = _tradeMenu.parentId == 0;

		_view.TypeBgSprite_UISprite.alpha = tIsMainMenu? 1 : 0;
		_view.SubSprite_Transform.gameObject.SetActive(!tIsMainMenu);
		
		_view.TypeNameLabel_UILabel.color = Color.white;
		
		_view.TypeNameLabel_UILabel.effectStyle = tIsMainMenu? UILabel.Effect.Outline8 : UILabel.Effect.None;
		_view.TypeNameLabel_UILabel.text = tIsMainMenu?
			string.Format("[fff9e3]{0}[-]", _tradeMenu.name) :
				string.Format("[6f3e1a]{0}[-]{1}", _tradeMenu.name, suitableSta? "[2beb54]（适用）[-]" : "");
		
		_view.TypeSelectSprite_UISprite.gameObject.SetActive(false);
		EventDelegate.Set (_view.TypeCellPrefabView_UIButton.onClick, OnBtnCellClick);
	}
	
	private void OnBtnCellClick() {
		if (_OnTypeItemSelectCallback != null) {
			_OnTypeItemSelectCallback(this);
		}
	}
}
