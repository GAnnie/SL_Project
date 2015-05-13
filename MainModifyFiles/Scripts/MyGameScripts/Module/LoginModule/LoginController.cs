using UnityEngine;
using System.Collections;

public class LoginController : MonoBehaviour,IViewController
{
	private LoginView _view;

	#region IViewController
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<LoginView> ();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.StartGameButton_UIButton.onClick, OnClickStartGameButton);
		EventDelegate.Set (_view.LastLoginInfo_UIButton.onClick, OnClickLastLoginInfoButton);

		ServerConfig.onServerListReturn += OnServerListReturn;

		LoginManager.Instance.loginMessage += OnLoginMessage;
		LoginManager.Instance.loginMsgProcess += OnLoginMsgProcess;
		LoginManager.Instance.callOnTokenExit += OnTokenExit;

	}

	public void Dispose(){
		ServerConfig.onServerListReturn -= OnServerListReturn;

		LoginManager.Instance.loginMessage -= OnLoginMessage;
		LoginManager.Instance.loginMsgProcess -= OnLoginMsgProcess;
		LoginManager.Instance.callOnTokenExit -= OnTokenExit;
	}
	#endregion

	private ServerInfo _currentServerInfo;
	private bool _isLogined = false;

	public void Open()
	{
		if (_view == null)
		{
			InitView();
			RegisterEvent();
			ServerConfig.Setup();
		}

		_isLogined = false;
		_currentServerInfo = null;
		_loginMessage = "";
		_loginMsgProcess = 0f;

		_view.VersionLabel_UILabel.text = Version.GetShowVersion ();

		_view.LoadingProgressBar_UISlider.sliderValue = 0;
		_view.LoadingLabel_UILabel.text = "";
		_view.LoadingTips_UILabel.text = LoadingTipManager.GetLoadingTip();

		_view.LoadingPanel_Transform.gameObject.SetActive(false);
		_view.StartPanel_Transform.gameObject.SetActive(true);

		//Texture2D tex = ResourceLoader.Load( "Textures/LoadingBG/loading2", "png" ) as  Texture2D;
		//_view.LoadingTexture_UITexture.mainTexture = tex;

//		Transform trans = this.transform;
//		float factor = UIRoot.GetPixelSizeAdjustment (this.gameObject);
//		
//		float newHeight = Screen.height * factor;
//
//		_view.LoadingTexture_UITexture.height = (int)newHeight;

		if (Version.ClientMode == AgencyPlatform.ClientMode_test || Version.ClientMode == AgencyPlatform.ClientMode_release )
		{
			_view.NameInput_UIInput.value = PlayerPrefs.GetString("dev_name");
		}
		else if (Version.ClientMode == AgencyPlatform.ClientMode_gmtest)
		{
			_view.NameInput_UIInput.value = PlayerPrefs.GetString("gm_name");
		}

		OnServerListReturn ();
	}

	private void OnServerListReturn()
	{
		if(!string.IsNullOrEmpty( PlayerPrefs.GetString("lastServerId") ))
		{
			OnSelectionChange(PlayerPrefs.GetString("lastServerId"));
		}
	}

	private void OnClickStartGameButton()
	{
		bool passCheck = true;
		string name = _view.NameInput_UIInput.value;
		
		if (Version.ClientMode == AgencyPlatform.ClientMode_test || Version.ClientMode == AgencyPlatform.ClientMode_release )
		{
			if (string.IsNullOrEmpty(name))
			{
				TipManager.AddTip("提示：请先输入账号");
				passCheck = false;
			}
			else if (name.Length > 20)
			{
				TipManager.AddTip("提示：用户名最多20个字符");
				passCheck = false;
			}
			else if (StringHelper.ValidateAccount(name) == false)
			{
				TipManager.AddTip("提示：用户名含有非法字符");
				passCheck = false;
			}
		}
		else if (Version.ClientMode == AgencyPlatform.ClientMode_gmtest)
		{
			//			string password = GmAccountInputController.password;
			//			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)){
			//				passCheck = false;
			//			}			
		}
		
		if (_currentServerInfo == null)
		{
			TipManager.AddTip("提示：选择服务器为空");
			passCheck = false;
		}
		
		if (passCheck)
		{
			if (Version.ClientMode == AgencyPlatform.ClientMode_test)
			{
				PlayerPrefs.SetString("dev_name", name );
			}
			
			if (_currentServerInfo != null)
			{
				if (_currentServerInfo.host == "t1.baoyugame.com")
				{	
					GameInfo.SDK_SERVER = "http://t1.baoyugame.com/h1";
					GameInfo.SSO_SERVER = "http://t1.baoyugame.com/h1";
				}else{
					GameInfo.SDK_SERVER = "http://192.168.1.97:9993";
					GameInfo.SSO_SERVER = "http://192.168.1.97:9999";
				}
			}
			
			RequestSdkAccountLogin(name);
		}
	}

	private void RequestSdkAccountLogin(string deviceId)
	{
		ServiceProviderManager.RequestSdkAccountLogin(deviceId, GameInfo.AppId, GameInfo.CpId, delegate(AccountResponse response) {
			LoginManager.Instance.loginId = deviceId;
			if (response.code == 0)
			{
				RequestSsoAccountLogin(response.data);
			}
			else
			{
				WindowManager.showMessageBox(response.msg);
			}
		});
	}
	
	private void RequestSsoAccountLogin(AccountSession session)
	{
		ServerManager.Instance.accountSession = session;
		
		Debug.Log("sid = " + session.sid);
		
		ServiceProviderManager.RequestSsoAccountLogin(session.sid, GameInfo.Channel, GameInfo.SubChannel, GameInfo.LoginWay, delegate(LoginAccountDto response) {
			if (response.code == 0)
			{
				ServerManager.Instance.loginAccountDto = response;
				
				string token = response.token;
				Debug.Log("token = " + token);
				LoginTo (token);
			}
			else
			{
				WindowManager.showMessageBox(response.msg);
			}
		});
	}
	
	public void LoginTo(string token)
	{
		if (_currentServerInfo == null){
			//OnClickBeginBtn();
		}else{
			if (_isLogined == false)
			{
				_isLogined = true;

				_view.StartPanel_Transform.gameObject.SetActive(false);
				
				NGUIFadeInOut.FadeOut(delegate() 
				                      {
					//Texture2D tex = ResourceLoader.Load( "Textures/LoadingBG/loading_bg", "png" ) as  Texture2D;
					//_view.LoadingTexture_UITexture.mainTexture = tex;

					_view.LoadingPanel_Transform.gameObject.SetActive(true);
					NGUIFadeInOut.FadeIn();

					LoginManager.Instance.start(token, _currentServerInfo);
				}, "", 0.5f);
			}
		}
	}
	
	private string _loginMessage = "";
	void OnLoginMessage(string msg)
	{
		_loginMessage = msg;
	}
	
	private float _loginMsgProcess = 0;
	void OnLoginMsgProcess(float msgProcess) {
		_loginMsgProcess = msgProcess;
	}
	
	public void OnSelectionChange (string val)
	{
		UpdateCurrentServerInfo (ServerConfig.GetServerInfo(val));
	}

	private void UpdateCurrentServerInfo(ServerInfo serverInfo)
	{
		if (serverInfo == null)
		{
			return;
		}

		PlayerPrefs.SetString ("lastServerId", serverInfo.GetServerUID());

		_currentServerInfo = serverInfo;
		GameDebuger.Log("OnSelectionChange = "+_currentServerInfo.name);
		ServerManager.Instance.SetServerInfo(serverInfo);

		_view.LastLoginInfo_label_UILabel.text = ServerNameGetter.GetServerName(_currentServerInfo);
		_view.LastLoginInfo_state_UISprite.spriteName = ServerNameGetter.GetServiceStateSpriteName( _currentServerInfo );
	}

	private void OnClickLastLoginInfoButton()
	{
		ProxyServerListModule.Open ((serverInfo)=>{
			UpdateCurrentServerInfo (serverInfo);
		});
	}

	private void OnTokenExit()
	{
	}


	void Update()
	{
		_view.LoadingLabel_UILabel.text = _loginMessage;
		_view.LoadingProgressBar_UISlider.value = _loginMsgProcess;
	}
}

