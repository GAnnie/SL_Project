﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  LuaManager.cs
// Author   : wenlin
// Created  : 2014/5/13 
// Purpose  : 
// **********************************************************************
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1008
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using UnityEngine;
using System;
using LuaInterface;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.assistskill.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;

public class LuaManager
{
	private static LuaManager _instance = null;

	public static LuaManager Instance
	{
		get
		{
			if( _instance == null )
			{
				_instance = new LuaManager();
			}
			return _instance;
		}
	}


	private LuaManager ()
	{
	}




	private LuaState _luaState = null;
	public void Setup()
	{
//		UluaDebugManager.DebugLogWarning("WARNING -->> 这里需要和LuaScriptMgr的lua保持一个对象");

		if ( _luaState != null )
		{
			_luaState.Close();
			_luaState = null;
		}

		_luaState = new LuaState();
//		
//		// First run the script so the function is created
//		l.DoString(script);
//		
//		// Get the function object
//		LuaFunction f = l.GetFunction("luaFunc");
//		
//		// Call it, takes a variable number of object parameters and attempts to interpet them appropriately
//		object[] r = f.Call("I called a lua function!");
//		
//		// Lua functions can have variable returns, so we again store those as a C# object array, and in this case print the first one
//		print(r[0]);
	}

	public void Dispose()
	{
		if ( _luaState != null )
		{
			_luaState.Close();
			_luaState = null;
		}
	}

	public bool DoString( string funcName , string script )
	{
		if( _luaState == null )
		{
			//Debug.LogError( "Lua State is Null " );
			return false;
		}

		// Get the function object
		if( _luaState[funcName] == null )
		{
			_luaState.DoString( script, funcName, null );
		}

		return true;
	}

	public bool IsFunctionExit( string funcName )
	{
		if( _luaState == null )
		{
			//Debug.LogError( "Lua State is Null " );
			return false;
		}

		return _luaState[funcName] != null;
	}


	public System.Object CallFunction( string funcName, params object[] args)
	{
		if( _luaState == null )
		{
			//Debug.LogError( "Lua State is Null " );
			return null;
		}

		if( _luaState[funcName] != null )
		{
			// Get the function object
			LuaFunction f = _luaState.GetFunction(funcName);
			
			// Call it, takes a variable number of object parameters and attempts to interpet them appropriately
			System.Object[] r = f.Call(args);

			return r[0];
		}
		return null;
	}

