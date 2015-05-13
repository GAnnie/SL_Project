using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.battle.data;

public class PetEquipmentTipsViewController : ItemTipsViewController {

	protected override void ShowTips ()
	{
		PetEquipment item = _dto.item as PetEquipment;
		AddIntroductionlbl(item);
		AddPropertylbl(item);
		AddDescriptionlbl(item);
		HandleOpt();
	}
	
	private void AddIntroductionlbl(PetEquipment item)
	{
		string introduction = string.Format("类型:{0}\n",ItemHelper.PetEqPartName(item));
		introduction += string.Format("等级:{0}",item.wearableLevel);
		AddDecLbl().text = introduction.WrapColor(ColorConstant.Color_UI_Tab_Str);
	}
	
	private void AddPropertylbl(PetEquipment item)
	{
		PetEquipmentExtraDto extra = _dto.extra as PetEquipmentExtraDto;
		if(extra != null)
		{
			AddSpace();
			UILabel propertylbl = AddDecLblWithTitle("装备属性");

			if(item.petEquipPartType != PetEquipment.PetEquipPartType_Amulet){
				/** 战斗属性 */
				if(extra.battleBaseProperty != null)
					PropertyStr(propertylbl,ItemHelper.BattleBasePropertyTypeName(extra.battleBaseProperty.battleBasePropertyType),extra.battleBaseProperty.value);
				else
					propertylbl.text +="空";
			}else{
				if(extra.petSkillIds != null && extra.petSkillIds.Count>0){
					for(int i=0;i<extra.petSkillIds.Count;++i){
						Skill skill = DataCache.getDtoByCls<Skill>(extra.petSkillIds[i]);
						if(skill != null){
							propertylbl.text += string.Format("[{0}]\n",skill.name);
						}
					}
				}else
					propertylbl.text +="空";
			}
		}
	}
	
	private void PropertyStr(UILabel propertylbl,string name,int value)
	{
		if(value != 0)
		{
			if(value > 0)
			{
				propertylbl.text += (name + "+" + value + " ").WrapColor(ColorConstant.Color_Channel_Guild_Str);;
			}
			else
			{
				propertylbl.text += (name + value + " ").WrapColor(ColorConstant.Color_Channel_Guild_Str);;
			}
		}
	}
	
	private void AddDescriptionlbl(PetEquipment item)
	{
		if(!string.IsNullOrEmpty(item.description))
		{
			AddSpace();
			AddDecLbl().text = item.description.WrapColor(ColorConstant.Color_UI_Tab_Str);
		}
	}
	
	private void HandleOpt()
	{
		Llbl.text = "合成";
		Rlbl.text = "装备";
	}
}
