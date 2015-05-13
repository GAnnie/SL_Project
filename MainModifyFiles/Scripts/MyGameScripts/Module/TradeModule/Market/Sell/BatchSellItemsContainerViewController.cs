// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ItemsContainerViewController.cs
// Author   : willson
// Created  : 2015/1/15 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class BatchSellItemsContainerViewController : MonoBehaviourBase,IViewController
{
	private const string ItemContainerName = "Prefabs/Module/TradeModule/Market/BatchSellItemContainer";

	private ItemsContainerView _view;

	private List<TabBtnController> _tabBtnList;
	private List<BatchSellItemContainerController> _itemContainerControllerList;

	private int _packEnum;
	private bool isSelecting = false;

	private System.Action<MarketSellItemCellController> _callback = null;

	public void InitView() {
		_view = gameObject.GetMissingComponent<ItemsContainerView> ();
		_view.Setup(this.transform);

		UIHelper.AdjustDepth(_view.ContainerScrollView.gameObject, 1);

		_tabBtnList = new List<TabBtnController>();
		_itemContainerControllerList = new List<BatchSellItemContainerController>();
		
		RegisterEvent();
	}

	public void RegisterEvent() {
		_view.ContainerScrollView.onStoppedMoving = onStoppedMoving;
		_view.ItemContainerCenterOnChild.onCenter = OnCenter;
	}
	
	#region 背包控制元素
	public void SetData(int packEnum,int capability, System.Action<MarketSellItemCellController> callback) {
		_callback = callback;

		_packEnum = packEnum;
		int tmpCapability = capability;
		int page = 1;
		while(tmpCapability > 0) {
			if(tmpCapability >= TradeDataModel.maxBatchSellPageCapability) {
				AddItemContainer(packEnum, page, TradeDataModel.maxBatchSellPageCapability);
			} else {
				AddItemContainer(packEnum,page, tmpCapability);
			}
			
			page += 1;
			//	每页显示15个
			tmpCapability -= TradeDataModel.maxBatchSellPageCapability;
		}

		if(tmpCapability == 0 && capability < 125) {
			// 增加空锁页 
			AddItemContainer(packEnum,page, tmpCapability);
		}

		SetBackpackPage(0);

		if(packEnum == H1Item.PackEnum_Backpack) {
			BackpackModel.Instance.OnAddCapability += OnAddCapability;
		}
		else if(packEnum == H1Item.PackEnum_Warehouse) {
			WarehouseModel.Instance.OnAddCapability += OnAddCapability;
			WarehouseModel.Instance.OnNewPage += OnNewPage;
		}
	}
	
	private void UpdateTabBtnState (int selectIndex) {
		for (int i=0; i<_tabBtnList.Count; ++i) {
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}
	
	private void AddItemContainer(int packEnum,int page,int pageCapability) {
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemContainerName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemContainerGrid.gameObject,prefab);

		module.name = string.Format("{0}_{1}", page.ToString(), module.name);
		UIHelper.AdjustDepth(module,1);

		BatchSellItemContainerController tBatchSellItemContainerController = module.GetMissingComponent<BatchSellItemContainerController >();
		tBatchSellItemContainerController .InitView();
		tBatchSellItemContainerController.SetData(packEnum,page, pageCapability, OnSelectItem);
		_itemContainerControllerList.Add(tBatchSellItemContainerController);

		_view.ItemContainerGrid.Reposition();

		_view.PageGroup.AddPage();
		_view.PageGroup.RefreshLayout();
	}

	public void OnSelectTabBtn(int page) {
		if(isSelecting)
			return;

		int currentPage = GetCurrentPage();
		if(currentPage != page) {
			isSelecting = true;
			
			SetBackpackPage(page);

			Vector3 localOffset = new Vector3(400f * (page - currentPage),0f,0f);
			UIScrollView mScrollView = _view.ContainerScrollView;
			Transform panelTrans = mScrollView.panel.cachedTransform;
			SpringPanel.Begin(mScrollView.panel.cachedGameObject,panelTrans.localPosition - localOffset, 8f).onFinished = OnClickDragFinished;
		}
	}
	
	private void onStoppedMoving() {
		isSelecting = false;
	}
	
	private void OnClickDragFinished() {
		_view.ItemContainerCenterOnChild.Recenter();
		isSelecting = false;
	}
	
	private void OnCenter (GameObject centeredObject) {
		if ( centeredObject == null )
			return;
		
		UIPageInfo pageInfo = centeredObject.GetComponent<UIPageInfo>();
		if ( pageInfo == null ) {
			Debug.LogError ("pageInfo == null");
			return;
		}
		
		SetBackpackPage(pageInfo.page - 1);
	}
	
	private void SetBackpackPage(int page) {
		UpdateTabBtnState(page);
		_view.PageGroup.SetCurrentPage(page);

		if(_packEnum == H1Item.PackEnum_Warehouse) {
			WarehouseModel.Instance.CurrentPage = page;
		}
	}

	public int GetCurrentPage() {
		return _view.PageGroup.GetCurrentPage();
	}
	#endregion

	public void OnSelectItem(MarketSellItemCellController cell) {

		if(cell.IsLock()) {
			AddCapability();
		} else {
			// 选择高亮
			GameDebuger.OrangeDebugLog("选择高亮");
			
			if (_callback != null) {
				_callback(cell);
			}

			/*
			for(int index = 0; index < _itemContainerControllerList.Count; index++) {
				_itemContainerControllerList[index].SelectItem(cell);
			}
			*/
		}
	}

	private void AddCapability() {
		if(_packEnum == H1Item.PackEnum_Backpack) {
			ProxyTipsModule.Open("扩充背包",DataHelper.GetStaticConfigValue(H1StaticConfigs.BACKPACK_EXPAND_CONSUME_ITEM_ID),BackpackModel.Instance.GetExpandTime(),ExpandCapability);
		} else if(_packEnum == H1Item.PackEnum_Warehouse) {
			ProxyTipsModule.Open("扩充仓库",3,DataHelper.GetStaticConfigValue(H1StaticConfigs.WAREHOUSE_EXPAND_CONSUME_COPPER ),ExpandCapability);
		}
	}

	private void ExpandCapability(List<ItemDto> costItems) {
		if(_packEnum == H1Item.PackEnum_Backpack) {
			ServiceRequestAction.requestServer(BackpackService.expand(),"expand", 
			(e) => {
				int add = DataHelper.GetStaticConfigValue(H1StaticConfigs.PACK_EXPAND_SIZE,5);
				GameLogicHelper.HandlerItemTipDto(costItems,string.Format("背包容量增加了{0}格",add));
				BackpackModel.Instance.AddCapability(add);
			});
		} else if(_packEnum == H1Item.PackEnum_Warehouse) {
			ServiceRequestAction.requestServer(WarehouseService.expand(),"expand", 
			(e) => {
				int add = DataHelper.GetStaticConfigValue(H1StaticConfigs.PACK_EXPAND_SIZE,5);
				GameLogicHelper.HandlerItemTipDto(costItems,string.Format("物品仓库容量增加了{0}格",add));
				WarehouseModel.Instance.AddCapability(add);
			});
		}
	}

	private void OnAddCapability(int capability) {
		BatchSellItemContainerController cell = _itemContainerControllerList[_itemContainerControllerList.Count - 1];
		cell.AddCapability(capability);

		if(capability % TradeDataModel.maxBatchSellPageCapability == 0 && capability < 125) {
			// 增加空锁页 
			AddItemContainer(_packEnum,_itemContainerControllerList.Count + 1,0);
		}
	}

	private void OnNewPage(int page) {
		OnSelectTabBtn(page);
	}

	public void Dispose() {
		for(int index = 0;index < _itemContainerControllerList.Count;index++) {
			_itemContainerControllerList[index].Dispose();
		}

		if(_packEnum == H1Item.PackEnum_Backpack) {
			BackpackModel.Instance.OnAddCapability -= OnAddCapability;
		} else if(_packEnum == H1Item.PackEnum_Warehouse) {
			WarehouseModel.Instance.OnAddCapability -= OnAddCapability;
			WarehouseModel.Instance.OnNewPage -= OnNewPage;
		}
	}
}