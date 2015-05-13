using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;

public class PetPropsTabContentController : MonoBehaviour,IViewController {

	private PetPropertyMainView _view;
	private PetPropertyInfo _petInfo;
	private List<BaseItemCellController> _petEqItemSlotList;
	private static readonly string[] petEqBgSpriteNames = new string[3]{"EqPart_Chaplet","EqPart_Armor","EqPart_Amulet"};

	private List<BaseItemCellController> _propItemList;
	private List<PackItemDto> _packItemDtoList;
	private PackItemDto _curSelectItemDto;

	#region IViewController implementation
	public void Open(PetPropertyMainView view){
		_view = view;
		InitView();
	}

	public void InitView ()
	{
		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		_petEqItemSlotList = new List<BaseItemCellController>(3);
		for(int i=0;i<3;++i){
			GameObject itemCell = NGUITools.AddChild(_view.petEqItemGrid.gameObject,itemCellPrefab);
			BaseItemCellController com = itemCell.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPetEqSlot);
			_petEqItemSlotList.Add(com);
		}
		
		_packItemDtoList = BackpackModel.Instance.GetPetPropsList();
		int itemCellCount = (_packItemDtoList.Count%2 == 0)?_packItemDtoList.Count:_packItemDtoList.Count+1;
		itemCellCount = Mathf.Max(10,itemCellCount);
		_propItemList = new List<BaseItemCellController>(itemCellCount);
		for(int i=0;i<itemCellCount;++i){
			GameObject itemCell = NGUITools.AddChild(_view.petPropsItemGrid.gameObject,itemCellPrefab);
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

		UpdatePetEqInfo();
	}

