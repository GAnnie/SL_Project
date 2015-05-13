// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistProductCellController.cs
// Author   : willson
// Created  : 2015/4/8 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.assistskill.data;

public class AssistProductCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private AssistProductCell _view;
	private ItemCellController _cell;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistProductCell>();
		_view.Setup(this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		_cell.InitView();
		_cell.CanDisplayCount(false);
		
		RegisterEvent();
	}

	public void RegisterEvent()
	{
	}

	public void SetData(AssistSkillProduct dto)
	{
		_cell.SetData(ItemHelper.ItemIdToPackItemDto(dto.id,1));
		_view.LvLabel.text = dto.level.ToString();
	}

	public void Dispose()
	{
	}
}