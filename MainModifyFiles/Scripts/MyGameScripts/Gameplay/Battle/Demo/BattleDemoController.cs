using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;

using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.demo.model;
using com.nucleus.h1.logic.core.modules.demo.dto;

public class BattleDemoController : MonoBehaviour ,IViewController{

	public static int BattleSceneId = 1103;
	public static int BattleCameraId = 0;  //0是2D战场。123对应之前的1无镜头3D战场、2普通镜头3D战场、3BOSS镜头3D战场

	public static int PlayerCount = 14;

	public static bool PositionMode = false;

	public DummySetting enemyDummySetting;

	private BattleDemoView _view;

	private bool HasInited = false;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<BattleDemoView> ();
		_view.Setup (this.transform);

		enemyDummySetting = new DummySetting ();
		enemyDummySetting.attackInput = _view.AttackInput;
		enemyDummySetting.defenseInput = _view.DefenseInput;
		enemyDummySetting.hpInput = _view.HpInput;
		enemyDummySetting.speedInput = _view.SpeedInput;
		enemyDummySetting.magicInput = _view.MagicInput_UIInput;
		enemyDummySetting.monsterIdInput = _view.StyleInput;
		enemyDummySetting.activeSkillInput = _view.activeSkillInput_UIInput;
		enemyDummySetting.passiveSkillInput = _view.passiveSkillInput_UIInput;
		enemyDummySetting.monsterNameLabel = _view.NameLabel_UILabel;
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set (_view.PositionButton.onClick, OnPositionButtonClick);
		EventDelegate.Set (_view.BattleButton.onClick, OnBattleButtonClick);
		EventDelegate.Set (_view.CloseButton.onClick, OnCloseButtonClick);
		EventDelegate.Set (_view.ResumeButton_UIButton.onClick, OnResumeButtonClick);
	}
	public void Dispose ()
	{
	}
	#endregion

	public void Open()
	{
		if (_view == null)
		{
			InitView ();
			RegisterEvent ();
			SetData ();
		}
	}

	private void SetData()
	{
		ServiceRequestAction.requestServer(DemoService.playerDemoInfo(), "获取菜鸟配置", OnPlayerDemoInfoSuccess);
	}

	private void OnPlayerDemoInfoSuccess(GeneralResponse response)
	{
		HasInited = true;

		_view.EnemyNumInput.value = "1";
		_view.SceneIdInput.value = BattleSceneId.ToString();
		_view.SceneCameraInput.value = BattleCameraId.ToString();

		enemyDummySetting.SupportModify = true;

		PlayerDemoInfoDto dto = response as PlayerDemoInfoDto;
		UpdateDummySetting(enemyDummySetting, dto.monsterInfo);
	}

	private void UpdateDummySetting(DummySetting setting, DemoMonsterConfigDto monsterConfig)
	{
		if (monsterConfig != null)
		{
			setting.SpeedValue = monsterConfig.speed;
			setting.AttackValue = monsterConfig.attack;
			setting.DefenseValue = monsterConfig.defense;
			setting.HpValue = monsterConfig.hp;
			setting.MagicValue = monsterConfig.magic;
			setting.IdValue = monsterConfig.monsterId;
			setting.ActiveSkillIds = monsterConfig.activeSkillIds;
			setting.PassiveSkillIds = monsterConfig.passiveSkillIds;
		}
	}
	
	public void OnBattleButtonClick()
	{
		PositionMode = false;
		EnterBattle ();
	}

	private void OnResumeButtonClick()
	{
		if (!HasInited) return;
		
		int monsterId = enemyDummySetting.IdValue;
		Monster monster = DataCache.getDtoByCls<Monster>(monsterId);
		if (monster != null)
		{
			DemoMonsterConfigDto config = new DemoMonsterConfigDto();
			config.monsterId = monster.id;
			config.hp = monster.hp;
			config.attack = monster.attack;
			config.defense = monster.defense;
			config.speed = monster.speed;
			config.magic = monster.magic;
			config.activeSkillIds = monster.activeSkillsInfo;
			UpdateDummySetting(enemyDummySetting, config);
		}
		else
		{
			TipManager.AddTip("没有这个怪物数据");
		}
	}

	public void OnPositionButtonClick()
	{
		PositionMode = true;
		EnterBattle ();
	}

	private void EnterBattle()
	{
		if (!HasInited) return;

		int monsterId = enemyDummySetting.IdValue;
		Monster monster = DataCache.getDtoByCls<Monster>(monsterId);
		if (monster != null)
		{
			ServiceRequestAction.requestServer(DemoService.updateEnemyDummy(enemyDummySetting.GetSettingInfo()), "updateEnemyDummy", OnUpdateSuccess);
		}
		else
		{
			TipManager.AddTip("没有这个怪物数据");
		}
	}

	private void OnUpdateSuccess(GeneralResponse response)
	{
		BattleSceneId = int.Parse (_view.SceneIdInput.value);
		BattleCameraId = int.Parse (_view.SceneCameraInput.value);
		
		int count = int.Parse(_view.EnemyNumInput.value);
		if (count > 0 && count <= 14)
		{
			ServiceRequestAction.requestServer(DemoService.fightDummy(count), "请求跟菜鸟战斗", OnFightDummySuccess);
		}
	}

	private void OnFightDummySuccess(GeneralResponse response)
	{
		ProxyBattleDemo.Hide();
	}

	public void OnCloseButtonClick()
	{
		ProxyBattleDemo.Hide();
	}
}