	private void UpdatePropsItemInfo (PackItemDto itemDto){
		for(int i=0;i<_packItemDtoList.Count;++i){
			if(_packItemDtoList[i].index == itemDto.index){
				_packItemDtoList[i] = itemDto;
				_propItemList[i].SetIcon(_packItemDtoList[i].item.icon);
				_propItemList[i].SetIconLbl(_packItemDtoList[i].count.ToString());
				return;
			}
		}

		_packItemDtoList.Add(itemDto);
		UpdatePropsItemList();
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
			_propItemList[i].ResetItem();
			if(i<_packItemDtoList.Count){
				_propItemList[i].SetIcon(_packItemDtoList[i].item.icon);
				_propItemList[i].SetIconLbl(_packItemDtoList[i].count.ToString());
			}
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
		if(itemDto.item is Props){
			Props props = itemDto.item as Props;
			if(props.logicId == Props.PropsLogicEnum_PET_WASH){
				//宠物洗点丹
				UseResetApPointProp();
			}else if(props.logicId == Props.PropsLogicEnum_PET_LIFE){
				//宠物恢复寿命道具
				RestoreLifePoint();
			}
		}

		if(itemDto.itemId == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXP_BOOK_ITEM_ID,10013)){
			//宠物经验心得
			int levelLimit = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_UPGRADABLE_EXTRA_LEVEL_COMPARE_MAIN_CHARACTOR_LEVEL,5);
			if(_petInfo.petDto.level - PlayerModel.Instance.GetPlayerLevel() < levelLimit){
				PetModel.Instance.UsePetExpBook(_petInfo.petDto.id,itemDto.itemId);
			}else
				TipManager.AddTip(string.Format("已高于人物{0}级，不能使用",levelLimit));
		}else if(itemDto.itemId == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_INCREASE_BASE_APTITUDE_ITEM_ID,10034)){
			//宠物资质丹
			UseIncreasePetBaseApProp();
		}else if(itemDto.item.itemType == H1Item.ItemTypeEnum_PetSkillBook){
			//魔兽要诀
			UsePetSkillBookProp();
		}else if(itemDto.item.itemType == H1Item.ItemTypeEnum_PetEquipment){
			WearEquipment();
		}
	}

	#region 宠物洗点
	private void UseResetApPointProp(){
		AptitudeProperties petApInfo = _petInfo.petDto.aptitudeProperties;
		int pointLimit = DataHelper.GetStaticConfigValue(H1StaticConfigs.MIN_APTITUDE_RESET_POINT,10);
		int minApPoint = _petInfo.petDto.level + pointLimit;
		int[] _canResetApPointArray = new int[5];
		_canResetApPointArray[0] = petApInfo.constitution - minApPoint;
		_canResetApPointArray[1] = petApInfo.intelligent - minApPoint;
		_canResetApPointArray[2] = petApInfo.strength - minApPoint;
		_canResetApPointArray[3] = petApInfo.stamina - minApPoint;
		_canResetApPointArray[4] = petApInfo.dexterity - minApPoint;

		List<int> _resetApPointIndexList = new List<int>(5); //此列表保存_canResetApPointArray中大于>0的数组索引
		for(int i=0;i<_canResetApPointArray.Length;++i){
			if(_canResetApPointArray[i] > 0){
				_resetApPointIndexList.Add(i);
			}
		}
		
		if(_resetApPointIndexList.Count>0){
			List<string> optionTitleList = new List<string>{"体质","魔力","力量","耐力","敏捷"};
			List<string> optionStrList = new List<string>(_resetApPointIndexList.Count);
			for(int i=0;i<_resetApPointIndexList.Count;++i){
				optionStrList.Add(string.Format("{0}(可洗点数{1})",optionTitleList[_resetApPointIndexList[i]],_canResetApPointArray[_resetApPointIndexList[i]]));
			}
			ProxyWorldMapModule.OpenCommonDialogue("宠物洗点","请选择你要重置的基础属性：",optionStrList,(selectIndex)=>{
				if(selectIndex < _resetApPointIndexList.Count){
					int originIndex = _resetApPointIndexList[selectIndex];
					int apType = originIndex+1;
					int resetPoint = Mathf.Min(_canResetApPointArray[originIndex],DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_RESET_PER_APTITUDE,2));
					if(resetPoint == 1){
						string hint = string.Format("该属性只有{0}点属性点可转化，是否继续",resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str));
						ProxyWindowModule.OpenConfirmWindow(hint,"",()=>{
							PetModel.Instance.ResetPetAptitude(_petInfo,_curSelectItemDto.index,apType,resetPoint);
						});
					}
					else
						PetModel.Instance.ResetPetAptitude(_petInfo,_curSelectItemDto.index,apType,resetPoint);
				}
			});
		}else
			TipManager.AddTip(string.Format("{0}所有基础属性点均已达到自身等级+{1}，不能继续减少",
			                                _petInfo.pet.name.WrapColor(ColorConstant.Color_Tip_Item_Str),
			                                pointLimit));
	}
	#endregion

	#region 提升宠物资质
	private void UseIncreasePetBaseApProp(){
		if(_petInfo == null){
			TipManager.AddTip("请选择需要提升资质的宠物");
			return;
		}

		if(PetModel.Instance.isFullBasePetAp(_petInfo))
			TipManager.AddTip("该宠物所有资质已达上限，无需使用");
		else{
			ProxyPetPropertyModule.OpenIncreaseBaseApView(_petInfo);
		}
	}
	#endregion

	#region 宠物打书
	private void UsePetSkillBookProp(){
		PetSkillBook skillBook = _curSelectItemDto.item as PetSkillBook;
		Skill skill = DataCache.getDtoByCls<Skill>(skillBook.petSkillId);
		if(skill == null){
			Debug.LogError("PetSkillBook skill is null");
			return;
		}

		for(int i=0;i<_petInfo.petDto.skillIds.Count;++i){
			if(_petInfo.petDto.skillIds[i] == skillBook.petSkillId)
			{
				TipManager.AddTip(string.Format("{0}已经学会了{1}，无需再使用",_petInfo.petDto.name,skill.name));
				return;
			}
		}

		string content=string.Format("确定要对{0}使用{1}吗？\n{1}:{2}",_petInfo.petDto.name.WrapColor(ColorConstant.Color_Name),skill.name,skill.description);
		List<string> optionStrList = new List<string>(2);
		if(_petInfo.petDto.skillIds.Count<4){
			optionStrList.Add("确定使用（有一定几率替换一个原有技能）");
		}else if(_petInfo.petDto.skillIds.Count >= 4){
			optionStrList.Add("确定使用（100%替换一个原有技能）");
		}
		optionStrList.Add("取消操作");
		ProxyWorldMapModule.OpenCommonDialogue("宠物打书",content,optionStrList,OnSelectFightSkillOption);
	}

	private void OnSelectFightSkillOption(int index){
		if(index == 0){
			PetModel.Instance.FightSkillBook(_petInfo,_curSelectItemDto);
		}
	}
	#endregion

	#region 宠物装备
	private void UpdatePetEqInfo(){
		for(int i=0;i<_petEqItemSlotList.Count;++i){
			_petEqItemSlotList[i].SetIcon(petEqBgSpriteNames[i]);
			if(_petInfo != null){
				PetEquipmentExtraDto petEqExtraDto = _petInfo.GetPetEquipmentByPart(i+1);
				if(petEqExtraDto != null){
					_petEqItemSlotList[i].SetIcon("0");
				}
			}
		}
	}

	private void OnSelectPetEqSlot(int index){
		if(_petInfo == null) return;

		PetEquipmentExtraDto petEqExtraDto = _petInfo.GetPetEquipmentByPart(index+1);
		if(petEqExtraDto != null){
			ProxyItemTipsModule.OpenPetEqInfo(petEqExtraDto,_petEqItemSlotList[index].gameObject);
		}
	}

	private void WearEquipment(){
		PetModel.Instance.WearPetEquipment(_petInfo,_curSelectItemDto);
	}
	#endregion

	#region 恢复寿命
	private void RestoreLifePoint(){
		if(_petInfo.petDto.lifePoint >= DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_MAX_LIFE,60000)){
			TipManager.AddTip(string.Format("{0}当前寿命已经足够多了\n不需要增加寿命",_petInfo.pet.name.WrapColor(ColorConstant.Color_Tip_Item)));
		}else if(_petInfo.petDto.lifePoint < 0){
			TipManager.AddTip("这只宠物是永生的，不需要增加寿命");
		}else
			PetModel.Instance.AddLifeByProps(_curSelectItemDto,_petInfo);
	}
	#endregion
}
