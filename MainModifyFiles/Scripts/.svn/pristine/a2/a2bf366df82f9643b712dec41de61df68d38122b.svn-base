// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmenOptCellController.cs
// Author   : willson
// Created  : 2015/1/22 
// Porpuse  : 
// **********************************************************************
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.data;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;

public class EquipmentOptCellController : MonoBehaviourBase,IViewController
{
	private EquipmentOptCell _view;

	private PackItemDto _dto;
	private System.Action<EquipmentOptCellController> _OnSelectEquip;
	
	private bool _gemMode;
	private bool _enabled;

	public void InitView()
	{
		_enabled = true;
		_view = gameObject.GetMissingComponent<EquipmentOptCell> ();
		_view.Setup(this.transform);

		//_view.SelectSprite.enabled = false;
		_view.GemTransferSprite.enabled = false;

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.EquipmenOptCellBtn.onClick,OnEquipClick);
	}

	public void SetData(PackItemDto dto,bool gemMode,System.Action<EquipmentOptCellController> OnSelectEquip)
	{
		gameObject.SetActive(true);

		_dto = dto;
		_gemMode = gemMode;
		_OnSelectEquip = OnSelectEquip;

		Equipment equip = _dto.item as Equipment;
		if(equip != null)
		{
			_view.EquipNameLabel.text = equip.name;
			_view.EquipPartLabel.text = string.Format("{0}级{1}",equip.equipLevel,ItemHelper.EquipmentPartName(equip));

			if(_view.EquipIcon.atlas.GetSprite(equip.icon) != null)
				_view.EquipIcon.spriteName = equip.icon;
			else
				_view.EquipIcon.spriteName = "0";
			_view.EquipIcon.MakePixelPerfect();
		}

		SetGemModeView();
	}

	public PackItemDto GetData()
	{
		return _dto;
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

    public bool IsHide()
    {
        return !gameObject.activeSelf;
    }

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if(_enabled != value)
			{
				_enabled = value;
				_view.EquipIcon.isGrey = !_enabled;
				_view.GemTopIcon.isGrey = !_enabled;
				_view.GemBottomIcon.isGrey = !_enabled;
                //_view.SelectSprite.isGrey = !_enabled;
				_view.EquipmenOptCellBtn.enabled = _enabled;
			}
		}
	}

	public void SetGemTransferSprite(bool isCurr)
	{
		GemTransferSpriteEnabled = true;
		_view.GemTransferSprite.spriteName = isCurr ? "zhuanyifang":"shouyifang";
	}

	public bool GemTransferSpriteEnabled
	{
		get
		{
			return _view.GemTransferSprite.enabled;
		}
		set
		{
			if(_view.GemTransferSprite.enabled != value)
			{
				_view.GemTransferSprite.enabled = value;
			}
		}
	}

	private void SetGemModeView()
	{
		_view.GemTopCell.SetActive(false);
		_view.GemBottomCell.SetActive(false);

		if(_gemMode)
		{
			_view.EquipNameLabel.transform.localPosition = new Vector3(53,13,0);
			_view.EquipPartLabel.transform.localPosition = new Vector3(53,-13,0);

			if(_gemMode == true && _dto.extra is EquipmentExtraDto)
			{
				EquipmentExtraDto extraDto = _dto.extra as EquipmentExtraDto;
				
				if(extraDto.equipmentEmbedInfo != null && extraDto.equipmentEmbedInfo.embedItemIds != null && extraDto.equipmentEmbedInfo.embedItemIds.Count > 0)
				{
					for(int index = 0;index < extraDto.equipmentEmbedInfo.embedItemIds.Count;index++)
					{
						if(index < extraDto.equipmentEmbedInfo.embedLevels.Count)
						{
							int gemId = extraDto.equipmentEmbedInfo.embedItemIds[index];
							int gemLv = extraDto.equipmentEmbedInfo.embedLevels[index];
							Props gemItem = DataCache.getDtoByCls<GeneralItem>(gemId) as Props;

							if(index == 0)
							{
								_view.GemBottomCell.SetActive(true);
								
								if(_view.GemBottomIcon.atlas.GetSprite(gemItem.icon) != null)
									_view.GemBottomIcon.spriteName = gemItem.icon;
								else
									_view.GemBottomIcon.spriteName = "0";
							}
							else
							{
								_view.GemTopCell.SetActive(true);
								
								if(_view.GemTopIcon.atlas.GetSprite(gemItem.icon) != null)
									_view.GemTopIcon.spriteName = gemItem.icon;
								else
									_view.GemTopIcon.spriteName = "0";
							}
						}
					}
				}
			}
		}
		else
		{
			_view.EquipNameLabel.transform.localPosition = new Vector3(33,13,0);
			_view.EquipPartLabel.transform.localPosition = new Vector3(33,-13,0);
		}
	}

	private void OnEquipClick()
	{
		if(_OnSelectEquip != null && _enabled)
			_OnSelectEquip(this);
	}

	public void SetSelect(bool b)
	{
		if(b)
		{
			_view.SelectSprite.spriteName = "the-no-choice-lines";
			_view.EquipmenOptCellBtn.normalSprite = "the-no-choice-lines";
		}
		else
		{
			_view.SelectSprite.spriteName = "the-choice-lines";
			_view.EquipmenOptCellBtn.normalSprite = "the-choice-lines";
		}
	}

	public void Dispose()
	{
	}
}