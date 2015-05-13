using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.formation.data;

public class CrewMatrixTabContentController : MonoBehaviour {

	private CrewMainView _view;

	private const int FORMATIONCASE_TOGGLE_GROUPID = 50;
	private List<BaseToggleController> _formationCaseToggleList;

	private List<LayoutItemController> _acquiredLayoutItemList;
	private List<LayoutItemController> _moreLayoutItemList;
	private GameObject _moreLayoutItemPanel;
	private const string LayoutItemWidgetName = "Prefabs/Module/PlayerTeamModule/LayoutItemWidget";

	private const string FormationItemWidgetName = "Prefabs/Module/CrewModule/FormationItemWidget";
	private FormationItemSlotController _playerFormationItem;
	private List<FormationItemSlotController> _crewFormationItemList;


	public void InitView (CrewMainViewController crewMainViewController)
	{
		_view = crewMainViewController._view;

		EventDelegate.Set (_view.moreMatrixInfoBtn.onClick,OnClickMoreMatrixInfo);

		GameObject uiPrefab;
		//阵法选择列表初始化
		List<Formation> allFormationList = DataCache.getArrayByCls<Formation>();
		if(allFormationList != null && allFormationList.Count > 0){
			List<int> acquiredIdList= PlayerModel.Instance.GetAcquiredFormationIdList();
			uiPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (LayoutItemWidgetName) as GameObject;
			_acquiredLayoutItemList = new List<LayoutItemController> (acquiredIdList.Count);
			_moreLayoutItemList = new List<LayoutItemController> (allFormationList.Count - acquiredIdList.Count);
			
			for(int i=0;i<allFormationList.Count;++i){
				Formation formation = allFormationList[i];
				if(acquiredIdList.Contains(formation.id)){
					GameObject item = NGUITools.AddChild (_view.acquiredLayoutItemGrid.gameObject, uiPrefab);
					LayoutItemController com = item.GetMissingComponent<LayoutItemController> ();
					com.InitItem (formation,OnSelectAcquiredLayoutItem);
					_acquiredLayoutItemList.Add (com);
				}else{
					GameObject item = NGUITools.AddChild (_view.moreLayoutItemGrid.gameObject, uiPrefab);
					LayoutItemController com = item.GetMissingComponent<LayoutItemController> ();
					com.InitItem (formation,OnSelectMoreLayoutItem);
					_moreLayoutItemList.Add (com);
				}
			}
			_view.acquiredMatrixListWidget.bottomAnchor.absolute = -(45 + _acquiredLayoutItemList.Count * 45);
		}
		_moreLayoutItemPanel =_view.moreLayoutItemGrid.transform.parent.gameObject;
		_moreLayoutItemPanel.SetActive(false);

		//初始化布阵方案按钮
		uiPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TOGGLE_WIDGET) as GameObject;
		_formationCaseToggleList = new List<BaseToggleController>(4);
		for(int i=0;i<4;++i){
			GameObject item;
			if(i == 0)
				item = NGUITools.AddChild(_view.defConfigToggleGrid.gameObject, uiPrefab);
			else
				item = NGUITools.AddChild (_view.atkConfigToggleGrid.gameObject, uiPrefab);
			
			BaseToggleController com = item.GetMissingComponent<BaseToggleController> ();
			com.InitItem (i,OnSelectFormationCaseToggle);
			_formationCaseToggleList.Add(com);
			UpdateFormationCaseToggleInfo(i);
		}
		_formationCaseToggleList[0].SetSelected(true);
		_view.matrixSelectedEffect.SetActive(false);

		//初始化阵法槽
		uiPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(FormationItemWidgetName) as GameObject;
		_crewFormationItemList = new List<FormationItemSlotController>(4);
		for(int i=0;i<5;++i){
			GameObject item = NGUITools.AddChild(_view.matrixItemPanel,uiPrefab);
			FormationItemSlotController com = item.GetMissingComponent<FormationItemSlotController>();
			if(i == 0){
				com.InitItem(CrewMainViewController.PLAYERITEM_INDEX,null,null);
				_playerFormationItem = com;
				_playerFormationItem.SetIcon("10017");
				_playerFormationItem.SetPosLbl(1);
			}
			else
			{
				com.InitItem(i-1,crewMainViewController.OnDragStartCrewInfoItem,crewMainViewController.OnDragEndCrewInfoItem);
				_crewFormationItemList.Add(com);
			}
		}

