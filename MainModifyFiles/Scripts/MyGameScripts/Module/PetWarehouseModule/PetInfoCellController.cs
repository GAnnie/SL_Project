// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetInfoCellController.cs
// Author   : willson
// Created  : 2015/3/20 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.charactor.dto;

public class PetInfoCellController : MonoBehaviourBase,IViewController
{
	private PetInfoCell _view;

	private PetCharactorDto _petDto;
	private bool _isBattlePet;

	private System.Action<PetInfoCellController> _onClickCallBack;
	private System.Action<PetInfoCellController> _onSelectCellBack;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetInfoCell> ();
		_view.Setup(this.transform);

		this.isAddCapacity = false;
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.iconSpriteEventTrigger.onClick,OnShowPetInfo);
	}

	public void SetData(PetCharactorDto petDto,bool isBattlePet,System.Action<PetInfoCellController> onClickCallBack,System.Action<PetInfoCellController> onSelectCellBack)
	{
		this.gameObject.SetActive(true);
		SetSelect(false);

		_petDto = petDto;
		_isBattlePet = isBattlePet;
		_onClickCallBack = onClickCallBack;
		_onSelectCellBack = onSelectCellBack;

		_view.iconFlagSprite.enabled = _isBattlePet;

		if(_petDto != null)
		{
			_view.NameLbl.text = _petDto.name;
			_view.LvLbl.text = "等级： " + _petDto.level;

			_view.iconSprite.enabled = true;
			_view.iconSprite.spriteName = "0";
		}
		else
		{
			_view.iconSprite.enabled = false;
			//_view.iconSprite.spriteName = "flag_add";
			_view.NameLbl.text = "";
			_view.LvLbl.text = "";
		}
	}

	public PetCharactorDto GetData()
	{
		return _petDto;
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	private bool _isAddCapacity;
	public bool isAddCapacity
	{
		get{
			return _isAddCapacity;
		}
		set
		{
			if(_isAddCapacity != value)
			{
				_isAddCapacity = value;
				if(_isAddCapacity)
				{
					_view.iconSprite.enabled = true;
					_view.iconSprite.spriteName = "flag_add";
					_view.NameLbl.text = "增加空间";
					_view.LvLbl.text = "";
				}
				else
				{
				}
			}
		}
	}

	public void OnClick()
	{
		// 1.调用增加背包
		if(_petDto == null && isAddCapacity == true && _onClickCallBack != null)
		{
			_onClickCallBack(this);
			return;
		}

		// 2.调用选中
		if(_petDto != null && _onSelectCellBack != null)
		{
			_onSelectCellBack(this);
			return;
		}
	}

	public void OnDoubleClick()
	{
		if(_isBattlePet)
		{
			TipManager.AddTip("该宠物处于参战状态，不能放入仓库");
			return;
		}

		if(_petDto != null && _onClickCallBack != null)
		{
			_onClickCallBack(this);
		}
	}

	public void SetSelect(bool b)
	{
		if(b)
		{
			_view.SelectSprite.spriteName = "the-no-choice-lines";
			_view.PetInfoCellBtn.normalSprite = "the-no-choice-lines";
		}
		else
		{
			_view.SelectSprite.spriteName = "the-choice-lines";
			_view.PetInfoCellBtn.normalSprite = "the-choice-lines";
		}
	}

	private void OnShowPetInfo()
	{
		if(_petDto != null)
			ProxyPetTipsModule.Open(new PetPropertyInfo(_petDto));
		else if(_petDto == null && isAddCapacity && _onClickCallBack != null)
			_onClickCallBack(this);
	}

	public void Dispose()
	{
	}
}