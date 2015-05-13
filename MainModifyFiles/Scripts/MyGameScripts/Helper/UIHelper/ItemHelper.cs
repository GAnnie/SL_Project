// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemHelper.cs
// Author   : willson
// Created  : 2015/1/27 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.battlebuff.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;

public class ItemHelper
{
	public static PackItemDto ItemDtoToPackItemDto(ItemDto dto)
	{
		return ItemIdToPackItemDto(dto.itemId,dto.itemCount);
	}

	public static PackItemDto ItemIdToPackItemDto(int itemId,int count = 1)
	{
		GeneralItem itemInfo = DataCache.getDtoByCls<GeneralItem>(itemId);
		if(itemInfo == null) 
			return null;

		return H1ItemToPackItemDto(itemInfo,count);
	}

	public static PackItemDto PetEqExtraDtoToPackItemDto(PetEquipmentExtraDto extra){
		PackItemDto dto = new PackItemDto();
		dto.itemId = extra.petEquipmentId;
		dto.extra = extra;
		return dto;
	}

	public static PackItemDto H1ItemToPackItemDto(GeneralItem info,int count = 1)
	{
		PackItemDto dto = new PackItemDto();
		if(info is H1Item)
		{
			dto.circulationType = (info as H1Item).circulationType;
		}
		dto.itemId = info.id;
		dto.count = count;
		return dto;
	}

	public static PackItemDto GetOneInPackItemDto(PackItemDto itemDto)
	{
		PackItemDto dto = new PackItemDto();
		dto.circulationType = itemDto.circulationType;
		dto.itemId = itemDto.itemId;
		dto.count = 1;
		dto.index = itemDto.index;
		return dto;
	}

	public static int GetEquipGemLv(PackItemDto equipDto)
	{
		int gemLv = 0;
		EquipmentExtraDto extraDto = equipDto.extra as EquipmentExtraDto;
		if(extraDto != null)
		{
			EquipmentEmbedInfo embedInfo = extraDto.equipmentEmbedInfo;
			if(embedInfo != null && embedInfo.embedLevels != null && embedInfo.embedLevels.Count > 0)
			{
				for(int index = 0;index < embedInfo.embedLevels.Count;index++)
				{
					gemLv += embedInfo.embedLevels[index];
				}
			}
		}
		return gemLv;
	}

	public static string WeaponTypeName(int mainCharactorId)
	{
		switch(mainCharactorId)
		{
		case 1:
			return "利剑";
		case 2:
			return "双刀";
		case 3:
			return "扇子";
		case 4:
			return "法杖";
		case 5:
			return "枪戟";
		case 6:
			return "双环";
		default:
			return "未知";
		}
	}

	/*
	public static string EquipmentPartName(int partType)
	{
		switch(partType)
		{
		case Equipment.EquipPartType_Weapon:
			return "武器";
		case Equipment.EquipPartType_Helmet:
			return "头盔";
		case Equipment.EquipPartType_Armor:
			return "铠甲";
		case Equipment.EquipPartType_Necklace:
			return "项链";
		case Equipment.EquipPartType_Girdle:
			return "腰带";
		case Equipment.EquipPartType_Shoe:
			return "鞋子";
		default:
			return "未知";
		}
	}
	*/

	public static string EquipmentPartName(Equipment equip)
	{
		switch(equip.equipPartType)
		{
		case Equipment.EquipPartType_Weapon:
			return WeaponTypeName(equip.mainCharactorId);
		case Equipment.EquipPartType_Helmet:
			return equip.equipSexType == PlayerDto.Gender_Female? "女帽":"男帽";
		case Equipment.EquipPartType_Armor:
			return equip.equipSexType == PlayerDto.Gender_Female? "女衣":"男衣";
		case Equipment.EquipPartType_Necklace:
			return "项链";
		case Equipment.EquipPartType_Girdle:
			return "腰带";
		case Equipment.EquipPartType_Shoe:
			return equip.equipSexType == PlayerDto.Gender_Female? "女鞋":"男鞋";
		default:
			return "未知";
		}
	}

	public static string PetEqPartName(PetEquipment petEq){
		switch(petEq.petEquipPartType){
		case PetEquipment.PetEquipPartType_Chaplet:
			return "项圈";
		case PetEquipment.PetEquipPartType_Armor:
			return "装甲";
		case PetEquipment.PetEquipPartType_Amulet:
			return "护符";
		default:
			return "未知";
		}
	}

	public static string AptitudeTypeName(int aptitudeType)
	{
		switch(aptitudeType)
		{
		case AptitudeProperties.AptitudeType_Constitution:
			return "体质";
		case AptitudeProperties.AptitudeType_Intelligent:
			return "魔力";
		case AptitudeProperties.AptitudeType_Strength:
			return "力量";
		case AptitudeProperties.AptitudeType_Stamina:
			return "耐力";
		case AptitudeProperties.AptitudeType_Dexterity:
			return "敏捷";
		case AptitudeProperties.MP_TYPE:
			return "魔法";
		default:
			return "未知";
		}
	}

	public static string PetBaseAptitudePropertyName(int aptitudeType){
		switch(aptitudeType)
		{
		case BaseAptitudeProperties.BaseAptitudeType_Attack:
			return "攻击资质";
		case BaseAptitudeProperties.BaseAptitudeType_Defense:
			return "防御资质";
		case BaseAptitudeProperties.BaseAptitudeType_Physical:
			return "体力资质";
		case BaseAptitudeProperties.BaseAptitudeType_Magic:
			return "法力资质";
		case BaseAptitudeProperties.BaseAptitudeType_Speed:
			return "速度资质";
		default:
			return "未知";
		}
	}

	public static string BattleBasePropertyTypeName(int battleBasePropertyType)
	{
		switch(battleBasePropertyType)
		{
		case BattleBuff.BattleBasePropertyType_Hp:
			return "气血";
		case BattleBuff.BattleBasePropertyType_Speed:
			return "速度";
		case BattleBuff.BattleBasePropertyType_Attack:
			return "攻击";
		case BattleBuff.BattleBasePropertyType_Defense:
			return "防御";
		case BattleBuff.BattleBasePropertyType_CritRate:
			return "暴击率";
		case BattleBuff.BattleBasePropertyType_HitRate:
			return "命中率";
		case BattleBuff.BattleBasePropertyType_DodgeRate:
			return "闪避率";
		case BattleBuff.BattleBasePropertyType_Mp:
			return "魔法";
		case BattleBuff.BattleBasePropertyType_MagicCritRate:
			return "法术暴击率";
		case BattleBuff.BattleBasePropertyType_MagicHitRate:
			return "法术命中率";
		case BattleBuff.BattleBasePropertyType_MagicDodgeRate:
			return "法术闪避率";
		case BattleBuff.BattleBasePropertyType_Magic:
			return "灵力";
        case BattleBuff.BattleBasePropertyType_MagicAttack:
            return "法攻";
        case BattleBuff.BattleBasePropertyType_MagicDefense:
            return "法防";
		default:
			return "未知";
		}
	}
}