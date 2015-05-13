// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  HeroView.cs
// Author   : willson
// Created  : 2014/12/3 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class HeroView : PlayerView
{
	private AutoFramWalk _autoFramWalk;

	public void ChangeView(int modelId)
	{
		_modelId = modelId;
		_testMode = true;
		string petStylePath = ModelHelper.GetCharacterPrefabPath(_modelId);
		ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
	}

	public void MutateTest(string colorParams)
	{
		if (colorParams != "")
		{
			ModelHelper.SetPetLook (_heroModelObject, _modelId, 0, colorParams);
		}
		else
		{
			ModelHelper.SetPetLook (_heroModelObject, _modelId, _modelId, colorParams);
		}
	}

	public void TextPlayAnimation(string action)
	{
		ModelHelper.PlayAnimation(curAnimation, action, false, null, false);
	}

	public void SetAutoFram(bool b)
	{
		if(_autoFramWalk == null)
		{
			_autoFramWalk = this.gameObject.AddMissingComponent<AutoFramWalk>();
		}
		_autoFramWalk.IsAutoFram = b;
	}

	public bool GetAutoFram()
	{
		return _autoFramWalk != null ? _autoFramWalk.IsAutoFram:false;
	}
}