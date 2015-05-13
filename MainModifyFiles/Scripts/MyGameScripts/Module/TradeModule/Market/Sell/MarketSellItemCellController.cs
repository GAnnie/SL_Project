// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MarketSellItemCellController.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;

using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;
using com.nucleus.player.msg;

public class MarketSellItemCellController : MonoBehaviourBase,IViewController {
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";
	
	private BackpackItemCellInMarketSellView _view;
	private ItemCellController _cell;
	
	private int _packEnum;
	
	private System.Action<MarketSellItemCellController> _onClickCallBack;
	
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<BackpackItemCellInMarketSellView> ();
		_view.Setup(this.transform);
		_view.LockSprite_UISprite.enabled = false;
		
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.gameObject,prefab);
		_cell = module.GetMissingComponent<ItemCellController>();
		_cell.InitView();
		_cell.CanDisplayCount(false);
		
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		UIEventTrigger trigger = _cell.DragDropItem.AddComponent<UIEventTrigger>();
		if(trigger)
		{
			EventDelegate.Set(trigger.onDoubleClick,OnDoubleClickItem);
			EventDelegate.Set(trigger.onPress,OnPressItem);
			EventDelegate.Set(trigger.onRelease,OnReleaseItem);
		}
	}
	
	public void SetData(int packEnum,int index,System.Action<MarketSellItemCellController> onClickCallBack)
	{
		_packEnum = packEnum;
		
		Index = index;
		_onClickCallBack = onClickCallBack;
		
		PackItemDto dto = null;
		if(packEnum == H1Item.PackEnum_Backpack)
			dto = BackpackModel.Instance.GetItemByIndex(index);
		else if(packEnum == H1Item.PackEnum_Warehouse)
			dto = WarehouseModel.Instance.GetItemByIndex(index);
		
		if (TradeDataModel.debugFirstSta && dto == null) {
			TradeDataModel.debugFirstSta = false;
			GameDebuger.OrangeDebugLog("SetData Method -> Set the itemcell data is NULL");
		}
		
		_cell.SetPackId(packEnum);
		
		// dto = null
		_cell.SetData(dto,SelectItem);
		//_cell.SetDragDropItem(true);
	}

	
	#region 设置摆摊市场出售标签下物品上架比率
	public void SetSellRatio(bool showSta, int ratioNum = 100) {
		_view.SellratioLabel_UILabel.enabled = showSta;

		if (showSta) {
			_view.SellratioLabel_UILabel.text = string.Format("{0}%", ratioNum);
		}
	}
	#endregion

	#region 市场出售设置itemcell -------------------
	public ItemCellController GetItemCellController() {
		return _cell;
	}
	
	public void SetDataInBatchSellItem(int packEnum,int index,System.Action<MarketSellItemCellController> onClickCallBack) {
		_packEnum = packEnum;
		
		Index = index;
		_onClickCallBack = onClickCallBack;
		
		PackItemDto dto = null;
		if(packEnum == H1Item.PackEnum_Backpack)
			dto = BackpackModel.Instance.GetItemByIndex(index);
		else if(packEnum == H1Item.PackEnum_Warehouse)
			dto = WarehouseModel.Instance.GetItemByIndex(index);
		
		if (TradeDataModel.debugFirstSta && dto == null) {
			TradeDataModel.debugFirstSta = false;
			GameDebuger.OrangeDebugLog("SetDataInBatchSellItem Method -> Set The Itemcell Data Is NULL");
		}
		
		_cell.SetPackId(packEnum);
		
		_cell.SetData(dto, delegate(ItemCellController cell) {
			if(_onClickCallBack != null) {
				GameDebuger.OrangeDebugLog("_onClickCallBack 点击回调");
				_onClickCallBack(this);
			}
			
			ProxyItemTipsModule.Open(cell.GetData(), cell.gameObject, false);
		});
		
		if (dto != null) {
			_cell.isGrey = !(dto.circulationType == PackItemDto.CirculationType_Stall && dto.stallable)
				|| (dto.circulationType == PackItemDto.CirculationType_Free && dto.stallable);
		}
	}
	#endregion
	
	public PackItemDto GetData()
	{
		return _cell.GetData();
	}
	
	public void SetLock(bool b)
	{
		_view.LockSprite_UISprite.enabled = b;
	}
	
	public bool IsLock()
	{
		return _view.LockSprite_UISprite.enabled;
	}
	
	public int Index
	{ 
		get
		{
			return _cell.Index;
		}
		
		set
		{
			_cell.Index = value;
		}
	}
	
	public void SelectItem(ItemCellController cell)
	{
		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			ProxyItemTipsModule.Open(cell.GetData(),cell.gameObject);
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			ProxyItemTipsModule.Open(cell.GetData(),cell.gameObject,false);
		}
		
		if(_onClickCallBack != null)
		{
			_onClickCallBack(this);
		}
	}
	
	public void OnDoubleClickItem()
	{
		if(_cell.GetData() != null)
		{
			ProxyItemTipsModule.Close();
			if(_packEnum == H1Item.PackEnum_Backpack)
			{
				if(ItemsContainerConst.ModuleType == ItemsContainerConst.ModuleType_Backpack)
				{
					//背包模式,穿戴装备 : 无改模式
				}
				else if(ItemsContainerConst.ModuleType == ItemsContainerConst.ModuleType_Warehouse)
				{
					//仓库背包模式,移入仓库
					BackpackModel.Instance.MoveToWarehouse(_cell.GetData());
				}
				
			}
			else if(_packEnum == H1Item.PackEnum_Warehouse)
			{
				ServiceRequestAction.requestServer(WarehouseService.moveTo(_cell.GetData().index,-1));
			}
		}
	}
	
	void Update () 
	{
		if(isPress)
		{
			if(++isPressTimer == 15)
			{
				_cell.SetDragDropItem(true);
				_cell.UpDragItem();
			}
		}
		else
		{
			isPressTimer = 0;
		}
	}
	
	private bool isPress = false;
	private int isPressTimer = 0;
	
	public void OnPressItem()
	{
		if(_cell.GetData() != null)
		{
			isPressTimer = 0;
			isPress = true;
		}
	}
	
	public void OnReleaseItem()
	{
		isPress = false;
		isPressTimer = 0;
		
		_cell.SetDragDropItem(false);
		_cell.DownDragItem();
	}
	
	public void Dispose()
	{
		
	}
}
