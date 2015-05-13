using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using System;

public class BattleItemUseController : ItemUseViewController
{
	override protected void InitLeftGroup()
	{
		_view.OptlblLabel.text = "使用";
		_leftView = BattleItemUseViewController.Setup(_view.LGroup);
	}
	
	/*
	public void SetOptBtn(string btnName,System.Action<PackItemDto> onClickFun)
	{
	}
	*/

	private int _itemUsedCount = 0;
	private Action<PackItemDto> _callBackDelegate;

	public void SetOtherParam(int itemUsedCount, Action<PackItemDto> callBackDelegate)
	{
		_itemUsedCount = itemUsedCount;
		_callBackDelegate = callBackDelegate;
		(_leftView as BattleItemUseViewController).UpdateItemUsedCount(_itemUsedCount);
	}

	private void Refresh()
	{
		bool isChange = false;
		// 删除已经鉴定的装备
		for(int index = _items.Count - 1;index >= 0;index--)
		{
		}
		
		if(isChange)
		{
			int index = 0;
			for(;index < _items.Count;index++)
			{
				_itemCellList[index].SetData(_items[index]);
				_itemCellList[index].SelectSingle(_leftView.GetData().index == _items[index].index);
			}
			
			while(index < _itemCellList.Count)
			{
				_itemCellList[index].SetData(null);
				index++;
			}
		}
	}
	
	override protected void OnOptBtn()
	{
		PackItemDto dto = _leftView.GetData();
		if(dto != null)
		{
			if (_itemUsedCount >= 10)
			{
				TipManager.AddTip("本场战斗使用物品数量已达上限");
			}
			else
			{
				if (_callBackDelegate != null)
				{
					_callBackDelegate(dto);
					_callBackDelegate = null;
				}
				ProxyItemUseModule.Close();
			}
		}
		else
		{
			TipManager.AddTip("请选择需要使用的物品");
		}
	}
	
	private void UpdateRigth(PackItemDto itemDto)
	{
		for(int index = 0;index < _items.Count;index++)
		{
			if(_items[index].index == itemDto.index)
			{
				_items[index] = itemDto;
				break;
			}
		}
		
		for(int index = 0;index < _itemCellList.Count;index++)
		{
			if(_itemCellList[index].GetData().index == itemDto.index)
			{
				_itemCellList[index].SetData(itemDto);
				break;
			}
		}
	}

	virtual public void Dispose()
	{
		_callBackDelegate = null;
	}
}

