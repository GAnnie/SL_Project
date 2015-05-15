﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.chat.modules.dto;
using com.nucleus.h1.logic.core.modules.system.dto;
using com.nucleus.h1.logic.chat.modules.data;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;

public class ChatInfoViewController : MonoBehaviour,IViewController {

	private ChatInfoView _view;
	private const string CHANNLFUNBTNPANTH = "Prefabs/Module/FriendModule/ChannlItemCell";
	private const string FacePanelViewPath = "Prefabs/Module/FriendModule/FacePanelView";
	private FacePanelViewController facePanelViewController = null;

	private ChannlViewController _lastTypeBtn;

	List<ChannlViewController> _channlFUNList;     //频道

	List<ChatNotify> _worldTalkList;
	List<ChatNotify> _guildTalkList;
	List<ChatNotify> _teamTalkList;
	List<SystemNotify> _sysTalkList;

	List<HearsayNotify> _hearsayTalkList;



	//记录当前是的是什么频道
	public int _curChannl = -1;


	private const string ChatItemCellViewPath = "Prefabs/Module/FriendModule/ChatItemCellView";
	private const string ChatSysItemCellPath = "Prefabs/Module/ChatPrefab/ChannelChatItemCell";

	List<ChatItemCellController> chatItemCellControllerList = new List<ChatItemCellController>();

	List<ChannelChatItemCellController> ChannelChatItemCellControllerList = new List<ChannelChatItemCellController>();

	public void InitView(){
		_view = gameObject.GetMissingComponent<ChatInfoView> ();
		_view.Setup (this.transform);

		GameObject channlBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CHANNLFUNBTNPANTH) as GameObject;
		_channlFUNList = new List<ChannlViewController> (5);
		for (int i = 0; i < 5; i++) {
			GameObject item = NGUITools.AddChild (_view.LeftBtnGroupGrid.gameObject, channlBtnPrefab);
			ChannlViewController com = item.GetMissingComponent<ChannlViewController>();
			_channlFUNList.Add(com);
		}

		_channlFUNList [0].InitItem (0, OnSelectCannlTypeBtn);
		_channlFUNList [0].SetBtnName ("帮派");
		_channlFUNList [1].InitItem (1, OnSelectCannlTypeBtn);
		_channlFUNList [1].SetBtnName ("队伍");
		_channlFUNList [2].InitItem (2, OnSelectCannlTypeBtn);
		_channlFUNList [2].SetBtnName ("世界");
		_channlFUNList [3].InitItem (3, OnSelectCannlTypeBtn);
		_channlFUNList [3].SetBtnName ("传闻");
		_channlFUNList [4].InitItem (4, OnSelectCannlTypeBtn);
		_channlFUNList [4].SetBtnName ("系统");




