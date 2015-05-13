using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.battlebuff.data;

public class PetPropertyInfo
{
	public PetCharactorDto petDto;
	
	/** 敏捷 */
	public int speed;
	
	/** 攻击力 */
	public int attack;
	
	/** 防御力 */
	public int defense;
	
	/** hp */
	public int hp;
	
	/** mp */
	public int mp;
	
	/** 灵力 */
	public int magic;

	/** 战斗力 */
	public int ranking;

	private Pet _pet;
	public  Pet pet{
		get
		{
			if (_pet !=null){
				return _pet;
			}else{
				_pet = DataCache.getDtoByCls<GeneralCharactor>(petDto.charactorId) as Pet;
				return _pet;
			}
		}
	}
	
	public PetPropertyInfo (PetCharactorDto dto)
	{
		petDto = dto;
		PetModel.CalculatePetBp(this);
		PetModel.CalculatePetRanking(this);
	}

	public void ResetPetDto(PetCharactorDto dto){
		petDto = dto;
		_pet = null;
		PetModel.CalculatePetBp(this);
		PetModel.CalculatePetRanking(this);
	}

	public void IncreasePetBaseApPoint(int addPoint,int baseApType){
		if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Attack)
			petDto.baseAptitudeProperty.attack += addPoint;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Defense)
			petDto.baseAptitudeProperty.defense += addPoint;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Physical)
			petDto.baseAptitudeProperty.physical += addPoint;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Magic)
			petDto.baseAptitudeProperty.magic += addPoint;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Speed)
			petDto.baseAptitudeProperty.speed += addPoint;
		
		PetModel.CalculatePetBp(this);
		PetModel.CalculatePetRanking(this);
	}

	public PetEquipmentExtraDto GetPetEquipmentByPart(int partType){
		if(petDto.petEquipments != null && petDto.petEquipments.Count > 0){
			for(int i=0;i<petDto.petEquipments.Count;++i){
				PetEquipmentExtraDto petEqExtraDto = petDto.petEquipments[i];
				PetEquipment petEq = DataCache.getDtoByCls<GeneralItem>(petEqExtraDto.petEquipmentId) as PetEquipment;
				if(petEq.petEquipPartType == partType)
					return petEqExtraDto;
			}
		}
		return null;
	}

	public int GetCurBaseApVal(int baseApType){
		if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Attack)
			return petDto.baseAptitudeProperty.attack;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Defense)
			return petDto.baseAptitudeProperty.defense;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Physical)
			return petDto.baseAptitudeProperty.physical;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Magic)
			return petDto.baseAptitudeProperty.magic;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Speed)
			return petDto.baseAptitudeProperty.speed;
		else
			return 0;
	}

	public int GetMaxBaseApVal(int baseApType){
		if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Attack)
			return petDto.baseAptitudeProperty.maxAttack;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Defense)
			return petDto.baseAptitudeProperty.maxDefense;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Physical)
			return petDto.baseAptitudeProperty.maxPhysical;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Magic)
			return petDto.baseAptitudeProperty.maxMagic;
		else if(baseApType == BaseAptitudeProperties.BaseAptitudeType_Speed)
			return petDto.baseAptitudeProperty.maxSpeed;
		else
			return 0;
	}
}

public class PetModel
{
	private static readonly PetModel _instance = new PetModel ();

	public static PetModel Instance {
		get {
			return _instance;
		}
	}

	private PetModel ()
	{

	}

	private int _battlePetIndex = -1;
	private List<PetPropertyInfo> _petPropertyInfoList;
	private int _petOpenSlotCount = 5;

	public event System.Action<int> OnPetExpUpdate;
	public event System.Action<int> OnPetGradeUpdate;
	public event System.Action OnPetInfoListUpdate; 	//Remove or Add
	public event System.Action<int> OnPetInfoUpdate; 	//ChangeName or ResetPet
	public event System.Action<int> OnChangeBattlePet;

	#region Getter
	public int PetOpenSlotCount {
		get {
			return _petOpenSlotCount;
		}
	}

	public bool isFullPet{
		get{
			return _petPropertyInfoList.Count == _petOpenSlotCount;
		}
	}

	public bool isNaturePetAndNotInBattle(PetPropertyInfo petInfo){
		if(petInfo == null) return false;

		if(petInfo.pet.petType == Pet.PetType_Regular || petInfo.pet.petType == Pet.PetType_Rare){
			if(!petInfo.petDto.ifMutate && !petInfo.petDto.ifBaobao)
			{
				if(_battlePetIndex != GetPetIndex(petInfo.petDto.id))
					return true;
			}
		}

		return false;
	}

