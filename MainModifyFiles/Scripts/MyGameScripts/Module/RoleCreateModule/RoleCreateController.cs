using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using System;
using com.nucleus.h1.logic.core.modules.charactor.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.faction.data;

public class RoleCreateController : MonoBehaviour,IViewController
{
	private RoleCreateView _view;

	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<RoleCreateView> ();
		_view.Setup (this.transform);

		RegisterEvent ();
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent()
	{
		EventDelegate.Set (_view.StartGameButton.onClick, OnStartGameButtonClick);
		EventDelegate.Set (_view.ServerNameButton.onClick, OnServerNameButtonClick);
	}

	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose()
	{
	}

	private ServerInfo _serverInfo;
	private Action<GeneralResponse> _onCreatePlayerSuccess;

	public void SetData(ServerInfo info, Action<GeneralResponse> onCreatePlayerSuccess)
	{
		_serverInfo = info;
		_onCreatePlayerSuccess = onCreatePlayerSuccess;

		List<MainCharactor> newList = new List<MainCharactor> ();
		List<GeneralCharactor> list = DataCache.getArrayByCls<GeneralCharactor>();
		for (int i=0,len=list.Count; i<len; i++)
		{
			GeneralCharactor charactor = list[i];
			if (charactor is MainCharactor)
			{
				newList.Add(charactor as MainCharactor);
				//AddCharactorButton(charactor as MainCharactor);
			}
		}

		int romIndex = UnityEngine.Random.Range (0, newList.Count);
		for (int i=0,len=newList.Count; i<len; i++)
		{
			AddCharactorButton(newList[i], i == romIndex);
		}

		_view.ServerNameLabel.text = _serverInfo.name;

		_view.PlayerNameLabel.text = "";
	}

	private MainCharactor _selectCharactor;
	private UIButton _selectCharactorButton;
	
	private void AddCharactorButton(MainCharactor charactor, bool select){
		GameObject btn = DoAddButton(_view.CharactorTable.gameObject, charactor.name, "Charactor_"+charactor.id);
		UIButton uiButton = btn.GetComponent<UIButton>();
		if (select)
		{
			_selectCharactor = charactor;
			_selectCharactorButton = uiButton;
			SetButtonState(_selectCharactorButton, true);
			SelectMainCharactor(_selectCharactor);
		}
		EventDelegate.Set (uiButton.onClick, delegate() {
			if (_selectCharactorButton != null)
			{
				SetButtonState(_selectCharactorButton, false);
			}
			_selectCharactorButton = uiButton;
			SetButtonState(_selectCharactorButton, true);
			SelectMainCharactor(charactor);
		});
	}

	private void SetButtonState(UIButton button, bool select)
	{
		UISprite uisprite = button.GetComponentInChildren<UISprite>();
		if (select)
		{
			uisprite.color = ColorExt.HexToColor("10ffff");
		}
		else
		{
			uisprite.color = ColorExt.HexToColor("ffffff");
		}
	}

	private void SelectMainCharactor(MainCharactor charactor)
	{
		_selectFaction = null;
		_selectCharactor = charactor;
		_view.FactionTable.gameObject.RemoveChildren ();
		foreach(int id in _selectCharactor.factionIds)
		{
			Faction faction = DataCache.getDtoByCls<Faction>(id);
			AddFactionButton(faction);
		}
	}

	private Faction _selectFaction;
	private UIButton _selectFactionButton;

	private void AddFactionButton(Faction faction)
	{
		GameObject btn = DoAddButton(_view.FactionTable.gameObject, faction.name, "Faction_"+faction.id);
		UIButton uiButton = btn.GetComponent<UIButton>();

		EventDelegate.Set (uiButton.onClick, delegate() {
			if (_selectFactionButton != null)
			{
				SetButtonState(_selectFactionButton, false);
			}
			_selectFaction = faction;
			_selectFactionButton = uiButton;
			SetButtonState(_selectFactionButton, true);
		});
		_view.FactionTable.repositionNow = true;
	}

	public GameObject DoAddButton(GameObject parent, string label, string goName="Button"){
		GameObject go = NGUITools.AddChild(parent, (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/RoleCreateModule/RoleCreateButton"));
		go.name = goName;
		go.GetComponentInChildren< UILabel >().text = label;
		return go;
	}

	private void OnStartGameButtonClick()
	{
		if (_selectCharactor == null)
		{
			return;
		}

		if (_selectFaction == null)
		{
			TipManager.AddTip("请选择门派");
			return;
		}

		int charactorId = _selectCharactor.id;
		int factionId = _selectFaction.id;

		string token = ServerManager.Instance.loginAccountDto.token;
		ServiceRequestAction.requestServer(PlayerLoginService.create(token, HaApplicationContext.getConfiguration().getLocalIp(), charactorId, factionId, _serverInfo.serviceId), "创建角色中",
		                                   CreatePlayerSuccess, CreatePlayerFail);
	}

	private void CreatePlayerSuccess(GeneralResponse e)
	{
		if (_onCreatePlayerSuccess != null)
		{
			_onCreatePlayerSuccess(e);
			_onCreatePlayerSuccess = null;
		}

		ProxyRoleCreateModule.Close ();
	}
	
	private void CreatePlayerFail(ErrorResponse e)
	{
		TipManager.AddTip (e.message);
	}

	private void OnServerNameButtonClick()
	{
		ProxyServerListModule.Open (OnServerSelectCallback);
		//ProxyRoleCreateModule.Close ();
	}

	private void OnServerSelectCallback(ServerInfo info)
	{
		_serverInfo = info;
		_view.ServerNameLabel.text = info.name;
	}
}