		RegisterEvent ();
		OnSelectCannlTypeBtn (2);
	}
	

	public void RegisterEvent(){
		EventDelegate.Set (_view.SpeedOrInputBtn.onClick, OnSpeedOrInputBtnClick);
		EventDelegate.Set (_view.ExpressionBtn.onClick, OnExpressionBtnClick);
		EventDelegate.Set (_view.InputTxt.onSubmit, OnSubmit);
		EventDelegate.Set (_view.HideBtn.onClick, OnHideBtnClick);
		EventDelegate.Set (_view.SockBtn.onClick, SockBtnClick);

		EventDelegate.Set(_view.UnReadBtn.onClick,OnUnReadBtnClick);
		EventDelegate.Set(_view.SendBtn.onClick,OnSubmit);
		EventDelegate.Set(_view.InputTxt.onChange,onChange);


		ChatModel.Instance.OnNewNotifyUpdate += AddNewNotify;
		ChatModel.Instance.ShowUnRead += ShowUnReadMsg;
		ChatModel.Instance.SetCoolDownData += SetCoolDownData;
		_view.ItemScrollView.onDragFinished += JudgePos;

		_view.SpeedBtnEventListener.onPress += OnPress;
		_view.SpeedBtnEventListener.onDrag += OnDrag;


	}

	public void OnSelectCannlTypeBtn(int index){
		ChatModel.Instance.SetCurChannel(index);

		//如果我就在当前频道，再次点击还是没变化
		if(_curChannl == index){
			return;
		}

		if(_curChannl != index){
			ChatModel.Instance.SetLock(false);
			SetUnLock();
			HideUnReadMsg();
		}

		_curChannl = index;

		_view.inputLbl.text = string.IsNullOrEmpty(_view.InputTxt.value) ? "请点击输入" : _view.InputTxt.value;
		if(_lastTypeBtn != null)
			_lastTypeBtn.SetSelected(false);
		
		_lastTypeBtn = _channlFUNList[index];
		_lastTypeBtn.SetSelected(true);

		switch (index) {
		case 0:
			OnGuildBtnClick();
			break;
		case 1:
			OnTeamBtnClick();
			break;
		case 2:
			OnWorldBtnClick();
			break;
		case 3:
			OnHearsayClick();
			break;
		case 4:
			OnSysBtnClick();
			break;
		default:
			break;
		}
	}

	#region 拖动后
	public void JudgePos(){
		if(_view.ItemScrollView.transform.localPosition.y >0){
			SetLock();
		}
		else{
			SetUnLock();
			HideUnReadMsg();
			OnSelectCannlTypeBtn(_curChannl);
		}
	}
	#endregion

	#region 检测一输入多少个表情
	bool hasFive = false;
	public void onChange(){

		string str = _view.InputTxt.value;
		int count = 0;
		System.Text.RegularExpressions.Regex  regex = new System.Text.RegularExpressions.Regex(@"^[1-9]\d*$");
		for(int i = 0; i <str.Length;i++){
			int acs = str[i];
			if(acs == 35 && i +1 < str.Length ){
				if(regex.IsMatch(str[i+1].ToString())){
					++count;
				}
			}
		}
		if(count >5){
			hasFive =  true;
		}
		else{
			hasFive = false;
		}
	}
	#endregion



	#region 发送信息
	bool canSend = true;
	public void OnSubmit(){
		if (facePanelViewController != null) {
			Destroy(facePanelViewController.gameObject);
			facePanelViewController = null;
		}

		if (string.IsNullOrEmpty (_view.InputTxt.value)) {
			TipManager.AddTip("不能发送空消息");
			return;
		}

		if (_curChannl == 1) {
			if(!TeamModel.Instance.HasTeam()){
				TipManager.AddTip("你没有队伍");
				return;
			}
		}


		if (_curChannl == 3 || _curChannl == 4) {
			TipManager.AddTip("该频道不能发言");
			return;	
		}



		string msg = ChatModel.Instance.GetUrlInfo(_view.InputTxt.value,itemId,uniquideId,type);



		switch (_curChannl) {
		case 0:
			onChange();
			if(hasFive){
				TipManager.AddTip("只能输入5个表情");
				return;
			}
			if(ChatModel.Instance.GuildCanChat()){
				ServiceRequestAction.requestServer (ChatService.talk(2,0,false,msg),"guildTalk",delegate(GeneralResponse e) {
					_view.InputTxt.value ="";
					ChatModel.Instance.GuildChatCoolDown();
				},SendFail);
			}
			else{
				TipManager.AddTip("发言太快了");
			}
			break;

		case 1:
			onChange();
			if(hasFive){
				TipManager.AddTip("只能输入5个表情");
				return;
			}
			if(ChatModel.Instance.TeamCanChat()){
				ServiceRequestAction.requestServer (ChatService.talk(3,0,false,msg),"teamTalk",delegate(GeneralResponse e) {
					_view.InputTxt.value ="";
					ChatModel.Instance.TeamChatCoolDown();
			},SendFail);
			}
			else{
				TipManager.AddTip("发言太快了");
			}
			break;
		case 2:

			if(!ChatModel.Instance.isMoreThenTen()){
				TipManager.AddTip("世界发言需要人物等级10级");
				return;
			}

			if(!PlayerModel.Instance.isEnoughVigour(10)){
				TipManager.AddTip("活力不足，世界发言需要消耗10点活力");
				return;
			}

			onChange();
			if(hasFive){
				TipManager.AddTip("只能输入5个表情");
				return;
			}

			if(ChatModel.Instance.WorldCanChat()){
				ServiceRequestAction.requestServer (ChatService.talk(1,0,false,msg),"WorldTalk",SendSuccess,SendFail);

			}
			else{
				float t = ChatModel.Instance.GetCoolDownTime();
				string str= string.Format("世界频道还要{0}秒才能发言",(int)t);
				TipManager.AddTip(str);
			}
			break;
		case 3:
			break;
		case 4:
			break;
		}

	}

	//发送成功
	public void SendSuccess(GeneralResponse e){
		_view.InputTxt.value = "";
		TipManager.AddTip("发言成功，消耗了10点活力");
		ChatModel.Instance.WorldChatCoolDown();
	}
	//发送失败
	public void SendFail( ErrorResponse e){
		Debug.LogError(e.message);
	}

	//只有世界频道才显示倒计时
	public void SetCoolDownData(float t){

		if(_curChannl == 2){
			if(!string.IsNullOrEmpty(_view.InputTxt.value) ||_view.InputTxt.isSelected ){
				_view.inputLbl.text = _view.InputTxt.value;
			}
			else{
				if(t <0.03f){
					_view.inputLbl.text = string.Format("请点击输入");
				}
				else{
					_view.inputLbl.text = string.Format("请{0}秒后输入",(int)t);
				}
			}	
		}
		else{
			
		}
	}
	#endregion



	#region 新信息
	public void AddNewNotify(){
		HideUnReadMsg();

		switch (_curChannl) {
		case 0:
			OnGuildBtnClick();
			break;
		case 1:
			OnTeamBtnClick();
			break;
		case 2:
			OnWorldBtnClick ();
			break;
		case 3:
			OnHearsayClick ();
			break;
		case 4:
			OnSysBtnClick ();
			break;
		}
		
	}
	#endregion



	#region 帮派 
	
	public void OnGuildBtnClick(){
		_view.ChatInputGroupObj.SetActive(true);
		_view.WarningObj.SetActive(false);

		if(ChannelChatItemCellControllerList.Count > 0)
		for (int i = 0; i <ChannelChatItemCellControllerList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(false);
		}


		_guildTalkList = ChatModel.Instance.GetGuildTalkList ();


		//如果数量不够 就多添加几个~
		if (chatItemCellControllerList.Count < _guildTalkList.Count) {
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatItemCellViewPath) as GameObject;

			for(int i = chatItemCellControllerList.Count ; i <  _guildTalkList.Count; i++){
				GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
				module.transform.SetAsFirstSibling();
				ChatItemCellController com = module.GetMissingComponent<ChatItemCellController>();
				com.InitView();
				chatItemCellControllerList.Add(com);
			}
		}



		//更改前面内容
		for (int i =0; i < _guildTalkList.Count; i++) {



			chatItemCellControllerList[i].ClearExpressionInfo();
			if(_guildTalkList[i].fromPlayer.id != PlayerModel.Instance.GetPlayerId()){
				chatItemCellControllerList[i].SetDate(_guildTalkList[i],false,275,400);
			}
			else{
				chatItemCellControllerList[i].SetDate(_guildTalkList[i],true,275,400);
			}
			chatItemCellControllerList[i].gameObject.SetActive(true);
		}
