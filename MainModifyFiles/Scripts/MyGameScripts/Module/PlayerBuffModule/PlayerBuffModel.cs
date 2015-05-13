using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerBuffModel
{
	private static readonly PlayerBuffModel instance = new PlayerBuffModel ();
	public static PlayerBuffModel Instance {
		get {
			return instance;
		}
	}
	private PlayerBuffModel (){}

	public const int SATIATION_ID = 601;		//饱食度
	public const int DOUBLEEXP_ID = 604;		//双倍点数
	public const int EQBREAKDOWN_ID = 607;		//装备损毁
	public const int WORLDJUBILATION_ID = 610;  //普天同庆
	public const int TRANSFORM_ID = 613;			//变身卡
//	public const int RESETPET_ID = 616;				//洗宠申请

	private Dictionary<int,StateBarDto> _playerStateBarDtoDic = new Dictionary<int, StateBarDto> ();
	private HashSet<int> _activeBuffIdSet = new HashSet<int> ();

	public event System.Action OnPlayerBuffUpdate;

	public void Setup (List<StateBarDto> stateBarDtoList)
	{
		for (int i=0; i<stateBarDtoList.Count; ++i) {
			StateBarDto stateDto = stateBarDtoList [i];
			_playerStateBarDtoDic [stateDto.id] = stateDto;

			if(stateDto is DoubleExpStateBarDto)
				PlayerModel.Instance.SetupDoubleExpDto(stateDto as DoubleExpStateBarDto);

			//--TestCode--
//			if(stateDto is WorldJubilationStateBarDto)
//				((WorldJubilationStateBarDto)stateDto).expiredTime = SystemTimeManager.Instance.GetUTCTimeStamp()+20000;
		}

		CheckOutWorldJubilationState();
	}

	#region 普天同庆
	public void UpdateWorldJubilationState(WorldJubilationStateBarNotify notify){
		if(PlayerModel.Instance.GetPlayerId() == notify.playerId){
			WorldJubilationStateBarDto stateDto = _playerStateBarDtoDic[WORLDJUBILATION_ID] as WorldJubilationStateBarDto;
			stateDto.expiredTime = notify.expiredTime;
			CheckOutWorldJubilationState();
		}
	}

	public bool CheckOutWorldJubilationState(){
		WorldJubilationStateBarDto stateDto = _playerStateBarDtoDic[WORLDJUBILATION_ID] as WorldJubilationStateBarDto;
		if(stateDto.expiredTime > 0){
			//CoolDownManager以秒为单位计算
			float cd = (float)(stateDto.expiredTime - SystemTimeManager.Instance.GetUTCTimeStamp())/1000f;
			if(cd > 0f){
				CoolDownManager.Instance.SetupCoolDown("WorldJubilationStateTimer",cd,null,WorldJubilationStateTimerFinish);
				ToggleWorldJubilationBuffTip(true);
				return true;
			}
		}
		CoolDownManager.Instance.CancelCoolDown("WorldJubilationStateTimer");
		ToggleWorldJubilationBuffTip(false);
		return false;
	}

	public void WorldJubilationStateTimerFinish(){
		WorldJubilationStateBarDto stateDto = _playerStateBarDtoDic[WORLDJUBILATION_ID] as WorldJubilationStateBarDto;
		stateDto.expiredTime = 0;
		ToggleWorldJubilationBuffTip(false);
	}
	#endregion

	#region Getter
	public StateBarDto GetStateBarDtoInfo(int stateId){
		return _playerStateBarDtoDic[stateId];
	}

	public List<int> GetActiveBuffList ()
	{
		List<int> result = new List<int> ();
		foreach (int stateId in _activeBuffIdSet) {
			result.Add (stateId);
		}

		result.Sort ();
		return result;
	}
	#endregion

	#region BuffTip Toggle Func
	//饱食状态开关
	public void ToggleSatiationBuffTip (bool active)
	{
		ToggleBuffState (SATIATION_ID, active);
	}

	//双倍经验状态开关
	public void ToggleDoubleExpBuffTip (bool active)
	{
		ToggleBuffState (DOUBLEEXP_ID, active);
	}

	//装备损毁状态开关
	public void ToggleEqBreakdownBuffTip (bool active)
	{
		ToggleBuffState (EQBREAKDOWN_ID, active);
	}

	//普天同庆状态开关
	public void ToggleWorldJubilationBuffTip(bool active){
		ToggleBuffState (WORLDJUBILATION_ID, active);
	}

	public void ToggleBuffState (int stateId, bool active)
	{
		if (active) {
			_activeBuffIdSet.Add (stateId);
			if (OnPlayerBuffUpdate != null)
				OnPlayerBuffUpdate ();
		} else {
			if (_activeBuffIdSet.Remove (stateId)) {
				if (OnPlayerBuffUpdate != null)
					OnPlayerBuffUpdate ();
			}
		}
	}

	public void Dispose()
	{
		_playerStateBarDtoDic.Clear();
	}
	#endregion
}