		OnSelectFormationCaseToggle(PlayerModel.Instance.ActiveFormationCaseIndex);
	}

	public void UpdateFormationCaseToggleInfo(int caseIndex){
		int formationId = PlayerModel.Instance.GetFormationCaseId(caseIndex);
		Formation formation = DataCache.getDtoByCls<Formation>(formationId);
		_formationCaseToggleList[caseIndex].SetLabel(formation.name);
	}

	private BaseToggleController _lastSelectFormationCaseToggle;
	public void OnSelectFormationCaseToggle(int index){
		if(index == PlayerModel.Instance.CurrentSelectFormationCaseIndex){
			UpdateFormationCaseInfo(index);
		}else{
			//更改阵型方案
			CrewModel.Instance.ChangeFormationCase(index);
		}
	}

	private void UpdateFormationCaseInfo(int caseIndex){
		if(caseIndex != PlayerModel.DEFENSE_FORMATION_INDEX){
			if(_lastSelectFormationCaseToggle != null)
				_lastSelectFormationCaseToggle.SetSelected(false);
			
			_lastSelectFormationCaseToggle = _formationCaseToggleList[caseIndex];
			_formationCaseToggleList[caseIndex].SetSelected(true);
			_view.arrowSprite.cachedTransform.localPosition = new Vector3(-415f,-3f,0f);
		}else
			_view.arrowSprite.cachedTransform.localPosition = new Vector3(140f,-3f,0f);
		
		int formationId = PlayerModel.Instance.GetFormationCaseId(caseIndex);
		for(int i=0;i<_acquiredLayoutItemList.Count;++i){
			if(formationId == _acquiredLayoutItemList[i].formation.id){
				OnSelectAcquiredLayoutItem(_acquiredLayoutItemList[i]);
				break;
			}
		}
	}

	private LayoutItemController _lastSelectAcquireLayoutItem;
	private Formation _curSelectFormation;
	private void OnSelectAcquiredLayoutItem (LayoutItemController com)
	{
		if(_lastSelectAcquireLayoutItem != null)
			_lastSelectAcquireLayoutItem.SetSelected(false);
		
		_lastSelectAcquireLayoutItem = com;
		com.SetSelected(true);
		_curSelectFormation = com.formation;

		if(com.formation.id != PlayerModel.Instance.GetFormationCaseId(PlayerModel.Instance.CurrentSelectFormationCaseIndex)){
			CrewModel.Instance.ChangeFormation(com.formation.id,()=>{
				UpdateFormationCaseToggleInfo(PlayerModel.Instance.CurrentSelectFormationCaseIndex);
				UpdateFormationView();
			});
		}else{
			UpdateFormationView();
		}
	}

	private void OnSelectMoreLayoutItem (LayoutItemController com)
	{
		_curSelectFormation = com.formation;
		UpdateFormationView();
	}

	public void UpdateFormationView(){
		_view.matrixDescLbl.text = _curSelectFormation.description;
		for(int i=0;i<5;++i){
			Vector3 itemPos = CrewModel.Instance.GetFormationPos(_curSelectFormation.playerPosition[i]);
			if(i == 0){
				_playerFormationItem.transform.localPosition = new Vector3(itemPos.x*79.5f,itemPos.y*76.8f,0f);
			}else{
				int crewFormationItemIndex = i-1;
				_crewFormationItemList[crewFormationItemIndex].transform.localPosition = new Vector3(itemPos.x*79.5f,itemPos.y*76.8f,0f);
				if(crewFormationItemIndex<CrewModel.Instance.InBattleCrewCount){
					CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(crewFormationItemIndex);
					if(crewInfo != null){
						_crewFormationItemList[crewFormationItemIndex].SetIcon("unknow");
						_crewFormationItemList[crewFormationItemIndex].SetNameLbl(crewInfo.crew.name);
                        _crewFormationItemList[crewFormationItemIndex].SetPosLbl(i+1);
						_crewFormationItemList[crewFormationItemIndex].CanDrag = true;
                    }else
                        _crewFormationItemList[crewFormationItemIndex].ResetItem();
				}else
					_crewFormationItemList[crewFormationItemIndex].ResetItem();
			}
		}
	}

	public void OnClickMoreMatrixInfo(){
		_moreLayoutItemPanel.SetActive(!_moreLayoutItemPanel.activeSelf);
	}
}
