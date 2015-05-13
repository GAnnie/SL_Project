// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillCellController.cs
// Author   : willson
// Created  : 2015/4/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.assistskill.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class AssistSkillCellController : MonoBehaviourBase,IViewController
{
	private AssistSkillCell _view;

	private bool _isAddMode;
	private AssistSkillVo _vo;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillCell>();
		_view.Setup(this.transform);

		_isAddMode = false;
		_view.CountLbl.text = "";
		_view.IconSprite.enabled = false;

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.IconSpriteBtn.onClick,OnClickIcon);
	}

	public void SetData(AssistSkillVo vo)
	{
		_vo = vo;
		_isAddMode = false;
		if(vo != null)
		{
			_view.IconSprite.enabled = true;
			_view.IconSprite.spriteName = "10013";
			_view.IconSpriteBtn.normalSprite = "10013";
			_view.CountLbl.text = vo.level.ToString();
		}
		else
		{
			_view.CountLbl.text = "";
			_view.IconSprite.enabled = false;
		}
	}

	public AssistSkillDto GetData()
	{
		if(_vo == null)
			return null;
		return _vo.dto as AssistSkillDto; 
	}

	public void UpDataLv(int lv)
	{
		_vo.level = lv;
		_view.CountLbl.text = lv.ToString();
	}

	public void SetAddMode()
	{
		_isAddMode = true;
		_view.IconSprite.enabled = true;
		_view.IconSprite.spriteName = "flag_add";
		_view.IconSpriteBtn.normalSprite = "flag_add";
	}

	private void OnClickIcon()
	{
		if(!_isAddMode)
		{
			if(_vo != null)
			{
				if(_vo.id == AssistSkill.AssistSkillEnum_Make)
				{
					ProxySkillModule.OpenAssistSkillMakeWin(_vo.dto as AssistSkillDto);
				}
				else if(_vo.id == AssistSkill.AssistSkillEnum_Tailoring)
				{
					ProxySkillModule.OpenAssistSkillTailoringWin(_vo.dto as AssistSkillDto);
				}
				else if(_vo.id == AssistSkill.AssistSkillEnum_Cook)
				{
					ProxySkillModule.OpenAssistSkillCookWin(_vo.dto as AssistSkillDto);
				}
				else if(_vo.id == AssistSkill.AssistSkillEnum_Refine)
				{
					ProxySkillModule.OpenAssistSkillRefineWin(_vo.dto as AssistSkillDto);
				}
				else if(_vo.id == AssistSkill.AssistSkillEnum_Pursuit || _vo.id == AssistSkill.AssistSkillEnum_Escape)
				{
					ProxySkillModule.ShowTips(_vo.name,_vo.memo,this.gameObject);
				}
				else if(_vo.id > 100)
				{
					ProxySkillModule.OpenAssistSkillFactionWin(_vo.dto as FactionSkillDto);
				}
			}
			else
				TipManager.AddTip("打开制作界面");
		}
		else
		{
			ProxySkillModule.OpenAssistSkillLearningWin();
		}
	}

	public void Dispose()
	{
	}
}