// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillLCellController.cs
// Author   : willson
// Created  : 2015/4/7 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules;

public class AssistSkillLCellController : MonoBehaviourBase,IViewController
{
	private AssistSkillLCell _view;

	private AssistSkillDto _dto;
	private System.Action<AssistSkillLCellController> _OnSelectCallBack;

	private int _spellMaxLv;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillLCell>();
		_view.Setup(this.transform);
		SetSelect(false);
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set (_view.SelectBtn.onClick, OnSelectCell);
	}

	public void SetData(AssistSkillDto dto,System.Action<AssistSkillLCellController> OnSelectCallBack)
	{
		_OnSelectCallBack = OnSelectCallBack;
		
		_dto = dto;
		_view.NameLabel.text = _dto.assistSkill.name;
		_view.TypeLabel.text = _dto.assistSkill.description;
		
		_spellMaxLv = PlayerModel.Instance.GetPlayerLevel() + 10;
		if(_spellMaxLv > DataHelper.GetStaticConfigValue(H1StaticConfigs.ASSIST_SKILL_MAX_LEVEL))
		{
			_spellMaxLv = DataHelper.GetStaticConfigValue(H1StaticConfigs.ASSIST_SKILL_MAX_LEVEL);
		}
		_view.LvLabel.text = _dto.level + "/" + _spellMaxLv;
	}

	public bool isMax()
	{
		return _dto.level >= _spellMaxLv;// && _dto.exp >= _dto.nextLevelExp;
	}

	public AssistSkillDto GetData()
	{
		return _dto;
	}

	public void SetSelect(bool b)
	{
		if(b)
		{
			_view.SelectSprite.spriteName = "the-no-choice-lines";
			_view.SelectBtn.normalSprite = "the-no-choice-lines";
		}
		else
		{
			_view.SelectSprite.spriteName = "the-choice-lines";
			_view.SelectBtn.normalSprite = "the-choice-lines";
		}
	}

	public void OnSelectCell()
	{
		if(_OnSelectCallBack != null)
		{
			_OnSelectCallBack(this);
		}
	}

	public void Dispose()
	{
	}
}