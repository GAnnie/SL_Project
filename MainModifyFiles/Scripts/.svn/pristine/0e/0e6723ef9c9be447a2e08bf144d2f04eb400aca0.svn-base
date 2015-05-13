// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WealthCellController.cs
// Author   : willson
// Created  : 2015/1/30 
// Porpuse  : 
// **********************************************************************

public class WealthCellController : MonoBehaviourBase,IViewController
{
	private WealthCell _view;

	private System.Action<int> _OnCellClickCallBack;
	private int _index;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<WealthCell>();
		_view.Setup(this.transform);
		
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.WealthCellBtn.onClick,OnCellClick);
	}

	public void SetData(int index,string icon,string costIcon,int cost,string valueIcon,int value,System.Action<int> OnCellClickCallBack)
	{
		_view.WealthIcon.spriteName = icon;

		_view.CostSprite.spriteName = costIcon;
		_view.CostLabel.text = cost.ToString();

		_view.WealthSprite.spriteName = valueIcon;
		_view.ValueLabel.text = value.ToString();

		_index = index;
		_OnCellClickCallBack = OnCellClickCallBack;
	}

	public void OnCellClick()
	{
		if(_OnCellClickCallBack != null)
		{
			_OnCellClickCallBack(_index);
		}
	}

	public void Dispose()
	{
	}
}