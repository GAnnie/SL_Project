using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.player.msg;

public class CrewPropertyTabContentController : MonoBehaviour {
	private CrewMainView _view;

	private ModelDisplayController _modelController;

	private List<TabBtnController> _bottomInfoTabBtnList;
	private TabBtnController _lastBottomInfoTabBtn;
	private GameObject _lastBottomInfoContentRoot;

	private List<PropertyItemController> _bpItemList;
	private List<PropertyItemController> _apItemList;

	private List<PropertySliderController> _crewApInfoItemList;
	private List<BaseItemCellController> _skillItemList;
	private List<CrewSkillInfo> _skillInfoList;

	private List<BaseItemCellController> _propItemList;
	private List<PackItemDto> _packItemDtoList;
	private PackItemDto _curSelectItemDto;

	private CrewPropertyInfo _crewInfo;

	public void InitView(CrewMainView view){
		_view = view;

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (256,256);

		_bottomInfoTabBtnList = new List<TabBtnController>(3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.crewInfoTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_bottomInfoTabBtnList.Add (com);
			_bottomInfoTabBtnList [i].InitItem (i, OnSelectBottomInfoTabBtn);
		}
		
		_bottomInfoTabBtnList [0].SetBtnName ("基础属性");
		_bottomInfoTabBtnList [1].SetBtnName ("伙伴资质");
		_bottomInfoTabBtnList [2].SetBtnName ("伙伴技能");

		//基础属性
		GameObject apItemGrid = _view.TabContent_BaseProperty.transform.Find ("ApItemGrid").gameObject;
		GameObject bpItemGrid = _view.TabContent_BaseProperty.transform.Find ("BpItemGrid").gameObject;
		_apItemList = new List<PropertyItemController> (6);
		GameObject propertyItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		List<string> apItemTitleList = new List<string>(6){"体质","魔力","力量","耐力","敏捷","潜力"};
		List<int> hintIdList = new List<int>(6){24,25,26,27,28,12};
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (apItemGrid, propertyItemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.SetBgWidth(186);
			_apItemList.Add (controller);
			_apItemList[i].SetEditable(false);
			_apItemList[i].InitItem(apItemTitleList[i],0,false,hintIdList[i]);
		}
		
		_bpItemList = new List<PropertyItemController> (6);
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (bpItemGrid, propertyItemPrefab);
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

		//伙伴资质
		_crewApInfoItemList = new List<PropertySliderController>(6);
		GameObject itemGrid = _view.TabContent_Aptitude.transform.Find("itemGrid").gameObject;
		GameObject propertySliderPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYSLIDER_WIDGET) as GameObject;
		List<string> baseApTitleList = new List<string>(6){"攻击资质","防御资质","体力资质","法力资质","速度资质","寿命"};
		hintIdList = new List<int>(6){13,14,15,16,17,0};
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (itemGrid, propertySliderPrefab);
			var controller = item.GetMissingComponent<PropertySliderController> ();
			_crewApInfoItemList.Add (controller);
			_crewApInfoItemList[i].InitItem(baseApTitleList[i],hintIdList[i]);
			_crewApInfoItemList[i].SetSliderVal(0f);
		}

		//伙伴技能
		_skillItemList = new List<BaseItemCellController>(12);
		GameObject skillItemGrid = _view.TabContent_Skill.transform.Find("skillItemGrid").gameObject;
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		for(int i=0;i<12;++i){
			GameObject skillItem = NGUITools.AddChild(skillItemGrid,itemCellPrefab);
			BaseItemCellController com = skillItem.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectSkillItem);
			_skillItemList.Add(com);
		}

		//伙伴道具栏
		_packItemDtoList = BackpackModel.Instance.GetCrewPropsList();
		int itemCellCount = (_packItemDtoList.Count%2 == 0)?_packItemDtoList.Count:_packItemDtoList.Count+1;
		itemCellCount = Mathf.Max(10,itemCellCount);
		_propItemList = new List<BaseItemCellController>(itemCellCount);
		for(int i=0;i<itemCellCount;++i){
			GameObject itemCell = NGUITools.AddChild(_view.propsItemGrid.gameObject,itemCellPrefab);
			itemCell.GetMissingComponent<UIDragScrollView>();
			BaseItemCellController com = itemCell.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPetPropItem);
			com.SetIconLblAnchor(UIWidget.Pivot.Right);
			_propItemList.Add(com);
			if(i<_packItemDtoList.Count){
				com.SetIcon(_packItemDtoList[i].item.icon);
				com.SetIconLbl(_packItemDtoList[i].count.ToString());
			}
		}

		OnSelectBottomInfoTabBtn(0);
	}

	void Start(){
		BackpackModel.Instance.OnUpdateItem += UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem += RemovePropsItem;
	}
	
	void OnDestroy(){
		BackpackModel.Instance.OnUpdateItem -= UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem -= RemovePropsItem;
	}

	public void UpdateViewInfo(CrewPropertyInfo crewInfo){
		if(crewInfo == null) return;

		_crewInfo = crewInfo;
		_modelController.SetupModel(_crewInfo.crew);

		UpdateBpItemInfo(_crewInfo.hp,_crewInfo.mp,_crewInfo.attack,_crewInfo.defense,_crewInfo.speed,_crewInfo.magic);
		UpdateApItemInfo(_crewInfo.apInfo.constitution,_crewInfo.apInfo.intelligent,_crewInfo.apInfo.strength,_crewInfo.apInfo.stamina,_crewInfo.apInfo.dexterity);
		UpdateCrewBaseApInfo();
		UpdatePetSkillInfo(_crewInfo.crew.crewSkillInfos);
	}

	public void CleanUpViewInfo(){

	}

	private void OnSelectBottomInfoTabBtn(int index){
		if(_lastBottomInfoTabBtn != null)
			_lastBottomInfoTabBtn.SetSelected(false);
		
		_lastBottomInfoTabBtn = _bottomInfoTabBtnList[index];
		_lastBottomInfoTabBtn.SetSelected(true);
		
		if(index == 0)
			ChangeBottomInfoTabContent(_view.TabContent_BaseProperty);
		else if(index == 1)
			ChangeBottomInfoTabContent(_view.TabContent_Aptitude);
		else if(index == 2)
			ChangeBottomInfoTabContent(_view.TabContent_Skill);
	}

	private void ChangeBottomInfoTabContent(GameObject contentRoot){
		if(_lastBottomInfoContentRoot != null)
			_lastBottomInfoContentRoot.SetActive(false);
		
		contentRoot.SetActive(true);
		_lastBottomInfoContentRoot = contentRoot;
	}

	private void UpdateBpItemInfo (int hp, int mp, int attack, int defense, int speed, int magic)
	{
		_bpItemList [0].SetValue (hp);
		_bpItemList [1].SetValue (mp);
		_bpItemList [2].SetValue (attack);
		_bpItemList [3].SetValue (defense);
		_bpItemList [4].SetValue (speed);
		_bpItemList [5].SetValue (magic);
	}
	
	private void UpdateApItemInfo (int constitution, int intelligent, int strength, int stamina, int dexterity)
	{
		_apItemList [0].SetValue (constitution);
		_apItemList [1].SetValue (intelligent);
		_apItemList [2].SetValue (strength);
		_apItemList [3].SetValue (stamina);
		_apItemList [4].SetValue (dexterity);
	}

	private void UpdateCrewBaseApInfo(){
		BaseAptitudeProperties baseApInfo = _crewInfo.crew.baseAptitudeProperties;
		_crewApInfoItemList[0].SetValLbl(baseApInfo.attack.ToString());
		_crewApInfoItemList[1].SetValLbl(baseApInfo.defense.ToString());
		_crewApInfoItemList[2].SetValLbl(baseApInfo.physical.ToString());
		_crewApInfoItemList[3].SetValLbl(baseApInfo.magic.ToString());
		_crewApInfoItemList[4].SetValLbl(baseApInfo.speed.ToString());

		if(_crewInfo.crew.lifePoint == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_IMMORTAL_LIEF_POINT,-999))
			_crewApInfoItemList[5].SetValLbl("永生");
		else
			_crewApInfoItemList[5].SetValLbl(_crewInfo.crew.lifePoint.ToString());
	}
	
	private void UpdatePetSkillInfo(List<CrewSkillInfo> skillInfos){
		if(skillInfos != null){
			_skillInfoList = new List<CrewSkillInfo>(skillInfos.Count);
			bool isNextOpenLv = true;
			int playerLv = PlayerModel.Instance.GetPlayerLevel();
			for(int i=0;i<_skillItemList.Count;++i){
				if(i < skillInfos.Count){
					if(playerLv >= skillInfos[i].acquireLevel){
						_skillInfoList.Add(skillInfos[i]);
						_skillItemList[i].SetIcon(skillInfos[i].skill.icon.ToString());
					}else{
						if(isNextOpenLv){
							isNextOpenLv = false;
							_skillInfoList.Add(skillInfos[i]);
							_skillItemList[i].SetIcon(skillInfos[i].skill.icon.ToString());
						}else
							_skillItemList[i].ResetItem();
					}
				}
				else
					_skillItemList[i].ResetItem();
			}
		}
		else{
			for(int i=0;i<_skillItemList.Count;++i){
				_skillItemList[i].ResetItem();
			}
		}
	}
	
	private void OnSelectSkillItem(int index){
		if(_crewInfo == null) return;
		
		if(index < _skillInfoList.Count){
			ProxySkillModule.ShowTips(_skillInfoList[index].skillId,_skillItemList[index].gameObject);
		}
	}

	private void UpdatePropsItemInfo (PackItemDto itemDto){
		for(int i=0;i<_packItemDtoList.Count;++i){
			if(_packItemDtoList[i].index == itemDto.index){
				_propItemList[i].SetIconLbl(itemDto.count.ToString());
				return;
			}
		}
	}
	
	private void RemovePropsItem(int itemIndex){
		int removeIndex = -1;
		for(int i=0;i<_packItemDtoList.Count;++i){
			if(_packItemDtoList[i].index == itemIndex){
				_propItemList[i].ResetItem();
				removeIndex = i;
				break;
			}
		}
		
		if(removeIndex != -1){
			_packItemDtoList.RemoveAt(removeIndex);
			UpdatePropsItemList();
		}
	}
	
	private void UpdatePropsItemList(){
		for(int i=0;i<_propItemList.Count;++i){
			if(i<_packItemDtoList.Count){
				_propItemList[i].SetIcon(_packItemDtoList[i].item.icon);
				_propItemList[i].SetIconLbl(_packItemDtoList[i].count.ToString());
			}else
				_propItemList[i].ResetItem();
		}
	}

	private void OnSelectPetPropItem(int index){
		if(index < _packItemDtoList.Count){
			_curSelectItemDto = _packItemDtoList[index];
			ProxyItemTipsModule.Open(_curSelectItemDto,_propItemList[index].gameObject,true,OnUseSelectItem);
		}else{
			_curSelectItemDto = null;
		}
	}

	private void OnUseSelectItem(PackItemDto itemDto){

	}
}
