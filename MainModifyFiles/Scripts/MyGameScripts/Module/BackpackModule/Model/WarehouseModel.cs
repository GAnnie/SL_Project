// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WarehouseModel.cs
// Author   : willson
// Created  : 2015/1/16 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.services;
using com.nucleus.player.msg;
using System.Collections.Generic;

public class WarehouseModel
{
	private static readonly WarehouseModel _instance = new WarehouseModel ();
	
	public static WarehouseModel Instance {
		get {
			return _instance;
		}
	}

	public event System.Action<PackItemDto> OnUpdateItem;
	public event System.Action<int> OnDeleteItem;
	public event System.Action<int> OnAddCapability;
	public event System.Action<int> OnNewPage;

	private PackDto _dto;
	private Dictionary<int, PackItemDto> _itemsDic;

	private WarehouseModel ()
	{
		_itemsDic = new Dictionary<int, PackItemDto>();
	}

	public void Setup()
	{
		if(_dto == null)
		{
			Update(null);
		}
	}

	public void Destroy()
	{
		_dto = null;
		_itemsDic.Clear ();
	}

	public void Update(System.Action callBackFun)
	{
        long version = GameDataManager.Instance.GetDataVersion(GameDataManager.Data_Self_PackDto_Warehouse);
		ServiceRequestAction.requestServer(WarehouseService.warehouse(version),"warehouse", 
		(e) => {
			_dto = e as PackDto;
			if (_dto == null)
			{
                _dto = GameDataManager.Instance.GetDataObj<PackDto>(GameDataManager.Data_Self_PackDto_Warehouse);
			}
			else
			{
                GameDataManager.Instance.PushData(GameDataManager.Data_Self_PackDto_Warehouse, _dto);
			}

			if(_dto != null)
			{
				_itemsDic.Clear();
				for(int index = 0;index < _dto.items.Count;index++)
				{
					_itemsDic[_dto.items[index].index] = _dto.items[index];
				}
			}

			if(callBackFun != null)
				callBackFun();
		});
	}

	public PackDto GetDto()
	{
		return _dto;
	}
	
	public PackItemDto GetItemByIndex(int index)
	{
		if(_itemsDic.ContainsKey(index))
		{
			return _itemsDic[index];
		}
		return null;
	}

	public void UpdateItem(PackItemDto dto)
	{
		for(int index = 0;index < _dto.items.Count;index++)
		{
			if(_dto.items[index].index == dto.index)
			{
				// update
				_dto.items[index] = dto;
				_itemsDic[dto.index] = dto;

				if(OnUpdateItem != null)
					OnUpdateItem(dto);
				return;
			}
		}
		
		// add
		_dto.items.Add(dto);
		_itemsDic[dto.index] = dto;

		if(OnUpdateItem != null)
			OnUpdateItem(dto);
	}
	
	public void DeleteItem(int index)
	{
		for(int i = 0;i < _dto.items.Count;i++)
		{
			if(_dto.items[i].index == index)
			{
				_dto.items.RemoveAt(i);
				break;
			}
		}
		
		if(_itemsDic.ContainsKey(index))
		{
			_itemsDic.Remove(index);
		}

		if(OnDeleteItem != null)
			OnDeleteItem(index);
	}

	public void AddCapability(int add)
	{
		_dto.capability += add;
		if(OnAddCapability != null)
		{
			OnAddCapability(_dto.capability);
		}
	}

	public int CurrentPage
	{
		get;set;
	}

	public void SetNewPage(int page)
	{
		if(OnNewPage != null)
			OnNewPage(page);
	}

	public void Sort(int page)
	{
		// page = [0,4]
		int indexBegin = ItemsContainerConst.PageCapability * page;
		int indexEnd = indexBegin + ItemsContainerConst.PageCapability - 1;
		int sortIndexBegin = 0;
		int sortCount = 0;
		for(int index = 0;index < _dto.items.Count;index++)
		{
			if(indexBegin == _dto.items[index].index)
			{
				sortIndexBegin = index;
			}

			if(indexBegin <= _dto.items[index].index && _dto.items[index].index <= indexEnd)
			{
				sortCount++;
			}
			else if(_dto.items[index].index > indexEnd)
			{
				break;
			}
		}

		if(sortCount > 0)
		{
			ServiceRequestAction.requestServer(WarehouseService.sort(page),"sort",
			(e) => {
				_dto.items.Sort(indexBegin,sortCount,new PackItemDtoComparer());
				_itemsDic.Clear();

				int sortIndexEnd = sortIndexBegin + sortCount;
				int pageIndex = 0;

				for(int index = 0;index < _dto.items.Count;index++)
				{
					if(sortIndexBegin <= index && index < sortIndexEnd)
					{
						if(_dto.items[index].index != indexBegin + pageIndex)
						{
							int removeIndex = _dto.items[index].index;
							_dto.items[index].index = indexBegin + pageIndex;
							_itemsDic[_dto.items[index].index] = _dto.items[index];

							if(OnDeleteItem != null)
								OnDeleteItem(removeIndex);

							if(OnUpdateItem != null)
								OnUpdateItem(_dto.items[index]);
						}

						pageIndex++;
					}
					else
					{
						_itemsDic[_dto.items[index].index] = _dto.items[index];
					}
				}
			});
		}
	}
}