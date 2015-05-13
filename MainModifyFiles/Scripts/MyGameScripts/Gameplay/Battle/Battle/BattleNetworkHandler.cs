using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.data;
using System;
using com.nucleus.h1.logic.core.modules.scene.data;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BattleNetworkHandler : IDtoListenerExcute
{
	private string gameID;
	
	private bool isDemo;
	
	private BattleController _battleController;

	private MultipleNotifyListener _listener;

	public void Start (BattleController battleController)
	{
		_battleController = battleController;
		StartNotifyListener();
	}

	private void StartNotifyListener()
	{
		if (_listener == null) 
		{
			_listener = new MultipleNotifyListener();
			_listener.AddNotify(typeof(VideoRound));
			_listener.AddNotify(typeof(BattleSoldierReadyNotify));
			_listener.AddNotify(typeof(VideoSoldierUpdateNotify));
			_listener.Start(this);
		}
	}
	
	public void StopNotifyListener()
	{
		if (_listener != null){
			_listener.Stop();
		}
		_listener = null;
	}	

	public void ExcuteDto(object dto)
	{
		if (BattleDemoController.PositionMode) 
		{
			return;
		}

		if (dto is VideoRound)
		{
			//战斗回合下发， PVP中， 当双方都请求了开战， 服务器主动下发，PVE则不通过下发这个， 直接请求接口返回
			VideoRound gvr = dto as VideoRound;
			
			//如果收到的回合数是0,则不处理
			if (gvr.count == 0){
				return;
			}

			_battleController.GetInstController().AddVideoRound(gvr);

			if (_battleController != null){
				//EnterBattleMode();
				_battleController.GetInstController().CheckNextRound();
				GameDebuger.Log("GameVideoRound Notify in Battle");
			}else{
				GameDebuger.Log("GameVideoRound Notify notin Battle");
			}
		}
		else if (dto is BattleSoldierReadyNotify)
		{
			_battleController.HandlerSoldierReadyNotify(dto as BattleSoldierReadyNotify);
		}
		else if (dto is VideoSoldierUpdateNotify)
		{
			_battleController.UpdateSoldiers(dto as VideoSoldierUpdateNotify);
		}
	}

	//马上出战斗结果
	public void ShowBattleResult()
	{
		_battleController.GetInstController().ShowBattleResult();
	}
}

