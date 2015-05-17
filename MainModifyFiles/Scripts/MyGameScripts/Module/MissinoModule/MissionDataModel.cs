
// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionDataModel.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.nucleus.commons.message;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.mission.data;
using com.nucleus.h1.logic.core.modules.scene.data;
using System.Linq;
using com.nucleus.h1.logic.core.modules.plot.data;
using System.Text.RegularExpressions;

public class MissionDataModel {
	/// <summary>
	/// 游戏中保存任务中心打开的菜单选项 ----------Struct Begin----------
	/// </summary>
	public struct MissionMenuHierarchy {
		private int _mainMenu;
		public int mainMenu{
			get { return _mainMenu; }
			set { _mainMenu = value; }
		}
		private int _subMenu;
		public int subMenu{
			get { return _subMenu; }
			set { _subMenu = value; }
		}
		private bool _settinged;
		public bool settinged {
			get { return _settinged; }
			set { _settinged = value; }
		}
		
		public MissionMenuHierarchy(int mainM, int subM, bool settinged) {
			this._mainMenu = mainM;
			this._subMenu = subM;
			this._settinged = settinged;
		}
	}
	//	游戏中保存任务中心打开的菜单选项 ----------Struct Endf----------


	private static readonly MissionDataModel instance = new MissionDataModel();
	public static MissionDataModel Instance {
		get {
			return instance;
		}
	}

	#region 当前记录
	//	当前任务菜单选择记录
	public MissionMenuHierarchy curMenuHierarchy;
	//	可接任务记录菜单选项
	public MissionMenuHierarchy aceMenuHierarchy;

	//	任务记录Tab选项卡
	private int _missionTabNum = 0;
	public int missionTabNum {
		get {
			if (!IsExistCurMission()) {
				_missionTabNum = 1;
			}
			return _missionTabNum;
		}
	}
	#endregion

	#region -----------------------	Enmu about -----------------------
	public enum MissionTypeEnum {
		/// <summary>
		/// 无类型 0 -- The nothing.
		/// </summary>
		Nothing = 0,
		/// <summary>
		/// 主线 1 -- The main.
		/// </summary>
		Main = 1,
		/// <summary>
		/// 支线 2 -- The extension.
		/// </summary>
		Extension = 2,
		/// <summary>
		/// 师门任务 3 -- The faction.
		/// </summary>
		Faction = 3,
		/// <summary>
		/// 捉鬼任务 4 -- The buffy.
		/// </summary>
		Buffy = 4,
		/// <summary>
		/// 宝图任务 5 -- The treasure.
		/// </summary>
		Treasure = 5,
		/// <summary>
		/// 任务链 6 -- The task chain.
		/// </summary>
		TaskChain = 6,
		/// <summary>
		/// 帮派任务 7 -- The guild.
		/// </summary>
		Guild = 7,
		/// <summary>
		/// 丝绸之路 8 -- The silk.
		/// </summary>
		Silk = 8,
		/// <summary>
		/// 英雄试炼 9 -- The hero.
		/// </summary>
		Hero = 9,
		/// <summary>
		/// 活动 10 -- The holiday.
		/// </summary>
		Holiday = 10
	}

	public enum SubmitDtoType {
		//	无类型
		Nothing = 0,
		//	对话
		Talk = 1,
		//	使用物品
		ApplyItem = 2,
		//	收集物品
		CollectionItem = 3,
		//	收集宠物
		CollectionPet = 4,
		//	暗雷击杀
		HiddenMonster = 5,
		//	明雷击杀
		ShowMonster = 6,
	}
	/*
	private SubmitDtoType _submitDtoType = SubmitDtoType.Nothing;
	public SubmitDtoType submitDtoType {
		get { return _submitDtoType; }
	}
	*/
	#endregion

	#region 一个对话选项的枚举
	public enum DialogueOption {
		/// <summary>
		/// 不存在选项菜单（初始状态）:0 -- The nothing option.
		/// </summary>
		NothingOption = 0,
		/// <summary>
		/// 只存在功能选项:1 -- The only function.
		/// </summary>
		OnlyFunction = 1,
		/// <summary>
		/// 存在功能选项和任务一级菜单:2 -- The function and main mission menu.
		/// </summary>
		FunctionAndMainMissionMenu = 2,
		/// <summary>
		/// 存在功能选项和任务一级菜单:3 -- The function and monster.
		/// </summary>
		FunctionAndMonster = 3,
		/// <summary>
		/// 存在功能选项和任务二级菜单（隐含表示拥有一个或多个NPC功能和有且仅有一个任务）:4 -- The function and sub mission menu.
		/// </summary>
		FunctionAndSubMissionMenu = 4,
		/// <summary>
		/// 只存在任务一级菜单:5 -- The only main mission menu.
		/// </summary>
		OnlyMainMissionMenu = 5,
		/// <summary>
		/// 只存在二级菜单（有且仅有一个任务）:6 -- The only sub mission menu.
		/// </summary>
		OnlySubMissionMenu = 6,
		/// <summary>
		/// 只存在二级菜单(有且仅有一个冥雷任务)：7 -- The only monster menu.
		/// </summary>
		OnlyMonsterMenu = 7,
		/// <summary>
		/// 快速地任务快捷选项：8 -- The quickly menu.
		/// </summary>
		OnlyQuicklyMenu = 8
	}
	private DialogueOption _dialogueOption = DialogueOption.NothingOption;
	public DialogueOption dialogueOption {
		get { return _dialogueOption; }
	}
	#endregion

	#region -----------------------	callback about -----------------------
	public System.Action refreshUICallback = null;
	public System.Action talkCallback = null;
	public System.Action finishCallback = null;
	public System.Action<Npc> submitCallback = null;
	public System.Action<AcceptGiftsDto> dropCallback = null;
	public System.Action<PlayerMissionDto> acceptCallback = null;
	#endregion

	#region -----------------------	Data About	--------------------------
	//	通用配置
	//	是否菜单倒序
	private readonly bool reverseOrder = false;

	//	是否请求服务器
	private bool _resquestServer = true;
	public bool resquestServer {
		get { return _resquestServer; }
		set { _resquestServer = value; }
	}

	private bool _openExpandContent = false;
	public bool openExpandContent {
		get { return _openExpandContent; }
		set { _openExpandContent = value; }
	}

	//	任务寻路等待时间
	private readonly float _findMissionWaitTime = 1.0f;

	//	使用物品倒计时间
	private readonly float _popupUseMissionWaitTime = 3.0f;

	//	环数
	private readonly int _missionRingNum = 10;

	//	地图玩家坐标比例
	private readonly int _mapScaleNum = 10;

	//	提示文字
	private readonly string _endDailyFinishStr = "今日已经做满20次师门任务，继续完成只能获得少量奖励，是否继续？";
	private readonly string _endRingsFinishStr = "已经在外历练了一段时间，先回去看看师傅有什么吩咐吧";

	//	所有任务菜单信息
	private Dictionary<int, MissionType> _missionTypeDic = null;
	private List<Mission> _missionMenuList = null;
	private Dictionary<int, Mission> _missionMenuDic = null;

	private PlayerMissionListDto _playerMissionListDto = null;

	//	维护一个采集任务Dic<missionID, ApplyItemSubmitDto>
	private Dictionary<int, ApplyItemSubmitDto> _applyItemSubmitDtoDic = new Dictionary<int, ApplyItemSubmitDto>();

	//	维护一个虚拟任务Dic<factionNpcID, FactionNpc>
	private Dictionary<int, FactionNpc> _factionNpcDic = null;

	//	Hero 引用
	private HeroView _heroView = null;

	//	是否可穿越传送阵
	private bool _heroCharacterControllerEnable = false;
	public bool heroCharacterControllerEnable {
		get { return _heroCharacterControllerEnable; }
		set { _heroCharacterControllerEnable = value; }
	}
	#endregion

	//	=======================================设置相关=======================================
	#region 设置Tab标签页
	/// <summary>
	/// 设置Tab标签页 -- Sets the mission tab number.
	/// </summary>
	/// <param name="tabIndex">Tab index.</param>
	public void SetMissionTabNum(int tabIndex) {
		if (_missionTabNum != tabIndex) {
			_missionTabNum = tabIndex;
		}
	}
	#endregion
	
	#region 刷新任务面板(任务 接受\提交 之后 \ 达到等级之后 并且任务面板打开状态)
	private void RefreshMissionPanelCallback() {
		if (refreshUICallback != null) {
			refreshUICallback();
		}
	}
	#endregion

	#region 提交任务返回奖励物品
	private void SubmitMissionCallback(SubmitRewardsDto submitRewardsDto, int missionID) {
		string tStr = "TADO -> 手动提交任务 || 手动Finish目标并且服务器自动提交任务有奖励返回物品\n";
		//SubmitRewards tSubmitRewards = submitRewardsDto.submitRewards;
		for (int i = 0, len = submitRewardsDto.submitRewards.rewardList.Count; i < len; i++) {
			tStr += string.Format("获得:{0} * {1}\n",
			                      submitRewardsDto.submitRewards.rewardList[i].item.name,
			                      submitRewardsDto.submitRewards.rewardList[i].itemCount);
			//TipManager.AddTip(tStr);
		}
		GameDebuger.OrangeDebugLog(string.Format("{0}", tStr));

		Npc tNextMissionNpc = GetNextAceMissionNpcByCurMissionID(missionID);
		if (submitCallback != null) {
			submitCallback(tNextMissionNpc);
		}
	}
	#endregion

	//	=======================================静态数据相关=======================================
	#region	获取任务类型（静态表）
	/// <summary>
	/// 获取任务类型（静态表） -- Gets the mission type list.
	/// </summary>
	/// <returns>The mission type list.</returns>
	private Dictionary<int, MissionType> GetMissionTypeDic() {
		if (_missionTypeDic == null) {
			_missionTypeDic = DataCache.getDicByCls<MissionType>();
		}

		return _missionTypeDic;
	}
	#endregion

	#region 获取所有任务菜单数据信息List（静态表）
	/// <summary>
	/// 获取所有任务菜单数据信息List（静态表） - Gets all mission menu list.
	/// </summary>
	/// <returns>The all mission menu list.</returns>
	public List<Mission> GetAllMissionMenuList() {
		if (_missionMenuList == null) {
			_missionMenuList = DataCache.getArrayByCls<Mission>();

			//	是否需要倒序id排列
			if (reverseOrder) {
				_missionMenuList.Sort(delegate(Mission x, Mission y) {
					return -x.id.CompareTo(y.id);
				});
			}
		}
		
		return _missionMenuList;
	}
	#endregion
	
	#region 获取所有任务菜单数据信息Dic（静态表）
	private Dictionary<int, Mission> GetAllMissionMenuDic() {
		if (_missionMenuDic == null) {
			_missionMenuDic = DataCache.getDicByCls<Mission>();
		}
		
		return _missionMenuDic;
	}
	#endregion
	
	#region 获取虚拟NPC数据信息Dic（静态表）
	private Dictionary<int, FactionNpc> GetAllFactionNpcDic() {
		if (_factionNpcDic == null) {
			_factionNpcDic = new Dictionary<int, FactionNpc>();

			for (int i = 1, len = 3; i < len; i++) {
				Npc tNpc = DataCache.getDtoByCls<Npc>(i);
				if (tNpc != null && tNpc is FactionNpc) {
					_factionNpcDic.Add(i, tNpc as FactionNpc);
				}
			}
		}

		return _factionNpcDic;
	}
	#endregion

	//	=======================================菜单数据处理相关=======================================
	#region	获取已接任务二级菜单数据信息
	private List<Mission> GetExistSubMissionMenuList() {
		List<Mission> tExistSubMissionMenuList = new List<Mission>();
		if (_playerMissionListDto == null) {
			return tExistSubMissionMenuList;
		}

		List<PlayerMissionDto> _tPlayerMissionDtoList = GetPlayerMissionDtoList();
		for (int i = 0, len = _tPlayerMissionDtoList.Count; i < len; i++) {
			tExistSubMissionMenuList.Add(_tPlayerMissionDtoList[i].mission);
		}

		return tExistSubMissionMenuList;
	}
	#endregion

	#region	获取可接任务二级菜单数据信息
	private List<Mission> GetMissedSubMissionMenuList() {
		Dictionary<int, Mission> tAllMissionDic = GetAllMissionMenuDic();
		if (tAllMissionDic == null) {
			return null;
		}

		List<Mission> tMissionMenuList = new List<Mission>();

		//	排除已提交的任务
		if (_playerMissionListDto != null) {
			List<int> tPreIDList = GetPreMissionIDList();
			for(int i = 0, len = tPreIDList.Count; i < len; i++) {
				tAllMissionDic.Remove(tPreIDList[i]);
			}
		}

		//	排除当前已接取的任务
		List<Mission> tExistSubMissionMenu = GetExistSubMissionMenuList();
		for (int i = 0, len = tExistSubMissionMenu.Count; i < len; i++) {
			tAllMissionDic.Remove(tExistSubMissionMenu[i].id);
		}

		//	排除当前因等级不足、暂未开放、VIP等原因导致的不显示数据
		foreach (Mission tMission in tAllMissionDic.Values) {
			if (FunctionOpenHelper.JudgeFunctionOpen(tMission.missionType.functionOpenId, false)) {
				//	过滤 主线/支线 中前置任务因数的数据
				if (IsMainOrExtension(tMission)) {
					if (IsMissionAcceptConditionByMission(tMission)) {
						tMissionMenuList.Add(tMission);
					}
				} else if (!IsExistBindMissionByTypeID(tMission.type)) {
					tMissionMenuList.Add(tMission);
				}
			}
		}

		GameDebuger.AquaDebugLog("=============  获取可接任务二级菜单数据信息 Begin =============");
		foreach (Mission tMission in tMissionMenuList) {
			GameDebuger.AquaDebugLog(string.Format("ID:{0} | Type:{1} | Name:{2}-{3}",
			                                       tMission.id, tMission.type, tMission.missionType.name, tMission.name));
		}
		GameDebuger.AquaDebugLog("============= 获取可接任务二级菜单数据信息 End =============");

		return tMissionMenuList;
	}
	#endregion

