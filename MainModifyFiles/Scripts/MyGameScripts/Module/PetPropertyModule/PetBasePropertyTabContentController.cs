using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;

public class PetBasePropertyTabContentController : MonoBehaviour
{
	private PetPropertyMainView _view;
	List<PropertyItemController> _bpItemList;
	List<PropertyItemController> _apItemList;
	private PropertyItemController _ppItemCom;
	private int[] _lastApAddPoint = new int[5]{0,0,0,0,0};	//记录最近一次加点配置
	private PetPropertyInfo _petInfo;

	public void InitView (PetPropertyMainView view)
	{
		_view = view;
		GameObject apItemGrid = this.transform.Find ("ApItemGrid").gameObject;
		GameObject bpItemGrid = this.transform.Find ("BpItemGrid").gameObject;
		_apItemList = new List<PropertyItemController> (5);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (apItemGrid, itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.OnAdd += OnApItemAdd;
			controller.OnMinus += OnApItemMinus;
			controller.SetBgWidth(186);
			_apItemList.Add (controller);
		}
		
		_apItemList [0].InitItem ("体质", 0, false,24);
		_apItemList [1].InitItem ("魔力", 0, false,25);
		_apItemList [2].InitItem ("力量", 0, false,26);
		_apItemList [3].InitItem ("耐力", 0, false,27);
		_apItemList [4].InitItem ("敏捷", 0, false,28);
		
		GameObject ppItem = NGUITools.AddChild (apItemGrid, itemPrefab);
		_ppItemCom = ppItem.GetMissingComponent<PropertyItemController> ();
		_ppItemCom.InitSimpleItem ("潜力", 0,12);
		_ppItemCom.SetBgWidth(186);
		
		_bpItemList = new List<PropertyItemController> (6);
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (bpItemGrid, itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.SetEditable (false);
			controller.SetBgWidth(186);
			_bpItemList.Add (controller);
		}
		
		_bpItemList [0].InitItem ("气血", 0, true,1);
		_bpItemList [1].InitItem ("魔法", 0, true,2);
		_bpItemList [2].InitItem ("攻击", 0, false,3);
		_bpItemList [3].InitItem ("防御", 0, false,4);
		_bpItemList [4].InitItem ("速度", 0, false,5);
		_bpItemList [5].InitItem ("灵力", 0, false,6);

		RegisterEvent ();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set (_view.apItemOpBtn.onClick, ConfirmPlayerAp);
		EventDelegate.Set (_view.ppInfoBtn.onClick, OpenPPDetailView);
	}

	public void UpdateViewInfo (PetPropertyInfo petInfo)
	{
		_petInfo = petInfo;
		
		_ppItemCom.SetValue (_petInfo.petDto.potential);
		UpdateApItemInfo (_petInfo.petDto.aptitudeProperties.constitution,
		                 _petInfo.petDto.aptitudeProperties.intelligent,
		                 _petInfo.petDto.aptitudeProperties.strength,
		                 _petInfo.petDto.aptitudeProperties.stamina,
		                 _petInfo.petDto.aptitudeProperties.dexterity,
		                 true);
		UpdateBpItemInfo (_petInfo.hp, _petInfo.mp, _petInfo.attack, _petInfo.defense, _petInfo.speed, _petInfo.magic, true);
		UpdateApItemOpBtnState ();
	}

	public void CleanUpViewInfo(){
		_petInfo = null;
		_ppItemCom.SetValue(0);
		UpdateApItemInfo(0,0,0,0,0,true);
		UpdateBpItemInfo(0,0,0,0,0,0,true);
		UpdateApItemOpBtnState();
	}

	#region 设置自动配点方案
	private PPDetailViewController _ppDetailController;

	private void OpenPPDetailView ()
	{
		if(_petInfo == null) return;
		if (_petInfo.petDto.onUpgradeAptitudeProperties != null) {
			_ppDetailController = ProxyPlayerPropertyModule.OpenPPDetailView (new Vector3 (346f, 20f, 0f));
			_ppDetailController.Open (_petInfo.petDto.onUpgradeAptitudeProperties);
			_ppDetailController.OnSave += OnSaveAutoApDistribution;
			_ppDetailController.OnCancel += OnCancelAutoApDistribution;
		}
	}
	
	private void OnSaveAutoApDistribution ()
	{
		ServiceRequestAction.requestServer (PetService.configAutoAssignAptitude (
			_petInfo.petDto.id,
			_ppDetailController.apItemList [0].GetValue (),
			_ppDetailController.apItemList [1].GetValue (),
			_ppDetailController.apItemList [2].GetValue (),
			_ppDetailController.apItemList [3].GetValue (),
			_ppDetailController.apItemList [4].GetValue ()
		), "保存宠物自动分配资质点数", (e) => {
			_ppDetailController.UpdateApInfoDto ();
			_ppDetailController.CloseView ();
			
			PlayerModel.AutoDistrubuteApPoint (_petInfo.petDto);
			PetModel.CalculatePetBp (_petInfo);
			UpdateViewInfo (_petInfo);
			TipManager.AddTip ("自动加点方案设置成功，相应的加点方案会在升级时自动分配");
		});
	}
	
