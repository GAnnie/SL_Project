// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentManufacturingViewController.cs
// Author   : willson
// Created  : 2015/1/21 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.data;

public class EquipmentManufacturingViewController : MonoBehaviourBase,IViewController
{
	private const string EquipmentOptCellName = "Prefabs/Module/EquipmentOptModule/EquipmentOptCell";
	private const string ItemCellExName = "Prefabs/Module/EquipmentOptModule/ItemCellEx";

	private EquipmentManufacturingView _view;

	private List<EquipmentOptCellController> _equipmentOptCellList;

	private List<ItemCellExController> _materialList;

	private ItemCellExController _productsEquipCell;
	private ItemCellExController _currEquipCell;
	private ItemCellExController _identificationCell;

	private List<int> _lvRange;
	private int _currEquipLv;

	private EquipmentSmithCostDto _currCost;
	private int _costIngot;

	private bool _isFastSmith;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<EquipmentManufacturingView> ();
		_view.Setup(this.transform);

		_isFastSmith = false;
		_view.QuickToggle.value = _isFastSmith;

		EquipmentModel.Instance.SetupSmith();
		_lvRange = EquipmentModel.Instance.GetEquipLvRange();

		InitEquipmentOptGroup();
		InitTopRightGroup();
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

		UIHelper.CreateBaseBtn(_view.IdentificationBtnPos,"鉴定",OnIdentificationBtn);

