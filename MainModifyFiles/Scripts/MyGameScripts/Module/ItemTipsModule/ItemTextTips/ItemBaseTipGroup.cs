// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemBaseTipGroup.cs
// Author   : willson
// Created  : 2015/1/26 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;

public class ItemBaseTipGroup
{
	protected PackItemDto _dto;
	protected UILabel _label;
    protected bool _isDeepBg;

	public void show(PackItemDto itemDto, UILabel label,bool isDeepBg)
	{
		_label = label;
		_label.text = "";

        _isDeepBg = isDeepBg;

		_dto = itemDto;
		initUI(itemDto);
	}
	
	protected virtual void initUI(PackItemDto itemDto)
	{
		addName(itemDto);
		
		addSpaceHeight();
		
		H1Item item = itemDto.item;
		
		if (itemDto.tradePrice > 0)
		{
			addLabel(string.Format("购买价格: {0}", itemDto.tradePrice),"FFD84C");
		}
		
		if(!string.IsNullOrEmpty(item.description))
		{
			addLabel(item.description,"FFFFFF");
		}
	}
	
	protected virtual void addName(PackItemDto itemDto)
	{
        addLabel(itemDto.item.name, _isDeepBg ? ColorConstant.Color_UI_Tab_Str : ColorConstant.Color_UI_Title_Str);
	}

	protected void addDesc(PackItemDto itemDto)
	{
		if (itemDto.item.description == null || itemDto.item.description.Length == 0) 
			return ; 
		
		addLabel(itemDto.item.description,"FF9900");
	}

	protected void addoverlayTip(PackItemDto itemDto)
	{
		if ( itemDto.item.maxOverlay > 1 ) {
			addRichText( "可堆叠: ","FFFFFF","1~" + itemDto.item.maxOverlay ,"FED36A");
		}
	}

	protected void addRichText(string title, string titleColor, string context, string contextColor) 
	{
		_label.text += string.Format("[{0}]{1}[-]",titleColor,title) + string.Format("[{0}]{1}[-]\n",contextColor,context);
	}
	
	//protected void addRichToText(String title, string titleColor, string context1, string context2, string contextColor)
	//{
	//    _label.text += AppUtils.createColorText(title, titleColor) + AppUtils.createColorText(context1, contextColor) + I18nMessagesManager.gettext("→") + AppUtils.createColorText(context2, contextColor)  + "\n";
	//}

	protected void addLabel(string label)
	{
		_label.text += string.Format("{0}\n",label);
	}

	protected void addLabel(string label, string color)
	{
		_label.text += string.Format("[{0}]{1}[-]\n",color,label);
	}
	
	protected void addSpaceHeight()
	{
		_label.text += "\n";
	}
	
}