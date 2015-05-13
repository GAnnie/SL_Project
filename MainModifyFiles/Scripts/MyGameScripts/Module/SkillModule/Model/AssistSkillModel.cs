// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillModel.cs
// Author   : willson
// Created  : 2015/4/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.assistskill.data;
using System.Collections.Generic;


public class AssistSkillVo
{
	public AssistSkillVo(AssistSkillDto dto)
	{
		this.dto = dto;
		AssistSkill skill = DataCache.getDtoByCls<AssistSkill>(dto.id);

		name = skill.name;
		id = dto.id;
		level = dto.level;

		acquireLevel = skill.levelLimit;

		icon = skill.icon;

		memo = skill.memo;
	}

	public AssistSkillVo(FactionSkillDto dto)
	{
		this.dto = dto;
		name = dto.factionSkill.name;

		if(!string.IsNullOrEmpty(dto.factionSkill.skillsFormula))
		{
			string[] subSkillInfo  = dto.factionSkill.skillsFormula.Split(':');
			id = int.Parse(subSkillInfo[0]);
			acquireLevel = int.Parse(subSkillInfo[1]);
			icon = subSkillInfo[0];
		}

		level = dto.factionSkillLevel;

		memo = dto.factionSkill.description;
	}

	public string name;

	public int id;
	
	public int level;

	public int acquireLevel;

	public string icon;

	public string memo;

	public bool IsShow()
	{
		return level >= acquireLevel;
	}

	public object dto;
}

public class AssistSkillModel
{
	private static readonly AssistSkillModel instance = new AssistSkillModel ();

	public event System.Action<AssistSkillDto> OnUpdateAssistSkill;
	public event System.Action OnUpdateAll;

	public static AssistSkillModel Instance {
		get {
			return instance;
		}
	}

	private List<AssistSkillDto> _assistSkills;

	private AssistSkillModel ()
	{
	}

	public void Setup(PlayerAssistSkillInfoDto dto)
	{
		_assistSkills = dto.assistSkills;
	}

	public void UpDate(AssistSkillDto dto)
	{
		for(int index = 0;index < _assistSkills.Count;index++)
		{
			if(_assistSkills[index].id == dto.id)
			{
				if(_assistSkills[index].level < _assistSkills[index].assistSkill.levelLimit
				   && dto.level >= dto.assistSkill.levelLimit)
				{
					_assistSkills[index] = dto;

					if(OnUpdateAll != null)
						OnUpdateAll();
				}
				else if(_assistSkills[index].level >= _assistSkills[index].assistSkill.levelLimit)
				{
					_assistSkills[index] = dto;

					if(OnUpdateAssistSkill != null)
						OnUpdateAssistSkill(dto);
				}
				else
				{
					_assistSkills[index] = dto;
				}
				return;
			}
		}

		_assistSkills.Add(dto);
		_assistSkills.Sort(delegate(AssistSkillDto lhs, AssistSkillDto rhs) {
			return lhs.id.CompareTo(rhs.id);
		});


	}
	
	public List<AssistSkillVo> GetAssistSkills()
	{
		List<AssistSkillVo> skills = new List<AssistSkillVo>(); 
		FactionSkillDto dto = FactionSkillModel.Instance.GetFactionAssistSkill();

		if(dto != null)
		{
			skills.Add(new AssistSkillVo(dto));
		}

		if(_assistSkills != null && _assistSkills.Count > 0)
		{
			for(int index = 0;index < _assistSkills.Count;index++)
			{
				skills.Add(new AssistSkillVo(_assistSkills[index]));
			}
		}
		return skills;
	}

	public List<AssistSkillDto> GetLearnAssistSkills()
	{
		if(_assistSkills.Count == AssistSkill.AssistSkillEnum_Escape)
			return _assistSkills;

		//List<AssistSkillDto> skills = new List<AssistSkillDto>();

		for(int index = 0;index < AssistSkill.AssistSkillEnum_Escape;index++)
		{
			if(_assistSkills != null)
			{
				if(_assistSkills.Find(x => x.id == (index + 1)) == null)
				{
					AssistSkillDto dto = new AssistSkillDto();
					dto.id = index + 1;
					dto.level = 0;
					_assistSkills.Add(dto);
				}
			}
			else
			{
				AssistSkillDto dto = new AssistSkillDto();
				dto.id = index + 1;
				dto.level = 0;
				_assistSkills.Add(dto);
			}
		}

		_assistSkills.Sort(delegate(AssistSkillDto lhs, AssistSkillDto rhs) {
			return lhs.id.CompareTo(rhs.id);
		});

		return _assistSkills;
	}
}
