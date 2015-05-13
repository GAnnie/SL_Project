using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.mail.dto;
using com.nucleus.h1.logic.core.modules.friend.dto;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.chat.modules.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.services;

public class FriendViewController : MonoBehaviour,IViewController {
	
	private FriendView _view;
	private List<TabBtnController> _typeTabBtnList;
	private TabBtnController _lastTypeTabBtn;

	private List<PlayerInfoItemCellController> _curInfoItemList = new List<PlayerInfoItemCellController> ();
	
	private List < FriendDto > _friendsList = new List < FriendDto >();

	private Dictionary<long,FriendDto> _friendDic = new Dictionary<long, FriendDto>();

	private List<MailDto> _emailsList;

	private List<RewardItemCellController> _itemCellControllerList;

	private FacePanelViewController facePanelViewController = null;

	private List<ChatNotify > friendMsgList  = new List<ChatNotify>();  //当前聊天信息//

	List<AddFriendItemCellController> FriendItemCellControllerList = new List<AddFriendItemCellController>();  //添加好友面板的item

	private const string PlayerInfoItemPrefabPath = "Prefabs/Module/FriendModule/playerInfoItemCell";
	private const string RewardItemCellPrefabPath = "Prefabs/Module/FriendModule/RewardItemCell";
	private const string AddFriendItemCellPath = "Prefabs/Module/FriendModule/AddFriendItemCell";
	private const string FacePanelViewPath = "Prefabs/Module/FriendModule/FacePanelView";
	private const string ChatItemCellViewPath = "Prefabs/Module/FriendModule/ChatItemCellView";         //玩家聊天的prefab
	private const string tipsStr = "你正在与陌生人聊天,请提防被骗！";
	private const string choiceOneMail = "请选择要阅读的邮件！";
	private const string NoMailStr = "暂时没有邮件哦！";

	List<ChatItemCellController> chatItemCellControllerList = new List<ChatItemCellController>();

	int _curTab = 0;
	int lastPlayInfoBtn = -1;
	int _curChoiseMail = 0;
	
	public void InitView (){
		
		_view = gameObject.GetMissingComponent<FriendView> ();
		_view.Setup (this.transform);
		RegisterEvent ();

		EmailModel.Instance.GetMailList ();

		_friendsList = FriendModel.Instance.GetFriendList();

		for(int i = 0 ; i < _friendsList.Count; i++){
			_friendDic.Add(_friendsList[i].shortPlayerDto.id,_friendsList[i]);
		}
		
		//tab btn start***************************************
		_typeTabBtnList = new List<TabBtnController>(2);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.TABBUTTON_H2) as GameObject;
		for (int i=0; i<2; ++i) {
			GameObject item = NGUITools.AddChild (_view.TypeTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_typeTabBtnList.Add (com);
			_typeTabBtnList [i].InitItem (i, OnSelectPetTypeTabBtn);
		}
		
		_typeTabBtnList [0].SetBtnName ("联系人");
		_typeTabBtnList [1].SetBtnName ("邮箱");

		//end********************************************

		OnSelectPetTypeTabBtn(0);
	}

	void Awake(){
		FriendModel.Instance.SetCon(this);
	}

	int intervalTime = 0;//检测语音的能量值//
	void Update(){
		if( VoiceRecognitionManager.Instance.IsRecord() )
		{
			if( intervalTime %5 ==4 )
			{
				_view.EnergyBar.fillAmount = VoiceRecognitionManager.Instance.GetVoiceAveraged() ;
				intervalTime  = 0;
			}
			else
			{
				intervalTime++;
			}
		}
		else
		{
			intervalTime = 0;
		}
	}

