// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FactionSKillModel.cs
// Author   : willson
// Created  : 2014/12/27 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.faction.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.battle.data;

public class FactionSkillModel
{
	private static readonly FactionSkillModel instance = new FactionSkillModel ();
	
	public static FactionSkillModel Instance {
		get {
			return instance;
		}
	}
	
	private FactionSkillModel ()
	{
	}

	private FactionSkillsInfoDto _dtos = null;

	public void Setup(FactionSkillsInfoDto dto){
		_dtos = dto;
	}

	public List<FactionSkillDto> GetFactionSkillDtos()
	{
		return _dtos.factionSkills;
	}

	public void UpDate(FactionSkillDto dto)
	{
		for(int index = 0;index < _dtos.factionSkills.Count;index++)
		{
			if(_dtos.factionSkills[index].factionSkillId == dto.factionSkillId)
			{
				_dtos.factionSkills[index] = dto;
			}
		}
	}

	public FactionSkillDto GetFactionSkill(int skillId)
	{
		for(int index = 0;index < _dtos.factionSkills.Count;index++)
		{
			if(_dtos.factionSkills[index].factionSkillId == skillId)
			{
				return _dtos.factionSkills[index];
			}
		}
		return null;
	}

	public int GetFactionSkillLevel(int skillId)
	{
		for(int index = 0;index < _dtos.factionSkills.Count;index++)
		{
			if(_dtos.factionSkills[index].factionSkillId == skillId)
			{
				return _dtos.factionSkills[index].factionSkillLevel;
			}
		}
		return 0;
	}

	public FactionSkillDto GetFactionAssistSkill()
	{
		for(int index = 0;index < _dtos.factionSkills.Count;index++)
		{
			if(_dtos.factionSkills[index].factionSkillId == PlayerModel.Instance.GetPlayer().faction.assistFactionSkillId)
			{
				return _dtos.factionSkills[index];
			}
		}
		return null;
	}

	public bool IsFactionAssistSkill(FactionSkillDto dto)
	{
		return dto.factionSkillId == PlayerModel.Instance.GetPlayer().faction.assistFactionSkillId;
	}
}