// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentGemTransferViewController.cs
// Author   : willson
// Created  : 2015/2/7 
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
using com.nucleus.h1.logic.core.modules;

public class EquipmentGemTransferViewController : MonoBehaviourBase,IViewController
{
	private const string EquipmentOptCellName = "Prefabs/Module/EquipmentOptModule/EquipmentOptCell";
	private const string ItemCellExName = "Prefabs/Module/EquipmentOptModule/ItemCellEx";

	private EquipmentGemTransferView _view;

	private List<EquipmentOptCellController> _equipmentOptCellList;

	private ItemCellExController _targetEquipCell;
	private ItemCellExController _currEquipCell;
	private ItemCellExController _materialCell;

	private const int PAGE_COUNT = 6;
	
	private List<PackItemDto> _equips;
	private Summary _summary;

	private int _stateType = 0;
	private const int StateType_Init = 0; // 初始化状态
	private const int StateType_Target = 1; // 选择了转移方装备

	private int _costIngot;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentGemTransferView> ();
		_view.Setup(this.transform);

		_view.LButton.gameObject.SetActive(false);
		_view.RButton.gameObject.SetActive(false);

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
		EventDelegate.Set(_view.LButton.onClick,OnLBtn);
		EventDelegate.Set(_view.RButton.onClick,OnRBtn);

