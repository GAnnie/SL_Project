using UnityEngine;
using System.Collections;
using System;
using com.nucleus.h1.logic.core.modules.team.dto;

public class TeamDtoListener : BaseDtoListener {

	override public void process( object message )
	{
		TeamDto teamDto = message as TeamDto;
#if TEAM_DEBUGINFO
		Debug.Log(string.Format("TeamDto teamUID:{0} 队长id:{1} 队员数量:{2}",teamDto.teamUniqueId,teamDto.leaderPlayerId,teamDto.teamMembers.Count).WrapColorWithLog());
#endif
		TeamModel.Instance.SetupTeamInfo(teamDto);
		if(teamDto.curBattleVideo != null)
		{
			BattleManager.Instance.PlayBattle(teamDto.curBattleVideo);
			WorldManager.Instance.EnterWithSceneDto(teamDto.curSceneDto);
		}
		else if(teamDto.curSceneDto != null){
			WorldManager.Instance.EnterWithSceneDto(teamDto.curSceneDto);
		}
	}
	
	override protected Type getDtoClass()
	{
		return typeof(TeamDto);
	}
}
