// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TradePetLvCellController.cs
// Author   : willson
// Created  : 2015/3/31 
// Porpuse  : 
// **********************************************************************

public class TradePetLvCellController : MonoBehaviourBase,IViewController
{
	private TradePetLvCell _view;
	private int _tradePetLv;

	private System.Action<TradePetLvCellController> _OnClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<TradePetLvCell> ();
		_view.Setup(this.transform);

		_view.SelectSprite.enabled = false;
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{

	}

	public void SetData(int lv,System.Action<TradePetLvCellController> OnClickCallBack)
	{
		this.TradePetLv = lv;
		_OnClickCallBack = OnClickCallBack;
	}

	public int TradePetLv
	{
		get
		{
			return _tradePetLv;
		}
		set
		{
			_tradePetLv = value;
			_view.NameLabel.text = _tradePetLv + " 级宠物";
		}
	}

	void OnClick()
	{
		if(_OnClickCallBack != null)
			_OnClickCallBack(this);
	}

	public void SetSelect(bool b)
	{
		_view.SelectSprite.enabled = b;
	}

	public void Dispose()
	{
	}
}