		EventDelegate.Set(_view.OptButton.onClick,OnManufacturingBtn);
		EventDelegate.Set(_view.QuickToggleBtn.onClick,OnQuickToggleBtn);
		//EventDelegate.Set(_view.QuickToggle.on,OnQuickToggleChange);
	}

	private void InitEquipmentOptGroup()
	{
		_equipmentOptCellList = new List<EquipmentOptCellController>(6);

		for(int index = 0;index < 6;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentOptCellName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.EquipmentGroupGrid.gameObject,prefab);
			EquipmentOptCellController cell = module.GetMissingComponent<EquipmentOptCellController>();
			cell.InitView();
			UIHelper.AdjustDepth(module,3);
			_equipmentOptCellList.Add(cell);
			cell.Hide();
		}
	}

	private void InitTopRightGroup()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ProductsPos,prefab);
		_productsEquipCell = module.GetMissingComponent<ItemCellExController>();
		_productsEquipCell.InitView();
		UIHelper.AdjustDepth(module,3);


		prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		module = GameObjectExt.AddChild(_view.CurrEquipPos,prefab);
		_currEquipCell = module.GetMissingComponent<ItemCellExController>();
		_currEquipCell.InitView();
		UIHelper.AdjustDepth(module,3);


		prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
		module = GameObjectExt.AddChild(_view.IdentificationPos,prefab);
		_identificationCell = module.GetMissingComponent<ItemCellExController>();
		_identificationCell.InitView();
		UIHelper.AdjustDepth(module,3);
	}

	private void InitMaterialGrid()
	{
		_materialList = new List<ItemCellExController>(3);

		for(int index = 0;index < 3;index++)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellExName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.MaterialGrid.gameObject,prefab);
			ItemCellExController cell = module.GetMissingComponent<ItemCellExController>();
			cell.InitView();
			UIHelper.AdjustDepth(module,3);
			_materialList.Add(cell);
		}
	}

	public void SetData()
	{
		int equipLv = PlayerModel.Instance.GetPlayerLevel()/10*10;
		_view.CostLabel.gameObject.SetActive(false);
		if(_lvRange.IndexOf(equipLv) != -1)
		{
			SetEquipment(equipLv);
		}
		else
		{
			if(_lvRange.Count == 0)
			{
				SetEquipment(equipLv);
			}
			else if(_lvRange[0] > equipLv)
			{
				SetEquipment(_lvRange[0]);
			}
			else if(_lvRange[_lvRange.Count - 1] < equipLv)
			{
				SetEquipment(_lvRange[_lvRange.Count - 1]);
			}
		}
	}

	public void SetEquipment(int equipLv)
	{
		_currEquipLv = equipLv;
		List<Equipment> equips = EquipmentModel.Instance.GetEquipLvList(_currEquipLv);

		if(equips != null)
		{
			for(int index = 0;index < equips.Count;index++)
			{
				if(index < _equipmentOptCellList.Count)
				{
					_equipmentOptCellList[index].SetData(ItemHelper.ItemIdToPackItemDto(equips[index].id),false,OnSelectEquip);
				}
			}

			OnSelectEquip(_equipmentOptCellList[0]);
		}

		_view.LBtn.gameObject.SetActive(_lvRange.Count != 0 && _lvRange[0] != equipLv);
		_view.RBtn.gameObject.SetActive(_lvRange.Count != 0 && _lvRange[_lvRange.Count - 1] != equipLv);
	}

	private void OnLBtn()
	{
		int index = _lvRange.IndexOf(_currEquipLv);
		if(index != -1 && index - 1 >= 0)
		{
			SetEquipment(_lvRange[index - 1]);
		}
	}

	private void OnRBtn()
	{
		int index = _lvRange.IndexOf(_currEquipLv);
		if(index != -1 && index + 1 < _lvRange.Count)
		{
			SetEquipment(_lvRange[index + 1]);
		}
	}

	private void OnSelectEquip(EquipmentOptCellController cell)
	{
		for(int index = 0;index < _equipmentOptCellList.Count;index++)
		{
			_equipmentOptCellList[index].SetSelect(cell == _equipmentOptCellList[index]);
		}
		
		SetCurrEquip(cell.GetData());
		SetMaterial(cell.GetData());
		SetProductsEquip(null);

		EquipmentModel.Instance.GetSmithCost(cell.GetData().itemId,
			(dto) =>
			{
				_currCost = dto;
				SetCost();
			}
		);
	}

	private void SetCost()
	{
		if(_view.QuickToggle.value)
		{
			_costIngot = _currCost.smithIngot + _currCost.identifyIngot;
		}
		else
		{
			_costIngot = _currCost.smithIngot;
		}

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

	private void SetCurrEquip(PackItemDto dto)
	{
		Equipment equip = dto.item as Equipment;

		PackItemDto curr = BackpackModel.Instance.GetEquipByPartType(equip.equipPartType);
		_currEquipCell.SetData(curr);
		if(curr != null)
		{
			_view.EquipmentPropertyLabel.text = "";
			ItemTextTipManager.Instance.ShowEquipProperty(curr,_view.EquipmentPropertyLabel);
		}
		else
		{
			_view.EquipmentPropertyLabel.text = "";
		}
	}

	private void SetMaterial(PackItemDto dto)
	{
		Equipment equip = dto.item as Equipment;

		List<EquipmentSmithMaterialByLevel> firstMaterial = PlayerModel.Instance.GetPlayerGender() == PlayerDto.Gender_Male?
			PlayerModel.Instance.GetPlayer().faction.smithMaleMaterials:PlayerModel.Instance.GetPlayer().faction.smithFemaleMaterials;

		for(int index = 0;index < firstMaterial.Count;index++)
		{
			if(firstMaterial[index].equpimentLevel == equip.equipLevel)
			{
				_materialList[0].SetData(ItemHelper.ItemIdToPackItemDto(firstMaterial[index].itemDto.itemId),firstMaterial[index].itemDto.itemCount,BackpackModel.Instance.GetItemCount(firstMaterial[index].itemDto.itemId));
			}
		}

		for(int index = 0;index < equip.smithMaterials.Count;index++)
		{
			EquipmentSmithMaterial material = equip.smithMaterials[index];
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

			if(index + 1 < _materialList.Count)
			{
				if(material.selectionItemIds[materialIndex] == H1VirtualItem.VirtualItemEnum_COPPER)
				{
					_materialList[index + 1].SetData(ItemHelper.ItemIdToPackItemDto(material.selectionItemIds[materialIndex]),material.itemCount, PlayerModel.Instance.GetWealth().copper);
				}
				else
				{
					_materialList[index + 1].SetData(ItemHelper.ItemIdToPackItemDto(material.selectionItemIds[materialIndex]),material.itemCount,BackpackModel.Instance.GetItemCount(material.selectionItemIds[materialIndex]));
				}
			}
		}
	}

	private void SetProductsEquip(PackItemDto dto)
	{
		if(dto == null)
		{
			_productsEquipCell.SetData(null);
			_view.IdentifyGroup.SetActive(false);
			_view.ProductsPropertyLabel.text = "";
			_view.newSprite.enabled = false;
		}
		else
		{
			_productsEquipCell.SetData(dto);
			_view.newSprite.enabled = true;
			Equipment equip = dto.item as Equipment;
			EquipmentExtraDto extra = dto.extra as EquipmentExtraDto;
			if(!extra.hasIdentified)
			{
				// 未鉴定,显示鉴定
				_view.IdentifyGroup.SetActive(true);
				_view.ProductsPropertyLabel.text = "";

				if(equip.identifyMaterials.Count > 0)
				{
					EquipmentSmithMaterial material = equip.identifyMaterials[0];
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
					int identificationId = material.selectionItemIds[materialIndex];
					_identificationCell.SetData(ItemHelper.ItemIdToPackItemDto(identificationId,material.itemCount),material.itemCount,BackpackModel.Instance.GetItemCount(identificationId));
				}
			}
			else
			{
				// 已鉴定,显示属性
				_view.IdentifyGroup.SetActive(false);
				_view.ProductsPropertyLabel.text = "";
				ItemTextTipManager.Instance.ShowEquipProperty(dto,_view.ProductsPropertyLabel);
			}
		}
	}

	private void SetIdentificationActive(bool b)
	{
		_view.IdentificationPos.SetActive(b);
		_view.IdentificationBtnPos.SetActive(b);
	}

	private void OnIdentificationBtn()
	{
		if(_productsEquipCell.GetData() != null)
		{
			if(!_identificationCell.isHas())
			{
				List<ItemDto> items = new List<ItemDto>();
				ItemDto dto = new ItemDto();
				dto.itemId = _identificationCell.GetData().itemId;
				dto.itemCount = _identificationCell.GetLack();
				items.Add(dto);

				ProxyTipsModule.Open("材料不足",items,_currCost.identifyIngot,(ingot)=>{
					EquipmentModel.Instance.IdentifyEquip(_productsEquipCell.GetData().uniqueId,_currCost.identifyIngot,OnUpDataProductsEquip);
				});
			}
			else
			{
				EquipmentModel.Instance.IdentifyEquip(_productsEquipCell.GetData().uniqueId,_currCost.identifyIngot,OnUpDataProductsEquip);
			}
		}
	}
	
	private void OnQuickToggleBtn()
	{
		if(_isFastSmith == false)
		{
			_view.QuickToggle.value = _isFastSmith;
			ProxyWindowModule.OpenConfirmWindow (string.Format("开启便捷打造将自动花费{0}购买缺少的打造材料和鉴定符。是否开启便捷打造？",ItemIconConst.Ingot), "便捷打造",
			()=>{
				_isFastSmith = true;
				_view.QuickToggle.value = _isFastSmith;
				SetCost();
			},null,UIWidget.Pivot.Left);
		}
		else
		{
			_isFastSmith = false;
			_view.QuickToggle.value  = _isFastSmith;
			SetCost();
		}
	}

	private void OnManufacturingBtn()
	{
		if(_costIngot <= 0)
		{
			EquipmentModel.Instance.SmithEquip(_currCost.equipmentId,_costIngot,_view.QuickToggle.value,OnUpDataProductsEquip);
		}
		else
		{
			if(_view.QuickToggle.value)
			{
				// 快捷打造
				if(PlayerModel.Instance.isEnoughIngot(_costIngot,true))
				{
					EquipmentModel.Instance.SmithEquip(_currCost.equipmentId,_costIngot,_view.QuickToggle.value,OnUpDataProductsEquip);
				}
			}
			else
			{
				List<ItemDto> items = new List<ItemDto>();
				// 物品提示
				for(int index = 0;index < _materialList.Count;index++)
				{
					ItemCellExController cell = _materialList[index];
					if(!cell.isHas())
					{
						ItemDto dto = new ItemDto();
						dto.itemId = cell.GetData().itemId;
						dto.itemCount = cell.GetLack();
						items.Add(dto);
					}
				}

				ProxyTipsModule.Open("材料不足",items,_costIngot,(ingot)=>{
					EquipmentModel.Instance.SmithEquip(_currCost.equipmentId,_costIngot,_view.QuickToggle.value,OnUpDataProductsEquip);
				});
			}
		}
		/*
		if(PlayerModel.Instance.isEnoughIngot(_costIngot, true))
		{
			EquipmentModel.Instance.SmithEquip(_currCost.equipmentId,_costIngot,_view.QuickToggle.value,OnUpDataProductsEquip);
		}
		*/
	}

	private void OnUpDataProductsEquip(GeneralResponse e)
	{
		if(e is PackItemDto)
		{
			PackItemDto dto = e as PackItemDto;
			SetProductsEquip(dto);
		}
		else if(e is EquipmentSmithCostDto)
		{
			_currCost = e as EquipmentSmithCostDto;
			SetCost();
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
			// 更新材料
			for(int index = 0;index < _materialList.Count;index++)
			{
				if(_materialList[index].GetData().itemId == H1VirtualItem.VirtualItemEnum_COPPER)
				{
					_materialList[index].SetData(_materialList[index].GetData(),_materialList[index].GetNeedCount(), PlayerModel.Instance.GetWealth().copper);
				}
				else
				{
					_materialList[index].SetData(_materialList[index].GetData(),_materialList[index].GetNeedCount(),BackpackModel.Instance.GetItemCount(_materialList[index].GetData().itemId));
				}
			}
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

		EquipmentModel.Instance.SmithCostCacheClear();
	}
}