//		


		//多余的就隐藏掉

		for (int i = _guildTalkList.Count; i < chatItemCellControllerList.Count; i++) {
			chatItemCellControllerList[i].gameObject.SetActive(false);	
		}

		_view.ItemTable.Reposition();
		_view.ItemScrollView.ResetPosition();
	}
	#endregion


	#region 队伍
	public void OnTeamBtnClick(){
		_view.ChatInputGroupObj.SetActive(true);
		_view.WarningObj.SetActive(false);
		
		if(ChannelChatItemCellControllerList.Count > 0)
		for (int i = 0; i <ChannelChatItemCellControllerList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(false);
		}
		
		
		_teamTalkList = ChatModel.Instance.GetTeamTalkList ();
		
		
		//如果数量不够 就多添加几个~
		if (chatItemCellControllerList.Count < _teamTalkList.Count) {
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatItemCellViewPath) as GameObject;
			
			for(int i = chatItemCellControllerList.Count ; i <  _teamTalkList.Count; i++){
				GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
				module.transform.SetAsFirstSibling();
				ChatItemCellController com = module.GetMissingComponent<ChatItemCellController>();
				com.InitView();
				chatItemCellControllerList.Add(com);
			}
		}
		
		
		
		//更改前面内容
		for (int i =0; i < _teamTalkList.Count; i++) {
			
			chatItemCellControllerList[i].ClearExpressionInfo();


			if(_teamTalkList[i].fromPlayer.id != PlayerModel.Instance.GetPlayerId()){
				chatItemCellControllerList[i].SetDate(_teamTalkList[i],false,275,400);
			}
			else{
				chatItemCellControllerList[i].SetDate(_teamTalkList[i],true,275,400);
			}	
			chatItemCellControllerList[i].gameObject.SetActive(true);
		}
		//		
		
		
		//多余的就隐藏掉
		
		for (int i = _teamTalkList.Count; i < chatItemCellControllerList.Count; i++) {
			chatItemCellControllerList[i].gameObject.SetActive(false);	
		}
		
		_view.ItemTable.Reposition();
		_view.ItemScrollView.ResetPosition();
	}
	#endregion


	#region 世界
	public void OnWorldBtnClick(){
		_view.ChatInputGroupObj.SetActive(true);
		_view.WarningObj.SetActive(false);

		if(ChannelChatItemCellControllerList.Count > 0)
		for (int i = 0; i <ChannelChatItemCellControllerList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(false);
		}


		_worldTalkList = ChatModel.Instance.GetWorldTalkList ();


		//如果数量不够 就多添加几个~
		if (chatItemCellControllerList.Count < _worldTalkList.Count) {
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatItemCellViewPath) as GameObject;

			for(int i = chatItemCellControllerList.Count ; i <  _worldTalkList.Count; i++){
				GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
				module.transform.SetAsFirstSibling();
				ChatItemCellController com = module.GetMissingComponent<ChatItemCellController>();
				com.InitView();
				chatItemCellControllerList.Add(com);
			}
		}



		//更改前面内容
		for (int i =0; i < _worldTalkList.Count; i++) {

			chatItemCellControllerList[i].ClearExpressionInfo();

			if(_worldTalkList[i].fromPlayer.id != PlayerModel.Instance.GetPlayerId()){
				chatItemCellControllerList[i].ClearExpressionInfo();
				chatItemCellControllerList[i].SetDate(_worldTalkList[i],false,275,400);
			}
			else{
				chatItemCellControllerList[i].ClearExpressionInfo();
				chatItemCellControllerList[i].SetDate(_worldTalkList[i],true,275,400);
			}	
			chatItemCellControllerList[i].gameObject.SetActive(true);
		}
