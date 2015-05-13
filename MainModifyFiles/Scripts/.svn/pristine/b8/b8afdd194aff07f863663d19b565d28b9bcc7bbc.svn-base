// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  UseAndAddItemLogic.cs
// Author   : willson
// Created  : 2015/4/16 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;

public class UseAndAddItemLogic : IPropUseLogic 
{
	private PackItemDto _itemDto;
	
	public UseAndAddItemLogic()
	{
	}
	
	public bool usePropByPos(PackItemDto packItem)
	{
		_itemDto = packItem;
		Props item = _itemDto.item as Props;
		if(item.propsParam is PropsParam_13)
		{
			PropsParam_13 propsParam = item.propsParam as PropsParam_13;
			if(propsParam.itemId == H1VirtualItem.VirtualItemEnum_VIGOUR)
			{
				if(PlayerModel.Instance.Vigour == PlayerModel.Instance.VigourMax)
				{
                    TipManager.AddTip("活力已达上限，无法继续使用该道具");
					return false;
				}

				if(PlayerModel.Instance.Vigour + propsParam.itemCount > PlayerModel.Instance.VigourMax)
				{
                    ProxyWindowModule.OpenConfirmWindow("使用该道具后，活力会超出上限，超出部分将被扣除。你确定要继续使用吗？", "", () =>
                    {
						ServiceRequestAction.requestServer(BackpackService.apply(packItem.index,1));
					},null,UIWidget.Pivot.Left);
					return false;
				}
				else
				{
					ServiceRequestAction.requestServer(BackpackService.apply(packItem.index,1));
					return true;
				}
			}
		}
		return false;
	}
}