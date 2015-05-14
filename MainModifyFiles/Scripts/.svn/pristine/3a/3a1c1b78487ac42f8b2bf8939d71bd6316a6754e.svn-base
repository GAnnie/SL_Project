// **********************************************************************
// Copyright (c) 2015 cilugame. All rights reserved.
// File     : ServerListItemViewController.cs
// Author   : senkay <senkay@126.com>
// Created  : 05/13/2015 
// Porpuse  : 
// **********************************************************************
//
using UnityEngine;
using System.Collections;

public class ServerListItemController : MonoBehaviour,IViewController
{
	private ServerListItem _view;

	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<ServerListItem> ();
		_view.Setup (this.transform);
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set(_view.ServerListItem_UIButton.onClick, OnListItemClick);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose(){
	}

	//服务器回调信息
	public delegate void GetServerMessageFunc(ServerListItemController item);
	private GetServerMessageFunc _callBackFunc = null;

	private ServerInfo _serverInfo;
	
	public void SetData(ServerInfo serverInfo, GetServerMessageFunc func)
	{
		if (_view == null)
		{
			InitView();
			RegisterEvent();
		}

		_serverInfo = serverInfo;
		
		_callBackFunc = func;
		
		UpdateServerInfo();
	}
	
	public void SetupServerPlayerMessage(ServerPlayerMessage message)
	{
		//Debug.Log(" ----- SetupServerPlayerMessage ----- ");
		//Debug.Log( message );
		
		//Debug.Log(_serverMessage.name + "/" + _serverMessage.serviceId );
		if (message != null)
		{
			_view.RoleSprite.SetActive(true);
			_view.RoleCountLabel_UILabel.text = "1";
			//if (portrait != null)
			//{
				//                if (message.gender == 1)
				//                {
				//                    portrait.spriteName = MAN_STATE;
				//                }
				//                else
				//                {
				//                    portrait.spriteName = WOMAN_STATE;
				//                }
				//                portrait.gameObject.SetActive(true);
			//}
			
			//            if (levelLabel != null)
			//            {
			//                levelLabel.gameObject.SetActive(true);
			//                levelLabel.text = string.Format("LV.{0}", message.grade.ToString());
			//            }
		}
		
		
	}
	
	public void HidePlayerMessage()
	{
		////        if (levelLabel != null)
		////            levelLabel.gameObject.SetActive(false);
		//
		//        if (portrait != null)
		//            portrait.gameObject.SetActive(false);
	}

	private Color _selectColor = new Color(167/256f,112/256f,36/256f,255/256f);
	/// <summary>
	/// 选择服务器
	/// </summary>
	public void SelectServer()
	{
		//维护状态的服务器不给选择
//		if (_serverInfo.dboState == 2)
//		{
//			return;
//		}
		
//		if (_isSelect) return;
//		_isSelect = true;
//		
//		
//		_backgroudSprite.color = _selectColor;
		_view.BgSprite_UISprite.spriteName = "exp_front";
	}
	
	/// <summary>
	/// 不选中状态
	/// </summary>
	public void UnSelectServer()
	{
//		_isSelect = false;
		_view.BgSprite_UISprite.spriteName = "panel_border_001";
	}
	
	public void OnListItemClick()
	{
		if (_callBackFunc != null && _serverInfo != null)
		{
			_callBackFunc(this);
		}
	}
	
	public ServerInfo GetServerInfo()
	{
		return _serverInfo;
	}

	private void UpdateServerInfo()
	{
		if (_serverInfo == null) return;
		
		_view.NameLabel_UILabel.text = _serverInfo.name;
		_view.NewSprite.SetActive(false);
		_view.RoleSprite.SetActive(false);
		_view.RoleCountLabel_UILabel.text = "";
	}
}

