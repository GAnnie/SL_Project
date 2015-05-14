﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.commons.dispatcher;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.demo.dto;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.equipment.data;

public class BattleController : MonoBehaviourBase, IViewController
{
	public static BattleController Instance;

	// 战斗视图
	private BattleView _battleView;

	//战斗网络处理
	private BattleNetworkHandler _battleNetworkHandler;
	//战斗指令处理	
	private BattleInstController _instController;

	//游戏对象
	private Video _gameVideo;

	//战斗回合时间控制
	private BattleLaunchTimer _battleLaunchTimer;

	//当前下达指令的角色
	private MonsterController choosePet = null;

	
	private GameObject battleCameraObj;
	private Camera battleCamera;
	private string currentCameraName = "";
	
	private List< MonsterController > monsterList = new List<MonsterController>();
	
	private MonoTimer monoTimer;
	
	//是否使用托管,静态保持这个设置状态
	public static bool isAIManagement = true;
	
    private BattleTurnController battleTurnControl;
	//private BattleSkillDisplayPanel battleSkillDisplayPanel = null;

	//---------------------------------------------------------------------------------------------------
	
	//当前捕捉警告率， 当警告率大于100时, 则不可捕捉
	private int _currentAlertRate;

	//当前战斗
	//private IBattle battle = null;

    /* BATTLE ROUND */
    public int _battleRound;
    public bool _isGameOver;
	public bool _isAutoPlayMode;

	//本回合死亡的怪物Id列表
	private List<int> battleRoundDieMonsterIds;

	//战斗中的hero列表
	private List<MonsterController> battleHeroList= null;

	//战斗中的敌方列表
	private List<MonsterController> battleEnemyList= null;

	//上阵过的战斗宠物列表
	private List<long> _oldBattlePetList = null;

	//已经使用的道具数量
	private int _itemUsedCount = 0;
    
    /** 胜利玩家编号，0表示怪物，null表平局 */
    public long _winId;

	public int _lastPlayerSkillId = 0;
	public int _lastPetSkillId = 0;

	public static readonly float PLAYER_Y_Rotation = 100f;
	public static readonly float ENEMY_Y_Rotation = -80f;

	public enum BattleResult
	{
		DRAW = 0,
		WIN,
		LOSE,
		Retreat,
	}

	public enum BattleSceneStat
	{
		GAME_OVER = 0,
		BATTLE_READY,
		FINISH_COMMAND,
		ON_PROGRESS,
		ON_SELECT_SKILL,
		ON_SELECT_TARGET,
		ON_CAPTURE,
		ON_CAPTURE_SUCCESS,
	}

	public enum ActionState
	{
		HERO = 0,
		PET,
	}

	public bool isInBattle = false;
	public bool isEscapedSuccess = false;
	public bool isEscapedFailed = false;
	
	public bool isStarted = false;
	
	public BattleSceneStat battleState = BattleSceneStat.BATTLE_READY;

	public ActionState actionState = ActionState.HERO;

	public static string Prefab_Path = "Prefabs/Module/Battle/BattleView";

	#region Interface for IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_battleView = gameObject.GetMissingComponent<BattleView> ();
		_battleView.Setup(this.transform);

		_battleView.AutoSkillSelectGroup.SetActive (false);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set(_battleView.AutoButton.onClick, OnAutoButtonClick);
		EventDelegate.Set(_battleView.ManualButton.onClick, OnManualButtonClick);
		EventDelegate.Set(_battleView.AttackButton.onClick, OnAttackButtonClick);
		EventDelegate.Set(_battleView.SkillButton.onClick, OnSkillButtonClick);
		EventDelegate.Set(_battleView.SummonButton.onClick, OnSummonButtonClick);
		EventDelegate.Set(_battleView.StuntButton.onClick, OnStuntButtonClick);
		EventDelegate.Set(_battleView.ItemButton.onClick, OnItemButtonClick);
		EventDelegate.Set(_battleView.DefenseButton.onClick, OnDefenseButtonClick);
		EventDelegate.Set(_battleView.ProtectButton.onClick, OnProtectButtonClick);
		EventDelegate.Set(_battleView.CatchButton.onClick, OnCatchButtonClick);
		EventDelegate.Set(_battleView.RetreatButton.onClick, OnRetreatButtonClick);
		EventDelegate.Set(_battleView.CancelButton.onClick, OnCancelButtonClick);

		EventDelegate.Set(_battleView.DefaultSkillButton.onClick, OnDefaultSkillClick);
		EventDelegate.Set(_battleView.PlayerDefaultSkillButton.onClick, OnPlayerDefaultSkillClick);
		EventDelegate.Set(_battleView.PetDefaultSkillButton.onClick, OnPetDefaultSkillClick);

