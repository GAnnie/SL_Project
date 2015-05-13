using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.battle.data;
using System.Collections.Generic;

public class ProxyBattleModule
{
	#region NAME_BattleSkillSelect
	private const string NAME_BattleSkillSelect = "Prefabs/Module/Battle/BattleSkillSelectView";
	
	public static void OpenSkillSelect(MonsterController mc, Action<Skill> OnSkillSelect)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule( NAME_BattleSkillSelect ,UILayerType.DefaultModule, true);
		var controller = ui.GetMissingComponent<BattleSkillSelectController>();
		controller.Open (mc, OnSkillSelect);
	}

	public static void CloseSkillSelect()
	{
		UIModuleManager.Instance.CloseModule( NAME_BattleSkillSelect );
	}
	
	public static void HideSkillSelect()
	{
		UIModuleManager.Instance.HideModule( NAME_BattleSkillSelect );
	}
	#endregion

	#region NAME_BattleStuntSelect
	public static void OpenStuntSelect(MonsterController mc, List<int> skills, Action<Skill> OnStuntSelect)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule( NAME_BattleSkillSelect ,UILayerType.DefaultModule, true);
		var controller = ui.GetMissingComponent<BattleSkillSelectController>();
		controller.OpenStunt (mc, skills, OnStuntSelect);
	}
	
	public static void CloseStuntSelect()
	{
		UIModuleManager.Instance.CloseModule( NAME_BattleSkillSelect );
	}
	
	public static void HideStuntSelect()
	{
		UIModuleManager.Instance.HideModule( NAME_BattleSkillSelect );
	}
	#endregion

	#region NAME_BattleSummon
	private const string NAME_BattleSummon = "Prefabs/Module/Battle/BattleSummonView";
	
	public static void OpenSummon(Action<long> onSelectDelegate)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule( NAME_BattleSummon ,UILayerType.DefaultModule, true);
		var controller = ui.GetMissingComponent<BattleSummonController>();
		controller.Open (onSelectDelegate);
	}
	
	public static void CloseSummon()
	{
		UIModuleManager.Instance.CloseModule( NAME_BattleSummon );
	}
	
	public static void HideSummon()
	{
		UIModuleManager.Instance.HideModule( NAME_BattleSummon );
	}
	#endregion
}

