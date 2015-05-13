// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MallShoppingModel.cs
// Author   : willson
// Created  : 2015/1/28 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.shop.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.shop.data;

public class MallShoppingModel
{
	private static readonly MallShoppingModel _instance = new MallShoppingModel ();
	
	public static MallShoppingModel Instance {
		get {
			return _instance;
		}
	}

	private Dictionary<int, ShopItemRestrictDto> _itemsDic;

	private MallShoppingModel()
	{
		_itemsDic = new Dictionary<int, ShopItemRestrictDto>();
	}

	public void UpData(System.Action onSuccess)
	{
		_itemsDic.Clear();
		ServiceRequestAction.requestServer(ShopService.week(),"week", 
		(e) => {
			DataList list = e as DataList;
			for(int index = 0;index < list.items.Count;index++)
			{
				ShopItemRestrictDto dto = list.items[index] as ShopItemRestrictDto;
				_itemsDic[dto.shopItemId] = dto;
			}

			if(onSuccess != null)
				onSuccess();
		});
	}

	public int GetCount(ShopItem item)
	{
		if(item.restrictCount <= 0)
		{
			return -1;
		}

		if(_itemsDic.ContainsKey(item.id))
		{
			int count = item.restrictCount - _itemsDic[item.id].buyCount;
			return count >= 0?count:0;
		}
		else
		{
			return item.restrictCount;
		}
	}

	public int BuyItem(ShopItem item,int count)
	{
		if(_itemsDic.ContainsKey(item.id))
		{
			_itemsDic[item.id].buyCount += count;
			count = item.restrictCount - _itemsDic[item.id].buyCount;
			return count >= 0?count:0;
		}
		else if(item.restrictCount > 0)
		{
			ShopItemRestrictDto dto = new ShopItemRestrictDto();
			dto.shopItemId = item.id;
			dto.buyCount = count;
			_itemsDic[item.id] = dto;
			count = item.restrictCount - count;
			return count >= 0?count:0;
		}

		return -1;
	}
	
	#region 设置粗体GetBold
	public string GetBold(string str, bool bold = true) {
		return StringHelper.WrapSymbol(str, bold? "b" : "");
		//return string.Format("{0}{1}", bold? "[b]" : "", str);
	}
	public string GetBold(int str, bool bold = true) {
		return GetBold(str.ToString());
	}
	#endregion
}