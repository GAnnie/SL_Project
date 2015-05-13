// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ShopItemCellVontroller.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.shop.data;

public class ShopItemCellController : MonoBehaviourBase,IViewController
{
	private ShopItemCell _view;
	private ShopItem _dto;

	private int _remain;

	private System.Action<ShopItemCellController> _OnClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ShopItemCell>();
		_view.Setup(this.transform);

		_remain = -1;
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.ShopItemBtn.onClick,OnShopItemBtn);
	}

	public void SetData(ShopItem dto,System.Action<ShopItemCellController> OnClickCallBack,int remain = -1)
	{
		_dto = dto;
		_OnClickCallBack = OnClickCallBack;

		if(_view.ItemIcon.atlas.GetSprite(_dto.item.icon) != null)
			_view.ItemIcon.spriteName = _dto.item.icon;
		else
			_view.ItemIcon.spriteName = "0";

		_view.NameLabel.text = _dto.item.name;
		_view.CostLabel.text = _dto.virtualCount.ToString();
		_view.CostIconSprite.spriteName = ItemIconConst.GetIconAltasConstByItemId(_dto.virtualItemId);

		SetRestrictCount(remain);
	}

	public void SetRestrictCount(int remain)
	{
		_remain = remain;
		if(_dto.restrictCount > 0)
		{
			_view.ItemCountLabel.text = remain.ToString();
		}
		else
		{
			_view.ItemCountLabel.text = "";
		}
	}

	public int GetRemain()
	{
		return _remain;
	}

	public bool IsMax(int count)
	{
		if(_remain != -1 && _remain < count)
		{
			return true;
		}

		return false;
	}

	public ShopItem GetData()
	{
		return _dto;
	}

	public void OnShopItemBtn()
	{
		if(_OnClickCallBack != null)
			_OnClickCallBack(this);
	}

	public void SetSelect(bool b)
	{
		if(b)
		{
			_view.SelectSprite.spriteName = "the-no-choice-lines";
			_view.ShopItemBtn.normalSprite = "the-no-choice-lines";
		}
		else
		{
			_view.SelectSprite.spriteName = "the-choice-lines";
			_view.ShopItemBtn.normalSprite = "the-choice-lines";
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