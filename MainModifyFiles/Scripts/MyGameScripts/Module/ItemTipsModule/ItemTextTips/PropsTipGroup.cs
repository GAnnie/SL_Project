// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PropsTipGroup.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PropsTipGroup : ItemBaseTipGroup
{
	override protected void initUI(PackItemDto itemDto)
	{
		addName(itemDto);

		addSpaceHeight();

		Props item = itemDto.item as Props;
		if(!string.IsNullOrEmpty(item.introduction))
		{
            addLabel(item.introduction, _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}

        // 增加动态部分
        AddPropsExtra();

		if (itemDto.tradePrice > 0)
		{
            addLabel(string.Format("购买价格: {0}", itemDto.tradePrice), _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}

		if(!string.IsNullOrEmpty(item.description))
		{
            addLabel(item.description, _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
		}
	}

    private void AddPropsExtra()
    {
        if (_dto.extra != null)
        {
            string txt = "";
            if (_dto.extra is PropsExtraDto_11)
            {
                txt = string.Format("等级{0}", (_dto.extra as PropsExtraDto_11).level);
            }
            else if (_dto.extra is PropsExtraDto_21)
            {
                txt = string.Format("品质{0}", (_dto.extra as PropsExtraDto_21).rarity);
            }

            if (_dto.extra is PropsExtraDto_21 && (_dto.extra as PropsExtraDto_21).rarity > 0)
            {
                Props props = _dto.item as Props;
                if (props != null && props.propsParam is PropsParam_21)
                {
                    PropsParam_21 param = props.propsParam as PropsParam_21;
                    txt += ",恢复" + ItemHelper.BattleBasePropertyTypeName(param.propertyType)
                        + LuaManager.Instance.DoPropsParam21Formula("PropsParam_21_" + props.id, param.formula, (_dto.extra as PropsExtraDto_21).rarity);
                }
            }

            if (!string.IsNullOrEmpty(txt))
            {
                addLabel(txt,ColorConstant.Color_Channel_Guild_Str);
            }
        }
    }
}