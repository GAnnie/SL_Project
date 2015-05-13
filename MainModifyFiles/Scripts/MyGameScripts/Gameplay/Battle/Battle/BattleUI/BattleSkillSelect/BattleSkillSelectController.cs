// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleSkillSelectView.cs
// Author   : SK
// Created  : 2014/11/25
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.faction.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.data;
using System;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.dto;

public class BattleSkillSelectController : MonoBehaviourBase,IViewController
{
	private static string SkillCellPrefabPath = "Prefabs/Module/Battle/BattleUI/SkillCellPrefab";

	private BattleSkillSelectView _view;

	public event Action<Skill> OnSkillSelect;

	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<BattleSkillSelectView> ();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.CloseBtn_UIButton.onClick, OnCloseButtonClick);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose()
	{
		if (OnSkillSelect != null)
		{
			OnSkillSelect(null);
			OnSkillSelect = null;
		}
	}

	public void OpenStunt(MonsterController mc, List<int> skills, Action<Skill> OnStuntSelectDelegate)
	{
		_mc = mc;

		if (_view == null)
		{
			InitView ();
			RegisterEvent ();
		}
		OnSkillSelect = OnStuntSelectDelegate;

		UpdateSkillList (skills);
	}

	public void Open(MonsterController mc, Action<Skill> OnSkillSelectDelegate)
	{
		_mc = mc;

		if (_view == null)
		{
			InitView ();
			RegisterEvent ();
		}
		OnSkillSelect = OnSkillSelectDelegate;

		if (mc.IsPet())
		{
			ShowPetSkill(mc);
		}
		else
		{
			ShowHeroSkill(mc);
		}
	}

	private MonsterController _mc;


	private void OnCloseButtonClick()
	{
		if (OnSkillSelect != null)
		{
			OnSkillSelect(null);
			OnSkillSelect = null;
		}

		ProxyBattleModule.HideSkillSelect();
	}

	private int _skillCount = 0;

	private void ShowHeroSkill(MonsterController mc)
	{
		Faction faction = mc.GetFaction ();

		List<int> fskills = new List<int>();
		fskills.Add(faction.mainFactionSkillId);
		fskills.AddRange(faction.propertyFactionSkillIds);

		List<int> skillIds = new List<int> ();

		for (int i=0,len=fskills.Count; i<len; i++)
		{
			FactionSkill factionSkill = DataCache.getDtoByCls<FactionSkill>(fskills[i]);
			foreach(SkillInfo info in factionSkill.skillInfos)
			{
				if (FactionSkillModel.Instance.GetFactionSkillLevel(factionSkill.id) >= info.acquireFactionSkillLevel)
				{
					skillIds.Add(info.skillId);
				}
			}
		}

		UpdateSkillList (skillIds);
	}

	private void AddSkillCell(Skill skill)
	{
		if (skill == null)
		{
			return;
		}

		if (skill.activeSkill == false)
		{
			return;
		}

		_skillCount++;

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(SkillCellPrefabPath ) as GameObject;
		GameObject go = NGUITools.AddChild(_view.Table.gameObject, prefab);

		int skillId = skill.id;
		if (skillId < 10)
		{
			skillId += 3000;
		}

		go.name = skillId.ToString();
		SkillCellPrefab skillCellPrefab = go.GetMissingComponent<SkillCellPrefab> ();
		skillCellPrefab.Setup (go.transform);

		////-trigger.maxHp()*0.05
		string limitTip = "";
		if (string.IsNullOrEmpty(skill.applyHpLimitFormula) == false)
		{
			int value = LuaManager.Instance.DoSkillFormula("applyHpLimitFormula"+skill.id, _mc.videoSoldier, skill.applyHpLimitFormula);
			if (_mc.currentHP < Math.Abs(value))
			{
				limitTip = "气血不符";
			}
		}

		if (string.IsNullOrEmpty(skill.applyHpMaxLimitFormula) == false)
		{
			int value = LuaManager.Instance.DoSkillFormula("applyHpMaxLimitFormula"+skill.id, _mc.videoSoldier, skill.applyHpMaxLimitFormula);
			if (_mc.currentHP > Math.Abs(value))
			{
				limitTip = "气血不符";
			}
		}

		if (string.IsNullOrEmpty(skill.spendHpFormula) == false)
		{
			int value = LuaManager.Instance.DoSkillFormula("spendHpFormula"+skill.id, _mc.videoSoldier, skill.spendHpFormula);
			if (_mc.currentHP < Math.Abs(value))
			{
				limitTip = "气血不足";
			}
		}

		if (string.IsNullOrEmpty(skill.spendMpFormula) == false)
		{
			int value = LuaManager.Instance.DoSkillFormula("applyHpMaxLimitFormula"+skill.id, _mc.videoSoldier, skill.spendMpFormula);
			if (_mc.currentMP < Math.Abs(value))
			{
				limitTip = "魔法不足";
			}
		}

		if (string.IsNullOrEmpty(skill.spendSpFormula) == false)
		{
			int value = LuaManager.Instance.DoSkillFormula("applySpLimitFormula"+skill.id, _mc.videoSoldier, skill.spendSpFormula);
			if (_mc.currentSP < Math.Abs(value))
			{
				limitTip = string.Format("需要{0}愤怒", value);
			}
		}

		//1813、1814、1815、1816这4个技能优先判断自身当前是否有变身buff（buffid：120）
		if (skillId == 1813 || skillId == 1814 || skillId == 1815 || skillId == 1816)
		{
			if (_mc.ContainsBuff(120) == false)
			{
				limitTip = "需要变身";
			}
		}

		skillCellPrefab.NameLabel_UILabel.text = skill.name;
		skillCellPrefab.TypeLabel_UILabel.text = limitTip==""?skill.shortDescription:limitTip;
		if (limitTip == "")
		{
			skillCellPrefab.TypeLabel_UILabel.color = ColorConstant.Color_Battle_SkillCanUseTip;
		}
		else
		{
			skillCellPrefab.TypeLabel_UILabel.color = ColorConstant.Color_Battle_SkillCanNotUseTip;
		}

		skillCellPrefab.IconSprite_UISprite.spriteName = skill.id.ToString();

		UIButton button = skillCellPrefab.SkillCellPrefab_UIButton;
		EventDelegate.Set(button.onClick, delegate {
			if (OnSkillSelect != null)
			{
				OnSkillSelect(skill);
				OnSkillSelect = null;
			}
			ProxyBattleModule.HideSkillSelect();
		});
	}

	private void ShowPetSkill(MonsterController mc)
	{
		PetPropertyInfo petPropertyInfo = PetModel.Instance.GetPetInfoByUID (mc.GetId());

		List<int> skills = petPropertyInfo.petDto.skillIds;
		UpdateSkillList (skills);
	}

	private void UpdateSkillList(List<int> list)
	{
		this.gameObject.SetActive(true);
		_view.Table.gameObject.RemoveChildren ();

		_skillCount = 0;

		for (int i=0,len=list.Count; i<len; i++)
		{
			Skill skill = DataCache.getDtoByCls<Skill>(list[i]);
			AddSkillCell(skill);
		}
		
		//_view.BGSprite_UISprite.height = 87 * _skillCount + 25;
		
		_view.Table.repositionNow = true;
	}
}

