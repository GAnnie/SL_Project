// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemCellExController.cs
// Author   : willson
// Created  : 2015/1/20 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.data;

public class ItemCellExController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private ItemCellEx _view;
	private ItemCellController _cell;

	private int _needCount;
	private long _hasCount;

	private System.Action<ItemCellExController> _onTakeOffCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemCellEx> ();
		_view.Setup(this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		_cell.InitView();
		_cell.CanDisplayCount(false);

		nameLblColor = "6f3e1a";

		RegisterEvent();

		_view.TakeOffBtn.gameObject.SetActive(false);
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.TakeOffBtn.onClick,OnTakeOffBtn);
	}

	public void SetData(PackItemDto dto)
	{
		gameObject.SetActive(true);

		_cell.SetData(dto,ShowEquipTips);
		if(dto != null)
		{
			GeneralItem itemInfo = DataCache.getDtoByCls<GeneralItem>(dto.itemId);
			_view.NameLabel.text = string.Format("[{1}]{0}[-]",itemInfo.name,nameLblColor);

			if(dto.count > 1)
				_view.CountLabel.text = dto.count.ToString();
			else
				_view.CountLabel.text = "";

			if(_onTakeOffCallBack != null)
				_view.TakeOffBtn.gameObject.SetActive(true);
		}
		else
		{
			_view.TakeOffBtn.gameObject.SetActive(false);
			_view.CountLabel.text = "";
			_view.NameLabel.text = "";
		}
	}

	/**
	 *  long hasCount : 根据这个数量类型判断是否物品
	 */
	public void SetData(PackItemDto dto,int needCount,int hasCount)
	{
		SetData(dto);
		_needCount = needCount;
		_hasCount = hasCount;
		_view.CountLabel.text = string.Format("[{0}]{1}/{2}[-]",needCount > hasCount?"fc7b6a":"5cf37c",hasCount,needCount);
	}

	/**
	 *  long hasCount : 根据这个数量类型判断是否铜币
	 */
	public void SetData(PackItemDto dto,int needCount,long hasCount)
	{
		SetData(dto);
		_needCount = needCount;
		_hasCount = hasCount;
		_view.CountLabel.text = string.Format("[{0}]{1}[-]",needCount > hasCount?"fc7b6a":"5cf37c",needCount);
	}

	public bool isHas()
	{
		return _hasCount >= _needCount;
	}

	public int GetNeedCount()
	{
		return _needCount;
	}

	public int GetLack()
	{
		return (int)(_needCount - _hasCount);
	}

	public PackItemDto GetData()
	{
		return _cell.GetData();
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public bool isShow()
	{
		return gameObject.activeSelf;
	}

	public void SetTakeOffBtn(System.Action<ItemCellExController> onTakeOffCallBack)
	{
		_view.TakeOffBtn.gameObject.SetActive(true);
		_onTakeOffCallBack = onTakeOffCallBack;
	}

	private void OnTakeOffBtn()
	{
		if(_onTakeOffCallBack != null)
			_onTakeOffCallBack(this);
	}

	private void ShowEquipTips(ItemCellController cell)
	{
		if(cell != null && cell.GetData() != null 
		   && cell.GetData().item != null && cell.GetData().item.itemType == H1Item.ItemTypeEnum_Equipment)
		{
			ProxyItemTipsModule.OpenEquipSimpleInfo(cell.GetData(),cell.gameObject);
		}
	}

	public void Dispose()
	{
	}

	public string nameLblColor
	{
		get;
		set;
	}
}