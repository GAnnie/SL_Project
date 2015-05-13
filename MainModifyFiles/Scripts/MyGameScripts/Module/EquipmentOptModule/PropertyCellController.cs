// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PropertyCellController.cs
// Author   : willson
// Created  : 2015/1/22 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.equipment.dto;
using UnityEngine;

public class PropertyCellController : MonoBehaviourBase,IViewController
{
	private PropertyCell _view;

	private EquipmentAptitude _dto;

	private System.Action<PropertyCellController> _onPropertyClickCallBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PropertyCell> ();
		_view.Setup(this.transform);

		//_view.SelectSprite.enabled = false;
	}

	public void RegisterEvent()
	{
	}

	public void SetData(EquipmentAptitude dto,System.Action<PropertyCellController> onPropertyClickCallBack)
	{
		gameObject.SetActive(true);
		_view.SelectSprite.enabled = false;

		_dto = dto;
		_onPropertyClickCallBack = onPropertyClickCallBack;

		PropertyStr(_view.PropertyLabel,ItemHelper.AptitudeTypeName(dto.aptitudeType),dto.value);
	}

	public void SetNull()
	{
		Index = -1;
		_view.PropertyLabel.text = "无";
	}

	private void PropertyStr(UILabel lbl,string name,int value)
	{
		if(value != 0)
		{
			if(value > 0)
			{
				lbl.text = name + "+" + value + " ";
			}
			else
			{
				lbl.text = name + value + " ";
			}
		}
		else
		{
			lbl.text = name + value + " ";
		}
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public bool isShow()
	{
		return gameObject.activeSelf;
	}

	public void SetSelect(bool b)
	{
		_view.SelectSprite.enabled = b;
	}

	public int Index
	{
		get;
		set;
	}

	void OnClick()
	{
		if(_onPropertyClickCallBack != null)
			_onPropertyClickCallBack(this);
	}

	public void Dispose()
	{
	}
}