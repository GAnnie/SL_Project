// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TradePetInfoCellController.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.whole.modules.trade.dto;

public class TradePetInfoCellController : MonoBehaviourBase,IViewController
{
	private TradePetInfoCell _view;

	private TradePetDto _tradePetDto;

	private System.Action<TradePetInfoCellController> _OnClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<TradePetInfoCell> ();
		_view.Setup(this.transform);
		_view.SelectSprite.enabled = false;
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		
	}

	public void SetData(TradePetDto tradePetDto,System.Action<TradePetInfoCellController> OnClickCallBack)
	{
		this.gameObject.SetActive(true);

		_tradePetDto = tradePetDto;
		_OnClickCallBack = OnClickCallBack;

		_view.NameLabel.text = tradePetDto.tradePet.pet.name;
		_view.PriceLabel.text = tradePetDto.price.ToString();
		_view.CountLabel.text = tradePetDto.amount.ToString();
	}

	public TradePetDto GetData()
	{
		return _tradePetDto;
	}

	void OnClick()
	{
		if(_OnClickCallBack != null)
			_OnClickCallBack(this);
	}

	public void SetSelect(bool b)
	{
		_view.SelectSprite.enabled = b;
	}

	public bool IsShow()
	{
		return this.gameObject.activeSelf;
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void Dispose()
	{
	}
}