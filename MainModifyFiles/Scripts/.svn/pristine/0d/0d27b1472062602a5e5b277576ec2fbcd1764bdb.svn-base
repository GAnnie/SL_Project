using UnityEngine;
using System.Collections;

public class GameTempDateManager 
{
	//用于处理游戏中暂存的数据 ， 比如是否使用元宝//
	private static readonly GameTempDateManager instance = new GameTempDateManager();
   
	public static GameTempDateManager Instance
	{
		get
		{
			return instance; 
		}
	}
	
	private GameTempDateManager()
    {
    }
	
	public bool _IsUseGoldToNextMazeScene
	{
		get;
		set;
	}
	
	//在通缉任务中选择一键提交任务需要消耗元宝是否需要提示//
	public bool _IsUseGoldForImmedFinishMission
	{
		get;
		set;
	}

	//在通缉任务中刷新任务使用元宝是否在本次登录是否需要再提示//
	public bool _IsUseGoldForRefreshMission
	{
		get;
		set;
	}
}
