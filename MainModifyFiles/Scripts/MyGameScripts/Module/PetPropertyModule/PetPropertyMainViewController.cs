using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class PetPropertyMainViewController : MonoBehaviour,IViewController {

	private PetPropertyMainView _view;
	private ModelDisplayController _modelController;

	#region IViewController implementation
	public void Open(int defaultRightTab){
		InitView();

		OnSelectBottomInfoTabBtn(0);
		OnSelectRightTabBtn(defaultRightTab);
	}

	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetPropertyMainView> ();
		_view.Setup (this.transform);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (150,150);

		RegisterEvent();

		InitTabBtn();
		InitRightTabContent();
		InitPetPropertyInfoGroup();
		InitPetSlotItemList();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,CloseView);
		EventDelegate.Set(_view.decorationBtn.onClick,OnClickDecorationBtn);
		EventDelegate.Set(_view.petChangeBtn.onClick,OnClickPetChangeBtn);
		EventDelegate.Set(_view.petTypeBtn.onClick,OnClickPetTypeBtn);
		EventDelegate.Set(_view.petRankingBtn.onClick,OnClickPetRankingBtn);
		EventDelegate.Set(_view.petGrowthInfoBtn.onClick,OnClickPetGrowthInfoBtn);
		EventDelegate.Set(_view.freeCaptiveBtn.onClick,OnClickFreeCaptive);
		EventDelegate.Set(_view.changeNameBtn.onClick,OnClickChangeName);
		EventDelegate.Set(_view.restOrBattleBtn.onClick,OnClickRestOrBattle);
		EventDelegate.Set(_view.resetPetApBtn.onClick,OnClickResetPetApBtn);

		PetModel.Instance.OnPetExpUpdate += HandleOnPetExpUpdate;
		PetModel.Instance.OnPetInfoUpdate += HandleOnPetInfoUpdate;
		PetModel.Instance.OnChangeBattlePet += HandleOnChangeBattlePet;
		PetModel.Instance.OnPetInfoListUpdate += UpdatePetSlotItemList;
	}

	public void Dispose ()
	{
		PetModel.Instance.OnPetExpUpdate -= HandleOnPetExpUpdate;
		PetModel.Instance.OnPetInfoUpdate -= HandleOnPetInfoUpdate;
		PetModel.Instance.OnChangeBattlePet -= HandleOnChangeBattlePet;
		PetModel.Instance.OnPetInfoListUpdate -= UpdatePetSlotItemList;
		_petPropsController.Dispose();
		_petResetController.Dispose();
	}
	#endregion

	#region BaseInfoGroup 
	private List<CharacterInfoItemController> _petSlotItemList;

	private void InitTabBtn(){
		_rightTabBtnList = new List<TabBtnController>(3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.rightTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_rightTabBtnList.Add (com);
			_rightTabBtnList [i].InitItem (i, OnSelectRightTabBtn);
		}
		
		_rightTabBtnList [0].SetBtnName ("宠物属性");
		_rightTabBtnList [1].SetBtnName ("洗   宠");
		_rightTabBtnList [2].SetBtnName ("图   鉴");
		
		_bottomInfoTabBtnList = new List<TabBtnController>(3);
		tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.petInfoTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_bottomInfoTabBtnList.Add (com);
			_bottomInfoTabBtnList [i].InitItem (i, OnSelectBottomInfoTabBtn);
		}
		
		_bottomInfoTabBtnList [0].SetBtnName ("基础属性");
		_bottomInfoTabBtnList [1].SetBtnName ("宠物资质");
		_bottomInfoTabBtnList [2].SetBtnName ("宠物技能");
	}

	private void InitPetSlotItemList(){
	
		int maxSlotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_MAX_COMPANY_AMOUNT,10);
		int petSlotCount = Mathf.Min(PetModel.Instance.PetOpenSlotCount+1,maxSlotCount);

		_petSlotItemList = new List<CharacterInfoItemController> (petSlotCount);
		GameObject memberItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ProxyPlayerTeamModule.CHARACTERINFO_WIDGET) as GameObject;
		for (int i=0; i<petSlotCount; ++i) {
			GameObject item = NGUITools.AddChild (_view.petItemGrid.gameObject, memberItemPrefab);
			item.GetMissingComponent<UIDragScrollView>();
			CharacterInfoItemController com = item.GetMissingComponent<CharacterInfoItemController> ();
			com.InitItem (i, OnSelectPetSlotItem);
			_petSlotItemList.Add (com);
			UpdatePetSlotItemInfo(i);
		}

		SelectDefaultPetInfo();
	}

	private void UpdatePetSlotItemList(){
		for(int i=0;i<_petSlotItemList.Count;++i){
			UpdatePetSlotItemInfo(i);
		}

		SelectDefaultPetInfo();
	}

	private void SelectDefaultPetInfo(){
		_curSelectPetItemIndex = -1;
		_curSelectPetInfo = null;
		if(PetModel.Instance.GetPetCount() > 0){
			int battlePetIndex = PetModel.Instance.GetBattlePetIndex();
			if(battlePetIndex != -1)
				OnSelectPetSlotItem(battlePetIndex);
			else
				OnSelectPetSlotItem(0);
		}
		else
			CleanupPetDetialInfo();
	}

	private void UpdatePetSlotItemInfo(int index){
		PetPropertyInfo petInfo = PetModel.Instance.GetPetInfoByIndex(index);
		_petSlotItemList[index].ResetItem();
		if(petInfo != null){
			_petSlotItemList[index].SetInfoLbl(string.Format("{0}\n等级：{1}",petInfo.petDto.name,petInfo.petDto.level));
			_petSlotItemList[index].SetIcon("0");
			if(petInfo == PetModel.Instance.GetBattlePetInfo())
				_petSlotItemList[index].SetIconFlagState(true);
			else
				_petSlotItemList[index].SetIconFlagState(false);
		}
		else{
			if(index == PetModel.Instance.PetOpenSlotCount)
				_petSlotItemList[index].SetIcon("flag_lock");
			else
				_petSlotItemList[index].SetIcon("flag_add");
		}
	}

	//用元宝开启新的宠物槽
	private void ExpandPetSlotItem(){
		_petSlotItemList[_petSlotItemList.Count-1].SetIcon("flag_add");

		//加入下一个可开锁槽，满10个时不再添加
		if(_petSlotItemList.Count < DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_MAX_COMPANY_AMOUNT,10)){
			GameObject memberItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ProxyPlayerTeamModule.CHARACTERINFO_WIDGET) as GameObject;
			GameObject item = NGUITools.AddChild (_view.petItemGrid.gameObject, memberItemPrefab);
			item.GetMissingComponent<UIDragScrollView>();
			CharacterInfoItemController com = item.GetMissingComponent<CharacterInfoItemController> ();
			com.InitItem (_petSlotItemList.Count, OnSelectPetSlotItem);
			_petSlotItemList.Add (com);
			com.ResetItem();
			com.SetIcon("flag_lock");

			_view.petItemGrid.repositionNow = true;
		}
    }

	private int _curSelectPetItemIndex = -1;
	private PetPropertyInfo _curSelectPetInfo = null;
	private void OnSelectPetSlotItem(int index){
		PetPropertyInfo petInfo = PetModel.Instance.GetPetInfoByIndex(index);
		if(petInfo != null){
			if(_curSelectPetItemIndex == index) return;

			if (_curSelectPetItemIndex != -1)
				_petSlotItemList [_curSelectPetItemIndex].SetSelected (false);

			_curSelectPetItemIndex = index;
			_petSlotItemList[index].SetSelected(true);
			ShowPetDetailInfo(petInfo);
		}
		else
		{
			if(index == PetModel.Instance.PetOpenSlotCount)
				OnClickLockedSlotItem();
			else
			{
				ProxyWindowModule.OpenConfirmWindow("是否前往宠物图鉴界面查看可携带的宠物？","",()=>{
					OnSelectRightTabBtn(2);
				});
			}
		}
	}

	private void CleanupPetDetialInfo(){
		_curSelectPetItemIndex = -1;
		_curSelectPetInfo = null;
		_modelController.CleanUpModel();
		_view.modelBtnGroup.SetActive(false);
		_view.petRankingLbl.text = "------";
		_view.petGrowthInfoBtn.normalSprite = "GrowthFlag_green";
		_view.expLbl.text = "------";
		_view.expSlider.value =0f;
		_petResetController.CleanupViewInfo();
		_petPropertyViewController.CleanUpViewInfo();
		_petAptitudeViewController.CleanUpViewInfo();
		_petPropsController.UpdateViewInfo(null);
		UpdatePetSkillInfo(null);
	}

	private void ShowPetDetailInfo(PetPropertyInfo petInfo){
		_curSelectPetInfo = petInfo;

		_modelController.SetupModel(petInfo);
		_view.modelBtnGroup.SetActive(true);
		_view.petTypeBtn.normalSprite = PetModel.Instance.GetPetTypeSprite(petInfo);
		_view.petRankingLbl.text = string.Format("{0}({1})",PetModel.Instance.GetPetRankingDesc(petInfo),petInfo.ranking);
		_view.petGrowthInfoBtn.normalSprite = PetModel.Instance.GetPetGrowthFlag(petInfo);

		UpdatePetExpInfo(petInfo);

		_petPropertyViewController.UpdateViewInfo(petInfo);
		_petAptitudeViewController.UpdateViewInfo(petInfo);
		UpdatePetSkillInfo(petInfo);

		UpdateResetPetApBtnState();
		//更新休息按钮状态
		UpdateRestOrBattleBtnState();

		_petPropsController.UpdateViewInfo(_curSelectPetInfo);
		_petResetController.UpdateViewInfo(_curSelectPetInfo);

		if(petInfo.pet.petType == Pet.PetType_Myth || petInfo.pet.petType == Pet.PetType_Precious)
			_view.petChangeBtn.gameObject.SetActive(!petInfo.petDto.joinedBattle);
		else
			_view.petChangeBtn.gameObject.SetActive(false);
	}

	private void UpdatePetExpInfo(PetPropertyInfo petInfo){
		ExpGrade expGrade = DataCache.getDtoByCls<ExpGrade> (petInfo.petDto.level+1);
		_view.expLbl.text = string.Format ("{0}/{1}", petInfo.petDto.exp, expGrade.petExp);
		_view.expSlider.value = (float)petInfo.petDto.exp / (float)expGrade.petExp;
	}

	private void OnClickLockedSlotItem(){
		int curSlotCount = PetModel.Instance.PetOpenSlotCount;
		int maxSlotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_MAX_COMPANY_AMOUNT,10);
		if(curSlotCount < maxSlotCount){
			int propsId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_COMPANY_VACANCY_EXPAND_ITEM_ID,10036);
			//需要道具数量=（当前栏数+初始需要道具数）-初始化栏数
			int needCount = (curSlotCount+DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXPAND_COMPANY_VACANCY_INIT_ITEM_AMOUNT,3))-DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_INIT_CARRY_CAPACITY,5);

			ProxyTipsModule.Open(string.Format("宠物携带数量{0}→{1}",curSlotCount,curSlotCount+1),propsId,needCount,(ingot)=>{
				PetModel.Instance.ExpandPetSlot(needCount,ExpandPetSlotItem);
			});
		}
		else
			TipManager.AddTip(string.Format("最多扩大到{0}个携带栏位",maxSlotCount));
	}

	private void OnClickPetRankingBtn(){
		GameHintManager.Open(_view.petRankingBtn.gameObject,"宠物的综合评价");
	}

	private void OnClickPetGrowthInfoBtn(){
		string hint = string.Format("宠物的成长率：{4}<{3}<{2}<{1}<{0}",
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

	private void OnClickPetChangeBtn(){

		int ingotCount = 0;
		string petTypeStr = "";
		if( _curSelectPetInfo.pet.petType == Pet.PetType_Myth)
		{
			ingotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_MYTH_GOLD,4000);
			petTypeStr="神兽";
		}else{
			ingotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_PRECIOUS_GOLD,2000);
			petTypeStr="珍兽";
		}

		ProxyWindowModule.OpenConfirmWindow(string.Format("确认消耗{0}元宝再随机抽取一次{1}类型吗？",ingotCount,petTypeStr),"",()=>{
			if(PlayerModel.Instance.isEnoughIngot(ingotCount,true))
				PetModel.Instance.ChangeMythOrPreciousPet(_curSelectPetInfo);
		},null,UIWidget.Pivot.Left);
	}

	private void OnClickFreeCaptive(){
		PetModel.Instance.DropPet(_curSelectPetItemIndex);
	}

	private void OnClickChangeName(){
		ProxyPetPropertyModule.OpenChangeNameView(_curSelectPetInfo);
	}

	private void OnClickRestOrBattle(){
		if(_curSelectPetInfo == null) return;

		if(_curSelectPetInfo == PetModel.Instance.GetBattlePetInfo()){
			PetModel.Instance.DeactiveBattlePet();
		}else{
			PetModel.Instance.ChangeBattlePet(_curSelectPetItemIndex);
		}
	}

	private void OnClickResetPetApBtn(){
		if(_curSelectPetInfo == null) return;

		if(_curSelectPetInfo.petDto.level <10)
		{
			TipManager.AddTip("重置属性点需要宠物等级≥10级");
		}else{
			if(_curSelectPetInfo != PetModel.Instance.GetBattlePetInfo()){
				ProxyWindowModule.OpenConfirmWindow("每个宠物宝宝只有1次免费重置所有已分配潜力点的机会，确认要现在进行重置吗？","", ()=>{
						PetModel.Instance.ResetPetAllAp(_curSelectPetInfo.petDto.id);
				},null,UIWidget.Pivot.Left);
			}else
				TipManager.AddTip("该宠物处于参战状态，不能重置属性");
		}
	}

	private void UpdateResetPetApBtnState(){

		if(!_curSelectPetInfo.petDto.hasCustomAptitude){
			_view.resetPetApBtn.gameObject.SetActive(true);
			UISprite sprite = _view.resetPetApBtn.GetComponent<UISprite>();
			UILabel label = _view.resetPetApBtn.transform.Find("Label").GetComponent<UILabel>();
			bool isGrey = _curSelectPetInfo.petDto.level < 10;
			sprite.isGrey = isGrey;
			label.isGrey = isGrey;
		}else
			_view.resetPetApBtn.gameObject.SetActive(false);
	}

	private void UpdateRestOrBattleBtnState(){
		if(_curSelectPetInfo == PetModel.Instance.GetBattlePetInfo())
			_view.restOrBattleBtnLbl.text = "休  息";
		else
			_view.restOrBattleBtnLbl.text = "参  战";
	}

	void HandleOnChangeBattlePet (int battlePetIndex)
	{
		for(int i=0;i<_petSlotItemList.Count;++i){
			if(battlePetIndex == i)
				_petSlotItemList[i].SetIconFlagState(true);
			else
				_petSlotItemList[i].SetIconFlagState(false);
		}

		_view.petChangeBtn.gameObject.SetActive(false);
		UpdateRestOrBattleBtnState();
	}

	void HandleOnPetInfoUpdate (int petIndex)
	{
		UpdatePetSlotItemInfo(petIndex);
		if(petIndex == _curSelectPetItemIndex){
			_petSlotItemList[_curSelectPetItemIndex].SetSelected(true);
			ShowPetDetailInfo(_curSelectPetInfo);
		}
    }

	void HandleOnPetExpUpdate (int petIndex)
	{
		UpdatePetSlotItemInfo(petIndex);
		if(petIndex == _curSelectPetItemIndex){
			_petSlotItemList[_curSelectPetItemIndex].SetSelected(true);
			ShowPetDetailInfo(_curSelectPetInfo);
		}
	}
	#endregion

	#region RightTabContent
	private List<TabBtnController> _rightTabBtnList;
	private TabBtnController _lastRightTabBtn;
	private GameObject _lastRightContentRoot;
	private PetPropsTabContentController _petPropsController;
	private PetResetTabContentController _petResetController;
	private PetHandbookViewController _petHandbookController;

	private void InitRightTabContent(){
		_petPropsController = _view.TabContent_PetProps.GetMissingComponent<PetPropsTabContentController>();
		_petPropsController.Open(_view);
		_lastRightContentRoot = _view.TabContent_PetProps;
		
		_petResetController = _view.TabContent_ResetPetAp.GetMissingComponent<PetResetTabContentController>();
		_petResetController.Open(_view);
	}

	private void OnSelectRightTabBtn(int index){
		if(_lastRightTabBtn != null)
			_lastRightTabBtn.SetSelected(false);

		_lastRightTabBtn = _rightTabBtnList[index];
		_lastRightTabBtn.SetSelected(true);

		if(index == 0)
			OnSelectPetProps();
		else if(index == 1)
			OnSelectResetPetAp();
		else if(index == 2)
			OnSelectPetHandBook();
	}

	private void ChangeRightTabContent(GameObject contentRoot){
		_view.BaseInfoGroup.SetActive(true);
		_view.PetHandbookGroup.SetActive(false);
		if(_lastRightContentRoot != null)
			_lastRightContentRoot.SetActive(false);

		contentRoot.SetActive(true);
		_lastRightContentRoot = contentRoot;
	}

	private void OnSelectPetProps(){
		SetupTitleSprite("title_petProperty");
		ChangeRightTabContent(_view.TabContent_PetProps);
		_view.pageFlagGrid.SetActive(true);
		_view.rightTabContentTitle.text ="道具栏";
	}

	private void OnSelectResetPetAp(){
		SetupTitleSprite("title_washPetAp");
		ChangeRightTabContent(_view.TabContent_ResetPetAp);
		OnSelectBottomInfoTabBtn(2);
		_view.pageFlagGrid.SetActive(false);
		_view.rightTabContentTitle.text ="洗宠";
	}

	private void OnSelectPetHandBook(){
		SetupTitleSprite("title_petHandBook");
		_view.BaseInfoGroup.SetActive(false);
		_view.PetHandbookGroup.SetActive(true);

		if(_petHandbookController == null){
			GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab( ProxyPetPropertyModule.PETHANDBOOK_VIEW ) as GameObject;
			GameObject module = NGUITools.AddChild(_view.PetHandbookGroup,modulePrefab);
			UIHelper.AdjustDepth(module,1);
			_petHandbookController = module.GetMissingComponent<PetHandbookViewController>();
			_petHandbookController.InitView();
		}
	}

	private void SetupTitleSprite(string spriteName){
		_view.titleSprite.spriteName = spriteName;
		_view.titleSprite.MakePixelPerfect();
	}
	#endregion

	#region PetProperty TabContent
	private List<TabBtnController> _bottomInfoTabBtnList;
	private TabBtnController _lastBottomInfoTabBtn;
	private GameObject _lastBottomInfoContentRoot;
	private PetBasePropertyTabContentController _petPropertyViewController;
	private PetAptitudeTabContentController _petAptitudeViewController;
	private List<BaseItemCellController> _skillItemList;

	private void InitPetPropertyInfoGroup(){

		_petPropertyViewController = _view.TabContent_BaseProperty.GetMissingComponent<PetBasePropertyTabContentController>();
		_petPropertyViewController.InitView(_view);
		
		_petAptitudeViewController = _view.TabContent_PetAptitude.GetMissingComponent<PetAptitudeTabContentController>();
		_petAptitudeViewController.InitView();

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

		_petResetController.UpdateHintItemInfo();
	}

	private void OnSelectBasePropertyView(){
		ChangeBottomInfoTabContent(_view.TabContent_BaseProperty);
	}

	private void OnSelectPetAptitudeView(){
		ChangeBottomInfoTabContent(_view.TabContent_PetAptitude);
	}

	private void OnSelectPetSkillView(){
		ChangeBottomInfoTabContent(_view.TabContent_PetSkill);
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

	public void CloseView(){
		ProxyPetPropertyModule.Close();
	}
}
