// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentGemViewController.cs
// Author   : willson
// Created  : 2015/1/21 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.player.msg;
using System;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.dto;

public class EquipmentGemViewController : MonoBehaviourBase,IViewController
{
	private const string EquipmentOptCellName = "Prefabs/Module/EquipmentOptModule/EquipmentOptCell";
	private const string ItemCellExName = "Prefabs/Module/EquipmentOptModule/ItemCellEx";
	private const string GemItemCellName = "Prefabs/Module/EquipmentOptModule/GemItemCell";

	private EquipmentGemView _view;

	private List<EquipmentOptCellController> _equipmentOptCellList;
	private List<GemItemCellController> _gemItemList;

	private ItemCellExController _currEquipCell;
	private ItemCellExController _materialCell;

	private const int PAGE_COUNT = 6;

	private List<PackItemDto> _equips;
	private Summary _summary;

	private int _unitPrice;
	private int _needCount;

	private int _costIngot;
	private int _currEquipGemLv;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentGemView> ();
		_view.Setup(this.transform);

		_view.LBtn.gameObject.SetActive(false);
		_view.RBtn.gameObject.SetActive(false);

		_equipmentOptCellList = new List<EquipmentOptCellController>();
		for(int index = 0;index < PAGE_COUNT;index++)
		{
			AddEquipmentOptCell();
		}

		InitTopRightGroup();
		InitGemGroup();
		InitMaterialGrid();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
		
		BackpackModel.Instance.OnUpdateItem += OnUpdateItem;
		BackpackModel.Instance.OnDeleteItem += OnDeleteItem;

		EventDelegate.Set(_view.LBtn.onClick,OnLBtn);
		EventDelegate.Set(_view.RBtn.onClick,OnRBtn);
		
