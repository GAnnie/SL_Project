// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NPCPetWarehouseViewController.cs
// Author   : willson
// Created  : 2015/3/20 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.player.dto;

public class NPCPetWarehouseViewController : MonoBehaviourBase,IViewController
{
	private const string PetInfoCellName = "Prefabs/Module/PetWarehouseModule/PetInfoCell";

	private NPCPetWarehouseView _view;

	private List<PetInfoCellController> _bodyCells;
	private List<PetInfoCellController> _warehouseCells;

	private PetInfoCellController _currCell;

	public void SetActive(bool b)
	{
		this.gameObject.SetActive(b);
	}
	
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<NPCPetWarehouseView> ();
		_view.Setup(this.transform);

		_bodyCells = new List<PetInfoCellController>();
		_warehouseCells = new List<PetInfoCellController>();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"放生",OnOptBtn);
		PetModel.Instance.OnPetInfoListUpdate += ShowBodyPets;
	}

	private PetInfoCellController AddPetInfo(PetCharactorDto petDto,bool isBattlePet,System.Action<PetInfoCellController> onClickCallBack,UIGrid grid)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( PetInfoCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(grid.gameObject,prefab);
		PetInfoCellController cell = module.GetMissingComponent<PetInfoCellController>();
		cell.InitView();
		cell.SetData(petDto,isBattlePet,onClickCallBack,OnSelectPet);

		//grid.Reposition();
		return cell;
	}

	public void SetData()
	{
		_bodyCells.Clear();
		_warehouseCells.Clear();

		ShowBodyPets();
		ShowWarehousePets();
	}

	private void ShowBodyPets()
	{
		_currCell = null;

		List<PetPropertyInfo> bodyList = PetModel.Instance.GetPetPropertyInfoList();
		int index = 0;
		if(bodyList != null)
		{
			for(;index < bodyList.Count;index++)
			{
				if(index < _bodyCells.Count)
				{
					_bodyCells[index].SetData(bodyList[index].petDto,index == PetModel.Instance.GetBattlePetIndex(),OnMoveToPetWarehouse,OnSelectPet);
				}
				else
				{
					PetInfoCellController cell = AddPetInfo(bodyList[index].petDto,index == PetModel.Instance.GetBattlePetIndex(),OnMoveToPetWarehouse,_view.BodyGrid);
					_bodyCells.Add(cell);
				}
			}
		}

		while(index < _bodyCells.Count)
		{
			_bodyCells[index].Hide();
			index++;
		}

		_view.BodyGrid.Reposition();
	}

	private void ShowWarehousePets()
	{
		_currCell = null;

		List<PetCharactorDto> petCharactorDtos = PetWarehouseModel.Instance.GetPetCharactorDtos();
		int index = 0;

		if(petCharactorDtos != null)
		{
			for(;index < petCharactorDtos.Count;index++)
			{
				if(index < _warehouseCells.Count)
				{
					_warehouseCells[index].SetData(petCharactorDtos[index],false,OnMoveToBody,OnSelectPet);
					_warehouseCells[index].isAddCapacity = false;
				}
				else
				{
					PetInfoCellController cell = AddPetInfo(petCharactorDtos[index],false,OnMoveToBody,_view.WarehouseGrid);
					cell.isAddCapacity = false;
					_warehouseCells.Add(cell);
				}
			}
		}

		while(index < PetWarehouseModel.Instance.GetCapacity())
		{
			if(index < _warehouseCells.Count)
			{
				_warehouseCells[index].SetData(null,false,OnMoveToBody,OnSelectPet);
				_warehouseCells[index].isAddCapacity = false;
			}
			else
			{
				PetInfoCellController cell = AddPetInfo(null,false,OnMoveToBody,_view.WarehouseGrid);
				cell.isAddCapacity = false;
				_warehouseCells.Add(cell);
			}
			index++;
		}

		// 加号图标
		if(PetWarehouseModel.Instance.GetCapacity() < DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_WAREHOUSE_MAX_CAPACITY))
		{
			if(index < _warehouseCells.Count)
			{
				if(_warehouseCells[index].isAddCapacity == false)
				{
					_warehouseCells[index].SetData(null,false,OnMoveToBody,OnSelectPet);
					_warehouseCells[index].isAddCapacity = true;
				}
			}
			else
			{
				PetInfoCellController cell = AddPetInfo(null,false,OnMoveToBody,_view.WarehouseGrid);
				cell.isAddCapacity = true;
				_warehouseCells.Add(cell);
			}
			index++;
		}

		while(index < _warehouseCells.Count)
		{
			_warehouseCells[index].Hide();
			index++;
		}

		_view.WarehouseGrid.Reposition();
	}

	private void OnSelectPet(PetInfoCellController cell)
	{
		for(int index = 0;index < _bodyCells.Count;index++)
		{
			_bodyCells[index].SetSelect(_bodyCells[index] == cell);
		}

		for(int index = 0;index < _warehouseCells.Count;index++)
		{
			_warehouseCells[index].SetSelect(_warehouseCells[index] == cell);
		}

		_currCell = cell;
	}

	private void OnMoveToPetWarehouse(PetInfoCellController cell)
	{
		PetWarehouseModel.Instance.MoveTo(cell.GetData(),()=>
		{
			ShowBodyPets();
			ShowWarehousePets();
		});
	}

	private void OnMoveToBody(PetInfoCellController cell)
	{
		if(cell.isAddCapacity)
		{
			ProxyTipsModule.Open("扩充宠物仓库",3,DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_WAREHOUSE_EXPAND_CONSUME_COPPER),ExpandCapability);
		}
		else
		{
			if(PetModel.Instance.isFullPet != true)
			{
				PetWarehouseModel.Instance.MoveOut(cell.GetData(),()=>
				{
					ShowBodyPets();
					ShowWarehousePets();
				});
			}
			else
			{
				TipManager.AddTip("你携带的宠物已满，无法取出");
			}
		}
	}

	private void ExpandCapability(List<ItemDto> costItems)
	{
		PetWarehouseModel.Instance.ExpandWarehouseCapacity(()=>
		{
			ShowBodyPets();
			ShowWarehousePets();
			GameLogicHelper.HandlerItemTipDto(costItems,"宠物仓库容量增加了1格");
		});
	}
		
	private void OnOptBtn()
	{
		if(_currCell != null && _currCell.GetData() != null)
		{
			// GetPetIndex(petUID)
			int index = PetModel.Instance.GetPetIndex(_currCell.GetData().id);
			if(index != -1)
			{
				// 身上
				PetModel.Instance.DropPet(index);
			}
			else
			{
				// 仓库
				PetWarehouseModel.Instance.DropPet(_currCell.GetData(),ShowWarehousePets);
			}
		}
		else
		{
			TipManager.AddTip("请选择要放生的宠物");
		}
	}

	public void Dispose()
	{
		PetModel.Instance.OnPetInfoListUpdate -= ShowBodyPets;
	}
}