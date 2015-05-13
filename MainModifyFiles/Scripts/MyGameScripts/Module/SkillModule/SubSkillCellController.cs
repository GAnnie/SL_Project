// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SubSkillCellController.cs
// Author   : willson
// Created  : 2014/12/19 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class SubSkillCellController : MonoBehaviourBase,IViewController
{
	private SubSkillCell _view;
	private SkillInfo _skillInfo;
	private FactionSkillDto _factionSkillDto;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<SubSkillCell>();
		_view.Setup(this.transform);
		
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set (_view.SubSkillCellBtn.onClick, OnCellClick);
	}

	public void SetData(SkillInfo skillInfo,FactionSkillDto factionSkillDto)
	{
		this.gameObject.SetActive(true);
		_view.NameLabel.text = skillInfo.skill.name;
        _view.IconSprite.spriteName = skillInfo.skill.icon.ToString();
		//_view.TypeLabel.text = factionSkill.shortDesc;

		_skillInfo = skillInfo;
		_factionSkillDto = factionSkillDto;
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void OnCellClick()
	{
		if(!FactionSkillModel.Instance.IsFactionAssistSkill(_factionSkillDto))
		{
			ProxySkillModule.ShowTips(_skillInfo,this.gameObject);
		}
		else
		{
			ProxySkillModule.OpenAssistSkillFactionWin(_factionSkillDto);
		}
	}

	public void Dispose()
	{
	}
}