	public int GetBattlePetIndex(){
		return _battlePetIndex;
	}

	public PetPropertyInfo GetBattlePetInfo ()
	{
		if(_battlePetIndex != -1)
			return _petPropertyInfoList [_battlePetIndex];

		return null;
	}

	public int GetBattlePetLevel(){
		if(_battlePetIndex != -1)
			return _petPropertyInfoList [_battlePetIndex].petDto.level;

		return 0;
	}

	public PetPropertyInfo GetPetInfoByIndex (int index)
	{
		if (index == -1 || _petPropertyInfoList == null)
			return null;
		
		if (index < _petPropertyInfoList.Count)
			return _petPropertyInfoList [index];
		
		return null;
	}

	public PetPropertyInfo GetPetInfoByUID (long petUID)
	{
		if (_petPropertyInfoList == null)
			return null;

		for (int i=0; i<_petPropertyInfoList.Count; ++i) {
			if (_petPropertyInfoList [i].petDto.id == petUID)
				return _petPropertyInfoList [i];
		}

		return null;
	}

	public int GetPetIndex(long petUID){
		if (_petPropertyInfoList == null)
			return -1;
		
		for (int i=0; i<_petPropertyInfoList.Count; ++i) {
			if (_petPropertyInfoList [i].petDto.id == petUID)
				return i;
		}
		
		return -1;
	}

	public int GetPetCount(){
		if(_petPropertyInfoList == null)
			return -1;

		return _petPropertyInfoList.Count;
	}

	public List<PetPropertyInfo> GetPetPropertyInfoList()
	{
		return _petPropertyInfoList;
	}

	public string GetPetTypeSprite(PetPropertyInfo petInfo){
		if(petInfo == null) return "";
		if(petInfo.pet.petType == Pet.PetType_Regular || petInfo.pet.petType == Pet.PetType_Rare){
			 if(petInfo.petDto.ifMutate)
				return "bianhua";     //变异
			else if(petInfo.petDto.ifBaobao)
				return "baby";		  //宝宝
			else
				return "nature";      //野生
		}else if(petInfo.pet.petType == Pet.PetType_Precious)
			return "zhenshou";        //珍兽
		else if(petInfo.pet.petType == Pet.PetType_Myth)
			return "shenshou";		  //神兽
		else
			return "";
	}

	public string GetPetTypeSprite(Pet pet){
		if(pet == null) return "";
		if(pet.petType == Pet.PetType_Precious)
			return "zhenshou";
		else if(pet.petType == Pet.PetType_Myth)
			return "shenshou";
		else
			return "";
	}

	public bool isFullBasePetAp(PetPropertyInfo petInfo){
		BaseAptitudeProperties petBaseApInfo = petInfo.petDto.baseAptitudeProperty;
		if(petBaseApInfo.attack == petBaseApInfo.maxAttack
		   && petBaseApInfo.defense == petBaseApInfo.maxDefense
		   && petBaseApInfo.physical == petBaseApInfo.maxPhysical
		   && petBaseApInfo.magic == petBaseApInfo.maxMagic
		   && petBaseApInfo.speed == petBaseApInfo.maxSpeed)
		{
			return true;
		}
		
		return false;
	}
	
	public float GetPetBaseApPercent(PetPropertyInfo petInfo,int curVal,int maxVal,int baseVal){
		float result = 0f;
		if(petInfo.petDto.ifMutate){
			result = (27.0f-(maxVal-curVal)*100.0f/baseVal)/27.0f;
		}
		else{
			result = (23.0f-(maxVal-curVal)*100.0f/baseVal)/23.0f;
		}
		result = Mathf.Max(result,0f);
		return result;
	}
	#endregion

