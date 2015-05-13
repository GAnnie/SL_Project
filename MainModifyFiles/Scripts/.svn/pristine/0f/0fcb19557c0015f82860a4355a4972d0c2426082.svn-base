using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.battle.data;

public class PetResetTabContentController : MonoBehaviour,IViewController {

	private PetPropertyMainView _view;
	private BaseItemCellController _propItemCom;
	private PetPropertyInfo _petInfo;
	private int _ingotCost=0;
	private int _propId;
	private int _needCount;

	private const string HINTITEM = "Prefabs/Module/PetPropertyModule/PetConvenientHintItem";
	private List<PetConvenientHintItemController> _hintItemList;

	#region IViewController implementation
	public void Open(PetPropertyMainView view){
		_view = view;
		InitView();
	}

	public void InitView ()
	{
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		GameObject propItem = NGUITools.AddChild(_view.resetPropItemAnchor,itemCellPrefab);
		_propItemCom = propItem.GetMissingComponent<BaseItemCellController>();
		_propItemCom.InitItem(0,OnSelectPropItem);
		
		_propId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_RESET_ITEM_ID,10001);
		_propItemCom.SetItemLbl("还童丹");
		_propItemCom.SetIcon(_propId.ToString());
		_propItemCom.SetIconLblAnchor(UIWidget.Pivot.Right);
		
		EventDelegate.Set(_view.resetPetBtn.onClick,OnClickResetPetBtn);
		
		_hintItemList = new List<PetConvenientHintItemController>(6);

