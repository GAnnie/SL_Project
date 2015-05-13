using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.dto;

public class PetBaseInfoViewController : MonoBehaviour,IViewController {

	private PetBaseInfoView _view;
	private ModelDisplayController _modelController;

	private PetPropertyInfo _curSelectPetInfo;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetBaseInfoView>();
		_view.Setup(this.transform);

		_modelController = ModelDisplayController.GenerateUICom(_view.modelAnchor);
		_modelController.Init(150,150);

		RegisterEvent();

		_bottomInfoTabBtnList = new List<TabBtnController>(3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.petInfoTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_bottomInfoTabBtnList.Add (com);
			_bottomInfoTabBtnList [i].InitItem (i, OnSelectBottomInfoTabBtn);
		}
		
		_bottomInfoTabBtnList [0].SetBtnName ("基础属性");
		_bottomInfoTabBtnList [1].SetBtnName ("宠物资质");
		_bottomInfoTabBtnList [2].SetBtnName ("宠物技能");

		//宠物装备栏
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		_petEqItemSlotList = new List<BaseItemCellController>(3);
		for(int i=0;i<3;++i){
			GameObject itemCell = NGUITools.AddChild(_view.petEqItemGrid.gameObject,itemCellPrefab);
			BaseItemCellController com = itemCell.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPetEqSlot);
			_petEqItemSlotList.Add(com);
		}

		InitPetPropertyInfoGroup();
		OnSelectBottomInfoTabBtn(0);
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.decorationBtn.onClick,OnClickDecorationBtn);
		EventDelegate.Set(_view.petTypeBtn.onClick,OnClickPetTypeBtn);
		EventDelegate.Set(_view.petRankingBtn.onClick,OnClickPetRankingBtn);
		EventDelegate.Set(_view.petGrowthInfoBtn.onClick,OnClickPetGrowthInfoBtn);
	}
	public void Dispose ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion

	private void OnClickPetRankingBtn(){
		GameHintManager.Open(_view.petRankingBtn.gameObject,"宠物的综合评价");
	}
	
	private void OnClickPetGrowthInfoBtn(){
		string hint = string.Format("宠物的成长率：{0}>{1}>{2}>{3}>{4}",
		                            ItemIconConst.GrowthFlag_Red,
		                            ItemIconConst.GrowthFlag_Orange,
		                            ItemIconConst.GrowthFlag_Purple,
		                            ItemIconConst.GrowthFlag_Blue,
		                            ItemIconConst.GrowthFlag_Green);
		GameHintManager.Open(_view.petGrowthInfoBtn.gameObject,hint);
	}
	
	private void OnClickPetTypeBtn(){
		string hint = GameHintManager.GetPetTypeHintString(_curSelectPetInfo.pet.petType,_curSelectPetInfo.petDto.ifMutate,_curSelectPetInfo.petDto.ifBaobao);
		GameHintManager.Open(_view.petTypeBtn.gameObject,hint);
	}
	
	private void OnClickDecorationBtn(){
		TipManager.AddTip("暂无宠物饰品");
	}

	public void ShowPetDetailInfo(PetPropertyInfo petInfo){
		_curSelectPetInfo = petInfo;

		_modelController.SetupModel(_curSelectPetInfo);
		_view.modelBtnGroup.SetActive(true);
		_view.petTypeBtn.normalSprite = PetModel.Instance.GetPetTypeSprite(_curSelectPetInfo);
		_view.petRankingLbl.text = string.Format("{0}({1})",PetModel.Instance.GetPetRankingDesc(_curSelectPetInfo),_curSelectPetInfo.ranking);
		_view.petGrowthInfoBtn.normalSprite = PetModel.Instance.GetPetGrowthFlag(_curSelectPetInfo);
		
		UpdatePetExpInfo(_curSelectPetInfo);
		UpdatePetEqInfo(_curSelectPetInfo);
		
		UpdatePetPropertyInfo(_curSelectPetInfo);
		_petAptitudeViewController.UpdateViewInfo(_curSelectPetInfo);
		UpdatePetSkillInfo(_curSelectPetInfo);
	}

	public void CleanupPetDetialInfo(){
		_curSelectPetInfo = null;
		_modelController.CleanUpModel();
		_view.modelBtnGroup.SetActive(false);
		_view.petRankingLbl.text = "------";
		_view.petGrowthInfoBtn.normalSprite = "GrowthFlag_green";
		_view.expLbl.text = "------";
		_view.expSlider.value =0f;
		UpdatePetPropertyInfo(null);
		_petAptitudeViewController.CleanUpViewInfo();
		UpdatePetSkillInfo(null);
	}

	private void UpdatePetExpInfo(PetPropertyInfo petInfo){
		ExpGrade expGrade = DataCache.getDtoByCls<ExpGrade> (petInfo.petDto.level+1);
		_view.expLbl.text = string.Format ("{0}/{1}", petInfo.petDto.exp, expGrade.petExp);
		_view.expSlider.value = (float)petInfo.petDto.exp / (float)expGrade.petExp;
	}

	#region 宠物装备
	private List<BaseItemCellController> _petEqItemSlotList;
	private static readonly string[] petEqBgSpriteNames = new string[3]{"EqPart_Chaplet","EqPart_Armor","EqPart_Amulet"};
	private void UpdatePetEqInfo(PetPropertyInfo petInfo){
		for(int i=0;i<_petEqItemSlotList.Count;++i){
			if(petInfo != null){
				PetEquipmentExtraDto petEqExtraDto = petInfo.GetPetEquipmentByPart(i+1);
				if(petEqExtraDto != null){
					_petEqItemSlotList[i].SetIcon("0");
				}else{
					_petEqItemSlotList[i].SetIcon(petEqBgSpriteNames[i]);
				}
			}else
				_petEqItemSlotList[i].SetIcon("0");
		}
	}
	
	private void OnSelectPetEqSlot(int index){
		PetEquipmentExtraDto petEqExtraDto = _curSelectPetInfo.GetPetEquipmentByPart(index+1);
		if(petEqExtraDto != null){
			ProxyItemTipsModule.OpenPetEqInfo(petEqExtraDto,_petEqItemSlotList[index].gameObject);
		}
	}
	#endregion

	#region 宠物属性、宠物资质、宠物技能分页
	private List<TabBtnController> _bottomInfoTabBtnList;
	private TabBtnController _lastBottomInfoTabBtn;
	private GameObject _lastBottomInfoContentRoot;
	
	private List<PropertyItemController> _bpItemList;
	private List<PropertyItemController> _apItemList;
	private PropertyItemController _ppItemCom;
	private PetAptitudeTabContentController _petAptitudeViewController;
	private List<BaseItemCellController> _skillItemList;

	private void InitPetPropertyInfoGroup(){
		//宠物属性
		GameObject apItemGrid = _view.TabContent_BaseProperty.transform.Find ("ApItemGrid").gameObject;
		GameObject bpItemGrid = _view.TabContent_BaseProperty.transform.Find ("BpItemGrid").gameObject;
		_apItemList = new List<PropertyItemController> (5);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (apItemGrid, itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.SetEditable (false);
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

		//宠物资质
		_petAptitudeViewController = _view.TabContent_PetAptitude.GetMissingComponent<PetAptitudeTabContentController>();
		_petAptitudeViewController.InitView();

		//宠物技能
		_skillItemList = new List<BaseItemCellController>(12);
		GameObject skillItemGrid = _view.TabContent_PetSkill.transform.Find("skillItemGrid").gameObject;
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		for(int i=0;i<12;++i){
			GameObject skillItem = NGUITools.AddChild(skillItemGrid,itemCellPrefab);
			BaseItemCellController com = skillItem.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPetSkillItem);
			_skillItemList.Add(com);
		}
	}

	private void OnSelectBottomInfoTabBtn(int index){
		if(_lastBottomInfoTabBtn != null)
			_lastBottomInfoTabBtn.SetSelected(false);
		
		_lastBottomInfoTabBtn = _bottomInfoTabBtnList[index];
		_lastBottomInfoTabBtn.SetSelected(true);
		
		if(index == 0)
			ChangeBottomInfoTabContent(_view.TabContent_BaseProperty);
		else if(index == 1)
			ChangeBottomInfoTabContent(_view.TabContent_PetAptitude);
		else if(index == 2)
			ChangeBottomInfoTabContent(_view.TabContent_PetSkill);
	}

	private void ChangeBottomInfoTabContent(GameObject contentRoot){
		if(_lastBottomInfoContentRoot != null)
			_lastBottomInfoContentRoot.SetActive(false);
		
		contentRoot.SetActive(true);
		_lastBottomInfoContentRoot = contentRoot;
	}

	private void UpdatePetPropertyInfo(PetPropertyInfo petInfo){
		if(petInfo != null){
			_ppItemCom.SetValue(petInfo.petDto.potential);
			_apItemList [0].SetValue (petInfo.petDto.aptitudeProperties.constitution, true);
			_apItemList [1].SetValue (petInfo.petDto.aptitudeProperties.intelligent, true);
			_apItemList [2].SetValue (petInfo.petDto.aptitudeProperties.strength, true);
			_apItemList [3].SetValue (petInfo.petDto.aptitudeProperties.stamina, true);
			_apItemList [4].SetValue (petInfo.petDto.aptitudeProperties.dexterity, true);

			_bpItemList [0].SetValue (petInfo.hp, true);
			_bpItemList [1].SetValue (petInfo.mp, true);
			_bpItemList [2].SetValue (petInfo.attack, true);
			_bpItemList [3].SetValue (petInfo.defense, true);
			_bpItemList [4].SetValue (petInfo.speed, true);
			_bpItemList [5].SetValue (petInfo.magic, true);
		}
		else
		{
			_ppItemCom.SetValue(0);
			for(int i=0;i<_apItemList.Count;++i){
				_apItemList[i].SetValue(0,true);
			}
			for(int i=0;i<_bpItemList.Count;++i){
				_bpItemList[i].SetValue(0,true);
			}
		}
	}

	private List<int> _curPetSkillList;
	private void UpdatePetSkillInfo(PetPropertyInfo petInfo){
		if(petInfo != null){
			_curPetSkillList = new List<int>(_skillItemList.Count);
			_curPetSkillList.AddRange(petInfo.petDto.skillIds);
			//宠物护符技能
			PetEquipmentExtraDto petEqExtraDto = petInfo.GetPetEquipmentByPart(PetEquipment.PetEquipPartType_Amulet);
			if(petEqExtraDto != null){
				_curPetSkillList.AddRange(petEqExtraDto.petSkillIds);
			}
			
			for(int i=0;i<_skillItemList.Count;++i){
				if(i < _curPetSkillList.Count)
					_skillItemList[i].SetIcon(_curPetSkillList[i].ToString());
				else if(i == _curPetSkillList.Count && petInfo.petDto.skillIds.Count < 4)
					_skillItemList[i].SetIcon("flag_add");
				else
					_skillItemList[i].ResetItem();
			}
		}
		else{
			_curPetSkillList = null;
			for(int i=0;i<_skillItemList.Count;++i){
				_skillItemList[i].ResetItem();
			}
		}
	}

	private void OnSelectPetSkillItem(int index){
		if(_curSelectPetInfo == null || _curPetSkillList == null) return;
		
		if(index < _curPetSkillList.Count){
			ProxySkillModule.ShowTips(_curPetSkillList[index],_skillItemList[index].gameObject);
		}else if(index == _curPetSkillList.Count)
			TipManager.AddTip("可通过魔兽要诀领悟新技能");
	}
	#endregion
}