//		


		//多余的就隐藏掉

		for (int i = _worldTalkList.Count; i < chatItemCellControllerList.Count; i++) {
			chatItemCellControllerList[i].ClearExpressionInfo();
			chatItemCellControllerList[i].gameObject.SetActive(false);	
		}

		_view.ItemTable.Reposition();
		_view.ItemScrollView.ResetPosition();
		
	}
	#endregion





	#region 系统
	public void OnSysBtnClick(){

		_view.WarningObj.SetActive(true);
		_view.ChatInputGroupObj.SetActive(false);

		if(chatItemCellControllerList.Count > 0)
		for (int i = 0; i <chatItemCellControllerList.Count; i++) {
			chatItemCellControllerList[i].gameObject.SetActive(false);
		}


		_sysTalkList = ChatModel.Instance.GetSysTalkList ();

		//如果数量不够 就多添加几个~
		if (ChannelChatItemCellControllerList.Count < _sysTalkList.Count) {

			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatSysItemCellPath) as GameObject;
			
			for(int i = ChannelChatItemCellControllerList.Count ; i <  _sysTalkList.Count; i++){
				GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
				module.transform.SetAsFirstSibling();
				ChannelChatItemCellController com = module.GetMissingComponent<ChannelChatItemCellController>();
				com.InitView();
				ChannelChatItemCellControllerList.Add(com);
			}
		}

		//更改前面内容
		for (int i =0; i < _sysTalkList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(true);

			switch(_sysTalkList[i].lableType){
				case ChatChannel.LableTypeEnum_System:
					ChannelChatItemCellControllerList[i].SetData("系统",_sysTalkList[i].content,1);
					break;
				case ChatChannel.LableTypeEnum_Prompt:
					ChannelChatItemCellControllerList[i].SetData("提示",_sysTalkList[i].content,3);
					break;
				case ChatChannel.LableTypeEnum_Help:
					ChannelChatItemCellControllerList[i].SetData("帮助",_sysTalkList[i].content,2);
					break;
			}

		}

		//多余的就隐藏掉
		
		for (int i = _sysTalkList.Count; i < ChannelChatItemCellControllerList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(false);	
		}
		
		_view.ItemTable.Reposition();
		_view.ItemScrollView.ResetPosition();

	}
	#endregion

	#region 传闻
	public void OnHearsayClick(){

		_view.WarningObj.SetActive(true);
		_view.ChatInputGroupObj.SetActive(false);

		if(chatItemCellControllerList.Count > 0)
		for (int i = 0; i <chatItemCellControllerList.Count; i++) {
			chatItemCellControllerList[i].gameObject.SetActive(false);
		}

		_hearsayTalkList = ChatModel.Instance.GetHearsayTalkList ();


		//如果数量不够 就多添加几个~
		if (ChannelChatItemCellControllerList.Count < _hearsayTalkList.Count) {
			
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatSysItemCellPath) as GameObject;
			
			for(int i = ChannelChatItemCellControllerList.Count ; i <  _hearsayTalkList.Count; i++){
				GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
				module.transform.SetAsFirstSibling();
				ChannelChatItemCellController com = module.GetMissingComponent<ChannelChatItemCellController>();
				com.InitView();
				ChannelChatItemCellControllerList.Add(com);
			}
		}
		
		//更改前面内容
		for (int i =0; i < _hearsayTalkList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(true);

			ChannelChatItemCellControllerList[i].SetData("传闻",_hearsayTalkList[i].content,2);

		}
		
		//多余的就隐藏掉
		
		for (int i = _hearsayTalkList.Count; i < ChannelChatItemCellControllerList.Count; i++) {
			ChannelChatItemCellControllerList[i].gameObject.SetActive(false);	
		}
		
		_view.ItemTable.Reposition();
		_view.ItemScrollView.ResetPosition();





	}
	#endregion
	

	
	bool isSpeedType =true;
	public void OnSpeedOrInputBtnClick(){

		if (isSpeedType) {
			_view.SpeedBtnEventListener.gameObject.SetActive(true);
			_view.InputTxt.gameObject.SetActive(false);
			isSpeedType = false;
			_view.SpeedOrInputBg.spriteName = "keyboard-little-icon";
			
		}
		else{
			_view.SpeedBtnEventListener.gameObject.SetActive(false);
			_view.InputTxt.gameObject.SetActive(true);
			isSpeedType = true;
			_view.SpeedOrInputBg.spriteName = "sould-little-icon";
		}
	}


	#region 语音
	void OnPress(GameObject go,bool isPress) 
	{ 
		if(isPress){
//			TipManager.AddTip("1111");
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(true);
			_view.GiveUpSprite.gameObject.SetActive(false);
			_view.ShortSprite.gameObject.SetActive(false);
			_view.TipLbl.text = "手指划开,取消发送";
			switch(_curChannl){
			case 0:
				VoiceRecognitionManager.Instance.BeginRecord(OnGuildChatBtnRelese);
				break;
			case 1:
				VoiceRecognitionManager.Instance.BeginRecord(OnTeamChatBtnRelese);
				break;
			case 2:
				VoiceRecognitionManager.Instance.BeginRecord(OnWorldChatBtnRelese);
				break;
			}

		}
		else{
			_view.SpeechAnchorObj.SetActive(false);
			switch(_curChannl){
			case 0:
				OnGuildChatBtnRelese();
				break;
			case 1:
				OnTeamChatBtnRelese();
				break;
			case 2:
				OnWorldChatBtnRelese();
				break;
			}
		}
	}
	
	public void OnWorldChatBtnRelese(){
		_view.SpeechAnchorObj.SetActive(false);

		VoiceRecognitionManager.Instance.FinishRecord( delegate(string msg,string time) {
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"0");
			ServiceRequestAction.requestServer (ChatService.talk(1,0,false,str),"world msg",null,SendFail);

		},delegate(string msg) {
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"1");
			ChatModel.Instance.SendMessage(1,str,0,true);
		},delegate {
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(false);
			_view.GiveUpSprite.gameObject.SetActive(false);
			
			_view.ShortSprite.gameObject.SetActive(true);
			
			CoolDownManager.Instance.SetupCoolDown("showShortVoiceTip",1f,null,delegate {
				_view.SpeechAnchorObj.SetActive(false);
				_view.ShortSprite.gameObject.SetActive(false);
			});
		} );
	}
	public void OnTeamChatBtnRelese(){
		_view.SpeechAnchorObj.SetActive(false);
		
		VoiceRecognitionManager.Instance.FinishRecord( delegate(string msg,string time) {
			
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"0");

			ServiceRequestAction.requestServer (ChatService.talk(3,0,false,str),"world msg",null,SendFail);


		}, delegate(string msg){
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"1");
			ChatModel.Instance.SendMessage(3,str,0,true);
		},delegate {
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(false);
			_view.GiveUpSprite.gameObject.SetActive(false);
			
			_view.ShortSprite.gameObject.SetActive(true);
			
			CoolDownManager.Instance.SetupCoolDown("showShortVoiceTip",1f,null,delegate {
				_view.SpeechAnchorObj.SetActive(false);
				_view.ShortSprite.gameObject.SetActive(false);
			});
	});
	} 
	public void OnGuildChatBtnRelese(){
		_view.SpeechAnchorObj.SetActive(false);
		
		VoiceRecognitionManager.Instance.FinishRecord( delegate(string msg,string time) {
			
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"0");

			ServiceRequestAction.requestServer (ChatService.talk(2,0,false,str),"world msg",null,SendFail);

		},delegate(string msg) {
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"1");
			ChatModel.Instance.SendMessage(2,str,0,true);
		},delegate {
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(false);
			_view.GiveUpSprite.gameObject.SetActive(false);
			
			_view.ShortSprite.gameObject.SetActive(true);
			
			CoolDownManager.Instance.SetupCoolDown("showShortVoiceTip",1f,null,delegate {
				_view.SpeechAnchorObj.SetActive(false);
				_view.ShortSprite.gameObject.SetActive(false);
			});
		} );
	}
	
	
	public void OnDrag(GameObject go, Vector2 delta){
		
		if(delta.y <0 && Mathf.Abs(delta.y) > 10f){
			_view.SpeechObj.SetActive(false);
			_view.GiveUpSprite.gameObject.SetActive(true);
			_view.ShortSprite.gameObject.SetActive(false);
			_view.TipLbl.text = "松开手指,取消发送";
			VoiceRecognitionManager.Instance.FingersDragOut();
		}
		if(delta.y >0 && Mathf.Abs(delta.y) > 10f){
			_view.SpeechObj.SetActive(true);
			_view.GiveUpSprite.gameObject.SetActive(false);
			_view.ShortSprite.gameObject.SetActive(false);
			_view.TipLbl.text = "手指划开,取消发送";
			VoiceRecognitionManager.Instance.FingersDragOver();
		}
	}
	#endregion

	#region 表情
	int itemId;
	long uniquideId;
	int type;
	public void OnExpressionBtnClick(){
		GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab (FacePanelViewPath) as GameObject;
		GameObject module = NGUITools.AddChild (this.gameObject, modulePrefab);
		UIHelper.AdjustDepth (module, 1);
		facePanelViewController = module.GetMissingComponent<FacePanelViewController> ();
		facePanelViewController.InitView ();
		facePanelViewController.SetCallback (delegate(string str,int itemId,long uniquideId,int type) {

			if(type ==1){
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = type;
				OnSubmit();
			}

			if(type == 2){  //技能
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 6;
				OnSubmit();
			}
			if(type == 3 ){ // 宠物
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 2;
				OnSubmit();
			}
			if(type == 4){  //伙伴
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 7;
				OnSubmit();
			}


		}, null,delegate(string name, int id ,int whichType) {
			///type : 1 任务  2.成就 3.称谓    
			/// 
			switch(whichType){

			case 1:
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 3;

				break;
			case 2:
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 4;
				break;

			case 3:
				_view.InputTxt.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 5;
				break;

			}

			OnSubmit();
	});


		//表情
		facePanelViewController.SetExpressionCallback(delegate(int no) {

			string s = _view.InputTxt.value;
			_view.InputTxt.value += "#"+no;
			onChange();

			if(hasFive){
				_view.InputTxt.value = s;
				TipManager.AddTip("只能输入5个表情");
			}
		});

	}
	#endregion

	#region 关闭
	public void OnHideBtnClick(){
		ProxyChatModule.Close ();
	}
	#endregion

	#region 锁屏
	bool open = true;
	public void SockBtnClick(){
		if (!open) {
			open =true;
			_view.LockIconWidget.leftAnchor.absolute = 2;
			_view.LockIconWidget.rightAnchor.absolute = 2;
			_view.LockIconWidget.spriteName = "lock-open";
			_view.Foreground.gameObject.SetActive(false);

			ChatModel.Instance.SetLock(false);
		}
		else{
			open =false;

			_view.LockIconWidget.leftAnchor.absolute = 31;
			_view.LockIconWidget.rightAnchor.absolute = 31;
			_view.LockIconWidget.spriteName = "lock";
			_view.Foreground.gameObject.SetActive(true);
			ChatModel.Instance.SetLock(true);
		}
	}


	public void SetUnLock(){
		open =true;
		_view.LockIconWidget.leftAnchor.absolute = 2;
		_view.LockIconWidget.rightAnchor.absolute = 2;
		_view.LockIconWidget.spriteName = "lock-open";
		_view.Foreground.gameObject.SetActive(false);
		ChatModel.Instance.SetLock(false);
	}


	public void SetLock(){
		open =false;
		_view.LockIconWidget.leftAnchor.absolute = 31;
		_view.LockIconWidget.rightAnchor.absolute = 31;
		_view.LockIconWidget.spriteName = "lock";
		_view.Foreground.gameObject.SetActive(true);
		ChatModel.Instance.SetLock(true);
	}
	#endregion


	#region 点击未读显示
	public void OnUnReadBtnClick(){
		HideUnReadMsg();
		AddNewNotify();
	}
	#endregion



	#region 未读
	int count = 0;
	public void ShowUnReadMsg(){
		++count;
		_view.UnReadPanel.SetActive(true);
//		_view.UnReadBtn.gameObject.SetActive(true);
		_view.UnReadCountLbl.text = string.Format("未读信息{0}条",count);
	}

	public void HideUnReadMsg(){
		_view.UnReadPanel.SetActive(false);
//		_view.UnReadBtn.gameObject.SetActive(false);
		count = 0;
		SetUnLock();
	}

	#endregion

	public void Dispose(){
		_view.ItemScrollView.onDragFinished -= JudgePos;
		ChatModel.Instance.OnNewNotifyUpdate -= AddNewNotify;
		ChatModel.Instance.ShowUnRead -= ShowUnReadMsg;
		ChatModel.Instance.SetCoolDownData -= SetCoolDownData;

	}


}