	//	=======================================菜单通用数据处理相关=======================================
	#region	获取任务一级菜单数据信息(已接\可接 -- 通用方法，使用existSta区分是否已接取任务)
	/// <summary>
	/// 获取任务一级菜单数据信息(已接\可接 -- 通用方法，使用existSta区分是否已接取任务) -- Gets the main mission menu list.
	/// </summary>
	/// <returns>The main mission menu list.</returns>
	/// <param name="existSta">If set to <c>true</c> exist sta.</param>
	public List<MissionType> GetMainMissionMenuList(bool existSta) {
		/*
		 *  |-- 主线
		 *  |-- 支线
		 *  |-- 日常
		 *      |-- 师门
		 * 		|--	捉鬼
		 * 		|-- 宝图
		 * 		|-- 封妖
		 * 		|-- 星宿
		 * 		|-- ......
		 */
		List<Mission> subMissionMenuList = GetSubMissionMenuList(existSta);
		List<MissionType> tMainMissionMenuList = new List<MissionType>();

		//	是否已添加日常任务类型
		bool tExistDailyType = false;
		for (int i = 0, len = subMissionMenuList.Count; i < len; i++) {
			Mission tMission = subMissionMenuList[i];
			if (tMission.missionType == null) {
				Debug.LogError(string.Format("ERROR: -- @策划 -- 当前Mission Name -> {0} Type Is Null,请策划人员查看mission表id为{1}的数据信息",
				                             tMission.name, tMission.id));
				return null;
			}

			//	当前任务类型是否已存在列表中
			bool tExistInList = false;
			for (int j = 0, tlen = tMainMissionMenuList.Count; j < tlen; j++) {
				if (IsMainOrExtension(tMission)) {
					if (tMission.type == tMainMissionMenuList[j].id) {
						tExistInList = true;
						break;
					}
				} else if (tMission.type > 2) {
					//	IsExistBindMissionByTypeID(id)
					if (!tExistDailyType && tMainMissionMenuList[j].id <= 2) {
						tExistDailyType = true;
					} else {
						tExistInList = true;
					}
					break;
				} else {
					GameDebuger.OrangeDebugLog(string.Format("ERROR：未分大类任务 ID：{0} | Name：{1}-{2}",
					                           tMission.id, tMission.missionType.name, tMission.name));
					break;
				}
			}

			if (!tExistInList) {
				tMainMissionMenuList.Add(tMission.missionType);
			}
		}

		//	MissionType排序
		tMainMissionMenuList.Sort((x, y) => {
			return x.id.CompareTo(y.id);
		});
		
		GameDebuger.OrangeDebugLog(string.Format("开始一级任务分类菜单 -> {0} 数据信息List", existSta? "已接" : "可接"));
		foreach (MissionType tMissionType in tMainMissionMenuList) {
			GameDebuger.OrangeDebugLog(string.Format("Main Menu ======== ID -> {0} | Name -> {1} ========",
			                                         tMissionType.id, tMissionType.id > 2? "日常任务" : tMissionType.name));
		}

		return tMainMissionMenuList;
	}
	#endregion
	
	#region 获取二级细分菜单(已接\可接 -- 通用方法，使用existSta区分是否已接取任务)
	/// <summary>
	/// 获取二级细分菜单(已接\可接 -- 通用方法，使用existSta区分是否已接取任务) -- Gets the subdivision menu list.
	/// </summary>
	/// <returns>The subdivision menu list.</returns>
	/// <param name="mainMenuId">Main menu identifier.</param>
	/// <param name="existSta">If set to <c>true</c> exist sta.</param>
	public List<Mission> GetSubdivisionMenuList(int mainMenuId, bool existSta) {
		List<Mission> tSubMissionMenuList = GetSubMissionMenuList(existSta);
		List<Mission> tSubdivisionMenuList = new List<Mission>();

		for (int i = 0, len = tSubMissionMenuList.Count; i < len; i++) {
			Mission tMission = tSubMissionMenuList[i];

			if (existSta) {
				if (tMission.type == mainMenuId) {
					tSubdivisionMenuList.Add(tMission);
				}
			} else {
				if (mainMenuId <= 2) {
					if (tMission.type == mainMenuId) {
						tSubdivisionMenuList.Add(tMission);
					}
				} else if (tMission.type > 2) {
					tSubdivisionMenuList.Add(tMission);
				}
			}
		}

		//	Mission排序
		tSubdivisionMenuList.Sort((x, y) => {
			return x.id.CompareTo(y.id);
		});
		
		GameDebuger.OrangeDebugLog(string.Format("开始二级任务细分菜单 -> {0} 数据信息List", existSta? "已接" : "可接"));
		foreach (Mission tMission in tSubdivisionMenuList) {
			GameDebuger.OrangeDebugLog(string.Format("Sub Menu -------- ID -> {0} | Name -> {1} --------", tMission.id, tMission.name));
		}

		return tSubdivisionMenuList;
	}
	#endregion

	#region 获取所有二级菜单（通用方法） -- Local
	/// <summary>
	/// 获取所有二级菜单（通用方法） -- Local | Gets the sub mission menu list.
	/// </summary>
	/// <returns>The sub mission menu list.</returns>
	/// <param name="existSta">If set to <c>true</c> exist sta.</param>
	public List<Mission> GetSubMissionMenuList(bool existSta) {
		return existSta? GetExistSubMissionMenuList() : GetMissedSubMissionMenuList();
	}
	#endregion

	#region 主界面获取二级菜单方法（该方法支持了使用物品的回调设置）
	/// <summary>
	/// 主界面获取二级菜单方法（该方法支持了使用物品的回调设置） -- Gets the sub mission menu list in main user interface expand.
	/// </summary>
	/// <returns>The sub mission menu list in main user interface expand.</returns>
	public List<Mission> GetSubMissionMenuListInMainUIExpand() {
		List<Mission> tSubMissionMenuList = GetExistSubMissionMenuList();

		//	设置收集任务回调
		for (int i = 0, len = tSubMissionMenuList.Count; i < len; i++) {
			Mission tMission = tSubMissionMenuList[i];
			SubmitDto tSubmitDto = GetSubmitDtoByMission(tMission);
			if (IsApplyItem(tSubmitDto)) {
				if (!_applyItemSubmitDtoDic.ContainsKey(tMission.id)) {
					GameDebuger.OrangeDebugLog(string.Format(" ===== TADO:把当前MissionID：{0} 的任务加入到采集Dic中", tMission.id));
					_applyItemSubmitDtoDic.Add(tMission.id, tSubmitDto as ApplyItemSubmitDto);
				}
			}
		}

		//	当当前有某个任务的目标是收集任务，设置人物角色移动回调到MissionDataModel
		if (_heroView == null) {
			_heroView = WorldManager.Instance.GetHeroView();
		}
		if (_heroView != null) {
			if (_heroView.checkMissioinCallback == null && _applyItemSubmitDtoDic.Count > 0) {
				_heroView.checkMissioinCallback = AppltMissionPlayerInRadius;
			} else {
				_heroView.checkMissioinCallback = null;
			}
		}
		
		return tSubMissionMenuList;
	}
	#endregion
	/*
	#region 获取指定NPC对话一级菜单类型列表
	public List<string> GetMainMissionMenuNameList(Npc) {
		List<MissionOption> tMissionOption = GetMissionOptionListByNpcID(0);
		List<MissionType> tMissionType = GetMainMissionMenuList();
	}
	#endregion
	*/

	//	================================== Dto相关（玩家数据）==================================
	#region 获取常规项数据
	private PlayerMissionListDto GetPlayerMissionListDto() {
		return _playerMissionListDto;
	}
	
	/// <summary>
	/// 外部获取当前绑定所有任务 -- Gets the player mission dto list.
	/// </summary>
	/// <returns>The player mission dto list.</returns>
	public List<PlayerMissionDto> GetPlayerMissionDtoList() {
		List<PlayerMissionDto> tPlayerMissionDtoList = null;
		if (_playerMissionListDto != null) {
			tPlayerMissionDtoList = _playerMissionListDto.bodyMissions;
		}
		return tPlayerMissionDtoList;
	}

	private PlayerMissionDto GetPlayerMissionDtoByMissionID(int missionID) {
		List<PlayerMissionDto> tPlayerMissionDtoList = GetPlayerMissionDtoList();
		if (tPlayerMissionDtoList != null) {
			for (int i = 0, len = tPlayerMissionDtoList.Count; i < len; i++) {
				if (tPlayerMissionDtoList[i].missionId == missionID) {
					return tPlayerMissionDtoList[i];
				}
			}
		}
		return null;
	}

	private Mission GetCurrentMissionByMissionID(int missionID) {
		Mission tMission = null;
		PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(missionID);
		if (tPlayerMissionDto != null) {
			tMission = tPlayerMissionDto.mission;
		}

		return tMission;
	}
	
	private List<int> GetPreMissionIDList() {
		return _playerMissionListDto.submitMissionIds;
	}
	#endregion

	//	================================== 任务提交项相关==================================
	#region 获取任务提交项列表（ByMission）
	private List<SubmitDto> GetSubmitDtoListByMission(Mission mission) {
		List<SubmitDto> tSubmitDtoList = new List<SubmitDto>();

		PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(mission.id);
		if (tPlayerMissionDto != null) {
			tSubmitDtoList = tPlayerMissionDto.completions;
		}
		return tSubmitDtoList;
	}
	#endregion
	
	#region 获取任务提交具体条件（未完成的那一个,可提交任务时候返回空）
	/// <summary>
	/// 获取任务提交具体条件（未完成的那一个,可提交任务时候返回空） -- Gets the submit dto by mission.
	/// </summary>
	/// <returns>The submit dto by mission.</returns>
	/// <param name="mission">Mission.</param>
	public SubmitDto GetSubmitDtoByMission(Mission mission) {
		List<SubmitDto> tSubmitDtoList = GetSubmitDtoListByMission(mission);
		for (int i = 0, len = tSubmitDtoList.Count; i < len; i++) {
			SubmitDto tSubmitDto = tSubmitDtoList[i];
			if (!tSubmitDtoList[i].finish) {
				string tDebugStr = string.Format("########### 打印任务当前提交项条件 ###########\n" +
				                                 "| 任务ID:{0} | Name:{1}-{2}\n" +
				                                 "| 提交具体条件 List Count:{3} | 任务自动提交:{4}\n" +
				                                 "| SubmitDto ID：{5} | 完成状态：{6}\n" +
				                                 "| 目标是否自动提交：{7} | Need:{8} | Count:{9}\n",
				                                 mission.id, mission.missionType.name, mission.name, tSubmitDtoList.Count, mission.autoSubmit,
				                                 tSubmitDto.id, tSubmitDto.finish, tSubmitDto.auto, tSubmitDto.needCount, tSubmitDto.count);
				SubmitDtoType tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(tSubmitDto);

				switch (tSubmitDtoType) {
				case SubmitDtoType.Nothing:
					tDebugStr += string.Format("没有任务目标啊~！ 目标类型为：SubmitDtoType.Nothing （执行到这里表示报错了）\n");
					break;
				case SubmitDtoType.Talk:
					TalkSubmitDto tTalkSubmitDto = tSubmitDto as TalkSubmitDto;
					tDebugStr += string.Format("| 寻人任务：找到 -> {0}\n", GetNpcNameByNpc(tTalkSubmitDto.submitNpc, true, false));
					break;
				case SubmitDtoType.ApplyItem:
					ApplyItemSubmitDto tApplyItemSubmitDto = tSubmitDto as ApplyItemSubmitDto;
					if (tApplyItemSubmitDto.item == null) {
						tDebugStr += string.Format("| 到达某地任务：到达 -> {0}的坐标{1}-{2}\n",
						                           tApplyItemSubmitDto.acceptScene.sceneMap.name,
						                           tApplyItemSubmitDto.acceptScene.x/_mapScaleNum, tApplyItemSubmitDto.acceptScene.z/_mapScaleNum);
					} else {
						tDebugStr += string.Format("| 使用物品任务：到达 -> {0}的坐标{1}-{2}\n 使用 {3}\n",
						                           tApplyItemSubmitDto.acceptScene.sceneMap.name,
						                           tApplyItemSubmitDto.acceptScene.x/_mapScaleNum, tApplyItemSubmitDto.acceptScene.z/_mapScaleNum,
						                           tApplyItemSubmitDto.item.name);
					}
					break;
				case SubmitDtoType.CollectionItem:
					CollectionItemSubmitDto tCollectionItemSubmitDto = tSubmitDto as CollectionItemSubmitDto;
					tDebugStr += string.Format("| 收集物品任务：需要 -> {0} 个 {1} 去找 {2} 购买\n",
					                           tCollectionItemSubmitDto.needCount,
					                           tCollectionItemSubmitDto.item == null?
					                           string.Format("没有 ID:{0} 的物品，请检查数据表", tCollectionItemSubmitDto.itemId)
					                           : tCollectionItemSubmitDto.item.name,
					                           GetNpcNameByNpc(tCollectionItemSubmitDto.acceptNpc.npc, true, false));
					break;
				case SubmitDtoType.CollectionPet:
					CollectionPetSubmitDto tCollectionPetSubmitDto = tSubmitDto as CollectionPetSubmitDto;
					tDebugStr += string.Format("| 收集宠物任务：需要 -> {0} 个 {1} 去找 {2} 购买\n",
					                           tCollectionPetSubmitDto.needCount,
					                           tCollectionPetSubmitDto.pet == null?
					                           string.Format("没有 ID:{0} 的宠物，请检查数据表", tCollectionPetSubmitDto.petId)
					                           : tCollectionPetSubmitDto.pet.name,
					                           GetNpcNameByNpc(tCollectionPetSubmitDto.acceptNpc.npc, true, false));
					break;
				case SubmitDtoType.HiddenMonster:
					HiddenMonsterSubmitDto tHiddenMonsterSubmitDto = tSubmitDto as HiddenMonsterSubmitDto;
					tDebugStr += string.Format("| 暗雷任务： -> 巡逻 {0} 场景 杀掉 {1} 个 {2}\n",
					                           tHiddenMonsterSubmitDto.acceptScene.sceneMap.name,
					                           tHiddenMonsterSubmitDto.needCount,
					                           tHiddenMonsterSubmitDto.monster == null?
					                           tHiddenMonsterSubmitDto.monsterId.ToString() : tHiddenMonsterSubmitDto.monster.name);
					break;
				case SubmitDtoType.ShowMonster:
					ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;
					if (IsBuffyMissionType(mission)) {
						tDebugStr += string.Format("| 捉鬼任务： -> 打败 {0}\n",
						                           GetNpcNameByNpc(tShowMonsterSubmitDto, true, false));
					} else {
						tDebugStr += string.Format("| 冥雷任务： -> 打败 {0}\n",
						                           GetNpcNameByNpc(tShowMonsterSubmitDto.acceptNpc.npc, true, false));
					}
					break;
				default:
					tDebugStr += string.Format("没有任务目标啊~！ 目标类型为：SubmitDtoType.Nothing （执行到这里表示报错了）\n");
					break;
				}

				GameDebuger.OrangeDebugLog(tDebugStr);
				return tSubmitDtoList[i];
			}
		}
		GameDebuger.OrangeDebugLog(string.Format("####### 已没有下一个提交项，可提交当前任务 | 当前任务ID:{0} | Name:{1}-{2} | Auto:{3} #######",
		                                         mission.id, mission.missionType.name, mission.name, mission.autoSubmit));
		return null;
	}
	#endregion

