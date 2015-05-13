using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ServerListController : MonoBehaviour, IViewController
{
	private ServerListView _view;
	private GameObject _serverListItemPrefab;
	private Dictionary<int, ServerListItem> _serverListItemDict = null;
	private Action<ServerInfo> _onSelectCallBack;
	
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<ServerListView> ();
		_view.Setup (this.transform);

		_serverListItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/ServerListModule/ServerListItem") as GameObject;

		_serverListItemDict = new Dictionary<int, ServerListItem> ();
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.CloseButton.onClick, OnCloseButtonClick);
	}

	private void OnCloseButtonClick()
	{
		ProxyServerListModule.Close ();
	}

	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose(){
		_serverListItemDict.Clear ();
	}


	public void Open(Action<ServerInfo> selectCallback)
	{
		if (_view == null)
		{
			InitView();
			RegisterEvent();
		}

		_onSelectCallBack = selectCallback;

		RefreshServerListItem ();
	}

	private void RefreshServerListItem()
	{
		GameDebuger.Log("Refresh the ServerList");
		_serverListItemDict.Clear();
		//删除原本的子列表

		_view.ServerListTable.gameObject.RemoveChildren ();
		_view.LastLoginTable.gameObject.RemoveChildren ();
		
		string lastServerId = PlayerPrefs.GetString("lastServerId");
		string preServerId = PlayerPrefs.GetString("preServerId");
		
		bool defaultServer = false;
		
		//更新服务器列表,如果存在默认选默认，否则选择最后一个推荐的服务器
		if (ServerConfig.serverList.Count > 0)
		{
			GameDebuger.Log("ServerList Count="+ServerConfig.serverList.Count);
			
			if (_serverListItemPrefab != null)
			{
				//推荐服务器
				ServerInfo recommendServerMsg = null;
				ServerInfo lastServerMsg = null;
				//临时
				ServerListItem tmpServerItem = null;
				
				//初始化ServerListTable
				//ServerConfig.SortServerList();  //取消客户端排序操作，服务端控制
				for (int i = 0; i < ServerConfig.serverList.Count; i++)
				{
					ServerInfo serverMsg = ServerConfig.serverList[i];
					
					//GameDebuger.Log("service " + i + " : " + serverMsg.GetServerUID());
					
					//版本控制flag
					bool passVerLimit = true;
					//                    if (ServiceProviderManager.HasSP() && Version.Release)
					//                    {
					//                        passVerLimit = (Version.ver >= serverMsg.limitVer && serverMsg.limitMaxVer >= Version.ver);
					//                    }
					
					if (serverMsg != null && serverMsg.dboState != 0 && passVerLimit)
					{
						//加入到所有服务器Table中
						GameObject item = NGUITools.AddChild(_view.ServerListTable.gameObject, _serverListItemPrefab);
						tmpServerItem = item.GetComponent<ServerListItem>();
						tmpServerItem.Setup(serverMsg, OnSelectServer);
						
						//最近登录列表中的服务器，只有处于“开放”状态的才加进去
						if (preServerId == serverMsg.GetServerUID() && serverMsg.dboState == 1)
						{
							GameObject tmp = NGUITools.AddChild(_view.LastLoginTable.gameObject, _serverListItemPrefab);
							ServerListItem serverItem = tmp.GetComponent<ServerListItem>();
							serverItem.Setup(serverMsg, OnSelectServer);
						}
						
						if (lastServerId == serverMsg.GetServerUID() && serverMsg.dboState == 1)
						{
							GameObject tmp = NGUITools.AddChild(_view.LastLoginTable.gameObject, _serverListItemPrefab);
							ServerListItem serverItem = tmp.GetComponent<ServerListItem>();
							serverItem.Setup(serverMsg, OnSelectServer);
							SelectServerItem(serverItem);
							defaultServer = true;
						}
						
						if (!_serverListItemDict.ContainsKey(serverMsg.serviceId))
						{
							GameDebuger.Log("Add serviceId : " + serverMsg.serviceId);
							_serverListItemDict.Add(serverMsg.serviceId, tmpServerItem);
						}
						
						//记录一个已开放的推荐服
						if (serverMsg.recommend == 1 && serverMsg.dboState == 1)
						{
							recommendServerMsg = serverMsg;
						}
						lastServerMsg = serverMsg;
					}
				}
				
				//若无默认服务器，选择最新推荐服
				if (defaultServer == false)
				{
					if(recommendServerMsg != null)
					{
						GameObject tmp = NGUITools.AddChild(_view.LastLoginTable.gameObject, _serverListItemPrefab);
						ServerListItem serverItem = tmp.GetComponent<ServerListItem>();
						serverItem.Setup(recommendServerMsg, OnSelectServer);
						SelectServerItem(serverItem);
					}
					else //若无推荐服务器，选择最后一个服，加入最近登录列表
					{
						if (lastServerMsg != null){
							GameObject tmp = NGUITools.AddChild(_view.LastLoginTable.gameObject, _serverListItemPrefab);
							ServerListItem serverItem = tmp.GetComponent<ServerListItem>();
							serverItem.Setup(lastServerMsg, OnSelectServer);
							SelectServerItem(serverItem);
						}
					}
				}
			}
		}
		
		#if !UNITY_EDITOR
		RepositionTable();
		#endif
	}
	
	private void RepositionTable()
	{
		_view.ServerListTable.repositionNow = true;
		_view.LastLoginTable.repositionNow = true;
	}

	private void SelectServerItem(ServerListItem serverItem)
	{
		serverItem.SelectServer ();

		_currentServerInfo = serverItem.GetServerInfo ();
		
		if (_currentServerListItem != null)
			_currentServerListItem.UnSelectServer();
		_currentServerListItem = serverItem;
		
		UpdateCurrentServerMessage();
		
		if (_onSelectCallBack != null)
		{
			_onSelectCallBack(_currentServerInfo);
		}
	}

	private ServerInfo _currentServerInfo;
	private ServerListItem _currentServerListItem;
	/// <summary>
	/// 选择当前服务器
	/// </summary>
	/// <param name="message"></param>
	private void OnSelectServer(ServerListItem listItem)
	{
		SelectServerItem (listItem);

		OnCloseButtonClick ();
	}

	/// <summary>
	/// 更新当前服务器信息
	/// </summary>
	private void UpdateCurrentServerMessage()
	{
		if (_currentServerInfo == null)
			return;

		_view.CurServerListItem.gameObject.SetActive (true);
		_view.CurServerListItem.Setup (_currentServerInfo, null);
	}
}

