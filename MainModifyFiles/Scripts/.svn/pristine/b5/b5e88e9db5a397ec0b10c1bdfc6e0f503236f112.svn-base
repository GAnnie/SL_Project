using UnityEngine;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.scene.data;

public class StoryManager {
	
	
	private static  StoryManager instance = null;
	public static StoryManager Instance
	{
		get
		{   if (instance == null) {
				instance = new StoryManager ();
				instance.SetUpManager();
			}
			return instance;
		}
	}

	private StoryPlayer storyPlayer;

	#region 监听任务触发的剧情事件
	private  void SetUpManager() {
		MissionPlotModel.Instance.storyPlotCallback = MissionStoryPlay;
	}
	#endregion

	#region 播放
	public void PlayStory(int storyId , bool check = false)
	{
		string storyPath = string.Format("ConfigFiles/StoryConfig/StoryConfig{0}", storyId);
		JsonStoryInfo configInfo = DataHelper.GetJsonFile<JsonStoryInfo>(storyPath, "bytes", false);
		if (configInfo == null)
		{
			TipManager.AddTip("NO story " + storyId);
			return;
		}


		NGUIFadeInOut.FadeOut (delegate() 
		{
			if (storyPlayer == null)
			{
				GameObject go = new GameObject("StoryPlayer");
				go.GetMissingComponent<DontDestroyObject>();
				storyPlayer = go.GetMissingComponent<StoryPlayer>();
			}
			
			storyPlayer.PlayStory (storyId,check);
		});
	}
	#endregion
	#region 提前跳出
	public void EndStory(){
		storyPlayer.StopStory();
	}
	#endregion


	#region 任务触发的剧情播放
	System.Action<PlayerMissionDto> missionStoryPlayCallBack;
	public  void  MissionStoryPlay(int misstionStoryId, System.Action<PlayerMissionDto> callBack){

		missionStoryPlayCallBack = callBack;

		PlayStory(misstionStoryId);
	}
	#endregion
	

	#region 告诉服务器已播放过这个剧情
	public void HaveBeenPlayed(int missionStoryId){

		ServiceRequestAction.requestServer(PlotService.end(missionStoryId),"HaveBeenPlayedStory",
		                                   delegate(com.nucleus.commons.message.GeneralResponse e) {

											if(e is PlayerMissionDto){

												if(missionStoryPlayCallBack != null){
													missionStoryPlayCallBack(e as PlayerMissionDto);
													missionStoryPlayCallBack = null;
												}

												MissionDataModel.Instance.StoryEndPlotCallback(e as PlayerMissionDto);

											}

											else if(e is SceneDto){

											SceneDto dto = e as SceneDto;
											Npc npc = new Npc();
											npc.x = dto.sceneMap.x;
											npc.z = dto.sceneMap.y;
											npc.sceneId =dto.id;
											WorldManager.Instance.WalkToByNpc(npc);

											}
		}
		);
	}
	#endregion


	#region 登陆时候的剧情播放
	public  int LastPlotId;
	public void JudgeStory(){
		if(LastPlotId >0){
			PlayStory(LastPlotId);
			LastPlotId = 0;
		}
	}
	#endregion
}