	#region 获取已接任务寻路目标地点（ByMission）
	private Npc GetBindMissionNpcByMission(Mission mission) {
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		Npc tNpc = tSubmitDto == null?
			mission.submitNpc : GetBindMissionNpcBySubmit(tSubmitDto, mission.id);

		//	当改NPC是虚拟NPC是转换为具体NPC
		if (tNpc is FactionNpc) {
			tNpc = NpcVirturlToEntity(mission.submitNpc);
		}

		return tNpc;
	}
	#endregion

	#region 获取已接任务寻路目标地点（BySubmit）
	private Npc GetBindMissionNpcBySubmit(SubmitDto submitDto, int missionID) {
		Npc tBindMissionToNpc = null;
		bool tReFinish = submitDto.count >= submitDto.needCount;
		
		SubmitDtoType tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(submitDto);
		switch (tSubmitDtoType) {
		case SubmitDtoType.Nothing:
			return null;
			break;
		case SubmitDtoType.Talk:
			TalkSubmitDto tTalkSubmitDto = (submitDto as TalkSubmitDto);
			tBindMissionToNpc = tTalkSubmitDto.submitNpc;
			break;
		case SubmitDtoType.ApplyItem:
			ApplyItemSubmitDto tApplyItemSubmitDto = submitDto as ApplyItemSubmitDto;
			tBindMissionToNpc = new Npc();
			
			tBindMissionToNpc.id = 0;
			tBindMissionToNpc.name = tApplyItemSubmitDto.acceptScene.sceneMap.name;
			tBindMissionToNpc.sceneId = tApplyItemSubmitDto.acceptScene.id;
			tBindMissionToNpc.x = tApplyItemSubmitDto.acceptScene.x;
			tBindMissionToNpc.y = 0;
			tBindMissionToNpc.z = tApplyItemSubmitDto.acceptScene.z;
			
			tBindMissionToNpc = tReFinish? tApplyItemSubmitDto.submitNpc.npc : tBindMissionToNpc;			
			break;
		case SubmitDtoType.CollectionItem:
			CollectionItemSubmitDto tCollectionItemSubmitDto = submitDto as CollectionItemSubmitDto;
			
			tBindMissionToNpc = tReFinish? tCollectionItemSubmitDto.submitNpc.npc : tCollectionItemSubmitDto.acceptNpc.npc;
			break;
		case SubmitDtoType.CollectionPet:
			CollectionPetSubmitDto tCollectionPetSubmitDto = submitDto as CollectionPetSubmitDto;
			
			tBindMissionToNpc = tReFinish? tCollectionPetSubmitDto.submitNpc.npc : tCollectionPetSubmitDto.acceptNpc.npc;
			break;
		case SubmitDtoType.HiddenMonster:
			HiddenMonsterSubmitDto tHiddenMonsterSubmitDto = submitDto as HiddenMonsterSubmitDto;
			tBindMissionToNpc = new Npc();
			
			tBindMissionToNpc.id = 0;
			tBindMissionToNpc.name = tHiddenMonsterSubmitDto.acceptScene.sceneMap.name;
			tBindMissionToNpc.sceneId = tHiddenMonsterSubmitDto.acceptScene.id;
			tBindMissionToNpc.x = tHiddenMonsterSubmitDto.acceptScene.x;
			tBindMissionToNpc.y = 0;
			tBindMissionToNpc.z = tHiddenMonsterSubmitDto.acceptScene.z;
			
			tBindMissionToNpc = tReFinish? tHiddenMonsterSubmitDto.submitNpc.npc : tBindMissionToNpc;
			break;
		case SubmitDtoType.ShowMonster:
			ShowMonsterSubmitDto tShowMonsterSubmitDto = submitDto as ShowMonsterSubmitDto;
			
			tBindMissionToNpc = tReFinish? tShowMonsterSubmitDto.submitNpc.npc : tShowMonsterSubmitDto.acceptNpc.npc;
			break;
		default:
			break;
		}

		//	当改NPC是虚拟NPC是转换为具体NPC
		if (tBindMissionToNpc is FactionNpc) {
			tBindMissionToNpc = NpcVirturlToEntity(tBindMissionToNpc);
		}

		return tBindMissionToNpc;
	}
	#endregion

	#region SubmitDtoType 获取任务提交项类型
	/// <summary>
	/// 获取任务提交项类型 -- Gets the submit dto type by mission.
	/// </summary>
	/// <returns>The submit dto type by mission.</returns>
	/// <param name="mission">Mission.</param>
	public SubmitDtoType GetSubmitDtoTypeByMission(Mission mission) {
		SubmitDtoType tSubmitDtoType = SubmitDtoType.Nothing;
		if (mission != null) {
			SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
			if (tSubmitDto != null) {
				tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(tSubmitDto);
			}
		}

		return tSubmitDtoType;
	}
	#endregion

	#region SubmitDtoType 获取任务提交项类型
	private SubmitDtoType GetSubmitDtoTypeBySubmitDto(SubmitDto submitDto) {
		SubmitDtoType tSubmitDtoType = SubmitDtoType.Nothing;

		if (submitDto != null) {
			if (submitDto is TalkSubmitDto) {
				tSubmitDtoType = SubmitDtoType.Talk;
			} else if (submitDto is ApplyItemSubmitDto) {
				tSubmitDtoType = SubmitDtoType.ApplyItem;
			} else if (submitDto is CollectionItemSubmitDto) {
				tSubmitDtoType = SubmitDtoType.CollectionItem;
			} else if (submitDto is CollectionPetSubmitDto) {
				tSubmitDtoType = SubmitDtoType.CollectionPet;
			} else if (submitDto is HiddenMonsterSubmitDto) {
				tSubmitDtoType = SubmitDtoType.HiddenMonster;
			} else if (submitDto is ShowMonsterSubmitDto) {
				tSubmitDtoType = SubmitDtoType.ShowMonster;
			}
		}

		return tSubmitDtoType;
	}
	#endregion

	#region 玩家移动到任务相关地点后意图Finish任务
	/// <summary>
	/// 玩家移动到任务相关地点后意图Finish任务 -- Wants to finish submit dto.
	/// </summary>
	/// <param name="mission">Mission.</param>
	/// <param name="isExistState">If set to <c>true</c> is exist state.</param>
	/// <param name="tFindToTargetNpc">T find to target npc.</param>
	private void WalkEndToFinishSubmitDto(Mission mission, bool isExistState, Npc tFindToTargetNpc) {
		if (isExistState) {
			SubmitDtoType tSubmitDtoType = GetSubmitDtoTypeByMission(mission);
			//	当前任务是使用物品类型
			if (tSubmitDtoType == SubmitDtoType.ApplyItem) {
				if (!_heroCharacterControllerEnable) {
					ApplySubmitProcessing(mission);
				}
			} else if (tSubmitDtoType == SubmitDtoType.Nothing) {
				GameDebuger.OrangeDebugLog("WARNING:未知SubmitDto类型");
			}
		}
	}
	#endregion

	#region 采集任务进行部分（注意：只针对采集任务）
	private void ApplySubmitProcessing (Mission mission) {
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		ApplyItemSubmitDto tApplyItemSubmitDto = tSubmitDto as ApplyItemSubmitDto;
		Npc tFindToTargetNpc = GetBindMissionNpcBySubmit(tSubmitDto, mission.id);
		if (tFindToTargetNpc == null) return;

		if (tSubmitDto == null) {
			//	未完成的那一个,可提交任务时候返回空
		} else if (tApplyItemSubmitDto.item == null) {
			FinishMission(mission.id);
		} else {
			//	弹出使用物品Tips
			GameDebuger.OrangeDebugLog("弹出使用物品Tips");
			MainUIViewController.Instance.PopupUseMissionPropsBtn(tApplyItemSubmitDto, () => {
				if (tFindToTargetNpc.sceneId == WorldManager.Instance.GetModel().GetSceneId()) {
					string tDesStr = string.Format("{0}{1}[-]", _whiteColor, tApplyItemSubmitDto.item.description);
					MainUIViewController.Instance.SetMissionUsePropsProgress(true, tDesStr);
					CoolDownManager.Instance.SetupCoolDown("__PopupUseMissionProps", _popupUseMissionWaitTime, (floatTime) => {
						MainUIViewController.Instance.SetMissionUsePropsProgress(1-floatTime/_popupUseMissionWaitTime);
					}, () => {
						MainUIViewController.Instance.SetMissionUsePropsProgress(1);
						MainUIViewController.Instance.SetMissionUsePropsProgress(false);
						FinishTargetSubmitDto(mission, tFindToTargetNpc);
					}, 0);
				} else {
					//	去对应场景
					FindToMissionNpcByMission(mission, true);
				}
			}, true);
		}
	}
	#endregion
	
	#region 对话后意图Finish任务
	/// <summary>
	/// 对话后意图Finish任务 (return true:关闭对话， false:不需要关闭对话) -- Finishs the target submit dto.
	/// </summary>
	/// <returns><c>true</c>, if target submit dto was finished, <c>false</c> otherwise.</returns>
	/// <param name="mission">Mission.</param>
	/// <param name="npc">Npc.</param>
	public bool FinishTargetSubmitDto(Mission mission, Npc npc) {
		bool tReturnState = true;

		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		SubmitDtoType submitDtoType = GetSubmitDtoTypeBySubmitDto(tSubmitDto);
		bool tIsFinishCountState = tSubmitDto.count >= tSubmitDto.needCount;

		if (submitDtoType == SubmitDtoType.CollectionItem || submitDtoType == SubmitDtoType.CollectionPet) {
			tReturnState = tIsFinishCountState;

			//	收集类型任务，confirm表示是否提交物品选项
			if (tIsFinishCountState) {
				if (tSubmitDto.confirm) {
					GameDebuger.OrangeDebugLog(string.Format("{0}-{1}任务 对白后需要确认窗口", mission.missionType.name, mission.name));
					ProxyWindowModule.OpenConfirmWindow(string.Format("是否提交{0}-{1}？", mission.missionType.name, mission.name),
					                                    string.Format("{0}-{1}", mission.missionType.name, mission.name),
					                                    () => {
						FinishMission(mission.id);
					});
				} else {
					FinishMission(mission.id);
				}
			}
		} else if (submitDtoType == SubmitDtoType.Talk) {
			tReturnState = false;

			//	对话任务
			TalkMission(npc.id, () => {
				if (tSubmitDto.auto) {
					ProxyWorldMapModule.CloseNpcDialogue();
				} else {
					FinishMission(mission.id);
				}
			});
		} else if (submitDtoType == SubmitDtoType.ApplyItem) {
			//	使用物品任务 \ 到达指定地点任务
			FinishMission(mission.id);
		} else if (submitDtoType == SubmitDtoType.HiddenMonster) {
			//	暗雷类型任务
		} else if (submitDtoType == SubmitDtoType.ShowMonster && !tIsFinishCountState) {
			//	明雷类型任务，confirm是否在战斗前出现选项
			//	请求战斗
			BattleShowMonster(mission.id, npc, () => {
				//	TADO
			});
		}

		return tReturnState;
	}
	#endregion

	//	=======================================玩家移动相关=======================================
	#region 寻路到师门师傅 \ 首席
	private void FindToMasterOrChief(int virturlID) {
		Npc tFactionMasterNpc = GetFactionNpcByVirtualID(virturlID);
		WorldManager.Instance.WalkToByNpc(tFactionMasterNpc);
	}
	#endregion

	#region 玩家移动到任务相关地点(马上寻路)
	/// <summary>
	/// 玩家移动到任务相关地点(马上寻路) -- Finds to mission npc by mission.
	/// </summary>
	/// <param name="mission">Mission.</param>
	/// <param name="isExistState">If set to <c>true</c> is exist state.</param>
	public void FindToMissionNpcByMission(Mission mission, bool isExistState) {
		if (mission == null) {
			GameDebuger.OrangeDebugLog("当前任务是空：Current Mission Is Null");
			return;
		}

		GameDebuger.OrangeDebugLog("开始玩家移动");
		HeroCharacterControllerEnable(false);

		//	已接任务判断是否是暗雷
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		if (isExistState && IsHiddenMonster(tSubmitDto)) {
			HiddenMonsterSubmitDto tHiddenMonsterSubmitDto = tSubmitDto as HiddenMonsterSubmitDto;
			
			if(WorldManager.Instance.GetModel().GetSceneId() == tHiddenMonsterSubmitDto.acceptScene.id) {
				PlayerModel.Instance.StartAutoFram();
			} else {
				WorldManager.Instance.Enter(tHiddenMonsterSubmitDto.acceptScene.id, false, true);
			}
		} else {
			Npc tFindToTargetNpc = GetMissionNpcByMission(mission, isExistState);

			//	当改NPC是虚拟NPC是转换为具体NPC
			if (tFindToTargetNpc is FactionNpc) {
				tFindToTargetNpc = NpcVirturlToEntity(tFindToTargetNpc);
			}

			if (tFindToTargetNpc == null) return;

			WorldManager.Instance.WalkToByNpc(tFindToTargetNpc, () => {
				GameDebuger.OrangeDebugLog("玩家移动到任务相关地点");
				
				WalkEndToFinishSubmitDto(mission, isExistState, tFindToTargetNpc);
			});
		}
	}
	/*
	public void FindToMissionNpcByNpcID(int NpcID) {
		HeroCharacterControllerEnable(false);
	}*/
	#endregion
	
	#region GoToMission Wait Time 玩家移动到任务相关地点(寻路等待时间)
	/// <summary>
	/// GoToMission Wait Time 玩家移动到任务相关地点(寻路等待时间) -- Waits the time find to mission.
	/// </summary>
	/// <param name="mission">Mission.</param>
	/// <param name="isExitState">If set to <c>true</c> is exit state.</param>
	public void WaitTimeFindToMission(Mission mission, bool isExitState) {
		CoolDownManager.Instance.SetupCoolDown("_waitTimeForNextMission", _findMissionWaitTime, null, () => {
			FindToMissionNpcByMission(mission, isExitState);
		});
	}
	#endregion

