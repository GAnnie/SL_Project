using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.services;

public class PlayerInfoViewController : MonoBehaviour,IViewController
{

	private PlayerInfoView _view;
	private SimplePlayerDto _playerDto;
	

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PlayerInfoView> ();
		_view.Setup (this.transform);

		_view.VipSprite.isGrey = !_playerDto.vip;  

		_view.Sex.text = (DataCache.getDtoByCls<GeneralCharactor>(_playerDto.charactor.id) as MainCharactor).gender  == 1 ? "男" : "女";


		//门派
		switch(_playerDto.factionId){
		case 1:
			//大唐
			_view.FactionSprite.spriteName  = "factionIcon_1";
			break;
		case 2:
			// 化生寺
			_view.FactionSprite.spriteName  = "factionIcon_2";
			break;
		case 3:
			//方寸山
			_view.FactionSprite.spriteName  = "factionIcon_3";
			break;
		case 4:
			//天宫
			_view.FactionSprite.spriteName  = "factionIcon_4";
			break;
		case 5:
			//龙宫
			_view.FactionSprite.spriteName  = "factionIcon_5";
			break;
		case 6:
			//普陀
			_view.FactionSprite.spriteName  = "factionIcon_6";
			break;
		case 7:
			//魔王
			_view.FactionSprite.spriteName  = "factionIcon_7";
			break;
		case 8:
			//狮驼
			_view.FactionSprite.spriteName  = "factionIcon_8";
			break;
		case 9:
			//盘丝
			_view.FactionSprite.spriteName  = "factionIcon_9";
			break;
		default:
			_view.FactionSprite.gameObject.SetActive(false);
			break;
		}
		_view.TeamNOLbl.gameObject.SetActive(false);
		_view.PlayerNameLbl.text = string.Format ("{0}", _playerDto.nickname);
		_view.Level.text = string.Format ("{0}级", _playerDto.grade);
//		_view.SexLbl.text = string.Format ("{0}", _playerDto.sex);     //sex

		_view.OnLine.text =  _playerDto.online == true ? "在线" : "离线";    //online

//		_view.TeamNOLbl.text =     //team number

		_view.IdLbl.text = string.Format ("{0}", _playerDto.id);
//		_view.AppellationLbl.text = string.Format("{0}",_playerDto.)      //称谓

		_view.FactionLbl.text = string.Format ("{0}", _playerDto.guildName);
//		_view.PositionLbl.text = string.Format("{0}",_playerDto)       //位置

		if(_playerDto.teamStatus != PlayerDto.PlayerTeamStatus_NoTeam){
			_view.TeamSprite.gameObject.SetActive (true);
		}
		else{
			_view.TeamSprite.gameObject.SetActive (false);
		}


		if (_playerDto.teamStatus != PlayerDto.PlayerTeamStatus_NoTeam && !TeamModel.Instance.HasTeam()) {
			_view.InviteTeamLbl.text = "申请入队";
		} else if (_playerDto.teamStatus == PlayerDto.PlayerTeamStatus_NoTeam && TeamModel.Instance.HasTeam()) {
			_view.InviteTeamLbl.text = "邀请入队";
		}else {
			_view.InviteTeamBtn.gameObject.SetActive (false);
		}

		if(FriendModel.Instance.IsMyFriend(_playerDto.id)){
			isStrange = false;
			_view.AddFriendLbl.text = "删除好友";
		}
		else{
			isStrange = true;
			_view.AddFriendLbl.text = "添加好友";
		}




		_view.Bg.height = 245 + 55 * (5 + (_view.InviteTeamBtn.gameObject.activeSelf ? 1 : 0));

		_view.ButtonGrid.Reposition ();
		RegisterEvent ();
	}

	public void RegisterEvent ()
	{
//		EventDelegate.Set (_view.closeBtn.onClick, CloseView);
		EventDelegate.Set (_view.InviteTeamBtn.onClick, OnClickTeamBtn);
		EventDelegate.Set (_view.GiveBtn.onClick, OnGiveBtnClick);
		EventDelegate.Set (_view.AddFriendBtn.onClick, OnClickAddFriendBtn);
		EventDelegate.Set (_view.BeginChatBtn.onClick, OnBeginChatBtnClick);
		EventDelegate.Set (_view.PKBtn.onClick, OnPKBtnClick);
		EventDelegate.Set (_view.VisitHomeBtn.onClick, OnVisitHomeBtnClick);
	}

	public void Dispose ()
	{
	}
	#endregion

	public void Open (SimplePlayerDto playerDto)
	{
		_playerDto = playerDto;
		InitView ();
	}

	public void Open (SimplePlayerDto playerDto,Vector3 position)
	{
		_playerDto = playerDto;
		InitView ();
		this.transform.localPosition = position;
	}

	void OnEnable ()
	{
		UICamera.onClick += ClickEventHandler;
	}
	
	void OnDisable ()
	{
		UICamera.onClick -= ClickEventHandler;
	}

	#region Button Event Handler
	public void OnClickTeamBtn ()
	{
		if (_playerDto.teamStatus == PlayerDto.PlayerTeamStatus_Leader) {
			TeamModel.Instance.JoinTeam (_playerDto.id, CloseView);
		} else if (_playerDto.teamStatus == PlayerDto.PlayerTeamStatus_NoTeam) {
			TeamModel.Instance.InviteMember (_playerDto.id, CloseView);
		}
	}
	#endregion

	#region 添加/删除好友
	bool isStrange = true;
	public void OnClickAddFriendBtn(){
		if(isStrange){
			FriendModel.Instance.AddFriend(_playerDto.id);
		}
		else{
			FriendModel.Instance.DelFriend(_playerDto.id);
		}

	}

	#endregion


	//
	public void OnBeginChatBtnClick(){
		FriendModel.Instance.BeginChat(_playerDto);
	}

	public void OnPKBtnClick(){
		ServiceRequestAction.requestServer(ArenaService.challenge(_playerDto.id), "challenge");
		CloseView();
	}

	public void OnVisitHomeBtnClick(){
		TipManager.AddTip ("该功能正在研发中");
	}



	void ClickEventHandler (GameObject clickGo)
	{
		UIPanel panel = UIPanel.Find (clickGo.transform);
		if (panel != _view.uiPanel)
			CloseView ();
	}

	public void CloseView ()
	{
		ProxyMainUIModule.ClosePlayerInfoView ();
	}

	public void OnGiveBtnClick(){
		ProxyGiftPropsModule.Open (_playerDto);
		CloseView ();
	}
}
