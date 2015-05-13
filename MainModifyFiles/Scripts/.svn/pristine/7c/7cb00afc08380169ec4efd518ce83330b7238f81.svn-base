// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FactionSkillCellController.cs
// Author   : willson
// Created  : 2014/12/9 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class FactionSkillCellController : MonoBehaviourBase,IViewController
{
	private FactionSkillCell _view;

	private FactionSkillDto _dto;
	private System.Action<FactionSkillCellController> _OnSelectCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<FactionSkillCell> ();
		_view.Setup(this.transform);
		SetSelect(false);
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set (_view.SelectBtn.onClick, OnSelectCell);
	}

	public void SetData(FactionSkillDto dto,System.Action<FactionSkillCellController> OnSelectCallBack)
	{
		_OnSelectCallBack = OnSelectCallBack;

		_dto = dto;
		_view.NameLabel.text = _dto.factionSkill.name;
		_view.TypeLabel.text = _dto.factionSkill.shortDesc;

        if (_view.IconSprite.atlas != null || _view.IconSprite.atlas.GetSprite(_dto.factionSkill.id.ToString()) != null)
        {
            _view.IconSprite.spriteName = _dto.factionSkill.id.ToString();
        }

		int skillMaxLv = PlayerModel.Instance.GetPlayerLevel() + 5;

		if(skillMaxLv > PlayerModel.Instance.ServerGrade + 5)
		{
			_view.LvLabel.text = _dto.factionSkillLevel + "/" + skillMaxLv;
		}
		else
		{
			_view.LvLabel.text = _dto.factionSkillLevel + "/" + skillMaxLv;
		}
	}

	public FactionSkillDto GetData()
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