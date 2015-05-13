using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.scene.dto;
using com.nucleus.h1.logic.core.modules.scene.data;
using System;

public class LoginManager
{
    /**
     *登录管理器
     * @author senkay
     * @date Nov 6, 2010
     */
    public static readonly string ERROR_time_out = "链接超时";
    public static readonly string ERROR_socket_error = "网络错误";
    public static readonly string ERROR_socket_close = "网络已断开";
    public static readonly string ERROR_sid_error = "用户账号错误";
    public static readonly string ERROR_user_invalid = "用户无效";

    private static readonly LoginManager instance = new LoginManager();
    public static LoginManager Instance
    {
        get
        {
            return instance;
        }
    }
	
    private string _token;
    private bool _keepSocket;

    private static int MAX_TRY_COUNT = 5;
    private int _tryCount;

    public List<ServiceInfo> serviceInfoList = new List<ServiceInfo>();

	public event Action onHaConnected;
	public event Action finishedLogin;
	public event Action callOnTokenExit;
	public event Action<string> loginMessage;
	public event Action<float> loginMsgProcess;

//    public delegate void LoginQueue(LoginQueuePlayerDto dto);

//    public LoginQueue loginQueue;

    //	public delegate void CallOnTokenNotExist();
    //	public CallOnTokenNotExist callOnTokenNotExist;

	private ServerInfo _serverInfo;

    private bool _reLogin = false;

    //是否重复登录
    public static bool DuplicateLogin = false;

	//断线重连成功后的回调//
	public delegate void CallBackReLoginSuccess();
	public CallBackReLoginSuccess _callBackReLoginSuccess;


    public LoginManager()
    {
        _keepSocket = true;
        _tryCount = MAX_TRY_COUNT;
    }

	public string loginId
	{
		get;
		set;
	}

    public string GetPlayerID()
    {
        return _token;
    }

	public void start(string token, ServerInfo serverInfo)
    {
		_reLogin = false;

		_serverInfo = serverInfo;

//        if (ServiceProviderManager.HasSP())
//        {
//            _token = null;
//        }
//        else
//        {
            _token = token;
//        }

        ActionCallbackManager.init();

		SocketManager.Instance.Setup();
		SocketManager.Instance.OnHAConnected += HandleOnHAConnected;
		SocketManager.Instance.OnHaError += HandleOnHaError;
		SocketManager.Instance.GetHaConnector().OnStateEvent += HandleOnStateEvent;

		GameDebuger.Log("Login With " + token + " At " + _serverInfo.host + ":" + _serverInfo.port + " accessId=" + _serverInfo.accessId + " gameServerId=" + _serverInfo.serviceId);
        debugLog("连接服务器...");

		Connect();
    }

	
	private uint haState = HaStage.LOGINED;
	void HandleOnStateEvent(uint state)
	{
		haState = state;
		if (haState == HaStage.LOGINED && _playerDto != null)
		{
			DoLogin(_playerDto);
		}
	}

	void HandleOnHAConnected ()
	{
		GameDebuger.Log("OnHAConnected");
		
		showMessageBox("账号验证中，请稍候。。。");

//		if (_token == null){
//			if (Version.ClientMode == AgencyPlatform.ClientMode_gmtest){
//				PlayerPrefs.SetString("gm_name", GmAccountInputController.name);
//				PlayerPrefs.SetString("gm_password", GmAccountInputController.password);
//				
//				ServerManager.Instance.RequestGmToken(OnRequestTokenCallback, GmAccountInputController.name, MD5.Md5_str(GmAccountInputController.password));
//			}else{
//				ServerManager.Instance.RequestToken(OnRequestTokenCallback);
//			}
//		}else{
			OnRequestTokenCallback(_token, "");
//		}
		
		ExitGameScript.CheckConnected = true;
	}

	void HandleOnHaError (string msg)
	{
		WindowManager.showMessageBox(msg, delegate() {
			GotoLoginScene();
		}, false);

		showMessageBox(msg);
	}

    private void Connect()
    {
		SocketManager.Instance.Connect(_serverInfo);
    }

    public string GetLoginToken()
    {
        return _token;
    }

    public void OnRequestTokenCallback(string token, string errorMsg)
    {
        _token = token;
        if (_token == null)
        {
            showMessageBox("账号验证失败:" + errorMsg);

			WindowManager.showMessageBox("账号验证失败:" + errorMsg, delegate() {
				GotoLoginScene();
			}, false);
        }
        else
        {
			LoadData();
        }
    }

    private void __OnHATimeOut()
    {
        if (reTryConnect() == false)
        {
            showMessageBox(ERROR_time_out);
        }
        else
        {

        }
    }

    private void __OnSocketErr()
    {
        if (reTryConnect() == false)
		{
            showMessageBox(ERROR_socket_error);
			ExitGameScript.CheckReloginWhenConnectClose(ExitGameScript.CheckConnected==false);
		}
    }

