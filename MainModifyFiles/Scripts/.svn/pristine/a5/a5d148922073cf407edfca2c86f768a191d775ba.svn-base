// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillLearningWinUI.cs
// Author   : willson
// Created  : 2015/4/3 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class AssistSkillLearningWinUIController : MonoBehaviourBase,IViewController
{ 
	private const string NAME = "Prefabs/Module/SkillModule/AssistSkillLearningView";

	private AssistSkillLearningWinUI _view;

	private AssistSkillLearningViewController _assistSkillLearningViewController;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillLearningWinUI>();
		_view.Setup(this.transform);

		RegisterEvent();

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( NAME ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		UIHelper.AdjustDepth(module,1);
		
		_assistSkillLearningViewController = module.GetMissingComponent<AssistSkillLearningViewController>();
		_assistSkillLearningViewController.InitView();

		_assistSkillLearningViewController.SetData();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
	}

	private void OnCloseBtn()
	{
		ProxySkillModule.CloseAssistSkillLearningWin();
	}

	public void Dispose()
	{
	}
}