	public void RegisterEvent (){

		_view.SpeedBtn.onPress += OnPress;
		_view.SpeedBtn.onDrag += OnDrag;

		EventDelegate.Set (_view.SpeedOrInputBtn.onClick, OnSpeedOrInputBtnClick);
		EventDelegate.Set (_view.FaceBtn.onClick, OnFaceBtnClick);
		EventDelegate.Set (_view.AddFriendBtn.onClick, OnAddFriendBtnClick);
		EventDelegate.Set (_view.HomeBtn.onClick, OnHomeBtnClick);/// 这里的命名需要修改
		EventDelegate.Set (_view.GiftBtn.onClick, OnGiftBtnClick);
		EventDelegate.Set (_view.addBtn.onClick, OnAddBtnClick);
		EventDelegate.Set (_view.CloseBtn.onClick, OnCloseBtnClick);
		EventDelegate.Set (_view.RewardBtn.onClick, OnRewardBtnClick);
		EventDelegate.Set (_view.CloseAddFriendViewBtn.onClick, CloseAddFriendView);
		EventDelegate.Set (_view.ChatInputLbl.onSubmit, OnSendBtnClick);
		EventDelegate.Set (_view.ChangeBtn.onClick, OnChangeBtnClick);
		EventDelegate.Set (_view.SeachBtn.onClick, OnSeachBtnClick);
		EventDelegate.Set(_view.SendBtn.onClick,OnSendBtnClick);


		FriendModel.Instance.OnNewNotifyUpdateView += UpdateChatMsg;
		FriendModel.Instance.BeginChatEvt += OnSelectFriend;
		FriendModel.Instance.UpdateFriendView += UpdateFriendInfoItemView;
		FriendModel.Instance.OnOFFLineEVT += FriendOnOffLine;
	}
	
	
	#region 语音	
	void OnPress(GameObject go,bool isPress) 
	{ 
		if(FriendModel.Instance.GetTalkToWho() == -1){
			TipManager.AddTip("请选择朋友进行聊天");
			return;
		}

		if(isPress){
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(true);
			_view.GiveUpObj.SetActive(false);
			_view.ShortVoiceObj.SetActive(false);
			_view.VoiceTipLbl.text = "手指划开,取消发送";
			VoiceRecognitionManager.Instance.BeginRecord(OnChatBtnRelease);
		}
		else{
			_view.SpeechAnchorObj.SetActive(false);
			OnChatBtnRelease();
		}
	}


	public void OnDrag(GameObject go, Vector2 delta){
		
		if(delta.y <0 && Mathf.Abs(delta.y) > 10f){
			_view.SpeechObj.SetActive(false);
			_view.GiveUpObj.SetActive(true);
			_view.ShortVoiceObj.SetActive(false);
			_view.VoiceTipLbl.text = "松开手指,取消发送";
			VoiceRecognitionManager.Instance.FingersDragOut();
		}
		if(delta.y >0 && Mathf.Abs(delta.y) > 10f){
			_view.SpeechObj.SetActive(true);
			_view.GiveUpObj.SetActive(false);
			_view.ShortVoiceObj.SetActive(false);
			_view.VoiceTipLbl.text = "手指划开,取消发送";
			VoiceRecognitionManager.Instance.FingersDragOver();
		}
	}

