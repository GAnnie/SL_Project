// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyItemTipsModule.cs
// Author   : willson
// Created  : 2015/1/13 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;

public class ProxyItemTipsModule
{
	private const string NAME = "Prefabs/Module/ItemTipsModule/ItemTipsView";

    private static ItemCellController _SelectItem;

    public static ItemCellController SelectItem
    {
        set
        {
            _SelectItem = value;
            _SelectItem.isSelect = true;
        }
    }

    /**
     * 带选中框
     */
    public static void Open(ItemCellController cell, bool hasOpt = true, System.Action<PackItemDto> onUseCallback = null, int tipAnchor = 0)
    {
        if (cell.GetData() != null)
        {
            SelectItem = cell;
            Open(cell.GetData(), cell.gameObject, hasOpt, onUseCallback, tipAnchor);
        }
    }

    /**
     *  bool hasOpt : 是否显示底部操作按钮
     *  int tipAnchor : 一个标志位，0表示原本默认状态，其他值的话表示tip的位置在anchor上方
     */
	public static void Open(PackItemDto dto,GameObject anchor,bool hasOpt = true,System.Action<PackItemDto> onUseCallback=null,int tipAnchor = 0)
	{
		if(dto == null) return;
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.Guide,false);

		ItemTipsViewController controller = null;
		switch (dto.item.itemType)
		{
			case H1Item.ItemTypeEnum_Props:
				controller = view.GetMissingComponent<PropsTipsViewController>();
				break;
			case H1Item.ItemTypeEnum_Equipment:
				controller = view.GetMissingComponent<EquipmentTipsViewController>();
				break;
			case H1Item.ItemTypeEnum_PetEquipment:
				controller = view.GetMissingComponent<PetEquipmentTipsViewController>();
				break;
			default:
				controller = view.GetMissingComponent<ItemTipsViewController>();
				break;
		}

		if(controller != null)
		{
			controller.InitView();
			controller.SetData(dto,anchor,hasOpt,onUseCallback,tipAnchor);
		}
	}

	public static void Open(int itemId,GameObject anchor,int tipAnchor = 0)
	{
		PackItemDto dto = ItemHelper.ItemIdToPackItemDto(itemId);
		if(dto == null) return;

		ProxyItemTipsModule.Open(dto,anchor,false,null,tipAnchor);
	}

	public static void OpenPetEqInfo(PetEquipmentExtraDto petEqExtraDto,GameObject anchor){
		PackItemDto dto = ItemHelper.PetEqExtraDtoToPackItemDto(petEqExtraDto);
		ProxyItemTipsModule.Open(dto,anchor,false,null);
	}

	public static void OpenEquipSimpleInfo(PackItemDto dto,GameObject anchor)
	{
		if(dto.item.itemType == H1Item.ItemTypeEnum_Equipment)
		{
			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.FourModule,false);
			EquipSimpleInfoTipsViewController controller = view.GetMissingComponent<EquipSimpleInfoTipsViewController>();
			
			if(controller != null)
			{
				controller.InitView();
				controller.SetData(dto,anchor,false,null);
			}
		}
	}
	
	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.FourModule,false);
	}
	
	public static void Hide()
	{
		UIModuleManager.Instance.HideModule(NAME);	
	}
	
	public static void Close()
	{
        if (_SelectItem != null)
        {
            _SelectItem.isSelect = false;
            _SelectItem = null;
        }
		UIModuleManager.Instance.CloseModule(NAME);
	}
}