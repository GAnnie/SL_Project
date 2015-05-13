// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillCookWinUIController.cs
// Author   : willson
// Created  : 2015/4/8 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.assistskill.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using UnityEngine;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class AssistSkillCookWinUIController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private AssistSkillCookWinUI _view;

	private AssistSkillDto _dto;
	private List<ItemCellController> _cookItems;

	private int _vigourConsume;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillCookWinUI>();
		_view.Setup(this.transform);

		InitCookItem();
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"烹饪",OnOptBtn);

		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
	}

	private void InitCookItem()
	{
		_cookItems = new List<ItemCellController>(8);
		for(int index = 0;index < 8;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.ItemGrid.gameObject,prefab);
			ItemCellController cell = module.GetMissingComponent<ItemCellController>();
			cell.InitView();
			cell.CanDisplayCount(false);
			_cookItems.Add(cell);
		}
		_view.ItemGrid.Reposition();
	}

	public void SetData(AssistSkillDto dto)
	{
		_dto = dto;

		List<AssistSkillCookProduct> _products = DataCache.getArrayByClsWithoutSort<AssistSkillCookProduct>();
		for(int index = _products.Count - 1;index >= 0;index--)
		{
			if(_products[index].skillLevelLimit > dto.level + 10)
			{
				_products.RemoveAt(index);
			}
		}

		_vigourConsume = LuaManager.Instance.DoVigourConsumeFormula(_dto,_dto.level);
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;

		_view.UsageDescLabel.text = _dto.assistSkill.usageDesc;
		for(int index = 0;index < _products.Count;index++)
		{
			if(index < _cookItems.Count)
			{
				_cookItems[index].SetData(ItemHelper.ItemIdToPackItemDto(_products[index].showItemId),OnClickItem);
				_cookItems[index].isGrey = index == _products.Count - 1;
			}
		}
	}

	private void OnSubWealthChanged(SubWealthNotify notify)
	{
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;
	}

	private void OnClickItem(ItemCellController cell)
	{
		ProxyItemTipsModule.Open(cell.GetData(),cell.gameObject,false);
	}

	private void OnOptBtn()
	{
		if(PlayerModel.Instance.isEnoughVigour(_vigourConsume,true))
		{
			ServiceRequestAction.requestServer(AssistSkillService.cook(),"cook",(e)=>{
				AssistSkillProductResultDto dto = e as AssistSkillProductResultDto;
				//GameLogicHelper.HandlerItemTipDto(dto.item,false);
				if(dto != null)
					TipManager.AddTip("获得"+dto.item.item.name);
			});
		}
	}

	private void OnCloseBtn()
	{
		ProxySkillModule.CloseAssistSkillCookWin();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
	}
}