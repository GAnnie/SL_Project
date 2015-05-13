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
using System.Collections;

public class ItemsContainerViewController : MonoBehaviourBase,IViewController
{
	private const string ItemContainerName = "Prefabs/Module/BackpackModule/ItemContainer";

	private ItemsContainerView _view;

	private List<TabBtnController> _tabBtnList;
	private List<ItemContainerController> _itemContainerControllerList;

	private int _packEnum;
	private bool isSelecting = false;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<ItemsContainerView> ();
		_view.Setup(this.transform);

		UIHelper.AdjustDepth(_view.ContainerScrollView.gameObject,1);

		_tabBtnList = new List<TabBtnController>();
		_itemContainerControllerList = new List<ItemContainerController>();
		
		//InitBackpackPage();
		
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		_view.ContainerScrollView.onStoppedMoving = onStoppedMoving;
		_view.ItemContainerCenterOnChild.onCenter = OnCenter;


	}
	
	#region 背包控制元素
	public void SetData(int packEnum,int capability)
	{
		_packEnum = packEnum;
		int tmpCapability = capability;
		int page = 1;
		while(tmpCapability > 0)
		{
			if(tmpCapability >= ItemsContainerConst.PageCapability)
			{
				AddItemPage(packEnum,page,ItemsContainerConst.PageCapability);
			}
			else
			{
				AddItemPage(packEnum,page,tmpCapability);
			}
			
			page += 1;
			tmpCapability -= ItemsContainerConst.PageCapability;
		}

		if(tmpCapability == 0 && capability < 125)
		{
			// 增加空锁页 
			AddItemPage(packEnum,page,tmpCapability);
		}

		SetBackpackPage(0);

		if(packEnum == H1Item.PackEnum_Backpack)
		{
			BackpackModel.Instance.OnAddCapability += OnAddCapability;
		}
		else if(packEnum == H1Item.PackEnum_Warehouse)
		{
			WarehouseModel.Instance.OnAddCapability += OnAddCapability;
			WarehouseModel.Instance.OnNewPage += OnNewPage;
		}
	}
	
	public string GetTabName(int page)
	{
		switch(page)
		{
			case 1: return "一";
			case 2: return "二";
			case 3: return "三";
			case 4: return "四";
			case 5: return "五";
			default : return "Max";
		}
	}
	
	private void UpdateTabBtnState (int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i)
		{
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}
	
	private void AddItemPage(int packEnum,int page,int pageCapability)
	{
		AddTabBtn(page,GetTabName(page));
		AddItemContainer(packEnum,page,pageCapability);
	}
	
	private void AddTabBtn(int page,string tabName)
	{
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_H1) as GameObject;
		GameObject item = NGUITools.AddChild(_view.TabsGroup.gameObject, tabBtnPrefab);
		TabBtnController com = item.GetMissingComponent<TabBtnController> ();
		
		com.InitItem (page - 1, OnSelectTabBtn);
		com.SetBtnName(tabName);
		_tabBtnList.Add(com);

		_view.TabsGroup.Reposition();
	}
	
	private void AddItemContainer(int packEnum,int page,int pageCapability)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemContainerName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemContainerGrid.gameObject,prefab);
		module.name = page.ToString();
		UIHelper.AdjustDepth(module,1);
		
		ItemContainerController itemContainerController = module.GetMissingComponent<ItemContainerController>();
		itemContainerController.InitView();
		itemContainerController.SetData(packEnum,page,pageCapability,OnSelectItem);
		
		_itemContainerControllerList.Add(itemContainerController);
		_view.ItemContainerGrid.Reposition();

		_view.PageGroup.AddPage();
		_view.PageGroup.RefreshLayout();
	}

	public void OnSelectTabBtn(int page)
	{
        PageMove(page);
	}
	
    private void SelectTabBtn(int page)
    {
        int currentPage = GetCurrentPage();
        PageMove(page, 410f * (page - currentPage));
    }


    private void PageMove(int page,float speed = 8f)
    {
        if (isSelecting)
            return;

        int currentPage = GetCurrentPage();
        if (currentPage != page)
        {
            isSelecting = true;

            SetBackpackPage(page);

            Vector3 localOffset = new Vector3(410f * (page - currentPage), 0f, 0f);
            UIScrollView mScrollView = _view.ContainerScrollView;
            Transform panelTrans = mScrollView.panel.cachedTransform;
            SpringPanel.Begin(mScrollView.panel.cachedGameObject, panelTrans.localPosition - localOffset, speed).onFinished = OnClickDragFinished;
        }
    }

	private void onStoppedMoving()
	{
		isSelecting = false;
	}
	
	private void OnClickDragFinished()
	{
		_view.ItemContainerCenterOnChild.Recenter();
		isSelecting = false;
	}
	
	private void OnCenter (GameObject centeredObject)
	{
		if ( centeredObject == null )
			return;
		
		UIPageInfo pageInfo = centeredObject.GetComponent<UIPageInfo>();
		if ( pageInfo == null )
		{
			Debug.LogError ("pageInfo == null");
			return;
		}
		
		SetBackpackPage(pageInfo.page - 1);
	}
	
	private void SetBackpackPage(int page)
	{
		UpdateTabBtnState(page);
		_view.PageGroup.SetCurrentPage(page);

		if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			WarehouseModel.Instance.CurrentPage = page;
		}
	}

	public int GetCurrentPage()
	{
		return _view.PageGroup.GetCurrentPage();
	}
	#endregion

    public void ShowItemTips(int itemIndex)
    {
        StartCoroutine(WaitSelectTabBtn(itemIndex));
    }

    IEnumerator WaitSelectTabBtn(int itemIndex)
    {
        yield return true;
        // 1 计算index所在的背包页
        int page = itemIndex / 25;
        SelectTabBtn(page);
        // 2 获得相应的 ItemCell
        if (page < _itemContainerControllerList.Count)
        {
            ItemContainerController controller = _itemContainerControllerList[page];
            BackpackOrWarehouseItemCellController cell = controller.GetItemCell(itemIndex);

            // 3 选择 ItemCell
            if(cell != null && cell.GetData() != null)
            {
                yield return true;
                cell.SelectItem();
            }
        }
    }   

	public void OnSelectItem(BackpackOrWarehouseItemCellController cell)
	{
		if(cell.IsLock())
		{
			AddCapability();
		}
		else
		{
			// 选择高亮
			for(int index = 0;index < _itemContainerControllerList.Count;index++)
			{
				_itemContainerControllerList[index].SelectItem(cell);
			}
		}
	}

	private void AddCapability()
	{
		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			ProxyTipsModule.Open("扩充背包",DataHelper.GetStaticConfigValue(H1StaticConfigs.BACKPACK_EXPAND_CONSUME_ITEM_ID),BackpackModel.Instance.GetExpandTime(),ExpandCapability);
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			ProxyTipsModule.Open("扩充仓库",3,DataHelper.GetStaticConfigValue(H1StaticConfigs.WAREHOUSE_EXPAND_CONSUME_COPPER ),ExpandCapability);
		}
	}

	private void ExpandCapability(List<ItemDto> costItems)
	{
		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			ServiceRequestAction.requestServer(BackpackService.expand(),"expand", 
			(e) => {
				int add = DataHelper.GetStaticConfigValue(H1StaticConfigs.PACK_EXPAND_SIZE,5);
				GameLogicHelper.HandlerItemTipDto(costItems,string.Format("背包容量增加了{0}格",add));
				BackpackModel.Instance.AddCapability(add);
			});
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			ServiceRequestAction.requestServer(WarehouseService.expand(),"expand", 
			(e) => {
				int add = DataHelper.GetStaticConfigValue(H1StaticConfigs.PACK_EXPAND_SIZE,5);
				GameLogicHelper.HandlerItemTipDto(costItems,string.Format("物品仓库容量增加了{0}格",add));
				WarehouseModel.Instance.AddCapability(add);
			});
		}
	}

	private void OnAddCapability(int capability)
	{
		ItemContainerController cell = _itemContainerControllerList[_itemContainerControllerList.Count - 1];
		cell.AddCapability(capability);

		if(capability % ItemsContainerConst.PageCapability == 0 && capability < 125)
		{
			// 增加空锁页 
			AddItemPage(_packEnum,_itemContainerControllerList.Count + 1,0);
		}
	}

	private void OnNewPage(int page)
	{
		OnSelectTabBtn(page);
	}

	public void Dispose()
	{
		for(int index = 0;index < _itemContainerControllerList.Count;index++)
		{
			_itemContainerControllerList[index].Dispose();
		}

		if(_packEnum == H1Item.PackEnum_Backpack)
		{
			BackpackModel.Instance.OnAddCapability -= OnAddCapability;
		}
		else if(_packEnum == H1Item.PackEnum_Warehouse)
		{
			WarehouseModel.Instance.OnAddCapability -= OnAddCapability;
			WarehouseModel.Instance.OnNewPage -= OnNewPage;
		}
	}
}