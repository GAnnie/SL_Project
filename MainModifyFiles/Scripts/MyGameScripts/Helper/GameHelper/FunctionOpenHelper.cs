// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FunctionOpenHelper.cs
// Author   : willson
// Created  : 2015/3/11 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.player.data;

public class FunctionOpenHelper
{
	public static bool JudgeFunctionOpen(int functionId ,bool showTip = true)
	{
		FunctionOpen functionOpen = GetFuctionOpenById (functionId);
		if(functionOpen == null)
			return false;
		
		if(functionOpen.close)
		{
			TipManager.AddTip(string.Format("{0}功能暂未开放",functionOpen.name));
			return false;
		}
		
		int playerGrade =  PlayerModel.Instance.GetPlayerLevel();
		if(functionOpen.vip)
		{
			if(playerGrade >= functionOpen.grade || PlayerModel.Instance.IsVip())
			{
				return true;
			}
			else
			{
				if( showTip)
				{
					TipManager.AddTip(string.Format("{0}级或VIP开启{1}功能" , functionOpen.grade, functionOpen.name));
				}
				return false;
			}
		}
		else
		{
			if(playerGrade >= functionOpen.grade)
			{
				return true;
			}
			else
			{
				if(showTip)
					TipManager.AddTip(string.Format("{0}级开启{1}功能" , functionOpen.grade, functionOpen.name));
				return false;
			}
		}
		
		return true;
	}
	
	public static FunctionOpen GetFuctionOpenById( int functionId )
	{
		return  DataCache.getDtoByCls< FunctionOpen > (functionId);
	}

	#region 装备打造
	public static bool IsEquipmentOpt()
	{
		return JudgeFunctionOpen(1);
	}

	public  static FunctionOpen GetEquipmentOpt()
	{
		return GetFuctionOpenById(1);
	}
	#endregion

	#region 修炼技能
	public static bool IsSpellOpt()
	{
		return JudgeFunctionOpen(2,false);
	}
	
	//获取充值开启限制数据//
	public  static FunctionOpen GetSpellOpt()
	{
		return GetFuctionOpenById(2);
	}
	#endregion

	#region 辅助技能
	public static bool IsAssistSkillOpt()
	{
		return JudgeFunctionOpen(3,false);
	}
	
	//获取充值开启限制数据//
	public  static FunctionOpen GetAssistSkillOpt()
	{
		return GetFuctionOpenById(3);
	}
	#endregion
}