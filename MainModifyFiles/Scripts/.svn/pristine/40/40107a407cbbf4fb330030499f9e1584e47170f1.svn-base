// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PropsTipsViewController.cs
// Author   : willson
// Created  : 2015/2/2 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.data;
using UnityEngine;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PropsTipsViewController : ItemTipsViewController
{
	protected override void ShowTips ()
	{
		Props item = _dto.item as Props;

		if(!string.IsNullOrEmpty(item.introduction))
		{
			AddSpace();
			AddDecLbl().text = item.introduction.WrapColor(ColorConstant.Color_UI_Tab_Str);
		}

		// 增加动态部分
		AddPropsExtra();
		base.ShowTips();
	}

	private void AddPropsExtra()
	{
		if(_dto.extra != null)
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

            if(!string.IsNullOrEmpty(txt))
            {
                AddDecLbl().text = txt.WrapColor(ColorConstant.Color_Channel_Guild_Str);
            }
		}
	}
}