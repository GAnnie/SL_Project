using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.scene.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.player.data;

public class PetHandbookViewController : MonoBehaviour,IViewController {

	private PetHandBookView _view;
	private ModelDisplayController _modelController;
	private List<TabBtnController> _petTypeTabBtnList;
	private List<BaseItemCellController> _petInfoItemList;
	private TabBtnController _lastPetTypeTabBtn;
	private List<PropertySliderController> _petApInfoItemList;
	private List<BaseItemCellController> _skillItemList;

	private BaseItemCellController _propsItemSlot;
	private Pet _curSelectPetInfo;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetHandBookView>();
		_view.Setup(this.transform);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (150,150);

		RegisterEvent();

		_petTypeTabBtnList = new List<TabBtnController>(2);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i=0; i<2; ++i) {
			GameObject item = NGUITools.AddChild (_view.petTypeTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_petTypeTabBtnList.Add (com);
			_petTypeTabBtnList [i].InitItem (i, OnSelectPetTypeTabBtn);
		}
		
		_petTypeTabBtnList [0].SetBtnName ("普通宠物");
		_petTypeTabBtnList [1].SetBtnName ("特殊宠物");

		//初始化资质UI控件
		_petApInfoItemList = new List<PropertySliderController>(5);
		GameObject propertySliderPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYSLIDER_WIDGET) as GameObject;
		List<string> baseApTitleList = new List<string>(5){"攻击资质","防御资质","体力资质","法力资质","速度资质"};
		List<int> hintIdList = new List<int>(5){13,14,15,16,17};
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (_view.petApItemGrid.gameObject, propertySliderPrefab);
			var controller = item.GetMissingComponent<PropertySliderController> ();
			controller.InitItem(baseApTitleList[i],hintIdList[i]);
			_petApInfoItemList.Add (controller);
		}

		//初始化技能列表UI控件
		_skillItemList = new List<BaseItemCellController>(5);
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		for(int i=0;i<5;++i){
			GameObject skillItem = NGUITools.AddChild(_view.skillItemGrid.gameObject,itemCellPrefab);
			BaseItemCellController com = skillItem.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPetSkillItem);
			_skillItemList.Add(com);
		}

		GameObject propSlotItem = NGUITools.AddChild(_view.propItemAnchor,itemCellPrefab);
		_propsItemSlot = propSlotItem.GetMissingComponent<BaseItemCellController>();
		_propsItemSlot.InitItem(0,null);
		_propsItemSlot.SetIconLblAnchor(UIWidget.Pivot.Right);

		_petInfoItemList = new List<BaseItemCellController>(40);
		OnSelectPetTypeTabBtn(0);
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.exchangeBtn.onClick,OnClickExchangeBtn);
		EventDelegate.Set(_view.buyBtn.onClick,OnClickBuyBtn);
		EventDelegate.Set(_view.captiveBtn.onClick,OnClickCaptiveBtn);
		EventDelegate.Set(_view.buyHintBtn.onClick,OnClickBuyHintBtn);
		EventDelegate.Set(_view.captiveHintBtn.onClick,OnClickCaptiveHintBtn);
		EventDelegate.Set(_view.petTypeBtn.onClick,OnClickPetTypeBtn);
	}
	public void Dispose ()
	{
	}
	#endregion

	private void OnSelectPetTypeTabBtn(int index){
		if(_lastPetTypeTabBtn != null)
			_lastPetTypeTabBtn.SetSelected(false);
		
		_lastPetTypeTabBtn = _petTypeTabBtnList[index];
		_lastPetTypeTabBtn.SetSelected(true);
		
		if(index == 0)
			OnSelectPetType_Regular();
		else if(index == 1)
			OnSelectPetType_Myth();
	}

	private List<Pet> _curPetInfoList;
	private void OnSelectPetType_Regular(){
		_curPetInfoList = PetModel.Instance.GetRegularPetInfoList();
		UpdatePetInfoItemView();
	}

	private void OnSelectPetType_Myth(){
		_curPetInfoList = PetModel.Instance.GetMythPetInfoList();
		UpdatePetInfoItemView();
	}

	private void UpdatePetInfoItemView(){
		if(_petInfoItemList.Count < _curPetInfoList.Count)
		{
			GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
			for(int i=_petInfoItemList.Count;i<_curPetInfoList.Count;++i){
				GameObject item = NGUITools.AddChild(_view.petItemGrid.gameObject,itemPrefab);
				item.GetMissingComponent<UIDragScrollView>();
				BaseItemCellController com = item.GetMissingComponent<BaseItemCellController>();
				com.InitItem(i,OnSelectPetItem);
				_petInfoItemList.Add(com);
			}
		}
		
		for(int i=0;i<_curPetInfoList.Count;++i){
			Pet pet = _curPetInfoList[i];
			_petInfoItemList[i].gameObject.SetActive(true);
			_petInfoItemList[i].SetIcon("unknow");
			if(pet.companyLevel > PlayerModel.Instance.GetPlayerLevel()){
				_petInfoItemList[i].SetGrey(true);
				_petInfoItemList[i].SetIconLbl(pet.companyLevel.ToString().WrapColor("ff0000"));
			}else{
				_petInfoItemList[i].SetGrey(false);
				_petInfoItemList[i].SetIconLbl(pet.companyLevel.ToString());
			}
		}
		  
		for(int i=_curPetInfoList.Count;i<_petInfoItemList.Count;++i){
			_petInfoItemList[i].gameObject.SetActive(false);
		}

		_view.petItemGrid.PageGrid.Reposition();
		_view.petItemGrid.SkipToPage(0,false);
		OnSelectPetItem(0);
	}

	private int _lastSelectPetItemIndex = -1;
	private void OnSelectPetItem(int index){
		if(index < _curPetInfoList.Count){
			if(_lastSelectPetItemIndex != -1)
				_petInfoItemList[_lastSelectPetItemIndex].SetSelected(false);

			_petInfoItemList[index].SetSelected(true);
			_lastSelectPetItemIndex = index;

			Pet pet = _curPetInfoList[index];
			_curSelectPetInfo = pet;
			_modelController.SetupModel(pet);
			_view.petNameLbl.text = pet.name;
			bool isMyth = (pet.petType == Pet.PetType_Myth || pet.petType == Pet.PetType_Precious);
			string petTypeSpriteName = PetModel.Instance.GetPetTypeSprite(pet);
			if(petTypeSpriteName != ""){
				_view.petTypeBtn.gameObject.SetActive(true);
				_view.petTypeBtn.normalSprite = petTypeSpriteName;
			}else
				_view.petTypeBtn.gameObject.SetActive(false);

			UpdatePetApInfo(pet.baseAptitudeProperties,isMyth);
			UpdatePetAchieveInfo(isMyth);
		}
	}

	private void UpdatePetApInfo(BaseAptitudeProperties baseApInfo,bool isMyth){
		SetupApInfo(_petApInfoItemList[0],baseApInfo.attack,isMyth);
		SetupApInfo(_petApInfoItemList[1],baseApInfo.defense,isMyth);
		SetupApInfo(_petApInfoItemList[2],baseApInfo.physical,isMyth);
		SetupApInfo(_petApInfoItemList[3],baseApInfo.magic,isMyth);
		SetupApInfo(_petApInfoItemList[4],baseApInfo.speed,isMyth);
	}

	private void SetupApInfo(PropertySliderController com,int baseVal,bool isMyth){
		if(isMyth)
			com.SetValLbl(string.Format("{0} - {1}",baseVal,baseVal));
		else{
			int minVal = Mathf.FloorToInt((float)baseVal * 1.03f);
			int maxVal = Mathf.FloorToInt((float)baseVal * 1.30f);
			com.SetValLbl(string.Format("{0} - {1}",minVal,maxVal));
		}
	}

	private void UpdatePetSkillInfo(List<int> skillIds){
		for(int i=0;i<_skillItemList.Count;++i){
			if(i < skillIds.Count)
				_skillItemList[i].SetIcon("x");
			else if(i == skillIds.Count)
				_skillItemList[i].SetIcon("unknow");
			else
				_skillItemList[i].ResetItem();
		}
	}

	private void OnSelectPetSkillItem(int index){
//		if(index < _curSelectPetInfo.skillIds.Count){
//			ProxySkillModule.ShowTips(_curSelectPetInfo.skillIds[index],_skillItemList[index].gameObject);
//		}else if(index == _curSelectPetInfo.skillIds.Count)
//			TipManager.AddTip("可通过魔兽要诀领悟新技能");
	}

	private void UpdatePetAchieveInfo(bool isMyth){
		if(_curSelectPetInfo.id == DataHelper.GetStaticConfigValue(H1StaticConfigs.DEFAULT_PET_ID,2010)){
			_view.achieveTipsLbl.cachedGameObject.SetActive(true);
			_view.regularGroup.SetActive(false);
			_view.mythGroup.SetActive(false);
			_view.achieveTipsLbl.text = "新角色第一次进入游戏时自动获得";
		}else if(_curSelectPetInfo.petType == Pet.PetType_Rare){
			_view.achieveTipsLbl.cachedGameObject.SetActive(true);
			_view.regularGroup.SetActive(false);
			_view.mythGroup.SetActive(false);
			_view.achieveTipsLbl.text = "对携带等级≤35级的宠物进行洗宠时，有小概率获得\n\n有极小概率在挂机场景的暗雷战斗中出现，可通过捕捉获得";
		}else{
			_view.achieveTipsLbl.cachedGameObject.SetActive(false);
			_view.regularGroup.SetActive(!isMyth);
			_view.mythGroup.SetActive(isMyth);
			if(!isMyth){
				if(_curSelectPetInfo.captureSceneId != 0){
					SceneMap sceneMap = DataCache.getDtoByCls<SceneMap>(_curSelectPetInfo.captureSceneId);
					_view.captiveBtnLbl.text = string.Format("前往{0}捕捉",sceneMap.name);
				}else
					_view.captiveBtnLbl.text = "无法捕捉";
	        }else{
				if(_curSelectPetInfo.petType == Pet.PetType_Myth){
					int propId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_MYTH_PROP_ID,10023);
					UpdatePropSlotInfo(propId);
				}else if(_curSelectPetInfo.petType == Pet.PetType_Precious){
					int propId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_PRECIOUS_PROP_ID,10022);
					UpdatePropSlotInfo(propId);
				}
			}
		}
	}

	private void UpdatePropSlotInfo(int propId){
		GeneralItem propItem = DataCache.getDtoByCls<GeneralItem>(propId);
		_propsItemSlot.SetIcon(propItem.icon);
		int needCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_COUNT,80);
		int propCount = BackpackModel.Instance.GetItemCount(propId);
		string countStr = string.Format("{0}/{1}",propCount,needCount);
		_propsItemSlot.SetIconLbl(propCount >= needCount ? countStr:countStr.WrapColor("fc7b6a"));
	}

	private void OnClickBuyBtn(){
		ProxyPetPropertyModule.Close();
		int shopNPCId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_SHOP_NPC_ID,5024);
		WorldManager.Instance.WalkToByNpcId(shopNPCId);
	}

	private void OnClickBuyHintBtn(){
		GameHintManager.Open(_view.buyHintBtn.gameObject,"购买获得的宠物是野生的，需要在宠物界面洗宠后才会变为宝宝，\n宝宝比野生有更高的培养价值",GameHintViewController.Side.Left);
	}

	private void OnClickCaptiveBtn(){
		if(_curSelectPetInfo.captureSceneId != 0){
			if(WorldManager.Instance.GetModel().GetSceneId() == _curSelectPetInfo.captureSceneId)
			{
				PlayerModel.Instance.StartAutoFram();
			}
			else
			{
				//PlayerModel.Instance.IsAutoFram = true;
				WorldManager.Instance.Enter(_curSelectPetInfo.captureSceneId, false,true);
	        }
			ProxyPetPropertyModule.Close();
		}else
			TipManager.AddTip("无法捕捉");
	}

	private void OnClickCaptiveHintBtn(){
		GameHintManager.Open(_view.captiveHintBtn.gameObject,"大部分为野生宠物，有可能遇到宝宝",GameHintViewController.Side.Left);
	}

	private void OnClickExchangeBtn(){
		ProxyPetPropertyModule.Close();
		WorldManager.Instance.WalkToByNpcId(5002);
	}

	private void OnClickPetTypeBtn(){
		string hint = GameHintManager.GetPetTypeHintString(_curSelectPetInfo.petType,false,false);
		GameHintManager.Open(_view.petTypeBtn.gameObject,hint);
	}
}
