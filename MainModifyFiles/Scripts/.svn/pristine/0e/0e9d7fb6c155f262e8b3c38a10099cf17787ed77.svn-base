// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentTipGroup.cs
// Author   : willson
// Created  : 2015/2/3 
// Porpuse  : 
// **********************************************************************

using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.charactor.model;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.data;

public class EquipmentTipGroup : ItemBaseTipGroup
{
	override protected void initUI(PackItemDto itemDto)
	{
		Equipment item = _dto.item as Equipment;

		addName(itemDto);
		addSpaceHeight();

		AddIntroductionlbl(item);
		addSpaceHeight();

		AddPropertylbl(item);
		AddGemlbl(item);
        AddBufflbl(item);

		addSpaceHeight();
		AddDescriptionlbl(item);
	}

	protected void AddIntroductionlbl(Equipment item)
	{
		if(item.equipPartType == Equipment.EquipPartType_Weapon)
		{
            addLabel(string.Format("类型:{0}", ItemHelper.WeaponTypeName(item.mainCharactorId)), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
            addLabel(string.Format("等级:{0}", item.equipLevel), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
            addLabel(string.Format("适用:{0}", item.mainCharactor.name), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}
		else
		{
            addLabel(string.Format("类型:{0}", ItemHelper.EquipmentPartName(item)), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
            addLabel(string.Format("等级:{0}", item.equipLevel), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}
	}
	
	protected void AddPropertylbl(Equipment item)
	{
		EquipmentExtraDto extra = _dto.extra as EquipmentExtraDto;
		if(extra != null)
		{
			string propertyStr = "";
            addLabel("装备属性", _isDeepBg ? ColorConstant.Color_Channel_Team_Str : ColorConstant.Color_UI_Title_Str);
			if(!extra.hasIdentified)
			{
                addLabel("  未鉴定", _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_Title_Str);
			}
			else
			{
				/** 战斗属性 */
				if(extra.battleBaseProperties != null)
				{
					PropertyStr(ref propertyStr,"速度",extra.battleBaseProperties.speed);
					PropertyStr(ref propertyStr,"攻击",extra.battleBaseProperties.attack);
					PropertyStr(ref propertyStr,"防御",extra.battleBaseProperties.defense);
					PropertyStr(ref propertyStr,"气血",extra.battleBaseProperties.hp);
					PropertyStr(ref propertyStr,"魔法",extra.battleBaseProperties.mp);
					PropertyStr(ref propertyStr,"灵力",extra.battleBaseProperties.magic);
					//PropertyStr(ref propertyStr,"最大气血",extra.battleBaseProperties.maxHp);
					//PropertyStr(ref propertyStr,"最大法力",extra.battleBaseProperties.maxMp);
				}
				
				/** 资质属性 */
				if(extra.aptitudeProperties != null && extra.aptitudeProperties.Count > 0)
				{
					propertyStr += "\n";

					List<EquipmentAptitude> aptitudeProperties = EquipmentTipGroup.MergeAptitudeProperties(extra.aptitudeProperties);
					for(int index = 0;index < aptitudeProperties.Count;index++)
					{
						PropertyStr(ref propertyStr,ItemHelper.AptitudeTypeName(aptitudeProperties[index].aptitudeType),aptitudeProperties[index].value);
					}
				}

                /** 特技 */
                if (extra.activeSkillIds != null && extra.activeSkillIds.Count > 0)
                {
                    propertyStr += "\n";
                    List<string> skills = new List<string>();
                    for (int index = 0; index < extra.activeSkillIds.Count; index++)
                    {
                        EquipmentSkill skill = DataCache.getDtoByCls<Skill>(extra.activeSkillIds[index]) as EquipmentSkill;
                        if (skill != null)
                        {
                            string str = string.Format("特技：[{0}]", skill.name).WrapColor(ColorConstant.Color_Equip_Skill_Str) + skill.description.WrapColor(ColorConstant.Color_Channel_Guild_Str);
                            skills.Add(str);
                        }
                    }
                    propertyStr += string.Join("\n", skills.ToArray());
                }

				propertyStr += "\n";
				propertyStr += string.Format("耐久:{0}",extra.duration);
			}
			
			if(!string.IsNullOrEmpty(extra.smithee))
			{
				propertyStr += "\n";
				propertyStr += string.Format("制作人:{0}",extra.smithee);
			}

			addLabel(propertyStr,"0882D6");
		}
	}


	protected void AddGemlbl(Equipment item)
	{
		EquipmentExtraDto extraDto = _dto.extra as EquipmentExtraDto;
		if(extraDto != null)
		{
			if(extraDto.equipmentEmbedInfo != null 
			   && extraDto.equipmentEmbedInfo.embedItemIds != null 
			   && extraDto.equipmentEmbedInfo.embedItemIds.Count > 0)
			{
				addLabel("宝石属性","6F3E1A");
				string propertyStr = "";
				string gemName = "镶嵌 ";
				int lv = 0;
				for(int index = 0;index < extraDto.equipmentEmbedInfo.embedItemIds.Count;index++)
				{
					if(index < extraDto.equipmentEmbedInfo.embedLevels.Count)
					{
						int gemId = extraDto.equipmentEmbedInfo.embedItemIds[index];
						int gemLv = extraDto.equipmentEmbedInfo.embedLevels[index];
						lv += gemLv;

						Props gem = DataCache.getDtoByCls<GeneralItem>(gemId) as Props;
						gemName += gem.name + "、";

						PropsParam_3 param = gem.propsParam as PropsParam_3;

						int value = 0;
						if(param.battleBasePropertyValues[0] != AptitudeProperties.USE_ILV)
						{
							value = param.battleBasePropertyValues[0]*gemLv;
						}
						else
						{
							value = item.equipLevel*gemLv;
						}

						// 属性+10 属性+10 锻造等级x 镶嵌 宝石名字、宝石名字

						if(value > 0)
						{
							propertyStr += ItemHelper.BattleBasePropertyTypeName(param.battleBasePropertyTypes[0]) + "+" + value + " ";
						}
						else
						{
							propertyStr += ItemHelper.BattleBasePropertyTypeName(param.battleBasePropertyTypes[0]) + value + " ";
						}
					}
				}

				propertyStr += "锻造等级" + lv + " ";
				if(!string.IsNullOrEmpty(gemName))
				{
					gemName = gemName.Substring(0,gemName.Length - "、".Length);
					propertyStr += gemName;
				}
                addLabel(propertyStr, ColorConstant.Color_Channel_Guild_Str);
			}
		}
	}

    private void AddBufflbl(Equipment item)
    {

    }

	private void PropertyStr(ref string propertyStr,string name,int value)
	{
		if(value != 0)
		{
			if(value > 0)
			{
				propertyStr += name + "+" + value + " ";
			}
			else
			{
				propertyStr += name + value + " ";
			}
		}
	}

	protected void AddDescriptionlbl(Equipment item)
	{
		if(!string.IsNullOrEmpty(item.description))
		{
            addLabel(item.description, _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}
	}

	public static List<EquipmentAptitude> MergeAptitudeProperties(List<EquipmentAptitude> aptitudeProperties)
	{
		if(aptitudeProperties != null && aptitudeProperties.Count > 1)
		{
			List<EquipmentAptitude> list = new List<EquipmentAptitude>();
			for(int index = 0;index < aptitudeProperties.Count;index++)
			{
				bool hasIt = false;
				for(int i = 0;i < list.Count;i++)
				{
					if(list[i].aptitudeType == aptitudeProperties[index].aptitudeType)
					{
						list[i].value += aptitudeProperties[index].value;
						hasIt = true;
						break;
					}
				}

				if(!hasIt)
				{
					EquipmentAptitude aptitude = new EquipmentAptitude();
					aptitude.aptitudeType = aptitudeProperties[index].aptitudeType;
					aptitude.value = aptitudeProperties[index].value;
					list.Add(aptitude);
				}
			}
			return list;
		}
		else
		{
			return aptitudeProperties;
		}
	}
}