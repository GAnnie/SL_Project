// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SkillLearningWinUIController.cs
// Author   : willson
// Created  : 2015/1/7 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillLearningWinUIController : MonoBehaviourBase,IViewController
{
	private const string FactionSkillName = "Prefabs/Module/SkillModule/FactionSkillLearningView";
	private const string PracticeSkillName = "Prefabs/Module/SkillModule/PracticeSkillView";
	private const string AssistSkillViewName = "Prefabs/Module/SkillModule/AssistSkillView";

	private FactionSkillLearningViewController _factionSkillLearningViewController = null;
	private PracticeSkillViewController _practiceSkillViewController = null;
	private AssistSkillViewController _assistSkillViewController = null;

	private SkillLearningWinUI _view;
	private List<TabBtnController> _tabBtnList;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<SkillLearningWinUI> ();
		_view.Setup(this.transform);

		InitTabBtn();
		RegisterEvent();
	}

	private void InitTabBtn()
	{
		_tabBtnList = new List<TabBtnController> (3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;

		int index = 0;
		index =	AddTabBtn ("门派技能",true,index,tabBtnPrefab);
		index = AddTabBtn ("修炼技能",FunctionOpenHelper.IsSpellOpt(),index,tabBtnPrefab);
		index = AddTabBtn ("辅助技能",FunctionOpenHelper.IsAssistSkillOpt(),index,tabBtnPrefab);
	}

	private int AddTabBtn(string name,bool isOpen,int index,GameObject tabBtnPrefab)
	{
		if(isOpen)
		{
			GameObject item = NGUITools.AddChild(_view.TabGroup.gameObject, tabBtnPrefab);
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			com.InitItem (index,OnSelectTabBtn);
            com.SetBtnName(name);
			_tabBtnList.Add(com);
			index++;
		}
		return index;
	}


	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick, CloseView);
	}

	public void OnSelectTabBtn (int index)
	{
		if (index == 0)
			OnSelectFactionSkillView();
		else if (index == 1)
			OnSelectPracticeSkillView();
		else if (index == 2)
			OnSelectAssistSkillView();
	}

	private void UpdateTabBtnState (int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i)
		{
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}

	public void OnSelectFactionSkillView()
	{
		if(_factionSkillLearningViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( FactionSkillName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.Pos,prefab);
			UIHelper.AdjustDepth(module,1);

			_factionSkillLearningViewController = module.GetMissingComponent<FactionSkillLearningViewController>();
			_factionSkillLearningViewController.InitView();
		}
		_factionSkillLearningViewController.SetActive(true);
		_factionSkillLearningViewController.SetData();

		if(_practiceSkillViewController != null) _practiceSkillViewController.SetActive(false);
		if(_assistSkillViewController != null) _assistSkillViewController.SetActive(false);
		UpdateTabBtnState (0);
	}

	public void OnSelectPracticeSkillView()
	{
		if(_practiceSkillViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( PracticeSkillName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.Pos,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_practiceSkillViewController = module.GetMissingComponent<PracticeSkillViewController>();
			_practiceSkillViewController.InitView();
		}
		_practiceSkillViewController.SetActive(true);
		_practiceSkillViewController.SetData();

		if(_factionSkillLearningViewController != null) _factionSkillLearningViewController.SetActive(false);
		if(_assistSkillViewController != null) _assistSkillViewController.SetActive(false);
		UpdateTabBtnState(1);
	}

	public void OnSelectAssistSkillView()
	{
		if(_assistSkillViewController == null)
		{
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( AssistSkillViewName ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.Pos,prefab);
			UIHelper.AdjustDepth(module,1);
			
			_assistSkillViewController = module.GetMissingComponent<AssistSkillViewController>();
			_assistSkillViewController.InitView();
		}
		_assistSkillViewController.SetActive(true);
		_assistSkillViewController.SetData();

		if(_factionSkillLearningViewController != null) _factionSkillLearningViewController.SetActive(false);
		if(_practiceSkillViewController != null) _practiceSkillViewController.SetActive(false);
		UpdateTabBtnState(2);
	}

	public void CloseView()
	{
		ProxySkillModule.Close();
	}

	public void Dispose()
	{
		if(_factionSkillLearningViewController != null)
			_factionSkillLearningViewController.Dispose();
		if(_practiceSkillViewController != null)
			_practiceSkillViewController.Dispose();
		if(_assistSkillViewController != null)
			_assistSkillViewController.Dispose();
	}
}