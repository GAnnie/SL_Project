// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetShopWinUIController.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using com.nucleus.h1.logic.whole.modules.trade.dto;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PetShopWinUIController : MonoBehaviourBase,IViewController
{
	private const string TradePetLvName = "Prefabs/Module/TradeModule/TradePet/TradePetLvCell";
	private const string TradePetInfoCellName = "Prefabs/Module/TradeModule/TradePet/TradePetInfoCell";
	private const string MyTradePetInfoCellName = "Prefabs/Module/TradeModule/TradePet/MyTradePetInfoCell";

	private PetShopWinUI _view;

	private List<TradePetLvCellController> _tradePetLvCells;
	private List<TradePetInfoCellController> _tradePetInfoCells;
	private List<MyTradePetInfoCellController> _myTradePetInfoCel;
	
	private TradePetInfoCellController _currCell;
	private int _currTradePetLv = 0;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetShopWinUI> ();
		_view.Setup(this.transform);

		_tradePetLvCells = new List<TradePetLvCellController>();
		_tradePetInfoCells = new List<TradePetInfoCellController>();
		_myTradePetInfoCel = new List<MyTradePetInfoCellController>();

		OnWealthChanged(PlayerModel.Instance.GetWealth());

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;

		TradePetModel.Instance.OnUpDateTradePet += OnUpDateTradePet;

		PetModel.Instance.OnPetInfoListUpdate += ShowMyTradePet;

		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"购买",OnBuyOpt);
	}

	public void SetData()
	{
		ShowTradePetLv();
		ShowMyTradePet();
	}

	private void ShowTradePetLv()
	{
		List<TradePetDto> tradePets = TradePetModel.Instance.GetTradePets();
		List<int> tradePetLv = new List<int>();
		for(int index = 0;index < tradePets.Count;index++)
		{
			TradePetDto tradePetDto = tradePets[index];
			if(tradePetDto != null && tradePetDto.tradePet != null 
			   && tradePetLv.IndexOf(tradePetDto.tradePet.petLevel) == -1 
			   && tradePetDto.tradePet.visibleLevel <= PlayerModel.Instance.GetPlayerLevel())
			{
				tradePetLv.Add(tradePetDto.tradePet.petLevel);
			}
		}
		tradePetLv.Sort();
		for(int index = 0;index < tradePetLv.Count;index++)
		{
			AddTradePetLvCell(tradePetLv[index]);
		}

		if(_tradePetLvCells.Count > 0)
			OnSelectTradePetLv(_tradePetLvCells[_tradePetLvCells.Count - 1]);
	}

	private void AddTradePetLvCell(int lv)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( TradePetLvName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.PetLvGrid.gameObject,prefab);
		TradePetLvCellController cell = module.GetMissingComponent<TradePetLvCellController>();
		cell.InitView();
		cell.SetData(lv,OnSelectTradePetLv);
		//cell.TradePetLv = lv;
		_tradePetLvCells.Add(cell);
		_view.PetLvGrid.Reposition();
	}

	private void OnSelectTradePetLv(TradePetLvCellController cell)
	{
		for(int index = 0;index < _tradePetLvCells.Count;index++)
		{
			_tradePetLvCells[index].SetSelect(_tradePetLvCells[index] == cell);
		}

		_currTradePetLv = cell.TradePetLv;
		ShowTradePetInfo(_currTradePetLv);
	}

	private void ShowTradePetInfo(int lv)
	{
		int count = 0;
		List<TradePetDto> tradePets = TradePetModel.Instance.GetTradePets();
		for(int index = 0;index < tradePets.Count;index++)
		{
			TradePetDto tradePetDto = tradePets[index];
			if(tradePetDto.tradePet.petLevel == lv
			   && tradePetDto.tradePet.visibleLevel <= PlayerModel.Instance.GetPlayerLevel())
			{
				if(count < _tradePetInfoCells.Count)
				{
					_tradePetInfoCells[count].SetData(tradePetDto,OnSelectTradePetInfo);
				}
				else
				{
					AddTradePetInfoCell(tradePetDto);
				}
				count++;
			}
		}

		while(count < _tradePetInfoCells.Count)
		{
			_tradePetInfoCells[count].Hide();
			count++;
		}

		if(_tradePetInfoCells.Count > 0)
			OnSelectTradePetInfo(_tradePetInfoCells[0]);
	}

	private void AddTradePetInfoCell(TradePetDto tradePetDto)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( TradePetInfoCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.PetInfoGrid.gameObject,prefab);
		TradePetInfoCellController cell = module.GetMissingComponent<TradePetInfoCellController>();
		cell.InitView();
		cell.SetData(tradePetDto,OnSelectTradePetInfo);
		_tradePetInfoCells.Add(cell);

		_view.PetInfoGrid.Reposition();
	}

	private void OnSelectTradePetInfo(TradePetInfoCellController cell)
	{
		for(int index = 0;index < _tradePetInfoCells.Count;index++)
		{
			_tradePetInfoCells[index].SetSelect(_tradePetInfoCells[index] == cell);
		}

		if(PlayerModel.Instance.isEnoughCopper(cell.GetData().price))
		{
			_view.priceLbl.text = string.Format("[{0}]{1}[-]","78f378",cell.GetData().price);
		}
		else
		{
			_view.priceLbl.text = string.Format("[{0}]{1}[-]","f57a7a",cell.GetData().price);
		}

		_currCell = cell;
	}

	private void ShowMyTradePet()
	{
		List<PetPropertyInfo> bodyList = PetModel.Instance.GetPetPropertyInfoList();
		int count = 0;
		if(bodyList != null)
		{
			for(int index = 0;index < bodyList.Count;index++)
			{
				if(PetModel.Instance.isNaturePetAndNotInBattle(bodyList[index])
				   && TradePetModel.Instance.GetPurchasePrice(bodyList[index].petDto.charactorId) > 0)
				{
					if(count < _myTradePetInfoCel.Count)
					{
						_myTradePetInfoCel[count].SetData(bodyList[index]);
					}
					else
					{
						AddMyTradePet(bodyList[index]);
					}

					count++;
				}
			}

			while(count < _myTradePetInfoCel.Count)
			{
				_myTradePetInfoCel[count].Hide();
				count++;
			}
		}
	}

	private void AddMyTradePet(PetPropertyInfo petInfo)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( MyTradePetInfoCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.MyPetGrid.gameObject,prefab);
		MyTradePetInfoCellController cell = module.GetMissingComponent<MyTradePetInfoCellController>();
		cell.InitView();
		cell.SetData(petInfo);
		_myTradePetInfoCel.Add(cell);
		
		_view.MyPetGrid.Reposition();
	}

	private void OnUpDateTradePet(TradePetDto dto)
	{
		if(dto != null)
		{
			for(int index = 0;index < _tradePetInfoCells.Count;index++)
			{
				if(_tradePetInfoCells[index].GetData().id == dto.id)
				{
					if(_tradePetInfoCells[index].IsShow())
						_tradePetInfoCells[index].SetData(dto,OnSelectTradePetInfo);
					break;
				}
			}
		}
		else
		{
			ShowTradePetInfo(_currTradePetLv);
		}
	}

	private void OnWealthChanged(WealthNotify notify)
	{
		_view.hasLbl.text = notify.copper.ToString();

		if(_currCell != null && _currCell.GetData() != null)
		{
			if(PlayerModel.Instance.isEnoughCopper(_currCell.GetData().price))
			{
				_view.priceLbl.text = string.Format("[{0}]{1}[-]","78f378",_currCell.GetData().price);
			}
			else
			{
				_view.priceLbl.text = string.Format("[{0}]{1}[-]","f57a7a",_currCell.GetData().price);
			}
		}
	}

	private void OnBuyOpt()
	{
		if(PetModel.Instance.isFullPet)
		{
			TipManager.AddTip("身上的宠物已满");
			return;
		}

		if(_currCell != null && _currCell.GetData() != null 
		   && PlayerModel.Instance.isEnoughCopper(_currCell.GetData().price,true))
		{
			if(_currCell.GetData().amount > 0)
			{
				TradePetModel.Instance.PetBuy(_currCell.GetData());
			}
			else
			{
				TipManager.AddTip("该宠物暂时缺货");
			}
		}
	}

	private void OnCloseBtn()
	{
		ProxyTradePetModule.Close();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
		TradePetModel.Instance.OnUpDateTradePet -= OnUpDateTradePet;
		PetModel.Instance.OnPetInfoListUpdate -= ShowMyTradePet;
		//TradePetModel.Instance.PetShopExit();
		TradePetModel.Instance.Distroy();
	}
}