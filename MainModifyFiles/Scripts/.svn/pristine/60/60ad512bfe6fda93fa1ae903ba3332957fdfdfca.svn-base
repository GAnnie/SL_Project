// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ProxyFactionSkillModule.cs
// Author   : willson
// Created  : 2014/12/9 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;

public static class ProxySkillModule
{
	private const string NAME = "Prefabs/Module/SkillModule/SkillLearningWinUI";

	public static void Open(int index = 0,int depath = UILayerType.DefaultModule)
	{
		SpellModel.Instance.Setup();
        PlayerModel.Instance.RequestCharacterDto(() =>
        {
            GameObject view = UIModuleManager.Instance.OpenFunModule(NAME, depath, true);
            if (view != null)
            {
                var controller = view.GetMissingComponent<SkillLearningWinUIController>();
                controller.InitView();
                controller.OnSelectTabBtn(index);
            }
        });
	}

	public static void Show()
	{
		UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.DefaultModule,true);
	}

	public static void Hide()
	{
		UIModuleManager.Instance.HideModule(NAME);	
	}

	public static void Close()
	{
		UIModuleManager.Instance.CloseModule(NAME);
	}

	private const string NAME_TIPS = "Prefabs/Module/SkillModule/SkillTipsView";

	public static void ShowTips(string skillName,string skillDes,GameObject anchor)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_TIPS,UILayerType.ThreeModule,false);
		var controller = view.GetMissingComponent<SkillTipsViewController>();
		controller.InitView();
		controller.Show(skillName,skillDes,anchor);
	}

	public static void ShowTips(SkillInfo skillInfo,GameObject anchor)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_TIPS,UILayerType.ThreeModule,false);
		var controller = view.GetMissingComponent<SkillTipsViewController>();
		controller.InitView();
		controller.Show(skillInfo,anchor);
	}

	public static void ShowTips(int skillId,GameObject anchor){
		Skill skill = DataCache.getDtoByCls<Skill>(skillId);
		if(skill != null){
			GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_TIPS,UILayerType.ThreeModule,false);
			var controller = view.GetMissingComponent<SkillTipsViewController>();
			controller.InitView();
			controller.Show(skill,anchor);
		}
	}

	public static void CloseTips()
	{
		UIModuleManager.Instance.CloseModule(NAME_TIPS);
	}

	#region 辅助技能学习面板
	private const string NAME_AssistSkillLearning = "Prefabs/Module/SkillModule/AssistSkillLearningWinUI";
	public static void OpenAssistSkillLearningWin(int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillLearning,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillLearningWinUIController>();
			controller.InitView();
		}
	}

	public static void CloseAssistSkillLearningWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillLearning);
	}
	#endregion

	#region 辅助技能制作物品面板
	private const string NAME_AssistSkillMake = "Prefabs/Module/SkillModule/AssistSkillMakeWinUI";

	// 打造
	public static void OpenAssistSkillMakeWin(AssistSkillDto dto,int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillMake,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillMakeWinUIController>();
			controller.InitView();
			controller.SetData(dto);
		}
	}

	public static void CloseAssistSkillMakeWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillMake);
	}

	// 裁缝
	public static void OpenAssistSkillTailoringWin(AssistSkillDto dto,int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillMake,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillTailoringWinUIController>();
			controller.InitView();
			controller.SetData(dto);
		}
	}
	
	public static void CloseAssistSkillTailoringWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillMake);
	}

	// 辅助技能
	public static void OpenAssistSkillFactionWin(FactionSkillDto dto,int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillMake,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillFactionWinUIController>();
			controller.InitView();
			controller.SetData(dto);
		}
	}
	
	public static void CloseAssistSkillFactionWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillMake);
	}

	// 烹饪
	private const string NAME_AssistSkillCook = "Prefabs/Module/SkillModule/AssistSkillCookWinUI";

	public static void OpenAssistSkillCookWin(AssistSkillDto dto,int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillCook,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillCookWinUIController>();
			controller.InitView();
			controller.SetData(dto);
		}
	}
	
	public static void CloseAssistSkillCookWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillCook);
	}

	// 炼药
	private const string NAME_AssistSkillRefine = "Prefabs/Module/SkillModule/AssistSkillRefineWinUI";
	
	public static void OpenAssistSkillRefineWin(AssistSkillDto dto,int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(NAME_AssistSkillRefine,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<AssistSkillRefineWinUIController>();
			controller.InitView();
			controller.SetData(dto);
		}
	}
	
	public static void CloseAssistSkillRefineWin()
	{
		UIModuleManager.Instance.CloseModule(NAME_AssistSkillRefine);
	}
	#endregion
}