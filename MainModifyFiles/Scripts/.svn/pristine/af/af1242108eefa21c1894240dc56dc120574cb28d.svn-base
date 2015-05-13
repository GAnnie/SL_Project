// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FactionSkillLearningWinUIController.cs
// Author   : willson
// Created  : 2014/12/9 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.faction.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.battle.data;
using System;

public class FactionSkillLearningViewController : MonoBehaviourBase,IViewController
{
	private FactionSkillLearningView _view;

	private const string FactionSkillCellName = "Prefabs/Module/SkillModule/FactionSkillCell";
	private const string SubSkillCellName = "Prefabs/Module/SkillModule/SubSkillCell";

	private List<SubSkillCellController> _subSkillCells;

	private List<FactionSkillCellController> _factionSkillCell;

	private FactionSkillDto _currFactionSkill;
	private long _upgradeCost;

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<FactionSkillLearningView> ();
		_view.Setup(this.transform);

		InitStatic();
		RegisterEvent();
	}

	private void InitStatic()
	{
		_factionSkillCell = null;

		_view.RelationTitleLabel.text = string.Format("[ffd84c]{0}[-]","相关法术");
		_view.UpgradeTitleLabel.text = string.Format("[ffd84c]{0}[-]","升级需求");

		// 
		_subSkillCells = new List<SubSkillCellController>();
		_subSkillCells.Add(AddSubSkill(0f));
		_subSkillCells.Add(AddSubSkill(122.0f));
	}

	private SubSkillCellController AddSubSkill(float x)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( SubSkillCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.SubSkillGroup,prefab);

		SubSkillCellController cell = module.GetMissingComponent<SubSkillCellController>();
		cell.InitView();
		cell.transform.localPosition = new Vector3(x,0f,0f);
		return cell;
	}

	public void SetData()
	{
		Faction faction = PlayerModel.Instance.GetPlayer().faction;

		int count = 0;

		AddFactionSkillCell(FactionSkillModel.Instance.GetFactionSkill(faction.mainFactionSkillId),count);
		count ++;

		for(int index = 0;index < faction.propertyFactionSkillIds.Count;index++)
		{
			AddFactionSkillCell(FactionSkillModel.Instance.GetFactionSkill(faction.propertyFactionSkillIds[index]),count);
			count ++;
		}

		AddFactionSkillCell(FactionSkillModel.Instance.GetFactionSkill(faction.assistFactionSkillId),count);

		if(_factionSkillCell.Count > 0)
			_factionSkillCell[0].OnSelectCell();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
		UIHelper.CreateBaseBtn(_view.LearningBtnPos,"学习",OnLearningBtn);
		UIHelper.CreateBaseBtn(_view.AllBtnPos,"一键升级",OnAllBtn);
	}

	private void AddFactionSkillCell(FactionSkillDto dto,int count)
	{
		if(_factionSkillCell == null)
		{
			_factionSkillCell = new List<FactionSkillCellController>();
		}

		if(count < _factionSkillCell.Count)
		{
			_factionSkillCell[count].SetData(dto,OnSelectFactionSkill);
		}
		else
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( FactionSkillCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.FactionSKillCellGrid.gameObject,prefab);
			FactionSkillCellController cell = module.GetMissingComponent<FactionSkillCellController>();
			cell.InitView();
			cell.SetData(dto,OnSelectFactionSkill);
			_view.FactionSKillCellGrid.Reposition();
			_factionSkillCell.Add(cell);
		}
	}

	private void OnSelectFactionSkill(FactionSkillCellController cell)
	{
		for(int index = 0;index < _factionSkillCell.Count;index++)
		{
			_factionSkillCell[index].SetSelect(_factionSkillCell[index] == cell);
		}

		ShowFactionSkillInfo(cell.GetData());
	}
	
	private void ShowFactionSkillInfo(FactionSkillDto dto)
	{
		_currFactionSkill = dto;
		_view.FactionSkillInfoTitleLabel.text = string.Format("[ffd84c]{0}[-]",dto.factionSkill.name);
		_view.FactionSkillDescriptionLabel.text = dto.factionSkill.description;
	
		ShowSubSkill(dto);

		_upgradeCost = GetUpgradeCost(dto);
		_view.ConsumptionLabel.text = string.Format("消耗：{0}",_upgradeCost);
		_view.HaveLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().copper);
	}

	private void OnWealthChanged(WealthNotify notify)
	{
		_view.HaveLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().copper);
	}

	private void ShowSubSkill(FactionSkillDto dto)
	{
		if((dto.factionSkill.skillInfos == null || dto.factionSkill.skillInfos.Count == 0)
		   && !string.IsNullOrEmpty(dto.factionSkill.skillsFormula))
		{
			string[] subSkillInfo  = dto.factionSkill.skillsFormula.Split(':');

			SkillInfo skillInfo = new SkillInfo();
			skillInfo.skillId = int.Parse(subSkillInfo[0]);
			skillInfo.acquireFactionSkillLevel = int.Parse(subSkillInfo[1]);

			skillInfo.skill = new Skill();
			skillInfo.skill.name = dto.factionSkill.name;
			skillInfo.skill.description = dto.factionSkill.description;

			if(dto.factionSkill.skillInfos == null)
				dto.factionSkill.skillInfos = new List<SkillInfo>();
			dto.factionSkill.skillInfos.Add(skillInfo);
		}

		if(dto.factionSkill.skillInfos.Count == 1)
		{
			_view.subSkillTipsLbl.text = "";

			_subSkillCells[0].SetData(dto.factionSkill.skillInfos[0],dto);
			_subSkillCells[0].transform.localPosition = new Vector3(62f,0f,0f);

			_subSkillCells[1].Hide();
		}
		else if(dto.factionSkill.skillInfos.Count == 2)
		{
			_view.subSkillTipsLbl.text = "";

			_subSkillCells[0].SetData(dto.factionSkill.skillInfos[0],dto);
			_subSkillCells[0].transform.localPosition = new Vector3(0f,0f,0f);

			_subSkillCells[1].SetData(dto.factionSkill.skillInfos[1],dto);
		}
		else
		{
			_subSkillCells[0].Hide();
			_subSkillCells[1].Hide();

			_view.subSkillTipsLbl.text = "无";
		}
	}

	public void OnLearningBtn()
	{
        int rangeLv = DataHelper.GetStaticConfigValue(H1StaticConfigs.FACTION_SKILL_MAX_LEVEL_DIFF_AGAINST_MAIN_CHARACTOR_LEVEL, 5);
        int skillMaxLv = PlayerModel.Instance.GetPlayerLevel() + rangeLv;
		if(_currFactionSkill.factionSkillLevel < skillMaxLv)
		{
			if(PlayerModel.Instance.isEnoughCopper(_upgradeCost, true))
			{
                if (_currFactionSkill.factionSkillLevel < PlayerModel.Instance.ServerGrade + rangeLv)
				{
					LearnSkill();
				}
				else
				{
                    ProxyWindowModule.OpenConfirmWindow(string.Format("由于所学习的技能等级超过服务器等级+{0}，继续学习将消耗正常学习的1.5倍铜币，是否继续学习？", rangeLv), "",
						()=>{LearnSkill();},null,UIWidget.Pivot.Left);
				}
			}
		}
		else
		{
			TipManager.AddTip("技能已达学习上限，你无法继续学习");
		}
	}

	private void LearnSkill()
	{
		ServiceRequestAction.requestServer(FactionService.upgradeFactionSkill(_currFactionSkill.factionSkillId), "学习技能", (e) => {
			FactionSkillDto newFactionSkillDto = e as FactionSkillDto;
			if(newFactionSkillDto == null)
			{
				Debug.LogError("newFactionSkillDto is null");
				return;
			}
			_currFactionSkill = newFactionSkillDto;
			FactionSkillModel.Instance.UpDate(newFactionSkillDto);
//			_currFactionSkill.factionSkillLevel += 1;
			TipManager.AddTip(_currFactionSkill.factionSkill.name.WrapColor(ColorConstant.Color_Tip_Item) + "学习成功");

			PlayerModel.Instance.GetWealth().copper -= _upgradeCost;
			TipManager.AddLostCurrencyTip(_upgradeCost, ItemIconConst.Copper);

			for(int index = 0;index < _factionSkillCell.Count;index++)
			{
				if(_factionSkillCell[index].GetData().factionSkillId == _currFactionSkill.factionSkillId)
				{
					_factionSkillCell[index].SetData(_currFactionSkill,OnSelectFactionSkill);
					ShowFactionSkillInfo(_currFactionSkill);
					break;
				}
			}

			PlayerModel.Instance.CalculatePlayerBp(); //技能学习成功，重新计算人物战斗属性
		});
	}

	public void OnAllBtn()
	{
		// 寻找最低等级的技能,判断是否有钱
		FactionSkillDto dto = _factionSkillCell[0].GetData();
		for(int index = 1;index < _factionSkillCell.Count;index++)
		{
			if(_factionSkillCell[index].GetData() != null && 
			   dto.factionSkillLevel > _factionSkillCell[index].GetData().factionSkillLevel)
			{
				dto = _factionSkillCell[index].GetData();
			}
		}

        // 判断最低级技能是否满级
        int rangeLv = DataHelper.GetStaticConfigValue(H1StaticConfigs.FACTION_SKILL_MAX_LEVEL_DIFF_AGAINST_MAIN_CHARACTOR_LEVEL, 5);
        int skillMaxLv = PlayerModel.Instance.GetPlayerLevel() + rangeLv;
        if (dto.factionSkillLevel < skillMaxLv)
        {
            long minCost = GetCurrUpgradeCost(dto);
            if (PlayerModel.Instance.isEnoughCopper(minCost, true))
            {
                ServiceRequestAction.requestServer(FactionService.fastUpgradeFactionSkill(), "一键学习", OnFastUpgradeFactionSkill);
            }
        }
        else
        {
            TipManager.AddTip("技能已达学习上限，你无法继续学习");
        }
	}

	private void OnFastUpgradeFactionSkill(GeneralResponse e)
	{
		FactionSkillUpgradeStatusDto dto = e as FactionSkillUpgradeStatusDto;
		// 一键升级 (状态 - 3003:门派技能不存在 ，3004:门派技能等级已达到上限, 202:铜币不足)
		if(dto.errorCode != 3003)
		{
			List<int> upgradeSkillIds = new List<int>();
			long copper = PlayerModel.Instance.GetWealth().copper;
			int statusCode = 0;

			for(int index = 0;index < dto.updatedFactions.Count;index++)
			{
				FactionSkillDto newFactionSkillDto = dto.updatedFactions[index];
				upgradeSkillIds.Add(newFactionSkillDto.factionSkillId);

				FactionSkillDto oldFactionSkillDto = FactionSkillModel.Instance.GetFactionSkill(newFactionSkillDto.factionSkillId);
				PlayerModel.Instance.GetWealth().copper -= GetUpgradeCost(oldFactionSkillDto,newFactionSkillDto);

				FactionSkillModel.Instance.UpDate(newFactionSkillDto);
			}

			if(dto.errorCode == 3004)
			{
				TipManager.AddTip("技能已达学习上限，你无法继续学习");
			}
			else if(dto.errorCode == 302)
			{
				TipManager.AddTip("铜币不足");
			}

			copper -= PlayerModel.Instance.GetWealth().copper;
			if(copper > 0)
			{
				TipManager.AddLostCurrencyTip(copper,ItemIconConst.Copper);
			}

			List<FactionSkillDto> skills = FactionSkillModel.Instance.GetFactionSkillDtos();
			for(int index = 0;index < skills.Count;index++)
			{
				if(upgradeSkillIds.IndexOf(skills[index].factionSkillId) != -1)
				{
					TipManager.AddTip(string.Format("{0}技能等级提升为{1}",skills[index].factionSkill.name.WrapColor(ColorConstant.Color_Tip_Item),skills[index].factionSkillLevel));
				}
			}
		}
		else
		{
			TipManager.AddTip("门派技能不存在");
		}

		for(int index = 0;index < _factionSkillCell.Count;index++)
		{
			FactionSkillDto factionSkillDto = FactionSkillModel.Instance.GetFactionSkill(_factionSkillCell[index].GetData().factionSkillId);
			_factionSkillCell[index].SetData(factionSkillDto,OnSelectFactionSkill);
			if(factionSkillDto.factionSkillId == _currFactionSkill.factionSkillId)
			{
				_currFactionSkill = factionSkillDto;
				_factionSkillCell[index].OnSelectCell();
			}
		}


		PlayerModel.Instance.CalculatePlayerBp(); //技能学习成功，重新计算人物战斗属性
	}
	
    private long GetUpgradeCost(FactionSkillDto skillDto)
    {
        int rangeLv = DataHelper.GetStaticConfigValue(H1StaticConfigs.FACTION_SKILL_MAX_LEVEL_DIFF_AGAINST_MAIN_CHARACTOR_LEVEL, 5);
        float costMultiplier = 1;
        if (skillDto.factionSkillLevel >= PlayerModel.Instance.ServerGrade + rangeLv)
        {
            costMultiplier = DataHelper.GetStaticConfigValuef(H1StaticConfigs.UPGRADE_FACTION_SKILL_EXCEED_SERVER_GRADE_COST_MULTIPLIER, 1.5f);
        }
        else if (skillDto.factionSkillLevel <= PlayerModel.Instance.ServerGrade - rangeLv)
        {
            costMultiplier = DataHelper.GetStaticConfigValuef(H1StaticConfigs.UPGRADE_FACTION_SKILL_LESS_SERVER_GRADE_COST_MULTIPLIER, 1.5f);
        }
        return (long)Math.Ceiling((DataCache.getDtoByCls<FactionSkillUpgradeInfo>(skillDto.factionSkillLevel).copperCost * costMultiplier));
    }

	private long GetUpgradeCost(FactionSkillDto oldLvSkillDto,FactionSkillDto newLvSkillDto) 
	{
		long upgradeCopperCost = 0;
		while(oldLvSkillDto.factionSkillLevel < newLvSkillDto.factionSkillLevel)
		{
            upgradeCopperCost += GetUpgradeCost(oldLvSkillDto);
			oldLvSkillDto.factionSkillLevel += 1;
		}

		return upgradeCopperCost;
	}

	private long GetCurrUpgradeCost(FactionSkillDto skillDto)
	{
		float costMultiplier = 1;
		if (skillDto.factionSkillLevel >= PlayerModel.Instance.ServerGrade) 
		{
			costMultiplier = DataHelper.GetStaticConfigValuef(H1StaticConfigs.UPGRADE_FACTION_SKILL_EXCEED_SERVER_GRADE_COST_MULTIPLIER,1.5f);
		}
		
		return(long)(DataCache.getDtoByCls<FactionSkillUpgradeInfo>(skillDto.factionSkillLevel).copperCost * costMultiplier);
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
	}
}