	#region 虚拟NPC转换为配置NPC
	/// <summary>
	/// 虚拟NPC转换为配置NPC -- Npcs the virturl to entity.
	/// </summary>
	/// <returns>The virturl to entity.</returns>
	/// <param name="npc">Npc.</param>
	public Npc NpcVirturlToEntity(Npc npc) {
		FactionNpc tFactionNpc = npc as FactionNpc;
		int tFactionId = PlayerModel.Instance.GetPlayer().factionId;
		return DataCache.getDtoByCls<Npc>(tFactionNpc.npcIds[tFactionId-1]);
	}
	#endregion

	#region 根据门派ID获取 师门师傅Npc：1 \ 首席Npc：2
	/// <summary>
	/// 根据门派ID获取 师门师傅Npc：1 \ 首席Npc：2 -- Gets the faction npc by virtual I.
	/// </summary>
	/// <returns>The faction npc by virtual I.</returns>
	/// <param name="npcVirtualID">Npc virtual I.</param>
	public Npc GetFactionNpcByVirtualID(int npcVirtualID) {
		int tFactionId = PlayerModel.Instance.GetPlayer().factionId;
		//	虚拟NPC Dic
		if (npcVirtualID <= 0 || npcVirtualID > 2) {
			GameDebuger.OrangeDebugLog(string.Format("虚拟NpcID：{0}错误，师傅：1 | 首席：2", npcVirtualID));
			return null;
		}

		Dictionary<int, FactionNpc> tFactionNpcDic = GetAllFactionNpcDic();
		int npcID = tFactionNpcDic[npcVirtualID].npcIds[tFactionId-1];

		return DataCache.getDtoByCls<Npc>(npcID);
	}
	#endregion

	#region 通过ID获取任务Npc(当true：返回已接任务NPC， false：返回接取NPC)
	/// <summary>
	/// 通过ID获取任务Npc(当true：返回已接任务NPC， false：返回接取NPC) -- Gets the mission npc by mission.
	/// </summary>
	/// <returns>The mission npc by mission.</returns>
	/// <param name="mission">Mission.</param>
	/// <param name="existSta">If set to <c>true</c> exist sta.</param>
	public Npc GetMissionNpcByMission(Mission mission, bool existSta) {
		if (existSta) {
			return GetBindMissionNpcByMission(mission);
		}
		
		Npc tMissedNpc = mission.missionType.acceptNpc;
		return tMissedNpc == null? mission.acceptNpc : tMissedNpc;
	}
	#endregion

	#region 玩家采集任务玩家移动回调
	private bool _applyMissionToTargetState = false;
	private void AppltMissionPlayerInRadius() {
		if (_heroView == null) {
			return;
		}

		Vector3 tPlayerPosition = _heroView.transform.position;
		Vector2 tPlayerV2 = new Vector2(tPlayerPosition.x*_mapScaleNum, tPlayerPosition.z*_mapScaleNum);

		foreach (int missionID in _applyItemSubmitDtoDic.Keys) {
			ApplyItemSubmitDto tApplyItemSubmitDto = _applyItemSubmitDtoDic[missionID];
			//	当前场景是否使用物品场景判断
			bool tIsApplyItemScene = tApplyItemSubmitDto.acceptScene.id == WorldManager.Instance.GetModel().GetSceneId();
			if (!tIsApplyItemScene) {
				continue;
			}

			Vector2 tCenterV2 = new Vector2(tApplyItemSubmitDto.acceptScene.x, tApplyItemSubmitDto.acceptScene.z);
			float tDistanceInRadius = Vector2.Distance(tPlayerV2, tCenterV2);

			//GameDebuger.OrangeDebugLog(string.Format("=== DistanceInRadius ==== : {0}", tDistanceInRadius));
			bool appltMissionToTargetState = tDistanceInRadius <= tApplyItemSubmitDto.acceptRadius;
			if (_applyMissionToTargetState != appltMissionToTargetState) {
				_applyMissionToTargetState = appltMissionToTargetState;
				if (appltMissionToTargetState) {
					if (_heroCharacterControllerEnable) {
						ApplySubmitProcessing(GetCurrentMissionByMissionID(missionID));
					}
				} else {
					MainUIViewController.Instance.PopupUseMissionPropsBtn(null, null, true);
				}
			}
		}
	}
	#endregion

	//	=======================================NPC对话相关=======================================
	#region 遍历已接\可接任务列表，判断当前NPC是否拥有任务相关对话内容，并返回一个列表
	/// <summary>
	/// 遍历已接\可接任务列表，判断当前NPC是否拥有任务相关对话内容，并返回一个列表 -- Gets the mission option list by npc I.
	/// </summary>
	/// <returns>The mission option list by npc I.</returns>
	/// <param name="npc">Npc.</param>
	public List<MissionOption> GetMissionOptionListByNpc(Npc npc) {
		List<MissionOption> tMissionOptionList = new List<MissionOption>();
		
		//	查找已接任务列表
		List<Mission> tCurMissionList = GetExistSubMissionMenuList();
		//	查找可接任务列表
		List<Mission> tAceMissionList = GetMissedSubMissionMenuList();
		//	列表合并
		List<Mission> tAllMissionList = tCurMissionList.Concat(tAceMissionList).ToList();

		//	遍历它们判断是否拥有任务相关对话内容
		GameDebuger.OrangeDebugLog("开始遍历已接、可接任务列表，判断项是否拥有任务相关对话内容 =======");
		for (int i = 0, len = tAllMissionList.Count; i < len; i++) {
			bool tIsExist = i < tCurMissionList.Count;
			Npc tNpc = GetMissionNpcByMission(tAllMissionList[i], tIsExist);

			if (tNpc != null && npc.id == tNpc.id) {
				MissionOption tMissionOption = new MissionOption(tAllMissionList[i], tIsExist);

				//	冥雷怪物
				SubmitDto tSubmitDto = GetSubmitDtoByMission(tAllMissionList[i]);
				if (tIsExist && IsShowMonster(tSubmitDto)) {
					ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;
					if (tShowMonsterSubmitDto.confirm) {
						tMissionOptionList.Add(tMissionOption);
					}
				} else {
					tMissionOptionList.Add(tMissionOption);
				}

				GameDebuger.OrangeDebugLog(string.Format("当前匹配项 Type:{0}任务 | ID:{1} | Name:{2} - {3} ----------",
				                                         tMissionOption.optionType? "绑定":"可接",
				                                         tMissionOption.mission.id,
				                                         tMissionOption.mission.missionType.name,
				                                         tMissionOption.mission.name));
			}
		}
		GameDebuger.OrangeDebugLog(string.Format("End NPC ID:{0} | Name:{1} | 当前匹配项Count:{2} ========",
		                                         npc.id, npc.name, tMissionOptionList.Count));
		return tMissionOptionList;
	}
	#endregion

	#region 遍历已接\可接任务列表，判断当前NPC是否拥有任务相关对话内容，并返回一个列表（DataModel内部对 NPC（功能\师门\帮派） 进行不同处理机制）
	/// <summary>
	/// 遍历已接\可接任务列表，判断当前NPC是否拥有任务相关对话内容，并返回一个列表（DataModel内部对 NPC（功能\师门\帮派） 进行不同处理机制） -- Gets the mission option list by npc I.
	/// </summary>
	/// <returns>The mission option list by npc I.</returns>
	/// <param name="npc">Npc.</param>
	public List<MissionOption> GetMissionOptionListByNpcInternal(Npc npc) {
		List<MissionOption> tMissionOptionList = new List<MissionOption>();
		
		//	查找已接任务列表
		List<Mission> tCurMissionList = GetExistSubMissionMenuList();
		//	查找可接任务列表
		List<Mission> tAceMissionList = GetMissedSubMissionMenuList();
		//	列表合并
		List<Mission> tAllMissionList = tCurMissionList.Concat(tAceMissionList).ToList();

		//	获取玩家门派师傅
		Npc tFactionNpc = GetFactionNpcByVirtualID(1);
		if  (tFactionNpc.id == npc.id) {
			GameDebuger.AquaDebugLog(string.Format("玩家门派师傅名称:{0}", tFactionNpc.name));
			GameDebuger.AquaDebugLog(string.Format("是否存在师门任务：{0}", IsExistBindFactionMission()));
			//	当对话Npc是玩家门派师傅 _> 判断玩家是否绑定师门任务
			if (!IsExistBindFactionMission()) {
				//	当玩家没有绑定存在师门任务
				Dictionary<int, MissionType> tMissionTypeDic = GetMissionTypeDic();

				Mission tMission = new Mission();
				tMission.repeatType = Mission.MissionRepeatType_Faction;
				tMission.type = (int)MissionTypeEnum.Faction;
				tMission.missionType = tMissionTypeDic[tMission.type];
				MissionOption tMissionOption = new MissionOption(tMission, false);

				//	将师门接受任务加入NPC选项列表中
				tMissionOptionList.Add(tMissionOption);
			}
		}

		//	遍历它们判断是否拥有任务相关对话内容
		GameDebuger.OrangeDebugLog("开始遍历已接、可接任务列表，判断项是否拥有任务相关对话内容 =======");
		for (int i = 0, len = tAllMissionList.Count; i < len; i++) {
			bool tIsExist = i < tCurMissionList.Count;
			Npc tNpc = GetMissionNpcByMission(tAllMissionList[i], tIsExist);
			
			if (tNpc != null && npc.id == tNpc.id) {
				MissionOption tMissionOption = new MissionOption(tAllMissionList[i], tIsExist);
				
				//	冥雷怪物
				SubmitDto tSubmitDto = GetSubmitDtoByMission(tAllMissionList[i]);
				if (tIsExist && IsShowMonster(tSubmitDto)) {
					ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;
					if (tShowMonsterSubmitDto.confirm) {
						tMissionOptionList.Add(tMissionOption);
					}
				} else {
					tMissionOptionList.Add(tMissionOption);
				}
				
				GameDebuger.OrangeDebugLog(string.Format("当前匹配项 Type:{0}任务 | ID:{1} | Name:{2} - {3} ----------",
				                                         tMissionOption.optionType? "绑定":"可接",
				                                         tMissionOption.mission.id,
				                                         tMissionOption.mission.missionType.name,
				                                         tMissionOption.mission.name));
			}
		}
		GameDebuger.OrangeDebugLog(string.Format("End NPC ID:{0} | Name:{1} | 当前匹配项Count:{2} ========",
		                                         npc.id, npc.name, tMissionOptionList.Count));
		
		return tMissionOptionList;
	}
	#endregion

	#region 获取当前对话选项模式 return _dialogueOption
	public DialogueOption GetDialogueOptionByMissionOptionList(List<MissionOption> missonOptionList, NpcGeneral npc) {
		//	任务选项数据枚举
		_dialogueOption = DialogueOption.NothingOption;

		int tMissionOptionCount = missonOptionList.Count;
		int tFunctionCount = npc.dialogFunctionId.Count;
		
		//	当NPC绑定功能（既是该NPC挂有多个功能选项 -> 任务选项需要一级分类）
		if (tFunctionCount > 0) {
			//	当NPC绑定任务
			if (tMissionOptionCount > 0) {
				
				if (tMissionOptionCount == 1) {
					//	获取列表第一个任务 和 任务当前目标提交项
					Mission tMission = missonOptionList[0].mission;
					SubmitDto tSubmitDto = GetSubmitDtoByMission(tMission);
					if (tSubmitDto is ShowMonsterSubmitDto) {
						//	当前只有一个任务且任务类型是冥雷闲人战斗任务（变更对话选项为功能和一级菜单方式）=========================
						_dialogueOption = MissionDataModel.DialogueOption.FunctionAndMainMissionMenu;
						
						//CreateDialogueMonsterOption(tMission, npc);
					} else {
						//	判断是否任务数量为1 （变更对话选项为功能和（根据任务类型判断具体显示 一\二 级菜单））====================
						_dialogueOption = MissionDataModel.DialogueOption.FunctionAndSubMissionMenu;
						
						//CreateDialogueOptionByList(tMissonOptionList, tFunctionCount);
					}
				} else if (tMissionOptionCount > 1) {
					//	任务数量大于1（变更对话选项为功能和一级菜单方式）========================================================
					_dialogueOption = MissionDataModel.DialogueOption.FunctionAndMainMissionMenu;
					
					//	这里是要处理的，还没弄好
					//CreateDialogueOptionByList(tMissonOptionList, tFunctionCount);
				}
			} else {
				//	仅有NPC功能选项 不做处理===================================================================================
				_dialogueOption = MissionDataModel.DialogueOption.OnlyFunction;
			}
		} else {
			if (tMissionOptionCount > 0) {
				if (tMissionOptionCount == 1) {
					//	获取列表第一个任务 和 任务当前目标提交项
					MissionOption tMissionOption = missonOptionList[0];
					Mission tMission = missonOptionList[0].mission;
					SubmitDto tSubmitDto = GetSubmitDtoByMission(tMission);
					
					if (tMissionOption.optionType) {
						if (tMission.missionType.submitConfirm) {
							if (tSubmitDto is ShowMonsterSubmitDto) {
								//	当前只有一个任务且任务类型是冥雷闲人战斗任务（变更对话选项为一级菜单方式）=========================
								_dialogueOption = MissionDataModel.DialogueOption.OnlyMonsterMenu;
								
								//	调用冥雷对话方法
								//CreateDialogueMonsterOption(tMission, npc);
								//return;
							} else {
								//	仅生成二级对话选项（既是没有选项，直接提交做改任务逻辑）===============================================
								_dialogueOption = MissionDataModel.DialogueOption.OnlySubMissionMenu;
								
								//CreateDialogueOptionByList(tMissonOptionList, tFunctionCount);
							}
						} else {
							_dialogueOption = MissionDataModel.DialogueOption.OnlyQuicklyMenu;
							if (tSubmitDto is ShowMonsterSubmitDto) {
								ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;
								//SetMonsterSubOption(tMission, npc);
							} else {
								//	任务提交项（明雷任务回调处理）
								/*
								if (FinishTargetSubmitDto(tMission, npc)) {
									ProxyWorldMapModule.CloseNpcDialogue();
								}
								*/
							}
						}
					} else {
						_dialogueOption = MissionDataModel.DialogueOption.FunctionAndMainMissionMenu;
						//	接任务列表不用做这个处理的
						//CreateDialogueOptionByList(tMissonOptionList, tFunctionCount);
					}
				} else {
					//	仅有一级对话选项 ======================================================================================
					_dialogueOption = MissionDataModel.DialogueOption.OnlyMainMissionMenu;
					
					//	这里是要处理的，还没弄好
					//CreateDialogueOptionByList(tMissonOptionList, tFunctionCount);
				}
			} else {
				//	当执行到这里表示没有菜单选项 不做处理=========================================================================
				GameDebuger.OrangeDebugLog(string.Format("{0}", "1）NPC没有功能选项 2）且该时刻没有对应NPC任务绑定 3）"));
				_dialogueOption = MissionDataModel.DialogueOption.NothingOption;
			}
		}
		GameDebuger.AquaDebugLog(string.Format("NPC对话选项分类:{0}", _dialogueOption));

		return _dialogueOption;
	}

