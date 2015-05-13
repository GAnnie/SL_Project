using UnityEngine;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.commons.message;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class BattleManager
{
	private static readonly BattleManager instance = new BattleManager();
    public static BattleManager Instance
    {
        get
		{
			return instance;
		}
    }

	public static bool NeedBattleMap = false;

	public BattleManager()
	{
	}
	
    private Video _currentGameVideo;

	private List<object> _battleRewardNotifyList = new List<object>();
	
	#region Battle FinisHCallBack
	public event System.Action<bool> OnBattleFinishCallback;
	#endregion

	public int curVillageId { get ; private set;}

	private bool _isInBattle;
	public bool IsInBattle(){
		return _isInBattle;
	}

	public void AddBattleRewardNotiryList(object notify)
	{
		_battleRewardNotifyList.Add (notify);
	}

	public void PlayBattle(Video gv)
	{
		//AudioManager.Instance.PlaySound("sound_UI/sound_battle_start");
		//PlayerView.StopStepSound();
		_isInBattle = true;

		UIBattleStatusEffect.InitEffect ();

		if (WorldManager.Instance.GetHeroView() != null)
		{
			WorldManager.Instance.GetHeroView ().StopAndIdle ();
		}

		if ( gv == null )
		{
			GameDebuger.Log("Can't get Video");
			NGUIFadeInOut.FadeIn();
			return;
		}

		MainUIViewController.Instance.ChangeMode(UIMode.Mode_Battle);

		_currentGameVideo = gv;

		//答应战斗具体的信息
		GameDebuger.Log(GetBattleInfo());

//        if (UIManager.UILOCK)
//        {
//			Debug.LogError("Can't enter battle (UIManager.UILOCK = true)");
//			GameDebuger.Log("Can't enter battle (UIManager.UILOCK = true)");
//			//如果进入不了战斗,就把战斗临时保存起来,进入场景后再进入
//			tempGameVideo = gv;
//            NpcView.EnableTrigger = true;
//			GamePauseController.Instance.Play();
//            return;
//        }
		
		//LayerManager.Instance.GetPlayerNameCamera().SetActive(false);
		
        LayerManager.UILOCK = true;
        //NpcView.EnableTrigger = false;
		
//      UIManager.Instance.HideMainUI();
		
        //UIManager.Instance.UIMode = UIManager.UI_MODES.BATTLE;
		
//		if(UIManager.Instance.GetMainUIObj().activeSelf)
//			_origCameraConfigInfo = LayerManager.Instance.getWorldCamera().GetComponent<CameraController>().camInfo;

//		string battleSceneName = WorldManager.Instance.getWorldModel().GetBattleSceneName( _currentGameVideo.mapId );
//		SceneChangeEffect.LoadBattleScene(battleSceneName, _currentGameVideo, onStartMapLoad, OnBattleMapLoaded);

//        if (NGUIFadeInOut.IsNotAllBlack())
//        {
//            if (WorldManager.Instance.getWorldModel() != null)
//            {
//                //PlayBattleEffect();
//                //PlayBattleEffect2();
//            }
//            AppUtils.FadeOut(OnFinishFadeEffect, "进入战斗");
//        }
//        else
//        {
//            OnFinishFadeEffect();
//        }

		if (SystemDataModel.Instance.battleMapToggle && _currentGameVideo.mapId != 0)
		{
			NeedBattleMap = true;

			WorldManager.Instance.HideScene ();
			
			WorldMapLoader.Instance.loadMapFinish = OnBattleMapLoaded;
			WorldMapLoader.Instance.LoadBattleMap (_currentGameVideo.mapId);
		}
		else
		{
			NeedBattleMap = false;

			WorldManager.Instance.HideScene ();
			OnBattleMapLoaded();
		}
	}

	private void onStartMapLoad()
	{
		OnFinishFadeEffect();
	}

    private void OnFinishFadeEffect()
    {
		//PlayerModel.Instance.CheckPlayerStateLeaveScene();

//		ProxyMainModule.Close();
		
//		LayerManager.Instance.getWorldCamera().GetComponent<CameraController>().camInfo = null;
		//WorldManager.Instance.GetWorldView().CameraController.cameraMove.enabled = false;
		
		//UIManager.Instance.EnterBattleMode();
		
		//WorldManager.Instance.HideScene();
		
		//string battleSceneName = WorldManager.Instance.getWorldModel().GetBattleSceneName( curVillageId );
		//SceneChangeEffect.LoadBattleScene(battleSceneName, _currentGameVideo, OnBattleMapLoaded);
	    //BattleSceneLoader.Instance.LoadScene(battleSceneName, _currentGameVideo, OnBattleMapLoaded);
    }
	
	private void OnBattleMapLoaded()
    {
		WorldMapLoader.Instance.loadMapFinish = null;
		WorldMapLoader.Instance.SetReady(true);
		/*
		GameObject battleState = GameObject.Find("BattleStage");
		if (battleState != null)
		{
			GameObjectExt.AddPoolChild(LayerManager.Instance.World, battleState);
		}
		*/
		BattleController.Setup(_currentGameVideo);

		if (SystemDataModel.Instance.battleMapToggle && _currentGameVideo.mapId != 0)
		{
			//场景编号:1\2\3  3D无镜头场景、3D普通镜头场景、3DBOSS镜头场景
			WorldManager.Instance.PlayCameraAnimator (_currentGameVideo.mapId, _currentGameVideo.cameraId-1);
		}

		NGUIFadeInOut.FadeIn();
    }

    private void PlayBattleMusic()
    {
        string music = null;

//        if (_currentGameVideo is PveGameVideo)
//        {
//            PveGameVideo pve = (_currentGameVideo as PveGameVideo);
//
//            Tollgate tollgate = (pve.npcMonster as NpcMonster).tollgate;
//            if (tollgate != null)
//            {
//                music = tollgate.battleMusic;
//            }
//        }

        if (string.IsNullOrEmpty(music))
        {
			int index = Mathf.CeilToInt(Random.value*3);
            music = "music_scenes/music_battle_"+index;
        }

		//AudioManager.Instance.MusicVolume = 0.6f;
        AudioHelper.PlayMusic(music, "music_scenes/music_battle_1");
    }

	private void SetBattleMapFog()
	{
//		string battleSceneName = WorldManager.Instance.getWorldModel().GetBattleSceneName(_currentGameVideo.mapId);
//        MapDataInfo dataInfo = MapLoader.Instance.loadData(battleSceneName);
//		AppUtils.SetMapFog(battleSceneName, dataInfo);
	}

//    public void SetDrawResult(DrawResult result)
//    {
//        _drawResult = result;
//    }

    private bool _needShowEquip = false;
    public void NeedShowEquip(bool show)
    {
        _needShowEquip = show;
    }
	
	private BattleController.BattleResult _battleResult;
	private GeneralResponse _result;
	public void BattleDestroy(BattleController.BattleResult battleResult, Video gameVideo)
	{

		if (gameVideo is SceneMineVideo && battleResult == BattleController.BattleResult.LOSE)
		{
			BackpackModel.Instance.BattleFail();
		}

		//退出战斗时把战斗场景销毁
		GameObject go = GameObject.Find( "BattleStage" );
		if ( go != null ){
			GameObject.Destroy(go);
		}

		//force  save
		PlayerPrefs.Save ( );
		
		_battleResult = battleResult;
		
        LayerManager.UILOCK = false;

		//AudioManager.Instance.StopMusic();

		//UIManager.Instance.ShowChatLeftGroup();

//		if(restoreScene)
//		{
			//LayerManager.Instance.GetPlayerNameCamera().SetActive(true);
			//WorldManager.Instance.RestoreScene( HandleBattleFinish);
//		}

//		ProxyBattleDemo.Open();

		WorldManager.Instance.ResumeScene ();
		NGUIFadeInOut.FadeIn ();
		_isInBattle = false;

		foreach (object notify in _battleRewardNotifyList)
		{
			if (notify is CharactorExpNotify)
			{
				GameLogicHelper.UpdateCharactorExp(notify as CharactorExpNotify);
			}
			else if (notify is WealthNotify)
			{
				PlayerModel.Instance.UpdateWealth(notify as WealthNotify);
			}
			else if (notify is SubWealthNotify)
			{
				PlayerModel.Instance.UpdateSubWealth(notify as SubWealthNotify);
			}
			else if (notify is PackItemNotify)
			{
				PackItemNotifyListener.HandlePackItemNotify(notify as PackItemNotify);
			}
		}
		ChatModel.Instance.AddSysNotifyAffterBattle (_battleRewardNotifyList);
		_battleRewardNotifyList.Clear ();

		if (battleResult == BattleController.BattleResult.LOSE)
		{
			TipManager.AddTip("胜败乃兵家常事，提升能力继续挑战！");
		}
		else if (battleResult == BattleController.BattleResult.Retreat)
		{
			TipManager.AddTip("由于你逃跑了，战斗失败");
		}

		if( OnBattleFinishCallback != null )
		{
			OnBattleFinishCallback( battleResult == BattleController.BattleResult.WIN );
		}

		_currentGameVideo = null;
	}
	
	private void HandleBattleFinish()
	{
		//UIManager.Instance.ExitBattleMode();

//        LayerManager.Instance.getBattleLayer().SetActive(false);
		
//        if (WorldManager.Instance.getWorldModel() != null)
//        {
//            AppUtils.SetMapFog( WorldManager.Instance.getWorldModel().GetReourceID(), WorldManager.Instance.getWorldModel().GetMapRenderInfo());
//			LayerManager.Instance.getWorldCamera().GetComponent<CameraController>().setCamera( LayerManager.Instance.getWorldCameraObj(), _origCameraConfigInfo, null, true);
//        }

		//UIManager.Instance.ShowMainUI();


		_currentGameVideo = null;
	}
	
	public Video GetCurrentGameVideo(){
		return _currentGameVideo;
	}
	
	public string GetBattleInfo(){
		string info = "";
		
		if (_currentGameVideo != null){
			info += " 当前战斗:" + _currentGameVideo.id;
		}
		
		return info;
	}

	public void Destroy()
	{
		if (BattleController.Instance != null)
		{
			BattleController.Instance.Destory ();
		}

		//退出战斗时把战斗场景销毁
		GameObject go = GameObject.Find( "BattleStage" );
		if ( go != null ){
			GameObject.Destroy(go);
		}
		
		//force  save
		PlayerPrefs.Save ( );

		LayerManager.UILOCK = false;

		LayerManager.Instance.SwitchLayerMode (UIMode.Mode_Game);
		_isInBattle = false;

		_currentGameVideo = null;
		_battleRewardNotifyList.Clear ();
	}
	
	//	//是否在战斗模式
//	public static bool IsBattlePlaying(){
//		return true;
//		//return UIManager.Instance.UIMode == UIManager.UI_MODES.BATTLE;
//	}	

	public static int GetNormalAttackSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_ACTIVE_SKILL_ID, 1);
	}
	
	public static int GetDefenseSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_DEFENSE_SKILL_ID, 2);
	}

	public static int GetProtectSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_PROTECT_SKILL_ID, 3);
	}

	public static int GetCaptureSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_CAPTURE_SKILL_ID, 4);
	}

	public static int GetSummonSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_SUMMON_SKILL_ID, 5);
	}

	public static int GetRetreatSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.DEFAULT_RETREAT_SKILL_ID, 6);
	}

	public static int GetUseItemSkillId()
	{
		return DataHelper.GetStaticConfigValue (H1StaticConfigs.USE_ITEM_SKILL_ID, 7);
	}
}