		_battleLaunchTimer = _battleView.RoundTimeLabel.gameObject.GetMissingComponent<BattleLaunchTimer>();
		_battleLaunchTimer.OnAutoTimeFinish += HandleOnAutoTimeFinish;
		_battleLaunchTimer.OnFinishedDelegate += OnLaunchTimeFinishDelegate;
	}

	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose(){
	}
	#endregion

	#region BattleViewController
	private void HandleOnAutoTimeFinish ()
	{
		if (isAIManagement == true)
		{
			OnAutoButtonClick ();
		}
	}

	private void OnLaunchTimeFinishDelegate()
	{
		isAIManagement = true;
		OnAutoButtonClick ();
	}

	private void Hide()
	{
		_battleView.MainUI.gameObject.SetActive (false);
	}


	//设置自动模式
	public void SetAutoMode(bool auto, bool inProcess, bool inFinishCommand, ActionState actionState)
	{
		_battleView.ManualButton.gameObject.SetActive(auto);
		_battleView.AutoButton.gameObject.SetActive(!auto);
		bool hide = auto || inProcess || inFinishCommand;
		HideOperateButton (hide);

		if (!hide)
		{
			_battleView.CatchButton.gameObject.SetActive (actionState == ActionState.HERO);
			_battleView.SummonButton.gameObject.SetActive (actionState == ActionState.HERO);
			_battleView.StuntButton.gameObject.SetActive (actionState == ActionState.HERO);
		}

		auto = auto && !inFinishCommand;

		_battleView.PlayerDefaultSkillButton.gameObject.SetActive (auto);
		_battleView.PetDefaultSkillButton.gameObject.SetActive ( GetMyPet() && auto);
		_battleView.BottomRightBg.SetActive (auto);

		if (!auto)
		{
			_battleView.AutoSkillSelectGroup.SetActive (false);
		}

		//_battleView.BottomButtonGrid_UIGrid.gameObject.SetActive (false);
		//_battleView.RightButtonGrid_UIGrid.gameObject.SetActive (false);

		//_battleView.BottomButtonGrid_UIGrid.repositionNow = true;
		//_battleView.RightButtonGrid_UIGrid.repositionNow = true;

		//_battleView.BottomButtonGrid_UIGrid.gameObject.SetActive (true);
		//_battleView.RightButtonGrid_UIGrid.gameObject.SetActive (true);

		//CancelInvoke ("repositionNow");
		//Invoke ("repositionNow", 0.05f);

		repositionNow ();
	}

	private void repositionNow()
	{
		_battleView.BottomButtonGrid_UIGrid.Reposition ();
		_battleView.RightButtonGrid_UIGrid.Reposition();
	}
	
	private void HideOperateButton(bool hide)
	{
		if (hide)
		{
			_battleView.DefaultSkillButton.gameObject.SetActive (!hide);
		}
		_battleView.AttackButton.gameObject.SetActive(!hide);
		_battleView.SkillButton.gameObject.SetActive(!hide);
		_battleView.DefenseButton.gameObject.SetActive(!hide);
		_battleView.ProtectButton.gameObject.SetActive (!hide);
		_battleView.CatchButton.gameObject.SetActive (!hide);
		_battleView.RetreatButton.gameObject.SetActive (!hide);
		_battleView.SummonButton.gameObject.SetActive (!hide);
		_battleView.StuntButton.gameObject.SetActive (!hide);
		_battleView.ItemButton.gameObject.SetActive (!hide);
	}

	private void HideAllButton(bool hide)
	{
		_battleView.ManualButton.gameObject.SetActive(!hide);
		_battleView.AutoButton.gameObject.SetActive(!hide);
		HideOperateButton (hide);
	}

	private MonsterController _showStatusMonster = null;
	public void ShowMonsterStatus(MonsterController mc)
	{
		_showStatusMonster = mc;

		if (mc == null)
		{
			_battleView.StatusSprite.SetActive(false);
		}
		else
		{
			_battleView.StatusSprite.SetActive(true);
		}

		UpdateMonsterStatus ();
	}

	private void UpdateMonsterStatus()
	{
		if (_showStatusMonster != null)
		{
			_battleView.NameLabel_UILabel.text = "[b]"+_showStatusMonster.videoSoldier.name;
			_battleView.HPLabel_UILabel.text = "[b]"+_showStatusMonster.currentHP + "/" + _showStatusMonster.maxHP;
			_battleView.MPLabel_UILabel.text = "[b]"+_showStatusMonster.currentMP + "/" + _showStatusMonster.maxMP;

			if (_showStatusMonster.IsPet())
			{
				_battleView.SPGroup.SetActive(false);
				_battleView.StatusSprite_UISprite.height = 123;
			}
			else
			{
				_battleView.SPGroup.SetActive(true);
				_battleView.SPLabel_UILabel.text = "[b]"+_showStatusMonster.currentSP + "/" + _showStatusMonster.maxSP;
				_battleView.StatusSprite_UISprite.height = 152;
			}
		}
	}

	//显示提示
	public void ShowTip(string tip)
	{
		if (string.IsNullOrEmpty(tip))
		{
			_battleView.TipSprite.gameObject.SetActive(false);
		}
		else
		{
			_battleView.TipLabel.text = tip;
			_battleView.TipSprite.gameObject.SetActive(true);
		}
	}
	
	//显示目标选择
	public void ShowTargetSelect(Skill skill)
	{
		if (skill == null)
		{
			_battleView.TargetSelectSprite.gameObject.SetActive(false);
		}
		else
		{
			_battleView.CharactorLabel_UILabel.text = choosePet.videoSoldier.name;
			_battleView.SkillNameLabel.text = GetSkillName(skill);
			_battleView.TargetSelectSprite.gameObject.SetActive(true);
		}
	}
	
	public void ShowRoundWating(bool show)
	{
		_battleView.RoundWatingSprite.gameObject.SetActive (show);
	}
	#endregion

	public static void Setup(Video video)
	{
		GameObject ui = UIModuleManager.Instance.OpenFunModule(Prefab_Path, UILayerType.BaseModule, false);
		Instance = ui.GetMissingComponent<BattleController>();
		Instance.PlayBattle(video);
	}

	// Use this for initialization
	void Start()
	{
		_battleNetworkHandler = new BattleNetworkHandler();
		_battleNetworkHandler.Start(this);

		_instController = new BattleInstController();
		_instController.Start(this);

		GameObject go = null;
		
		battleCamera = LayerManager.Instance.GetBattleFollowCamera();
		battleCameraObj = battleCamera.gameObject;

		battleHeroList = new List<MonsterController>();
		battleEnemyList = new List<MonsterController>();

		_oldBattlePetList = new List<long> ();
		
		monoTimer = TimerManager.GetTimer("BattleTimer");
		
		go = GameObject.Find("BattleTurnPanel");
		if (go)
		{
			//battleTurnControl = go.GetComponent<BattleTurnController>();
			//battleTurnControl.Hide();
		}

//		GameObject cameraPathGo = GameObject.FindGameObjectWithTag("CameraPath");
//		if(cameraPathGo == null)
//		{
//			ResetCamera();
//		}

		//ReScaleStage();
		//SwitchCamera( "ready" );
		
		AdjustGameContingent();
		StartGameVideo();
	}

	private void CancelAutoButton()
	{
		if (isAIManagement)
		{
			isAIManagement = false;

		}
		UpdateBattleButtonView ();
		CancelInvoke("OnAutoButtonClick");
	}

	//响应手动战斗点击
	private void OnManualButtonClick()
	{
		if (_isGameOver)
		{
			return;
		}

		CancelInvoke("OnAutoButtonClick");

		if (battleState == BattleSceneStat.FINISH_COMMAND)
		{
			return;
		}

		ServiceRequestAction.requestServer(CommandService.cancelAutoBattle(_gameVideo.id), "manual", (e)=>{
			isAIManagement = false;
			
			UpdateBattleButtonView ();
			
			if (battleState == BattleSceneStat.ON_PROGRESS || battleState == BattleSceneStat.FINISH_COMMAND)
			{
				TipManager.AddTip("下回合开始时显示操作菜单");
			}
			else
			{
				ShowActionWaitTip();
			}
		});
	}

	//响应自动战斗点击
	private void OnAutoButtonClick()
	{
		if (_isGameOver)
		{
			return;
		}

		CancelInvoke("OnAutoButtonClick");

		ServiceRequestAction.requestServer(CommandService.autoBattle(_gameVideo.id), "auto", (e)=>{
			isAIManagement = true;
			UpdateBattleButtonView ();
			if (battleState != BattleSceneStat.ON_PROGRESS)
			{
				OnActionRequestSuccess(e);
			}
			else
			{
				SetBattleStat (BattleSceneStat.ON_PROGRESS);
			}
		});
	}

	//响应攻击按钮点击
	private void OnAttackButtonClick()
	{
		CancelAutoButton ();

		Skill skill = DataCache.getDtoByCls<Skill>(BattleManager.GetNormalAttackSkillId());

		ChoosePet(GetCurrentActionMonster(), skill);
	}

	//响应法术按钮点击
	private void OnSkillButtonClick()
	{
		CancelAutoButton ();

		int count = 0;
		MonsterController mc = GetCurrentActionMonster ();
		if (mc.IsPet())
		{
			PetPropertyInfo petPropertyInfo = PetModel.Instance.GetPetInfoByUID (mc.GetId());
			
			List<int> skills = petPropertyInfo.petDto.skillIds;

			for (int i=0,len=skills.Count; i<len; i++)
			{
				Skill skill = DataCache.getDtoByCls<Skill>(skills[i]);
				if (skill != null && skill.activeSkill)
				{
					count++;
				}
			}
		}
		else
		{
			count = 1;
		}

		if (count > 0) 
		{
			ProxyBattleModule.OpenSkillSelect (GetCurrentActionMonster(), OnSkillSelectDelegate);
			SetBattleStat (BattleSceneStat.ON_SELECT_SKILL);
			
			ShowTip (null);
		} 
		else 
		{
			TipManager.AddTip("你的宠物不会主动法术");
		}
	}

	//响应召唤按钮点击
	private void OnSummonButtonClick()
	{
		ProxyBattleModule.OpenSummon ( OnSummonPet );
	}

	private void OnSummonPet(long petId)
	{
		GeneralRequestInfo requestInfo = CommandService.changePet (_gameVideo.id, petId, _battleRound);
		
		bool waitNextAction = (actionState == ActionState.HERO && GetMyPet() != null);
		
		if (waitNextAction)
		{
			ServiceRequestAction.requestServer(requestInfo, "changePet", (e)=>{
				choosePet.NeedReady = false;
				actionState = ActionState.PET;
				SetBattleStat( BattleSceneStat.BATTLE_READY );
			});
		}
		else
		{
			ServiceRequestAction.requestServer(requestInfo, "changePet", (e)=>{
				choosePet.NeedReady = false;
				OnActionRequestSuccess(e);
			});
		}
	}

	//响应特技按钮点击		
	private void OnStuntButtonClick()
	{
		Dictionary<long, PackItemDto> equipDic = BackpackModel.Instance.GetBodyEquip();
		List<int> skills = new List<int>();
		foreach( PackItemDto dto in equipDic.Values )
		{
			EquipmentExtraDto extraDto = dto.extra as EquipmentExtraDto;
			
			if(extraDto != null && extraDto.activeSkillIds != null)
			{
				skills.AddRange(extraDto.activeSkillIds.ToArray());
			}
		}

		if (skills.Count > 0)
		{
			ProxyBattleModule.OpenStuntSelect (GetCurrentActionMonster(), skills, OnSkillSelectDelegate);
			SetBattleStat (BattleSceneStat.ON_SELECT_SKILL);
			
			ShowTip (null);
		}
		else
		{
			TipManager.AddTip("你的装备没有附带特技");
		}
	}

	//响应物品按钮点击
	private void OnItemButtonClick()
	{
		ProxyItemUseModule.OpenBattleItem(_itemUsedCount, GetCurrentActionMonster().GetCharactorType(), OnBattleSelectCallback);
	}

	private void OnBattleSelectCallback(PackItemDto packItem)
	{
		CancelAutoButton ();

		Skill skill = new Skill();
		skill.id = BattleManager.GetUseItemSkillId();
		skill.logicId = packItem.index;
		skill.name = packItem.item.name;
		skill.skillAiId = (packItem.item as Props).targetType;
		ChoosePet(GetCurrentActionMonster(), skill);
	}

	//响应防御按钮点击
	private void OnDefenseButtonClick()
	{
		CancelAutoButton ();

		GeneralRequestInfo requestInfo = CommandService.skillTarget(_gameVideo.id, _battleRound, !choosePet.IsPet(), BattleManager.GetDefenseSkillId(), choosePet.GetId());;

		bool waitNextAction = (actionState == ActionState.HERO && GetMyPet() != null);

		if (waitNextAction)
		{
			ServiceRequestAction.requestServer(requestInfo, "defense", (e)=>{
				choosePet.NeedReady = false;
				actionState = ActionState.PET;
				SetBattleStat( BattleSceneStat.BATTLE_READY );
			});
		}
		else
		{
			ServiceRequestAction.requestServer(requestInfo, "defense", (e)=>{
				choosePet.NeedReady = false;
				OnActionRequestSuccess(e);
			});
		}
	}

	private void OnActionRequestSuccess(GeneralResponse response)
	{
		ShowRoundWating (true);
		_battleLaunchTimer.Hide();
		SetBattleStat(BattleSceneStat.FINISH_COMMAND);
	}

	//响应保护按钮点击
	private void OnProtectButtonClick()
	{
		CancelAutoButton ();

		Skill skill = DataCache.getDtoByCls<Skill>(BattleManager.GetProtectSkillId());
		ChoosePet(GetCurrentActionMonster(), skill);
	}

	//响应捕捉按钮点击
	private void OnCatchButtonClick()
	{
		CancelAutoButton ();

		Skill skill = DataCache.getDtoByCls<Skill>(BattleManager.GetCaptureSkillId());
		
		if (actionState == ActionState.HERO)
		{
			ChoosePet(GetMyHero(), skill);
		}
		else if (actionState == ActionState.PET)
		{
			ChoosePet(GetMyPet(), skill);
		}
	}

	private void OnRetreatButtonClick()
	{
		CancelAutoButton ();

		GeneralRequestInfo requestInfo = CommandService.skillTarget(_gameVideo.id, _battleRound, !choosePet.IsPet(), BattleManager.GetRetreatSkillId(), choosePet.GetId());;
		
		bool waitNextAction = (actionState == ActionState.HERO && GetMyPet() != null);
		
		if (waitNextAction)
		{
			ServiceRequestAction.requestServer(requestInfo, "retreat", (e)=>{
				choosePet.NeedReady = false;
				actionState = ActionState.PET;
				SetBattleStat( BattleSceneStat.BATTLE_READY );
			});
		}
		else
		{
			ServiceRequestAction.requestServer(requestInfo, "retreat", (e)=>{
				choosePet.NeedReady = false;
				OnActionRequestSuccess(e);
			});
		}
	}

	//响应取消按钮点击
	private void OnCancelButtonClick()
	{
		int skillId = choosePet.GetBattleTargetSelector ().GetSkillId ();

		ShowTargetSelect(null);
		ShowTip(null);
		
		if (skillId != BattleManager.GetNormalAttackSkillId()
		    && skillId != BattleManager.GetProtectSkillId()
		    && skillId != BattleManager.GetCaptureSkillId()
		    && skillId != BattleManager.GetUseItemSkillId())
		{
			Skill skill = DataCache.getDtoByCls<Skill>(skillId);
			if (skill is EquipmentSkill)
			{
				OnStuntButtonClick();
			}
			else
			{
				OnSkillButtonClick();
			}
		}
		else
		{
			SetBattleStat( BattleSceneStat.BATTLE_READY );
		}
	}

	private void OnDefaultSkillClick()
	{
		if (actionState == ActionState.HERO)
		{
			ChoosePet(GetMyHero(), DataCache.getDtoByCls<Skill>(_lastPlayerSkillId));
		}
		else if (actionState == ActionState.PET)
		{
			ChoosePet(GetMyPet(), DataCache.getDtoByCls<Skill>(_lastPetSkillId));
		}
	}

	private void OnPlayerDefaultSkillClick()
	{
		OpenAutoSkillSelect (GetMyHero());
	}

	private void OnPetDefaultSkillClick()
	{
		OpenAutoSkillSelect (GetMyPet());
	}

	private static string SkillCellPrefabPath = "Prefabs/Module/Battle/BattleUI/SkillButtonCell";

	private void OpenAutoSkillSelect(MonsterController mc)
	{
		_battleView.AutoSkillSelectGroup.SetActive (true);
		_battleView.AutoSkillSelectGrid_UIGrid.gameObject.RemoveChildren ();

		string tip = "";

		List<int> skillIds = new List<int> ();

		if (mc.IsPet())
		{
			PetPropertyInfo petPropertyInfo = PetModel.Instance.GetPetInfoByUID (mc.GetId());
			tip = "选择宠物自动技能";

			skillIds.AddRange( petPropertyInfo.petDto.skillIds );
		}
		else
		{
			tip = "选择人物自动技能";

			Faction faction = mc.GetFaction ();
			
			List<int> fskills = new List<int>();
			fskills.Add(faction.mainFactionSkillId);
			fskills.AddRange(faction.propertyFactionSkillIds);
			
			for (int i=0,len=fskills.Count; i<len; i++)
			{
				FactionSkill factionSkill = DataCache.getDtoByCls<FactionSkill>(fskills[i]);
				foreach(SkillInfo info in factionSkill.skillInfos)
				{
					if (info.skill.activeSkill && FactionSkillModel.Instance.GetFactionSkillLevel(factionSkill.id) >= info.acquireFactionSkillLevel)
					{
						skillIds.Add(info.skillId);
					}
				}
			}
		}

		skillIds.Add (BattleManager.GetNormalAttackSkillId());
		skillIds.Add (BattleManager.GetDefenseSkillId());

		_battleView.AutoSkillSelectTipLabel_UILabel.text = tip;

		int showCount = 0;

		for (int i=0,len=skillIds.Count; i<len; i++)
		{
			Skill skill = DataCache.getDtoByCls<Skill>(skillIds[i]);

			if (skill != null && skill.activeSkill)
			{
				GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(SkillCellPrefabPath ) as GameObject;
				GameObject go = NGUITools.AddChild(_battleView.AutoSkillSelectGrid_UIGrid.gameObject, prefab);
				go.tag = "AutoSkillSelectIcon";

				int skillId = skill.id;
				if (skillId < 10)
				{
					skillId += 9000;
				}
				
				go.name = skillId.ToString();
				SkillButtonCell cell = go.GetMissingComponent<SkillButtonCell> ();
				cell.Setup (go.transform);

				SetDefaultSkillIcon(cell.SkillSprite_UISprite, cell.SkillIconGroup, cell.SkillIcon_UISprite, cell.NameLabel_UILabel, skill.id);
				
				EventDelegate.Set(cell.SkillButtonCell_UIButton.onClick, delegate {
					OnDefaultSkillSelectDelegate(mc, skill);
					HideAutoSkillSelect();
				});

				showCount++;
			}
		}

		_battleView.AutoSkillSelectBg_UISprite.height = Mathf.CeilToInt (skillIds.Count / 3f) * 85 + 43;
		_battleView.AutoSkillSelectGrid_UIGrid.Reposition ();
	}

	private void HideAutoSkillSelect()
	{
		if (_battleView.AutoSkillSelectGroup.activeInHierarchy) 
		{
			_battleView.AutoSkillSelectGrid_UIGrid.gameObject.RemoveChildren ();
			_battleView.AutoSkillSelectGroup.SetActive (false);
		}
	}
	
	private void UpdatePlayerAndPetDefaultSkill()
	{
		Skill skill;

		if (GetMyHero() != null)
		{
			SetDefaultSkillIcon(_battleView.PlayerDefaultSkillSprite_UISprite, _battleView.PlayerDefaultSkillIconGroup, _battleView.PlayerDefaultSkillIcon_UISprite, _battleView.PlayerDefaultSkillNameLabel_UILabel, GetMyHero().videoSoldier.defaultSkillId);
		}
		
		if (GetMyPet() != null)
		{
			SetDefaultSkillIcon(_battleView.PetDefaultSkillSprite_UISprite, _battleView.PetDefaultSkillIconGroup, _battleView.PetDefaultSkillIcon_UISprite, _battleView.PetDefaultSkillNameLabel_UILabel, GetMyPet().videoSoldier.defaultSkillId);
		}
	}

	private void SetDefaultSkillIcon(UISprite sprite, GameObject iconGroup, UISprite iconSprite, UILabel label, int skillId)
	{
		Skill skill = DataCache.getDtoByCls<Skill>(skillId);

		if ( IsDefaultSkill(skill.id) )
		{
			sprite.gameObject.SetActive(true);
			iconGroup.SetActive(false);
			sprite.spriteName = GetDefaultSkillIconName(skillId);
		}
		else
		{
			sprite.gameObject.SetActive(false);
			iconGroup.SetActive(true);
			label.text = "[b]"+skill.name;
			iconSprite.spriteName = GetDefaultSkillIconName(skillId);
		}
	}
	
	//响应法术选择结果
	private void OnDefaultSkillSelectDelegate(MonsterController mc,  Skill skill)
	{
		Debug.Log("OnDefaultSkillSelectDelegate");
		if (skill != null)
		{
			int playerSkillId = 0;

			if (mc.IsPlayerMainCharactor())
			{
				GetMyHero().videoSoldier.defaultSkillId = skill.id;
				SetDefaultSkillIcon(_battleView.PlayerDefaultSkillSprite_UISprite, _battleView.PlayerDefaultSkillIconGroup, _battleView.PlayerDefaultSkillIcon_UISprite, _battleView.PlayerDefaultSkillNameLabel_UILabel, skill.id);
			}

			int petSkillId = 0;

			if (mc.IsPlayerPet())
			{
				GetMyPet().videoSoldier.defaultSkillId = skill.id;
				SetDefaultSkillIcon(_battleView.PetDefaultSkillSprite_UISprite, _battleView.PetDefaultSkillIconGroup, _battleView.PetDefaultSkillIcon_UISprite, _battleView.PetDefaultSkillNameLabel_UILabel, skill.id);
			}

			if (GetMyHero() != null)
			{
				playerSkillId = GetMyHero().videoSoldier.defaultSkillId;
			}

			if (GetMyPet() != null)
			{
				petSkillId = GetMyPet().videoSoldier.defaultSkillId;
			}

			ServiceRequestAction.requestServer(CommandService.populateDefaultBattleSkillId(playerSkillId, petSkillId));
		}
	}

	private string GetDefaultSkillIconName(int skillId)
	{
		if (skillId == BattleManager.GetNormalAttackSkillId())
		{
			return "attack";
		}
		else if (skillId == BattleManager.GetDefenseSkillId())
		{
			return "defense";
		}
		else
		{
			return skillId.ToString();
		}
	}

	private bool IsDefaultSkill(int skillId)
	{
		if (skillId == BattleManager.GetNormalAttackSkillId())
		{
			return true;
		}
		else if (skillId == BattleManager.GetDefenseSkillId())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//响应退出按钮点击
	public void OnExitButtonClick()
	{
		CancelAutoButton ();

		ServiceRequestAction.requestServer(DemoService.exitBattle(), "exitBattle");
		ExitBattleWithoutReport ();
		Debug.Log("OnExitButtonClick");
	}

	//响应取消法术选择点击
	private void OnSkillSelectCancelButtonClick()
	{
		Debug.Log("OnSkillSelectCancelButtonClick");
		SetBattleStat( BattleSceneStat.BATTLE_READY );
		ProxyBattleModule.HideSkillSelect ();
	}

	//响应法术选择结果
	private void OnSkillSelectDelegate(Skill skill)
	{
		Debug.Log("OnSkillSelectDelegate");

		if (actionState == ActionState.HERO)
		{
			if (skill != null)
			{
				_lastPlayerSkillId = skill.id;
			}
			ChoosePet(GetMyHero(), skill);
		}
		else if (actionState == ActionState.PET)
		{
			if (skill != null)
			{
				_lastPetSkillId = skill.id;
			}
			ChoosePet(GetMyPet(), skill);
		}
	}

	private void UpdateDefaultSkillIcon()
	{
		string name = "";
		int defaultSkillId = 0;

		if (actionState == ActionState.HERO)
		{
			if (_lastPlayerSkillId > 10)
			{
				name = _lastPlayerSkillId.ToString();
				defaultSkillId = _lastPlayerSkillId;
			}
		}
		else if (actionState == ActionState.PET)
		{
			if (_lastPetSkillId > 10)
			{
				name = _lastPetSkillId.ToString();
				defaultSkillId = _lastPetSkillId;
			}
		}

		if (string.IsNullOrEmpty(name) || isAIManagement)
		{
			_battleView.DefaultSkillButton.gameObject.SetActive(false);
		}
		else
		{
			Skill skill = DataCache.getDtoByCls<Skill> (defaultSkillId);

			_battleView.DefaultSkillButton.gameObject.SetActive(true);
			_battleView.DefaultSkillButton.GetComponentInChildren<UISprite> ().spriteName = name;
			_battleView.DefaultSkilllNameLabel_UILabel.text = skill.name;
		}
	}

	private void ShowActionWaitTip()
	{
		ChoosePet(GetCurrentActionMonster(), null);
	}

	//获取技能名字
	private string GetSkillName(Skill skill)
	{
		if (skill == null)
		{
			return "";
		}
		else
		{
			return skill.name;
		}
	}

	public void PlayBattle(Video video)
	{
		_gameVideo = video;
		_battleRound = video.currentRound;
		if (video is DemoVideo || video is PvpVideo)
		{
			isAIManagement = false;
		}

		BattleLaunchTimer.MAX_INSTRUCTION_TIME = _gameVideo.commandOptSec;

		InitView ();
		RegisterEvent ();
	}

	private BattlePlayerInfoDto GetHeroBattlePlayerInfo()
	{
		if (_gameVideo.playerInfos != null)
		{
			foreach(BattlePlayerInfoDto info in _gameVideo.playerInfos)
			{
				if (info.playerId == PlayerModel.Instance.GetPlayerId())
				{
					return info;
				}
			}
		}
		return null;
	}

	public void StartGameVideo()
	{
		BattlePlayerInfoDto info = GetHeroBattlePlayerInfo();
		if (info != null)
		{
			_itemUsedCount = info.useItemCount;
			foreach(long id in info.allPetSoldierIds)
			{
				AddOldBattlePetList(id);
			}
		}

		ShowTeamMonsters(MonsterController.MonsterSide.Enemy);
		ShowTeamMonsters(MonsterController.MonsterSide.Player);

		ShowBattleUI();

		UpdateDefaultSkillIcon ();
		UpdatePlayerAndPetDefaultSkill ();

		UpdateBattleButtonView ();

		PlayGameVideo();

		if (PlayerModel.Instance.IsAutoFram || isAIManagement)
		{
			Invoke("OnAutoButtonClick", 4f);
		}
	}

	private void PlayGameVideo()
	{
		_instController.PlayGameVideo(_gameVideo);
	}

	private int OnVideoSoldierSort(VideoSoldier x, VideoSoldier y)
	{
		if (x.position > y.position){
			return 1;
		}else if (x.position < y.position){
			return -1;
		}else{
			return 0;
		}
	}
	
	//调整队伍，把主角的队伍放到A队, 也就是判断B队伍是否主角，如果是主角， 则换成AB队互换
	private void AdjustGameContingent()
	{
//		if (_gameVideo is NpcMonsterVideo)
//		{
//			NpcAppearanceDto npcAppearanceDto = (_gameVideo as NpcMonsterVideo).npcAppearanceDto;
//			if (npcAppearanceDto != null)
//			{
//				for (int i=0,len=_gameVideo.bteam.teamSoldiers.Count; i<len; i++)
//				{
//					VideoSoldier soldier = _gameVideo.bteam.teamSoldiers[i];
//					if (soldier.id == npcAppearanceDto.soldierId)
//					{
//						soldier.wpmodel = npcAppearanceDto.wpmodel;
//					}
//				}
//			}
//		}

		long playerId = PlayerModel.Instance.GetPlayerId();
		if (_gameVideo.bteam.playerIds.Contains(playerId))
		{
			VideoTeam temp = _gameVideo.ateam;
			_gameVideo.ateam = _gameVideo.bteam;
			_gameVideo.bteam = temp;
		}

		_gameVideo.ateam.teamSoldiers.Sort(OnVideoSoldierSort);
		_gameVideo.bteam.teamSoldiers.Sort(OnVideoSoldierSort);
	}

	public BattleInstController GetInstController()
	{
		return _instController;
	}

	private void ShowTeamMonsters(MonsterController.MonsterSide side)
	{
		VideoTeam videoTeam = null;
		
		if (side == MonsterController.MonsterSide.Player)
		{
			videoTeam = _gameVideo.ateam;
		}
		else
		{
			videoTeam = _gameVideo.bteam;
		}
		
		CreateMonsters( videoTeam.teamSoldiers, side);
	}

	public void SetBattleStat( BattleSceneStat stat )
	{
		UpdateBattleStatus (stat);

		BattleSceneStat lastBattleState = battleState;
		battleState = stat;

		Debug.Log("SetBattleStat " + stat.ToString());

		if (stat == BattleSceneStat.BATTLE_READY)
		{
			ShowTargetSelect(null);
			ShowTip(null);
			if (isAIManagement == false)
			{
				ProxyBattleModule.HideSkillSelect();
			}
			ShowRoundWating (false);
			HideSelectEffect();
			UpdateDefaultSkillIcon();
			UpdateBattleButtonView();

			if (isAIManagement == false)
			{
				ShowActionWaitTip();
			}
		}
		else if (stat == BattleSceneStat.ON_PROGRESS)
		{
			if (lastBattleState != BattleSceneStat.FINISH_COMMAND)
			{
				isAIManagement = true;
			}

			foreach (MonsterController mc in monsterList)
			{
				mc.NeedReady = false;
			}

			ShowTargetSelect(null);
			ShowTip(null);
			if (isAIManagement == false)
			{
				ProxyBattleModule.HideSkillSelect();
			}
			_battleLaunchTimer.Hide();
			ShowRoundWating (false);
			HideSelectEffect();
			UpdateBattleButtonView();
			ProxyBattleModule.CloseSummon();
			ProxyItemUseModule.Close();
		}
		else if (stat == BattleSceneStat.FINISH_COMMAND)
		{
			foreach (MonsterController mc in monsterList)
			{
				if (mc.IsPlayerCtrlCharactor())
				{
					mc.NeedReady = false;
				}
			}

			ShowTargetSelect(null);
			ShowTip(null);
			if (isAIManagement == false)
			{
				ProxyBattleModule.HideSkillSelect();
			}
			_battleLaunchTimer.Hide();
			ShowRoundWating (true);
			HideSelectEffect();
			UpdateBattleButtonView();
		}
	}

	#region Camera Control

	
	public void ResetCamera()
	{
		LayerManager.Instance.GameCameraAnimator.enabled = false;
//		CameraConfigInfo info = new CameraConfigInfo();
//		info.panAngle = 220.0f;
//		info.tiltAngle = 34.0f;
//		info.distance  = 12.0f;
//		info.lookAtPosition = Vector3.zero;		
		
//		CameraController ctl = battleCameraObj.GetComponent< CameraController >();
//		ctl.enabled = true;
//		ctl.camInfo = info;
//		ctl.TransformCamera();
//		ctl.enabled = false;
	}

	public Camera GetCurrentCamera()
	{
		return battleCamera;
	}	
	#endregion
	
	public bool CreateMonsters( List< VideoSoldier > strikers, MonsterController.MonsterSide side)
	{
		if ( strikers == null || strikers.Count <= 0 )
			return false;
		
		float yRotation = PLAYER_Y_Rotation;
		if ( side == MonsterController.MonsterSide.Enemy )
		{
			yRotation = ENEMY_Y_Rotation;
		}

		int mcIndex = 1;
		foreach ( VideoSoldier soldier in strikers )
		{
			MonsterController mc = CreateMonster( soldier, yRotation, BattlePositionCalculator.GetMonsterPosition(soldier, side), side, true );
			
			if (side == MonsterController.MonsterSide.Player){
				battleHeroList.Add(mc);
			}
			else
			{
				battleEnemyList.Add(mc);
			}

			mc.gameObject.name = mc.gameObject.name + "_" + mcIndex;

			mcIndex ++;
		}
		
		return true;
	}

	public void ShowBattleUI()
	{
		SetPlayMode( IsAutoGameVideo() );

		ShowMonsterStatus (null);
		UpdateBattleButtonView ();
		ShowTip("");
		ShowTargetSelect(null);
		ShowRoundWating (false);
		UpdateBattleRound(null);

		//Invoke("HideAllMonsterName", 7f);
	}

	private void UpdateBattleButtonView()
	{
		SetAutoMode(isAIManagement, battleState == BattleSceneStat.ON_PROGRESS, battleState == BattleSceneStat.FINISH_COMMAND, actionState);
	}

	MonsterController CreateMonster( VideoSoldier monsterData, float yRotation, Vector3 position, MonsterController.MonsterSide side, bool isStriker )
	{
		GameObject go = new GameObject();
		MonsterController mc = go.AddComponent< MonsterController >();
		if ( mc == null )
		{
			GameDebuger.Log("Add MonsterController component failed!!!!");
			return null;
		}

		GameObjectExt.AddPoolChild (LayerManager.Instance.BattleActors, go);
		
		//float scale = monsterData.pet.scale / 10000.0f;
		
		mc.transform.localEulerAngles = new Vector3( 0, yRotation, 0 );
		//mc.transform.localScale = new Vector3( scale, scale, scale );
		mc.transform.localScale = Vector3.one;
		mc.transform.localPosition = position;

		NpcAppearanceDto npcAppearanceDto = null;

		if (_gameVideo is NpcMonsterVideo)
		{
			npcAppearanceDto =  (_gameVideo as NpcMonsterVideo).npcAppearanceDto;
			if (npcAppearanceDto != null && npcAppearanceDto.soldierId != monsterData.id)
			{
				npcAppearanceDto = null;
			}
		}
		mc.InitMonster( monsterData, side, isStriker, this, npcAppearanceDto );

		Debug.Log(mc.GetDebugInfo());

		if (mc.IsPlayerPet())
		{
			AddOldBattlePetList(mc.GetId());
		}

		monsterList.Add( mc );

		return mc;
	}

	public bool IsAutoGameVideo()
	{
		return IsPvPGameVideo();
	}

	public bool IsPvPGameVideo()
	{
		return false;
	}	

	public void UpdateBattleRound(VideoRound videoRound)
    {
		if (videoRound != null)
		{
			_battleRound = videoRound.count;
			_winId = videoRound.winId;
			_isGameOver = videoRound.over;
		}

		int showRound = _battleRound == 0 ? 1 : _battleRound;
		UpdateBattleRound (showRound);
		_battleLaunchTimer.Hide ();
		ShowRoundWating (false);
    }

	public void UpdateBattleRound(int round)
	{
		_battleView.RoundLabel.text = round.ToString();
	}

	//从战斗中逃跑
	public void RetreatBattle(long playerId)
	{
		if (playerId == PlayerModel.Instance.GetPlayerId())
		{
			_winId = 0;
			_isGameOver = true;
			_Result = BattleResult.Retreat;
			
			HideBattleUI();
			SetBattleStat( BattleSceneStat.GAME_OVER );
			
			RetreatOtherMonster(playerId, true);
		}
		else
		{
			RetreatOtherMonster(playerId, false);
		}
	}

	//召唤宠物
	public void SwitchPet(VideoSoldier soldier)
	{
		float yRotation = PLAYER_Y_Rotation;

		int count = 5-GetOldBattlePetCount()-1;

		MonsterController myPet = GetMyPet ();
		if (myPet != null)
		{
			myPet.LeaveBattle();
		}
		
		MonsterController mc = CreateMonster( soldier, yRotation, BattlePositionCalculator.GetMonsterPosition(soldier, MonsterController.MonsterSide.Player), MonsterController.MonsterSide.Player, true );
		mc.PlaySummonEffect ();

		if (count > 0)
		{
			TipManager.AddTip(string.Format("本场战斗还能召唤{0}只宠物", count));
		}

		PetModel.Instance.ChangeBattlePetByUID(soldier.id);
		battleHeroList.Add(mc);

		mc.gameObject.name = mc.gameObject.name + "_" + soldier.id;

		if (GetMyPet() != null)
		{
			SetDefaultSkillIcon(_battleView.PetDefaultSkillSprite_UISprite, _battleView.PetDefaultSkillIconGroup, _battleView.PetDefaultSkillIcon_UISprite, _battleView.PetDefaultSkillNameLabel_UILabel, GetMyPet().videoSoldier.defaultSkillId);
		}
	}

	//召唤小怪
	public void CallPet(VideoSoldier soldier)
	{
		float yRotation = PLAYER_Y_Rotation;
		
		MonsterController mc = CreateMonster( soldier, yRotation, BattlePositionCalculator.GetMonsterPosition(soldier, MonsterController.MonsterSide.Player), MonsterController.MonsterSide.Player, true );
		mc.PlaySummonEffect ();
		
		battleHeroList.Add(mc);
		
		mc.gameObject.name = mc.gameObject.name + "_" + soldier.id;
	}


	public void UpdateBattleStatus(BattleSceneStat stat)
	{
		//_battleView.RoundLabel.text = stat.ToString ();
	}

	private void SetPlayMode(bool autoPlayMode){
		_isAutoPlayMode = autoPlayMode;
	}
	
	#region  for UI
	//-----------------------------  Testing ----------------------------------------------

	public List< MonsterController > GetMonsterList( MonsterController.MonsterSide side = MonsterController.MonsterSide.Player, bool includeDead = true )
	{
		List< MonsterController > ml = new List<MonsterController>();
		
		foreach ( MonsterController mc in monsterList )
		{
			if ( mc == null || ( mc.side != side && side != MonsterController.MonsterSide.None ) 
				|| ( mc.IsDead() && !includeDead ) )
			{
				continue;
			}

			ml.Add( mc );
		}
		
		return ml;		
	}
	
	public List< MonsterController > GetMonsterList( bool isStriker, MonsterController.MonsterSide side = MonsterController.MonsterSide.Player, bool includeDead = true )
	{
		List< MonsterController > ml = new List<MonsterController>();
		
		foreach ( MonsterController mc in monsterList )
		{
			if ( mc == null || ( mc.side != side && side != MonsterController.MonsterSide.None ) || mc.isStriker != isStriker
				|| ( mc.IsDead() && !includeDead ) )
			{
				continue;
			}

			ml.Add( mc );
		}
		
		return ml;
	}


	private void CheckNextRound(){
		UpdateBuffState();
		
		_instController.CheckNextRound();
	}

	//战斗准备状态
	public void SetReadyState()
	{
		ShowRoundWating (false);
		if (_gameVideo.currentRoundCommandOptRemainSec != 0)
		{
			_battleLaunchTimer.LaunchTimer(_gameVideo.currentRoundCommandOptRemainSec, _gameVideo.cancelAutoSec);
			_gameVideo.currentRoundCommandOptRemainSec = 0;
		}
		else
		{
			_battleLaunchTimer.LaunchTimer(_gameVideo.commandOptSec, _gameVideo.cancelAutoSec);
		}
		isStarted = false;

		if (monsterList != null){
			foreach (MonsterController mc in monsterList)
			{
				if (mc.IsPet() || mc.IsMainCharactor())
				{
					mc.NeedReady = true;
				}
				else
				{
					mc.NeedReady = false;
				}
			}
		}
		
		if (isAIManagement == false)
		{
			actionState = ActionState.HERO;
		}

		SetBattleStat (BattleSceneStat.BATTLE_READY);
		UpdateBattleRound (_battleRound+1);
	}

	public void AddDieMonster(long monsterId)
	{
//		if (battleRoundDieMonsterIds.Contains(monsterId) == false){
//			battleRoundDieMonsterIds.Add(monsterId);
//		}
	}

	#endregion

    public bool NeedReport()
    {
		return true;
    }

    public void ShowBattleReport()
    {
		ExitBattleWithoutReport();
    }

	private void HideBattleUI()
	{
		Hide();
	}
	
	private void DelayeDestroyBattleReport()
	{
		Destory();
	}
	
	private void ExitBattleWithoutReport()
	{
		NGUIFadeInOut.FadeOut(OnFinishFadeOut);
	}
	
    private void OnFinishFadeOut()
    {
        Destory();

		BattleManager.Instance.BattleDestroy(_Result, _gameVideo);
    }

	private Transform CheckCameraCollider( Vector3 point )
	{
		List< MonsterController > mcList = GetMonsterList( true, MonsterController.MonsterSide.None );
		
		if ( mcList == null )
			return null;
		
		foreach ( MonsterController mc in mcList )
		{
			if ( mc == null )
				continue;

			if (mc.petCollider == null)
				continue;

			if ( mc.petCollider.enabled == false )
				continue;
			
			if ( mc.gameObject.activeSelf == false )
				continue;
			
			if ( mc.petCollider.bounds.Contains( point ) )
				return mc.modelRoot;
		}
		
		return null;
	}

	// Update is called once per frame
	void Update()
	{
		if (_isGameOver)
		{
			return;
		}

		UpdateMonsterStatus ();

		if (Input.GetMouseButtonDown(0))
		{
			ShowMonsterStatus(null);

			if (_battleView.AutoSkillSelectGroup.activeInHierarchy)
			{
				GameObject hitObject = null;
				
				Vector3 mousePosition = Input.mousePosition;
				Ray ray = LayerManager.Instance.UICamera.ScreenPointToRay( mousePosition );
				
				RaycastHit[] hits = Physics.RaycastAll(ray,500f);
				RaycastHit hit;

				bool needHideAutoSkillSelect = true;

				for (int i=0,len=hits.Length; i<len; i++)
				{
					hit = hits[i];
					if (hit.collider.gameObject.tag == "AutoSkillSelectIcon")
					{
						needHideAutoSkillSelect = false;
						break;
					}
				}

				if (needHideAutoSkillSelect)
				{
					HideAutoSkillSelect();
				}
			}


			if (battleState == BattleSceneStat.ON_SELECT_TARGET && choosePet != null)
			{
				GameObject hitObject = null;
				
				Ray ray = battleCamera.ScreenPointToRay( Input.mousePosition );
				
				RaycastHit hit;
				
				if ( !Physics.Raycast( ray, out hit, 500f, 1 << LayerMask.NameToLayer(GameTag.Tag_BattleActor) ))
				{
					Transform mc = CheckCameraCollider( battleCamera.transform.position );
					
					if ( null != mc )
					{
						hitObject = mc.gameObject;
					}
					else
					{
						return;
					}
				}
				
				if ( hitObject == null )
					hitObject = hit.collider.gameObject;
				
				MonsterController target = null;
				
				if ( hitObject.tag == GameTag.Tag_BattleActor )
				{
					MonsterController hitMonster = hitObject.transform.parent.parent.gameObject.GetComponent< MonsterController >();
					if (hitMonster == null){
						hitMonster = hitObject.transform.parent.parent.parent.gameObject.GetComponent< MonsterController >();
					}
					
					if (hitMonster == null){
						hitMonster = hitObject.transform.parent.parent.parent.parent.gameObject.GetComponent< MonsterController >();
					}
					
					if (hitMonster == null){
						hitMonster = hitObject.transform.parent.gameObject.GetComponent< MonsterController >();
					}
					
					Debug.Log("check battleState = " + battleState.ToString());
					
					target = hitMonster;
					
					bool passChooseTarget = ChooseTargetPet(target);

					if (passChooseTarget)
					{
						CancelAutoButton ();
					}
				}
			}
		}
	}

	//选择目标宠物
	public bool ChooseTargetPet(MonsterController target)
	{
		if (choosePet == null || choosePet.battleTargetSelector == null)
		{
			return false;
		}
		
		if ( !choosePet.battleTargetSelector.CanSetTarget( choosePet, target ) )
		{
			return false;
		}

		if ( !choosePet.battleTargetSelector.CanSetCaptureTarget( choosePet, target ) )
		{
			return false;
		}

		GameDebuger.Log("Set Target Success");
		choosePet.SetSkillTarget( target.GetId() );

		BattleTargetSelector targetSelector = choosePet.battleTargetSelector;
		GameDebuger.Log( choosePet.battleTargetSelector.getSelectParam() );

		GeneralRequestInfo requestInfo = null;
		if (targetSelector.IsItemSkill())
		{
			requestInfo = CommandService.useItem(_gameVideo.id, _battleRound, targetSelector.IsMainCharactor(), targetSelector.GetTargetSoldierId(), targetSelector.GetSkillLogicId());
		}
		else
		{
			requestInfo = CommandService.skillTarget(_gameVideo.id, _battleRound, targetSelector.IsMainCharactor(), targetSelector.GetSkillId(), targetSelector.GetTargetSoldierId());;
		}

		bool waitNextAction = (actionState == ActionState.HERO && GetMyPet() != null);

		if (waitNextAction)
		{
			ServiceRequestAction.requestServer(requestInfo, "skill", (e)=>{
				choosePet.NeedReady = false;
				choosePet = null;
				actionState = ActionState.PET;
				target.PlayTargetClickEffect ();
				SetBattleStat( BattleSceneStat.BATTLE_READY );
			});
		}
		else
		{
			ServiceRequestAction.requestServer(requestInfo, "skill", (e)=>{
				choosePet.NeedReady = false;
				choosePet = null;
				target.PlayTargetClickEffect ();
				OnActionRequestSuccess(e);
			});
		}

		return true;
	}

	//选择下达指令宠物
	public void ChoosePet( MonsterController pet, Skill skill)
	{
		choosePet = pet;

		if ( choosePet == null )
			return;
		
		if (!choosePet.CanChoose()){
			TipManager.AddTip("本回合不能行动");
			return;
		}

		UpdateDefaultSkillIcon ();
		ShowTargetSelect(skill);

		if (skill == null)
		{
			if (actionState == ActionState.HERO)
			{
				ShowTip("请下达人物指令");
			}
			else if (actionState == ActionState.PET)
			{
				ShowTip("请下达宠物指令");
			}
			skill = DataCache.getDtoByCls<Skill>(BattleManager.GetNormalAttackSkillId());
			UpdateBattleButtonView();
		}
		else
		{
			ShowTip(null);
			HideAllButton(true);
		}

		choosePet.SetSelectSkill(skill);

		ShowSelectEffect ();

		SetBattleStat( BattleSceneStat.ON_SELECT_TARGET );
	}

	public void RemoveMonster(MonsterController mc)
	{
		monsterList.Remove (mc);
		if (mc.IsPlayerPet())
		{
			AddOldBattlePetList(mc.GetId());
		}
		UpdateBattleButtonView();
	}

	private void AddOldBattlePetList(long id)
	{
		if (_oldBattlePetList.Contains(id) == false)
		{
			_oldBattlePetList.Add(id);
		}
	}

	public void ShowSelectEffect()
	{
		foreach ( MonsterController mc in monsterList )
		{
			if ( choosePet.battleTargetSelector.CanSetTarget( choosePet, mc ) )
			{
				mc.ShowSelectEffect();
			}
			else{
				mc.HideSelectEffect();
			}
		}
	}

	public void HideSelectEffect()
	{
		foreach ( MonsterController mc in monsterList )
		{
			mc.HideSelectEffect();
		}
	}

	private MonsterController GetCurrentActionMonster()
	{
		if (actionState == ActionState.HERO)
		{
			return GetMyHero();
		}
		else if (actionState == ActionState.PET)
		{
			return GetMyPet();
		}
		else
		{
			return null;
		}
	}

	//获得我的英雄
	public MonsterController GetMyHero()
	{
		foreach ( MonsterController mc in monsterList )
		{
			if (mc.IsPlayerMainCharactor())
			{
				return mc;
			}
		}
		return  null;
	}

	// 获得我的宠物
	public MonsterController GetMyPet()
	{
		foreach ( MonsterController mc in monsterList )
		{
			if (mc.IsPlayerPet())
			{
				return mc;
			}
		}
		return  null;
	}

	public MonsterController GetPlayerPet(long playerId)
	{
		foreach ( MonsterController mc in monsterList )
		{
			if (mc.IsPet() && mc.GetPlayerId() == playerId)
			{
				return mc;
			}
		}
		return  null;
	}

	private void RetreatOtherMonster(long playerId, bool exitBattle)
	{
		MonsterController[] mcList = monsterList.ToArray ();

		int count = 0;

		foreach ( MonsterController mc in mcList )
		{
			if ( !mc.IsPet() && !mc.IsMainCharactor() && mc.GetPlayerId() == playerId)
			{
				mc.RetreatFromBattle(2, 0.5f*count);
				count++;
			}
		}

		if (count > 0)
		{
			if (exitBattle)
			{
				Invoke ("ShowBattleReport", 0.5f*count);
			}
			else
			{
				Invoke ("AfterRetreatOtherMonster", 0.5f*count);
			}
		}
		else
		{
			if (exitBattle)
			{
				ShowBattleReport();
			}
			else
			{
				AfterRetreatOtherMonster();
			}
		}
	}

	private void AfterRetreatOtherMonster()
	{
		_instController.CheckFinish ();
	}

	//-------------------------------------------------------------------------------------------------------------	
	private BattleResult _Result = BattleResult.LOSE;

	public void CheckGameState()
	{
		GameDebuger.Log("CheckGameState "+battleState);
		
		if ( _isGameOver && battleState != BattleSceneStat.GAME_OVER )
		{
			HideBattleUI();

			GameDebuger.Log( "Game Over" );
			SetBattleStat( BattleSceneStat.GAME_OVER );
			
			if (_Result != BattleResult.Retreat)
			{
				if ( _winId == null )
				{
					// DRAW
					_Result = BattleResult.DRAW;
				}
				else if (_winId == GetPlayerTeamLeaderId())
				{
					// WIN
					_Result = BattleResult.WIN;
					
				}
				else
				{
					// LOST
					_Result = BattleResult.LOSE;
				}
			}

			ShowBattleReport();

			return;
		}
		else
		{
			CheckNextRound();
		}
	}

	private long GetPlayerTeamLeaderId()
	{
		return _gameVideo.ateam.teamSoldiers[0].playerId;
	}

	public MonsterController GetMonsterFromSoldierID( long id )
	{
		foreach ( MonsterController mc in monsterList )
		{
			if ( mc.GetId() == id )
				return mc;
		}
		
		return null;
	}
	
	private void UpdateBuffState(){
		foreach ( MonsterController mc in monsterList )
		{
			if ( mc && mc.side == MonsterController.MonsterSide.Player )
				mc.battleTargetSelector = null;	
			
			if ( ! mc.isStriker )
				continue;
			
			mc.UpdateBuffState();
		}		
	}
	
    public void ResetPetMessageState()
    {
        foreach (MonsterController mc in monsterList)
        {
            mc.ClearMessageEffect(true);
        }
    }
	
	public MonoTimer GetMonoTimer()
	{
		return monoTimer;
    }

	public string CurrentCameraName
	{
		get
		{
			return currentCameraName;
		}
	}
	
	public void SetSkillPanelText( string text , System.Action OnSkillNameShowCallback)
	{
		//battleSkillDisplayPanel.SetText( text , OnSkillNameShowCallback);
	}

	//******************************************************************************************************************************************************
	
    public void Destory()
    {
		//BattleController.isAIManagement = false;

		_battleNetworkHandler.StopNotifyListener();
		ProxyBattleModule.CloseSkillSelect ();

//		CameraMask.Instance.HideWhite();
		
		UIModuleManager.Instance.CloseModule(Prefab_Path);		
		
		if (monsterList != null){
	        foreach (MonsterController mc in monsterList)
	        {
	            mc.DestroyMe();
	        }
	
	        monsterList.Clear();
	        monsterList = null;			
		}

		RenderSettings.skybox = null;

		ResetCamera();
		Resources.UnloadUnusedAssets();
		if (_battleLaunchTimer != null) {
			_battleLaunchTimer.OnAutoTimeFinish -= HandleOnAutoTimeFinish;
			_battleLaunchTimer.OnFinishedDelegate -= OnLaunchTimeFinishDelegate;
			_battleLaunchTimer.DestroyIt ();
		}
		//System.GC.Collect();
    }

	//托管
	public void OnAutoBattleButtonClicked()
	{
		if (IsPvPGameVideo())
		{
			return;
		}

		isAIManagement = !isAIManagement;
	}

	//点击战斗屏幕
	public void OnClickBattleScreen()
	{
		if (monsterList != null){
			foreach (MonsterController mc in monsterList)
			{
				mc.ShowMonsterName(true);
			}
		}

		monsterNameShow = true;

		CancelInvoke("HideAllMonsterName");
		Invoke("HideAllMonsterName", 10f);
	}

	public bool monsterNameShow = true;

	private void HideAllMonsterName()
	{
		monsterNameShow = false;

		if (monsterList != null){
			foreach (MonsterController mc in monsterList)
			{
				mc.ShowMonsterName(false);
			}
		}
	}

	public void HandlerSoldierReadyNotify(BattleSoldierReadyNotify notify)
	{
		MonsterController mc = GetMonsterFromSoldierID (notify.soldierId);
		if (mc != null)
		{
			mc.NeedReady = false;
		}
	}

	public void UpdateSoldiers(VideoSoldierUpdateNotify notify)
	{
		foreach (VideoSoldier soldier in notify.soldiers)
		{
			MonsterController mc = GetMonsterFromSoldierID(soldier.id);
			mc.currentHP = soldier.hp;
			mc.currentMP = soldier.mp;
			mc.currentSP = soldier.sp;
		}
	}

	public bool IsOldBattlePet(long petId)
	{
		return _oldBattlePetList.Contains (petId);
	}

	public bool IsBattlePet(long petId)
	{
		MonsterController mc = GetMyPet ();
		if (mc != null && mc.GetId() == petId)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int GetOldBattlePetCount()
	{
		return _oldBattlePetList.Count;;
	}

	public void AddItemUsedCount()
	{
		_itemUsedCount++;
	}
}
