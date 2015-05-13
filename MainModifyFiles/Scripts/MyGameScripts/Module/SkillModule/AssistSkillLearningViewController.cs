// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillLearningViewController.cs
// Author   : willson
// Created  : 2015/4/3 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using UnityEngine;
using com.nucleus.h1.logic.services;

public class AssistSkillLearningViewController : MonoBehaviourBase,IViewController
{
	private const string AssistSkillCellName = "Prefabs/Module/SkillModule/AssistSkillLCell";

	private AssistSkillLearningView _view;

	private AssistSkillLCellController _currAssistSkill;
	private List<AssistSkillLCellController> _assistSkillCell;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillLearningView>();
		_view.Setup(this.transform);
		
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		UIHelper.CreateBaseBtn(_view.LearningBtnPos,"学习",OnLearnBtn);
		UIHelper.CreateBaseBtn(_view.FastLearningBtnPos,"学习五次",OnFastLearnBtn);
	}

	public void SetData()
	{
		ShowAssistSkills();
	}

	private void ShowAssistSkills()
	{
		List<AssistSkillDto> skills = AssistSkillModel.Instance.GetLearnAssistSkills();
		for(int index = 0;index < skills.Count;index++)
		{
			AddAssistSkillCell(skills[index],index);
		}

		if(_assistSkillCell.Count > 0)
		{
			OnSelectAssistSkill(_assistSkillCell[0]);
		}
	}

	private void AddAssistSkillCell(AssistSkillDto dto,int count)
	{
		if(_assistSkillCell == null)
		{
			_assistSkillCell = new List<AssistSkillLCellController>();
		}
		
		if(count < _assistSkillCell.Count)
		{
			_assistSkillCell[count].SetData(dto,OnSelectAssistSkill);
		}
		else
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( AssistSkillCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.AssistSkillCellGrid.gameObject,prefab);
			//UIHelper.AdjustDepth(module,1);
			AssistSkillLCellController cell = module.GetMissingComponent<AssistSkillLCellController>();
			cell.InitView();
			cell.SetData(dto,OnSelectAssistSkill);
			_view.AssistSkillCellGrid.Reposition();
			_assistSkillCell.Add(cell);
		}
	}

	private void OnSelectAssistSkill(AssistSkillLCellController cell)
	{
		for(int index = 0;index < _assistSkillCell.Count;index++)
		{
			_assistSkillCell[index].SetSelect(_assistSkillCell[index] == cell);
		}

		_currAssistSkill = cell;
		ShowAssistSkillInfo(cell.GetData());
	}

	private int _currCopperCost;
	private int _currContributeCost;

	private void ShowAssistSkillInfo(AssistSkillDto dto)
	{
		_view.AssistSkillInfoTitleLabel.text = string.Format("[ffd84c]{0}[-]",dto.assistSkill.name);
		_view.AssistSkillDescriptionLabel.text = dto.assistSkill.memo;

		_currCopperCost = LuaManager.Instance.DoAssistSkillCopperFormula(dto);
		_view.copperHasLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().copper);
		_view.copperCostLabel.text = string.Format("消耗：{0}",_currCopperCost);

		_currContributeCost = LuaManager.Instance.DoAssistSkillContributeFormula(dto);
		HandleContribute(_currContributeCost);
	}

	private void HandleContribute(int contribute)
	{
		if(contribute > 0)
		{
			_view.contributeHasLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().contribute);
			_view.contributeCostLabel.text = string.Format("消耗：{0}",_currContributeCost);
			_view.contributeIcon2.SetActive(true);
			_view.contributeIcon1.SetActive(true);
		}
		else
		{
			_view.contributeHasLabel.text = "";
			_view.contributeCostLabel.text = "";
			_view.contributeIcon2.SetActive(false);
			_view.contributeIcon1.SetActive(false);
		}
	}

	private void OnLearnBtn()
	{
		LearnSkillHandle(1);
	}
	
	private void OnFastLearnBtn()
	{
		LearnSkillHandle(5);
	}

	private void LearnSkillHandle(int times)
	{
		if(_currAssistSkill.isMax())
		{
			TipManager.AddTip("该技能已达你可学习的上限");
			return;
		}

        int copperCost = _currCopperCost;
        int contributeCost = _currContributeCost;

        if (times > 1)
        {
            AssistSkillDto dto = _currAssistSkill.GetData();
            for(int t = 2;t <= times;t++)
            {
                copperCost += LuaManager.Instance.DoAssistSkillCopperFormula(dto, dto.level + t);
                contributeCost += LuaManager.Instance.DoAssistSkillContributeFormula(dto, dto.level + t);
            }
        }


        if (!PlayerModel.Instance.isEnoughCopper(copperCost, true))
		{
			return;
		}

        if (PlayerModel.Instance.GetWealth().contribute < contributeCost)
		{
            int needContribute = contributeCost - PlayerModel.Instance.GetWealth().contribute;
			int needCopper = CurrencyExchange.ContributeToCopper(needContribute);
			string tips = string.Format("使用{0}{1}代替{2}点{3}？",needCopper,ItemIconConst.Copper,needContribute,ItemIconConst.Contribute);
			ProxyWindowModule.OpenConfirmWindow(tips,"帮贡不足",
			()=>{
                if (PlayerModel.Instance.isEnoughCopper(copperCost + needCopper, true))
				{
					LearnSkillAction(times);
				}
			});
			
			return;
		}
		
		LearnSkillAction(times);
	}

	private void LearnSkillAction(int times)
	{
		ServiceRequestAction.requestServer(AssistSkillService.study(_currAssistSkill.GetData().id,times), "study", (e) => {
			AssistSkillStudyResultDto newSkillDto = e as AssistSkillStudyResultDto;
			if(newSkillDto == null)
			{
				Debug.LogError("newSkillDto is null");
				return;
			}

			if(newSkillDto.errorCode == 9001)
			{
				TipManager.AddTip("该技能已达你可学习的上限");
				//return;
			}
			else if(newSkillDto.errorCode == 302)
			{
				TipManager.AddTip("铜币不足");
				//return;
			}

			if(newSkillDto.assistSkill == null)
				return;

			AssistSkillModel.Instance.UpDate(newSkillDto.assistSkill);
			
			TipManager.AddTip(newSkillDto.assistSkill.assistSkill.name.WrapColor(ColorConstant.Color_Tip_Item) + "学习成功");

			PlayerModel.Instance.GetWealth().copper -= newSkillDto.copper;
			TipManager.AddLostCurrencyTip(newSkillDto.copper, ItemIconConst.Copper);
			if(newSkillDto.contribute > 0)
			{
				PlayerModel.Instance.GetWealth().contribute -= newSkillDto.contribute;
				TipManager.AddLostCurrencyTip(newSkillDto.contribute, ItemIconConst.Contribute);
			}

			_currAssistSkill.SetData(newSkillDto.assistSkill,OnSelectAssistSkill);
			ShowAssistSkillInfo(newSkillDto.assistSkill);
		});
	}

	public void Dispose()
	{
	}
}