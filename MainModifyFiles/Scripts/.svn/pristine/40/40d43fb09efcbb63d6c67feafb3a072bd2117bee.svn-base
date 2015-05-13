
// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	NpcDialogueController.cs
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
using com.nucleus.h1.logic.core.modules.scene.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.mission.data;

public class NpcDialogueController : MonoBehaviour,IViewController {

	private Npc _npc = null;
	private NpcDialogueView _view;
	//	选项列表GO临时存储
	private List<GameObject> _optionGo = new List<GameObject>();

	//	一个对话进程的枚举
	private enum DialogueProcess {
		Wait = 0,
		InProgress = 1,
		TheEnd = 2
	}
	private DialogueProcess _dialogueProcess = DialogueProcess.Wait;

	//	一个对话选项的枚举
	private enum DialogueOption {
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
		/// 存在功能选项和任务二级菜单（隐含表示拥有一个或多个NPC功能和有且仅有一个任务）:3 -- The function and sub mission menu.
		/// </summary>
		FunctionAndSubMissionMenu = 3,
		/// <summary>
		/// 只存在任务一级菜单:4 -- The only main mission menu.
		/// </summary>
		OnlyMainMissionMenu = 4,
		/// <summary>
		/// 只存在二级菜单（有且仅有一个任务）:5 -- The only sub mission menu.
		/// </summary>
		OnlySubMissionMenu = 5
	}
	private DialogueOption _dialogueOption = DialogueOption.NothingOption;

	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView() {
		_view = gameObject.GetMissingComponent<NpcDialogueView>();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent() {
		EventDelegate.Set(_view.ClickBtnBoxCollider_UIButton.onClick, ClickDialogueBtn);
		EventDelegate.Set(_view.BoxCollider_UIButton.onClick, ClickDialogueBtn);

		MissionDataModel.Instance.acceptCallback += AcceptMissionCallback;
		MissionDataModel.Instance.finishCallback += FinishMissionCallback;
		MissionDataModel.Instance.submitCallback += SubmitMissionCallback;
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose() {
		ClearDialogueData();

		MissionDataModel.Instance.acceptCallback -= AcceptMissionCallback;
		MissionDataModel.Instance.finishCallback -= FinishMissionCallback;
		MissionDataModel.Instance.submitCallback -= SubmitMissionCallback;
	}

	#region 普通Npc对话Open
	/// <summary>
	/// 这里参数Npc进行分类处理 -- Open the specified npc.
	/// 1、普通Npc对话Open
	/// 2、明雷（随机生成，非闲人明雷）怪物对话Open
	/// </summary>
	/// <param name="npc">Npc.</param>
	public void Open(Npc npc) {
		if (_npc == npc) return;
		
		_npc = npc;
		InitView();
		RegisterEvent();

		if (npc is NpcGeneral) {
			GameDebuger.OrangeDebugLog("普通Npc对话Open !!!!!");
			SetNpcGeneralData(npc as NpcGeneral);
		} else if (npc is NpcMonster) {
			GameDebuger.OrangeDebugLog("明雷怪物对话Open !!!!!");
			SetNpcMonsterData(npc);
		}
	}
	#endregion

	#region 普通Npc对话Open数据设置
	private void SetNpcGeneralData(NpcGeneral npc) {
		//	执行时序需要，将设置对话内容放到最后（当时冥雷闲人是需要调用其他处理方式）
		//string tShowDialogMsg = npc.dialogContent[Random.Range(0, npc.dialogContent.Count)];
		//SetLabelContent(npc.name, tShowDialogMsg);

		//	任务相关	###############################	任务相关	###############################
		List<MissionOption> tMissonOptionList = MissionDataModel.Instance.GetMissionOptionListByNpcInternal(npc);
		int tOptionCount = npc.dialogFunctionId.Count + tMissonOptionList.Count;
		bool tIsShowOption = tOptionCount > 0;

		_view.OptionBg.gameObject.SetActive(tIsShowOption);

		if (tIsShowOption) {
			//	OptionBg高度需要增加任务系统选项
			_view.OptionBg.topAnchor.absolute = 100 + tOptionCount * 60;

			//	隐藏上一次生成的选项实例
			HidderLastOption();

			//	Option 功能选项按钮实例一下
			for(int i = 0, len = npc.dialogFunctionId.Count; i < len; ++i){
				DialogFunction dialogFunction = DataCache.getDtoByCls<DialogFunction>(npc.dialogFunctionId[i]);
				GameObject tInherentOptionGo = i == 0?
					_view.FunctionCellPrefab : NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
				_optionGo.Add(tInherentOptionGo);

				tInherentOptionGo.GetComponentInChildren<UILabel>().text = dialogFunction.name;
				UIButton button = tInherentOptionGo.GetComponentInChildren<UIButton>();
				EventDelegate.Set(button.onClick, () => {
					OpenDialogueFunction(dialogFunction);
					ProxyWorldMapModule.CloseNpcDialogue();
				});
			}


			//	Alot 任务对话相关测试版代码，与主线版本互不干涉 ********************************* Begin *********************************
			if (GameDebuger.openDebugLogOrange) {
				
				//	任务选项数据枚举
				_dialogueOption = DialogueOption.NothingOption;
				
				//	当NPC绑定功能（既是该NPC挂有多个功能选项 -> 任务选项需要一级分类）
				if (npc.dialogFunctionId.Count > 0) {
					//	当NPC绑定任务
					if (tMissonOptionList.Count > 0) {

						if (tMissonOptionList.Count == 1) {
							//	获取列表第一个任务 和 任务当前目标提交项
							Mission tMission = tMissonOptionList[0].mission;
							SubmitDto tSubmitDto = MissionDataModel.Instance.GetSubmitDtoByMission(tMission);
							if (tSubmitDto is ShowMonsterSubmitDto) {
								//	当前只有一个任务且任务类型是冥雷闲人战斗任务（变更对话选项为功能和一级菜单方式）=========================
								_dialogueOption = DialogueOption.FunctionAndMainMissionMenu;

								CreateDialogueMonsterOption(tMission, npc);
							} else {
								//	判断是否任务数量为1 （变更对话选项为功能和（根据任务类型判断具体显示 一\二 级菜单））====================
								_dialogueOption = DialogueOption.FunctionAndSubMissionMenu;
								CreateDialogueOptionByList(tMissonOptionList, npc.dialogFunctionId.Count);
							}
						} else if (tMissonOptionList.Count > 1) {
							//	任务数量大于1（变更对话选项为功能和一级菜单方式）========================================================
							_dialogueOption = DialogueOption.FunctionAndMainMissionMenu;

							//	这里是要处理的，还没弄好
							
							CreateDialogueOptionByList(tMissonOptionList, npc.dialogFunctionId.Count);
						}
					} else {
						//	仅有NPC功能选项 不做处理===================================================================================
						_dialogueOption = DialogueOption.OnlyFunction;
					}
				} else {
					if (tMissonOptionList.Count > 0) {
						if (tMissonOptionList.Count == 1) {
							//	获取列表第一个任务 和 任务当前目标提交项
							Mission tMission = tMissonOptionList[0].mission;
							SubmitDto tSubmitDto = MissionDataModel.Instance.GetSubmitDtoByMission(tMission);
							if (tSubmitDto is ShowMonsterSubmitDto) {
								//	当前只有一个任务且任务类型是冥雷闲人战斗任务（变更对话选项为功能和一级菜单方式）=========================
								_dialogueOption = DialogueOption.FunctionAndMainMissionMenu;
								//	调用冥雷对话方法
								//SetNpcMonsterData(npc);
								CreateDialogueMonsterOption(tMission, npc);
								return;
							} else {
								//	仅生成二级对话选项（既是没有选项，直接提交做改任务逻辑）===============================================
								_dialogueOption = DialogueOption.OnlySubMissionMenu;
								CreateDialogueOptionByList(tMissonOptionList, npc.dialogFunctionId.Count);
							}
						} else {
							//	仅有一级对话选项 ======================================================================================
							_dialogueOption = DialogueOption.OnlyMainMissionMenu;
							
							//	这里是要处理的，还没弄好
							CreateDialogueOptionByList(tMissonOptionList, npc.dialogFunctionId.Count);
						}
					} else {
						//	当执行到这里表示没有菜单选项 不做处理=========================================================================
						GameDebuger.OrangeDebugLog(string.Format("{0}", "1）NPC没有功能选项 2）且该时刻没有对应NPC任务绑定 3）"));
						_dialogueOption = DialogueOption.NothingOption;
					}
				}
				GameDebuger.AquaDebugLog(string.Format("NPC对话选项分类:{0}", _dialogueOption));
			}
			//	Alot 任务对话相关测试版代码，与主线版本互不干涉  ********************************** End ***********************************

			else {
				//	任务相关	###############################	任务相关	###############################
				for (int i = 0, len = tMissonOptionList.Count; i < len; i++) {
					GameObject tMissionOptionGo = (i == 0 && npc.dialogFunctionId.Count <= 0)?
						_view.FunctionCellPrefab : NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
					_optionGo.Add(tMissionOptionGo);

					MissionOption tOption = tMissonOptionList[i];
					UILabel tUILabel = tMissionOptionGo.GetComponentInChildren<UILabel>();
					tMissionOptionGo.GetComponentInChildren<UILabel>().text =
						string.Format("{0}|{1}-{2}", tOption.optionType? "提交" : "可接", tOption.mission.missionType.name, tOption.mission.name);

					UIButton button = tMissionOptionGo.GetComponentInChildren<UIButton>();
					EventDelegate.Set(button.onClick, () => {
						OpenMissionOption(tOption);
					});
				}
			}

			_view.FunctionGrid.Reposition();
		}

		string tShowDialogMsg = npc.dialogContent[Random.Range(0, npc.dialogContent.Count)];
		SetLabelContent(npc.name, tShowDialogMsg);
	}
	#endregion

	#region 实例任务相关的NPC对话选项内容
	//	实例任务相关的NPC对话选项内容（List，int）
	private void CreateDialogueOptionByList(List<MissionOption> missionOptionList, int npcDialogueOptionCount) {
		for (int i = 0, len = missionOptionList.Count; i < len; i++) {
			bool tSelfGoState = i == 0 && npcDialogueOptionCount <= 0;

			Mission tMission = missionOptionList[i].mission;
			SubmitDto tSubmitDto = MissionDataModel.Instance.GetSubmitDtoByMission(tMission);
			bool tIsShowMonster = MissionDataModel.Instance.IsShowMonster(tSubmitDto);

			if (tIsShowMonster) {
				CreateDialogueMonsterOption(tMission, _npc, i, false);
			} else {
				CreateDialogueOption(missionOptionList[i], tSelfGoState);
			}
		}
	}

	//	实例任务相关的NPC对话选项内容（MissionOption，bool）
	private void CreateDialogueOption(MissionOption missionOption, bool tSelfGoState) {
		GameObject tMissionOptionGo = tSelfGoState? _view.FunctionCellPrefab
			: NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
		_optionGo.Add(tMissionOptionGo);

		string tStr = string.Format("{0}|", missionOption.optionType? "提交" : "可接");
		if (MissionDataModel.Instance.IsMainOrExtension(missionOption.mission)) {
			tStr += string.Format("{0}-{1}", missionOption.mission.missionType.name, missionOption.mission.name);
		} else {
			tStr += MissionDataModel.Instance.GetMissionTitleName(missionOption.mission, missionOption.optionType, false);
		}

		tMissionOptionGo.GetComponentInChildren<UILabel>().text = tStr;
		
		UIButton button = tMissionOptionGo.GetComponentInChildren<UIButton>();
		EventDelegate.Set(button.onClick, () => {
			OpenMissionOption(missionOption);
		});
	}

	//	实例明雷怪相关NPC对话选项内容
	private void CreateDialogueMonsterOption(Mission mission, Npc npc, int index = 0, bool reSetDialogContent = true) {
		if (reSetDialogContent) {
			MissionDialog tMissionDialog = MissionDataModel.Instance.GetMissionSubmitDtoDialog(mission);
			SetLabelContent(npc.name, tMissionDialog.tips);

			//	OptionBg高度需要增加任务系统选项
			_view.OptionBg.topAnchor.absolute = 100 + 1 * 60;
			//	隐藏上一次生成的选项实例
			HidderLastOption();
		}

		GameObject tMissionOptionGo = index == 0?
			_view.FunctionCellPrefab : NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
		tMissionOptionGo.SetActive(true);
		_optionGo.Add(tMissionOptionGo);
		
		UILabel tUILabel = tMissionOptionGo.GetComponentInChildren<UILabel>();
		tUILabel.text = string.Format("战斗-{0}", MissionDataModel.Instance.GetMissionTitleName(mission, true, false));
		
		UIButton button = tMissionOptionGo.GetComponentInChildren<UIButton>();
		EventDelegate.Set(button.onClick, () => {
			//	先关闭对话，清除上次的数据信息
			//ProxyWorldMapModule.CloseNpcDialogue();
			//	打开冥雷任务二级菜单选项
			SetMonsterSubOption(mission, npc);
		});
		
		_view.FunctionGrid.Reposition();
	}
	#endregion

	#region 明雷（随机生成，非闲人明雷）怪物对话数据设置
	private void SetNpcMonsterData(Npc npc) {
		//	(明雷怪物有且仅有一个任务)若这里出错，既是策划填写数据问题，该问题已跟策划确认。
		//	任务相关	###############################	任务相关	###############################
		List<MissionOption> tMissonOptionList = MissionDataModel.Instance.GetMissionOptionListByNpc(npc);

		if(tMissonOptionList.Count > 0) {
			OpenMissionOption(tMissonOptionList[0], () => {
				_view.OptionBg.gameObject.SetActive(true);
				MissionDialog tMissionDialog = MissionDataModel.Instance.GetMissionSubmitDtoDialog(tMissonOptionList[0].mission);

				SetLabelContent(npc.name, tMissionDialog.tips);
				CreateDialogueMonsterOption(tMissonOptionList[0].mission, npc);
			});
		}
	}
	#endregion

	#region 冥雷战斗二级菜单选项显示
	private List<string> _subMonsterOptionStrList = new List<string>() {
		"开始战斗",
		"有钱任性",
		"门派求助"
	};
	private void SetMonsterSubOption(Mission mission, Npc npc) {
		int tOptionCount =  _subMonsterOptionStrList.Count;

		//	隐藏上一次生成的选项实例
		for (int i = 0, len = _optionGo.Count; i < len; i++) {
			//Destroy(_optionGo[i]);
			_optionGo[i].SetActive(false);
		}
		//_optionGo.Clear();

		_view.OptionBg.gameObject.SetActive(tOptionCount > 0);

		//	OptionBg高度需要增加任务系统选项
		_view.OptionBg.topAnchor.absolute = 100 + tOptionCount * 60;

		//	Option 功能选项按钮实例一下
		for(int i = 0, len = tOptionCount; i < len; ++i){
			int tIndex = i;
			GameObject tInherentOptionGo = NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
			tInherentOptionGo.SetActive(true);
			_optionGo.Add(tInherentOptionGo);

			UILabel tUILabel = tInherentOptionGo.GetComponentInChildren<UILabel>();
			tUILabel.text = string.Format("{0}", _subMonsterOptionStrList[i]);
			UIButton button = tInherentOptionGo.GetComponentInChildren<UIButton>();

			EventDelegate.Set(button.onClick, () => {
				GameDebuger.AquaDebugLog(string.Format("当前点击二级菜单选项是: {0}", tIndex));
				switch (tIndex) {
				case 0:
					//	开始请求战斗
					MissionDataModel.Instance.FinishTargetSubmitDto(mission, npc);
					break;
				case 1:
					//	TADO	花钱过关卡
					TipManager.AddTip(string.Format("有钱任性......还没有做啊~！重选一个"));
					return;
					//break;
				case 2:
					//	TADO	帮派求助
					TipManager.AddTip(string.Format("帮派求助......还没有做啊~！重选一个"));
					return;
					//break;
				default:
					break;
				}
				ProxyWorldMapModule.CloseNpcDialogue();
			});
		}
		_view.FunctionGrid.Reposition();
	}
	#endregion

	#region 隐藏上次的选项信息
	private void HidderLastOption() {
		//	隐藏上次的选项实例
		for (int i = 0, len = _optionGo.Count; i < len; i++) {
			if (i > 0) {
				_optionGo[i].SetActive(false);
			}
		}
	}
	#endregion

	#region 通用对话（学习阵法，角色洗点，宠物洗点，宠物打书，Npc）
	private System.Action<int> _onSelectOption;
	private int _maxOptionWidth = int.MinValue;
	/// <summary>
	/// 通用对话（学习阵法，角色洗点，宠物洗点，宠物打书，Npc） -- Opens the common dialogue.
	/// </summary>
	/// <param name="npcName">Npc name.</param>
	/// <param name="content">Content.</param>
	/// <param name="optionList">Option list.</param>
	/// <param name="onSelect">On select.</param>
	/// <param name="optionBgWidth">Option background width.</param>
	public void OpenCommonDialogue(string npcName,string content,List<string> optionList,System.Action<int> onSelect,int optionBgWidth){
		InitView ();
		RegisterEvent ();

		_view.NameLabel.text = npcName;
		_view.MsgLabel.text = content;
		_onSelectOption = onSelect;

		bool tOptionListCount = (optionList.Count + optionList.Count) > 0;
		_view.OptionBg.gameObject.SetActive(tOptionListCount);

		if (tOptionListCount) {
			_view.OptionBg.topAnchor.absolute = 100 +  optionList.Count * 60;
			_view.OptionBg.leftAnchor.absolute = -optionBgWidth;
			
			//	隐藏上一次生成的选项实例
			HidderLastOption();

			for(int i = 0, len = optionList.Count; i < len; ++i) {
				int index = i;
				GameObject go = i==0? _view.FunctionCellPrefab : NGUITools.AddChild(_view.FunctionGrid.gameObject,  _view.FunctionCellPrefab);
				go.GetComponentInChildren<UILabel>().text = optionList[i];

				UIButton button = go.GetMissingComponent<UIButton>();
				EventDelegate.Set (button.onClick, () => {
					if(_onSelectOption != null) {
						_onSelectOption(index);
					}
					
					ProxyWorldMapModule.CloseNpcDialogue();
				});
			}
			
			_view.FunctionGrid.Reposition();
		}
	}
	#endregion

	#region 普通绑定Npc对话选项点击回调
	/// <summary>
	/// 普通绑定Npc对话选项点击回调 -- Opens the dialogue function.
	/// </summary>
	/// <param name="dialogFunction">Dialog function.</param>
	public static void OpenDialogueFunction(DialogFunction dialogFunction) {
		switch(dialogFunction.type) {
		case DialogFunction.DialogFunctionEnum_Shop:
			ProxyShopModule.OpenNpcShop(dialogFunction.param);
			break;
		case DialogFunction.DialogFunctionEnum_Warehouse:
			ProxyWarehouseModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_Trade:
			ProxyTradeModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_PetResume:
			ProxyPetResumeModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_PetCage:
			ProxyPetWarehouseModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_ItemResume:
			ProxyItemRetrieveModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_PetShop:
			ProxyTradePetModule.Open();
			break;
		case DialogFunction.DialogFunctionEnum_ConvertRareMyth:
			PetExchangeDialogueLogic.Open(dialogFunction.param);
			break;
		case DialogFunction.DialogFunctionEnum_Dye:
			ProxyPlayerPropertyModule.SelectDyeOption(dialogFunction.param);
			break;
		}
	}
	#endregion

	// ===================================== 任务相关 =====================================================
	#region NPC对话任务选项按钮打开
	/// <summary>
	/// NPC对话选项按钮打开 -- Opens the mission option.
	/// </summary>
	/// <param name="missionOption">Mission option.</param>
	/// <param name="callback">Callback.</param>
	private void OpenMissionOption(MissionOption missionOption, System.Action callback = null) {
		//	清除Option Grid
		_view.OptionBg.gameObject.SetActive(false);

		_dialogMagCount = 0;
		_dialogueProcess = DialogueProcess.InProgress;

		//	是否师门任务类型
		bool tIsFactionMission = MissionDataModel.Instance.IsFactionMissionType(missionOption.mission);

		//	获取追踪Npc
		bool tIsExistState = missionOption.optionType;
		Npc tMissionNpc = MissionDataModel.Instance.GetMissionNpcByMission(missionOption.mission, tIsExistState);

		//	当改NPC是虚拟NPC是转换为具体NPC
		if (tMissionNpc is FactionNpc) {
			tMissionNpc = MissionDataModel.Instance.NpcVirturlToEntity(tMissionNpc);
		}

		//	对话内容
		MissionDialog tMissionDialog = null;

		//	分类处理
		if (tIsExistState) {
			//	Bind Mission 提交项
			SubmitDto tSubmitDto = MissionDataModel.Instance.GetSubmitDtoByMission(missionOption.mission);
			
			//	判断是否达成最后一个目标（true：提交任务并return）并关闭对话界面
			if (tSubmitDto == null) {
				MissionDataModel.Instance.SubmitMission(missionOption.mission.id);
				return;
			}
			
			//	获取MissionDialogue
			tMissionDialog = tSubmitDto.dialog;
			//	获取追踪Npc(这里当NPC为空，已在上面tSubmitDto == null做了处理(return 掉了))
			if (tMissionNpc == null) {
				Debug.LogError(string.Format("ERROR -> 当前任务ID：{0} | Name：{1}-{2}任务提交NPC为空",
				                             missionOption.mission.id, missionOption.mission.missionType.name, missionOption.mission.name));
			}
		} else {
			if (tIsFactionMission) {

			} else {
				tMissionDialog = missionOption.mission.dialog;
			}
		}

		//	打印对话内容信息
		MissionDataModel.Instance.PrintDialogueInfo(tMissionDialog);
		
		//	回调设置 CloseClickCallback Begin	#######################################
		_closeClickCallback = () => {
			GameDebuger.OrangeDebugLog(string.Format(" ==== {0} ClickClose | 回调 {1} 任务对话 | SubmitDtoType:{2} ==== ",
			                                         tIsExistState? "绑定":"可接",
			                                         _dialogueProcess,
			                                         MissionDataModel.Instance.GetSubmitDtoTypeByMission(missionOption.mission)));

			switch (_dialogueProcess) {
			case DialogueProcess.Wait:
				break;
			case DialogueProcess.InProgress:
				PlayMissionDialogueMsg(tMissionDialog, tIsExistState, tMissionNpc);
				break;
			case DialogueProcess.TheEnd:
				_dialogueProcess = DialogueProcess.Wait;
				_closeClickCallback = null;

				if (tIsExistState) {
					//	任务提交项（明雷任务回调处理）
					if (callback != null) {
						callback();
					} else {
						if (MissionDataModel.Instance.FinishTargetSubmitDto(missionOption.mission, _npc)) {
							ProxyWorldMapModule.CloseNpcDialogue();
						}
					}
				} else {
					if (tIsFactionMission) {
						//	接受师门任务
						MissionDataModel.Instance.AcceptFactionMission();
					} else {
						//	接受 主线\支线 任务
						MissionDataModel.Instance.AcceptMission(missionOption.mission.id);
					}
				}
				break;
			default:
				break;
			}
		};
		//	回调设置 CloseClickCallback End		#######################################

		//	判断是否师门任务选项
		if (tIsFactionMission) {
			_dialogueProcess = DialogueProcess.TheEnd;
			if (_closeClickCallback != null) {
				_closeClickCallback();
			}
		} else {
			MissionDataModel.Instance.PrintMissionInfo(missionOption.mission, tMissionDialog, tMissionNpc);
			PlayMissionDialogueMsg(tMissionDialog, tIsExistState, tMissionNpc);
		}
	}
	#endregion

	#region 设置详细对话内容
	private int _dialogMagCount = 0;
	private void PlayMissionDialogueMsg(MissionDialog missionDialogue, bool isBind, Npc missionNpc) {
		MissionDialog tMissionDialog = missionDialogue;
		Npc tTalkSubmitDtoNpc = missionNpc;

		List<string> tNpcDialogueList = isBind? tMissionDialog.submitNpc : tMissionDialog.acceptNpc;
		List<string> tPlayerDialogueList = isBind? tMissionDialog.submitPlayer : tMissionDialog.acceptPlayer;

		//	获取填充内容
		bool tState = _dialogMagCount%2 == 0;
		int tDiaNpcCount = tNpcDialogueList.Count;
		int tDiaPlayerCount = tPlayerDialogueList.Count;
		//	tState(Npc \ Player)
		if ((tState && tDiaNpcCount <= 0) || (!tState && tDiaPlayerCount <= 0)) {
			_dialogueProcess = DialogueProcess.TheEnd;

			if (_closeClickCallback != null) {
				_closeClickCallback();
			}
			return;
		}

		string tName = tState? tTalkSubmitDtoNpc.name : PlayerModel.Instance.GetPlayerName();
		int tContentIndex = _dialogMagCount/2;
		List<string> tContentList = tState? tNpcDialogueList : tPlayerDialogueList;
		SetLabelContent(tName, tContentList[tContentIndex]);

		//	对话次数自加加
		if (++_dialogMagCount >= (tDiaNpcCount + tDiaPlayerCount)) {
			_dialogueProcess = DialogueProcess.TheEnd;
		}
	}
	#endregion

	#region 设置当前对话信息 FaceSprite \ Name \ Content
	private void SetLabelContent(string npcName, string npaMsg) {
		_view.NameLabel.text = npcName;
		_view.MsgLabel.text = npaMsg;
	}
	#endregion
	
	#region 点击对话内容
	private System.Action _closeClickCallback = null;
	private void ClickDialogueBtn() {
		if (_closeClickCallback != null) {
			_closeClickCallback();
		} else {
			ProxyWorldMapModule.CloseNpcDialogue();
		}
	}
	#endregion

	#region NPC对话接受任务回调
	/// <summary>
	/// NPC对话接受任务回调 -- Accepts the mission callback.
	/// </summary>
	/// <param name="playerMissionDto">Player mission dto.</param>
	private void AcceptMissionCallback(PlayerMissionDto playerMissionDto) {
		GameDebuger.OrangeDebugLog("NPC对话 接受任务回调 -- Test -- 注意，这里回调中做以下代码处理");
		//	寻路到指定NPC处，在AcceptMission的时候Midel中已经做了，查看MissionDataModel Line 802
		//MissionDataModel.Instance.FindToMissionNpcByMission(playerMissionDto.mission, true);
		
		//	关闭NPC对话（之后根据CH需要分类进行处理）
		if (playerMissionDto.mission.quickFindWay) {

		} else {
			ProxyWorldMapModule.CloseNpcDialogue();
		}
	}
	#endregion
	
	#region 任务便捷寻路
	public void OpenQuickFindPath(Mission tMission) {
		InitView ();
		RegisterEvent ();

		if (tMission.quickFindWay) {
			//	npc对话接取任务 \ 师门任务接取
			if (_npc == null) {
				_npc = MissionDataModel.Instance.GetFactionNpcByVirtualID(1);
			}

			MissionOption tMissionOption = new MissionOption(tMission, true);
			//	是否师门任务 && 询问是否需要便捷寻路
			OpenMissionOption(tMissionOption, () => {
				MissionDialog tMissionDialog = MissionDataModel.Instance.GetMissionSubmitDtoDialog(tMission);
				string tMsg = MissionDataModel.Instance.GetCurDescriptionContent(tMission);
				SetLabelContent(_npc.name, tMsg);
				
				_view.OptionBg.gameObject.SetActive(true);
				
				//	OptionBg高度需要增加任务系统选项
				_view.OptionBg.topAnchor.absolute = 100 + 1 * 60;
				
				//	隐藏上一次生成的选项实例
				HidderLastOption();
				
				//	任务相关	###############################	任务相关	###############################
				GameObject tMissionOptionGo = _view.FunctionCellPrefab;
				//GameObject tMissionOptionGo = NGUITools.AddChild(_view.FunctionGrid.gameObject, _view.FunctionCellPrefab);
				tMissionOptionGo.SetActive(true);
				_optionGo.Add(tMissionOptionGo);
				
				UILabel tUILabel = tMissionOptionGo.GetComponentInChildren<UILabel>();
				tMissionOptionGo.GetComponentInChildren<UILabel>().text =
					string.Format("{0}", MissionDataModel.Instance.GetMissionTitleName(tMission, true, false));
				
				UIButton button = tMissionOptionGo.GetComponentInChildren<UIButton>();
				EventDelegate.Set(button.onClick, () => {
					MissionDataModel.Instance.FindToMissionNpcByMission(tMission, true);
					ProxyWorldMapModule.CloseNpcDialogue();
				});
				
				_view.FunctionGrid.Reposition();
			});
		} else {
			ProxyWorldMapModule.CloseNpcDialogue();
		}
	}
	#endregion

	#region Finish任务目标回调
	private void FinishMissionCallback() {
		ProxyWorldMapModule.CloseNpcDialogue();
	}
	#endregion

	#region 提交任务回调
	private void SubmitMissionCallback(Npc tNextMissionNpc) {
		//	判断是否下一个任务接受NpcID 是当前对话NPCID
		if (tNextMissionNpc != null && tNextMissionNpc.id == _npc.id) {
			//	置空，否者不能打开对话（因为当npc=_npc时【Line 59】中直接return掉对话）
			ClearDialogueData();
			GameDebuger.OrangeDebugLog("TADO -> 当前对话Npc是上一个任务提交Npc，不需要关闭对话框");
		} else {
			ProxyWorldMapModule.CloseNpcDialogue();
		}
	}
	#endregion

	#region 关闭对话清除数据信息
	private void ClearDialogueData() {
		_npc = null;
		_dialogMagCount = 0;
		_closeClickCallback = null;
		_dialogueProcess = DialogueProcess.Wait;
		_dialogueOption = DialogueOption.NothingOption;

		_optionGo.Clear();
	}
	#endregion
	//	===================================== 任务相关 End =====================================================
}

