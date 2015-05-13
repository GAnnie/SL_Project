// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TradePetModel.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.trade.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.whole.modules.trade.data;
using com.nucleus.h1.logic.whole.modules.trade.dto;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using UnityEngine;

public class TradePetModel : IDtoListenerExcute
{
	private static readonly TradePetModel _instance = new TradePetModel ();

	public event System.Action<TradePetDto> OnUpDateTradePet;

	public static TradePetModel Instance {
		get {
			return _instance;
		}
	}

	private MultipleNotifyListener _multiListener;

	private List<TradePetDto> _tradePets;
	private int _canSellAmount;

	public TradePetModel()
	{
		_tradePets = null;
	}

	private void InitListener()
	{
		if (_multiListener == null)
		{
			_multiListener = new MultipleNotifyListener();
			_multiListener.AddNotify(typeof(TradePetDto));
			_multiListener.AddNotify(typeof(TradePetCenterDto));
			_multiListener.Start(this);
		}
	}

	public void ExcuteDto(object dto)
	{
		if (dto is TradePetDto)
		{
			UpDateTradePet(dto as TradePetDto);
		}
		else if(dto is TradePetCenterDto)
		{
			_tradePets = (dto as TradePetCenterDto).items;
			if(OnUpDateTradePet != null)
				OnUpDateTradePet(null);
		}
	}

	public List<TradePetDto> GetTradePets()
	{
		return _tradePets;
	}

	public void UpDateTradePet(TradePetDto dto)
	{
		for(int index = 0;index < _tradePets.Count;index++)
		{
			if(_tradePets[index].id == dto.id)
			{
				_tradePets[index] = dto;
				break;
			}
		}

		if(OnUpDateTradePet != null)
			OnUpDateTradePet(dto);
	}

	public int GetPurchasePrice(int petId)
	{
		for(int index = 0;index < _tradePets.Count;index++)
		{
			if(_tradePets[index].id == petId)
			{
				return _tradePets[index].purchasePrice;
			}
		}
		return 0;
	}

	public bool IsPetMax(int petId)
	{
		for(int index = 0;index < _tradePets.Count;index++)
		{
			if(_tradePets[index].id == petId)
				return _tradePets[index].amount >= _tradePets[index].tradePet.maxSaleAmount;
		}
		return false;
	}

	public void PetShopEnter(System.Action OnEnterSuccess)
	{
		InitListener();

		if(_tradePets == null)
		{
			ServiceRequestAction.requestServer(TradePetWindowService.enter(),"enter", 
			(e) => {
				TradePetCenterDto dto = e as TradePetCenterDto;
				_tradePets = dto.items;
				_canSellAmount = dto.canSellAmount;

				//for(int index = 0;index < dto.items.Count;index++)
				//{
				//	TradePetDto tradePetDto = dto.items[index];
				//}

				if(OnEnterSuccess != null)
					OnEnterSuccess();
			});
		}
		else
		{
			if(OnEnterSuccess != null)
				OnEnterSuccess();
		}
	}
	
	private void PetShopExit()
	{
		ServiceRequestAction.requestServer(TradePetWindowService.exit());
	}

	public void PetBuy(TradePetDto petDto)
	{
		ServiceRequestAction.requestServer(TradePetService.buy(petDto.id),"buy", 
		(e) => {
			_canSellAmount += 1;
			PetCharactorDto dto = e as PetCharactorDto;
			PetModel.Instance.AddPet(dto);

			TipManager.AddTip(string.Format("购买了一只{0}，消耗{1}{2}",
		                        dto.name.WrapColor(ColorConstant.Color_Tip_Item),
		                        petDto.price.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency),
		                        ItemIconConst.Copper));
		});
	}

	public void PetSell(PetCharactorDto petDto,int price)
	{
		if(IsPetMax(petDto.charactorId))
		{
			TipManager.AddTip("该宠物库存已满，暂停收购");
			return;
		}
	
		if(_canSellAmount == 0)
		{
			TipManager.AddTip("今天不可以出售宠物");
			return;
		}

		ServiceRequestAction.requestServer(TradePetService.sell(petDto.id),"sell", 
		(e) => {
			_canSellAmount -= 1;
			_canSellAmount = _canSellAmount > 0 ? _canSellAmount:0;
			PetModel.Instance.RemovePetByUID(petDto.id);
			TipManager.AddTip(string.Format("出售一只{0}，获得{1}{2}(今天还可以出售{3}只宠物)",
								petDto.name.WrapColor(ColorConstant.Color_Tip_Item),
							    price.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),
							    ItemIconConst.Copper,
			                    _canSellAmount.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency)));
		});
	}

	public void Distroy()
	{
		PetShopExit();

		if (_multiListener != null)
		{
			_multiListener.Stop();
			_multiListener = null;
		}

		_tradePets = null;
	}
}