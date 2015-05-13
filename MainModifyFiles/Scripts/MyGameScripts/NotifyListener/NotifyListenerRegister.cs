// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NotifyListenerRegister.cs
// Author   : SK
// Created  : 2014/11/11
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;

public class NotifyListenerRegister
{
	private static bool HasInited = false;
	public static void Setup ()
	{
		if (HasInited)
		{
			return;
		}

		HasInited = true;

		// battlevideo
		addListener(new VideoListener());
		addListener (new DemoVideoListener ());
		addListener (new SceneMineVideoListener ());
		addListener (new PvpVideoListener());
		addListener (new NpcMonsterVideoListener());
		addListener (new TollgateVideoListener());

		// server
		addListener(new GameServerTimeDtoListener());
		addListener(new GameServerGradeDtoListener());
		addListener (new DuplicateLoginNotifyListener ());

		// player
		addListener(new CharactorExpNotifyListener());
		addListener(new CharactorExpInfoNotifyListener());
		addListener(new WealthNotifyListener());
		addListener(new DoubleExpDtoListener());
		addListener(new SubWealtthNotifyListener());
		addListener(new FallRewardNotifyListener());
		addListener(new SpellNotifyListener());
		addListener(new PlayerTitleNotifyListener());
		addListener(new PlayerTitleDtoListener());
		addListener(new WorldJubilationStateBarNotifyListener());
		addListener(new PlayerDyeNotifyListener());
		addListener(new PlayerMeritsNotifyListener());

		// pet
		addListener(new PetCharactorDtoListener());
		addListener(new PetAptitudeNotifyListener());
		addListener(new PetLifeNotifyListener());

		// item
		addListener(new PackItemNotifyListener());
		addListener(new PackItemDiscardNotifyListener());
        addListener(new EquipmentBuffUpdateNotifyListener());

		//Team
		addListener(new TeamDtoListener());
		addListener(new AwayTeamNotifyListener());
		addListener(new BackTeamNotifyListener());
		addListener(new JoinTeamDtoListener());
		addListener(new KickOutMemberNotifyListener());
		addListener(new LeaderChangeNotifyListener());
		addListener(new LeaveTeamNotifyListener());
		addListener(new TeamMemberOfflineNotifyListener());

		addListener(new JoinTeamRequestNotifyListener());
		addListener(new TeamInviteNotifyListener());
		addListener(new TeamMatchInfoNotifyListener());
		addListener(new PublishTeamNotifyListener());

		addListener(new SummonAwayTeamPlayersNotifyListener());
		addListener(new TeamMemberUpgradeNotifyListener());

		//Scene
		addListener(new PlayerTeamStatusChangeNotifyListener());
		addListener(new SceneDtoListener());
		addListener(new MainCharactorUpgradeSceneNotifyListener());
		addListener(new BattleSceneNotifyListener());

		//邮件监听
		addListener (new MailNotifyListener ());

		//聊天玩家对话监听
		addListener (new ChatNotifyListener ());
		//聊天系统监听
		addListener (new ChatSysNotifyListener ());
		//聊天传闻监听
		addListener (new ChatHearsayNotifyListener ());
		//好友度
		addListener(new FriendDegreeNotifyListener());
		//好友上下线
		addListener(new FriendOnLineNotifyListener());

		//	接受任务通知注册
		addListener(new PlayerMissionNotifyListener());
		//	任务提交项通知变更注册
		addListener(new MissionNotifyListener());
		//	收集宠物项任务删除宠物通知
		addListener(new MissionCollectionPetListeren());
	}

	private static void addListener (BaseDtoListener lis)
	{
		SocketManager.Instance.addMessageProcessor (lis);
	}

	public static BaseDtoExcuteListener regDtoExcuter(Type clsName, IDtoListenerExcute excuter) {
		if (clsName == null || excuter == null ) return null ;
		
		//GameDebuger.Log( string.Format("<<< Listener Register ==> clsName = {0} , excuter = {1} >>>",clsName.ToString(), excuter.GetType().ToString()) );
		
		BaseDtoExcuteListener dtoExcute = new BaseDtoExcuteListener(clsName, excuter) ; 
		addListener(dtoExcute) ;
		return dtoExcute ;
	}
	
	public static void unRegDtoExcuter(BaseDtoExcuteListener listener){
		SocketManager.Instance.removeMessageProcessor( listener );
	}
}

