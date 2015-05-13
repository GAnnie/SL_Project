
// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionController.cs
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
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.mission.data;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.scene.data;

public class MissionController : MonoBehaviour, IViewController {
	private MissionView _view;

	private List<TabBtnController> _tabBtnList = new List<TabBtnController>();
	private TabBtnController _lastTabBtn = null;
	private string[] _tabNames= {"当前任务", "可接任务"};

	public const string missionTypePath = "Prefabs/Module/MissionModule/MissionTypeCellView";
	private GameObject _missionTypePrefab = null;

	private bool _unToggledMain = false;

	private int _tabIndex = 0;

	private Mission _currentClickMission = null;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = this.gameObject.GetMissingComponent<MissionView>();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		EventDelegate.Set(_view.Button_UIButton.onClick, OnClickCloseBtn);
		//	已接任务面板
		EventDelegate.Set(_view.CurItemBgSprite_UIButton.onClick, OnClickCurItemBtn);
		EventDelegate.Set(_view.CurDropBtn_UIButton.onClick, OnClickDropBtn);
		EventDelegate.Set(_view.CurConveyBtn_UIButton.onClick, OnClickConveyBtn);

		//	可接任务面板
		EventDelegate.Set(_view.AceBtn_UIButton.onClick, OnClickAceBtn);

