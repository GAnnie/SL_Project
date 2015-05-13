// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetResumeWinUIController.cs
// Author   : willson
// Created  : 2015/3/18 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class PetResumeWinUIController : MonoBehaviour,IViewController
{
	private const string PetResumeCellName = "Prefabs/Module/PetResumeModule/PetResumeCell";

	private PetResumeWinUI _view;

	private List<PetPropertyInfo> _petPropertyInfoList;
	private List<PetResumeCellController> _cells;
	private PetResumeCellController _curSelectCell;
	private CostButton _costBtn;

	private PetBaseInfoViewController _petBaseInfoViewController;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetResumeWinUI> ();
		_view.Setup(this.transform);
		_cells = new List<PetResumeCellController>();

		RegisterEvent ();

		_petBaseInfoViewController = UIModuleManager.Instance.
			AddChildPanel(ProxyPetPropertyModule.PETBASEINFO_VIEW,_view.LGroup).GetMissingComponent<PetBaseInfoViewController>();
		_petBaseInfoViewController.InitView();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);
		_costBtn = UIHelper.CreateCostBtn(_view.CostBtnPos,"找回","ICON_1",OnResume);
	}

	private PetResumeCellController AddPetResumeCell()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( PetResumeCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.PetGrid.gameObject,prefab);
		PetResumeCellController cell = module.GetMissingComponent<PetResumeCellController>();
		cell.InitView();
		return cell;
	}
	
	public void SetData(List<object> petDtos)
	{
		_petPropertyInfoList = new List<PetPropertyInfo>(petDtos.Count);
		for(int i=0;i<petDtos.Count;++i){
			_petPropertyInfoList.Add(new PetPropertyInfo(petDtos[i] as PetCharactorDto));
		}

		if(_petPropertyInfoList.Count > 1)
		{
			_petPropertyInfoList.Sort(delegate(PetPropertyInfo lhs, PetPropertyInfo rhs) {
				return rhs.petDto.opTime.CompareTo(lhs.petDto.opTime);
			});
		}

		UpdatePetListView();
	}

	private void UpdatePetListView(){
		int index = 0;
		for(;index < _petPropertyInfoList.Count;index++)
		{
			PetPropertyInfo petInfo =_petPropertyInfoList[index];
			if(index < _cells.Count)
			{
				_cells[index].SetData(petInfo,OnSelectPet);
			}
			else
			{
				PetResumeCellController cell = AddPetResumeCell();
				cell.SetData(petInfo,OnSelectPet);
				_cells.Add(cell);
			}
		}
		
		while(index < _cells.Count)
		{
			_cells[index].Hide();
			index++;
		}
		
		if(_petPropertyInfoList.Count > 0)
		{
			OnSelectPet(_cells[0]);
		}
		else
		{
			OnSelectPet(null);
		}
		
		_view.PetGrid.Reposition();
	}

	private void OnSelectPet(PetResumeCellController cell)
	{
		_curSelectCell = cell;
		if(_curSelectCell != null)
		{
			for(int index = 0;index < _cells.Count;index++)
			{
				_cells[index].SetSelect(_cells[index] == cell);
			}

			PetPropertyInfo petInfo = _curSelectCell.GetData();
			// show pet info
			ShowselectPet(petInfo);

			Pet pet = petInfo.pet;
			if(pet != null)
			{
				PetResume cost = DataCache.getDtoByCls<PetResume>(pet.companyLevel);
				if(cost != null)
				{
					_costBtn.Cost = cost.ingot;
					return;
				}
			}

			_costBtn.Cost = 0;
		}
		else
		{
			_costBtn.Cost = 0;
			ShowselectPet(null);
		}

	}

	private void ShowselectPet(PetPropertyInfo petInfo)
	{
		// todo
		if(petInfo != null)
		{
			_petBaseInfoViewController.ShowPetDetailInfo(petInfo);
		}
		else
		{
			_petBaseInfoViewController.CleanupPetDetialInfo();
		}
	}

	private void OnResume()
	{
		if(_curSelectCell == null)
		{
			TipManager.AddTip("没有可以找回的宠物");
			return;
		}

		if(!PlayerModel.Instance.isEnoughIngot(_costBtn.Cost,true))
			return;

		PetPropertyInfo petInfo = _curSelectCell.GetData();
		ServiceRequestAction.requestServer(PetService.resume(petInfo.petDto.id),"resume",(e) => {
			TipManager.AddTip(string.Format("成功找回{0}",petInfo.petDto.name));

			for(int index = 0;index < _petPropertyInfoList.Count;index++)
			{
				if(_petPropertyInfoList[index] == petInfo)
				{
					_petPropertyInfoList.RemoveAt(index);
					break;
				}
			}

			UpdatePetListView();
			PetModel.Instance.AddPet(petInfo);
		});
	}

	private void OnClose()
	{
		ProxyPetResumeModule.Close();
	}

	public void Dispose()
	{

	}
}