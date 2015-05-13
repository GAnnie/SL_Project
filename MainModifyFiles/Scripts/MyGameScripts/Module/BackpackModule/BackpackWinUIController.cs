// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BackpackWinUIController.cs
// Author   : willson
// Created  : 2015/1/9 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.equipment.data;
using System;

public class BackpackWinUIController : MonoBehaviourBase,IViewController
{
	private const string ItemsContainerViewName = "Prefabs/Module/BackpackModule/ItemsContainerView";
	private const string EquipmentCellName = "Prefabs/Module/BackpackModule/EquipmentCell";

	private BackpackWinUI _view;
	private ItemsContainerViewController _backpack;
	private ModelDisplayController _modelController;

	private Dictionary<int,EquipmentCellController> _equipDic;
	private List<PropertyItemController> _bpItemList;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<BackpackWinUI> ();
		_view.Setup(this.transform);

		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ItemsContainerViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.ItemsPos,prefab);
		//module.name = page.ToString();

		_backpack = module.GetMissingComponent<ItemsContainerViewController>();
		_backpack.InitView();
		_backpack.SetData(H1Item.PackEnum_Backpack,BackpackModel.Instance.GetDto().capability);

		RegisterEvent();

		IntiPlayerProperty();

		OnWealthChanged(PlayerModel.Instance.GetWealth());

		_modelController = ModelDisplayController.GenerateUICom (_view.ModelAnchor.transform);
		_modelController.Init (350,350);
		_modelController.SetupModel(PlayerModel.Instance.GetPlayer());

		InitEquipment();
	}

	public void RegisterEvent()
	{
		PlayerModel.Instance.OnWealthChanged += OnWealthChanged;
		PlayerModel.Instance.OnPlayerPropertyUpdate += SetPlayerProperty;
		BackpackModel.Instance.OnWearEquip += OnWearEquip;
		BackpackModel.Instance.OnTakeOffEquip += OnTakeoffEquipment;

		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		UIHelper.CreateBaseBtn(_view.ArrangeBtnPos,"整理",OnArrangeBtn);

		EventDelegate.Set(_view.AddCopperBtn.onClick,OnAddCopperBtn);
		EventDelegate.Set(_view.AddSilverBtn.onClick,OnAddSilverBtn);
	}

	private void IntiPlayerProperty()
	{
		_bpItemList = new List<PropertyItemController> (6);
		AddPlayerProperty("气血",435,_view.HpInfoPos,1);
		AddPlayerProperty("魔法",435,_view.MpInfoPos,2);
		AddPlayerProperty("攻击",210,_view.AttackInfoPos,3);
		AddPlayerProperty("防御",210,_view.DefInfoPos,4);
		AddPlayerProperty("速度",210,_view.SpeedInfoPos,5);
		AddPlayerProperty("灵力",210,_view.MagicInfoPos,6);

		SetPlayerProperty();
	}

	private void SetPlayerProperty()
	{
		PlayerPropertyInfo playerInfo = PlayerModel.Instance.GetPlayerPropertyInfo();
		
		if(playerInfo != null)
		{
			_bpItemList [0].SetValue(playerInfo.hpMax,true);
			_bpItemList [1].SetValue(playerInfo.mpMax,true);
			_bpItemList [2].SetValue(playerInfo.attack);
			_bpItemList [3].SetValue(playerInfo.defense);
			_bpItemList [4].SetValue(playerInfo.speed);
			_bpItemList [5].SetValue(playerInfo.magic);
		}
	}

	private void AddPlayerProperty(string name,int size,GameObject pos,int hintId)
	{
		GameObject propertyItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		GameObject item = NGUITools.AddChild (pos, propertyItemPrefab);
		var controller = item.GetMissingComponent<PropertyItemController> ();
		controller.SetEditable (false);
		controller.SetBgWidth(size);
		controller.InitItem (name, 0, true,hintId);
		_bpItemList.Add (controller);
	}

	private void InitEquipment()
	{
		_equipDic = new Dictionary<int, EquipmentCellController>();
		AddEquipmentCell(Equipment.EquipPartType_Weapon,_view.LEquipGrid.gameObject);
		AddEquipmentCell(Equipment.EquipPartType_Armor,_view.LEquipGrid.gameObject);
		AddEquipmentCell(Equipment.EquipPartType_Girdle,_view.LEquipGrid.gameObject);

		AddEquipmentCell(Equipment.EquipPartType_Helmet,_view.REquipGrid.gameObject);
		AddEquipmentCell(Equipment.EquipPartType_Necklace,_view.REquipGrid.gameObject);
		AddEquipmentCell(Equipment.EquipPartType_Shoe,_view.REquipGrid.gameObject);
	}

	private void AddEquipmentCell(int partType,GameObject go)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( EquipmentCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(go,prefab);
		EquipmentCellController cell = module.GetMissingComponent<EquipmentCellController>();
		//UIHelper.AdjustDepth(module,3);
		
		cell.InitView();
		cell.SetData(partType);

		_equipDic[partType] = cell;
	}

	private void OnWearEquip(PackItemDto dto)
	{
		int partType = (dto.item as Equipment).equipPartType;
		if(dto != null && dto.item is Equipment && _equipDic.ContainsKey(partType))
		{
			_equipDic[partType].SetData(partType);
		}

//		SetPlayerProperty();
	}

	private void OnTakeoffEquipment(PackItemDto dto)
	{
		int partType = (dto.item as Equipment).equipPartType;
		if(dto != null && dto.item is Equipment && _equipDic.ContainsKey(partType))
		{
			_equipDic[partType].SetData(partType);
		}
		
//		SetPlayerProperty();
	}

    public void ShowItemTips(int itemIndex)
    {
        _backpack.ShowItemTips(itemIndex);
    }

	private void OnWealthChanged(WealthNotify notify)
	{
		_view.CopperValLbl.text = notify.copper.ToString();
		_view.SilverValLbl.text = notify.silver.ToString();
	}

	private long _arrangeSpaceTime = 0;

	private void OnArrangeBtn()
	{
		long time = SystemTimeManager.Instance.GetUTCTimeStamp();
		int spaceTime = (int)(5 - (time - _arrangeSpaceTime)/1000);
		if(spaceTime <= 0)
		{
			_arrangeSpaceTime = time;
			BackpackModel.Instance.Sort();
		}
		else
		{
			TipManager.AddTip(string.Format("你刚刚进行了整理，请隔{0}秒再试",spaceTime));
		}
	}

	private void OnAddCopperBtn()
	{
		ProxyPayModule.OpenCopper();
	}

	private void OnAddSilverBtn()
	{
		ProxyPayModule.OpenSilver();
	}

	public void OnCloseBtn()
	{
		ProxyBackpackModule.Close();
	}
	
	public void Dispose()
	{
		PlayerModel.Instance.OnWealthChanged -= OnWealthChanged;
		PlayerModel.Instance.OnPlayerPropertyUpdate -= SetPlayerProperty;
		BackpackModel.Instance.OnWearEquip -= OnWearEquip;
		BackpackModel.Instance.OnTakeOffEquip -= OnTakeoffEquipment;

		_backpack.Dispose();
	}
}