	#endregion

	#region 获取Npc一级选项MissionOption
	/// <summary>
	/// 获取Npc一级选项MissionOption -- Gets the main menu option list.
	/// </summary>
	/// <returns>The main menu option list.</returns>
	/// <param name="missionOptionList">Mission option list.</param>
	public List<int> GetMainMenuOptionList(List<MissionOption> missionOptionList) {
		List<int> tMissionMainOptionList = new List<int>();

		for (int i = 0, len = missionOptionList.Count; i < len; i++) {
			MissionOption tMissionOption = missionOptionList[i];
			bool tIsExist = false;

			for (int j = 0, tlen = tMissionMainOptionList.Count; j < tlen; j++) {
				if (tMissionOption.mission.type == tMissionMainOptionList[j]) {
					tIsExist = true;
					break;
				}
			}

			//	当不存在与列表中，把当前TypeID存入列表
			if (!tIsExist) {
				tMissionMainOptionList.Add(tMissionOption.mission.type);
			}
		}

		return tMissionMainOptionList;
	}
	#endregion

	#region 根据MissionTypeID获取任务列表
	/// <summary>
	/// 根据MissionTypeID获取任务列表 -- Gets the mission option list by mission type I.
	/// </summary>
	/// <returns>The mission option list by mission type I.</returns>
	/// <param name="missionOptionList">Mission option list.</param>
	/// <param name="missionTypeID">Mission type I.</param>
	public List<MissionOption> GetMissionOptionListByMissionTypeID(List<MissionOption> missionOptionList, int missionTypeID) {
		List<MissionOption> tMissionOptionList = new List<MissionOption>();

		for (int i = 0, len = missionOptionList.Count; i < len; i++) {
			MissionOption tMissionOption = missionOptionList[i];
			if (tMissionOption.mission.type == missionTypeID) {
				tMissionOptionList.Add(tMissionOption);
			}
		}

		return tMissionOptionList;
	}
	#endregion

	#region 获取提交条件submitCondition对话内容
	/// <summary>
	/// 获取提交条件submitCondition对话内容 -- Gets the mission dialog.
	/// </summary>
	/// <returns>The mission dialog.</returns>
	/// <param name="mission">Mission.</param>
	public MissionDialog GetMissionSubmitDtoDialog(Mission mission) {
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		return tSubmitDto == null? mission.dialog : tSubmitDto.dialog;
	}
	#endregion

	#region 根据当前任务编号获取下一个任务
	private Mission GetPlayerNextMissionByMissionID(int missionID) {
		Mission tMission = null;
		
		PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(missionID);
		if (tPlayerMissionDto != null) {
			tMission = tPlayerMissionDto.mission.next;
		}
		
		return tMission;
	}
	#endregion

	#region 获取下一个任务（如果有的话）接受任务NPC
	private Npc GetNextAceMissionNpcByCurMissionID(int missionID) {
		Npc tNextNpc = null;

		if (IsNextMission(missionID)) {
			Mission tNextMission = GetPlayerNextMissionByMissionID(missionID);
			tNextNpc = GetMissionNpcByMission(tNextMission, false);
		}

		return tNextNpc;
	}
	#endregion

	//	=======================================明雷任务相关=======================================
	#region 获取当前场景ID中存在的明雷怪物Npc
	public List<Npc> GetShowMonsterNpcListBySceneID() {
		List<Npc> tNpcList = new List<Npc>();
		//	获取当前任务列表
		List<Mission> tCurMissionMenuList = GetSubMissionMenuList(true);
		int sceneID = WorldManager.Instance.GetModel().GetSceneId();

		for (int i = 0, len = tCurMissionMenuList.Count; i < len; i++) {
			SubmitDto tSubmitDto = GetSubmitDtoByMission(tCurMissionMenuList[i]);

			//	当前为明雷任务类型
			if (IsShowMonster(tSubmitDto)) {
				Npc tNpc = GetNpcShowMonsterBySubmitDto(tSubmitDto, sceneID);
				if (tNpc != null) {
					tNpcList.Add(tNpc);
				}
			}
		}
		return tNpcList;
	}

	private Npc GetNpcShowMonsterBySubmitDto(SubmitDto tSubmitDto, int sceneID) {
		Npc tNpc = null;
		ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;

		if (sceneID == tShowMonsterSubmitDto.acceptNpc.sceneId) {
			if (tShowMonsterSubmitDto.acceptNpc.npc != null) {
				tNpc = tShowMonsterSubmitDto.acceptNpc.npc;
			}

			if (tShowMonsterSubmitDto.acceptNpc.npcAppearance != null) {
				//tNpc.id *= -1;
				//	变异颜色
				tNpc.modelId = tShowMonsterSubmitDto.acceptNpc.npcAppearance.modelId;
				tNpc.model = tShowMonsterSubmitDto.acceptNpc.npcAppearance.model;
				tNpc.mutateColor = tShowMonsterSubmitDto.acceptNpc.npcAppearance.mutateColor;
				tNpc.mutateTexture = tShowMonsterSubmitDto.acceptNpc.npcAppearance.mutateTexture;
			}
			
			//	变更一下Name的值
			tNpc.name = tShowMonsterSubmitDto.acceptNpc.name;//string.Format(tNpc.name, tShowMonsterSubmitDto.acceptNpc.name);
			//Regex.Replace(tNpc.name, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tShowMonsterSubmitDto.acceptNpc.name));
		}

		return tNpc;
	}
	#endregion

	//	=======================================判断相关=======================================
	#region 判断任务领取条件（只是针对 主线/支线 可接任务才需要这个处理）
	/// <summary>
	/// 判断任务领取条件（只是针对 主线/支线 可接任务才需要这个处理） -- Determines whether this instance is mission accept condition by mission the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is mission accept condition by mission the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	private bool IsMissionAcceptConditionByMission(Mission mission) {
		List<AcceptCondition> tAcceptConditionList = mission.acceptConditions.acceptConditionList;

