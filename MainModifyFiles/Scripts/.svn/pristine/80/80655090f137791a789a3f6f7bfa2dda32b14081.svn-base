// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PracticeSkillCellController.cs
// Author   : willson
// Created  : 2015/3/23 
// Porpuse  : 
// **********************************************************************

using com.nucleus.h1.logic.core.modules.spell.dto;
using com.nucleus.h1.logic.core.modules;

public class PracticeSkillCellController : MonoBehaviourBase,IViewController
{
	private PracticeSkillCell _view;
	
	private SpellDto _dto;
	private System.Action<PracticeSkillCellController> _OnSelectCallBack;

	private int _spellMaxLv;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PracticeSkillCell> ();
		_view.Setup(this.transform);

		_view.PracticeIconSprite.SetActive(false);
		SetSelect(false);
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set (_view.SelectBtn.onClick, OnSelectCell);
	}
	
	public void SetData(SpellDto dto,System.Action<PracticeSkillCellController> OnSelectCallBack)
	{
		_OnSelectCallBack = OnSelectCallBack;
		
		_dto = dto;
		_view.NameLabel.text = _dto.spell.name;
		_view.TypeLabel.text = _dto.spell.effectDesc;
		
		_spellMaxLv = PlayerModel.Instance.GetPlayerLevel()/5;
		int maxlv = DataHelper.GetStaticConfigValue(H1StaticConfigs.SPELL_MAX_LEVEL);
		if(_spellMaxLv > maxlv)
			_spellMaxLv = maxlv;
		_view.LvLabel.text = _dto.level + "/" + _spellMaxLv;
	}
	
	public SpellDto GetData()
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

	public void SetPracticeIcon(bool b)
	{
		_view.PracticeIconSprite.SetActive(b);
	}

	public bool isMax()
	{
		return _dto.level >= _spellMaxLv;// && _dto.exp >= _dto.nextLevelExp;
	}

	public void Dispose()
	{
	}
}