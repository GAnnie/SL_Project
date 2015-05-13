// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PracticeSkillViewController.cs
// Author   : willson
// Created  : 2015/3/13 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.spell.dto;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PracticeSkillViewController : MonoBehaviourBase,IViewController
{
	private const string FactionSkillCellName = "Prefabs/Module/SkillModule/PracticeSkillCell";

	private PracticeSkillView _view;

	private List<PracticeSkillCellController> _spellCells;
	private PracticeSkillCellController _currCell;
	private SpellDto _currSpellDto;

	private int _upgradeCost;

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}
	
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PracticeSkillView> ();
		_view.Setup(this.transform);

		_upgradeCost = DataHelper.GetStaticConfigValue(H1StaticConfigs.SPELL_STUDY_FEE);

		//InitStatic();
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;

		UIHelper.CreateBaseBtn(_view.LearningBtnPos,"学习",OnLearnBtn);
		UIHelper.CreateBaseBtn(_view.FastLearningBtnPos,"学习十次",OnFastLearnBtn);

		EventDelegate.Set(_view.PracticeToggleBtn.onClick,OnSetCurrentSpell);
		EventDelegate.Set(_view.TipsButton.onClick,OnClickTips);
	}

	public void SetData()
	{
		List<SpellDto> dtos = SpellModel.Instance.GetSpellDtos();
		for(int index = 0;index < dtos.Count;index++)
		{
			SpellDto dto = dtos[index];
			AddSpellCell(dto,index);
		}

		if(_currSpellDto != null)
		{
			ShowSpellDto(_currSpellDto);
		}
		else if(_spellCells != null && _spellCells.Count > 0)
		{
			OnSelectSpell(_spellCells[0]);
		}

		for(int index = 0;index < _spellCells.Count;index++)
		{
			_spellCells[index].SetPracticeIcon(_spellCells[index].GetData().spellId == SpellModel.Instance.GetSelectSpellId());
		}
	}

	private void AddSpellCell(SpellDto dto,int count)
	{
		if(_spellCells == null)
		{
			_spellCells = new List<PracticeSkillCellController>();
		}
		
		if(count < _spellCells.Count)
		{
			_spellCells[count].SetData(dto,OnSelectSpell);
		}
		else
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( FactionSkillCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.PracticeSkillCellGrid.gameObject,prefab);
			PracticeSkillCellController cell = module.GetMissingComponent<PracticeSkillCellController>();
			cell.InitView();
			cell.SetData(dto,OnSelectSpell);
			_view.PracticeSkillCellGrid.Reposition();
			_spellCells.Add(cell);
		}
	}

	private void OnSelectSpell(PracticeSkillCellController cell)
	{
		for(int index = 0;index < _spellCells.Count;index++)
		{
			_spellCells[index].SetSelect(_spellCells[index] == cell);
		}
		_currCell = cell;
		ShowSpellDto(cell.GetData());
	}


	private void ShowSpellDto(SpellDto dto)
	{
		_currSpellDto = dto;
		_view.PracticeSkillInfoTitleLabel.text = string.Format("[ffd84c]{0}[-]",dto.spell.name);
		_view.PracticeSkillDescriptionLabel.text = dto.spell.desc;
		
		// ShowSubSkill(dto);

		_view.ConsumptionLabel.text = string.Format("消耗：{0}",_upgradeCost);
		_view.HaveLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().copper);
		_view.ExpLabel.text = string.Format("升级：{0}/{1}",dto.exp,dto.nextLevelExp);

		_view.PracticeToggle.value = _currSpellDto.spellId == SpellModel.Instance.GetSelectSpellId();
	}

	private void OnWealthChanged(WealthNotify notify)
	{
		_view.HaveLabel.text = string.Format("拥有：{0}",PlayerModel.Instance.GetWealth().copper);
	}

	private void OnSetCurrentSpell()
	{
		if(_currSpellDto.spellId != SpellModel.Instance.GetSelectSpellId())
		{
			ServiceRequestAction.requestServer(SpellService.select(_currSpellDto.spellId), "默认修炼", (e) => {
				SpellModel.Instance.SetSelectSpellId(_currSpellDto.spellId);
				_view.PracticeToggle.value = true;

				for(int index = 0;index < _spellCells.Count;index++)
				{
					_spellCells[index].SetPracticeIcon(_spellCells[index].GetData().spellId == SpellModel.Instance.GetSelectSpellId());
				}
			});
		}
		else
		{
			_view.PracticeToggle.value = true;
		}
	}

	private void OnClickTips()
	{
		GameHintManager.Open(_view.TipsButton.gameObject,"修炼丹、任务和修炼宝箱等途经获得的修炼经验会自动增加到当期设置的修炼类型");
	}

	private void OnLearnBtn()
	{
		if(_currCell.isMax())
		{
			TipManager.AddTip("技能已达修炼上限，你无法继续修炼");
			return;
		}

		if(PlayerModel.Instance.isEnoughCopper(_upgradeCost,true))
		{
			ServiceRequestAction.requestServer(SpellService.study(_currSpellDto.spellId,1), "修炼技能", (e) => {
				LearnSuccess(e as SpellStudyResultDto);
			});
		}
	}

	private void OnFastLearnBtn()
	{
		if(_currCell.isMax())
		{
			TipManager.AddTip("技能已达修炼上限，你无法继续修炼");
			return;
		}

		if(PlayerModel.Instance.isEnoughCopper(_upgradeCost,true))
		{
			ServiceRequestAction.requestServer(SpellService.study(_currSpellDto.spellId,10), "修炼技能", (e) => {
				LearnSuccess(e as SpellStudyResultDto);
			});
		}
	}

	private void LearnSuccess(SpellStudyResultDto dto)
	{
		if(dto.errorCode == 3004)
		{
			TipManager.AddTip("技能已达修炼上限，你无法继续修炼");
			//return;
		}
		else if(dto.errorCode == 302)
		{
			TipManager.AddTip("铜币不足");
			//return;
		}

		if(dto.spell == null)
			return;

		PlayerModel.Instance.GetWealth().copper -= dto.copperConsume;
		if(dto.copperConsume > 0)
		{
			TipManager.AddLostCurrencyTip( dto.copperConsume,ItemIconConst.Copper);
		}

		TipManager.AddTip(string.Format("你获得了{0}点{1}{2}",dto.exp.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),_currSpellDto.spell.name,ItemIconConst.Exp2));

		SpellDto newSpell = dto.spell;
		SpellModel.Instance.UpDate(newSpell);

		_currCell.SetData(newSpell,OnSelectSpell);
		ShowSpellDto(newSpell);
	}
	
	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
	}
}