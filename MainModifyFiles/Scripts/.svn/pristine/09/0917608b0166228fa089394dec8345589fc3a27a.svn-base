// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetWarehouseModel.cs
// Author   : willson
// Created  : 2015/3/23 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class PetWarehouseModel
{
	private static readonly PetWarehouseModel _instance = new PetWarehouseModel ();
	
	public static PetWarehouseModel Instance {
		get {
			return _instance;
		}
	}

	private PetWarehouseModel()
	{
	}

	private PetWarehouseDto _dto;

	public void Setup(System.Action funCallBack)
	{
		if(_dto != null)
		{
			if(funCallBack != null)
				funCallBack();
		}
		else
		{
			ServiceRequestAction.requestServer(PetService.warehouse(),"warehouse", 
			(e) => {
				_dto = e as PetWarehouseDto;
				/*
				if(_dto.petCharactorDtos != null && _dto.petCharactorDtos.Count > 0)
				{
					_dto.petCharactorDtos.Sort(delegate(PetCharactorDto lhs, PetCharactorDto rhs) {
						return lhs.opTime.CompareTo(rhs.opTime);
					});
				}
				*/

				if(funCallBack != null)
					funCallBack();
			});
		}
	}

	/** 仓库里的宠物列表信息 */
	public List<PetCharactorDto> GetPetCharactorDtos()
	{
		return _dto.petCharactorDtos;
	}
	
	/** 拥有的仓库格子数 */
	public int GetCapacity()
	{
		return _dto.capacity;
	}

	public void AddPet(PetCharactorDto dto)
	{
		_dto.petCharactorDtos.Add(dto);
	}

	private void RemovePet(PetCharactorDto dto)
	{
		for(int index = 0;index < _dto.petCharactorDtos.Count;index++)
		{
			if(_dto.petCharactorDtos[index].id == dto.id)
			{
				_dto.petCharactorDtos.RemoveAt(index);
				break;
			}
		}
	}

	//宠物移入仓库
	public void MoveTo(PetCharactorDto dto,System.Action funCallBack)
	{
		ServiceRequestAction.requestServer(PetService.moveTo(dto.id),"MoveTo", 
		(e) => {
			PetModel.Instance.RemovePetByUID(dto.id);
			AddPet(dto);
			if(funCallBack != null)
				funCallBack();
		});
	}

	//宠物移出仓库
	public void MoveOut(PetCharactorDto dto,System.Action funCallBack)
	{
		ServiceRequestAction.requestServer(PetService.moveOut(dto.id),"MoveOut", 
		(e) => {
			PetModel.Instance.AddPet(dto);
			RemovePet(dto);
			if(funCallBack != null)
				funCallBack();
		});
	}
			
	//仓库扩充容量
	public void ExpandWarehouseCapacity(System.Action funCallBack)
	{
		ServiceRequestAction.requestServer(PetService.expandWarehouseCapacity(),"ExpandWarehouseCapacity", 
		(e) => {
			_dto.capacity += 1;
			//TipManager.AddTip(消耗了【数量】铜币/元宝，宠物仓库容量增加了1格)
			if(funCallBack != null)
				funCallBack();
		});
	}

	public void DropPet(PetCharactorDto petDto,System.Action funCallBack)
	{
		Pet pet = petDto.charactor as Pet;
		if(pet.petType == Pet.PetType_Precious || pet.petType == Pet.PetType_Myth)
		{
			TipManager.AddTip("珍兽和神兽不允许放生");
			return;
		}

		if(petDto.name == pet.name)
		{
			if(petDto.joinedBattle && (petDto.ifBaobao || petDto.ifMutate)){
				ProxyWindowModule.OpenConfirmWindow(string.Format("确认要放生{0}吗？放生后可在仓库管理员处找回",petDto.name.WrapColor("ff0000")),"",
				()=>{
					ServiceRequestAction.requestServer(PetService.dropPet(petDto.id),"DropPet",(e) => {
						RemovePet(petDto);
						if(funCallBack != null)
							funCallBack();
					});
				});
			}else{
				ProxyWindowModule.OpenConfirmWindow(string.Format("确认要放生{0}吗？放生后无法找回",petDto.name.WrapColor("ff0000")),"",
				()=>{
					ServiceRequestAction.requestServer(PetService.dropPet(petDto.id),"DropPet",(e) => {
						RemovePet(petDto);
						if(funCallBack != null)
							funCallBack();
					});
				});
			}
		}
		else
		{
			TipManager.AddTip("需要将宠物改为默认的名称才能放生");
		}

	}
}