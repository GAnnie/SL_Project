// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  GemItemCellController.cs
// Author   : willson
// Created  : 2015/1/22 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.msg;

public class GemItemCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";
	
	private GemItemCell _view;
	private ItemCellController _cell;

	private System.Action<GemItemCellController> _OnGemCellClick;
	private System.Action<GemItemCellController> _OnRomeveGemClick;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<GemItemCell> ();
		_view.Setup(this.transform);
		
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		
		_cell.InitView();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.RomeveBtn.onClick,OnRomeveBtn);
	}

	public void SetData(Props gems,System.Action<GemItemCellController> OnGemCellClick,System.Action<GemItemCellController> OnRomeveGemClick)
	{
		_OnGemCellClick = OnGemCellClick;
		_OnRomeveGemClick = OnRomeveGemClick;

		_cell.AlwaysDisplayCount(true);
		_cell.SetData(ItemHelper.H1ItemToPackItemDto(gems,BackpackModel.Instance.GetItemCount(gems.id)),OnGemClick);
	}

	public PackItemDto GetData()
	{
		return _cell.GetData();
	}

	public bool isGrey
	{
		get{
			return _cell.isGrey;
		}
		set
		{
			if(_cell.isGrey != value)
			{
				if(value)
					_view.RomeveBtn.gameObject.SetActive(!value);
				_cell.isGrey = value;
			}
		}
	}

	public bool isRomeve
	{
		get{
			return _view.RomeveBtn.gameObject.activeSelf;
		}
		set
		{
			if(_view.RomeveBtn.gameObject.activeSelf != value)
			{
				_view.RomeveBtn.gameObject.SetActive(value);
			}
		}
	}

	public bool isSelect
	{
		get
		{
			return _cell.isSelect;
		}
		set
		{
			if(_cell.isSelect != value)
			{
				_cell.isSelect = value;
			}
		}
	}

	private void OnGemClick(ItemCellController cell)
	{
		if(_cell.isGrey)
		{
			TipManager.AddTip("不能镶嵌该宝石");
		}
		else
		{
			if(_OnGemCellClick != null)
				_OnGemCellClick(this);
		}
	}

	private void OnRomeveBtn()
	{
		if(!_cell.isGrey)
		{
			if(_OnRomeveGemClick != null)
				_OnRomeveGemClick(this);
		}
	}

	public void Dispose()
	{
	}
}