// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MyTradePetInfoCellController.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************

public class MyTradePetInfoCellController : MonoBehaviourBase,IViewController
{
	private MyTradePetInfoCell _view;
	private PetPropertyInfo _petInfo;

	private int _price;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<MyTradePetInfoCell> ();
		_view.Setup(this.transform);
		_view.SelectSprite.enabled = false;
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"出售",OnSellOpt);
	}

	public void SetData(PetPropertyInfo petInfo)
	{
		this.gameObject.SetActive(true);
		_petInfo = petInfo;
		_view.NameLabel.text = petInfo.petDto.name;
		_view.LvLabel.text = petInfo.petDto.level.ToString();

		_price = TradePetModel.Instance.GetPurchasePrice(petInfo.petDto.charactorId);
		_view.PriceLabel.text = _price.ToString();
	}
	
	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	private void OnSellOpt()
	{
		TradePetModel.Instance.PetSell(_petInfo.petDto,_price);
	}

	public void Dispose()
	{
	}
}