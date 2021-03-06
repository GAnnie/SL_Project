﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PlayerModel.cs
// Author   : SK
// Created  : 2013/1/31
// Purpose  : 玩家数据模型
// **********************************************************************
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.whole.modules.system.data;
using com.nucleus.h1.logic.whole.modules.system.dto;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.battlebuff.data;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.equipment.model;
using com.nucleus.h1.logic.core.modules.formation.data;
using com.nucleus.h1.logic.core.modules.title.dto;
using com.nucleus.h1.logic.core.modules.title.data;
using com.nucleus.h1.logic.core.modules.guild.dto;

public class PlayerPropertyInfo{

	public CharactorDto playerDto;

	/** 敏捷 */
	public int speed;
	
	/** 攻击力 */
	public int attack;
	
	/** 防御力 */
	public int defense;
	
	/** hpMax */
	public int hpMax;

//	public int hp;

	/** mpMax */
	public int mpMax;

//	public int mp;

	/** 灵力 */
	public int magic;

	/** 装备资质属性 */
	public int[] EqAps;

	public PlayerPropertyInfo(CharactorDto dto){
		playerDto = dto;
		EqAps = new int[5];
	}

	public PlayerPropertyInfo(PlayerPropertyInfo playerInfo){
		this.playerDto = new CharactorDto();
		this.EqAps = new int[5];
		ResetPlayerInfo(playerInfo);
	}

	public void ResetPlayerInfo(PlayerPropertyInfo playerInfo){
		this.playerDto.level = playerInfo.playerDto.level;
		this.playerDto.aptitudeProperties = playerInfo.playerDto.aptitudeProperties;
		this.playerDto.potential = playerInfo.playerDto.potential;

		playerInfo.EqAps.CopyTo(this.EqAps,0);
		this.hpMax = playerInfo.hpMax;
		this.mpMax = playerInfo.mpMax;
		this.attack = playerInfo.attack;
		this.defense = playerInfo.defense;
		this.speed = playerInfo.speed;
		this.magic = playerInfo.magic;
	}

	public int[] ToApInfoArray(){
		int[] apInfoArray = new int[5]{
			this.playerDto.aptitudeProperties.constitution,
			this.playerDto.aptitudeProperties.intelligent,
			this.playerDto.aptitudeProperties.strength,
			this.playerDto.aptitudeProperties.stamina,
			this.playerDto.aptitudeProperties.dexterity
		};

		return apInfoArray;
	}
}

public class PlayerModel
{
	private static readonly PlayerModel instance = new PlayerModel ();
	public static PlayerModel Instance {
		get {
			return instance;
		}
	}

	private PlayerModel ()
	{
	}
	
	private PlayerDto _playerDto;
	private PlayerPropertyInfo _playerPropertyInfo;
	public event System.Action OnPlayerExpUpdate;
	public event System.Action OnPlayerGradeUpdate;
	public event System.Action OnPlayerPropertyUpdate;
	public event System.Action OnServerGradeChange;
	public event System.Action<WealthNotify> OnWealthChanged;
	public event System.Action<SubWealthNotify> OnSubWealthChanged;
	public event System.Action<string> OnPlayerNicknameUpdate;
	public event System.Action OnOpenDoublePointChanged;
	public event System.Action OnPlayerTitleUpdate;
	public event System.Action OnTitleListUpdate;
	public event System.Action OnPlayerVipUpdate;

	#region ReserveExp
	public int ReserveExp{
		get{
			return _playerDto.subWealth.reserveExp;
		}
	}

	private ReserveExpDto _reserveExpDto = null;
	public ReserveExpDto ReserveExpDto{
		get{
			return _reserveExpDto;
		}
		set{
			_reserveExpDto = value;
		}
	}
	#endregion ReserveExp

	#region Vigour
	public bool isEnoughVigour (int needVigour, bool tip = false)
	{
		if(_playerDto.subWealth.vigour >= needVigour){
			return true;
		}
		else{
			if (tip)
			{
				TipManager.AddTip("活力不足");
			}
			return false;
		}
	}

	public int Vigour
	{
		get
		{
			return _playerDto.subWealth.vigour;
		}
		set
		{
			_playerDto.subWealth.vigour = value;
		}
	}

	public int VigourMax
	{
		get
		{
			return this.GetPlayerLevel() * 20 + 50;
		}
	}

	#endregion Vigour

	#region ServerGrade
	private GameServerGradeDto _serverGradeDto;
	private long _nextServerGradeOpenTime;

	public void UpdateServerGradeDto(GameServerGradeDto serverGradeDto){
		if(serverGradeDto == null)
		{
			Debug.LogError("GameServerGradeDto is null");
			return;
		}

		_serverGradeDto = serverGradeDto;
		_nextServerGradeOpenTime = GetNextServerOpenTime(serverGradeDto);
	}

	public int ServerGrade 
	{
		get
		{
			return _serverGradeDto.serverGrade;
		}
	}

	public long NextServerGradeOpenTime{
		get{
			return _nextServerGradeOpenTime;
		}
	}

	//返回-1代表已到达服务器最高等级上限，否则返回下次服务器等级开放时间
	private long GetNextServerOpenTime(GameServerGradeDto serverGradeDto){
		List<GameServerGrade> _serverGradeInfoList = DataCache.getArrayByCls<GameServerGrade>();
		int nextGradeIndex = -1;
		for(int i=0;i<_serverGradeInfoList.Count;++i){
			if(_serverGradeInfoList[i].grade == serverGradeDto.serverGrade)
			{
				nextGradeIndex = i+1;
				break;
			}
		}

		if(nextGradeIndex != -1 && nextGradeIndex < _serverGradeInfoList.Count){
			long daySpan = _serverGradeInfoList[nextGradeIndex].id - _serverGradeInfoList[nextGradeIndex-1].id;
			return serverGradeDto.openTime + daySpan * 86400000L;
		}else
			return -1L;
	}
	#endregion

	#region VIP
	public bool IsVip()
	{
		return _playerDto.vipExpiredTime > _playerDto.gameServerTime;
	}

	public void BuyVip(int vipId){
		Vip vip = DataCache.getDtoByCls<Vip>(vipId);
		ServiceRequestAction.requestServer(PlayerService.buyVip(vipId),"BuyVip",(e)=>{
			int day = (int)(vip.time / 86400000);
			TipManager.AddTip(string.Format("购买成功，VIP天数增加{0}天", day));
			_playerDto.vipExpiredTime = _playerDto.gameServerTime + vip.time;

			if(OnPlayerVipUpdate != null)
				OnPlayerVipUpdate();
		});
	}
	#endregion
	