	#region 请求接口
	public void ExpandPetSlot(int needCount,System.Action onSuccess){
		int propsId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_COMPANY_VACANCY_EXPAND_ITEM_ID,10036);
		int propCount = BackpackModel.Instance.GetItemCount(propsId);
		bool useIngot = propCount < needCount;
		ServiceRequestAction.requestServer(PetService.expandCarryCapacity(useIngot),"ExpandPetSlot",(e)=>{
			if(useIngot){
				H1Item itemInfo = DataCache.getDtoByCls<GeneralItem>(propsId) as H1Item;
				int needIngotCount =(needCount-propCount)*itemInfo.buyPrice;
				PlayerModel.Instance.UseIngot(needIngotCount);
			}

			_petOpenSlotCount +=1;
			if(onSuccess != null)
				onSuccess();
		});
	}

	public void DropPet(int petIndex){
		if(petIndex == -1) return;

		PetPropertyInfo petInfo = _petPropertyInfoList[petIndex];
		if(petInfo.pet.petType == Pet.PetType_Precious || petInfo.pet.petType == Pet.PetType_Myth)
		{
			TipManager.AddTip("珍兽和神兽不允许放生");
			return;
		}

		if(petIndex != _battlePetIndex){
			if(petInfo.petDto.name == petInfo.pet.name)
			{
				if(petInfo.petDto.joinedBattle && (petInfo.petDto.ifBaobao || petInfo.petDto.ifMutate)){
					ProxyWindowModule.OpenConfirmWindow(string.Format("确认要放生{0}吗？放生后可在仓库管理员处找回",
	                                                    petInfo.petDto.name.WrapColor("ff0000")),"",
					                                    ()=>{
						ServiceRequestAction.requestServer(PetService.dropPet(petInfo.petDto.id),"DropPet",(e) => {
							RemovePet(petIndex);
						});
					});
				}else{
					ProxyWindowModule.OpenConfirmWindow(string.Format("确认要放生{0}吗？放生后无法找回",
					                                    petInfo.petDto.name.WrapColor("ff0000")),"",
					                                    ()=>{
						ServiceRequestAction.requestServer(PetService.dropPet(petInfo.petDto.id),"DropPet",(e) => {
							RemovePet(petIndex);
						});
					});
				}
			}
			else
				TipManager.AddTip("需要将宠物改为默认的名称才能放生");
		}else
			TipManager.AddTip("该宠物处于参战状态，不能放生");
	}

	public void ChangePetName(long petUID,string newName){
		ServiceRequestAction.requestServer(PetService.rename(petUID,newName),"ChangePetName",(e) => {
			int petIndex = GetPetIndex(petUID);
			_petPropertyInfoList[petIndex].petDto.name = newName;
			if(OnPetInfoUpdate != null){
				OnPetInfoUpdate(petIndex);
			}
			ProxyPetPropertyModule.CloseChangeNameView();
		});
	}

	public void DeactiveBattlePet(){
		ServiceRequestAction.requestServer(PetService.rest(),"DeactiveBattlePet",(e) => {
			_battlePetIndex = -1;
			if(OnChangeBattlePet != null){
				OnChangeBattlePet(_battlePetIndex);
			}
		});
	}

	public void ChangeBattlePet(int index){
		PetPropertyInfo petInfo = GetPetInfoByIndex(index);
		if(petInfo.petDto.level <= PlayerModel.Instance.GetPlayerLevel()+10){
			ServiceRequestAction.requestServer(PetService.joinBattle(petInfo.petDto.id),"ChangeBattlePet",(e) => {
				_battlePetIndex = index;
				petInfo.petDto.joinedBattle = true;
				if(OnChangeBattlePet != null){
					OnChangeBattlePet(_battlePetIndex);
				}
			});
		}else
			TipManager.AddTip("宠物等级已经超过人物等级10级，不能参战");
	}

	public void ResetPet(long petUID,int ingotCost){
		ServiceRequestAction.requestServer(PetService.resetPet(petUID),"ResetPet",(e)=>{
			PetCharactorDto petDto = e as PetCharactorDto;
			if(petDto != null){
				PlayerModel.Instance.UseIngot(ingotCost);
				int petIndex = GetPetIndex(petUID);
				_petPropertyInfoList[petIndex].ResetPetDto(petDto);
				if(OnPetInfoUpdate != null)
					OnPetInfoUpdate(petIndex);
			}
		});
	}

	public void UsePetExpBook(long petUID,int itemId){
		ServiceRequestAction.requestServer(PetService.expBook(petUID),"UsePetExpBook");
	}

	//宠物洗点
	public void ResetPetAptitude(PetPropertyInfo petInfo,int itemIndex,int apType,int resetPoint){
		ServiceRequestAction.requestServer(PetService.resetPerAptitude(itemIndex,petInfo.petDto.id,apType),"ResetPetAptitude",(e)=>{
			if(apType == AptitudeProperties.AptitudeType_Constitution)
				petInfo.petDto.aptitudeProperties.constitution -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Intelligent)
				petInfo.petDto.aptitudeProperties.intelligent -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Strength)
				petInfo.petDto.aptitudeProperties.strength -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Stamina)
				petInfo.petDto.aptitudeProperties.stamina -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Dexterity)
				petInfo.petDto.aptitudeProperties.dexterity -= resetPoint;

			petInfo.petDto.potential +=resetPoint;
			PetModel.CalculatePetBp(petInfo);

			TipManager.AddTip(string.Format("{0}的{1}属性减掉了{2}点，潜力点增加{3}点",petInfo.pet.name.WrapColor(ColorConstant.Color_Tip_Item_Str),
			                                ItemHelper.AptitudeTypeName(apType).WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
			                                resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
			                                resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency_Str)));

			int petIndex = GetPetIndex(petInfo.petDto.id);
			if(OnPetInfoUpdate != null)
				OnPetInfoUpdate(petIndex);
		});
	}

	public void IncreaseBaseAptitude(PetPropertyInfo petInfo,int baseApType,System.Action onSuccess){
		ServiceRequestAction.requestServer(PetService.increaseBaseAptitude(petInfo.petDto.id,baseApType),"IncreaseBaseAptitude",(e)=>{
			PetIncreaseBaseAptitudeDto increaseInfo = e as PetIncreaseBaseAptitudeDto;
			if(increaseInfo.petUniqueId == petInfo.petDto.id){
				petInfo.IncreasePetBaseApPoint(increaseInfo.baseAptitudeGain,baseApType);

				TipManager.AddTip(string.Format("{0}的{1}增加了{2}点",
				                                petInfo.petDto.name.WrapColor(ColorConstant.Color_Tip_Item_Str),
				                                ItemHelper.PetBaseAptitudePropertyName(baseApType).WrapColor(ColorConstant.Color_Tip_Item_Str),
				                                increaseInfo.baseAptitudeGain.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency_Str)));

				int petIndex = GetPetIndex(petInfo.petDto.id);
				if(OnPetInfoUpdate != null)
					OnPetInfoUpdate(petIndex);

				if(onSuccess != null)
					onSuccess();
			}
		});
	}

	public void FightSkillBook(PetPropertyInfo petInfo,PackItemDto skillBookItem){
		ServiceRequestAction.requestServer(PetService.fightBook(petInfo.petDto.id,skillBookItem.uniqueId),"FightSkillBook",(e)=>{
			PetAddSkillDto addSkillDto = e as PetAddSkillDto;
			if(addSkillDto.replaceSkillId != 0){
				for(int i=0;i<petInfo.petDto.skillIds.Count;++i){
					if(petInfo.petDto.skillIds[i] == addSkillDto.replaceSkillId)
					{
						petInfo.petDto.skillIds[i]=addSkillDto.newSkillId;
						TipManager.AddTip(string.Format("{0}学会了新技能{1}，但遗忘了{2}",petInfo.petDto.name,addSkillDto.newSkill.name,addSkillDto.replaceSkill.name));
						return;
					}
				}
			}else{
				petInfo.petDto.skillIds.Add(addSkillDto.newSkillId);
				TipManager.AddTip(string.Format("{0}学会了新技能{1}",petInfo.petDto.name,addSkillDto.newSkill.name));
			}
			PetModel.CalculatePetRanking(petInfo);

			int petIndex = GetPetIndex(petInfo.petDto.id);
			if(OnPetInfoUpdate != null)
				OnPetInfoUpdate(petIndex);
		});
	}

	public void ResetPetAllAp(long petUID){
		ServiceRequestAction.requestServer(PetService.resetAptitude(petUID),"ResetPetAllAp",(e)=>{
			PetCharactorDto petDto = e as PetCharactorDto;
			if(petDto != null){
				int petIndex = GetPetIndex(petUID);
				_petPropertyInfoList[petIndex].ResetPetDto(petDto);
				if(OnPetInfoUpdate != null)
					OnPetInfoUpdate(petIndex);
			}
		});
	}

	public void WearPetEquipment(PetPropertyInfo petInfo,PackItemDto eqItemDto){
		PetEquipmentExtraDto newEqExtraDto = eqItemDto.extra as PetEquipmentExtraDto;
		PetEquipment newPetEqInfo = DataCache.getDtoByCls<GeneralItem>(newEqExtraDto.petEquipmentId) as PetEquipment;
		if(petInfo.petDto.level >= newPetEqInfo.wearableLevel){
			ProxyWindowModule.OpenConfirmWindow("宠物装备佩戴后将无法取下，但可使用其他同类装备覆盖，确认要使用吗？","",()=>{
				ServiceRequestAction.requestServer(EquipmentService.petWear(petInfo.petDto.id,eqItemDto.uniqueId),"WearPetEquipment",(e)=>{
					bool isExisted = false;
					for(int i=0;i<petInfo.petDto.petEquipments.Count;++i){
						PetEquipmentExtraDto petEqExtraDto = petInfo.petDto.petEquipments[i];
						PetEquipment petEq = DataCache.getDtoByCls<GeneralItem>(petEqExtraDto.petEquipmentId) as PetEquipment;
						if(petEq.petEquipPartType == newPetEqInfo.petEquipPartType){
							petInfo.petDto.petEquipments[i] = newEqExtraDto;
							isExisted = true;
						}
					}
					
					if(!isExisted)
						petInfo.petDto.petEquipments.Add(newEqExtraDto);

					CalculatePetBp(petInfo);
					CalculatePetRanking(petInfo);
					int petIndex = GetPetIndex(petInfo.petDto.id);
					if(OnPetInfoUpdate != null)
						OnPetInfoUpdate(petIndex);
				});
			},null,UIWidget.Pivot.Left);
		}else
			TipManager.AddTip("宠物等级不足");
	}

	public void AddLifeByProps(PackItemDto itemDto,PetPropertyInfo petInfo){
		ServiceRequestAction.requestServer(PetService.addLifeByProps(itemDto.index,petInfo.petDto.id),"AddPetLifeByProps",(e)=>{
			Props props = itemDto.item as Props;
			PropsParam_7 propsParam = props.propsParam as PropsParam_7;
			int maxPetLifePoint = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_MAX_LIFE,60000);

			petInfo.petDto.lifePoint += propsParam.lifePoint;
			if(petInfo.petDto.lifePoint + propsParam.lifePoint > maxPetLifePoint)
				petInfo.petDto.lifePoint = maxPetLifePoint;

			TipManager.AddTip(string.Format("你的{0}寿命增加到{1}",petInfo.pet.name.WrapColor(ColorConstant.Color_Tip_Item),petInfo.petDto.lifePoint.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency_Str)));
			int petIndex = GetPetIndex(petInfo.petDto.id);
			if(OnPetInfoUpdate != null)
				OnPetInfoUpdate(petIndex);
		});
	}

	public void ChangeMythOrPreciousPet(PetPropertyInfo petInfo){
		ServiceRequestAction.requestServer(PetService.refresh(petInfo.petDto.id),"ChangeMythOrPreciousPet",(e)=>{
			PetCharactorDto petDto = e as PetCharactorDto;
			if(petDto != null){
				int petIndex = GetPetIndex(petInfo.petDto.id);
				_petPropertyInfoList[petIndex].ResetPetDto(petDto);
				TipManager.AddTip(string.Format("恭喜！你获得了一个{0}",_petPropertyInfoList[petIndex].pet.name.WrapColor(ColorConstant.Color_Tip_Item_Str)));
				if(OnPetInfoUpdate != null)
					OnPetInfoUpdate(petIndex);
			}
		});
	}
	#endregion

	#region Game Logic
	public void Setup (List<PetCharactorDto> petDtoList,int openSlotCount)
	{
		_petOpenSlotCount = Mathf.Max(openSlotCount,DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_INIT_CARRY_CAPACITY,5));
		_petPropertyInfoList = new List<PetPropertyInfo> (petDtoList.Count);
		for (int i=0; i<petDtoList.Count; ++i) {
			PetCharactorDto petDto = petDtoList [i] as PetCharactorDto;
			PetPropertyInfo petInfo = new PetPropertyInfo (petDto);
			if (petDto.id == PlayerModel.Instance.GetPlayer ().battlePetUniqueId)
				_battlePetIndex = i;
			_petPropertyInfoList.Add (petInfo);
		}
	}

	public void ChangeBattlePetByUID(long petUID){
		int petIndex = GetPetIndex(petUID);
		if(petIndex != -1){
			_battlePetIndex = petIndex;
			_petPropertyInfoList[petIndex].petDto.joinedBattle = true;
			if(OnChangeBattlePet != null)
				OnChangeBattlePet(_battlePetIndex);
		}
	}

	public void AddPet (PetCharactorDto petDto)
	{
		PetPropertyInfo petInfo = new PetPropertyInfo (petDto);
		_petPropertyInfoList.Add (petInfo);
		if(OnPetInfoListUpdate != null)
			OnPetInfoListUpdate();
	}

	public void AddPet (PetPropertyInfo petInfo){
		_petPropertyInfoList.Add(petInfo);
		if(OnPetInfoListUpdate != null)
			OnPetInfoListUpdate();
	}

	public void RemovePet (int index)
	{
		if (index < _petPropertyInfoList.Count && index >= 0) {
			PetPropertyInfo battlePetInfo = GetBattlePetInfo();
			_petPropertyInfoList.RemoveAt (index);

			//重置battlePetIndex
			for(int i=0;i<_petPropertyInfoList.Count;++i){
				if(_petPropertyInfoList[i] == battlePetInfo)
				{
					_battlePetIndex = i;
					break;
				}
			}

			if(OnPetInfoListUpdate != null)
				OnPetInfoListUpdate();
		}
	}

	public void RemovePetByUID(long petUID)
	{
		int index = GetPetIndex(petUID);
		if(index != -1)
		{
			RemovePet(index);
		}
	}

	public void HandlePetAptitudeNofity(PetAptitudeNotify notify){
		int petIndex = GetPetIndex(notify.petUniqeId);
		if(petIndex == -1)
			return;

		PetPropertyInfo petInfo = _petPropertyInfoList[petIndex];
		if(petInfo != null){
			notify.value = Mathf.Max(notify.value,0);
			if(notify.aptitudeTypeId == BaseAptitudeProperties.BaseAptitudeType_Attack)
				petInfo.petDto.baseAptitudeProperty.attack = notify.value;
			else if(notify.aptitudeTypeId == BaseAptitudeProperties.BaseAptitudeType_Defense)
				petInfo.petDto.baseAptitudeProperty.defense = notify.value;
			else if(notify.aptitudeTypeId == BaseAptitudeProperties.BaseAptitudeType_Physical)
				petInfo.petDto.baseAptitudeProperty.physical = notify.value;
			else if(notify.aptitudeTypeId == BaseAptitudeProperties.BaseAptitudeType_Magic)
				petInfo.petDto.baseAptitudeProperty.magic = notify.value;
			else if(notify.aptitudeTypeId == BaseAptitudeProperties.BaseAptitudeType_Speed)
				petInfo.petDto.baseAptitudeProperty.speed = notify.value;

			TipManager.AddTip(string.Format("{0}的{1}变为{2}了",petInfo.pet.name.WrapColor(ColorConstant.Color_Tip_Item_Str),
			                                ItemHelper.PetBaseAptitudePropertyName(notify.aptitudeTypeId).WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
			                                notify.value.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str)));

			PetModel.CalculatePetBp(petInfo);
			PetModel.CalculatePetRanking(petInfo);
			if(OnPetInfoUpdate != null){
				OnPetInfoUpdate(petIndex);
			}
		}
	}

	public void HandlePetLifeNotify(PetLifeNotify notify){
		int petIndex = GetPetIndex(notify.petUniqeId);
		if(petIndex == -1)
			return;
		
		PetPropertyInfo petInfo = _petPropertyInfoList[petIndex];
		if(petInfo != null){
			petInfo.petDto.lifePoint = notify.lifePoint;
			if(OnPetInfoUpdate != null){
				OnPetInfoUpdate(petIndex);
			}
		}
	}

	public void UpdatePetExpInfo(CharactorExpInfoNotify expNotify, bool needTip = true){
		if (expNotify == null)
			return;

		PetPropertyInfo petInfo = null;
		int petIndex = GetPetIndex(expNotify.id);
		if(petIndex != -1){
			petInfo = _petPropertyInfoList[petIndex];
		}
		else{
//			Debug.LogError("UpdatePetExpInfo petInfo is null");
			return;
		}

		if(!expNotify.maxLevelReached)
		{
			if (needTip)
			{
				TipManager.AddTip(string.Format("{0}获得了{1}{2}",petInfo.petDto.name.WrapColor(ColorConstant.Color_Tip_Item),expNotify.expGain.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_PET_EXP)));
			}
			petInfo.petDto.exp = expNotify.simpleCharactorDto.exp;
			petInfo.petDto.level = expNotify.simpleCharactorDto.level;

			if(expNotify.upgarded){
				int lvCount = expNotify.simpleCharactorDto.level - expNotify.oldLevel;
				for(int levelIndex=0; levelIndex<lvCount; ++levelIndex){
					//每级默认各资质属性增加1点
					PlayerModel.AddApPoint (petInfo.petDto.aptitudeProperties, 1, 1, 1, 1, 1);

					//潜力点按自动加点方案分配或者自主分配
					petInfo.petDto.potential += DataHelper.GetStaticConfigValue (H1StaticConfigs.DISPOSABLE_POTENTIAL_POINT_GAIN_PER_UPGRADE, 5);
					PlayerModel.AutoDistrubuteApPoint (petInfo.petDto);
                }
				PetModel.CalculatePetBp(petInfo);
                
                if(OnPetGradeUpdate != null)
					OnPetGradeUpdate(petIndex);
			}

			if(OnPetExpUpdate != null)
				OnPetExpUpdate(petIndex);
		}
		else
		{
			if (needTip)
			{
				TipManager.AddTip(string.Format("{0}超过你的等级5级，无法获得{1}",petInfo.petDto.name.WrapColor(ColorConstant.Color_Tip_Item),ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_PET_EXP)));
			}
		}
	}

	public void Dispose ()
	{
		_petPropertyInfoList = null;
		_battlePetIndex = -1;
	}
	#endregion

	#region 宠物属性计算
	/// <summary>
	/// 计算宠物战斗力，用作宠物评分
	/// </summary>
	public static void CalculatePetRanking (PetPropertyInfo petInfo)
	{
		int totalApMark = 0;
		totalApMark += petInfo.petDto.baseAptitudeProperty.attack;
		totalApMark += petInfo.petDto.baseAptitudeProperty.defense;
		totalApMark += petInfo.petDto.baseAptitudeProperty.physical;
		totalApMark += petInfo.petDto.baseAptitudeProperty.magic;
		totalApMark += petInfo.petDto.baseAptitudeProperty.speed;

		int totalSkillMark = 0;
		for (int i=0; i<petInfo.petDto.skillIds.Count; ++i) {
			Skill skill = DataCache.getDtoByCls<Skill> (petInfo.petDto.skillIds [i]);
			if (skill != null)
				totalSkillMark += skill.ranking;
		}

		//计算装备技能
		for(int i=0;i<petInfo.petDto.petEquipments.Count;++i){
			PetEquipmentExtraDto eqExtraDto = petInfo.petDto.petEquipments[i];
			for(int j=0;j<eqExtraDto.petSkillIds.Count;++j){
				Skill skill = DataCache.getDtoByCls<Skill>(eqExtraDto.petSkillIds[i]);
				if(skill != null){
					totalSkillMark += skill.ranking;
				}
			}
		}

		petInfo.ranking = totalApMark + totalSkillMark + 5000;
	}

	public string GetPetRankingDesc (PetPropertyInfo petInfo)
	{
		if (petInfo.ranking > 15500)
			return "SS";
		else if (petInfo.ranking > 13500 && petInfo.ranking <= 15500)
			return "S";
		else if (petInfo.ranking > 12500 && petInfo.ranking <= 13500)
			return "A";
		else if (petInfo.ranking > 11500 && petInfo.ranking <= 12500)
			return "B";
		else if (petInfo.ranking > 10500 && petInfo.ranking <= 11500)
			return "C";
		else if (petInfo.ranking > 9500 && petInfo.ranking <= 10500)
			return "D";
		else
			return "E";
	}

	public string GetPetGrowthFlag (PetPropertyInfo petInfo)
	{
		Pet pet = petInfo.pet;
		if (pet == null)
			return "";

		if(pet.petType == Pet.PetType_Myth || pet.petType == Pet.PetType_Precious)
			return "GrowthFlag_red";

		float percent = (float)petInfo.petDto.growth / (float)pet.growth;
		if (percent > 1.01f)
			return "GrowthFlag_red";
		else if (percent > 1.00f && percent <= 1.01f)
			return "GrowthFlag_purple";
		else if (percent > 0.99f && percent <= 1.00f)
			return "GrowthFlag_orange";
		else if (percent > 0.98f && percent <= 0.99f)
			return "GrowthFlag_blue";
		else
			return "GrowthFlag_green";
	}

	public void UpdatePetAp (PetPropertyInfo petInfo,int potential, int constitution, int intelligent, int strength, int stamina, int dexterity)
	{
		petInfo.petDto.potential = potential;
		petInfo.petDto.aptitudeProperties.constitution = constitution;
		petInfo.petDto.aptitudeProperties.intelligent = intelligent;
		petInfo.petDto.aptitudeProperties.strength = strength;
		petInfo.petDto.aptitudeProperties.stamina = stamina;
		petInfo.petDto.aptitudeProperties.dexterity = dexterity;
		
		PetModel.CalculatePetBp(petInfo);
	}

	public static void CalculatePetBp (PetPropertyInfo petInfo)
	{
		int petLevel = petInfo.petDto.level;
		BaseAptitudeProperties petBaseAp = petInfo.petDto.baseAptitudeProperty;
		AptitudeProperties petApInfo = petInfo.petDto.aptitudeProperties;

		float hp = (petBaseAp.physical * petLevel * (5.0f + petLevel / 100.0f) + petApInfo.constitution * 5.0f * petInfo.petDto.growth) / 1000.0f + 90.0f;
		float mp = petLevel * 10.0f + petApInfo.intelligent * 2.0f + petApInfo.strength * 2.0f;
		float atk = ((petBaseAp.attack * petLevel * 2.0f * (petInfo.petDto.growth / 2000.0f + 0.7f) + petApInfo.strength * 0.75f * petInfo.petDto.growth) / 1000.0f + 50.0f) * 4.0f / 3.0f;
		float def = (petBaseAp.defense * petLevel * 1.75f * (petInfo.petDto.growth / 2000.0f + 0.7f) + petApInfo.stamina * 1.5f * petInfo.petDto.growth) / 1000.0f;
		float speed = petBaseAp.speed * (petApInfo.constitution * 0.1f + petApInfo.intelligent * 0.1f + petApInfo.strength * 0.1f + petApInfo.stamina * 0.1f + petApInfo.dexterity * 0.7f) / 1000.0f * (petInfo.petDto.growth / 2000.0f + 0.5f);
		float magic = (petApInfo.constitution * 0.1f + petApInfo.intelligent * 0.7f + petApInfo.strength * 0.4f + petApInfo.stamina * 0.1f) * (petInfo.petDto.growth / 2000.0f + 0.4f) + petBaseAp.magic / 1000.0f * petLevel;

		//计算宠物装备属性
		for(int i=0;i<petInfo.petDto.petEquipments.Count;++i){
			PetEquipmentExtraDto petEqExtraDto = petInfo.petDto.petEquipments[i];
			if(petEqExtraDto.battleBaseProperty != null){
				if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Hp)
					hp += petEqExtraDto.battleBaseProperty.value;
				else if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Mp)
					mp += petEqExtraDto.battleBaseProperty.value;
				else if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Attack)
					atk += petEqExtraDto.battleBaseProperty.value;
				else if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Defense)
					def += petEqExtraDto.battleBaseProperty.value;
				else if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Speed)
					speed += petEqExtraDto.battleBaseProperty.value;
				else if(petEqExtraDto.battleBaseProperty.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Magic)
					magic += petEqExtraDto.battleBaseProperty.value;
			}
		}

		petInfo.hp = Mathf.FloorToInt(hp);
		petInfo.mp = Mathf.FloorToInt(mp);
		petInfo.attack = Mathf.FloorToInt(atk);
		petInfo.defense = Mathf.FloorToInt(def);
		petInfo.speed = Mathf.FloorToInt(speed);
		petInfo.magic = Mathf.FloorToInt(magic);
	}
	#endregion

	#region 宠物图鉴
	private List<Pet> _regularPetInfoList;
	private List<Pet> _mythPetInfoList;

	private void InitPetInfoList(){
		_regularPetInfoList = new List<Pet>(40);
		_mythPetInfoList = new List<Pet>(10);
		List<GeneralCharactor> charctorList = DataCache.getArrayByCls<GeneralCharactor>();
		for(int i=0;i<charctorList.Count;++i){
			if(charctorList[i] is Pet){
				Pet pet = charctorList[i] as Pet;
				if(pet.petType == Pet.PetType_Regular || pet.petType == Pet.PetType_Rare)
					_regularPetInfoList.Add(pet);
				else if(pet.petType == Pet.PetType_Precious || pet.petType == Pet.PetType_Myth)
					_mythPetInfoList.Add(pet);
			}
		}
	}

	public List<Pet> GetRegularPetInfoList(){
		if(_regularPetInfoList == null){
			InitPetInfoList();
		}
		int playerLevel = PlayerModel.Instance.GetPlayerLevel();
		int canShowPetLevel = (Mathf.FloorToInt(playerLevel / 5f)+2)*5;
		List<Pet> result = new List<Pet>(_regularPetInfoList.Count);
		for(int i=0;i<_regularPetInfoList.Count;++i){
			if(_regularPetInfoList[i].companyLevel <= canShowPetLevel)
				result.Add(_regularPetInfoList[i]);
		}

		return result;
	}

	public List<Pet> GetMythPetInfoList(){
		if(_mythPetInfoList == null){
			InitPetInfoList();
		}
		return _mythPetInfoList;
	}
	#endregion
}
