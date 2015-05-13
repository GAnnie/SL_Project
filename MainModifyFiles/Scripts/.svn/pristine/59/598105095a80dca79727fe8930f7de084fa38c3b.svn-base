// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  SpellModel.cs
// Author   : willson
// Created  : 2015/3/23 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.spell.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;

public class SpellModel
{
	private static readonly SpellModel instance = new SpellModel ();
	
	public static SpellModel Instance {
		get {
			return instance;
		}
	}
	
	private SpellModel ()
	{
	}

	private PlayerSpellList _dtos = null;

	public void Setup(PlayerSpellList dto){
		_dtos = dto;

		if(_dtos.spellList != null && _dtos.spellList.Count > 1)
		{
			_dtos.spellList.Sort(delegate(SpellDto lhs,SpellDto rhs) 
			{
				return lhs.spellId.CompareTo(rhs.spellId);
			});;
		}
	}

	public void Setup()
	{
		ServiceRequestAction.requestServer(SpellService.mySpells(),"", (e) => {
			PlayerSpellList dtos = e as PlayerSpellList;
			Setup(dtos);
		});
	}

	public List<SpellDto> GetSpellDtos()
	{
		return _dtos.spellList;
	}

	/**
	 * 选择当前修炼
	 * @parm spellId
     */
	public void SetSelectSpellId(int spellId)
	{
		_dtos.currentSpellId = spellId;
	}

	public int GetSelectSpellId()
	{
		return _dtos.currentSpellId;
	}

	public void UpDate(SpellDto dto)
	{
		for(int index = 0;index < _dtos.spellList.Count;index++)
		{
			if(_dtos.spellList[index].spellId == dto.spellId)
			{
				_dtos.spellList[index] = dto;
			}
		}
	}

	public SpellDto GetSpellDto(int spellId)
	{
		for(int index = 0;index < _dtos.spellList.Count;index++)
		{
			if(_dtos.spellList[index].spellId == spellId)
			{
				return _dtos.spellList[index];
			}
		}
		return null;
	}
}