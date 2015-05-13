// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  CurrencyExchange.cs
// Author   : willson
// Created  : 2015/1/19 
// Porpuse  : 
// **********************************************************************
using System;
using com.nucleus.h1.logic.core.modules;

public class CurrencyExchange
{
	public static long IngotToCopper(long ingot)
	{
		return (long)Math.Ceiling(ingot * 1.0 * (PlayerModel.Instance.ServerGrade * DataHelper.GetStaticConfigValue(H1StaticConfigs.INGOT_CONVERT_COPPER_FACTOR1) + DataHelper.GetStaticConfigValue(H1StaticConfigs.INGOT_CONVERT_COPPER_FACTOR2)));
	}

	public static int CopperToIngot(long copper)
	{
		// (SLV*1000+10000)
		return (int)Math.Ceiling(copper * 1.0 / (PlayerModel.Instance.ServerGrade * DataHelper.GetStaticConfigValue(H1StaticConfigs.INGOT_CONVERT_COPPER_FACTOR1) + DataHelper.GetStaticConfigValue(H1StaticConfigs.INGOT_CONVERT_COPPER_FACTOR2)));
	}

	public static int SilverToIngot(int silver)
	{
		return (int)Math.Ceiling(silver * 1.0/DataHelper.GetStaticConfigValue(H1StaticConfigs.INGOT_CONVERT_SILVER));
	}

	public static int ContributeToCopper(int contribute)
	{
		return contribute * DataHelper.GetStaticConfigValue(H1StaticConfigs.ASSIST_SKILL_CONTRIBUTE_TO_COPPER);
	}
}