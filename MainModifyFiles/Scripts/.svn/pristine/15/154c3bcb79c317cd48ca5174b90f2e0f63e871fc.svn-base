// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentPropertyViewController.cs
// Author   : willson
// Created  : 2015/1/21 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.player.msg;
using System;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.dto;

public class EquipmentPropertyViewController : MonoBehaviourBase,IViewController
{
	private const string EquipmentOptCellName = "Prefabs/Module/EquipmentOptModule/EquipmentOptCell";
	private const string ItemCellExName = "Prefabs/Module/EquipmentOptModule/ItemCellEx";
	private const string PropertyCellName = "Prefabs/Module/EquipmentOptModule/PropertyCell";

	private EquipmentPropertyView _view;

	private List<EquipmentOptCellController> _equipmentOptCellList;
	private List<PropertyCellController> _propertyCellList;

	private ItemCellExController _currEquipCell;
	private ItemCellExController _materialCell;

	private const int PAGE_COUNT = 6;
	
	private List<PackItemDto> _equips;
	private Summary _summary;

	private int _costIngot;
	private PropertyCellController _currPropertyCell;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentPropertyView> ();
		_view.Setup(this.transform);

		_view.LBtn.gameObject.SetActive(false);
		_view.RBtn.gameObject.SetActive(false);

		_propertyCellList = new List<PropertyCellController>();

		_equipmentOptCellList = new List<EquipmentOptCellController>();
		for(int index = 0;index < PAGE_COUNT;index++)
		{
			AddEquipmentOptCell();
		}

		InitTopRightGroup();
		InitMaterialGrid();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.LBtn.onClick,OnLBtn);
		EventDelegate.Set(_view.RBtn.onClick,OnRBtn);
		
		EventDelegate.Set(_view.OptButton.onClick,OnChangePropertyBtn);
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

	private void InitTopRightGroup()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.CurrEquipPos,prefab);
		_currEquipCell = module.GetMissingComponent<ItemCellExController>();
		_currEquipCell.InitView();
		_currEquipCell.SetData(null);
		UIHelper.AdjustDepth(module,3);
	}

	private PropertyCellController AddPropertyCell()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( PropertyCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.PropertyGrid.gameObject,prefab);
		PropertyCellController cell = module.GetMissingComponent<PropertyCellController>();
		cell.InitView();
		UIHelper.AdjustDepth(module,3);
		_propertyCellList.Add(cell);
		_view.PropertyGrid.Reposition();
		return cell;
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
		_equips = BackpackModel.Instance.GetAllIdentifiedEquip(40);
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
					_equipmentOptCellList[index].SetData(_equips[equipIndex],false,OnSelectEquip);
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

	private void OnSelectEquip(EquipmentOptCellController cell)
	{
		for(int index = 0;index < _equipmentOptCellList.Count;index++)
		{
			_equipmentOptCellList[index].SetSelect(cell == _equipmentOptCellList[index]);
		}
		
		SetProperty(cell.GetData());
		SetCurrEquip(cell.GetData());
		SetMaterial(cell.GetData());

		EquipmentModel.Instance.GetRefreshAptitudeCost(cell.GetData().uniqueId,
			(cost) =>
			{
				SetCost(cost);
			}
		);
	}

	private void SetProperty(PackItemDto dto)
	{
		EquipmentExtraDto extraDto = dto.extra as EquipmentExtraDto;
		if(extraDto == null)
			return;

		int index = 0;

		if(extraDto.aptitudeProperties != null && extraDto.aptitudeProperties.Count > 0)
		{
			for(;index < extraDto.aptitudeProperties.Count;index++)
			{
				if(index < _propertyCellList.Count)
				{
					_propertyCellList[index].SetData(extraDto.aptitudeProperties[index],OnPropertyClick);
					_propertyCellList[index].Index = index;
				}
				else
				{
					PropertyCellController cell = AddPropertyCell();
					cell.SetData(extraDto.aptitudeProperties[index],OnPropertyClick);
					cell.Index = index;
				}
			}
		}
		else
		{
			// 增加一个 "无" 的 cell
			if(_propertyCellList.Count > 0)
			{
				_currPropertyCell = _propertyCellList[0];
				_currPropertyCell.SetNull();
			}
			else
			{
				_currPropertyCell = AddPropertyCell();
				_currPropertyCell.SetNull();
			}

			index++;
		}

		while(index < _propertyCellList.Count)
		{
			_propertyCellList[index].Hide();
			index++;
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
			_materialCell.Hide();
		}
		else
		{
			Equipment equip = dto.item as Equipment;

			if(equip.propertyTransferMaterials != null && equip.propertyTransferMaterials.Count > 0)
			{
				EquipmentSmithMaterial material = equip.propertyTransferMaterials[0];
				int materialIndex = -1;
				for(int i = 0;i < material.selectionItemIds.Count;i++)
				{
					int count = BackpackModel.Instance.GetItemCount(material.selectionItemIds[i]);
					if(count >= material.itemCount)
					{
						materialIndex = i;
						break;
					}
				}
				
				materialIndex = materialIndex >= 0 ? materialIndex : 0;
				_materialCell.SetData(ItemHelper.ItemIdToPackItemDto(material.selectionItemIds[materialIndex]),material.itemCount,BackpackModel.Instance.GetItemCount(material.selectionItemIds[materialIndex]));
			}
		}
	}

	private void OnPropertyClick(PropertyCellController cell)
	{
		_currPropertyCell = cell;

		for(int index = 0;index < _propertyCellList.Count;index++)
		{
			if(_propertyCellList[index].isShow())
			{
				_propertyCellList[index].SetSelect(_propertyCellList[index] == cell);
			}
		}
	}

	public void OnChangePropertyBtn()
	{
		if(_currPropertyCell == null)
		{
			TipManager.AddTip("请选择要转换的属性");
			return;

		}

		if(_currPropertyCell.Index == -1)
		{
			TipManager.AddTip("该装备无属性可转换");
			return;
		}

		if(_costIngot <= 0)
		{
			EquipmentModel.Instance.RefreshAptitude(_currEquipCell.GetData(),_currPropertyCell.Index,_costIngot,OnRefreshAptitude);
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
				EquipmentModel.Instance.RefreshAptitude(_currEquipCell.GetData(),_currPropertyCell.Index,_costIngot,OnRefreshAptitude);
			});
		}
	}

	private void OnRefreshAptitude(GeneralResponse e)
	{
		if(e is EquipmentAptitude)
		{
			_currPropertyCell.SetData(e as EquipmentAptitude,OnPropertyClick);
			SetCurrEquip(_currEquipCell.GetData());
		}
		else if(e is EquipmentRefreshAptitudeCost)
		{
			EquipmentModel.Instance.GetRefreshAptitudeCost(_currEquipCell.GetData().uniqueId,
				(cost) =>
				{
					SetCost(cost);
				}
			);
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

	public void SetActive(bool b)
	{
		_currPropertyCell = null;
		this.gameObject.SetActive(b);
	}

	public void Dispose()
	{
	}
}