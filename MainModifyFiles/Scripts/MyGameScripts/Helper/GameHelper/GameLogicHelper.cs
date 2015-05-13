using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;

public class GameLogicHelper
{
	public static void UpdateCharactorExp(CharactorExpNotify expNotify)
	{
		PlayerModel.Instance.UpdatePlayerExpInfo(expNotify.mainCharactorExpInfo, expNotify.traceType.tip);
		PetModel.Instance.UpdatePetExpInfo(expNotify.petExpInfo, expNotify.traceType.tip);
	}

	//处理物品相关提示
	public static void HandlerItemTipDto(ItemTipDto itemTipDto, bool buyMode)
	{
		if (itemTipDto == null)
		{
			return;
		}

		List<string> tipList = new List<string> ();

		if (buyMode)
		{
			//【道具图标】购买了【道具名称】x【数量】，消耗9999【元宝图标】
			foreach(ItemDto itemDto in itemTipDto.gainItems)
			{
				if (ItemIconConst.IsCurrencyItem(itemDto.itemId))
				{
					tipList.Add( string.Format("购买了{0}{1}", itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_Item), ItemIconConst.GetIconConstByItemId(itemDto.itemId)) );
				}
				else
				{
					tipList.Add( string.Format("购买了" + "{0}x{1}".WrapColor(ColorConstant.Color_Tip_Item), itemDto.item.name, itemDto.itemCount) );
				}
			}
			
			foreach(ItemDto itemDto in itemTipDto.lostItems)
			{
				tipList.Add( string.Format("消耗{0}{1}", itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency), ItemIconConst.GetIconConstByItemId(itemDto.itemId)) );
			}
		}
		else
		{
			//【道具图标】出售了【道具名称】x1，获得9999【银币图标】
			foreach(ItemDto itemDto in itemTipDto.lostItems)
			{
				tipList.Add( string.Format("出售了"+"{0}x{1}".WrapColor(ColorConstant.Color_Tip_Item), itemDto.item.name, itemDto.itemCount) );
			}

			foreach(ItemDto itemDto in itemTipDto.gainItems)
			{
				tipList.Add( string.Format("获得{0}{1}", itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency), ItemIconConst.GetIconConstByItemId(itemDto.itemId)) );
			}
		}


		TipManager.AddTip (string.Join("，", tipList.ToArray()));
	}

	//处理物品相关提示
	public static void HandlerQuestionItemTipDto(ItemTipDto itemTipDto)
	{
		if (itemTipDto == null)
		{
			return;
		}
		
		List<string> playerTipList = new List<string> ();
		string petTip = "";

		itemTipDto.gainItems.Sort(delegate(ItemDto x, ItemDto y) {
			if (x.itemId > y.itemId){
				return -1;
			}else if (x.itemId < y.itemId){
				return 1;
			}else{
				return 0;
			}
		});
		
		foreach(ItemDto itemDto in itemTipDto.gainItems)
		{
			if (itemDto.itemId == H1VirtualItem.VirtualItemEnum_PET_EXP)
			{
				PetPropertyInfo petInfo = PetModel.Instance.GetBattlePetInfo();
				if (petInfo != null)
				{
					petTip = string.Format("{0}获得了{1}{2}", petInfo.petDto.name.WrapColor(ColorConstant.Color_Tip_Item), itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency), ItemIconConst.GetIconConstByItemId(itemDto.itemId));
				}
			}
			else
			{
				playerTipList.Add( string.Format("{0}{1}", itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency), ItemIconConst.GetIconConstByItemId(itemDto.itemId)) );
			}
		}

		if (playerTipList.Count > 0)
		{
			TipManager.AddTip ("你获得" + string.Join("、", playerTipList.ToArray()));
		}

		TipManager.AddTip (petTip);
	}

	public static void HandlerItemTipDto(List<ItemDto> lostItems, string tips)
	{
		if (lostItems == null || lostItems.Count == 0)
		{
			return;
		}
		
		List<string> tipList = new List<string>();

		foreach(ItemDto itemDto in lostItems)
		{
			tipList.Add( string.Format("消耗{0}{1}", itemDto.itemCount.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency), ItemIconConst.GetIconConstByItemId(itemDto.itemId)) );
		}

		TipManager.AddTip (string.Join("，", tipList.ToArray()) + "，" + tips);
	}
}

