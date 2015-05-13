using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class PetLookInfo
{
	public int model = 0;
	public int texture = 0;
	public int mutateTexture = 0;
	public string mutateColor = "";
	public int wpModel = 0;
	
	public static PetLookInfo ToInfo(GeneralCharactor charactor)
	{
		PetLookInfo info = new PetLookInfo ();
		info.model = charactor.modelId;
		info.texture = charactor.texture;
		
		if (charactor is Pet)
		{
			Pet pet = charactor as Pet;
			info.mutateTexture = pet.mutateTexture;
			info.mutateColor = pet.mutateColor;
			
			//			if (pet.id == 2170)
			//			{
			//				info.mutateTexture = 0;
			//				info.colorParams = "0,0,0;0,0,0.6;0,0,0";
			//			}
		}
		
		return info;
	}

	public static PetLookInfo ToInfo(VideoSoldier soldier, NpcAppearanceDto npcAppearanceDto)
	{
		PetLookInfo info = PetLookInfo.ToInfo(soldier);
		if (npcAppearanceDto != null)
		{
			info.wpModel = npcAppearanceDto.wpmodel;
			info.mutateColor = npcAppearanceDto.mutateColor;
			info.mutateTexture = npcAppearanceDto.mutateTexture;
			info.model = npcAppearanceDto.modelId;
			info.texture = npcAppearanceDto.modelId;
		}
		return info;
	}

	public static PetLookInfo ToInfo(VideoSoldier soldier)
	{
		PetLookInfo info = new PetLookInfo ();
		if (soldier.charactor != null)
		{
			info = ToInfo(soldier.charactor);
			if (soldier.mutate == false)
			{
				info.mutateTexture = 0;
				info.mutateColor = "";
			}
			info.mutateColor = PlayerModel.Instance.GetDyeColorParams(soldier.hairDyeId,soldier.dressDyeId,soldier.accoutermentDyeId);
			
			info.wpModel = soldier.wpmodel;
		}
		else
		{
			if (soldier.monster.modelId > 0)
			{
				info.model = soldier.monster.modelId;
			}
			else if (soldier.monster.pet != null)
			{
				info.model = soldier.monster.pet.modelId;
			}
			
			if (soldier.monster.texture > 0)
			{
				info.texture = soldier.monster.texture;
			}
			else if (soldier.monster.pet != null)
			{
				info.texture = soldier.monster.pet.texture;
			}
			
			if (soldier.monster.mutateTexture > 0)
			{
				info.mutateTexture = soldier.monster.mutateTexture;
			}
			else if (soldier.monster.pet != null)
			{
				info.mutateTexture = soldier.monster.pet.mutateTexture;
			}
			
			if (soldier.monster.mutateColor != "")
			{
				info.mutateColor = soldier.monster.mutateColor;
			}
			else if (soldier.monster.pet != null)
			{
				info.mutateColor = soldier.monster.pet.mutateColor;
			}
			
			info.wpModel = soldier.monster.wpmodel;
			
			if (soldier.monsterType != Monster.MonsterType_Mutate)
			{
				info.mutateTexture = 0;
				info.mutateColor = "";
			}
		}
		return info;
	}
}