		//	Set Callback
		MissionDataModel.Instance.refreshUICallback += RefreshUICallback;
		MissionDataModel.Instance.dropCallback += DropMissionCallback;
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		MissionDataModel.Instance.refreshUICallback -= RefreshUICallback;
		MissionDataModel.Instance.dropCallback -= DropMissionCallback;
	}
	#endregion
	
	public void Open() {
		if (_view == null) {
			InitView();
			RegisterEvent();
			
			InitTabBtn();
		}

		if (_missionTypePrefab == null) {
			_missionTypePrefab = (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab(missionTypePath);
		}

		OnSelectRightTabBtn(MissionDataModel.Instance.missionTabNum);
	}
	
	private void InitTabBtn(){
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i = 0, len = _tabNames.Length; i < len; i++) {
			GameObject item = NGUITools.AddChild (_view.TabGrid_UIGrid.gameObject, tabBtnPrefab);
			
			TabBtnController tTabBtnController = item.GetMissingComponent<TabBtnController>();
			_tabBtnList.Add (tTabBtnController);
			
			tTabBtnController.InitItem (i, OnSelectRightTabBtn);
			tTabBtnController.SetBtnName(_tabNames[i]);
		}
	}
	
	private void OnSelectRightTabBtn(int tabIndex){
		_tabIndex = tabIndex;

		//	判断是否是相同的View T:return F:go on
		if (_lastTabBtn == _tabBtnList[tabIndex]) return;
		
		if(_lastTabBtn != null) {
			_lastTabBtn.SetSelected(false);
		}
		_lastTabBtn = _tabBtnList[tabIndex];
		_lastTabBtn.SetSelected(true);

		MissionDataModel.Instance.SetMissionTabNum(tabIndex);

		_changeTypeCellSelect = true;
		if (_lastRootTypeCellController != null) {
			_lastRootTypeCellController.Selected(false);
			_lastRootTypeCellController = null;
		}
		_lastSecondTypeCellController = null;
		
		_view.CurGroup_Transform.gameObject.SetActive(tabIndex == 0);
		_view.AceGroup_Transform.gameObject.SetActive(tabIndex == 1);

		//	是否已接任务State
		bool tIsExist = tabIndex == 0;

		//	================	判断是否需要显示右侧面版 Begin	================
		Transform tGameObjectView = tIsExist? _view.CurGroup_Transform : _view.AceGroup_Transform;
		_missionTypeMenuList = MissionDataModel.Instance.GetMainMissionMenuList(tIsExist);
		tGameObjectView.gameObject.SetActive(_missionTypeMenuList.Count > 0);
		//	================	判断是否需要显示右侧面版 End	================

		menuHierarchy = tIsExist? MissionDataModel.Instance.curMenuHierarchy : MissionDataModel.Instance.aceMenuHierarchy;

		ReSetTypeGrid();
	}

	//	重设TypeGrid
	private void ReSetTypeGrid() {
		if (MissionDataModel.Instance.GetAllMissionMenuList() == null) return;

		InitBuyTabView();
	}

	#region 一级菜单
	private List<MissionType> _missionTypeMenuList = null;
	private List<MissionTypeCellController> _mainTypeCellControllerList = new List<MissionTypeCellController> ();
	//	当前记录菜单选项
	public MissionDataModel.MissionMenuHierarchy menuHierarchy;
	private void InitBuyTabView() {
		List<MissionType> tMissionTypeMenuList = _missionTypeMenuList;
		MissionTypeCellController tMissionTypeCellController = null;

		for (int i = 0, len = _mainTypeCellControllerList.Count; i < len; i++) {
			_mainTypeCellControllerList[i].gameObject.SetActive(i < tMissionTypeMenuList.Count);
			if (i < tMissionTypeMenuList.Count) {
				MissionType tMissionType = tMissionTypeMenuList[i];

				MissionTypeCellController tTypeCellController = _mainTypeCellControllerList[i];
				tTypeCellController.name = tMissionType.id.ToString();
				tTypeCellController.SetMissionTypeData(tMissionType, OnTypeCellSelect);

				//	默认选择一级菜单
				if ((menuHierarchy.settinged && tMissionType.id == menuHierarchy.mainMenu)
				    || (/*i == len - 1 && */tMissionTypeCellController == null)) {
					tMissionTypeCellController = tTypeCellController;
				}
			}
		}

		for (int i = _mainTypeCellControllerList.Count, len = tMissionTypeMenuList.Count; i < len; i++) {
			MissionType tMissionType = tMissionTypeMenuList[i];
			
			GameObject go = NGUITools.AddChild(_view.TypeGrid_UIGrid.gameObject, _missionTypePrefab);
			go.name = tMissionType.id.ToString();
			
			MissionTypeCellController tTypeCellController = go.AddMissingComponent<MissionTypeCellController>();
			tTypeCellController.SetMissionTypeData(tMissionType, OnTypeCellSelect);

			_mainTypeCellControllerList.Add(tTypeCellController);

			if ((menuHierarchy.settinged && tMissionType.id == menuHierarchy.mainMenu)
			    || (/*i == len - 1 && */tMissionTypeCellController == null)) {
				tMissionTypeCellController = tTypeCellController;
			}
		}
		
		_view.TypeGrid_UIGrid.Reposition();
		
		if (tMissionTypeCellController != null) {
			OnTypeCellSelect(tMissionTypeCellController);
		} else {
			HideSubMenuItem();
		}
	}
	#endregion
	
	#region 菜单选中回调
	private MissionTypeCellController _lastRootTypeCellController;
	private MissionTypeCellController _lastSecondTypeCellController;
	private int _lastSubMenuID = 0;
	private bool _changeTypeCellSelect = false;

	private void OnTypeCellSelect(MissionTypeCellController typeCellController) {
		//	MainMenu Judge
		if (_changeTypeCellSelect) {
			_changeTypeCellSelect = false;
		} else {
			if (_lastRootTypeCellController == typeCellController) {
				_unToggledMain = !_unToggledMain;
				
				GameDebuger.OrangeDebugLog(_unToggledMain? "一级菜单 -> 折叠" : "一级菜单 -> 展开");
				
				if (_unToggledMain) {
					HideSubMenuItem();
					return;
				}
			} else {
				_unToggledMain = false;
			}
		}
		
		//	SubMenu Judge
		if (_lastSecondTypeCellController != null && _lastSubMenuID == _lastSecondTypeCellController.GetMissionType().id) {
			return;
		}
		
		MissionType tMenuType = typeCellController.GetMissionType();
		if (typeCellController.GetIsMainMenu()) {
			//	一级菜单id保存
			if (MissionDataModel.Instance.missionTabNum == 0) {
				MissionDataModel.Instance.curMenuHierarchy.mainMenu = tMenuType.id;
			} else {
				MissionDataModel.Instance.aceMenuHierarchy.mainMenu = tMenuType.id;
			}
			
			if (_lastRootTypeCellController != null) {
				_lastRootTypeCellController.Selected(false);
			}
			
			_lastRootTypeCellController = typeCellController;
			_lastRootTypeCellController.Selected (true);
			
//			if (!TradeDataModel.Instance.GetPlayerStallCenterDtoByMenuidDic().ContainsKey(tMenuType.id)) {
//				TradeDataModel.Instance.MenuMarket(tMenuType.id, delegate() {
//					ShowSubTypeCell(tMenuType.id);
//				});
//			} else {
				ShowSubTypeCell(tMenuType.id);
//			}
		} else {
			//	二级菜单id保存
			if (MissionDataModel.Instance.missionTabNum == 0) {
				MissionDataModel.Instance.curMenuHierarchy.subMenu = tMenuType.id;
				MissionDataModel.Instance.curMenuHierarchy.settinged = true;
			} else {
				MissionDataModel.Instance.aceMenuHierarchy.subMenu = tMenuType.id;
				MissionDataModel.Instance.aceMenuHierarchy.settinged = true;
			}
			
			if (_lastSecondTypeCellController != null) {
				_lastSecondTypeCellController.Selected(false);
			}
			
			_lastSecondTypeCellController = typeCellController;
			_lastSecondTypeCellController.Selected (true);

			//ShowItemListByType(menu.id);

			//	Get数据并
			_currentClickMission = _lastSecondTypeCellController.GetMission();

			/*
			//	是否重设数据
			if (_lastSecondTypeCellController != null && typeCellController.GetMissionType().id == _lastSecondTypeCellController.GetMissionType().id) {
				return;
			}
			*/

			//	设置到当前面板
			if (_tabIndex == 0) {
				ReSetCurMission();
			} else if (_tabIndex == 1) {
				ReSetAceMission();
			}
		}
	}
	#endregion
	
	#region 二级菜单
	private List<MissionTypeCellController> _subTypeCellControllerList = new List<MissionTypeCellController>();
	private void ShowSubTypeCell(int mainMenuId) {
		MissionTypeCellController typeCellPrefab = null;
		
		//	预处理 SubMenuId 的任务 Begin
		bool tIsExistState = _tabIndex == 0;
		List<Mission> __tSubMissionMenuList = MissionDataModel.Instance.GetSubdivisionMenuList(mainMenuId, tIsExistState);
		List<Mission> tSubMissionMenuList = new List<Mission>();

		if (tIsExistState) {
			//	TADO	已接任务
			tSubMissionMenuList = __tSubMissionMenuList;
		} else {
			for (int i = 0, len = __tSubMissionMenuList.Count; i < len; i++) {
				Mission tMission = __tSubMissionMenuList[i];
				//	TADO	可接任务
				if (MissionDataModel.Instance.IsMainOrExtension(tMission)) {
					//	主线\支线
					tSubMissionMenuList.Add(tMission);
				} else {
					//	日常任务
					bool tExistDailyMission = false;
					for (int j = 0, tlen = tSubMissionMenuList.Count; j < tlen; j++) {
						if (tSubMissionMenuList[j].type == tMission.type) {
							tExistDailyMission = true;
							break;
						}
						continue;
					}

					if (!tExistDailyMission) {
						tSubMissionMenuList.Add(tMission);
					}
				}
			}
		}
		//	预处理 SubMenuId 的任务 End

		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(i < tSubMissionMenuList.Count);
			if (i < tSubMissionMenuList.Count) {
				Mission tMission = tSubMissionMenuList[i];
				
				MissionTypeCellController tTypeCellController = _subTypeCellControllerList[i];
				tTypeCellController.name = string.Format("{0}_{1}", tMission.type, tMission.id.ToString().PadLeft(4, '0'));
				tTypeCellController.SetMissionTypeData(tMission, OnTypeCellSelect);
				
				//	默认选择二级菜单
				if (menuHierarchy.settinged && tMission.id == menuHierarchy.subMenu) {
					typeCellPrefab = tTypeCellController;
				} else if (typeCellPrefab == null) {
					typeCellPrefab = tTypeCellController;
				}
			}
		}
		
		for (int i = _subTypeCellControllerList.Count, len = tSubMissionMenuList.Count; i < len; i++) {
			Mission tMission = tSubMissionMenuList[i];
			
			GameObject go = NGUITools.AddChild(_view.TypeGrid_UIGrid.gameObject, _missionTypePrefab);
			go.name = string.Format("{0}_{1}", tMission.type, tMission.id.ToString().PadLeft(4, '0'));
			
			MissionTypeCellController tTypeCellController = go.AddMissingComponent<MissionTypeCellController>();
			tTypeCellController.SetMissionTypeData(tMission, OnTypeCellSelect);
			
			_subTypeCellControllerList.Add(tTypeCellController);
			
			//	默认选择二级菜单
			if (menuHierarchy.settinged && tMission.id == menuHierarchy.subMenu) {
				typeCellPrefab = tTypeCellController;
			} else if (typeCellPrefab == null) {
				typeCellPrefab = tTypeCellController;
			}
		}
		
		_view.TypeGrid_UIGrid.Reposition();
		_view.ScrollView_UIScrollView.ResetPosition();
		
		if (typeCellPrefab != null) {
			OnTypeCellSelect(typeCellPrefab);
		}

		//	当没有二级菜单的处理方式
		if (tSubMissionMenuList.Count <= 0) {
			//ShowItemListByType (mainMenuId);
			if (MissionDataModel.Instance.missionTabNum == 0) {
				MissionDataModel.Instance.curMenuHierarchy.subMenu = mainMenuId;
			} else {
				MissionDataModel.Instance.aceMenuHierarchy.subMenu = mainMenuId;
			}
		}
	}
	#endregion
	
	#region 隐藏二级菜单
	private void HideSubMenuItem() {
		//	_unToggledMain
		for (int i = 0, len = _subTypeCellControllerList.Count; i < len; i++) {
			_subTypeCellControllerList[i].gameObject.SetActive(false);
		}
		_view.TypeGrid_UIGrid.Reposition();
		_view.ScrollView_UIScrollView.ResetPosition();
	}
	#endregion

	#region 已接任务重设	======================
	private void ReSetCurMission() {
		Mission mission = _currentClickMission;

		_view.CurIconSprite_UISprite.spriteName = "10017";
		_view.CurTypeLabel_UILabel.text = MissionDataModel.Instance.GetMissionTitleName(mission, true, false);
		_view.CurTargetLabel_UILabel.text = MissionDataModel.Instance.GetCurTargetContent(mission, false);
		_view.CurSubmitNPCLabel_UILabel.text = MissionDataModel.Instance.GetMisssionNpcName(mission, true, true);
		_view.CurDescriptionLabel_UILabel.text = MissionDataModel.Instance.GetCurDescriptionContent(mission);
	}
	#endregion

	#region 可接任务重设	======================
	private void ReSetAceMission() {
		Mission mission = _currentClickMission;

		_view.AceTypeLabel_UILabel.text = MissionDataModel.Instance.GetMissionTitleName(mission, false, false);
		_view.AceAcceptNPCLabel_UILabel.text = MissionDataModel.Instance.GetMisssionNpcName(mission, false, true);

		_view.AceGradeLabel_UILabel.text = mission.missionType.functionOpen == null?
			"" : string.Format("[6f3e1a]等级≥{0}{1}{2}[-]", mission.missionType.functionOpen.grade,
			                  mission.missionType.functionOpen.vip? "需要VIP" : "",
			                  string.IsNullOrEmpty(mission.missionType.appendDescription)?
			                  "" : string.Format(" {0}", mission.missionType.appendDescription));
		_view.AceDescriptionLabel_UILabel.text = MissionDataModel.Instance.GetAceMissionMsg(mission);
	}
	#endregion

	//	关闭任务面板Btn
	private void OnClickCloseBtn() {
		ProxyMissionModule.Close ();
	}

	//	Icon Sprite Button
	private void OnClickCurItemBtn() {
		GameDebuger.OrangeDebugLog("TADO:ItemIcon......");
		ProxyItemTipsModule.Open(10017, _view.CurItemBgSprite_UIButton.gameObject);
	}

	//	放弃选中任务Btn
	private void OnClickDropBtn() {
		if (MissionDataModel.Instance.IsMainOrExtension(_currentClickMission)) {
			TipManager.AddTip(string.Format("不能放弃{0}任务", _currentClickMission.missionType.name));
		} else {
			MissionDataModel.Instance.DropMission(_currentClickMission.id);
			OnClickCloseBtn();
		}
	}

	//	传送按钮回调
	private void OnConvey(Mission mission, bool b) {
		MissionDataModel.Instance.FindToMissionNpcByMission(mission, b);
		OnClickCloseBtn();
	}

	//	传送至任务需要场景Btn
	private void OnClickConveyBtn() {
		OnConvey(_currentClickMission, true);
	}

	//	接受任务Btn
	private void OnClickAceBtn() {
		OnConvey(_currentClickMission, false);
	}

	//	刷新任务面板回调
	private void RefreshUICallback() {
		GameDebuger.OrangeDebugLog("TADO:刷新任务面板......");
		ReSetTypeGrid();
	}

	//	放弃任务回调
	private void DropMissionCallback(AcceptGiftsDto acceptGiftsDto) {
		// 存在任务道具的需要清除任务道具数据
		GameDebuger.OrangeDebugLog("TADO:存在任务道具的需要清除任务道具数据 -> 赠送、收集......");
		//	TADO
	}
}