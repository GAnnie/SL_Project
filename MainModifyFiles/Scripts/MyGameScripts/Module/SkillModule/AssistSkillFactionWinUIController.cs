// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillFactionWinUIController.cs
// Author   : willson
// Created  : 2015/4/8 
// Porpuse  : 
// **********************************************************************

using com.nucleus.h1.logic.core.modules.assistskill.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.assistskill.data;
using com.nucleus.h1.logic.services;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class AssistSkillFactionWinUIController : MonoBehaviourBase,IViewController
{
	private const string AssistProductCellName = "Prefabs/Module/SkillModule/AssistProductCell";
	
	private AssistSkillMakeWinUI _view;
	private FactionSkillDto _dto;
	
	private List<AssistSkillProduct> _products;
	private int _currProductIndex;

	private FactionAssistSkillProduct _skillProduct;

	private AssistProductCellController _productCell;

	private int _vigourConsume;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillMakeWinUI>();
		_view.Setup(this.transform);
		
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( AssistProductCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemPos,prefab);
		_productCell = module.GetMissingComponent<AssistProductCellController>();
		_productCell.InitView();
		
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		EventDelegate.Set(_view.LBtn.onClick,OnLBtn);
		EventDelegate.Set(_view.RBtn.onClick,OnRBtn);
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"制符",OnOptBtn);

		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
	}
	
	public void SetData(FactionSkillDto dto)
	{
		_dto = dto;
		HandleProducts();
		_currProductIndex = _products.Count - 1;
		_view.UsageDescLabel.text = _skillProduct.description;
		ShowProduct();
	}

	private void HandleProducts()
	{
		_products = new List<AssistSkillProduct>();
		_skillProduct = DataCache.getDtoByCls<FactionAssistSkillProduct>(_dto.factionSkillId);
		if(!string.IsNullOrEmpty(_skillProduct.subProductStr))
		{
			string[] productStrs = _skillProduct.subProductStr.Split(',');
			for(int index = 0;index < productStrs.Length;index++)
			{
				AssistSkillProduct product = new AssistSkillProduct();
				string[] info = productStrs[index].Split(':');

				/** 道具id */
				product.id = int.Parse(info[0]);
				/** 产品等级 */
				product.level = int.Parse(info[1]);
				if(product.level == 1)
					product.level = _dto.factionSkillLevel;

				product.assistSkillId = 0;
				product.skillLevelLimit = 0;

				if(product.level <= _dto.factionSkillLevel)
					_products.Add(product);
			}
		}
	}

	private void ShowProduct()
	{
		if(_products.Count == 0 || _currProductIndex < 0 || _currProductIndex >= _products.Count)
			return;
		
		AssistSkillProduct currProduct = _products[_currProductIndex];
		
		_view.ItemNameLabel.text = currProduct.props.name;
		_productCell.SetData(currProduct);
		
		// 计算活力
		//_skillProduct.vigourConsumeFormula
		_vigourConsume = 0;
		if(_products.Count == 1)
		{
			// 附魔类产品
			_vigourConsume = LuaManager.Instance.DoVigourConsumeFormula("VigourConsume_" + _dto.factionSkillId,
			                                                           _skillProduct.vigourConsumeFormula,_dto.factionSkillLevel);
		}
		else
		{
			// 多个道具
			_vigourConsume = LuaManager.Instance.DoVigourConsumeFormula("VigourConsume_" + _dto.factionSkillId,
			                                                           _skillProduct.vigourConsumeFormula,currProduct.level);
		}
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;
		
		_view.LBtn.gameObject.SetActive(_currProductIndex > 0);
		_view.RBtn.gameObject.SetActive(_currProductIndex < _products.Count - 1);
	}
	
	private void OnSubWealthChanged(SubWealthNotify notify)
	{
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;
	}
	
	private void OnLBtn()
	{
		if(_currProductIndex > 0)
		{
			_currProductIndex --;
			ShowProduct();
		}
	}
	
	private void OnRBtn()
	{
		if(_currProductIndex < _products.Count - 1)
		{
			_currProductIndex ++;
			ShowProduct();
		}
	}
	
	private void OnOptBtn()
	{
		if(PlayerModel.Instance.isEnoughVigour(_vigourConsume,true))
		{
			ServiceRequestAction.requestServer(AssistSkillService.factionMake(_products[_currProductIndex].id),"factionMake",(e)=>{
				AssistSkillProductResultDto dto = e as AssistSkillProductResultDto;
				//GameLogicHelper.HandlerItemTipDto(dto.item,false);
				if(dto != null)
					TipManager.AddTip("获得"+dto.item.item.name);
			});
		}
	}
	
	private void OnCloseBtn()
	{
		ProxySkillModule.CloseAssistSkillFactionWin();
	}
	
	public void Dispose()
	{
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
	}
}