		EventDelegate.Set(_view.OptButton.onClick,OnEmbedTransfer);
	}

	private void AddEquipmentOptCell()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentOptCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.EquipmentGroup.gameObject,prefab);
		EquipmentOptCellController cell = module.GetMissingComponent<EquipmentOptCellController>();
		cell.InitView();
		UIHelper.AdjustDepth(module,3);
		_equipmentOptCellList.Add(cell);
		cell.Hide();
	}

	private void InitTopRightGroup()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.TargetEquipPos,prefab);
		_targetEquipCell = module.GetMissingComponent<ItemCellExController>();
		_targetEquipCell.InitView();
		_targetEquipCell.SetTakeOffBtn(OnTargetTakeOff);
		UIHelper.AdjustDepth(module,3);
		
		
		prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		module = GameObjectExt.AddChild(_view.CurrEquipPos,prefab);
		_currEquipCell = module.GetMissingComponent<ItemCellExController>();
		_currEquipCell.InitView();
		_currEquipCell.SetTakeOffBtn(OnCurrTakeOff);
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
					if(_stateType == StateType_Init)
					{
						// 宝石等级小于5,显示为灰色
						_equipmentOptCellList[index].Enabled = ItemHelper.GetEquipGemLv(_equips[equipIndex]) >= 5;
						_equipmentOptCellList[index].GemTransferSpriteEnabled = false;
					}
					else if(_stateType == StateType_Target)
					{
						_equipmentOptCellList[index].Enabled = true;
                        _equipmentOptCellList[index].GemTransferSpriteEnabled = false;
                        _equipmentOptCellList[index].SetSelect(false);

						if(_currEquipCell.GetData() != null && _currEquipCell.GetData() == _equips[equipIndex])
						{
							_equipmentOptCellList[index].SetGemTransferSprite(true);
                            _equipmentOptCellList[index].SetSelect(true);
						}

                        if (_targetEquipCell.GetData() != null && _targetEquipCell.GetData() == _equips[equipIndex])
						{
							_equipmentOptCellList[index].SetGemTransferSprite(false);
                            _equipmentOptCellList[index].SetSelect(true);
						}
					}
					index++;
				}
			}
			
			while (index < _equipmentOptCellList.Count)
			{
				_equipmentOptCellList[index].Hide();
				index++;
			}

			_view.LButton.gameObject.SetActive(page != 1);
			_view.RButton.gameObject.SetActive(page != _summary.getTotalPage());
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
		if(_stateType == StateType_Init)
		{
			for(int index = 0;index < _equipmentOptCellList.Count;index++)
			{
				_equipmentOptCellList[index].SetSelect(cell == _equipmentOptCellList[index]);
			}

			SetCurrEquip(cell.GetData());
			TargetStateType();
		}
		else if(_stateType == StateType_Target)
		{
            if (cell.GetData() == _currEquipCell.GetData())
                return;

			for(int index = 0;index < _equipmentOptCellList.Count;index++)
			{
				if(_equipmentOptCellList[index].GetData() != _currEquipCell.GetData())
				{
					if(cell == _equipmentOptCellList[index])
					{
						_equipmentOptCellList[index].SetSelect(true);
						SetTargetEquip(cell.GetData());
						cell.SetGemTransferSprite(false);
					}
					else
					{
						_equipmentOptCellList[index].SetSelect(false);
						_equipmentOptCellList[index].GemTransferSpriteEnabled = false;
					}
				}
			}
		}

		SetMaterial();
	}
	
	private void SetCurrEquip(PackItemDto dto)
	{
		_currEquipCell.SetData(dto);
		if(dto != null)
		{
            _view.LTipsLabel.text = "选择受益装备";
			_view.RuleTipsLabel.text = "转移方装备";
			_view.RuleLabel.enabled = false;
			_view.CurrEquipInfoGroup.SetActive(true);
			_view.EquipmentPropertyLabel.text = "";
			ItemTextTipManager.Instance.ShowEquipProperty(dto,_view.EquipmentPropertyLabel);
		}
		else
		{
            _view.LTipsLabel.text = "选择转移装备";
			_view.RuleTipsLabel.text = "转移规则";
			_view.RuleLabel.enabled = true;
			_view.CurrEquipInfoGroup.SetActive(false);
			_view.EquipmentPropertyLabel.text = "";
		}
	}

	private void SetTargetEquip(PackItemDto dto)
	{
		if(dto != null)
		{
			_view.TargetEquipmentPropertyLabel.text = "";
			ItemTextTipManager.Instance.ShowEquipProperty(dto,_view.TargetEquipmentPropertyLabel);
		}
		else
		{
			_view.TargetEquipmentPropertyLabel.text = "";

            for (int index = 0; index < _equipmentOptCellList.Count; index++)
            {
                if (_equipmentOptCellList[index].GetData() == _targetEquipCell.GetData())
                {
                    if(!_equipmentOptCellList[index].IsHide())
                    {
                        _equipmentOptCellList[index].SetSelect(false);
                        _equipmentOptCellList[index].GemTransferSpriteEnabled = false;
                    }
                    break;
                }
            }
		}

        _targetEquipCell.SetData(dto);
	}

	private void SetMaterial()
	{
		// 材料固定 常量 银币数
		int silver = DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_EMBED_TRANSFER_COST);
		_materialCell.SetData(ItemHelper.ItemIdToPackItemDto(H1VirtualItem.VirtualItemEnum_SILVER),silver, (long)PlayerModel.Instance.GetWealth().silver);

		// 花费元宝
		int needSilver = silver - PlayerModel.Instance.GetWealth().silver;
		if(needSilver > 0)
		{
			SetCost(CurrencyExchange.SilverToIngot(needSilver));
		}
		else
		{
			SetCost(0);
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

	public void SetData()
	{
		_stateType = StateType_Init;
		_equips = BackpackModel.Instance.GetAllIdentifiedEquip(0,true);
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

		SetTargetEquip(null);
		SetCurrEquip(null);
	}
	
	private void TargetStateType()
	{
		_stateType = StateType_Target;

		_equips = BackpackModel.Instance.GetGemTransferEquip(_currEquipCell.GetData());
		int iTotalPage = (int)Math.Ceiling((double)_equips.Count / PAGE_COUNT);
		_summary = Summary.create(_equips.Count,iTotalPage,1,PAGE_COUNT);
		
		SetPage(1);
	}

	private void OnTargetTakeOff(ItemCellExController cell)
	{
		SetTargetEquip(null);
	}

	private void OnCurrTakeOff(ItemCellExController cell)
	{
		SetData();
	}

	private void OnEmbedTransfer()
	{
		if(_currEquipCell.GetData() == null)
		{
			TipManager.AddTip("请选择转移方装备");
			return;
		}

		if(_targetEquipCell.GetData() == null)
		{
			TipManager.AddTip("请选择受益方装备");
			return;
		}

		if(_costIngot <= 0)
		{
			EquipmentModel.Instance.EmbedTransfer(_currEquipCell.GetData(),_targetEquipCell.GetData(),() =>
			{
				SetCurrEquip(_currEquipCell.GetData());
				SetTargetEquip(_targetEquipCell.GetData());
			});
		}
		else
		{
			ProxyTipsModule.Open("材料不足",H1VirtualItem.VirtualItemEnum_SILVER,DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_EMBED_TRANSFER_COST),(ingot)=>{
				EquipmentModel.Instance.EmbedTransfer(_currEquipCell.GetData(),_targetEquipCell.GetData(),() =>
				{
					SetCurrEquip(_currEquipCell.GetData());
					SetTargetEquip(_targetEquipCell.GetData());
				});
			});
		}


	}

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}

	public void Dispose()
	{
	}
}