// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PayUIWinController.cs
// Author   : willson
// Created  : 2015/1/30 
// Porpuse  : 
// **********************************************************************
using UnityEngine;

public class WealthBuyWinUIController : MonoBehaviourBase,IViewController
{
	private const string WealthCellNAME = "Prefabs/Module/PayModule/WealthCell";

	private WealthBuyWinUI _view;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<WealthBuyWinUI>();
		_view.Setup(this.transform);

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnClose);
	}

	public void AddWealthCell(int index,string icon,string costIcon,int cost,string valueIcon,int value)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( WealthCellNAME ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.WealthGrid.gameObject,prefab);
		WealthCellController cell = module.GetMissingComponent<WealthCellController>();

		cell.InitView();
		cell.SetData(index,icon,costIcon,cost,valueIcon,value,OnBuy);

		//_itemList.Add(cell);
	}

	protected virtual void OnBuy(int index)
	{

	}

	public void OnClose()
	{
		ProxyPayModule.Close();
	}

	public void Dispose()
	{
	}
}