	//-trigger.maxHp()*0.05
	public int DoSkillFormula( string funcName , VideoSoldier videoSoldier, String formula )
	{
		int value = 0;

		formula = formula.Replace ("()", "");

		if( LuaManager.Instance.IsFunctionExit( funcName ))
		{
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, 
			                                                       videoSoldier));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		else
		{
			formula = formula.Replace( "@", "," );
			formula = formula.Replace( "#", ":" );
			formula = formula.ToLower();
			
			string funcScript = string.Format( @"
												function {0}(trigger) 
													return {1};
												end",
			                                  funcName,
			                                  formula );
			
			try
			{
				LuaManager.Instance.DoString( funcName, funcScript );
			}
			catch( Exception e )
			{
				Debug.LogException( e );
				return 0;
			}
			
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, 
			                                                       videoSoldier));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		return value;
	}

	public int DoStallGoodsBasePriceFormula( string funcName , String formula )
	{
		int gameServerGrade = PlayerModel.Instance.ServerGrade;
		int value = 0;
		
		if( LuaManager.Instance.IsFunctionExit( funcName ))
		{
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, 
			                                                       gameServerGrade));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		else
		{
			formula = formula.Replace( "@", "," );
			formula = formula.Replace( "#", ":" );
			formula = formula.ToLower();
			
			string funcScript = string.Format( @"
												function {0}(gameservergrade) 
													return {1};
												end",
			                                  funcName,
			                                  formula );
			
			try
			{
				LuaManager.Instance.DoString( funcName, funcScript );
			}
			catch( Exception e )
			{
				Debug.LogException( e );
				return 0;
			}
			
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, 
			                                                       gameServerGrade));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		return value;
	}

	public int DoAssistSkillCopperFormula(AssistSkillDto dto,int lv = 0)
	{
        if(lv == 0)
        {
            lv = dto.level + 1;
        }

		AssistSkill skill = dto.assistSkill;
		int index = 0;
		int beginLv = 0;
		int endLv = 0;
		for(;index < skill.levelRangeStr.Count;index++)
		{
			string lvRange = skill.levelRangeStr[index];
			string[] lvRanges = lvRange.Split('-');
			beginLv = int.Parse(lvRanges[0]);
			endLv = int.Parse(lvRanges[1]);
			if(beginLv <= lv && lv <= endLv)
			{
				break;
			}
		}

		int value = 0;

		if(index < skill.copperFormula.Count)
		{
			string funcName = string.Format("AssistSkillCopper_{0}_{1}_{2}",skill.id,beginLv,endLv);
			if( LuaManager.Instance.IsFunctionExit( funcName ))
			{
				System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
				if( obj != null )
				{
					value = (int)Convert.ToInt32( obj );
				}
			}
			else
			{
				string formula = skill.copperFormula[index];
				formula = formula.Replace( "@", "," );
				formula = formula.Replace( "#", ":" );
				formula = formula.ToLower();
				
				string funcScript = string.Format( @"
												function {0}(lv) 
													return {1};
												end",
				                                  funcName,
				                                  formula );
				
				try
				{
					LuaManager.Instance.DoString( funcName, funcScript );
				}
				catch( Exception e )
				{
					Debug.LogException( e );
					return 0;
				}
				
				System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
				if( obj != null )
				{
					value = (int)Convert.ToInt32( obj );
				}
			}
		}
		return value;
	}

	public int DoAssistSkillContributeFormula(AssistSkillDto dto,int lv = 0)
	{
        if(lv == 0)
        {
            lv = dto.level + 1;
        }
		    
		if(lv < 30)
		{
			if(dto.id != AssistSkill.AssistSkillEnum_Pursuit && dto.id != AssistSkill.AssistSkillEnum_Escape)
			{
				return 0;
			}
		}

		AssistSkill skill = dto.assistSkill;


		int value = 0;
		string funcName = string.Format("AssistSkillContribute_{0}",skill.id);
		if( LuaManager.Instance.IsFunctionExit( funcName ))
		{
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		else
		{
			string formula = skill.contributeFormula;
			formula = formula.Replace( "@", "," );
			formula = formula.Replace( "#", ":" );
			formula = formula.ToLower();
			
			string funcScript = string.Format( @"
												function {0}(lv) 
													return {1};
												end",
			                                  funcName,
			                                  formula );
			
			try
			{
				LuaManager.Instance.DoString( funcName, funcScript );
			}
			catch( Exception e )
			{
				Debug.LogException( e );
				return 0;
			}
			
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}

		if(lv <= PlayerModel.Instance.ServerGrade - 20 && !string.IsNullOrEmpty(skill.contributeDiscountFormula))
		{
			return (int)Math.Ceiling(float.Parse(skill.contributeDiscountFormula) * value);
		}
		else
		{
			return value;
		}
	}

	public int DoVigourConsumeFormula(AssistSkillDto dto,int level)
	{
		AssistSkill skill = dto.assistSkill;

		int value = 0;
		string funcName = string.Format("VigourConsume_{0}",dto.id);
		if( LuaManager.Instance.IsFunctionExit( funcName ))
		{
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, level));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		else
		{
			string formula = skill.vigourConsumeFormula[0];
			formula = formula.Replace( "productLevel", "lv" );
			formula = formula.Replace( "skillLevel", "lv" );

			formula = formula.Replace( "@", "," );
			formula = formula.Replace( "#", ":" );
			formula = formula.ToLower();
			
			string funcScript = string.Format( @"
												function {0}(lv) 
													return {1};
												end",
			                                  funcName,
			                                  formula );
			
			try
			{
				LuaManager.Instance.DoString( funcName, funcScript );
			}
			catch( Exception e )
			{
				Debug.LogException( e );
				return 0;
			}
			
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, level));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}

		if(skill.vigourConsumeDiscount > 0 && level <= PlayerModel.Instance.ServerGrade - 20)
		{
			value = (int)Math.Ceiling(value * skill.vigourConsumeDiscount);
		}

		return value;
	}

	public int DoVigourConsumeFormula( string funcName , string formula ,int lv)
	{
		int value = 0;
		if( LuaManager.Instance.IsFunctionExit( funcName ))
		{
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		else
		{
			formula = formula.Replace( "productLevel", "lv" );
			formula = formula.Replace( "skillLevel", "lv" );

			formula = formula.Replace( "@", "," );
			formula = formula.Replace( "#", ":" );
			formula = formula.ToLower();
			
			string funcScript = string.Format( @"
												function {0}(lv) 
													return {1};
												end",
			                                  funcName,
			                                  formula );
			
			try
			{
				LuaManager.Instance.DoString( funcName, funcScript );
			}
			catch( Exception e )
			{
				Debug.LogException( e );
				return 0;
			}
			
			System.Object obj = ( LuaManager.Instance.CallFunction( funcName, lv));
			if( obj != null )
			{
				value = (int)Convert.ToInt32( obj );
			}
		}
		return value;
	}

    public int DoPropsParam21Formula(string funcName, string formula, int rarity)
    {
        if (formula.IndexOf("LV") != -1)
            return 0;

        int value = 0;
        int playerLv = PlayerModel.Instance.GetPlayerLevel();
        if (LuaManager.Instance.IsFunctionExit(funcName))
        {
            System.Object obj = (LuaManager.Instance.CallFunction(funcName, rarity, playerLv));
            if (obj != null)
            {
                value = (int)Convert.ToInt32(obj);
            }
        }
        else
        {
            formula = formula.Replace("targetLevel", "playerLv");

            formula = formula.Replace("@", ",");
            formula = formula.Replace("#", ":");
            formula = formula.ToLower();

            string funcScript = string.Format(@"
												function {0}(rarity,playerlv) 
													return {1};
												end",
                                              funcName,
                                              formula);

            try
            {
                LuaManager.Instance.DoString(funcName, funcScript);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return 0;
            }

            System.Object obj = (LuaManager.Instance.CallFunction(funcName, rarity, playerLv));
            if (obj != null)
            {
                value = (int)Convert.ToInt32(obj);
            }
        }
        return value;
    }
}