	public void Setup (object data)
	{
		_playerDto = data as PlayerDto;

		SetupColorScheme();

		CheckOutSatiationState();
		TeamModel.Instance.SetupTeamInfo(_playerDto.curTeamDto);
		SystemTimeManager.Instance.OnChangeNextDay += OnChangeNextDay;
	}

	public void OnChangeNextDay(){
		ResetAddPointPlanChangeTimes();
	}

	public void UpdateInfoWithAfterLoginDto(AfterLoginDto afterLoginDto){

		_playerDto.simpleMainCharactorDto.hp = afterLoginDto.shortBattlePropertyDto.hp;
		_playerDto.simpleMainCharactorDto.mp = afterLoginDto.shortBattlePropertyDto.mp;

		/** 本次储备经验，可为null表示没有 */
		_reserveExpDto = afterLoginDto.reserveExpDto;
		
		/** 总储备经验 */
		_playerDto.subWealth.reserveExp = afterLoginDto.reserverExp;
		
		/** 服务器等级信息 */
		UpdateServerGradeDto(afterLoginDto.gameServerGradeDto);
		
		/** 玩家阵法信息 */
		SetupFormationInfo(afterLoginDto);

		/** 玩家称谓信息 */
		_titleDtoList = afterLoginDto.titleDtoList;

        _guildInfo = afterLoginDto.guildInfo;
	}

	public void Dispose ()
	{
		_playerDto = null;
		_playerPropertyInfo = null;
	}

	#region 玩家基础信息
	public PlayerDto GetPlayer ()
	{
		return _playerDto;
	}

	public SimpleCharactorDto GetSimplePlayerInfo(){
		return _playerDto.simpleMainCharactorDto;
	}
	
	public string GetPlayerName ()
	{
		return _playerDto.nickname;
	}

	public void UpdatePlayerName(string nickName)
	{
		_playerDto.nickname = nickName;
		_playerPropertyInfo.playerDto.name = nickName;
		if (OnPlayerNicknameUpdate != null)
		{
			OnPlayerNicknameUpdate(nickName);
		}
	}

	public int GetPlayerGender ()
	{
		return _playerDto.gender;
	}
	
	public int GetPlayerLevel ()
	{
		return _playerDto.simpleMainCharactorDto.level;
	}
	
	public long GetPlayerId ()
	{
		return _playerDto.id;
	}
	
	public long GetPlayerExp(){
		return _playerDto.simpleMainCharactorDto.exp;
	}
	#endregion

	#region 玩家财富
	//是否有足够元宝
	public bool isEnoughIngot (int needIngot, bool tip = false)
	{
		if (_playerDto.wealth.ingot >= needIngot) {
			return true;
		}
		else{
			if (tip)
			{
				ProxyPayModule.OpenPay();
			}
			return false;
		}
	}
	
	public bool isEnoughCopper(long needCopper, bool tip = false)
	{
		if(_playerDto.wealth.copper >= needCopper){
			return true;
		}
		else{
			if (tip)
			{
				ProxyPayModule.OpenCopper();
			}
			return false;
		}
	}

	public bool isEnoughSilver(int needSilver, bool tip = false)
	{
		if(_playerDto.wealth.silver >= needSilver){
			return true;
		}
		else{
			if (tip)
			{
				ProxyPayModule.OpenSilver();
			}
			return false;
		}
	}