		RegisterEvent();
	}

	public void RegisterEvent ()
	{
		BackpackModel.Instance.OnUpdateItem += UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem += RemovePropsItem;
	}
	
	public void Dispose ()
	{
		BackpackModel.Instance.OnUpdateItem -= UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem -= RemovePropsItem;
	}

	#endregion

	public void UpdateViewInfo(PetPropertyInfo petInfo){
		_petInfo = petInfo;
		if(petInfo != null){
			int propItemCount = BackpackModel.Instance.GetItemCount(_propId);
			if(!petInfo.petDto.ifMutate){
				_needCount = 1;
				if(petInfo.pet.companyLevel > 0){
					int factor1 = petInfo.pet.companyLevel - petInfo.pet.companyLevel%5;
					int calcCompanyLevel = (factor1 / 5 % 2 == 0)?factor1 - 5: factor1;
					PetResetPillCost petResetCost = DataCache.getDtoByCls<PetResetPillCost>(calcCompanyLevel);
					_needCount = (petResetCost == null)? 1:petResetCost.pillCost;
				}
			}else{
				_needCount = 0;
			}

			//更新洗宠按钮文字
			if(propItemCount >= _needCount){
				_view.resetPetBtnLbl.text = "确  定";
				_ingotCost = 0;
				_propItemCom.SetIconLbl(string.Format("{0}/{1}",propItemCount,_needCount).WrapColor(ColorConstant.Color_Name));
			}
			else{
				Props props = DataCache.getDtoByCls<GeneralItem>(_propId) as Props;
				_ingotCost =(_needCount-propItemCount)*props.buyPrice;
				_view.resetPetBtnLbl.text = string.Format("{0} {1}洗宠",_ingotCost,ItemIconConst.Ingot);
				_propItemCom.SetIconLbl(string.Format("{0}/{1}",propItemCount,_needCount).WrapColor(Color.red));
			}
			
			//更新便捷提示信息
			UpdateHintItemInfo();
		}
	}

	public void CleanupViewInfo(){
		_petInfo = null;
		_view.resetPetBtnLbl.text = "确  定";
		for(int i=0;i<_hintItemList.Count;++i){
			_hintItemList[i].gameObject.SetActive(false);
		}
	}

	private void UpdatePropsItemInfo (PackItemDto itemDto){
		if(itemDto.itemId == _propId){
			UpdateViewInfo(_petInfo);
		}
	}
	
	private void RemovePropsItem(int itemIndex){
		UpdateViewInfo(_petInfo);
	}

	private void OnClickResetPetBtn(){
		if(_petInfo == null) return;

		if(_petInfo == PetModel.Instance.GetBattlePetInfo()){
			TipManager.AddTip("目标宠物处于参战状态，不能进行洗宠操作");
			return;
		}

		if(_petInfo.petDto.level >= 10 && _petInfo.petDto.ifBaobao){
			TipManager.AddTip("为避免错误操作造成损失，等级≥10级的宝宝不能进行洗宠操作");
			return;
		}

		Pet pet = _petInfo.pet;
		if(pet.petType == Pet.PetType_Myth 
		   || pet.petType == Pet.PetType_Precious 
		   || pet.petType == Pet.PetType_Rare){
			TipManager.AddTip("神兽、珍兽以及特殊宠物无法使用洗宠功能");
			return;
		}
		
		if(_ingotCost > 0 && !PlayerModel.Instance.isEnoughIngot(_ingotCost, true)){
			return;
		}

		if(_petInfo.petDto.ifMutate){
			ProxyWindowModule.OpenConfirmWindow("变异宝宝可以免费洗一次，但继续为变异的概率与普通宠物洗成变异的概率一致，即有很大概率变为普通。你确定要继续洗吗？","",()=>{
				PetModel.Instance.ResetPet(_petInfo.petDto.id,0);
			},null,UIWidget.Pivot.Left);
		}else
			PetModel.Instance.ResetPet(_petInfo.petDto.id,_ingotCost);
	}
	
	private void OnSelectPropItem(int index){
		ProxyItemTipsModule.Open(_propId,_view.resetPropItemAnchor);
	}

	#region 便捷提示
	public void UpdateHintItemInfo(){
		if(_petInfo == null) return;
		//更新便捷提示信息
		bool showPetAp = _view.TabContent_PetSkill.activeSelf;
		bool showPetSkill = _view.TabContent_PetAptitude.activeSelf;
		if(showPetAp || showPetSkill){
			_view.convenientHintItemGrid.gameObject.SetActive(true);
			if(showPetAp){
				ShowPetApHint();
			}else if(showPetSkill){
				ShowPetSkillHint();
			}
		}else
			_view.convenientHintItemGrid.gameObject.SetActive(false);
	}

	private void ShowPetApHint(){
		BaseAptitudeProperties baseApInfo = _petInfo.petDto.baseAptitudeProperty;
		string [] hintArray = new string[6];
		hintArray[0] = string.Format("攻资： {0}/{1}",baseApInfo.attack,baseApInfo.maxAttack);
		hintArray[1] = string.Format("防资： {0}/{1}",baseApInfo.defense,baseApInfo.maxDefense);
		hintArray[2] = string.Format("体资： {0}/{1}",baseApInfo.physical,baseApInfo.maxPhysical);
		hintArray[3] = string.Format("法资： {0}/{1}",baseApInfo.magic,baseApInfo.maxMagic);
		hintArray[4] = string.Format("速资： {0}/{1}",baseApInfo.speed,baseApInfo.maxSpeed);

		string lifePointStr;
		if(_petInfo.petDto.lifePoint == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_IMMORTAL_LIEF_POINT,-999))
			lifePointStr="永生";
		else
			lifePointStr=_petInfo.petDto.lifePoint.ToString();
		hintArray[5] = string.Format("寿命： {0}",lifePointStr);

		SetupHintItemInfo(hintArray);
	}

	private void ShowPetSkillHint(){
		string [] hintArray = new string[_petInfo.petDto.skillIds.Count];
		for(int i=0;i<_petInfo.petDto.skillIds.Count;++i){
			Skill skill = DataCache.getDtoByCls<Skill>(_petInfo.petDto.skillIds[i]);
			if(skill != null)
				hintArray[i] = skill.name;
		}

		SetupHintItemInfo(hintArray);
	}

	private void SetupHintItemInfo(string[] hintArray){
		if(_hintItemList.Count < hintArray.Length){
			GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(HINTITEM) as GameObject;
			int itemCount = Mathf.Max(6,hintArray.Length);
			for(int i=_hintItemList.Count;i<itemCount;++i){
				GameObject item = NGUITools.AddChild(_view.convenientHintItemGrid.gameObject,itemPrefab);
				PetConvenientHintItemController com = item.GetMissingComponent<PetConvenientHintItemController>();
				com.InitItem(i,OnSelectHintItem);
				_hintItemList.Add(com);
			}
			_view.convenientHintItemGrid.repositionNow = true;
		}

		for(int i=0;i<hintArray.Length;++i){
			_hintItemList[i].gameObject.SetActive(true);
			_hintItemList[i].SetHintLbl(hintArray[i]);
		}

		for(int i=hintArray.Length;i<_hintItemList.Count;++i){
			_hintItemList[i].gameObject.SetActive(false);
		}

		_view.convenientHintScrollView.ResetPosition();
	}

	private List<int> _hintIdList = new List<int>{19,20,21,22,23};
	private void OnSelectHintItem(int index){
		bool showPetAp = _view.TabContent_PetSkill.activeSelf;
		bool showPetSkill = _view.TabContent_PetAptitude.activeSelf;
		if(showPetAp){
			if(index < _hintIdList.Count){
				string hint = GameHintManager.GetHintIDString(_hintIdList[index]);
				GameHintManager.Open(_hintItemList[index].gameObject,hint,GameHintViewController.Side.Left);
			}
		}
	}
	#endregion
}