		EventDelegate.Set(_view.OptButton.onClick,OnInlayBtn);
	}

	private void AddEquipmentOptCell()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentOptCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.EquipmentGrid.gameObject,prefab);
		EquipmentOptCellController cell = module.GetMissingComponent<EquipmentOptCellController>();
		cell.InitView();
		UIHelper.AdjustDepth(module,3);
		_equipmentOptCellList.Add(cell);
		cell.Hide();
	}
	
	private void InitGemGroup()
	{
		EquipmentModel.Instance.SetupGem();
		List<Props> gems = EquipmentModel.Instance.GetGems();

		_gemItemList = new List<GemItemCellController>(gems.Count);

		for(int index = 0;index < gems.Count;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( GemItemCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.GemGrid.gameObject,prefab);
			GemItemCellController cell = module.GetMissingComponent<GemItemCellController>();
			cell.InitView();

			cell.SetData(gems[index],OnGemClick,OnRomeveGem);
			cell.isSelect = false;
			cell.isRomeve = false;

			UIHelper.AdjustDepth(module,3);
			_gemItemList.Add(cell);
		}
	}

	private void InitTopRightGroup()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.CurrEquipPos,prefab);
		_currEquipCell = module.GetMissingComponent<ItemCellExController>();
		_currEquipCell.InitView();
		_currEquipCell.SetData(null);
		UIHelper.AdjustDepth(module,3);
	}

	private void InitMaterialGrid()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.MaterialGrid.gameObject,prefab);
		_materialCell = module.GetMissingComponent<ItemCellExController>();
		_materialCell.InitView();
		_materialCell.Hide();
		UIHelper.AdjustDepth(module,3);
	}

	public void SetData()
	{
		_equips = BackpackModel.Instance.GetAllIdentifiedEquip();
		_view.CostLabel.gameObject.SetActive(false);
		if(_equips.Count > 0)
		{
			_view.OptButton.enabled = true;
			_view.OptButton.GetComponentInChildren<UISprite>().isGrey = false;

			int iTotalPage = (int)Math.Ceiling((double)_equips.Count / PAGE_COUNT);
			_summary = Summary.create(_equips.Count,iTotalPage,1,PAGE_COUNT);

			SetPage(1);
		}
		else
		{
			_view.OptButton.enabled = false;
			_view.OptButton.GetComponentInChildren<UISprite>().isGrey = true;
		}
	}

	private void SetPage(int page)
	{
		if (page >= 1 && page <= _summary.getTotalPage())
		{
			_summary.setCurrentPage(page);

			Range range = _summary.getRange();
			int start = (int)range.start;
			int end = (int)range.end;
			
			int index = 0;
			for (int equipIndex = start; equipIndex <= end; equipIndex++)
			{
				if (index < _equips.Count)
				{
					_equipmentOptCellList[index].SetData(_equips[equipIndex],true,OnSelectEquip);
					index++;
				}
			}
			
			while (index < _equipmentOptCellList.Count)
			{
				_equipmentOptCellList[index].Hide();
				index++;
			}

			OnSelectEquip(_equipmentOptCellList[0]);

			_view.LBtn.gameObject.SetActive(page != 1);
			_view.RBtn.gameObject.SetActive(page != _summary.getTotalPage());
		}
	}
	
	private void OnLBtn()
	{
		SetPage(_summary.getCurrentPage() - 1);
	}
	
	private void OnRBtn()
	{
		SetPage(_summary.getCurrentPage() + 1);
	}

	public void SelectEquip(PackItemDto dto)
	{
		for(int index = 0;index < _equipmentOptCellList.Count;index++)
		{
			if(_equipmentOptCellList[index].GetData().uniqueId == dto.uniqueId)
			{
				OnSelectEquip(_equipmentOptCellList[index]);
				break;
			}
		}
	}

	private void OnSelectEquip(EquipmentOptCellController cell)
	{
		for(int index = 0;index < _equipmentOptCellList.Count;index++)
		{
			_equipmentOptCellList[index].SetSelect(cell == _equipmentOptCellList[index]);
		}

		SetGem(cell.GetData());
		SetCurrEquip(cell.GetData());
		SetMaterial(null);
		SetCost(0);
	}

	private void SetGem(PackItemDto dto)
	{
		EquipmentExtraDto extraDto = dto.extra as EquipmentExtraDto;


		List<int> embeddableItemIds = extraDto.equipmentEmbedInfo.embedItemIds;
		if(extraDto.equipmentEmbedInfo.embedItemIds.Count >= 2)
			embeddableItemIds = extraDto.equipmentEmbedInfo.embedItemIds;
		else
		{
			Equipment equip = dto.item as Equipment;
			EquipmentPropertyInfo info = DataCache.getDtoByCls<EquipmentPropertyInfo>(equip.equipPartType);
			embeddableItemIds = info.embeddableItemIds;
		}

		for(int i = 0;i < _gemItemList.Count;i++)
		{
			_gemItemList[i].isSelect = false;
			for(int index = 0;index < embeddableItemIds.Count;index++)
			{
				if(_gemItemList[i].GetData().itemId == embeddableItemIds[index])
				{
					_gemItemList[i].isGrey = false;

					if(extraDto.equipmentEmbedInfo != null 
					   && extraDto.equipmentEmbedInfo.embedItemIds != null
					   && extraDto.equipmentEmbedInfo.embedItemIds.Count > 0)
					{
						_gemItemList[i].isRomeve = extraDto.equipmentEmbedInfo.embedItemIds.IndexOf(_gemItemList[i].GetData().itemId) != -1;
					}
					else
					{
						_gemItemList[i].isRomeve = false;
					}
					break;
				}
				else
				{
					_gemItemList[i].isGrey = true;
				}
			}
		}
	}

	private void SetCurrEquip(PackItemDto dto)
	{
		_currEquipCell.SetData(dto);
		if(dto != null)
		{
			_view.EquipmentPropertyLabel.text = "";
			ItemTextTipManager.Instance.ShowEquipProperty(dto,_view.EquipmentPropertyLabel);
		}
		else
		{
			_view.EquipmentPropertyLabel.text = "";
		}
	}

	private void SetMaterial(PackItemDto dto)
	{
		if(dto == null)
		{
			_materialCell.SetData(null);
			_materialCell.Hide();
		}
		else
		{
			// 获得消耗
			EquipmentModel.Instance.GetGemCost(dto.itemId,(unitPrice)=>{
				_unitPrice = unitPrice;
				UpDateMaterial(dto);
			});
		}
	}

	private void UpDateMaterial(PackItemDto gemDto)
	{
		if(_currEquipCell.GetData() != null)
		{
			EquipmentExtraDto extraDto = _currEquipCell.GetData().extra as EquipmentExtraDto;
			if(extraDto != null)
			{
				_currEquipGemLv = ItemHelper.GetEquipGemLv(_currEquipCell.GetData());
				_needCount = (int)Math.Pow(2,_currEquipGemLv);
				int cost = (_needCount - BackpackModel.Instance.GetItemCount(gemDto.itemId)) * _unitPrice;
				_materialCell.SetData(gemDto,_needCount,BackpackModel.Instance.GetItemCount(gemDto.itemId));
				SetCost(cost);
			}
		}
	}
	
	private void SetCost(int cost)
	{
		_costIngot = cost;

		if(_costIngot > 0)
		{
			_view.CostLabel.gameObject.SetActive(true);
			_view.CostLabel.text = _costIngot.ToString();
			_view.OptNameLabel.gameObject.transform.localPosition = new Vector3(0,-8,0);
		}
		else
		{
			_view.CostLabel.gameObject.SetActive(false);
			_view.OptNameLabel.gameObject.transform.localPosition = new Vector3(0,0,0);
		}
	}

	private void OnGemClick(GemItemCellController cell)
	{
		for(int index = 0;index < _gemItemList.Count;index++)
		{
			_gemItemList[index].isSelect = cell == _gemItemList[index];
		}

		SetMaterial(cell.GetData());
	}

	private void OnInlayBtn()
	{
		if(_currEquipCell == null || _currEquipCell.GetData() == null)
		{
			TipManager.AddTip("请选择装备");
			return;
		}

		if(_materialCell == null || _materialCell.GetData() == null)
		{
			TipManager.AddTip("请选择宝石");
			return;
		}

		int equipLv = (_currEquipCell.GetData().item as Equipment).equipLevel;
		if(_currEquipGemLv >= equipLv/10 + 2)
		{
			TipManager.AddTip("镶嵌宝石等级达到上限");
			return;
		}

		if(_costIngot <= 0)
		{
			EquipmentModel.Instance.EmbedGem(_currEquipCell.GetData(),_materialCell.GetData().itemId,_unitPrice,OnEmbedGem);
		}
		else
		{
			List<ItemDto> items = new List<ItemDto>();
			if(!_materialCell.isHas())
			{
				ItemDto dto = new ItemDto();
				dto.itemId = _materialCell.GetData().itemId;
				dto.itemCount = _materialCell.GetLack();
				items.Add(dto);
			}
			ProxyTipsModule.Open("材料不足",items,_costIngot,(ingot)=>{
				EquipmentModel.Instance.EmbedGem(_currEquipCell.GetData(),_materialCell.GetData().itemId,_unitPrice,OnEmbedGem);
			});
		}
	}

	private void OnRomeveGem(GemItemCellController cell)
	{
		if(_currEquipCell.GetData() != null)
		{
			ProxyWindowModule.OpenConfirmWindow(string.Format("是否拆除该装备上所有的{0}？拆除不会返还宝石",cell.GetData().item.name), "",
			()=>{
				EquipmentModel.Instance.RomeveGem(_currEquipCell.GetData(),cell.GetData().itemId,OnUpDateCurrEquip);
			});
		}
	}

	private void OnUpDateCurrEquip()
	{
		PackItemDto dto = _currEquipCell.GetData();
		SetCurrEquip(dto);
		// 更新左侧列表
		for(int index = 0;index < _equipmentOptCellList.Count;index++)
		{
			if(_equipmentOptCellList[index].GetData() != null && _equipmentOptCellList[index].GetData().uniqueId == dto.uniqueId)
			{
				_equipmentOptCellList[index].SetData(dto,true,OnSelectEquip);
				break;
			}
		}
		SetGem(dto);
	}
	
	private void OnEmbedGem(GeneralResponse e)
	{
		if(e is EquipmentEmbedInfo)
		{
			OnUpDateCurrEquip();
			UpDateMaterial(_materialCell.GetData());
		}
		else if(e is EquipmentEmbedCostInfo)
		{
			EquipmentModel.Instance.GetGemCost(_materialCell.GetData().itemId,(unitPrice)=>{
				_unitPrice = unitPrice;
				int cost = (_needCount - BackpackModel.Instance.GetItemCount(_materialCell.GetData().itemId)) * unitPrice;
				SetCost(cost);
			});
		}
	}

	private void OnWealthChanged(WealthNotify notify)
	{
		//EquipmentModel.Instance.SmithCostCacheClear();
	}
	
	private void OnUpdateItem(PackItemDto dto)
	{
		if(gameObject.activeSelf)
		{
			// 更新宝石数量
			for(int index = 0;index < _gemItemList.Count;index++)
			{
				_gemItemList[index].SetData(_gemItemList[index].GetData().item as Props,OnGemClick,OnRomeveGem);
			}

			// 更新材料
			if(_materialCell.isShow())
				UpDateMaterial(_materialCell.GetData());
		}
	}
	
	private void OnDeleteItem(int index)
	{
		OnUpdateItem(null);
	}

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
		
		BackpackModel.Instance.OnUpdateItem -= OnUpdateItem;
		BackpackModel.Instance.OnDeleteItem -= OnDeleteItem;
	}
}