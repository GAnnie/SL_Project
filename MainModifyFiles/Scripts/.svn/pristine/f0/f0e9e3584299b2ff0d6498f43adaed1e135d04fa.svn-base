// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemIconConst.cs
// Author   : willson
// Created  : 2015/1/27
// Porpuse  : 
// **********************************************************************
using System.Text.RegularExpressions;
using com.nucleus.h1.logic.core.modules.player.data;

public class ItemIconConst
{
	public const string Ingot = "#w1"; //元宝
	public const string Silver = "#w2";//银币
	public const string Copper = "#w3";//铜币
	public const string Score = "#w4";//积分
	public const string Contribute = "#w5";//帮贡
	public const string Exp = "#exp1";//经验
	public const string Exp2 = "#exp2";//经验2
	public const string Rmb = "#wrmb";//经验2

	public const string IngotAltas = "ICON_1"; //元宝
	public const string SilverAltas = "ICON_2";//银币
	public const string CopperAltas = "ICON_3";//铜币
	public const string ScoreAltas = "ICON_4";//积分
	public const string ContributeAltas = "ICON_5";//帮贡
	public const string ExpAltas = "ICON_11";//经验
	public const string Exp2Altas = "ICON_13";//经验2
	public const string RmbAltas = "RMB-little-icon";//经验2

	public const string GrowthFlag_Green ="#gf1";
	public const string GrowthFlag_Blue ="#gf2";
	public const string GrowthFlag_Orange ="#gf3";
	public const string GrowthFlag_Purple ="#gf4";
	public const string GrowthFlag_Red ="#gf5";
	
	/*
	 * 根据虚拟物品的type来获取相应的图标名
	*/
	public static string GetIconConstByItemId( int itemId )
	{
		string tempStr = string.Empty;
		
		switch ( itemId )
		{
			case H1VirtualItem.VirtualItemEnum_INGOT :
				tempStr = Ingot;
				break;
			case H1VirtualItem.VirtualItemEnum_SILVER:
				tempStr = Silver;
				break;
			case H1VirtualItem.VirtualItemEnum_COPPER:
				tempStr = Copper;
				break;
			case H1VirtualItem.VirtualItemEnum_SCORE:
				tempStr = Score;
				break;
			case H1VirtualItem.VirtualItemEnum_CONTRIBUTE:
				tempStr = Contribute;
				break;
			case H1VirtualItem.VirtualItemEnum_PLAYER_EXP:
			case H1VirtualItem.VirtualItemEnum_PET_EXP:
			case H1VirtualItem.VirtualItemEnum_RESERVE_EXP:
				tempStr = Exp;
				break;
			case H1VirtualItem.VirtualItemEnum_PRACTIVE_EXP:
				tempStr = Exp2;
				break;
		}
		
		return tempStr;
    }

	public static bool IsCurrencyItem(int itemId)
	{
		bool isCurrency = false;
		
		switch ( itemId )
		{
		case H1VirtualItem.VirtualItemEnum_INGOT :
		case H1VirtualItem.VirtualItemEnum_SILVER:
		case H1VirtualItem.VirtualItemEnum_COPPER:
		case H1VirtualItem.VirtualItemEnum_SCORE:
		case H1VirtualItem.VirtualItemEnum_CONTRIBUTE:
		case H1VirtualItem.VirtualItemEnum_PLAYER_EXP:
		case H1VirtualItem.VirtualItemEnum_PET_EXP:
		case H1VirtualItem.VirtualItemEnum_RESERVE_EXP:
		case H1VirtualItem.VirtualItemEnum_PRACTIVE_EXP:
			isCurrency = true;
			break;
		default:
			isCurrency = false;
			break;
		}

		return isCurrency;
	}

	/*
	 * 根据虚拟物品的type来获取 CommonUIAltas 相应的图标名
	*/
	public static string GetIconAltasConstByItemId( int itemId )
	{
		string tempStr = string.Empty;
		
		switch ( itemId )
		{
		case H1VirtualItem.VirtualItemEnum_INGOT :
			tempStr = IngotAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_SILVER:
			tempStr = SilverAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_COPPER:
			tempStr = CopperAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_SCORE:
			tempStr = ScoreAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_CONTRIBUTE:
			tempStr = ContributeAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_PLAYER_EXP:
		case H1VirtualItem.VirtualItemEnum_PET_EXP:
		case H1VirtualItem.VirtualItemEnum_RESERVE_EXP:
			tempStr = ExpAltas;
			break;
		case H1VirtualItem.VirtualItemEnum_PRACTIVE_EXP:
			tempStr = Exp2Altas;
			break;
		}
		
		return tempStr;
	}

}