	private void OnCancelAutoApDistribution ()
	{
		ServiceRequestAction.requestServer (PetService.cancelAutoAssignAptitude (_petInfo.petDto.id), "重置宠物自动分配资质点数", (e) => {
			for (int i=0; i<_ppDetailController.apItemList.Count; ++i) {
				_ppDetailController.apItemList [i].SetValue (0, true);
			}
			_ppDetailController.UpdateApInfoDto ();
		});
	}
	#endregion

	#region 更改资质属性点
	private void ConfirmPlayerAp ()
	{
		if (_ppItemCom.GetValue () != _petInfo.petDto.potential) {
			
			ServiceRequestAction.requestServer (PetService.increasePetAptitude (_petInfo.petDto.id,
		                                                                                        	_lastApAddPoint [0],
		                                                                                          	_lastApAddPoint [1],
		                                                                                           	_lastApAddPoint [2],
		                                                                                           	_lastApAddPoint [3],
		                                                                                           	_lastApAddPoint [4]),
			    "更改宠物资质属性", (e) => {
				CleanUpAddPointList ();
				PetModel.Instance.UpdatePetAp (_petInfo,
											   _ppItemCom.GetValue (),
		                                       _apItemList [0].GetValue (),
		                                       _apItemList [1].GetValue (),
		                                       _apItemList [2].GetValue (),
		                                       _apItemList [3].GetValue (),
		                                       _apItemList [4].GetValue ());
				UpdateViewInfo (_petInfo);
			});
		}
	}
	#endregion

	private void OnApItemAdd (PropertyItemController item)
	{
		int index = _apItemList.IndexOf (item);
		int val = item.GetValue () + 1;
		if (_ppItemCom.GetValue () - 1 >= 0) {
			item.SetValue (val, false);
			_ppItemCom.SetValue (_ppItemCom.GetValue () - 1);
			_lastApAddPoint [index] += 1;
			CalculatePreviewBattleProperties ();
		}
	}

	private void OnApItemMinus (PropertyItemController item)
	{
		int index = _apItemList.IndexOf (item);
		int point = _ppItemCom.GetValue () + 1;
		int val = item.GetValue () - 1;
		if (_lastApAddPoint [index] - 1 >= 0) {
			item.SetValue (val, false);
			_ppItemCom.SetValue (point);
			_lastApAddPoint [index] -= 1;
			CalculatePreviewBattleProperties ();
		}
	}

	private void UpdateApItemOpBtnState ()
	{
		if (_petInfo != null && _petInfo.petDto.potential > 0) {
			for (int i=0; i<_apItemList.Count; ++i) {
				_apItemList [i].SetEditable (true);
			}
			_view.apItemOpBtn.gameObject.SetActive (true);
			_view.changeNameBtn.gameObject.SetActive (false);
		} else {
			_view.apItemOpBtn.gameObject.SetActive (false);
			_view.changeNameBtn.gameObject.SetActive (true);
			for (int i=0; i<_apItemList.Count; ++i) {
				_apItemList [i].SetEditable (false);
			}
		}
	}

	private void UpdateBpItemInfo (int hp, int mp, int attack, int defense, int speed, int magic, bool needReset)
	{
		_bpItemList [0].SetValue (hp, needReset);
		_bpItemList [1].SetValue (mp, needReset);
		_bpItemList [2].SetValue (attack, needReset);
		_bpItemList [3].SetValue (defense, needReset);
		_bpItemList [4].SetValue (speed, needReset);
		_bpItemList [5].SetValue (magic, needReset);
	}

	private void UpdateApItemInfo (int constitution, int intelligent, int strength, int stamina, int dexterity, bool needReset)
	{
		_apItemList [0].SetValue (constitution, needReset);
		_apItemList [1].SetValue (intelligent, needReset);
		_apItemList [2].SetValue (strength, needReset);
		_apItemList [3].SetValue (stamina, needReset);
		_apItemList [4].SetValue (dexterity, needReset);
	}

	private void CalculatePreviewBattleProperties ()
	{
		float hp = _petInfo.hp;
		float attack = _petInfo.attack;
		float defense = _petInfo.defense;
		float speed = _petInfo.speed;
		float magic = _petInfo.magic;
		
		for (int index=0; index<_lastApAddPoint.Length; ++index) {
			int addPoint = _lastApAddPoint [index];
			if (addPoint > 0) {
				AptitudePropertyInfo apDataInfo = DataCache.getDtoByCls<AptitudePropertyInfo> (index + 1);
				
				hp += apDataInfo.hp * addPoint;
				attack += apDataInfo.attack * addPoint;
				defense += apDataInfo.defense * addPoint;
				speed += apDataInfo.speed * addPoint;
				magic += apDataInfo.magic * addPoint;
			}
		}
		
		UpdateBpItemInfo (Mathf.FloorToInt (hp),
		                  _petInfo.mp,
		                  Mathf.FloorToInt (attack),
		                  Mathf.FloorToInt (defense),
		                  Mathf.FloorToInt (speed),
		                  Mathf.FloorToInt (magic),
		                  false);
	}

	private int GetApAddPointTotalCount ()
	{
		int result = 0;
		for (int i=0; i<_lastApAddPoint.Length; ++i) {
			result += _lastApAddPoint [i];
		}
		
		return result;
	}

	private void CleanUpAddPointList ()
	{
		for (int i=0; i<_lastApAddPoint.Length; ++i) {
			_lastApAddPoint [i] = 0;
		}
	}
}
