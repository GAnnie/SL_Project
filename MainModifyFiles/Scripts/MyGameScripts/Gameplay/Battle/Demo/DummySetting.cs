using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.demo.dto;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.data;

public class DummySetting {

	public UILabel monsterNameLabel;

	public UIInput monsterIdInput;

	public UIInput attackInput;
	public UIInput defenseInput;
	public UIInput hpInput;
	public UIInput speedInput;
	public UIInput magicInput;

	public UIInput styleInput;
	public UIInput activeSkillInput;
	public UIInput passiveSkillInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int IdValue
	{
		get
		{
			return int.Parse(monsterIdInput.value);
		}
		set
		{
			Monster monster = DataCache.getDtoByCls<Monster>(value);
			if (monster != null)
			{
				monsterNameLabel.text = monster.name;
			}
			monsterIdInput.value = value.ToString();
		}
	}

	public string AttackValue
	{
		get
		{
			return attackInput.value;
		}
		set
		{
			attackInput.value = value;
		}
	}

	public string DefenseValue
	{
		get
		{
			return defenseInput.value;
		}
		set
		{
			defenseInput.value = value;
		}
	}

	public string HpValue
	{
		get
		{
			return hpInput.value;
		}
		set
		{
			hpInput.value = value;
		}
	}

	public string SpeedValue
	{
		get
		{
			return speedInput.value;
		}
		set
		{
			speedInput.value = value;
		}
	}

	public string MagicValue
	{
		get
		{
			return magicInput.value;
		}
		set
		{
			magicInput.value = value;
		}
	}

	public string ActiveSkillIds
	{
		get
		{
			return activeSkillInput.value;
		}
		set
		{
			activeSkillInput.value = value;
		}
	}

	public string PassiveSkillIds
	{
		get
		{
			return passiveSkillInput.value;
		}
		set
		{
			passiveSkillInput.value = value;
		}
	}

	public bool SupportModify
	{
		set
		{
			attackInput.enabled = value;
			speedInput.enabled = value;
			defenseInput.enabled = value;
			hpInput.enabled = value;
		}
	}

	public DemoMonsterConfigDto GetSettingInfo()
	{
		DemoMonsterConfigDto dto = new DemoMonsterConfigDto();
		dto.monsterId = IdValue;
		dto.speed = SpeedValue;
		dto.attack = AttackValue;
		dto.defense = DefenseValue;
		dto.hp = HpValue;
		dto.magic = MagicValue;
		dto.activeSkillIds = ActiveSkillIds;
		dto.passiveSkillIds = PassiveSkillIds;
		return dto;
	}
}
