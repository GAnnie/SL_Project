using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.friend.dto;

public class PlayerInfoItemCellController : MonoBehaviour {

	private PlayerInfoItemCell _view;
	private int _index;
	private System.Action<int> _onSelect;
	private FriendDto  _dto;
	private bool _selected;
	public void InitView(){

		_view = gameObject.GetMissingComponent<PlayerInfoItemCell> ();
		_view.Setup (this.transform);

	}
	public void RegisterEvent(){

	}



	public void SetData(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<PlayerInfoItemCell> ();
		_view.Setup (this.transform);
		
		_index = index;
		gameObject.name = "Email"+index;
		_onSelect = selectCallback; 

		_view.MoreBtn.gameObject.SetActive(false);

		_view.NameLbl.transform.localPosition = new Vector3(-50,24.5f,0);
		_view.From_LevelLbl.transform.localPosition = new Vector3(-50,-10f,0);

		EventDelegate.Set (_view.ClickBtn.onClick, OnClickItem);
	}
	#region 好友的
	public void SetData(int index,FriendDto dto,System.Action<int> selectCallback){

		_dto = dto;
		_index = index;
		gameObject.name = "Friend"+index;
		_onSelect = selectCallback;

		_view.MoreBtn.gameObject.SetActive(true);
		_view.NameLbl.transform.localPosition = new Vector3(-50,14.5f,0);
		_view.From_LevelLbl.transform.localPosition = new Vector3(-50,-13.2f,0);
		EventDelegate.Set (_view.ClickBtn.onClick, OnClickItem);
		EventDelegate.Set (_view.MoreBtn.onClick, OnClickMoreBtn);
		
	}

	#endregion




	#region 玩家头像
	public void SetFaceIcon(string id){
		_view.iconFace.spriteName = id;
	}
	//在线与否
	public void SetOnOffLine(bool b){
		_view.iconFace.isGrey = !b;
	}
	#endregion

	#region 姓名/邮件标题
	public void SetName(string name){
		_view.NameLbl.text = name;
	}

	#endregion

	#region 门派等级/发件人-时间
	public void SetFrom(string from){
		_view.From_LevelLbl.fontSize = 14;
		_view.From_LevelLbl.text = string.Format("[{0}]{1}[-]",ColorConstant.Color_UI_Title_Str,from) ;
	}

	public void SetLevel(string lv){
		_view.From_LevelLbl.fontSize = 18;
		_view.From_LevelLbl.text = string.Format("[{0}]{1}[-]",ColorConstant.Color_UI_Title_Str,lv) ;
	}
	#endregion

	#region 点击聊天 / 查看邮件
	public void OnClickItem(){
		if(_onSelect != null)
			_onSelect(_index);
	}
	#endregion

	#region 点击 玩家头像显示  更多
	private void OnClickMoreBtn(){
		ServiceRequestAction.requestServer (PlayerService.playerInfo(_dto.shortPlayerDto.id),"playerInfo",(e)=>{
			ProxyMainUIModule.ClosePlayerInfoView();
			ProxyMainUIModule.OpenPlayerInfoView(e as SimplePlayerDto,new Vector3(-35,-32,0));
		});
	}

	#endregion


	#region 角标
	public void SetAngleIcon(bool show = false){
		_view.RelationObj.SetActive (show);	
	}
	#endregion


	#region 更多
	public void SetClickBtnHide(){
		_view.MoreBtn.gameObject.SetActive (false);
	}
	public void SetClickBtnShow(){
		_view.MoreBtn.gameObject.SetActive  (true);
	}
	#endregion



	#region 选中状态
	public void SetSelected(bool active)
	{
		_selected = active;
		UpdateBtnState();
	}

	private void UpdateBtnState(){
		if(_selected)
		{
			_view.Bg.spriteName = "the-no-choice-lines";
		}
		else
		{
			_view.Bg.spriteName = "the-choice-lines";
		}
	}
	#endregion

	#region 小红点
	public void ShowRedPoint(){
		_view.RedPoint.gameObject.SetActive(true);
	}

	public void HideRedPoint(){
		_view.RedPoint.gameObject.SetActive(false);
	}
	#endregion


	#region 当前对应的是哪个好友
	public long MatchFriend(){
		if(_dto != null){
			return _dto.shortPlayerDto.id;
		}
		return 0;
	}
	#endregion
	public void ClearInfo(){
		_dto = null;
	}

	public void Dispose(){

	}
}