		for (int i = 0, len = tAcceptConditionList.Count; i < len; i++) {
			if (tAcceptConditionList[i] is AcceptionCondtion_1) {
				AcceptionCondtion_1 tAcceptionCondtion_1 = tAcceptConditionList[i] as AcceptionCondtion_1;
				if (PlayerModel.Instance.GetPlayerLevel() < tAcceptionCondtion_1.grade) {
					return false;
				}
			} else if (tAcceptConditionList[i] is AcceptionCondtion_2) {
				AcceptionCondtion_2 tAcceptionCondtion_2 = tAcceptConditionList[i] as AcceptionCondtion_2;
				if (!IsPreMissionByPreID(tAcceptionCondtion_2.preId)) {
					return false;
				}
			} else {
				//	TADO
			}
		}
		return true;
	}
	#endregion

	#region 判断接受任务前置任务编号（传入当前可接任务前置任务编号）
	private bool IsPreMissionByPreID(int preID) {
		//	遍历当前已提交任务编号，当可接任务前置任务编号在列表中存在（改前置任务已提交完成），则表明该可接任务可以领取。
		if (_playerMissionListDto != null) {
			List<int> tPreIDList = GetPreMissionIDList();
			for (int i = 0, len = tPreIDList.Count; i < len; i++) {
				if (preID == tPreIDList[i]) {
					return true;
				}
			}
		}
		return false;
	}
	#endregion
	
	#region 收集任务玩家是否移动到指定坐标点判断
	private bool IsApplyItemPlayerOnRadius() {
		//	TADO
		return false;
	}
	#endregion
	
	#region	任务服务器开放等级
	/// <summary>
	/// 任务服务器开放等级 -- Determines whether this instance is open menu the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is open menu the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsOpenMenu(Mission mission) {
		return PlayerModel.Instance.ServerGrade >= mission.missionType.functionOpen.grade;
	}
	#endregion

	#region 判断是否存在下一个任务目标(true:存在下一个任务目标 | false:没有下一个任务目标)
	/// <summary>
	/// 断是否存在下一个任务目标(true:存在下一个任务目标 | false:没有下一个任务目标) -- Determines whether this instance is next mission submit the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is next mission submit the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsNextMissionSubmit(Mission mission) {
		return GetSubmitDtoByMission(mission) != null;
	}
	#endregion

	#region 是否存在下一个任务(true:存在下一个任务 | false:没有下一个任务)注：与目标不是同一个
	/// <summary>
	/// 是否存在下一个任务(true:存在下一个任务 | false:没有下一个任务)注：与目标不是同一个 -- Determines whether this instance is next mission the specified missionID.
	/// </summary>
	/// <returns><c>true</c> if this instance is next mission the specified missionID; otherwise, <c>false</c>.</returns>
	/// <param name="missionID">Mission I.</param>
	public bool IsNextMission(int missionID) {
		return GetPlayerNextMissionByMissionID(missionID) != null;
	}
	#endregion
	
	#region 是否存在已接任务
	private bool IsExistCurMission() {
		List<Mission> tCurMissionList = GetExistSubMissionMenuList();
		return tCurMissionList.Count > 0;
	}
	#endregion

	#region 是否存在已接类型任务
	private bool IsExistBindMissionByTypeID(int typeID) {
		List<Mission> tCurMissionList = GetExistSubMissionMenuList();

		for (int i = 0, len = tCurMissionList.Count; i < len; i++) {
			if (tCurMissionList[i].type == typeID) {
				return true;
			}
		}

		return false;
	}
	#endregion

	#region 是否存在已接师门任务
	private bool IsExistBindFactionMission() {
		List<Mission> tCurMissionList = GetExistSubMissionMenuList();

		for (int i = 0, len = tCurMissionList.Count; i < len; i++) {
			if (IsFactionMissionType(tCurMissionList[i])) {
				return true;
			}
		}

		return false;
	}
	#endregion

	#region 是否玩家收集任务
	/// <summary>
	/// 是否玩家收集任务 -- Determines whether this instance is apply item the specified submitDto.
	/// </summary>
	/// <returns><c>true</c> if this instance is apply item the specified submitDto; otherwise, <c>false</c>.</returns>
	/// <param name="submitDto">Submit dto.</param>
	private bool IsApplyItem(SubmitDto submitDto) {
		SubmitDtoType tSubmitDtoType = SubmitDtoType.Nothing;
		if (submitDto != null) {
			tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(submitDto);
		}
		
		return tSubmitDtoType == SubmitDtoType.ApplyItem; 
	}
	#endregion
	
	#region 是否玩家暗雷任务
	/// <summary>
	/// 是否玩家暗雷任务 -- Determines whether this instance is hidden monster the specified submitDto.
	/// </summary>
	/// <returns><c>true</c> if this instance is hidden monster the specified submitDto; otherwise, <c>false</c>.</returns>
	/// <param name="submitDto">Submit dto.</param>
	public bool IsHiddenMonster(SubmitDto submitDto) {
		SubmitDtoType tSubmitDtoType = SubmitDtoType.Nothing;
		if (submitDto != null) {
			tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(submitDto);
		}

		return tSubmitDtoType == SubmitDtoType.HiddenMonster; 
	}
	#endregion
	
	#region 是否玩家明雷任务
	/// <summary>
	/// 是否玩家明雷任务 -- Determines whether this instance is show monster the specified submitDto.
	/// </summary>
	/// <returns><c>true</c> if this instance is show monster the specified submitDto; otherwise, <c>false</c>.</returns>
	/// <param name="submitDto">Submit dto.</param>
	public bool IsShowMonster(SubmitDto submitDto) {
		SubmitDtoType tSubmitDtoType = SubmitDtoType.Nothing;
		if (submitDto != null) {
			tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(submitDto);
		}

		return tSubmitDtoType == SubmitDtoType.ShowMonster; 
	}
	#endregion

	#region 是否主线 \ 支线任务
	/// <summary>
	/// 是否主线 \ 支线任务 -- Determines whether this instance is main or extension the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is main or extension the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsMainOrExtension(Mission mission) {
		return IsMainMissionType(mission) || IsExtensionMissionType(mission);
	}
	#endregion
	
	#region 判断是否主线任务
	/// <summary>
	/// 判断是否主线任务 -- Determines whether this instance is main mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is main mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsMainMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_Once) {
			return mission.type == (int)MissionTypeEnum.Main;
		}
		return false;
	}
	#endregion
	
	#region 判断是否支线任务
	/// <summary>
	/// 判断是否支线任务 -- Determines whether this instance is extension mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is extension mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsExtensionMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_Once) {
			return mission.type == (int)MissionTypeEnum.Extension;
		}
		return false;
	}
	#endregion

	#region 判断是否师门任务
	/// <summary>
	/// 判断是否师门任务 -- Determines whether this instance is faction mission type.
	/// </summary>
	/// <returns><c>true</c> if this instance is faction mission type; otherwise, <c>false</c>.</returns>
	public bool IsFactionMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_Faction) {
			return mission.type == (int)MissionTypeEnum.Faction;
		}
		return false;
	}
	#endregion
	
	#region 判断是否捉鬼任务
	/// <summary>
	/// 判断是否捉鬼任务 -- Determines whether this instance is buffy mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is buffy mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsBuffyMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_Ghost) {
			return mission.type == (int)MissionTypeEnum.Buffy;
		}
		return false;
	}
	#endregion
	
	#region 判断是否宝图任务
	/// <summary>
	/// 判断是否宝图任务 -- Determines whether this instance is treasure mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is treasure mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsTreasureMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.Treasure;
		}
		return false;
	}
	#endregion
	
	#region 判断是否任务链任务
	/// <summary>
	/// 判断是否任务链任务 -- Determines whether this instance is task chain mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is task chain mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsTaskChainMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.TaskChain;
		}
		return false;
	}
	#endregion
	
	#region 判断是否帮派任务
	/// <summary>
	/// 判断是否帮派任务 -- Determines whether this instance is guild mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is faction mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsGuildMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.Guild;
		}
		return false;
	}
	#endregion
	
	#region 判断是否丝绸之路任务
	/// <summary>
	/// 判断是否丝绸之路任务 -- Determines whether this instance is silk mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is silk mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsSilkMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.Silk;
		}
		return false;
	}
	#endregion
	
	#region 判断是否英雄试炼任务
	/// <summary>
	/// 判断是否英雄试炼任务 -- Determines whether this instance is hero mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is hero mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsHeroMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.Hero;
		}
		return false;
	}
	#endregion
	
	#region 判断是否活动任务
	/// <summary>
	/// 判断是否活动任务 -- Determines whether this instance is holiday mission type the specified mission.
	/// </summary>
	/// <returns><c>true</c> if this instance is holiday mission type the specified mission; otherwise, <c>false</c>.</returns>
	/// <param name="mission">Mission.</param>
	public bool IsHolidayMissionType(Mission mission) {
		if (mission.repeatType == Mission.MissionRepeatType_None) {
			return mission.type == (int)MissionTypeEnum.Holiday;
		}
		return false;
	}
	#endregion

	//	========================================剧情相关=======================================
	#region 剧情播放
	private System.Action<PlayerMissionDto> _storyPlotCallback = null;
	/// <summary>
	/// missionID:任务ID | acceptAndSubmit:是否接任务（true：是 false：否） callback:回调 -- Stories the plot in mission.
	/// </summary>
	/// <param name="missionID">Mission I.</param>
	/// <param name="acceptAndSubmit">If set to <c>true</c> accept and submit.</param>
	/// <param name="callback">Callback.</param>
	private void StoryPlotInMission(int missionID, bool acceptAndSubmit, System.Action<PlayerMissionDto> callback = null) {
		_storyPlotCallback = callback;

		GameDebuger.OrangeDebugLog(string.Format(" {0} 任务 ID:{1} | 任务系统开始调用剧情播放统一接口",
		                                         acceptAndSubmit? "接受" : "提交", missionID));
		MissionPlotModel.Instance.StoryPlotInPlotModel(missionID, acceptAndSubmit, StoryEndPlotCallback);
	}

	//	剧情系统播放完成后调用任务系统
	public void StoryEndPlotCallback(PlayerMissionDto playerMissionDto) {
		if (playerMissionDto == null) {
			GameDebuger.AquaDebugLog(string.Format("剧情播放结束后回调该信息| 内容：剧情播放结束后没有新的任务"));
			return;
		}

		//	加入列表并进行界面刷新
		AcceptMissionFinish(playerMissionDto);
		
		//	剧情播放完啦，回调任务继续执行
		if (_storyPlotCallback != null)  {
			_storyPlotCallback(playerMissionDto);
		}
	}
	#endregion

	//	========================================请求相关=======================================
	#region	获取任务列表
	/// <summary>
	/// 请求获取任务列表 -- Enters the mission.
	/// </summary>
	public void EnterMission() {
		GameDebuger.OrangeDebugLog(string.Format("开始获取任务列表"));

		ServiceRequestAction.requestServer(MissionService.enter(), "enter mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog(string.Format("获取任务列表成功"));

			//	PlayerMissionListDto
			if (e != null) {
				_playerMissionListDto = e as PlayerMissionListDto;
				foreach (PlayerMissionDto tPlayerMissionDto in _playerMissionListDto.bodyMissions) {
					GameDebuger.AquaDebugLog(string.Format("任务类型:{0}", tPlayerMissionDto.GetType().ToString().Split('.')[8]));
				}
			}
		});
	}
	#endregion

	#region	请求 主线\支线 任务
	/// <summary>
	/// 请求 主线\支线 任务(missionID) -- Accepts the mission.
	/// </summary>
	/// <param name="missionID">Mission identifier.</param>
	public void AcceptMission(int missionID) {
		GameDebuger.OrangeDebugLog(string.Format("开始接受任务 | Parameters -> missionID:{0}", missionID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.accept(missionID), "accept mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("接受任务成功");
			//	添加入本地数据
			PlayerMissionDto tPlayerMissionDto = e as PlayerMissionDto;

			if (tPlayerMissionDto == null) {
				GameDebuger.AquaDebugLog("请求任务返回空（PlayerMissionDto）为空：");
				return;
			}

			AcceptMissionFinish(tPlayerMissionDto);
		});
	}
	#endregion
	
	#region 请求师门任务
	/// <summary>
	/// 请求师门任务 -- Accepts the faction mission.
	/// </summary>
	public void AcceptFactionMission() {
		GameDebuger.OrangeDebugLog(string.Format("开始请求师门任务"));
		if (!_resquestServer) return;
		
		ServiceRequestAction.requestServer(MissionService.acceptFaction(), "accept faction mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog(string.Format("请求师门任务成功"));
			
			//	添加入本地数据
			PlayerFactionMissionDto tPlayerFactionMissionDto = e as PlayerFactionMissionDto;
			if (tPlayerFactionMissionDto == null) {
				GameDebuger.AquaDebugLog(string.Format("请求师门任务返回空"));
				return;
			}

			AcceptMissionFinish(tPlayerFactionMissionDto);
		});
	}
	#endregion

	#region 接受任务内部处理
	private System.Action _initShowMonsterCallback = null;
	private void AcceptMissionFinish(PlayerMissionDto tPlayerMissionDto) {
		GameDebuger.OrangeDebugLog(string.Format("获得新的任务ID:{0} Name:{1}-{2} | 任务类型:{3}",
		                                         tPlayerMissionDto.missionId,
		                                         tPlayerMissionDto.mission.missionType.name, tPlayerMissionDto.mission.name,
		                                         tPlayerMissionDto.GetType().ToString().Split('.')[8]));

		//	查看是否已存在当前任务列表中
		for (int i = 0, len = _playerMissionListDto.bodyMissions.Count; i < len; i++) {
			if (_playerMissionListDto.bodyMissions[i].missionId == tPlayerMissionDto.missionId) {
				//	已存在任务列表中的任务直接返回
				return;
			}
		}

		//	添加入本地任务数据列表并进行界面刷新
		_playerMissionListDto.bodyMissions.Add(tPlayerMissionDto);
		
		//	Refresh Pantl Callback
		if (acceptCallback != null) {
			acceptCallback(tPlayerMissionDto);
		}

		//	判断是否明雷任务（true(是):生成明雷任务怪  false(否):~|~）
		//	对应的会有明雷任务放弃（true:删除任务怪物）
		Mission tMission = tPlayerMissionDto.mission;

		//	判断是否还在战斗中状态
		if (BattleManager.Instance.IsInBattle()) {
			if (_showMonsterNpc != null) {
				//	_showMonsterNpc表示明雷战斗中，等待战斗结束才生成明雷NPC \ 显示对话内容操作
			} else {
				//BattleManager.Instance.OnBattleFinishCallback += ShowMonsterBattleFinishCallback;
				//	正在进行暗雷战斗任务 等待战斗结束才生成明雷NPC \ 显示对话内容操作
			}

			_initShowMonsterCallback = () => {
				SetAcceMissionQuickFindDialogue(tMission);
			};
		} else {
			//	延迟对话内容显示操作
			SetAcceMissionQuickFindDialogue(tMission);
		}
		
		//	面板刷新
		RefreshMissionPanelCallback();
		
		//	任务剧情
		StoryPlotInMission(tMission.id, true);
		
		//	寻路(具体需求看是否需要在这里做统一处理，还是做单个处理 TADO)
		if (IsTreasureMissionType(tMission)/*false 判断是否宝图任务*/) {
			WaitTimeFindToMission(tMission, true);
		}
	}
	
	private void SetAcceMissionQuickFindDialogue(Mission mission) {
		Npc tNpc = null;
		
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		if (IsShowMonster(tSubmitDto)) {
			//	生成明雷任务怪
			int sceneID = WorldManager.Instance.GetModel().GetSceneId();
			tNpc = GetNpcShowMonsterBySubmitDto(tSubmitDto, sceneID);

			if (tNpc != null) {
				//	延迟生成冥雷怪
				CoolDownManager.Instance.SetupCoolDown("__initShowMonster", _findMissionWaitTime, null, () => {
					WorldManager.Instance.GetNpcViewManager().InitShowMonster(tNpc);
				});
			}
		}

		//	当需要自动寻路 && 有提交项且拥有唯一提交项的时候便捷寻路
		if (mission.quickFindWay && mission.submitConditions.submitConditionArray.Count == 1) {
			ProxyWorldMapModule.OpenNpcDialogueInAcceptMission(mission);
		}
	}
	#endregion

	#region	手动提交某任务目标至完成状态，有可能触发奖励（这里指任务中的某个目标）
	/// <summary>
	/// 手动提交某任务目标至完成状态，有可能触发奖励(MissionID) -- Finishs the mission.
	/// </summary>
	/// <param name="missionID">Mission identifier.</param>
	public void FinishMission(int missionID) {
		GameDebuger.OrangeDebugLog(string.Format("开始提交任务目标 | Parameters -> missionID:{0}", missionID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.finish(missionID), "finish Mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("提交任务目标完成");

			//	Refresh Pantl Callback
			if (finishCallback != null) {
				finishCallback();
			}
			if (e != null && e is SubmitRewardsDto) {
				SubmitMissionCallback(e as SubmitRewardsDto, missionID);
			}/* else {
				Mission tMission = GetCurrentMissionByMissionID(missionID);
				if (tMission != null) {
					FindToMissionNpcByMission(tMission, true);
				}
			}*/

			//	注意时序问题
			RefreshMissionPanelCallback();
		});
	}
	#endregion

	#region	提交任务（这里是指一个任务）
	/// <summary>
	/// 提交任务(MissionID) -- Submits the mission.
	/// </summary>
	/// <param name="missionID">Mission identifier.</param>
	public void SubmitMission(int missionID) {
		GameDebuger.OrangeDebugLog(string.Format("开始提交任务 | Parameters -> missionID:{0}", missionID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.submit(missionID), "submit mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("提交任务成功");
			
			//	Refresh Pantl Callback
			SubmitMissionCallback(e as SubmitRewardsDto, missionID);
			
			//	从本地数据中删除相关信息(注意时序问题:submitCallback中会关闭对话面板)
			//	RemoveMissionDto中已经包含了（RefreshMissionPanelCallback()操作）
			RemovePlayerMissionDtoByMissionID(missionID);
		});
	}
	#endregion

	#region	放弃任务
	/// <summary>
	/// 放弃任务(mission) -- Drops the mission.
	/// </summary>
	/// <param name="mission">Mission.</param>
	/// <param name="callback">Callback.</param>
	public void DropMissionInController(Mission mission) {
		if (IsMainOrExtension(mission)) {
			TipManager.AddTip(string.Format("不能放弃{0}任务", mission.missionType.name));
			return;
		} else if (IsFactionMissionType(mission)) {
			ProxyWindowModule.OpenConfirmWindow(string.Format("是否放弃{0}-{1}？", mission.missionType.name, mission.name),
			                                    string.Format("{0}-{1}", mission.missionType.name, mission.name),
			                                    () => {
				DropMission(mission.id);
			});
		} else {
			DropMission(mission.id);
		}
	}

	private void DropMission(int missionID) {
		GameDebuger.OrangeDebugLog(string.Format("开始放弃任务 | Parameters -> missionID:{0}", missionID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.drop(missionID), "drop Mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("放弃任务完成");
			//	从本地数据中删除相关信息
			RemovePlayerMissionDtoByMissionID(missionID, true);

			//	Refresh Pantl Callback
			if (dropCallback != null) {
				dropCallback(e as AcceptGiftsDto);
			}
		});
	}
	#endregion

	#region	对话npc
	/// <summary>
	/// 对话npc(MissionID) -- Talks the mission.
	/// </summary>
	/// <param name="missionID">Mission identifier.</param>
	public void TalkMission(int npcID, System.Action callback) {
		GameDebuger.OrangeDebugLog(string.Format("开始对话NPC | Parameters -> missionID:{0}", npcID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.talk(npcID), "talk Mission", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("对话NPC完成");
			
			//	Refresh Pantl Callback
			if (callback != null) {
				callback();
			}

			if (talkCallback != null) {
				talkCallback();
			}
			RefreshMissionPanelCallback();
		});
	}
	#endregion

	#region 请求明雷打怪任务
	/// <summary>
	/// 请求明雷打怪任务 -- Battles the show monster.
	/// </summary>
	/// <param name="missionID">Mission I.</param>
	/// <param name="npc">Npc.</param>
	/// <param name="callback">Callback.</param>
	public void BattleShowMonster(int missionID, Npc npc, System.Action callback) {
		GameDebuger.OrangeDebugLog(string.Format("开始冥雷任务 | Parameters -> missionID:{0}", missionID));
		if (!_resquestServer) return;

		ServiceRequestAction.requestServer(MissionService.battle(missionID), "battle show monster", delegate(GeneralResponse e) {
			GameDebuger.OrangeDebugLog("请求冥雷任务成功");

			_showMonsterNpc = npc;

			BattleManager.Instance.OnBattleFinishCallback += ShowMonsterBattleFinishCallback;
			if (callback != null) {
				callback();
			}
		});
	}

	private Npc _showMonsterNpc = null;
	private void ShowMonsterBattleFinishCallback(bool winOrLoseSta) {
		BattleManager.Instance.OnBattleFinishCallback -= ShowMonsterBattleFinishCallback;

		//	判断输赢
		if (winOrLoseSta) {
			//	删除实例的冥雷NPC对象
			if (_showMonsterNpc != null) {
				WorldManager.Instance.GetNpcViewManager().DesShowMonster(_showMonsterNpc);
				_showMonsterNpc = null;
			}

			if (_initShowMonsterCallback != null) {
				_initShowMonsterCallback();
				_initShowMonsterCallback = null;
			}
		}
	}
	#endregion

	//	======================================== 数据操作相关 =======================================
	#region 从本地数据中删除相关信息
	/// <summary>
	/// 从本地数据中删除相关信息(移除任务ID，是否主动放弃的任务) -- Removes the player mission dto by mission I.
	/// </summary>
	/// <returns><c>true</c>, if player mission dto by mission I was removed, <c>false</c> otherwise.</returns>
	/// <param name="missionID">Mission I.</param>
	/// <param name="isDropMission">If set to <c>true</c> is drop mission.</param>
	private bool RemovePlayerMissionDtoByMissionID(int missionID, bool isDropMission = false) {
		bool b = false;
		Mission tNextMission = null;

		PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(missionID);
		if (tPlayerMissionDto != null) {
			//	注意时序问题：需要先加入已完成列表，在检查看是否有下一个任务，才去做删除当前任务操作
			
			//	加入已提交列表中
			GetPlayerMissionListDto().submitMissionIds.Add(missionID);

			//	是否放弃任务
			if (!isDropMission) {
				//	任务剧情
				StoryPlotInMission(missionID, false);
				
				//	是否有下一个任务(true:存在 -> 接取改任务 false：不存在 -> 不做处理)
				if (IsNextMission(missionID)) {
					Mission __tNextMission = GetPlayerNextMissionByMissionID(missionID);
					//	这里需要判断是否前置任务已完成，但改任务其实还未完成，需要手动添加该任务ID入列表中（上面已进行操作）
					if (IsMissionAcceptConditionByMission(__tNextMission)) {
						tNextMission = __tNextMission;
					}
				}

				//	是否师门且第十环
				if (tPlayerMissionDto is PlayerFactionMissionDto && IsFactionMissionType(tPlayerMissionDto.mission)) {
					PlayerFactionMissionDto tPlayerFactionMissionDto = tPlayerMissionDto as PlayerFactionMissionDto;
					
					//	判断是否还在战斗中状态
					if (BattleManager.Instance.IsInBattle()) {
						_initShowMonsterCallback = () => {
							FindToMasterNpc(tPlayerFactionMissionDto);
						};
					} else {
						FindToMasterNpc(tPlayerFactionMissionDto);
					}
				}
			}

			//	从当前绑定任务中删除信息
			_playerMissionListDto.bodyMissions.Remove(tPlayerMissionDto);

			b = true;
		}

		//	注意时序问题，需要先删除上一个任务，才去做下一个任务接取
		if (tNextMission != null) {
			if(IsTreasureMissionType(tNextMission)/*autoPath 判断是否宝图任务*/) {
				//	寻路到指定NPC处接取任务
				WaitTimeFindToMission(tNextMission, false);
				//FindToMissionNpcByMission(tNextMission, false);
			} else {
				//	直接接取下一个任务(请求服务器接口)
				//	这里服务端做了处理，下发下一个任务 By PlayerMissionNotify
				//AcceptMission(tNextMission.id);
			}
		}

		RefreshMissionPanelCallback();

		return b;
	}

	private void FindToMasterNpc(PlayerFactionMissionDto playerFactionMissionDto) {
		//	当前Remoce任务数量加一
		if (++playerFactionMissionDto.dailyFinishCount == /*20*/2 * _missionRingNum) {
			ProxyWindowModule.OpenConfirmWindow(_endDailyFinishStr, playerFactionMissionDto.mission.missionType.name, () => {
				FindToMasterOrChief(1);
			});
		} else if (playerFactionMissionDto.curRings == _missionRingNum) {
			ProxyWorldMapModule.OpenCommonDialogue(PlayerModel.Instance.GetPlayerName(),
			                                       _endRingsFinishStr,
			                                       new List<string>() {"回师门"}, (selectIndex) => {
				FindToMasterOrChief(1);
			});
		}
	}
	#endregion
	
	//	======================================== Notify 相关 =======================================
	#region 接受任务状态通知更新
	/// <summary>
	/// 接受任务状态通知更新 -- Updatas the player mission notify.
	/// </summary>
	/// <param name="notify">Notify.</param>
	public void UpdataPlayerMissionNotify(PlayerMissionNotify notify) {
		GameDebuger.OrangeDebugLog(string.Format("======  接受任务状态通知  =====\nMissionID：{0}\nMissionType:{1}",
		                                         notify.playerMissionDto.missionId,
		                                         notify.playerMissionDto.GetType().ToString().Split('.')[8]));
		
		//GameDebuger.OrangeDebugLog(string.Format("获取任务提交项类型 NULL -> 提交项信息（ID：{0} | 完成状态：{1}） | SubmitDto类型:{2}",
		//                                         submitDto.id, submitDto.finish, submitDto.GetType().ToString().Split('.')[8]));
		if (notify.playerMissionDto == null) {
			GameDebuger.AquaDebugLog(string.Format("接受任务状态通知更新返回空"));
			return;
		}

		AcceptMissionFinish(notify.playerMissionDto);
	}
	#endregion

	#region 更新任务提交项状态状态
	/// <summary>
	/// 更新任务提交项状态 -- Updatas the submit dto.
	/// </summary>
	/// <param name="notify">Notify.</param>
	public void UpdateSubmitDtoByStateNotify(MissionSubmitStateNotify notify){
		SubmitDto tSubmitDto = GetSubmitDtoByMission(notify.mission);
		GameDebuger.OrangeDebugLog(string.Format("======  服务端下发任务目标提交项状态变更通知  =====\nMissionID：{0} | SubmitID：{1} | Finish：{2}",
		                                         notify.missionId, notify.submitConditionId, notify.finish));

		if (tSubmitDto != null && tSubmitDto.id == notify.submitConditionId) {
			GameDebuger.OrangeDebugLog("提交项通知编号与本地编号一致");
		} else {
			int tCount = 0;
			List<SubmitDto> tSubmitDtoList = GetSubmitDtoListByMission(notify.mission);
			for (int i = 0, len = tSubmitDtoList.Count; i < len; i++) {
				tCount++;
				if (tSubmitDtoList[i].id == notify.submitConditionId) {
					tSubmitDto = tSubmitDtoList[i];
					break;
				}
			}
			GameDebuger.OrangeDebugLog(string.Format("遍历当前任务，{0}找到与通知编号一致的提交项", tCount < tSubmitDtoList.Count? "" : "未"));
		}

		//	重置采集任务人物角色移动回调
		if (_heroView != null) {
			_heroView.checkMissioinCallback = null;
		}

		//	是否finish = true
		tSubmitDto.count = notify.count;
		tSubmitDto.finish = notify.finish;

		if (notify.count >= tSubmitDto.needCount) {
			SubmitDtoType tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(tSubmitDto);
			if (tSubmitDtoType == SubmitDtoType.CollectionItem) {
				//	当任务类型是收集物品，关闭购买道具窗口
				ProxyShopModule.Close();
				ProxyTradeModule.Close();
			} else if (tSubmitDtoType == SubmitDtoType.CollectionPet) {
				//	当任务类型是收集宠物，关闭购买宠物窗口
				ProxyTradePetModule.Close();
			} else if (tSubmitDtoType == SubmitDtoType.HiddenMonster) {
				//	当任务类型是暗雷战斗，关闭场景巡逻
				PlayerModel.Instance.StopAutoFram();
			}
		}

		//	是否需要完成下一个任务目标（true（是）：刷新面板 | false（否）：提交任务）
		//	提交Finish成功，结束当前该目标 -> 判断是否存在下一个目标（true：执行下一个目标 false：根据是否自动提交执行）
		if (notify.finish && !IsNextMissionSubmit(notify.mission) && notify.mission.autoSubmit) {
			//	当自动提交的任务 删除该任务
			RemovePlayerMissionDtoByMissionID(notify.mission.id);
		} else{
			if (!_heroCharacterControllerEnable) {
				Mission tMission = GetCurrentMissionByMissionID(notify.missionId);
				if (tMission != null) {
					if (IsTreasureMissionType(tMission)/*autoPath 判断是否宝图任务*/) {
						WaitTimeFindToMission(tMission, true);
						//FindToMissionNpcByMission(tMission, true);
					}
				}
			}

			RefreshMissionPanelCallback();
		}
	}
	#endregion

	#region 收集宠物项删除宠物通知收集宠物项删除宠物通知
	/// <summary>
	/// 收集宠物项删除宠物通知收集宠物项删除宠物通知 -- Deletes the pet by state notify.
	/// </summary>
	/// <param name="notify">Notify.</param>
	public void DeletePetByStateNotify(CollectionPetSubmitNotify notify) {
		//	删除宠物项通知 -> 根据 UID 处理
		PetModel.Instance.RemovePetByUID(notify.petUniqueId);
	}
	#endregion

	#region 变更玩家角色传送阵触发开关
	/// <summary>
	/// 变更玩家角色传送阵触发开关 -- Heros the character controller enable.
	/// </summary>
	/// <param name="enableState">If set to <c>true</c> enable state.</param>
	public void HeroCharacterControllerEnable(bool enableState) {
		_heroCharacterControllerEnable = enableState;
		WorldManager.Instance.GetHeroCharacterController().enabled = enableState;
	}
	#endregion

	//	========================================静态方法=======================================
	public static Vector3 GetSceneStandPosition(Vector3 position, Vector3 defaultPosition) {
		Vector3 newPosition;
		
		RaycastHit hit;
		Ray ray = new Ray(new Vector3(position.x, 200, position.z), new Vector3(0, -1, 0));
		newPosition = Physics.Raycast(ray, out hit, 250, 1 << LayerMask.NameToLayer("Terrain"))?
			hit.point : defaultPosition.Equals(Vector3.zero)?
				new Vector3(position.x, position.y, position.z) : GetSceneStandPosition(defaultPosition, Vector3.zero);
		
		return newPosition;
	}

	//	========================================信息处理=======================================
	private enum MsgType {
		Nothing = 0,
		TargetDesc = 1,
		Description = 2,
		DialogueFirst = 3
	}
	//private MsgType _msgType = MsgType.Nothing;

	#region Color
	private string _defaultColor = "[6f3e1a]";

	//	PanelTitle
	private string _orange = "[d4862f]";

	//	Target
	private string _yellowColor = "[ffdd7b]";
	private string _whiteColor = "[fff9e3]";
	
	//	Description
	private string _greenColor = "[2beb54]";
	private string _blueColor = "[0c99c7]";

	//	MissionCell Color
	private string _mainColor = "[a748ff]";
	private string _threadColor = "[d44e0c]";
	private string _timeColor = "[e61a1a]";
	#endregion

	#region 已接任务目标信息文字转换
	/// <summary>
	/// 已接任务目标信息文字转换 -- Gets the content of the current target.
	/// </summary>
	/// <returns>The current target content.</returns>
	/// <param name="mission">Mission.</param>
	/// <param name="isMissionCellTarget">If set to <c>true</c> is mission cell target.</param>
	public string GetCurTargetContent(Mission mission, bool isMissionCellTarget) {
		string tStr = GetCurrentMissionStr(mission, MsgType.TargetDesc);
		if (tStr == "") {
			tStr = string.Format("{0}", mission.submitNpc == null?
			                     string.Format("{0}{1}-{2}[-]完成", _greenColor, mission.missionType.name, mission.name)
			                     : string.Format("回复{0}[-]", GetNpcNameByNpc(mission.submitNpc, true, isMissionCellTarget)));
		}

		return GetBold(string.Format("{0}{1}[-]", isMissionCellTarget? _whiteColor : _defaultColor, tStr), false/*isMissionCellTarget*/);
	}
	#endregion

	#region 已接任务详细内容文字转换
	/// <summary>
	/// 已接任务详细内容文字转换 -- Gets the current description conteny.
	/// </summary>
	/// <returns>The current description conteny.</returns>
	/// <param name="mission">Mission.</param>
	public string GetCurDescriptionContent(Mission mission) {
		string tStr = GetCurrentMissionStr(mission, MsgType.Description);
		return string.Format("{0}{1}[-]", _defaultColor, tStr);
		//return GetBold(string.Format("{0}{1}[-]", _defaultColor, tStr));
	}
	#endregion

	#region 对话内容详细文字转换
	/// <summary>
	/// 对话内容详细文字转换 -- Gets the content of the accept dialogue first.
	/// </summary>
	/// <returns>The accept dialogue first content.</returns>
	/// <param name="mission">Mission.</param>
	public string GetAcceptDialogueFirstContent(Mission mission) {
		string tStr = GetCurrentMissionStr(mission, MsgType.DialogueFirst);
		return string.Format("{0}{1}[-]", _whiteColor, tStr);
		//return GetBold(string.Format("{0}{1}[-]", _whiteColor, tStr));
	}
	#endregion
	
	#region MissionCell Title
	/// <summary>
	/// 已接任务Title -- Gets the name of the mission title.
	/// </summary>
	/// <returns>The mission title name.</returns>
	/// <param name="mission">Mission.</param>
	/// <param name="isMissionCellTitle">If set to <c>true</c> is mission cell title.</param>
	public string GetMissionTitleName(Mission mission, bool isExist, bool isMissionCellTitle) {
		string tStr = string.Format("{0}{1}{2}[-]", isMissionCellTitle? IsMainMissionType(mission)? _mainColor : _threadColor : _orange,
		                            //: IsExistCurMission(mission)? _threadColor : "" : "",
		                            mission.missionType.name,
		                            IsMainOrExtension(mission) || (IsFactionMissionType(mission) && isExist)? string.Format("-{0}", mission.name) : "");
		
		if (IsFactionMissionType(mission) && isExist) {
			PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(mission.id);
			if (tPlayerMissionDto is PlayerFactionMissionDto) {
				PlayerFactionMissionDto tPlayerFactionMissionDto = tPlayerMissionDto as PlayerFactionMissionDto;
				tStr = string.Format("{0}{1}({2}/{3})[-]", tStr, isMissionCellTitle? _threadColor : _orange,
				                     tPlayerFactionMissionDto.curRings, _missionRingNum);
			}
		}

		return GetBold(tStr, /*isMissionCellTitle*/false);
	}

	public string GetMissionTitleNameInDialogue(Mission mission, bool isExist) {
		string tType = "";
		string tName = "";

		if (IsMainOrExtension(mission)) {

		} else {
			tType = mission.missionType.name;
			if (IsFactionMissionType(mission)) {
				tType += "任务";
			}
		}

		if (isExist) {
			if (IsFactionMissionType(mission)) {
				PlayerMissionDto tPlayerMissionDto = GetPlayerMissionDtoByMissionID(mission.id);
				if (tPlayerMissionDto is PlayerFactionMissionDto) {
					PlayerFactionMissionDto tPlayerFactionMissionDto = tPlayerMissionDto as PlayerFactionMissionDto;
					tName = string.Format("-{0}({1}/{2})", mission.name, tPlayerFactionMissionDto.curRings, _missionRingNum);
				}
			}
		} else {
			if (!IsFactionMissionType(mission)) {
				tName = string.Format("-{0}", mission.name);
			}
		}

		return string.Format("{0}{1}{2}[-]", _whiteColor, tType, tName);
	}
	#endregion

	#region 任务NPC名称
	/// <summary>
	/// 任务NPC名称 -- Gets the name of the misssion npc.
	/// </summary>
	/// <returns>The misssion npc name.</returns>
	/// <param name="mission">Mission.</param>
	/// <param name="isExist">If set to <c>true</c> is exist.</param>
	/// <param name="needScene">If set to <c>true</c> is need scene.</param>
	public string GetMisssionNpcName(Mission mission, bool isExist, bool needScene) {
		Npc tNpc = GetMissionNpcByMission(mission, isExist);

		//	当改NPC是虚拟NPC是转换为具体NPC
		if (tNpc is FactionNpc) {
			tNpc = NpcVirturlToEntity(tNpc);
		}

		if (tNpc == null) {
			return string.Format("[b][ff0000]请检查MissionID：{0} | Name:{1}-{2} ->\n提交任务NPC是否填写正确!!![-]",
			                     mission.id, mission.missionType.name, mission.name);
		}

		return GetNpcNameByNpc(tNpc, needScene, false);
	}
	#endregion

	#region 获取NPC名称
	private string GetNpcNameByNpc(Npc npc, bool needScene, bool isMissionCell) {
		string tStr = string.Format("{0}{1}[-]", _blueColor, npc.name);
		if (needScene) {
			tStr = string.Format("{0}{1}[-]{2}的[-]{3}[-]", _greenColor, DataCache.getDtoByCls<SceneMap>(npc.sceneId).name,
			                     isMissionCell? _whiteColor : _defaultColor, tStr);
		}
		return tStr;
	}

	private string GetNpcNameByNpc(ShowMonsterSubmitDto showMonsterSubmitDto, bool needScene, bool isMissionCell) {
		string tStr = string.Format("{0}{1}[-]", _blueColor, showMonsterSubmitDto.acceptNpc.npc.name);
		if (needScene) {
			tStr = string.Format("{0}{1}[-]{2}的[-]{3}[-]",
			                     _greenColor, showMonsterSubmitDto.acceptNpc.scene.name,
			                     isMissionCell? _whiteColor : _defaultColor, tStr);
		}
		return tStr;
	}
	#endregion
	
	#region 宠物名称
	public string GetCurPetName(Mission mission) {
		return "";
	}
	#endregion

	#region 任务信息转换文字
	public string GetReplyNpcName(Mission mission) {
		return "";
	}
	#endregion

	#region 具体转换文字逻辑
	//	当前任务文字转换逻辑
	private string GetCurrentMissionStr(Mission mission, MsgType msgType) {
		SubmitDto tSubmitDto = GetSubmitDtoByMission(mission);
		MissionDialog tCurMissionDialog = GetMissionSubmitDtoDialog(mission);
		SubmitDtoType tSubmitDtoType = GetSubmitDtoTypeBySubmitDto(tSubmitDto);

		string tStr = msgType == MsgType.TargetDesc?
			tCurMissionDialog.goalDesc : msgType == MsgType.Description?
				tCurMissionDialog.description : msgType == MsgType.DialogueFirst?
				// 便捷寻路窗口（使用任务对话，非目标对话）接受任务对话第一条
				mission.dialog.acceptNpc.Count > 0? mission.dialog.acceptNpc[0]
				: string.Format("NULL -> 请策划人员查看:对话DiaID:{0} | 提交项SubID:{1} | 任务MisID:{2}",
				                tCurMissionDialog.id, tSubmitDto.id, mission.id)/*tCurMissionDialog.description*/ : "";

		if (tStr == string.Empty) return tStr;

		switch (tSubmitDtoType) {
		case SubmitDtoType.Nothing:
			tStr = "ERROR:SubmitDtoType.Nothing";
			break;
		case SubmitDtoType.Talk:
			TalkSubmitDto tTalkSubmitDto = tSubmitDto as TalkSubmitDto;

			if (msgType == MsgType.TargetDesc) {
				//tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tTalkSubmitDto.submitNpc.name));
			} else if (msgType == MsgType.Description) {
				tStr = Regex.Replace(tStr, "{submitscene}", string.Format("{0}{1}[-]", _greenColor,
				                                                          DataCache.getDtoByCls<SceneMap>(tTalkSubmitDto.submitNpc.sceneId).name));
				//tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tTalkSubmitDto.submitNpc.name));
			}
			tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tTalkSubmitDto.submitNpc.name));

			break;
		case SubmitDtoType.ApplyItem:
			ApplyItemSubmitDto tApplyItemSubmitDto = tSubmitDto as ApplyItemSubmitDto;

			if (msgType == MsgType.TargetDesc) {
				//	TADO
			} else if (msgType == MsgType.Description) {
				//	TADO
			}

			//	SceneID = 0 / tApplyItemSubmitDto.submitNpc.scene == null
			tStr = Regex.Replace(tStr, "{scene}", tApplyItemSubmitDto.acceptScene.sceneMap == null?
			                     string.Format("场景ID错误:{0}", tApplyItemSubmitDto.acceptScene.id)
			                     : string.Format("{0}{1}[-]", _greenColor, tApplyItemSubmitDto.acceptScene.sceneMap.name));
			//	ItemID = 0 / tApplyItemSubmitDto.item == null 表示前往某地 | 否者表示使用某物采集某物
			tStr = Regex.Replace(tStr, "{item}", tApplyItemSubmitDto.item == null?
			                     string.Format("物品ID错误:{0}", tApplyItemSubmitDto.itemId)
			                     : string.Format("{0}{1}[-]", _greenColor, tApplyItemSubmitDto.item.name));
			break;
		case SubmitDtoType.CollectionItem:
			CollectionItemSubmitDto tCollectionItemSubmitDto = tSubmitDto as CollectionItemSubmitDto;
			if (msgType == MsgType.TargetDesc) {
				//tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
				//tStr = Regex.Replace(tStr, "{item}", string.Format("{0}{1}[-]", _greenColor, tCollectionItemSubmitDto.item.name));
			} else if (msgType == MsgType.Description) {
				tStr = Regex.Replace(tStr, "{submitscene}", string.Format("{0}{1}[-]", _greenColor, tCollectionItemSubmitDto.submitNpc.scene.name));
				tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tCollectionItemSubmitDto.submitNpc.name));
				//tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
				//tStr = Regex.Replace(tStr, "{item}", string.Format("{0}{1}[-]", _greenColor, tCollectionItemSubmitDto.item.name));
			}
			tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
			tStr = Regex.Replace(tStr, "{item}", string.Format("{0}{1}[-]", _greenColor, tCollectionItemSubmitDto.item.name));
			break;
		case SubmitDtoType.CollectionPet:
			CollectionPetSubmitDto tCollectionPetSubmitDto = tSubmitDto as CollectionPetSubmitDto;
			if (msgType == MsgType.TargetDesc) {
				//tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
				//tStr = Regex.Replace(tStr, "{pet}", string.Format("{0}{1}[-]", _greenColor, tCollectionPetSubmitDto.pet.name));
			} else if (msgType == MsgType.Description) {
				tStr = Regex.Replace(tStr, "{submitscene}", string.Format("{0}{1}[-]", _greenColor, tCollectionPetSubmitDto.submitNpc.scene.name));
				tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tCollectionPetSubmitDto.submitNpc.name));
				//tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
				//tStr = Regex.Replace(tStr, "{pet}", string.Format("{0}{1}[-]", _greenColor, tCollectionPetSubmitDto.pet.name));
			}
			tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tSubmitDto.count, tSubmitDto.needCount));
			tStr = Regex.Replace(tStr, "{pet}", string.Format("{0}{1}[-]", _greenColor, tCollectionPetSubmitDto.pet.name));
			break;
		case SubmitDtoType.HiddenMonster:
			HiddenMonsterSubmitDto tHiddenMonsterSubmitDto = tSubmitDto as HiddenMonsterSubmitDto;
			if (msgType == MsgType.TargetDesc) {
				//tStr = Regex.Replace(tStr, "{scene}", string.Format("{0}{1}[-]", _greenColor, tHiddenMonsterSubmitDto.acceptScene.sceneMap.name));
			} else if (msgType == MsgType.Description) {
				//tStr = Regex.Replace(tStr, "{scene}", string.Format("{0}{1}[-]", _greenColor, tHiddenMonsterSubmitDto.acceptScene.sceneMap.name));
			}
			tStr = Regex.Replace(tStr, "{scene}", string.Format("{0}{1}[-]", _greenColor, tHiddenMonsterSubmitDto.acceptScene.sceneMap.name));
			tStr = Regex.Replace(tStr, "{count}", string.Format("{0}({1}/{2})[-]", _greenColor, tHiddenMonsterSubmitDto.count, tHiddenMonsterSubmitDto.needCount));
			break;
		case SubmitDtoType.ShowMonster:
			ShowMonsterSubmitDto tShowMonsterSubmitDto = tSubmitDto as ShowMonsterSubmitDto;
			if (msgType == MsgType.TargetDesc) {
				//tStr = Regex.Replace(tStr, "{acceptnpc}", string.Format("{0}{1}[-]", _blueColor, tShowMonsterSubmitDto.acceptNpc.name));
			} else if (msgType == MsgType.Description) {
				//tStr = Regex.Replace(tStr, "{acceptscene}", string.Format("{0}{1}[-]", _greenColor, tShowMonsterSubmitDto.acceptNpc.scene.name));
				//tStr = Regex.Replace(tStr, "{acceptnpc}", string.Format("{0}{1}[-]", _blueColor, tShowMonsterSubmitDto.acceptNpc.name));
				tStr = Regex.Replace(tStr, "{submitscene}", string.Format("{0}{1}[-]", _greenColor, tShowMonsterSubmitDto.submitNpc.scene.name));
				tStr = Regex.Replace(tStr, "{submitnpc}", string.Format("{0}{1}[-]", _blueColor, tShowMonsterSubmitDto.submitNpc.name));
			}
			tStr = Regex.Replace(tStr, "{acceptscene}", string.Format("{0}{1}[-]", _greenColor, tShowMonsterSubmitDto.acceptNpc.scene.name));
			tStr = Regex.Replace(tStr, "{acceptnpc}", string.Format("{0}{1}[-]", _blueColor, tShowMonsterSubmitDto.acceptNpc.name));
			break;
		default:
			tStr = "ERROR:SubmitDtoType.Nothing";
			break;
		}

		return tStr;
	}
	
	#region 未接任务信息转换文字
	/// <summary>
	/// 未接任务信息转换文字 -- Gets the ace mission message.
	/// </summary>
	/// <returns>The ace mission message.</returns>
	/// <param name="mission">Mission.</param>
	public string GetAceMissionMsg(Mission mission) {
		//  接取任务对话转换
		string tStr = GetAcceptMissionStr(mission);
		tStr = string.Format("{0}{1}[-]", _defaultColor, tStr);
		return tStr;
	}
	#endregion

	//	当前可接任务文字转换逻辑
	private string GetAcceptMissionStr(Mission mission) {
		string tStr = string.Empty;

		//	判断任务类型不是主线和支线（type = 1 \ type =  2）
		if (IsMainOrExtension(mission)) {
			//	拿任务第一个目标的对话的任务描述
			//tStr = string.Format("{0}-{1} | {2}", mission.missionType.name, mission.name, mission.dialog.progressNpc);
			if (mission.submitConditions != null && mission.submitConditions.submitConditionArray.Count > 0) {
				tStr = string.Format("{0}{1}[-]", _defaultColor, mission.submitConditions.submitConditionArray[0].dialog.description);
			}

			if (tStr == string.Empty) {
				tStr = string.Format("WRANING:当前MissionID:{0} | Name:{1}-{2}没有可接任务描述",
				                     mission.id, mission.missionType.name, mission.name);
				GameDebuger.OrangeDebugLog(tStr);
			}
		} else {
			tStr = mission.missionType.description;
		}
		return tStr;
	}
	#endregion
	
	#region Console Mission信息打印
	public void PrintMissionInfo(Mission mission, MissionDialog dilogue, Npc npc) {
		if (mission == null) {
			GameDebuger.OrangeDebugLog("Console Mission信息打印 mission:NULL");
			return;
		}

		GameDebuger.OrangeDebugLog(string.Format("点击展开Mission详细信息 ##\nMission ID:{0} | Name:{1}-{2}\n" +
		                                         "Npc ID:{3} | Name:{4}\n" +
		                                         "对话内容普通描述:{5}\n",
		                                         mission.id, mission.missionType.name, mission.name,
		                                         npc.id, npc.name,
		                                         PrintDialogueInfo(dilogue, false)));
	}
	#endregion
	
	#region Console Dialogue信息打印
	public string PrintDialogueInfo(MissionDialog dilogue, bool showLog = true) {
		if (dilogue == null) {
			GameDebuger.OrangeDebugLog("Console Dialogue信息打印 dilogue:NULL");
			return "";
		}

		string str = string.Format("点击展开Dialogue详细信息 ##\nDialogue ID:{0}\n" +
		                           "普通描述:{1}\n目标描述:{2}\n接受时npc对白:{3}\n接受时玩家对白:{4}\n" +
		                           "进行时npc对白:{5}\n提交时npc对白:{6}\n提交时npc对白:{7}\n",
		                           dilogue.id,
		                           dilogue.description,
		                           dilogue.goalDesc,
		                           GetMsgByStrList(dilogue.acceptNpc),
		                           GetMsgByStrList(dilogue.acceptPlayer),
		                           dilogue.progressNpc,
		                           GetMsgByStrList(dilogue.submitNpc),
		                           GetMsgByStrList(dilogue.submitPlayer));
		if (showLog) {
			GameDebuger.OrangeDebugLog(str);
		}
		return str;
	}
	#endregion

	#region 字符List串处理
	//	字符List串处理
	/// <summary>
	/// 字符List串处理 -- Gets the message by string list.
	/// </summary>
	/// <returns>The message by string list.</returns>
	/// <param name="strList">String list.</param>
	public string GetMsgByStrList(List<string> strList) {
		string strs = string.Empty;
		for (int i = 0, len = strList.Count; i < len; i++) {
			strs += string.Format("\n\t{0}", strList[i]);
		}
		return strs;
	}
	#endregion
	
	#region 设置粗体GetBold
	/// <summary>
	/// 设置粗体GetBold -- Gets the bold.
	/// </summary>
	/// <returns>The bold.</returns>
	/// <param name="str">String.</param>
	/// <param name="bold">If set to <c>true</c> bold.</param>
	public string GetBold(string str, bool bold = true) {
		//return StringHelper.WrapSymbol(str, bold? "b" : "");
		return string.Format("{0}{1}", bold? "[b]" : "", str);
	}
	public string GetBold(int str, bool bold = true) {
		return GetBold(str.ToString());
	}
	#endregion
}