    private void __OnSocketClose()
    {
        if (reTryConnect() == false)
        {
            showMessageBox(ERROR_socket_close);
			ExitGameScript.CheckReloginWhenConnectClose(ExitGameScript.CheckConnected==false);
        }
    }

    public void Relogin()
    {
        _reLogin = true;
		Connect();
    }

    private void __OnSidError()
    {
        showMessageBox(ERROR_sid_error);
    }

    private bool reTryConnect()
    {
        _tryCount--;
        if (_tryCount < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void FetchServerInfo(ArrayList _serviceInfoList)
    {
        if (serviceInfoList.Count > 0)
            serviceInfoList.Clear();

        foreach (ServiceInfo serviceInfo in _serviceInfoList)
        {
            serviceInfoList.Add(serviceInfo);
            GameDebuger.Log(serviceInfo.id + " " + serviceInfo.name + " " + serviceInfo.doc);
        }
    }

    private void login()
    {
        if (_token != null)
        {
			if (haState == HaStage.LOGINED && _playerDto != null)
			{
				DoLogin(_playerDto);
			}
			else
			{
				debugLog("账号登录...");
				GameDebuger.Log("LoginFromIp = " + HaApplicationContext.getConfiguration().getLocalIp());

				AccountPlayerDto dto = ServerManager.Instance.HasPlayerAtServer(_serverInfo.serviceId);

				if ( dto != null )
				{
					ServiceRequestAction.requestServer(PlayerLoginService.login(_token, HaApplicationContext.getConfiguration().getLocalIp(), dto.id), "账号重新登录",
					                                   OnLogin, OnNotLogin);
				}
				else
				{
					ProxyRoleCreateModule.Open(_serverInfo, CreatePlayerSuccess);
				}
			}
        }
    }

	private void CreatePlayerSuccess(GeneralResponse e)
	{
		OnLogin(e);
	}
	//-------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------
    private void OnNotLogin(ErrorResponse e)
    {
        GameDebuger.Log("OnNotLogin: ErrorResponse Message:" + e.message);
        debugLog("登录失败！");

		WindowManager.showMessageBox(string.Format("登录失败！({0})", e.message), delegate() {
			GotoLoginScene();
		}, false);
    }

	private PlayerDto _playerDto = null;

    void OnLogin(GeneralResponse e)
    {
        //是否需要排队
        if (e is PlayerDto)
        {
			if (haState == HaStage.LOGINED)
			{
				GameDebuger.Log("登录成功");
				DoLogin(e as PlayerDto);
			}
			else
			{	
				GameDebuger.Log("等待HaStage.LOGINED");
				_playerDto = e as PlayerDto;
			}
        }
//        else if (e is LoginQueuePlayerDto)
//        {
//            GameDebuger.LogTime("登陆排队");
//            debugLog("");
//
//            LoginQueuePlayerDto loginQueueDto = e as LoginQueuePlayerDto;
//
//            XSocketMsgSyn.CheckConnected = false;
//
//            destroy(true);
//
//            //进行排队操作
//            if (loginQueue != null)
//            {
//                loginQueue(loginQueueDto);
//            }
//        }
    }

	private void DoLogin(PlayerDto playerDto)
	{
		if(playerDto.sceneId == 0)
		{
			Debug.LogError("set test data PlayerDto.sceneId = 1003");
			playerDto.sceneId = 1003;
		}
		PlayerModel.Instance.Setup(playerDto);

		if (_reLogin == false)
		{
			AfterLogin();
		}
	}

    void OnReLogin(GeneralResponse e)
    {
		PlayerDto playerDto = e as PlayerDto;

		if (playerDto != null)
		{
			GameDebuger.Log("重新登录成功");
			TipManager.AddTip("重新登录成功");
			PlayerModel.Instance.Setup(playerDto);

			//统一在重新登陆后调用logon， 来获取新的信息, 例如战斗
			//ServiceRequestAction.requestServer(PlayerService.logon());
			CallBackReLogin();

			//WorldManager.Instance.ReEnterScene();
		}
		else
		{
            GameDebuger.Log("重新登录失败");

            WindowManager.showMessageBox("重新登陆失败， 请重新进入游戏", delegate()
            {
                ExitGameScript.Instance.HanderRelogin();
            });
		}
    }

    public void LoadData()
    {
		DataManager dm = DataManager.Instance;
        dm.dataLoadingMessge = DataLoadingMessage;
		dm.dataLoadingMsgProcess = DataLoadingMsgProcess;
        if (callOnTokenExit != null)
        {
            callOnTokenExit();
        }
        if (dm != null)
        {
            dm.Initialize(GetStaticDataSuccess);
        }
    }

    private void DataLoadingMessage(string msg)
    {
        if (loginMessage != null)
        {
            loginMessage(msg);
        }
    }

	private void DataLoadingMsgProcess(float msgProcess) {
		if (loginMsgProcess != null)
		{
			loginMsgProcess(msgProcess);
		}
	}

    public void GetStaticDataSuccess()
    {
		login ();
    }

	private void AfterLogin()
	{
		NotifyListenerRegister.Setup();
		AssetbundleManager.Instance.Setup();
		
		//debugLog("获取角色数据。。。");

		ServiceRequestAction.requestServer(PlayerService.afterLogin(), "AfterLogin", (e) => {
			AfterLoginDto afterLoginDto = e as AfterLoginDto;
			
			//	系统设置(是否开启声音、3d镜头、语音大小......)
			SystemDataModel.Instance.GetSystemDataFormLocal();

			/** 玩家相关信息 */
			PlayerModel.Instance.UpdateInfoWithAfterLoginDto(afterLoginDto);

			/** 门派技能信息 */
			FactionSkillModel.Instance.Setup(afterLoginDto.factionSkillsInfo);
			/** 修炼技能信息 取消 需要动态获取*/
			//SpellModel.Instance.Setup(afterLoginDto.spells);
			/** 辅助技能信息 */
			AssistSkillModel.Instance.Setup(afterLoginDto.assistSkillInfo);

			/** 服务器等级信息 */
			PetModel.Instance.Setup(afterLoginDto.petCharactorDtos,afterLoginDto.companyPetVacancy);

			/** 玩家已赠送数量-价值信息 */
			EmailModel.Instance.SetCount_Value(afterLoginDto.giftDto);

			/** 玩家状态栏信息 */
			PlayerBuffModel.Instance.Setup(afterLoginDto.stateBarDtos);

			PlayerModel.Instance.Merits = afterLoginDto.merits;


			// 获取邮件
			EmailModel.Instance.Setup();
			//获取聊天记录需要自身id
			ChatModel.Instance.WorldChatCoolDown();
			FriendModel.Instance.GetFriends();
			ReadyMapRes();

			//登陆播放剧情
			StoryManager.Instance.LastPlotId = afterLoginDto.plotId;
		});

        GameDataManager.Instance.Setup(() =>
        {
            BackpackModel.Instance.Setup();
            WarehouseModel.Instance.Setup();
        });

//		EmailModel.Instance.Setup ();



		//	获取任务数据
		MissionDataModel.Instance.EnterMission();
	}

	private void ReadyMapRes()
	{
		debugLog("载入场景资源。。。");
		int sceneId = PlayerModel.Instance.GetPlayer ().sceneId;
		SceneMap sceneMap = DataCache.getDtoByCls<SceneMap>(sceneId);
		WorldMapLoader.Instance.loadMapFinish = OnLoadMapFinish;
		WorldMapLoader.Instance.loadLevelProgress = OnLoadLevelProgress;
		
		WorldMapLoader.Instance.LoadWorldMap(sceneMap.resId);
	}

	private void OnLoadMapFinish()
	{
		WorldMapLoader.Instance.loadMapFinish = null;
		WorldMapLoader.Instance.loadLevelProgress = null;
		WorldMapLoader.Instance.SetReady(true);

		if (WorldManager.FirstEnter)
		{
			NGUIFadeInOut.FadeOut (delegate() {
				ExitLoginScene();
				if (finishedLogin != null)
					finishedLogin();
			});
		}
		else
		{
			ExitLoginScene();
			if (finishedLogin != null)
				finishedLogin();
		}
	}

	private void OnLoadLevelProgress(float precent)
	{
		//Debug.Log("OnLoadLevelProgress " + precent);
	}

    private void showMessageBox(string msg)
    {
        debugLog(msg);
        _keepSocket = false;
    }

    private void debugLog(string msg)
    {
        GameDebuger.Log(msg);
        if (loginMessage != null)
        {
            loginMessage(msg);
        }
    }

    public bool keepSocket()
    {
        return _keepSocket;
    }

	public void RemoveListener()
	{
		SocketManager.Instance.OnHAConnected -= HandleOnHAConnected;
		SocketManager.Instance.OnHaError -= HandleOnHaError;
		SocketManager.Instance.GetHaConnector().OnStateEvent -= HandleOnStateEvent;
	}
	
	public void destroy(bool close)
    {
        _tryCount = 0;
        ActionCallbackManager.destroy();

		RemoveListener();
		
		if (close)
		{
			SocketManager.Instance.Close(true);
		}
        
		SocketManager.Instance.Destroy();
    }

	public void CallBackReLogin()
	{
		//重新登录后的回调//
		if( _callBackReLoginSuccess != null )
		{
			_callBackReLoginSuccess();
		}
	}

	//open loginScene
	public void GotoLoginScene()
	{
		GameDebuger.Log("GotoLoginScene!!!");
		
		if (AppManager.Instance == null)
		{
			ExitGameScript.CheckConnected = false;
			LoginManager.Instance.destroy(true);

			ProxyLoginModule.Open();
		}
		else
		{
			ExitGameScript.CheckConnected = false;
			LoginManager.Instance.destroy(true);
			
			ProxyLoginModule.Open();
		}
	}


	public static void ExitLoginScene(){
		ProxyLoginModule.Close ();
		AppManager.InitAppManager();
	}
}
