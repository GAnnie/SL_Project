// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemTipManager.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;

public class ItemTextTipManager
{
	private static readonly ItemTextTipManager instance = new ItemTextTipManager();
	public static ItemTextTipManager Instance
	{
		get
		{
			return instance;
		}
	}

	private ItemBaseTipGroup _currentTip;
	private ItemBaseTipGroup _baseTip;
	private PropsTipGroup _propsTip;
	private EquipmentTipGroup _equipmentTip;
	private EquipmentPropertyTipGroup _equipmentPropertyTip;
	private EquipmentShopTipGroup _equipmentShopTip;

	public ItemTextTipManager()
	{
		_baseTip = new ItemBaseTipGroup ();
		_propsTip = new PropsTipGroup();

		_equipmentTip = new EquipmentTipGroup();
		_equipmentPropertyTip = new EquipmentPropertyTipGroup();
		_equipmentShopTip = new EquipmentShopTipGroup();
	}

    public void ShowItem(PackItemDto itemDto, UILabel label, bool isDeepBg = false)
	{
		if (itemDto == null || itemDto.item == null)
			return;
		
		switch (itemDto.item.itemType)
		{
			case H1Item.ItemTypeEnum_Props:
				_currentTip = _propsTip;
				break;
			case H1Item.ItemTypeEnum_Equipment:
				_currentTip = _equipmentTip;
				break;
			default:
				_currentTip = _baseTip;
				break;
		}

		_currentTip.show(itemDto,label,isDeepBg);
	}

    public void ShowEquipProperty(PackItemDto itemDto, UILabel label, bool isDeepBg = false)
	{
		if (itemDto == null || itemDto.item == null)
			return;
		if(itemDto.item.itemType == H1Item.ItemTypeEnum_Equipment)
		{
			_currentTip = _equipmentPropertyTip;
            _equipmentPropertyTip.show(itemDto, label, isDeepBg);
		}
	}

    public void ShowShopEquip(PackItemDto itemDto, UILabel label, string extraInfo, bool isDeepBg = false)
	{
		if (itemDto == null || itemDto.item == null)
			return;
		if(itemDto.item.itemType == H1Item.ItemTypeEnum_Equipment)
		{
			_currentTip = _equipmentShopTip;
			_equipmentShopTip.extraInfo = extraInfo;
            _equipmentShopTip.show(itemDto, label, isDeepBg);
		}
	}
}