// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemsContainerConst.cs
// Author   : willson
// Created  : 2015/1/16 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using System.Collections.Generic;

public class ItemsContainerConst
{
	// 1.背包 2.仓库 用于 BackpackOrWarehouseItemCellController 判读双击事件
	public static int ModuleType = 0;
	public const int ModuleType_Backpack = 1;
	public const int ModuleType_Warehouse = 2;

	public const int PageCapability = 25;

	static public int SortPackItemDto (PackItemDto lhs, PackItemDto rhs) 
	{
		int result = lhs.item.sort.CompareTo(rhs.item.sort);
		if(result == 0)
			result = lhs.itemId.CompareTo(rhs.itemId);
		if(result == 0)
			result = lhs.index.CompareTo(rhs.index);
		
		return result; 
	}
}

public class PackItemDtoComparer : IComparer<PackItemDto>
{
	public int Compare(PackItemDto lhs, PackItemDto rhs)
	{
		return ItemsContainerConst.SortPackItemDto(lhs,rhs);
	}
}