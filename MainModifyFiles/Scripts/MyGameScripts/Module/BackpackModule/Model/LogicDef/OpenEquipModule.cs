// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  OpenEquipModule.cs
// Author   : willson
// Created  : 2015/4/27 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;

public class OpenEquipModule : IPropUseLogic 
{
	private PackItemDto _itemDto;

    public OpenEquipModule()
	{
	}
	
	public bool usePropByPos(PackItemDto packItem)
	{
        Props props = packItem.item as Props;
        if (props == null)
            return false;

        if (props.logicId == Props.PropsLogicEnum_EMBED)
        {
            ProxyEquipmentOptModule.ShowEquipmentGem();
        }
        else if (props.logicId == Props.PropsLogicEnum_WishJade)
        {
            ProxyEquipmentOptModule.ShowEquipmentProperty();
        }
        else if (props.logicId == Props.PropsLogicEnum_EquipmentSpirit)
        {
            PropsParam_19 param = props.propsParam as PropsParam_19;
            ProxyEquipmentOptModule.ShopEquipmentManufacturing(param.grade);
        }
        else if (props.logicId == Props.PropsLogicEnum_Smith)
        {
            PropsParam_20 param = props.propsParam as PropsParam_20;
            ProxyEquipmentOptModule.ShopEquipmentManufacturing(param.grade);
        }
        return false;

	}
}