	public void OnChatBtnRelease(){

		_view.SpeechAnchorObj.SetActive(false);
		
		VoiceRecognitionManager.Instance.FinishRecord( delegate(string msg,string time) {
			
			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"0");
			
			FriendModel.Instance.Talk(str);

			ChatNotify notify = new ChatNotify();
			notify.channelId = 6;
			notify.content =str;
			notify.fromPlayer = new ShortPlayerDto();
			notify.fromPlayer.id = PlayerModel.Instance.GetPlayerId();
			FriendModel.Instance.AddMsg(notify);
			
		},delegate(string msg) {

			string  str = ChatModel.Instance.GetUrlInfo(msg,0,0,-1,"1");
			FriendModel.Instance.Talk(str);

			ChatNotify notify = new ChatNotify();
			notify.channelId = 6;
			notify.content = str;
			notify.fromPlayer = new ShortPlayerDto();
			notify.fromPlayer.id = PlayerModel.Instance.GetPlayerId();
			FriendModel.Instance.AddMsg(notify);

		},delegate {
			//太短时间
			_view.SpeechAnchorObj.SetActive(true);
			_view.SpeechObj.SetActive(false);
			_view.GiveUpObj.SetActive(false);
			
			_view.ShortVoiceObj.SetActive(true);
			
			CoolDownManager.Instance.SetupCoolDown("showShortVoiceTip",2f,null,delegate {
				_view.SpeechAnchorObj.SetActive(false);
				_view.ShortVoiceObj.SetActive(false);
			});
		} );
	}





	bool isSpeedType =true;
	public void OnSpeedOrInputBtnClick(){
		if (isSpeedType) {
			_view.SpeedBtn.gameObject.SetActive(true);
			_view.ChatInputLbl.gameObject.SetActive(false);
			isSpeedType = false;
			_view.SpeedOrInputBg.spriteName = "keyboard-little-icon";

		}
		else{
			_view.SpeedBtn.gameObject.SetActive(false);
			_view.ChatInputLbl.gameObject.SetActive(true);
			isSpeedType = true;
			_view.SpeedOrInputBg.spriteName = "sould-little-icon";
		}
	}


	#endregion


	#region 表情
	int itemId;
	long uniquideId;
	int type;
	public void OnFaceBtnClick(){

		GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab (FacePanelViewPath) as GameObject;
		GameObject module = NGUITools.AddChild (this.gameObject, modulePrefab);
		UIHelper.AdjustDepth (module, 1);
		facePanelViewController = module.GetMissingComponent<FacePanelViewController> ();
		facePanelViewController.InitView ();
		facePanelViewController.SetCallback (delegate(string str,int itemId,long uniquidId,int type) {

			if(FriendModel.Instance.GetTalkToWho() == -1){
				TipManager.AddTip("请选择朋友进行聊天");
				return;
			}


			if(type ==1){
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = type;
			}
			
			if(type == 2){  //技能
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 6;
			}
			if(type == 3 ){ // 宠物
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 2;
			}
			if(type == 4){  //伙伴
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",str,"%#%");
				this.itemId = itemId;
				this.uniquideId = uniquideId;
				this.type = 7;
			}
			OnSendBtnClick();

		}, null,delegate(string name, int id,int whichType) {
			//_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
			///type : 1 任务  2.成就 3.称谓    
			/// 
			switch(whichType){
				
			case 1:
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 3;
				
				break;
			case 2:
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 4;
				break;
				
			case 3:
				_view.ChatInputLbl.value += string.Format("{0}{1}{2}","%#%",name,"%#%");
				this.itemId = id;
				this.uniquideId = 0;
				this.type = 5;
				break;
			}
			OnSendBtnClick();

	});
		//表情
		facePanelViewController.SetExpressionCallback(delegate(int no) {
			
			string s = _view.ChatInputLbl.value;
			_view.ChatInputLbl.value += "#"+no;

			onChange();
			
			if(hasFive){
				_view.ChatInputLbl.value = s;
				TipManager.AddTip("只能输入5个表情");
			}
		});

	}

	#region 检测一输入多少个表情
	bool hasFive = false;
	public void onChange(){
		
		string str = _view.ChatInputLbl.value;
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
	#endregion


	private long _curFriendId;
	#region 发送
	public void OnSendBtnClick(){

		if (facePanelViewController != null) {
			Destroy(facePanelViewController.gameObject);
			facePanelViewController = null;
		}

		if(FriendModel.Instance.GetTalkToWho() == -1){
			TipManager.AddTip("请选择朋友进行聊天");
			return;
		}

		string msg = ChatModel.Instance.GetUrlInfo(_view.ChatInputLbl.value,itemId,uniquideId,type);

		FriendModel.Instance.Talk(FriendModel.Instance.GetUrlInfo(msg,itemId,uniquideId));


		ChatNotify notify = new ChatNotify();
		notify.channelId = 6;
		notify.content = msg;
		notify.fromPlayer = new ShortPlayerDto();
		notify.fromPlayer.id = PlayerModel.Instance.GetPlayerId();
		FriendModel.Instance.AddMsg(notify);
		_view.ChatInputLbl.value = "";
	}
	#endregion
	
	#region 添加好友面板
	bool canChangeRecommendFriend = true;
	bool autoChangeRecommendFriend = true;
	List<ShortPlayerDto>  recommendFriendList= new List<ShortPlayerDto>();

	//这里是推荐的
	public void OnAddFriendBtnClick(){

		if(autoChangeRecommendFriend){


			ServiceRequestAction.requestServer (FriendService.recommendFriend(),"recommendFriend",(e)=>{
				autoChangeRecommendFriend = false;

				recommendFriendList = (e as FriendSearchDto).items;
				
				if(FriendItemCellControllerList.Count < recommendFriendList.Count){
					
					GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(AddFriendItemCellPath) as GameObject;
					
					for(int i = FriendItemCellControllerList.Count; i <recommendFriendList.Count;i++){
						GameObject item = NGUITools.AddChild(_view.RecommendFriendsTable.gameObject,itemPrefab);
						AddFriendItemCellController com = item.GetMissingComponent<AddFriendItemCellController>();
						com.InitView();
						FriendItemCellControllerList.Add(com);
					}
					
				}
				
				for(int i = 0; i <recommendFriendList.Count ; i++){

					FriendItemCellControllerList[i].SetData(recommendFriendList[i]);
					FriendItemCellControllerList[i].gameObject.SetActive(true);

				}
				
				for(int i = recommendFriendList.Count; i < FriendItemCellControllerList.Count; i++){
					FriendItemCellControllerList[i].gameObject.SetActive(false);
				}
				
				
				_view.TipsObj.SetActive (false);
				_view.TitleSprite.spriteName = "bone-of-add-friend";
				_view.TitleSprite.MakePixelPerfect ();
				_view.LeftPanelObj.SetActive (false);
				_view.RightPanelObj.SetActive (false);
				_view.AddFriendsObj.SetActive (true);
				_view.RecommendFriendsTable.repositionNow = true;
	
				CoolDownManager.Instance.SetupCoolDown("autoChangeRecommendFriend",10f,null,delegate {
					autoChangeRecommendFriend = true;
				});

			});
		}
		else{
			for(int i = 0; i <recommendFriendList.Count ; i++){

				FriendItemCellControllerList[i].SetData(recommendFriendList[i]);
				FriendItemCellControllerList[i].gameObject.SetActive(true);
			}

			_view.TipsObj.SetActive (false);
			_view.TitleSprite.spriteName = "bone-of-add-friend";
			_view.TitleSprite.MakePixelPerfect ();
			_view.LeftPanelObj.SetActive (false);
			_view.RightPanelObj.SetActive (false);
			_view.AddFriendsObj.SetActive (true);
			_view.RecommendFriendsTable.repositionNow = true;
		}

	}
	#endregion
	#region 收藏
	public void OnHomeBtnClick(){
		TipManager.AddTip ("收藏?");
	}
	#endregion

	#region 分享
	public void OnGiftBtnClick(){
		TipManager.AddTip ("分享?");
	}
	#endregion

	#region 好友度处添加当前好友
	public void OnAddBtnClick(){
		FriendModel.Instance.AddFriend(_curFriendId);
	}
	#endregion


	#region 换一批
	public void OnChangeBtnClick(){

		if(canChangeRecommendFriend){

			ServiceRequestAction.requestServer (FriendService.recommendFriend(),"recommendFriend",(e)=>{
				canChangeRecommendFriend = false;

				recommendFriendList = (e as FriendSearchDto).items;
				
				if(FriendItemCellControllerList.Count < recommendFriendList.Count){
					
					GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(AddFriendItemCellPath) as GameObject;
					
					for(int i = FriendItemCellControllerList.Count; i <recommendFriendList.Count;i++){
						GameObject item = NGUITools.AddChild(_view.RecommendFriendsTable.gameObject,itemPrefab);
						AddFriendItemCellController com = item.GetMissingComponent<AddFriendItemCellController>();
						com.InitView();
						FriendItemCellControllerList.Add(com);
					}
					
				}
				
				for(int i = 0; i <recommendFriendList.Count ; i++){
					
					FriendItemCellControllerList[i].SetData(recommendFriendList[i]);
					FriendItemCellControllerList[i].gameObject.SetActive(true);
					
				}
				
				for(int i = recommendFriendList.Count; i < FriendItemCellControllerList.Count; i++){
					FriendItemCellControllerList[i].gameObject.SetActive(false);
				}
				
				
				_view.TipsObj.SetActive (false);
				_view.TitleSprite.spriteName = "bone-of-add-friend";
				_view.TitleSprite.MakePixelPerfect ();
				_view.LeftPanelObj.SetActive (false);
				_view.RightPanelObj.SetActive (false);
				_view.AddFriendsObj.SetActive (true);
				_view.RecommendFriendsTable.repositionNow = true;

				CoolDownManager.Instance.SetupCoolDown("ChangeRecommendFriend",10f,null,delegate {
					canChangeRecommendFriend = true;
				});
			});

		}
			else{
				TipManager.AddTip("推荐太频繁啦");
			}
	}

	#region 查找
	bool canSearch= true;
	public void OnSeachBtnClick(){

		if(string.IsNullOrEmpty(_view.InputLbl.value)){
			TipManager.AddTip("不能为空");
			return;
		}

		if(canSearch){

			canSearch =false;

			ServiceRequestAction.requestServer(FriendService.searchFriend(_view.InputLbl.value),"searchFriend",(e)=>{
				_view.InputLbl.value = "";

				ShortPlayerDto dto = null;

				if((e as FriendSearchDto).items.Count  > 0 ){
					dto = (e as FriendSearchDto).items[0] as ShortPlayerDto;
				}

				if(dto != null){

					if(FriendItemCellControllerList.Count <1 ){
						GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(AddFriendItemCellPath) as GameObject;

						GameObject item = NGUITools.AddChild(_view.RecommendFriendsTable.gameObject,itemPrefab);
						AddFriendItemCellController com = item.GetMissingComponent<AddFriendItemCellController>();
						com.InitView();
						FriendItemCellControllerList.Add(com);
					}

					FriendItemCellControllerList[0].gameObject.SetActive(true);
					FriendItemCellControllerList[0].SetData(dto);

					if(FriendModel.Instance.IsMyFriend(dto.id)){
						FriendItemCellControllerList[0].IsFriend();
					}

					for(int i =1;i < FriendItemCellControllerList.Count;i++){
						FriendItemCellControllerList[i].gameObject.SetActive(false);
					}
					
					_view.RecommendFriendsTable.repositionNow = true;
				}
				else{
					TipManager.AddTip("您搜索的玩家不存在或者不在线");
					for(int i =0;i < FriendItemCellControllerList.Count;i++){
						FriendItemCellControllerList[i].gameObject.SetActive(false);
					}
				}


				CoolDownManager.Instance.SetupCoolDown("searchFriend",5f,null,delegate {
					canSearch = true;
				});
			});

		}
		else{
			TipManager.AddTip("搜索太频繁啦");
		}

	}
	#endregion
	#endregion


	#region 联系人/邮箱 Tab
	List<long> redPoint = new List<long>();
	private void OnSelectPetTypeTabBtn(int index){
		FriendModel.Instance.SetTalkToWho(-1);
		lastPlayInfoBtn = -1;

		if(_lastTypeTabBtn != null)
			_lastTypeTabBtn.SetSelected(false);
		
		_lastTypeTabBtn = _typeTabBtnList[index];
		_lastTypeTabBtn.SetSelected(true);

		_view.TipsObj.gameObject.SetActive(false);
		FriendModel.Instance.SetTalkToWho(-1);
		_curFriendId = -1;

		if (index == 0) {
			redPoint = FriendModel.Instance.GetRedPointCount();
			OnSelectPetType_Regular(index);

		}
		else if(index == 1){
			OnSelectPetType_Myth(index);
		}
		
	}
	#endregion
	
	#region 根据 联系人/邮箱 Tab 更改显示内容
	private void OnSelectPetType_Regular(int index){
		_curTab = index;
		UpdateFriendInfoItemView (index);
	}
	
	private void OnSelectPetType_Myth(int index){
		_curTab = index;
		UpdateFriendInfoItemView (index);
	}
	
	public void UpdateFriendInfoItemView(int index){

		//0 好友  1 邮箱
		_view.RewardBtn.gameObject.SetActive (false);
		_view.ItemTable.gameObject.SetActive(false);
		_view.tipSpriteObj.gameObject.SetActive(true);
		_view.TitleSprite.spriteName = "bone-of-friend";
		_view.TitleSprite.MakePixelPerfect ();


		//隐藏好友度
		_view.ChatBtnGroupObj.SetActive(false);

		if (index == 0) {
			FriendModel.Instance.FriendSort();
			_friendsList = FriendModel.Instance.GetFriendList();

			_view.TipsLbl.text = tipsStr;
			_view.EmailObj.SetActive(false);
			_view.NoFriendTextObj.SetActive(true);
			_view.NoMailTextObj.SetActive(false);

		//如果数量不够 就多添加几个~

			if(_curInfoItemList.Count < _friendsList.Count){
				GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(PlayerInfoItemPrefabPath) as GameObject;
				
				for(int i = _curInfoItemList.Count; i <_friendsList.Count;i++){
					GameObject item = NGUITools.AddChild(_view.itemGrid.gameObject,itemPrefab);
					PlayerInfoItemCellController com = item.GetMissingComponent<PlayerInfoItemCellController>();
					com.InitView();
					_curInfoItemList.Add(com);
				}
			}
			

			for(int i = 0; i<_friendsList.Count; i++){

				_curInfoItemList[i].SetSelected(false);

				_curInfoItemList[i].gameObject.SetActive(true);

				//头像
				_curInfoItemList[i].SetFaceIcon("player_default");

				_curInfoItemList[i].SetData(i,_friendsList[i],OnSelectFriend);
				_curInfoItemList[i].SetOnOffLine(_friendsList[i].connected);

				_curInfoItemList[i].SetName(string.Format("[b]{0}[-]",_friendsList[i].shortPlayerDto.nickname));
				_curInfoItemList[i].SetLevel(string.Format("[b]{0}级 {1}[-]",_friendsList[i].shortPlayerDto.grade,DataCache.getDtoByCls<Faction>(_friendsList[i].shortPlayerDto.factionId).name));

				_curInfoItemList[i].SetAngleIcon(!_friendsList[i].friend);
			}




			for(int i = 0 ; i < _curInfoItemList.Count;i++){
				for(int j = 0; j < redPoint.Count; j++){
					_curInfoItemList[i].HideRedPoint();
				}
			}


			//如果有小红点要显示
			if(redPoint.Count > 0 ){
				for(int i = 0 ; i < _curInfoItemList.Count;i++){
					for(int j = 0; j < redPoint.Count; j++){
						if(_curInfoItemList[i].MatchFriend() == redPoint[j]){
							_curInfoItemList[i].ShowRedPoint();
						}
					}
				}
			}



			for(int i = _friendsList.Count;i<_curInfoItemList.Count;i++){
				_curInfoItemList[i].gameObject.SetActive(false);
			}

			_view.itemGrid.repositionNow = true;

		}


		//邮箱
			
		else{
			//隐藏聊天的table
			_view.ItemTable.gameObject.SetActive(false);

			_view.TitleSprite.spriteName ="bone-of-letter";
			_view.TitleSprite.MakePixelPerfect ();
			_emailsList = EmailModel.Instance.GetMailList();


			//如果数量不够 就多添加几个~
			if(_curInfoItemList.Count < _emailsList.Count){
				GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(PlayerInfoItemPrefabPath) as GameObject;
				
				for(int i = _curInfoItemList.Count; i <_emailsList.Count;i++){
					GameObject item = NGUITools.AddChild(_view.itemGrid.gameObject,itemPrefab);
					PlayerInfoItemCellController com = item.GetMissingComponent<PlayerInfoItemCellController>();
					com.InitView();
					_curInfoItemList.Add(com);
				}
			}

			_view.ChatBtnGroupObj.SetActive(false);
			_view.EmailObj.SetActive(true);
			_view.NoFriendTextObj.SetActive(false);
			_view.NoMailTextObj.SetActive(true);
			_view.RewardSplitObj.SetActive(false);
			_view.EmailContentLbl.text = "";
			_view.TipsObj.SetActive(false);
			_view.RewardItemGrid.gameObject.SetActive(false);

			if(_emailsList.Count ==0){
				_view.WarmLbl.text = NoMailStr;
//				return;
			}
			else{
				_view.WarmLbl.text = choiceOneMail;
			}
			if(_emailsList.Count > 0){
				OnSelectFriend(0);
			}

			
			//更改前面的信息
			for(int i = 0 ; i<_emailsList.Count; i++){

				_curInfoItemList[i].gameObject.SetActive(true);

				//***修改icon
				if(_emailsList[i].mailAttachments.Count > 0){
					_curInfoItemList[i].SetFaceIcon(_emailsList[i].mailAttachments[0].id.ToString());
				}
				else{
					_curInfoItemList[i].SetFaceIcon("letter-icon");
				}
				//***
				_curInfoItemList[i].SetData(i,OnSelectFriend);
				_curInfoItemList[i].SetAngleIcon(false);
				_curInfoItemList[i].SetName(_emailsList[i].subject);
				_curInfoItemList[i].SetOnOffLine(true);
				string overTime;

				System.DateTime dt = DataHelper.ConvertIntDateTime(_emailsList[i].createTime);
				dt=dt.AddDays((double)_emailsList[i].mailType.keepDay);
				overTime = string.Format("{0:yyyy-MM-dd HH:mm}", dt);
				string overYm = string.Format("{0:yyyy-MM-dd}", dt);          // yy:mm:dd 时间
				string overDT = overTime.Substring(overTime.Length -5);   //hh:mm时间
				_curInfoItemList[i].SetFrom(string.Format("发件人:{0}\n过期时间:{1}\n{2}",_emailsList[i].fromName,overYm,overDT));
				_curInfoItemList[i].SetClickBtnHide();
				_curInfoItemList[i].SetAngleIcon(false);
			}
			
			//多余的就隐藏掉
			for(int i = _emailsList.Count; i<_curInfoItemList.Count; i++){
				_curInfoItemList[i].gameObject.SetActive(false);
			}
			_view.itemGrid.repositionNow = true;
		}
		
		
	}
	#endregion



	#region 如果当前是聊天界面,并且正在聊天
	public void UpdateChatMsg(long id){

		//聊天界面
		if(_curTab == 0){

			//如果有消息来并且是我正在聊的这个
			if(_curFriendId == id){
				
				//显示聊天的table
				_view.ItemTable.gameObject.SetActive(true);
				
				
				// 取我当前玩家的这个聊天内容
				
				friendMsgList = FriendModel.Instance.GetChatFriendMsg(_curFriendId);
				
				//如果数量不够
				if(chatItemCellControllerList.Count < friendMsgList.Count){
					GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatItemCellViewPath) as GameObject;
					GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
					ChatItemCellController com = module.GetMissingComponent<ChatItemCellController>();
					module.transform.SetAsFirstSibling ();
					com.InitView ();
					chatItemCellControllerList.Add(com);
				}
				
				//修改前面内容
				for(int i = 0 ; i <friendMsgList.Count;i++){
					chatItemCellControllerList[i].HidePlayerName();
					if(friendMsgList[i].fromPlayer.id == PlayerModel.Instance.GetPlayerId()){
						chatItemCellControllerList[i].SetDate(friendMsgList[i],true,440,560);
					}
					else{
						chatItemCellControllerList[i].SetDate(friendMsgList[i],false,440,560);
					}
					chatItemCellControllerList[i].gameObject.SetActive(true);
				}
				
				//多余的隐藏掉
				for(int i = friendMsgList.Count; i <chatItemCellControllerList.Count; i++){
					chatItemCellControllerList[i].gameObject.SetActive(false);
				}
				
				_view.ItemTable.repositionNow = true;
				_view.ItemTable.Reposition();
				//			Debug.LogError("和我聊的这个来新消息了");
			}
			else{
				//			Debug.LogError("新消息是另一个家伙的");
				//可以加上显示红点
				for(int i = 0; i < _curInfoItemList.Count; i++){
					if(_curInfoItemList[i].MatchFriend() == id){
						_curInfoItemList[i].ShowRedPoint();
					}
				}
			}
		}



	}
	#endregion

	//点击
	public void OnSelectFriend(int i){
		_curChoiseMail = i;
		_curInfoItemList[i].SetSelected(true);
		_view.RewardBtn.gameObject.SetActive (true);
		_view.TipsObj.SetActive (true);



		if (lastPlayInfoBtn == -1) {
			lastPlayInfoBtn = i;
		}
		else{
			if(lastPlayInfoBtn != i){
				_curInfoItemList[lastPlayInfoBtn].SetSelected(false);
				lastPlayInfoBtn = i;
			}
		}


		
		// 如果当前是好友的点击回调
		if (_curTab == 0) {
			_view.ItemTable.gameObject.SetActive(true);
			_view.NoFriendTextObj.SetActive(false);
			_view.TipsObj.gameObject.SetActive(!_friendsList[i].friend);
			_curFriendId = _friendsList[i].shortPlayerDto.id;

			_view.ChatBtnGroupObj.SetActive(true);
			_view.FriendlyDegreeLbl.text = string.Format("{0}{1}","友好度 :  ",_friendsList[i].degree);
			string RelationShip = _friendsList[i].friend == true ? "[9eef5d]普通好友[-]" : "[F57A7A]陌生人[-]";
			if(_friendsList[i].friend){
				_view.addBtn.gameObject.SetActive(false);
			}
			else{
				_view.addBtn.gameObject.SetActive(true);
			}
			_view.RelationShipLbl.text = string.Format("{0}",RelationShip);

			FriendModel.Instance.SetTalkToWho(_curFriendId);
			FriendModel.Instance.RemoveRedPoint(_curFriendId);
//			Debug.LogError("当前点击的玩家id:" + _curFriendId + "||" + "名字:"+_friendsList[i].shortPlayerDto.nickname);

			friendMsgList = FriendModel.Instance.GetChatFriendMsg(_curFriendId);
			_curInfoItemList[lastPlayInfoBtn].HideRedPoint();
			
			//如果数量不够
			if(chatItemCellControllerList.Count < friendMsgList.Count){
				GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab (ChatItemCellViewPath) as GameObject;
				for(int index = chatItemCellControllerList.Count; index < friendMsgList.Count; index++){
					GameObject module = NGUITools.AddChild(_view.ItemTable.gameObject,prefab);
					ChatItemCellController com = module.GetMissingComponent<ChatItemCellController>();
					module.transform.SetAsFirstSibling ();
					com.InitView ();
					chatItemCellControllerList.Add(com);
				}
			}
			
			//修改前面内容
			for(int index = 0 ; index < friendMsgList.Count;index++){
				chatItemCellControllerList[index].HidePlayerName();

				if(friendMsgList[index].fromPlayer.id == PlayerModel.Instance.GetPlayerId()){
					chatItemCellControllerList[index].SetDate(friendMsgList[index],true,440,560);
				}
				else{
					chatItemCellControllerList[index].SetDate(friendMsgList[index],false,440,560);
				}
				chatItemCellControllerList[index].gameObject.SetActive(true);
			}
			
			//多余的隐藏掉
			for(int index = friendMsgList.Count; index <chatItemCellControllerList.Count; index++){
				chatItemCellControllerList[index].gameObject.SetActive(false);
			}
			
			_view.ItemTable.repositionNow = true;
			_view.ItemTable.Reposition();

		}

		//否则邮件点击回调
		else{

			//隐藏tip sprite
			_view.tipSpriteObj.gameObject.SetActive(false);
			_view.NoFriendTextObj.SetActive(false);
			//隐藏texture
			_view.NoMailTextObj.SetActive (false);
			//邮件内容
			_view.EmailContentLbl.text = _emailsList [i].content;
			//表头
			_view.TipsLbl.text =_emailsList [i].subject;
			_view.RewardItemGrid.gameObject.SetActive(true);

			//如果有奖励
			if (_emailsList [i].mailAttachments.Count != 0) {

				_view.RewardSplitObj.SetActive(true);

				GameObject rewardItemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(RewardItemCellPrefabPath) as GameObject;


				if(_itemCellControllerList == null){
					_itemCellControllerList = new List<RewardItemCellController>();
				}

				//如果已有的奖励itemcell 不够
				if(_itemCellControllerList.Count < _emailsList [i].mailAttachments.Count){

					GameObject rewardItemCellPrefab1 = ResourcePoolManager.Instance.SpawnUIPrefab(RewardItemCellPrefabPath) as GameObject;
				
					for(int index = _itemCellControllerList.Count; index < _emailsList[i].mailAttachments.Count; index++){
						GameObject item = NGUITools.AddChild(_view.RewardItemGrid.gameObject,rewardItemCellPrefab);
						RewardItemCellController com = item.GetMissingComponent<RewardItemCellController>();
						com.InitView();
						_itemCellControllerList.Add(com);
					}
				}

				//更改前面的信息
				for(int index = 0 ; index < _emailsList [i].mailAttachments.Count; index++){
					_itemCellControllerList[index].gameObject.SetActive(true);
					_itemCellControllerList[index].SetMailReward(_emailsList[i].mailAttachments[index].id,_emailsList[i].mailAttachments[index].count,RewardItemCellCallBack);
				}
				
				//多余的就隐藏掉
				for(int index = _emailsList [i].mailAttachments.Count; index < _itemCellControllerList.Count; index++){
					_itemCellControllerList[index].gameObject.SetActive(false);
				}
			
			}
			_view.RewardScrollView.ResetPosition();
		}
		
		_view.RewardItemGrid.repositionNow = true;
	}

	#region 奖励物品的点击回调
	public void RewardItemCellCallBack(RewardItemCellController con){
		ProxyItemTipsModule.Open (con.RewardItemId, con.gameObject,1);
	}
	#endregion


	//领取奖励 删除邮件
	public void OnRewardBtnClick(){

		if(BackpackModel.Instance.IsFull())
		{
			TipManager.AddTip("物品栏已满，无法领取");
			return;
		}

		ServiceRequestAction.requestServer (MailService.extract (_emailsList[lastPlayInfoBtn].id), "Get Reward from this mail",delegate {

			EmailModel.Instance.MarkThisMail(_emailsList[lastPlayInfoBtn].id);
			_emailsList.RemoveAt (lastPlayInfoBtn);
			StartCoroutine ("UpdateMailView");

		},delegate(com.nucleus.commons.message.ErrorResponse errorResponse) {
			TipManager.AddTip(errorResponse.message);
		});
	}

	int ChoisePosition;
	//更新邮件列表
	IEnumerator UpdateMailView(){
		yield return 0.5f;
		ChoisePosition = _curChoiseMail;
		_view.RewardBtn.gameObject.SetActive (false);
		UpdateFriendInfoItemView (1);
		ChoiseNextOne ();

	}
	public void ChoiseNextOne(){
		int i = ChoisePosition;
		while(i > _emailsList.Count-1){
			--i;
		}
		if (i <= 0) {
			UpdateFriendInfoItemView (1);		
		}
		else{
			OnSelectFriend(i);
		}
	}


	public void Dispose(){
		FriendModel.Instance.OnNewNotifyUpdateView -= UpdateChatMsg;
		FriendModel.Instance.BeginChatEvt -= OnSelectFriend;
		FriendModel.Instance.UpdateFriendView -= UpdateFriendInfoItemView;
		FriendModel.Instance.OnOFFLineEVT -= FriendOnOffLine;
	}


	//关闭
	public void OnCloseBtnClick(){
		FriendModel.Instance.SetTalkToWho(-1);
		ProxyFriendModule.Close ();
	}

	#region 关闭推荐好友界面
	public void CloseAddFriendView(){
		_view.AddFriendsObj.SetActive (false);
		_view.LeftPanelObj.SetActive (true);
		_view.RightPanelObj.SetActive (true);
		UpdateFriendInfoItemView (_curTab);
	}
	#endregion

	#region 上下线无刷新更改好友头像位置及颜色    其实也刷新了
	public void FriendOnOffLine(long id){

		for(int i = 0 ; i < _curInfoItemList.Count ; i++){
			_curInfoItemList[i].ClearInfo();
		}

		if(_curTab == 0){
			OnSelectPetTypeTabBtn(0);
		}

		if(id !=-1){
			for(int i = 0 ; i < _curInfoItemList.Count; i++){
				if(id == _curInfoItemList[i].MatchFriend()){
					_curInfoItemList[i].OnClickItem();
					break;
				}
			}
		}

	}
	#endregion

	#region 新消息来了重刷

	#endregion

	#region 删除好友清除点击内容
	public void ClearInfo(){
		lastPlayInfoBtn = -1;
		_curFriendId = -1;
	}
	#endregion
}
