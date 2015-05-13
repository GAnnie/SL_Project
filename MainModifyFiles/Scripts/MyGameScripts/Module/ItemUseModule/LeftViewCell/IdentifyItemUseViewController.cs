
// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  IdentifyItemUseViewController.cs
// Author   : willson
// Created  : 2015/3/16 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;

public class IdentifyItemUseViewController : UseLeftViewCell
{
	private const string ViewName = "Prefabs/Module/ItemUseModule/IdentifyItemUseView";

	public static IdentifyItemUseViewController Setup(GameObject pos)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(pos,prefab);
		IdentifyItemUseViewController leftView = module.GetMissingComponent<IdentifyItemUseViewController>();
		leftView.InitView();
		UIHelper.AdjustDepth(module,1);
		return leftView;
	}

	private IdentifyItemUseView _view;

	override public void InitView()
	{
		_view = gameObject.GetMissingComponent<IdentifyItemUseView> ();
		_view.Setup (this.transform);
	}
	
	override public void RegisterEvent()
	{
	}

	override public void SetUseDto(PackItemDto dto)
	{
		_useDto = dto;
		_view.EmptyInfo.SetActive(true);
		_view.DecLabel.text = _useDto.item.name + "\n" + _useDto.item.description;
		_view.EquipmentTipLabel.text = "";
		_view.CountLabel.text = string.Format("剩余数量：{0}",dto.count);
	}

	override public void SetData(PackItemDto dto)
	{
		_dto = dto;
		if(dto == null)
		{
			_view.EmptyInfo.SetActive(true);
			_view.DecLabel.text = _useDto.item.name + "\n" + _useDto.item.description;
			_view.EquipmentTipLabel.text = "";
		}
		else
		{
			_view.EmptyInfo.SetActive(false);
			ItemTextTipManager.Instance.ShowItem(dto,_view.EquipmentTipLabel,true);
		}
	}
	
	override public void Dispose()
	{
	}
}