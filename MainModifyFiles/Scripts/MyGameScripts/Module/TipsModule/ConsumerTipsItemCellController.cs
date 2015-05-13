// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ConsumerTipsItemCellController.cs
// Author   : willson
// Created  : 2015/3/10 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ConsumerTipsItemCellController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private ItemCellEx _view;
	private ItemCellController _cell;

	private int _itemId;
	private int _costCount;
	private int _ingot;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemCellEx> ();
		_view.Setup(this.transform);
		
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		_cell.InitView();
		_cell.CanDisplayCount(false);

		nameLblColor = "fff9e3";

		RegisterEvent();

		_ingot = 0;
		_view.TakeOffBtn.gameObject.SetActive(false);
	}

	public void RegisterEvent()
	{
	}

	public void SetData(int itemId,int needCount)
	{
		_itemId = itemId;
		_ingot = 0;
		_costCount = needCount;

		PackItemDto item = new PackItemDto();
		item.itemId = itemId;
		item.count = 1;

		string needCountStr = "";

		_cell.SetData(item);

		if(item.itemId < 100)
		{
			GeneralItem value = DataCache.getDtoByCls<GeneralItem>(item.itemId);
			if(item.itemId == H1VirtualItem.VirtualItemEnum_COPPER)
			{
				// 铜币 不足
				if(PlayerModel.Instance.isEnoughCopper(needCount) == false)
				{
					_costCount = (int)PlayerModel.Instance.GetWealth().copper;
					long needCopper = needCount - _costCount;
					_ingot = CurrencyExchange.CopperToIngot(needCopper); 
				}
				
				if(needCount >= 100000)
				{
					needCountStr = (needCount/10000 + "w");
				}
				else
				{
					needCountStr = needCount.ToString();
				}

				_view.NameLabel.text = string.Format("[{1}]{0}[-]",value.name,nameLblColor);
			}
			else if(item.itemId == H1VirtualItem.VirtualItemEnum_SILVER)
			{
				// 银币 不足
				if(PlayerModel.Instance.isEnoughSilver(needCount) == false)
				{
					_costCount = PlayerModel.Instance.GetWealth().silver;
					long needCopper = needCount - _costCount;
					_ingot = CurrencyExchange.SilverToIngot((int)needCopper); 
				}
				needCountStr = needCount.ToString();
				_view.NameLabel.text = string.Format("[{1}]{0}[-]",value.name,nameLblColor);
			}
		}
		else
		{
			int itemCount = BackpackModel.Instance.GetItemCount(itemId);
			
			if(itemCount >= needCount)
			{
				needCountStr = string.Format("{0}/{1}",itemCount,needCount);
			}
			else
			{
				_costCount = itemCount;
				needCountStr = string.Format("{0}/{1}",itemCount,needCount);
				_ingot = item.item.buyPrice * (needCount - itemCount);
			}

			_view.NameLabel.text = string.Format("[{1}]{0}[-]",item.item.name,nameLblColor);
		}

		_view.CountLabel.text = _ingot > 0 ? needCountStr.WrapColor("fc7b6a") : needCountStr;
	}

	public int GetIngot()
	{
		return _ingot;
	}

	public ItemDto GetCost()
	{
		if(_costCount == 0)
			return null;

		ItemDto dto = new ItemDto();
		dto.itemId = _itemId;
		dto.itemCount = _costCount;
		return dto;
	}

	public string nameLblColor
	{
		get;
		set;
	}

	public void Dispose()
	{
	}
}