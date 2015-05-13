// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetResumeCellController.cs
// Author   : willson
// Created  : 2015/3/18 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.dto;

public class PetResumeCellController : MonoBehaviour,IViewController
{
	private PetResumeCell _view;

	private PetPropertyInfo _petInfo;
	private System.Action<PetResumeCellController> _OnSelectCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetResumeCell> ();
		_view.Setup(this.transform);
		
		RegisterEvent ();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.PetResumeEventTrigger.onClick,OnSelectCell);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void SetData(PetPropertyInfo petInfo,System.Action<PetResumeCellController> OnSelectCallBack)
	{
		this.gameObject.SetActive(true);
		_view.SelectSprite.enabled = false;
		_petInfo = petInfo;
		_OnSelectCallBack = OnSelectCallBack;

		_view.NameLbl.text = _petInfo.petDto.name;
		_view.LvLbl.text = _petInfo.petDto.level.ToString();
	}

	public PetPropertyInfo GetData()
	{
		return _petInfo;
	}

	private void OnSelectCell()
	{
		if(_OnSelectCallBack != null)
			_OnSelectCallBack(this);
	}

	public void SetSelect(bool b)
	{
		_view.SelectSprite.enabled = b;
	}

	public void Dispose()
	{
		
	}
}