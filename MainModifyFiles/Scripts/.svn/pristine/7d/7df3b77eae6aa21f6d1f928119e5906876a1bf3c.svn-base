// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PackItemUserLogicDef.cs
// Author   : willson
// Created  : 2015/1/20 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.player.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PackItemUserLogicDef :IPropUseLogic
{
	private PackItemDto _itemDto;

	public PackItemUserLogicDef()
	{
	}

	public bool usePropByPos(PackItemDto packItem)
	{
        if (packItem.item is Props && (packItem.item as Props).scopeId == Props.ScopeEnum_Battle)
		{
            TipManager.AddTip("该道具只能在战斗中使用");
            return false;
		}

        if (packItem.itemId == DataHelper.GetStaticConfigValue(H1StaticConfigs.BACKPACK_EXPAND_CONSUME_ITEM_ID))
        {
            ProxyTipsModule.Open("扩充背包", DataHelper.GetStaticConfigValue(H1StaticConfigs.BACKPACK_EXPAND_CONSUME_ITEM_ID), BackpackModel.Instance.GetExpandTime(),
                (costItems) =>
                {
                    ServiceRequestAction.requestServer(BackpackService.expand(), "expand",
                    (e) =>
                    {
                        int add = DataHelper.GetStaticConfigValue(H1StaticConfigs.PACK_EXPAND_SIZE, 5);
                        GameLogicHelper.HandlerItemTipDto(costItems, string.Format("背包容量增加了{0}格", add));
                        BackpackModel.Instance.AddCapability(add);
                    });
                });
            return false;
        }
		else if (packItem.itemId == DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_ITEM_ID,10041) )
		{
			ProxyPlayerPropertyModule.OpenPlayerChangeNameView(true);
			return false;
		}
        else if(BackpackModel.Instance.isPetExchangeProps(packItem))
        {
			//前往神兽使者
			ProxyBackpackModule.Close();
			WorldManager.Instance.WalkToByNpcId(5002);
			return false;
		}
        else if (packItem.itemId == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_RESET_ITEM_ID, 10001))
        {
            // 打开洗宠分页
            ProxyBackpackModule.Close();
            ProxyPetPropertyModule.Open(1);
            return false;
        }
		else
		{
			ServiceRequestAction.requestServer(BackpackService.apply(packItem.index,1),"", 
			(e) => {
			});
			return true;
		}
	}
}