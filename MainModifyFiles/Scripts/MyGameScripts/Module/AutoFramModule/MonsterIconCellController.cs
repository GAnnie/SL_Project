// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MonsterIconCellController.cs
// Author   : willson
// Created  : 2015/3/9 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.battle.data;

public class MonsterIconCellController : MonoBehaviourBase,IViewController
{
	private MonsterIconCell _view;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<MonsterIconCell> ();
		_view.Setup(this.transform);

		RegisterEvent();
	}

	public void RegisterEvent()
	{
	}

	public void SetData(int monsterId)
	{
		Monster monster = DataCache.getDtoByCls<Monster>(monsterId);
		if(monster != null)
		{
			_view.NameLabel.text = monster.name;
			//_view.IconSprite.spriteName = "";
		}
		else
		{
			_view.NameLabel.text = "无怪物数据";
		}
	}

	public void Dispose()
	{
	}
}