	public bool isEnoughScore(int needScore, bool tip = false)
	{
		if(_playerDto.wealth.score >= needScore){
			return true;
		}
		else{
			if (tip) TipManager.AddTip(string.Format("你的{0}不足", ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_SCORE)));
			return false;
		}
	}

	public WealthNotify GetWealth()
	{
		return _playerDto.wealth;
	}

	public SubWealthNotify GetSubWealth()
	{
		return _playerDto.subWealth;
	}

	public void UpdateSubWealth(SubWealthNotify newSubWealth){
		SubWealthNotify oldNotify = _playerDto.subWealth;
		
		int changeVigour = newSubWealth.vigour - oldNotify.vigour;//活力
		int changeReserveExp = newSubWealth.reserveExp - oldNotify.reserveExp;//储备经验
		long changeNimbus = newSubWealth.nimbus - oldNotify.nimbus;//灵气
		int changeSatiation = newSubWealth.satiation - oldNotify.satiation; //饱食度

		if(newSubWealth.traceType != null && newSubWealth.traceType.tip) 
		{
			if (changeVigour < 0) {
				TipManager.AddLostCurrencyTip(changeVigour,"活力");
			} else if(changeVigour > 0) {
				TipManager.AddGainCurrencyTip(changeVigour,"活力");
			}
			
			if (changeReserveExp < 0) {
				TipManager.AddLostCurrencyTip(changeReserveExp, "储备经验");
			} else if(changeReserveExp > 0) {
				TipManager.AddGainCurrencyTip(changeReserveExp, "储备经验");
			}
			
			if (changeNimbus < 0) {
				TipManager.AddLostCurrencyTip(changeNimbus, "灵气");
			} else if(changeNimbus > 0) {
				TipManager.AddGainCurrencyTip(changeNimbus, "灵气");
			}

			if (changeSatiation < 0) {
				TipManager.AddLostCurrencyTip(changeSatiation, "饱食度");
			} else if(changeSatiation > 0) {
				TipManager.AddGainCurrencyTip(changeSatiation, "饱食度");
			}
		}
		
		_playerDto.subWealth = newSubWealth;
		if (OnSubWealthChanged != null)
		{
			OnSubWealthChanged(newSubWealth);
		}
		CheckOutSatiationState();
	}

	public bool CheckOutSatiationState(){
		if(_playerDto.subWealth.satiation > 0){
			PlayerBuffModel.Instance.ToggleSatiationBuffTip(true);
			return true;
		}
		PlayerBuffModel.Instance.ToggleSatiationBuffTip(false);
		return false;
	}

	//获取补充饱食度消耗铜币数量
	public long GetReplenishSatiationFee(){
		int serverGradeFactor = (_serverGradeDto.serverGrade/10)*10;
		int count = MaxSatiationVal - _playerDto.subWealth.satiation;
		return (long)((serverGradeFactor+10)*DataHelper.GetStaticConfigValuef(H1StaticConfigs.SATIATION_FILL_FACTOR,0.5f)*CurrencyExchange.IngotToCopper(1)/serverGradeFactor*count);
	}

	public int MaxSatiationVal{
		get{
			H1VirtualItem info = DataCache.getDtoByCls<GeneralItem>(H1VirtualItem.VirtualItemEnum_SATIATION) as H1VirtualItem;
			return info == null?0:(int)info.carryLimit;
		}
	}

	public bool isFullSatiation(){
		if(_playerDto.subWealth.satiation >= MaxSatiationVal){
			return true;
		}
		return false;
	}

	public void UpdateWealth (WealthNotify newWealth)
	{
		WealthNotify oldNotify = _playerDto.wealth;
		
		int changeGold = newWealth.ingot - oldNotify.ingot;//元宝
		int changeSilver = newWealth.silver - oldNotify.silver;//银币
		long changeCopper = newWealth.copper - oldNotify.copper;//铜币
		int changeScore = newWealth.score - oldNotify.score;//积分
		int changeContribute = newWealth.contribute - oldNotify.contribute;//帮贡
		
		if(newWealth.traceType != null && newWealth.traceType.tip) 
		{
			if (changeGold < 0) {
				TipManager.AddLostCurrencyTip(changeGold, ItemIconConst.Ingot);
			} else if(changeGold > 0) {
				TipManager.AddGainCurrencyTip(changeGold, ItemIconConst.Ingot);
			}
			
			if (changeSilver < 0) {
				TipManager.AddLostCurrencyTip(changeSilver, ItemIconConst.Silver);
			} else if(changeSilver > 0) {
				TipManager.AddGainCurrencyTip(changeSilver, ItemIconConst.Silver);
			}
			
			if (changeCopper < 0) {
				TipManager.AddLostCurrencyTip(changeCopper, ItemIconConst.Copper);
			} else if(changeCopper > 0) {
				TipManager.AddGainCurrencyTip(changeCopper, ItemIconConst.Copper);
			}
			
			if (changeScore < 0) {
				TipManager.AddGainCurrencyTip(changeScore, ItemIconConst.Score);
			} else if(changeScore > 0) {
				TipManager.AddGainCurrencyTip(changeScore, ItemIconConst.Score);
			}

			if (changeContribute < 0) {
				TipManager.AddGainCurrencyTip(changeContribute, ItemIconConst.Contribute);
			} else if(changeContribute > 0) {
				TipManager.AddGainCurrencyTip(changeContribute, ItemIconConst.Contribute);
			}
		}

		_playerDto.wealth = newWealth;
		if (OnWealthChanged != null)
		{
			OnWealthChanged(newWealth);
		}	
	}

	public void UseIngot(int cost){
		if(cost > 0){
			_playerDto.wealth.ingot -= cost;
			if(_playerDto.wealth.ingot < 0)
				_playerDto.wealth.ingot = 0;

			TipManager.AddLostCurrencyTip(cost, ItemIconConst.Ingot);

			if (OnWealthChanged != null)
			{
				OnWealthChanged(_playerDto.wealth);
			}
		}
	}

	public void UseCopper(long cost){
		if(cost > 0L){
			_playerDto.wealth.copper -= cost;
			if(_playerDto.wealth.copper < 0)
				_playerDto.wealth.copper = 0;

			TipManager.AddLostCurrencyTip(cost, ItemIconConst.Copper);

			if (OnWealthChanged != null)
			{
				OnWealthChanged(_playerDto.wealth);
			}
		}
	}
	#endregion

	#region 玩家阵型数据
	/** 已掌握阵法编号 */
	private List<int> _acquiredFormationIds;
	
	/** 阵型方案列表 (0为防御方案,之后为攻击方案) */
	private List<int> _formationCaseIds;
	
	/** 当前阵型方案序号 */
	public int ActiveFormationCaseIndex;
	public int CurrentSelectFormationCaseIndex;
	public const int DEFENSE_FORMATION_INDEX = 0;

	public void SetupFormationInfo(AfterLoginDto afterLoginDto){
		_acquiredFormationIds = afterLoginDto.acquiredFormationIds;
		_formationCaseIds = afterLoginDto.formationCaseIds;
		ActiveFormationCaseIndex = afterLoginDto.activeFormationCaseNum;
		CurrentSelectFormationCaseIndex = afterLoginDto.activeFormationCaseNum;
	}

	public List<int> GetAcquiredFormationIdList(){
		return _acquiredFormationIds;
	}

	public List<int> GetFormationCaseList(){
		return _formationCaseIds;
	}

	public int GetFormationCaseId(int caseIndex){
		if(caseIndex < _formationCaseIds.Count)
			return _formationCaseIds[caseIndex];

		return -1;
	}

	public void SetFormationCaseId(int caseIndex,int newFormationId){
		if(caseIndex < _formationCaseIds.Count){
			_formationCaseIds[caseIndex] = newFormationId;
		}
	}

	public void AddNewAcquiredFormation(int formationId){
		_acquiredFormationIds.Add(formationId);
	}

	public void LearnNewFormation(PackItemDto itemDto,Formation newFormation,bool free){
		ServiceRequestAction.requestServer(PlayerService.learnFormation(itemDto.index,free),"LearnFormation",(e)=>{
			LearnFormationDto learnFormatinDto = e as LearnFormationDto;
			_formationCaseIds = learnFormatinDto.formationCaseIds;

			int defaultCapacity = DataHelper.GetStaticConfigValue(H1StaticConfigs.FORMATION_DEFAULT_CAPACITY,4);
			if(_acquiredFormationIds.Count < defaultCapacity){
				TipManager.AddTip(string.Format("你学会了{0}",newFormation.name.WrapColor(ColorConstant.Color_Tip_LostCurrency_Str)));
			}else{
				if(free){
					for(int i=0;i<_acquiredFormationIds.Count;++i){
						int originFormationId = _acquiredFormationIds[i];
						if(!learnFormatinDto.formationIds.Contains(originFormationId))
						{
							Formation replaceFormation = DataCache.getDtoByCls<Formation>(originFormationId);
							TipManager.AddTip(string.Format("你学会了{0}，但遗忘了{1}",
							                                newFormation.name.WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
							                                replaceFormation.name.WrapColor(ColorConstant.Color_Tip_LostCurrency_Str)));
							break;
						}
					}
				}else{
					int ingotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.FORMATION_OVER_DEFAULT_CAPACITY,200);
					TipManager.AddTip(string.Format("消耗了{0}{1}，你学会了{2}",ingotCount.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
					                                ItemIconConst.Ingot,
					                                newFormation.name.WrapColor(ColorConstant.Color_Tip_LostCurrency_Str)));
				}
			}

			_acquiredFormationIds = learnFormatinDto.formationIds;
		});
	}
	#endregion

	#region 玩家属性信息
	//获取玩家信息数据
	public void RequestCharacterDto (System.Action onSuccess)
	{
		if(_playerPropertyInfo != null)
		{
			if(onSuccess !=null)
				onSuccess();
		}
		else{
			ServiceRequestAction.requestServer (PlayerService.charactorInfo (), "获取人物属性", (e) => {
				_playerPropertyInfo = new PlayerPropertyInfo(e as CharactorDto);
				CalculatePlayerBp();
				if (onSuccess != null)
					onSuccess ();
			});
		}
	}

	public void ResetPlayerPerAp(int itemIndex,int apType,int resetPoint,bool useIngot,System.Action onSuccess=null){
		ServiceRequestAction.requestServer(PlayerService.resetPerAptitude(itemIndex,apType,useIngot),"ResetPlayerAp",(e)=>{
			if(apType == AptitudeProperties.AptitudeType_Constitution)
				_playerPropertyInfo.playerDto.aptitudeProperties.constitution -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Intelligent)
				_playerPropertyInfo.playerDto.aptitudeProperties.intelligent -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Strength)
				_playerPropertyInfo.playerDto.aptitudeProperties.strength -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Stamina)
				_playerPropertyInfo.playerDto.aptitudeProperties.stamina -= resetPoint;
			else if(apType == AptitudeProperties.AptitudeType_Dexterity)
				_playerPropertyInfo.playerDto.aptitudeProperties.dexterity -= resetPoint;
			
			_playerPropertyInfo.playerDto.potential +=resetPoint;
			TipManager.AddTip(string.Format("你的{0}属性减掉了{1}点，潜力点增加{2}点",
			                                ItemHelper.AptitudeTypeName(apType).WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
			                                resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str),
			                                resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency_Str)));
			CalculatePlayerBp();
			if(onSuccess != null)
				onSuccess();
		});
	}

	public void UpdatePlayerExpInfo (CharactorExpInfoNotify expNotify, bool needTip = true)
	{
		if (expNotify == null)
			return;

		if(!expNotify.maxLevelReached && expNotify.simpleCharactorDto != null){
			if (needTip)
			{
				string tip = string.Format("你获得{0}{1}",expNotify.expGain.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_PLAYER_EXP));

				if (expNotify.copperGain > 0)
				{
					tip += "、" + string.Format("{0}{1}", expNotify.copperGain.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency), ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_COPPER));
				}
				TipManager.AddTip(tip);
			}

			//同步更新simpleMainCharactorDto的exp与level
			_playerDto.simpleMainCharactorDto.exp = expNotify.simpleCharactorDto.exp;
			_playerDto.simpleMainCharactorDto.level = expNotify.simpleCharactorDto.level;

			if(_playerPropertyInfo != null){
				_playerPropertyInfo.playerDto.exp = expNotify.simpleCharactorDto.exp;
				_playerPropertyInfo.playerDto.level = expNotify.simpleCharactorDto.level;
			}

			if (expNotify.upgarded) {
				//同步更新simpleMainCharactorDto的HP与MP值
				_playerDto.simpleMainCharactorDto.hp = expNotify.simpleCharactorDto.hp;
				_playerDto.simpleMainCharactorDto.mp = expNotify.simpleCharactorDto.mp;

				if(_playerPropertyInfo != null){
					int lvCount = expNotify.simpleCharactorDto.level - expNotify.oldLevel;

					for (int levelIndex=0; levelIndex<lvCount; ++levelIndex) {
						//每级默认各资质属性增加1点
						AddApPoint (_playerPropertyInfo.playerDto.aptitudeProperties, 1, 1, 1, 1, 1);

						//小于40级且未重置过资质属性，潜力点按默认配点分配
						if (_playerPropertyInfo.playerDto.level < DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POTENTIAL_MIN_LEVEL, 40) 
						    && !PlayerModel.Instance.HasCustomAptitude ()){
							AptitudeProperties defaultApDistrubute = _playerPropertyInfo.playerDto.faction.defaultAptitudeDistrubute;
							AddApPoint (_playerPropertyInfo.playerDto.aptitudeProperties,
							                    defaultApDistrubute.constitution,
							                    defaultApDistrubute.intelligent,
							                    defaultApDistrubute.strength,
							                    defaultApDistrubute.stamina,
							                    defaultApDistrubute.dexterity);
							
						} else {
							//40级后不自动分配潜力点
							_playerPropertyInfo.playerDto.potential += DataHelper.GetStaticConfigValue (H1StaticConfigs.DISPOSABLE_POTENTIAL_POINT_GAIN_PER_UPGRADE, 5);

//							AutoDistrubuteApPoint (_playerPropertyInfo.playerDto);
						}
					}
					//重新计算伙伴属性值
					CrewModel.Instance.CalculateAllCrewPropertyInfo();
					CalculatePlayerBp ();
				}

				if(OnPlayerGradeUpdate != null)
					OnPlayerGradeUpdate();
			}

			if(OnPlayerExpUpdate != null)
				OnPlayerExpUpdate();
		}

		if (expNotify.maxLevelReached && expNotify.copperGain > 0L)
		{
			TipManager.AddGainCurrencyTip(expNotify.copperGain, ItemIconConst.Copper);
		}

		if(expNotify.copperGain > 0L){
			_playerDto.wealth.copper += expNotify.copperGain;
		}
	}

	public PlayerPropertyInfo GetPlayerPropertyInfo ()
	{
		return _playerPropertyInfo;
	}
	
	public bool HasCustomAptitude ()
	{
		return _playerPropertyInfo.playerDto.hasCustomAptitude;
    }

	//重置主角所有资质属性
	public void RestPlayerApPoint ()
	{
		int oldTotalPoint = _playerPropertyInfo.playerDto.aptitudeProperties.constitution
						+_playerPropertyInfo.playerDto.aptitudeProperties.intelligent
						+_playerPropertyInfo.playerDto.aptitudeProperties.strength
						+_playerPropertyInfo.playerDto.aptitudeProperties.stamina
						+_playerPropertyInfo.playerDto.aptitudeProperties.dexterity
						+_playerPropertyInfo.playerDto.potential;

		int originPoint = _playerPropertyInfo.playerDto.level + DataHelper.GetStaticConfigValue (H1StaticConfigs.MIN_APTITUDE_RESET_POINT, 10);
		_playerPropertyInfo.playerDto.aptitudeProperties.constitution = originPoint;
		_playerPropertyInfo.playerDto.aptitudeProperties.intelligent = originPoint;
		_playerPropertyInfo.playerDto.aptitudeProperties.strength = originPoint;
		_playerPropertyInfo.playerDto.aptitudeProperties.stamina = originPoint;
		_playerPropertyInfo.playerDto.aptitudeProperties.dexterity = originPoint;

		_playerPropertyInfo.playerDto.potential = oldTotalPoint-5*originPoint;
		_playerPropertyInfo.playerDto.hasCustomAptitude = true;
		CalculatePlayerBp ();
	}

	public void UpdatePlayerAp (int potential, int constitution, int intelligent, int strength, int stamina, int dexterity)
	{
		_playerPropertyInfo.playerDto.potential = potential;
		_playerPropertyInfo.playerDto.aptitudeProperties.constitution = constitution;
		_playerPropertyInfo.playerDto.aptitudeProperties.intelligent = intelligent;
		_playerPropertyInfo.playerDto.aptitudeProperties.strength = strength;
		_playerPropertyInfo.playerDto.aptitudeProperties.stamina = stamina;
		_playerPropertyInfo.playerDto.aptitudeProperties.dexterity = dexterity;

		CalculatePlayerBp();
	}

	//结果属性计算公式：结果属性=资质属性+装备属性+宝石属性+附魔+门派技能属性
	public void CalculatePlayerBp (PlayerPropertyInfo playerPropertyInfo=null)
	{
		//playerPropertyInfo为空时，用于重新计算玩家自身属性
		if(playerPropertyInfo == null)
			playerPropertyInfo = _playerPropertyInfo;

		if(playerPropertyInfo == null)
			return;

		CharactorDto playerDto = playerPropertyInfo.playerDto;
		float hp = 0f,mp = 0f,attack = 0f, defense = 0f, speed = 0f, magic = 0f;
		hp +=DataHelper.GetStaticConfigValuef(H1StaticConfigs.MAIN_CHARACTOR_INIT_HP,200f);
		attack +=DataHelper.GetStaticConfigValuef(H1StaticConfigs.MAIN_CHARACTOR_INIT_ATTACK,40f);
		//人物基础属性
		List<AptitudePropertyInfo> apDataInfoList = DataCache.getArrayByCls<AptitudePropertyInfo> ();
		//确保playerApList加入的资质属性值次序跟apDataInfo静态表次序一致
		int[] playerAps = new int[5];
		//装备属性
		Dictionary<long,PackItemDto> eqDic = BackpackModel.Instance.GetBodyEquip();
		if(eqDic != null && eqDic.Count > 0){
			foreach(PackItemDto itemDto in eqDic.Values){
				Equipment equipmentInfo = itemDto.item as Equipment;
				EquipmentExtraDto eqExtraDto = itemDto.extra as EquipmentExtraDto;

				// 耐久为0不计算属性
				if(eqExtraDto.duration <= 0)
					continue;

				//装备资质属性
				for(int index = 0;index < eqExtraDto.aptitudeProperties.Count;index++)
				{
					if(eqExtraDto.aptitudeProperties[index].aptitudeType != AptitudeProperties.MP_TYPE)
					{
						int i = eqExtraDto.aptitudeProperties[index].aptitudeType - 1;
						if(i < playerAps.Length)
						{
							playerAps[i] = playerAps[i] + eqExtraDto.aptitudeProperties[index].value;
						}
					}
					else
					{
						mp += eqExtraDto.aptitudeProperties[index].value;
					}
				}
				//装备战斗属性
				hp += eqExtraDto.battleBaseProperties.hp;
				mp += eqExtraDto.battleBaseProperties.mp;
				attack += eqExtraDto.battleBaseProperties.attack;
				defense += eqExtraDto.battleBaseProperties.defense;
				speed += eqExtraDto.battleBaseProperties.speed;
				magic += eqExtraDto.battleBaseProperties.magic;
				//装备镶嵌战斗属性
				float[] eqEmbedBpVals = CalculateEqEmbedBp(eqExtraDto,equipmentInfo.equipLevel);
				if(eqEmbedBpVals != null){
					hp += eqEmbedBpVals[0];
					mp += eqEmbedBpVals[1];
					attack += eqEmbedBpVals[2];
					defense += eqEmbedBpVals[3];
					speed += eqEmbedBpVals[4];
					magic += eqEmbedBpVals[5];
				}
                //装备buff属性
                float[] eqBuffBpVals = CalculateEqBuffBp(eqExtraDto);
                if (eqBuffBpVals != null)
                {
                    hp += eqBuffBpVals[0];
                    mp += eqBuffBpVals[1];
                    attack += eqBuffBpVals[2];
                    defense += eqBuffBpVals[3];
                    speed += eqBuffBpVals[4];
                    magic += eqBuffBpVals[5];
                }
			}
		}
		//更新人物装备资质属性数值
		playerAps.CopyTo(playerPropertyInfo.EqAps,0);

		//人物自身资质
		playerAps[0] += playerDto.aptitudeProperties.constitution;
		playerAps[1] += playerDto.aptitudeProperties.intelligent;
		playerAps[2] += playerDto.aptitudeProperties.strength;
		playerAps[3] += playerDto.aptitudeProperties.stamina;
		playerAps[4] += playerDto.aptitudeProperties.dexterity;

		for (int i=0; i<playerAps.Length; ++i) {
			AptitudePropertyInfo ApInfo = apDataInfoList [i];
			int ApVal = playerAps [i];
			hp += ApVal * ApInfo.hp;
			attack += ApVal * ApInfo.attack;
			defense += ApVal * ApInfo.defense;
			speed += ApVal * ApInfo.speed;
			magic += ApVal * ApInfo.magic;
		}

		//计算门派技能战斗属性加成
		List<FactionSkillDto> factionSkills = FactionSkillModel.Instance.GetFactionSkillDtos();
		for(int i=0;i<factionSkills.Count;++i){
			if(factionSkills[i].propertyValues != null && factionSkills[i].propertyValues.Count > 0){
				foreach(SingleBattleBaseProperty bp in factionSkills[i].propertyValues){
					if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Hp)
						hp += bp.value;
					else if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Mp)
						mp += bp.value;
					else if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Attack)
						attack += bp.value;
					else if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Defense)
						defense += bp.value;
					else if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Speed)
						speed += bp.value;
					else if(bp.battleBasePropertyType == BattleBuff.BattleBasePropertyType_Magic)
						magic += bp.value;
				}
			}
		}
		
		playerPropertyInfo.hpMax = Mathf.FloorToInt (hp);
		playerPropertyInfo.mpMax = Mathf.FloorToInt(playerDto.level * 20f + 30f +mp);
		playerPropertyInfo.attack = Mathf.FloorToInt (attack);
		playerPropertyInfo.defense = Mathf.FloorToInt (defense);
		playerPropertyInfo.speed = Mathf.FloorToInt (speed);
		playerPropertyInfo.magic = Mathf.FloorToInt (magic);

		if(playerPropertyInfo == _playerPropertyInfo){
			if(OnPlayerPropertyUpdate != null)
				OnPlayerPropertyUpdate();
		}
	}

	private float[] CalculateEqEmbedBp(EquipmentExtraDto eqExtraDto,int eqLevel){
		if(eqExtraDto.equipmentEmbedInfo == null 
		   || eqExtraDto.equipmentEmbedInfo.embedItemIds == null 
		   || eqExtraDto.equipmentEmbedInfo.embedItemIds.Count<=0)
			return null;

		//hp,mp,atk,def,speed,magic
		float[] bpVals = new float[6];
		for(int i=0;i<eqExtraDto.equipmentEmbedInfo.embedItemIds.Count;++i){
			if(i < eqExtraDto.equipmentEmbedInfo.embedLevels.Count){
				int gemId = eqExtraDto.equipmentEmbedInfo.embedItemIds[i];
				int gemLv = eqExtraDto.equipmentEmbedInfo.embedLevels[i];
				Props gem = DataCache.getDtoByCls<GeneralItem>(gemId) as Props;

				PropsParam_3 param = gem.propsParam as PropsParam_3;
				int gemValue = 0;
				if(param.battleBasePropertyValues[0] != AptitudeProperties.USE_ILV)
					gemValue = param.battleBasePropertyValues[0]*gemLv;
				else
					gemValue = eqLevel*gemLv;

				if(gemValue > 0){
					int bpType = param.battleBasePropertyTypes[0];
					if(bpType == BattleBuff.BattleBasePropertyType_Hp){
						bpVals[0] += gemValue;
					}else if(bpType == BattleBuff.BattleBasePropertyType_Mp){
						bpVals[1] += gemValue;
					}else if(bpType == BattleBuff.BattleBasePropertyType_Attack){
						bpVals[2] += gemValue;
					}else if(bpType == BattleBuff.BattleBasePropertyType_Defense){
						bpVals[3] += gemValue;
					}else if(bpType == BattleBuff.BattleBasePropertyType_Speed){
						bpVals[4] += gemValue;
					}else if(bpType == BattleBuff.BattleBasePropertyType_Magic){
						bpVals[5] += gemValue;
					}
				}
			}
		}

		return bpVals;
	}

    private float[] CalculateEqBuffBp(EquipmentExtraDto eqExtraDto)
    {
        if (eqExtraDto.propertiesBuffList == null
           || eqExtraDto.propertiesBuffList.Count <= 0)
            return null;
        //hp,mp,atk,def,speed,magic
        float[] bpVals = new float[6];
        for (int index = 0; index < eqExtraDto.propertiesBuffList.Count;index++)
        {
            if(eqExtraDto.propertiesBuffList[index].remainSeconds > 0)
            {
                SingleBattleBaseProperty property = eqExtraDto.propertiesBuffList[index].property;
                if (property.value > 0)
                {
                    int bpType = property.battleBasePropertyType;
                    if (bpType == BattleBuff.BattleBasePropertyType_Hp)
                    {
                        bpVals[0] += property.value;
                    }
                    else if (bpType == BattleBuff.BattleBasePropertyType_Mp)
                    {
                        bpVals[1] += property.value;
                    }
                    else if (bpType == BattleBuff.BattleBasePropertyType_Attack)
                    {
                        bpVals[2] += property.value;
                    }
                    else if (bpType == BattleBuff.BattleBasePropertyType_Defense)
                    {
                        bpVals[3] += property.value;
                    }
                    else if (bpType == BattleBuff.BattleBasePropertyType_Speed)
                    {
                        bpVals[4] += property.value;
                    }
                    else if (bpType == BattleBuff.BattleBasePropertyType_Magic)
                    {
                        bpVals[5] += property.value;
                    }
                }
            }
        }
        return bpVals;
    }
	#endregion

	#region 玩家加点方案
	public int GetAddPointPlanOpenCount(){
		int openCount = 1;
		int playerLv = _playerDto.simpleMainCharactorDto.level;
		if(playerLv >= DataHelper.GetStaticConfigValue(H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POINT_PLAN_TWO_LEVEL,40) 
		   && playerLv < DataHelper.GetStaticConfigValue(H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POINT_PLAN_THREE_LEVEL,90))
			openCount = 2;
		else if(playerLv >= DataHelper.GetStaticConfigValue(H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POINT_PLAN_THREE_LEVEL,90))
			openCount = 3;

		return openCount;
	}

	public int GetAddPointPlanOpenLevel(int openCount){
		if(openCount == 2)
			return DataHelper.GetStaticConfigValue(H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POINT_PLAN_TWO_LEVEL,40);
		else if(openCount == 3)
			return DataHelper.GetStaticConfigValue(H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POINT_PLAN_THREE_LEVEL,90);

		return 0;
	}

	public int GetActivedAddPointPlanId(){
		return _playerPropertyInfo.playerDto.pointPlan;
	}

	public void UpdateActivedAddPointPlanId(int newPlanId,PlayerPropertyInfo newPropertyInfo){
		_playerPropertyInfo.playerDto.pointPlan = newPlanId;
		_playerPropertyInfo.playerDto.changeTimes += 1;

		_playerPropertyInfo.ResetPlayerInfo(newPropertyInfo);
		if(OnPlayerPropertyUpdate != null)
			OnPlayerPropertyUpdate();
	}

	public void ResetAddPointPlanChangeTimes(){
		if(_playerPropertyInfo != null)
			_playerPropertyInfo.playerDto.changeTimes = 0;
	}
	#endregion

	#region 自动分配潜力点
	//根据玩家加点配置分配剩余潜力点，同样适用于宠物
	//单一配点方案：将所有潜力点加到相应的资质点上
	//多项配点方案：仅当潜力点为10的倍数时，分配潜力点，不足10点的由玩家自主分配
	public static void AutoDistrubuteApPoint (CharactorDto characterDto)
	{
		if (!isEmptyApDistribution (characterDto)) {
			if (IsSingleApDistribution (characterDto)) {
				int presetMaxValue = DataHelper.GetStaticConfigValue (H1StaticConfigs.PRESET_ONUPGRADE_POTENTIAL_TOTAL_POINT, 10);
				AddApPoint (characterDto.aptitudeProperties,
				            characterDto.onUpgradeAptitudeProperties.constitution / presetMaxValue * characterDto.potential,
				            characterDto.onUpgradeAptitudeProperties.intelligent / presetMaxValue * characterDto.potential,
				            characterDto.onUpgradeAptitudeProperties.strength / presetMaxValue * characterDto.potential,
				            characterDto.onUpgradeAptitudeProperties.stamina / presetMaxValue * characterDto.potential,
				            characterDto.onUpgradeAptitudeProperties.dexterity / presetMaxValue * characterDto.potential);
				characterDto.potential = 0;
			} else {
				int count = characterDto.potential / 10;
				for (int i=0; i<count; ++i) {
					AddApPoint (characterDto.aptitudeProperties,
					            characterDto.onUpgradeAptitudeProperties.constitution,
					            characterDto.onUpgradeAptitudeProperties.intelligent,
					            characterDto.onUpgradeAptitudeProperties.strength,
					            characterDto.onUpgradeAptitudeProperties.stamina,
					            characterDto.onUpgradeAptitudeProperties.dexterity);
					characterDto.potential -= 10;
				}
			}
		}
	}
	
	//验证是否设置了自动加点方案
	public static bool isEmptyApDistribution (CharactorDto characterDto)
	{
		if (characterDto.onUpgradeAptitudeProperties.constitution == 0 &&
		    characterDto.onUpgradeAptitudeProperties.intelligent == 0 &&
		    characterDto.onUpgradeAptitudeProperties.strength == 0 &&
		    characterDto.onUpgradeAptitudeProperties.stamina == 0 &&
		    characterDto.onUpgradeAptitudeProperties.dexterity == 0) {
			return true;
		} else
			return false;
	}
	
	//验证是否设置了单一配点方案
	public static bool IsSingleApDistribution (CharactorDto characterDto)
	{
		if (characterDto.onUpgradeAptitudeProperties.constitution == 10 ||
		    characterDto.onUpgradeAptitudeProperties.intelligent == 10 ||
		    characterDto.onUpgradeAptitudeProperties.strength == 10 ||
		    characterDto.onUpgradeAptitudeProperties.stamina == 10 ||
		    characterDto.onUpgradeAptitudeProperties.dexterity == 10) {
			return true;
		} else
			return false;
	}
	
	public static void AddApPoint (AptitudeProperties apDto, int constitution, int intelligent, int strength, int stamina, int dexterity)
	{
		apDto.constitution += constitution;
		apDto.intelligent += intelligent;
		apDto.strength += strength;
		apDto.stamina += stamina;
		apDto.dexterity += dexterity;
	}
	#endregion

	#region 挂机
	private DoubleExpStateBarDto _doubleExpStateDto;
	public void SetupDoubleExpDto(DoubleExpStateBarDto dto){
		_doubleExpStateDto = dto;
		CheckOutDoubleExp();
	}

	public DoubleExpStateBarDto GetDoubleExpDto(){
		return _doubleExpStateDto;
	}

	public void UpdateDoubleExpDto(DoubleExpDto expDto){
		_doubleExpStateDto.openPoint = expDto.openPoint;
		_doubleExpStateDto.point = expDto.point;

		if(OnOpenDoublePointChanged != null)
			OnOpenDoublePointChanged(); 

		CheckOutDoubleExp();
	}

	public bool CheckOutDoubleExp(){
		if(_doubleExpStateDto != null && _doubleExpStateDto.openPoint > 0){
			PlayerBuffModel.Instance.ToggleDoubleExpBuffTip(true);
			return true;
		}
		PlayerBuffModel.Instance.ToggleDoubleExpBuffTip(false);
		return false;
	}

	public void FreezeDoubleExp(){
		if(_doubleExpStateDto != null && _doubleExpStateDto.openPoint == 0)
		{
			TipManager.AddTip("当前双倍点数为0");
			return;
		}
		
		ServiceRequestAction.requestServer(PatrolService.freezeDoubleExp(),"FreezeDoubleExp", 
        (e) => {
			DoubleExpDto expDto = e as DoubleExpDto;
            TipManager.AddTip("当前双倍点数已结算到本周剩余双倍点数");
			UpdateDoubleExpDto(expDto);
		});
	}

	public void ReceiveDoubleExp(){
		if(_doubleExpStateDto != null)
		{
			if(_doubleExpStateDto.openPoint >= DataHelper.GetStaticConfigValue(H1StaticConfigs.OPEN_MAX_DOUBLE_EXP_POINT,120)){
				TipManager.AddTip(string.Format("最多领取{0}点双倍点数",_doubleExpStateDto.openPoint));
				return;
			}
			else if(_doubleExpStateDto.point == 0){
              	TipManager.AddTip("本周的双倍点数已领完");
				return;
			}
		}
		
		ServiceRequestAction.requestServer(PatrolService.receiveDoubleExp(),"ReceiveDoubleExp", 
		                                   (e) => {
			DoubleExpDto expDto = e as DoubleExpDto;
			int point = expDto.point;
			if(_doubleExpStateDto != null)
				point = _doubleExpStateDto.point - expDto.point;
            
            TipManager.AddTip(string.Format("你领取了{0}点双倍点数，快去奋勇杀敌吧！",point));
			UpdateDoubleExpDto(expDto);
		});
	}

	public bool IsAutoFram
	{
		get;
		set;
	}

	public void StartAutoFram()
	{
		IsAutoFram = true;
		HeroView heroView = WorldManager.Instance.GetHeroView();
		if(heroView)
		{
			heroView.SetAutoFram(true);
			heroView.SetPatrolFlag(true);
		}
		ServiceRequestAction.requestServer(PatrolService.begin());
	}

	public void StopAutoFram(bool needStop = false)
	{
		if(IsAutoFram)
		{
			IsAutoFram = false;
			HeroView heroView = WorldManager.Instance.GetHeroView();
			if(heroView)
			{
				heroView.SetAutoFram(false);
				heroView.SetPatrolFlag(false);
				if(needStop)
				{
					heroView.StopAndIdle();
				}
			}
			ServiceRequestAction.requestServer(PatrolService.end());
		}
	}
	#endregion

	#region 玩家称号
	private List<PlayerTitleDto> _titleDtoList;
	public List<PlayerTitleDto> GetTitleList(){
		return _titleDtoList;
	}

	public void GainNewTitle(PlayerTitleDto newTitle){
		for(int i=0;i<_titleDtoList.Count;++i){
			if(_titleDtoList[i].titleId == newTitle.titleId)
			{
				_titleDtoList[i] = newTitle;
				if(OnTitleListUpdate != null)
					OnTitleListUpdate();
				return;
			}
		}

		_titleDtoList.Add(newTitle);
		if(OnTitleListUpdate != null)
			OnTitleListUpdate();
	}

	public void UpdateTitleId(int titleId){
		if(_playerDto != null){
			_playerDto.titleId = titleId;
			if(OnPlayerTitleUpdate != null)
				OnPlayerTitleUpdate();
		}
	}

	public string GetTitleName(){
		if(_playerDto.titleId != 0){
			Title titleInfo = DataCache.getDtoByCls<Title>(_playerDto.titleId);
			if(titleInfo != null)
				return titleInfo.name;
		}
		return "无";
	}
	#endregion

	#region 功德值
	private int _merits;
	public int Merits {
		get {
			return _merits;
		}
		set {
			_merits = value;
		}
	}

	public void UpdateMerits(int newMertis){
		int changeMerits = newMertis -_merits;
		_merits = newMertis;
		if (changeMerits < 0) {
			TipManager.AddGainCurrencyTip(changeMerits, "功德值");
		} else if(changeMerits > 0) {
			TipManager.AddGainCurrencyTip(changeMerits, "功德值");
		}
	}
	#endregion

	#region 玩家染色
	public bool IsDyeMode(){
		if(_playerDto.hairDyeId ==0 || _playerDto.dressDyeId == 0 || _playerDto.accoutermentDyeId == 0)
			return false;
		else
			return true;
	}

	//--TestCode--
	private void SetupColorScheme(){
		Dictionary<int,Dye> dyeInfoDic = DataCache.getDicByCls<Dye>();
		foreach(Dye dye in dyeInfoDic.Values){
			if(dye.colorStr.IndexOf(",") == -1){
				Vector3 colorVec = new Vector3(Random.Range(0f,360f),Random.Range(0f,2f),Random.Range(0f,2f));
				dye.colorStr = Vector3Helper.ToString(colorVec);
			}
		}
	}
	//--TestCode--

	//从静态表中筛选出当前角色各部位配色方案
	public Dictionary<int,List<Dye>> GetColorScheme(){
		Dictionary<int,Dye> dyeInfoDic = DataCache.getDicByCls<Dye>();
		if(dyeInfoDic == null){
			Debug.LogError("dyeInfoDic is null");
			return null;
		}

		Dictionary<int,List<Dye>> result = new Dictionary<int, List<Dye>>(3);
		result.Add(Dye.DyePartTypeEnum_Hair,new List<Dye>(10));
		result.Add(Dye.DyePartTypeEnum_Clothes,new List<Dye>(10));
		result.Add(Dye.DyePartTypeEnum_Ornaments,new List<Dye>(10));

		foreach(Dye dye in dyeInfoDic.Values){
			if(dye.charactorId == _playerDto.charactorId){
				result[dye.dyePartType].Add(dye);
			}
        }

		return result;
	}

	public string GetDyeColorParams(int hairId,int clothId,int decorationId){
		if(hairId == 0 || clothId == 0 || decorationId == 0)
			return "";

		string[] colorParams = new string[3];
		int[] colorSchemeIds = new int[3]{hairId,clothId,decorationId};

		for(int i=0;i<colorParams.Length;++i){
			if(colorSchemeIds[i] != 0)
				colorParams[i] = DataCache.getDtoByCls<Dye>(colorSchemeIds[i]).colorStr;
			else
				colorParams[i]= "0,1,1";
		}

		return string.Join(";",colorParams);
	}
	
	public int GetPartDyeId(int dyePartType){
		if(dyePartType == Dye.DyePartTypeEnum_Hair)
			return _playerDto.hairDyeId;
		else if(dyePartType == Dye.DyePartTypeEnum_Clothes)
			return _playerDto.dressDyeId;
		else if(dyePartType == Dye.DyePartTypeEnum_Ornaments)
			return _playerDto.accoutermentDyeId;

		return 0;
	}

	public void UpdateDyeIds(int hairId,int clothId,int decorationId){
		_playerDto.hairDyeId = hairId;
		_playerDto.dressDyeId = clothId;
		_playerDto.accoutermentDyeId = decorationId;
	}

	public void ChangeDyeMode(System.Action onSuccess = null){
		ServiceRequestAction.requestServer(PlayerService.changeDressing(),"ChangeDressing",(e)=>{
			PlayerDyeNotify notify = e as PlayerDyeNotify;

			PlayerModel.Instance.UpdateDyeIds(notify.hairDyeId,notify.dressDyeId,notify.accoutermentDyeId);
			WorldManager.Instance.GetModel().HandlePlayerDyeChange(notify);

			if(onSuccess != null)
				onSuccess();
		});
	}
	#endregion

    #region 帮派信息

    /** 帮派信息 */
    private PersonalGuildSimpleInfoDto _guildInfo;

    public bool HasGuild()
    {
        return _guildInfo != null && _guildInfo.guildId != 0 && !string.IsNullOrEmpty(_guildInfo.guildName);
    }

    public string GetGuildName()
    {
        return _guildInfo.guildName;
    }

    public string GetMyPosition()
    {
        return _guildInfo.position.name;
    }

    public void UpDateGuild(GuildDto dto)
    {
        if(_guildInfo == null)
        {
            _guildInfo = new PersonalGuildSimpleInfoDto();
        }
        _guildInfo.guildName = dto.name;
        _guildInfo.guildId = dto.id;

        _guildInfo.position = null;
        for(int index = 0;index < dto.players.Count;index++)
        {
            if(dto.players[index].playerId == GetPlayerId())
            {
                _guildInfo.positionId = dto.players[index].position;
                break;
            }
        }
    }
    #endregion
}