// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  IdentifyItemUseController.cs
// Author   : willson
// Created  : 2015/3/16 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.equipment.dto;

public class IdentifyItemUseController : ItemUseViewController
{
	override protected void InitLeftGroup()
	{
		_view.OptlblLabel.text = "鉴定";
		_leftView = IdentifyItemUseViewController.Setup(_view.LGroup);
	}

	/*
	public void SetOptBtn(string btnName,System.Action<PackItemDto> onClickFun)
	{
	}
	*/

	private void Refresh()
	{
		bool isChange = false;
		// 删除已经鉴定的装备
		for(int index = _items.Count - 1;index >= 0;index--)
		{
			EquipmentExtraDto extra = _items[index].extra as EquipmentExtraDto;
			if(extra.hasIdentified)
			{
				_items.RemoveAt(index);
				isChange = true;
			}
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
			if(_useDto.count > 0)
			{
				EquipmentExtraDto extra = dto.extra as EquipmentExtraDto;
				if(extra != null && !extra.hasIdentified)
				{
					Refresh();

					ServiceRequestAction.requestServer(EquipmentService.identifyBySelectProps(_useDto.index,dto.uniqueId),"identifyByProps", 
					(e) => {
						if(e is PackItemDto)
						{
							TipManager.AddTip("装备鉴定成功");
							PackItemDto itemDto = e as PackItemDto;
							BackpackModel.Instance.UpdateItem(itemDto);

							_useDto.count -= 1;
							_leftView.SetUseDto(_useDto);
							_leftView.SetData(itemDto);
							UpdateRigth(itemDto);
						}
					});
				}
				else
				{
					TipManager.AddTip("该装备已鉴定");
				}
			}
			else
			{
				TipManager.AddTip(string.Format("{0}已用完",_useDto.item.name));
			}
		}
		else
		{
			TipManager.AddTip("请选择需要的鉴定的装备");
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
}