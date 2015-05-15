﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.chat.modules.data;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.chat.modules.dto;
using com.nucleus.h1.logic.core.modules.system.dto;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.player.msg;
using System.Text.RegularExpressions;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.title.dto;
using com.nucleus.h1.logic.core.modules.title.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class ChatModel {

	private static readonly ChatModel instance = new ChatModel();
	public static ChatModel Instance
	{
		get{
			return instance;
		}
	}

	//新消息来了
	public event System.Action OnNewNotifyUpdate;
	//锁了显示未读
	public event System.Action ShowUnRead;
	//倒计时
	public event System.Action<float> SetCoolDownData;

	//MainUIAddNewMsg
	public event System.Action<string> MainUIAddNewChatMsg;

	public int _maxMessageCount = 100;//每个频道的小心上限//
	
	public List< ChatNotify > _worldTalkList = new List<ChatNotify>();   //世界频道信息//


	public List< ChatNotify > _guildTalkList  = new List<ChatNotify>();  //帮派频道信息//

	public List< ChatNotify > _teamTalkList  = new List<ChatNotify>();   //队伍频道信息//
	public List< SystemNotify > _SysTalkList  = new List<SystemNotify>();    //系统频道信息//

	public List< HearsayNotify > _HearsayTalkList  = new List<HearsayNotify>();//传闻频道信息//


	private bool isLock = false;

	public void SetLock(bool islock){
		isLock = islock;
	}

	
	private int _curChannel;
	public void SetCurChannel(int channel){
		_curChannel = channel;
	}


	public void addNotify(ChatNotify notify){

		switch (notify.channelId) {
			//帮派
		case ChatChannel.ChatChannelEnum_Guild:
			//加入自动播放队列
			if(SystemDataModel.Instance.factionToggle){
				AutoPlayMatch(notify.content);
			}



			//判断是否已经存在的 存在的只更新相应的
			if(FillTranslationContent(2,notify)){
				if(OnNewNotifyUpdate != null){
					OnNewNotifyUpdate();
				}
				MainUIViewController.Instance.UpdateChatMsg();
				break;
			}

			else{
				if(_guildTalkList.Count >99){
					_guildTalkList.RemoveAt(0);
				}
				_guildTalkList.Add(notify);


				//***这里如果是符合语音格式的话，就加上#leftV
				string guildMsg;
				if(MainUIContentMatch(notify.content)){
					guildMsg = jointMsg(5,notify.fromPlayer.nickname,"#leftV"+notify.content);
				}
				else{
					guildMsg = jointMsg(5,notify.fromPlayer.nickname,notify.content);
				}
				//[url=100026,-1,3b977d91-38d3-4ff1-b16b-c9d854a54a0b,100026-635652361858987490][/url]
				//****************************
				//是否已开启屏蔽
				if(receiveGuild){
					if(MainUIChatMsgList.Count >6){
						MainUIChatMsgList.RemoveAt(0);
					}
					MainUIChatMsgList.Add(guildMsg);

					if(MainUIAddNewChatMsg != null){
						MainUIAddNewChatMsg(guildMsg);
					}

				}

				if(isLock && _curChannel == 0){
					if(ShowUnRead != null){
						ShowUnRead();
					}
				}
				else{
					if(OnNewNotifyUpdate != null){
						OnNewNotifyUpdate();
					}
				}
				
				break;
			}

		break;

			//队伍
		case ChatChannel.ChatChannelEnum_Team:
			//加入自动播放队列
			if(SystemDataModel.Instance.contingentToggle)
			AutoPlayMatch(notify.content);


			//判断是否已经存在的
			if(FillTranslationContent(3,notify)){
				if(OnNewNotifyUpdate != null){
					OnNewNotifyUpdate();
				}
				MainUIViewController.Instance.UpdateChatMsg();
				break;
			}
			else{
				
				if(_teamTalkList.Count >99){
					_teamTalkList.RemoveAt(0);
				}
				_teamTalkList.Add(notify);
				string teamMsg;
				if(MainUIContentMatch(notify.content)){
					teamMsg = jointMsg(6,notify.fromPlayer.nickname,"#leftV"+notify.content);
				}
				else{
					teamMsg = jointMsg(6,notify.fromPlayer.nickname,notify.content);
				}
	

				//是否屏蔽
				if(receiveTeam){
					if(MainUIChatMsgList.Count >6){
						MainUIChatMsgList.RemoveAt(0);
					}
					MainUIChatMsgList.Add(teamMsg);
					if(MainUIAddNewChatMsg != null){
						MainUIAddNewChatMsg(teamMsg);
					}
				}

				if(isLock && _curChannel == 1){
					if(ShowUnRead != null){
						ShowUnRead();
					}
				}
				else{
					if(OnNewNotifyUpdate != null){
						OnNewNotifyUpdate();
					}
				}
				break;
			}
		break;
			//世界
		case ChatChannel.ChatChannelEnum_World:
			//加入自动播放队列
			if(SystemDataModel.Instance.worldToggle)
			AutoPlayMatch(notify.content);


			//判断是否已经存在的
			if(FillTranslationContent(1,notify)){
				if(OnNewNotifyUpdate != null){
					OnNewNotifyUpdate();
				}
				MainUIViewController.Instance.UpdateChatMsg();
				break;
			}
			
			else{
				if(_worldTalkList.Count >99){
					_worldTalkList.RemoveAt(0);
				}
				_worldTalkList.Add(notify);
				
				string worldMsg;
				if(MainUIContentMatch(notify.content)){
					worldMsg = jointMsg(4,notify.fromPlayer.nickname,"#leftV"+notify.content);
				}
				else{
					worldMsg = jointMsg(4,notify.fromPlayer.nickname,notify.content);
				}
				
				if(receiveWorld){
					if(MainUIChatMsgList.Count >6){
						MainUIChatMsgList.RemoveAt(0);
					}
					MainUIChatMsgList.Add(worldMsg);
					if(MainUIAddNewChatMsg != null){
						MainUIAddNewChatMsg(worldMsg);
					}

				}

				if(isLock){
					if(ShowUnRead != null && _curChannel == 2){
						ShowUnRead();
					}
				}
				else{
					if(OnNewNotifyUpdate != null){
						OnNewNotifyUpdate();
					}
				}
				
				break;
			}
		break;
		}

	}

	public string jointMsg(int type,string fr, string str){
		string s = "";




		switch(type){
		case 1:         //系统
			s = string.Format("[e61a1a]{0}[-] {1}","系统",str);
			break;
		case 2:         //帮助
			s = string.Format("[0c99c7]{0}[-] {1}","帮助",str);
			break;
		case 3:         //提示
			s = string.Format("[fff9e3]{0}[-] {1}","提示",str);
			break;
		case 4:         //世界
			str = str.Replace("[734228]","[2dac0a]");
			s = string.Format("[2dac0a]{0}[-]  [fff9e3][{1}][-] {2}","世界",fr,str);
			break;
		case 5:         //帮派
			str = str.Replace("[734228]","[0c99c7]");
			s = string.Format("[0c99c7]{0}[-]  [fff9e3][{1}][-] {2}","帮派",fr,str);
			break;
		case 6:         //队伍
			str = str.Replace("[734228]","[d44e0c]");
			s = string.Format("[d44e0c]{0}[-]  [fff9e3][{1}][-] {2}","队伍",fr,str);
			break;
		}
		return s;
	}



	//系统 消息              1.系统  2 .提示    3.帮助
	public void AddSysNotify(SystemNotify notify){

		switch (notify.lableType) {
		case ChatChannel.LableTypeEnum_System:
			MarqueeModel.Instance.AddSys(notify.content);
			_SysTalkList.Add(notify);
			string sysMsg =jointMsg(1,"",notify.content);

			if(MainUIChatMsgList.Count >6){
				MainUIChatMsgList.RemoveAt(0);
			}
			MainUIChatMsgList.Add(sysMsg);

			if(MainUIAddNewChatMsg != null){
				MainUIAddNewChatMsg(sysMsg);
			}

			break;
		case ChatChannel.LableTypeEnum_Prompt:
			_SysTalkList.Add(notify);

			string promptMsg = jointMsg(3,"",notify.content);

			if(MainUIChatMsgList.Count >6){
				MainUIChatMsgList.RemoveAt(0);
			}
			MainUIChatMsgList.Add(promptMsg);

			if(MainUIAddNewChatMsg != null){
				MainUIAddNewChatMsg(promptMsg);
			}

			break;
		case ChatChannel.LableTypeEnum_Help:
			_SysTalkList.Add(notify);

			string helpMsg = jointMsg(2,"",notify.content);

			if(MainUIChatMsgList.Count >6){
				MainUIChatMsgList.RemoveAt(0);
			}
			MainUIChatMsgList.Add(helpMsg);

			if(MainUIAddNewChatMsg != null){
				MainUIAddNewChatMsg(helpMsg);
			}

			break;
		}

		if(isLock){
			if(ShowUnRead != null && _curChannel == 4){
				ShowUnRead();
			}
		}
		else{
			if(OnNewNotifyUpdate != null){
				OnNewNotifyUpdate();
			}
		}


	}



	//mainUI
	List<string >MainUIChatMsgList = new List<string>(8);
	public List<string >GetMainUIChatMsg(){
		return MainUIChatMsgList;
	}

	public void AddSysNotifyAffterBattle(List<object> _battleRewardNotifyList){
		SystemNotify battleRewardNotify = new SystemNotify ();


		battleRewardNotify.lableType = 1;

		foreach (object notify in _battleRewardNotifyList)
		{
			if (notify is CharactorExpNotify)
			{
				//你获得[5CF37C]60[-]#exp1、478#w3

				CharactorExpNotify expNotify = notify as CharactorExpNotify;
//
//				PetPropertyInfo petInfo = null;
//				int petIndex = PetModel.Instance.GetPetIndex((notify as CharactorExpInfoNotify).id);
//				if(petIndex != -1){
//					petInfo = PetModel.Instance.GetPetInfoByIndex(petIndex);
//				}
				if(!string.IsNullOrEmpty((notify as CharactorExpNotify).mainCharactorExpInfo.expGain.ToString()))

				battleRewardNotify.content +=  string.Format("你获得[5cf37c]{0}[-]#exp1",(notify as CharactorExpNotify).mainCharactorExpInfo.expGain);
//				battleRewardNotify.content += (string.Format("{0}获得了{1}{2}",petInfo.petDto.name.WrapColor(ColorConstant.Color_Tip_Item),expNotify.expGain.ToString().WrapColor(ColorConstant.Color_Tip_GainCurrency),ItemIconConst.GetIconConstByItemId(H1VirtualItem.VirtualItemEnum_PET_EXP)));
			}
//			else if (notify is WealthNotify)
//			{
//				battleRewardNotify.content += "{0}#w3"+(notify as WealthNotify).copper;
//			}
//			else if (notify is SubWealthNotify)
//			{
//				PlayerModel.Instance.UpdateSubWealth(notify as SubWealthNotify);
//				battleRewardNotify.content += "{0}#w3"+(notify as SubWealthNotify).;
//			}
//			else if (notify is PackItemNotify)
//			{
//				PackItemNotifyListener.HandlePackItemNotify(notify as PackItemNotify);
//			}
		}
//		_SysTalkList.Add(battleRewardNotify);
		if(!string.IsNullOrEmpty(battleRewardNotify.content))
		AddSysNotify(battleRewardNotify);

	}





	//传闻 
	public void AddHearsayNotify(HearsayNotify notify){
		if(_HearsayTalkList.Count >99){
			_HearsayTalkList.RemoveAt(0);
		}
		_HearsayTalkList.Add (notify);

		if(isLock){
			if(ShowUnRead != null){
				ShowUnRead();
			}
		}
		else{
			if(OnNewNotifyUpdate != null){
				OnNewNotifyUpdate();
			}
		}
	}


	public List<ChatNotify> GetTeamTalkList(){
		return _teamTalkList;
	}


	public List<ChatNotify> GetWorldTalkList(){
		return _worldTalkList;
	}

	public List<ChatNotify> GetGuildTalkList(){
		return _guildTalkList;
	}



	public List<SystemNotify> GetSysTalkList(){
		return _SysTalkList;
	}

	public List<HearsayNotify> GetHearsayTalkList(){
		return _HearsayTalkList;
	}


	//聊天等级
	public bool isMoreThenTen(){

		return  PlayerModel.Instance.GetPlayerLevel() >= 10 ? true : false;

	}





	//聊天发送
	public void SendMessage(int channel,string msg,long playerId = 0,bool isSecond= false){
		ServiceRequestAction.requestServer (ChatService.talk(channel,playerId,isSecond,msg),"world msg");
	}


	//世界频道计时
	private float coolDownTime;
	private bool worldCanChat = true;
	public void WorldChatCoolDown(){
		worldCanChat = false;
		float coolTime = 300f- PlayerModel.Instance.GetPlayerLevel()*2 > 120f ? 300f- PlayerModel.Instance.GetPlayerLevel()*2 : 120f;
		CoolDownManager.Instance.SetupCoolDown("WorldChatCoolDownTime",coolTime,delegate(float remainTime) {
			coolDownTime = remainTime;
			if(SetCoolDownData != null && _curChannel ==2){
				SetCoolDownData(remainTime);
			}
			else{
				
			}
		},delegate() {
			worldCanChat = true;
		});
	}

	public bool WorldCanChat(){
		return worldCanChat;
	}

	#region 帮派说话间隔
//	private float guildCoolDownTime;
	private bool guildCanChat = true;
	public bool GuildCanChat(){
		return guildCanChat;
	}
	public void GuildChatCoolDown(){
		guildCanChat =false;
		CoolDownManager.Instance.SetupCoolDown("GuildChatCoolDownTime",3f,null//delegate(float remainTime) {
//			guildCoolDownTime = remainTime;}
			,delegate() {
			guildCanChat = true;
		});
	}
	#endregion

	#region 队伍说话间隔
	//	private float guildCoolDownTime;
	private bool teamCanChat = true;
	public bool TeamCanChat(){
		return teamCanChat;
	}
	public void TeamChatCoolDown(){
		teamCanChat =false;
		CoolDownManager.Instance.SetupCoolDown("TeamChatCoolDownTime",3f,null//delegate(float remainTime) {
//			guildCoolDownTime = remainTime;}
		    ,delegate() {
			teamCanChat = true;
		});
	}
	#endregion


	public float GetCoolDownTime(){
		return coolDownTime;
	}

	//找到相应的notify，加上翻译好的内容
	public bool FillTranslationContent(int type ,ChatNotify notify){

		for(int i =0;i <MainUIChatMsgList.Count;i++){
			List<string> old = match(MainUIChatMsgList[i]);
			List<string > News = match (notify.content);
			if(old.Count>3 && News.Count >3){
				if(old[3].Length > 10 &&  News[3].Length > 10 && old[3] == News[3]){
					string str ="";

					switch(notify.channelId){
					case ChatChannel.ChatChannelEnum_World:
						str =jointMsg(4,notify.fromPlayer.nickname,"#leftV"+notify.content);
						break;
					case ChatChannel.ChatChannelEnum_Guild:
						str =jointMsg(5,notify.fromPlayer.nickname,"#leftV"+notify.content);
						break;
					case ChatChannel.ChatChannelEnum_Team:
						str =jointMsg(6,notify.fromPlayer.nickname,"#leftV"+notify.content);
						break;
					default:
						break;
					}
					MainUIChatMsgList[i]=str;
				}
			}
		}

		if(type == 1){
			for(int i =0;i < _worldTalkList.Count;i++){
				List<string> old = match(_worldTalkList[i].content);
				List<string > News = match (notify.content);
				if(old.Count >3 && News.Count >3){
					if(old[3].Length > 10 &&   News[3].Length > 10 && old[3] == News[3]){
						_worldTalkList[i].content = notify.content;
						return true;
					}
				}
			}
		}

		if(type ==2){
			for(int i =0;i < _guildTalkList.Count;i++){
				List<string> old = match(_guildTalkList[i].content);
				List<string > News = match (notify.content);
				if(old.Count >3 && News.Count >3){
					if(old[3].Length > 10 &&   News[3].Length > 10 && old[3] == News[3]){
						_guildTalkList[i].content = notify.content;
						return true;
					}
				}	
			}
		}

		if(type ==3){
			for(int i =0;i < _teamTalkList.Count;i++){
				List<string> old = match(_teamTalkList[i].content);
				List<string > News = match (notify.content);
				if(old.Count >3 && News.Count >3){
					if(old[3].Length > 10 &&   News[3].Length > 10 && old[3] == News[3]){
						_teamTalkList[i].content = notify.content;
						return true;
					}
				}
				
			}
		}

		return false;
	}

	public List<string> match(string str){

		string pattern = "\\[url=(\\w*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		List<string> list = new List<string>();
		foreach (Match m in Regex.Matches(str, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				list.Add(m.Groups[ i ].ToString() );
			}
		}
		return list;
	}
	

	#region mainUI的显示匹配
	public bool MainUIContentMatch(string msg){
		string pattern = "\\[url=(\\d*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		List<string> tList = new List<string>();
		foreach (Match m in Regex.Matches(msg, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				tList.Add(m.Groups[ i ].ToString() );
			}
		}
		if(tList.Count >0){
			if(tList[2] == "-1"){
				return true;
			}
		}

		return false;

	}

	#endregion

	#region 自动播放的匹配
	public void AutoPlayMatch(string msg){
		string pattern = "\\[url=(\\d*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		List<string> tList = new List<string>();
		foreach (Match m in Regex.Matches(msg, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				tList.Add(m.Groups[ i ].ToString() );
			}
		}
		if(tList.Count >3){
			if(tList[3].Length > 10 && tList[4] == "0"){
				AutoPlayVoiceModel.Instance.AddVoice(tList[3]);
			}
		}
	}
	#endregion

	
	
	
	//组合物品url信息  
	/// <summary>
	/// Gets the URL info.
	/// </summary>
	/// <returns>The URL info.</returns>
	/// <param name="msg">Message.</param>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="uniqueId">Unique identifier.</param>
	/// <param name="type">Type.</param>     -1 语音。
	/// <param name="uid">Uid.</param>
	public string GetUrlInfo(string msg, int itemId,long uniqueId,int type,string uid = "" )     //
	{
		if(type == -1){

			int startIndex = msg.IndexOf("#*#");
			int lastIndex = msg.LastIndexOf("#*#");
			string fileName = msg.Substring(startIndex+3,lastIndex-startIndex-3);  //文件名

			string pattern = "#*#.*#*#(.*)";
			List<string> list = new List<string>();
			foreach (Match m in Regex.Matches(msg, pattern)) 
			{
				for (int i = 0; i < m.Groups.Count; i++) {
					list.Add(m.Groups[i].ToString());
				}
			}

			string translateContent = list[1];    //翻译好的内容

			return string.Format("[url={0},{1},{2},{3}]{4}[/url]" ,PlayerModel.Instance.GetPlayerId(),type,fileName,uid,translateContent);
		}

		int index = msg.IndexOf("%#%");
		if(index != -1){
			int lastIndex= msg.LastIndexOf("%#%");

			//玩家前面说话的内容,玩家id,类型,id,超链接id,物品名字
			return string.Format("[734228]{0}[-][3eff00][url={1},{2},{3},{4}][{5}][/url][-]" ,msg.Substring(0,index),PlayerModel.Instance.GetPlayerId(),type,itemId,uniqueId,msg.Substring(index+3,lastIndex-index-3));
		}
		else{
			return string.Format("[734228]{0}[-]" ,msg);
		}
	}


	#region 屏蔽
	private bool receiveWorld = true;
	private bool receiveGuild = true;
	private bool receiveTeam = true;
	public void WorldChannelShield(bool b){
		receiveWorld = b;
	}
	public bool GetWorldChannelShield(){
		return receiveWorld;
	}
	public void GuildChannelShield(bool b){
		receiveGuild = b;
	}
	public bool GetGuildChannelShield(){
		return receiveGuild;
	}
	public void TeamChannelShield(bool b){
		receiveTeam = b;
	}
	public bool GetTeamChannelShield(){
		return receiveTeam;
	}
	#endregion






	//分析类型   -1.语音  1.物品 2.宠物

	//[url=100169,1,114001,499421856390975488

	public void AnalysisAndDecision(string str,GameObject go){

		string [] content = str.Split(',');

		if(content.Length == 0) return;


		if(content[1] == "-1"){

			VoiceRecognitionManager.Instance.GetVoiceInQiniu(content[2],delegate(AudioClip obj) {
				
				if( obj != null )
				{
					
					VoiceRecognitionManager.Instance.PlayQiniuSoundByClip( obj );
				}
				else
				{
					TipManager.AddTip( "获取不到音频" +  content[2]);
				}
				
			});
		}



		if(content[1] == "1"){           //物品
			//如果有唯一id
			if(content[3].Length > 10){
				long playerId;
				long uniqueId;
				long.TryParse(content[0],out playerId);
				long.TryParse(content[3],out uniqueId);
				ServiceRequestAction.requestServer(FriendService.showPackItem(playerId,uniqueId),"",(e)=>{
					PackItemDto dto = e as PackItemDto;
					ProxyItemTipsModule.Open(e as PackItemDto,go,false,null,2);
				});
			}
			else{
				ProxyItemTipsModule.Open(int.Parse(content[2]),go,2);
			}
		}

		if(content[1] == "2"){            //宠物
			long playerId;
			long uniqueId;
			long.TryParse(content[0],out playerId);
			long.TryParse(content[3],out uniqueId);
			ServiceRequestAction.requestServer(FriendService.showPet(playerId,uniqueId),"",(e)=>{
				PetPropertyInfo info = new PetPropertyInfo(e as PetCharactorDto);
//				info.petDto = e as PetPropertyInfo;
				ProxyPetTipsModule.Open(info);
			});
		}

		if(content[1] == "3"){   //任务
			long playerId;
			int missionId;
			long.TryParse(content[0],out playerId);
			int.TryParse(content[2],out missionId);

			ServiceRequestAction.requestServer(FriendService.showPlayerMission(playerId,missionId),"",(e)=>{
				TipManager.AddTip((e as PlayerMissionDto).mission.id.ToString() +"~~~"+(e as PlayerMissionDto).mission.name);
				ProxyHyperlinkMissionModule.Open(e as PlayerMissionDto);
			});
		}


		if(content[1] == "5"){  //称谓
			long playerId;
			int titleId;
			long.TryParse(content[0],out playerId);
			int.TryParse(content[2],out titleId);
			
			ServiceRequestAction.requestServer(FriendService.showPlayerTitle(playerId,titleId),"",(e)=>{
				Title titleInfo = DataCache.getDtoByCls<Title>((e as PlayerTitleDto).titleId);
				ProxyHyperlinkMissionModule.Open(titleInfo);
			});
		}

		if(content[1] == "6"){        //技能
			long playerId;
			int skillId;
			long.TryParse(content[0],out playerId);
			int.TryParse(content[2],out skillId);
			if(skillId > 100){

				ServiceRequestAction.requestServer(FriendService.showFactionSkill(playerId,skillId),"",(e)=>{
					ProxyHyperlinkMissionModule.Open(e as FactionSkillDto);
				});
			}
			else{
				ServiceRequestAction.requestServer(FriendService.showAssistSkill(playerId,skillId),"",(e)=>{
					ProxyHyperlinkMissionModule.Open(e as AssistSkillDto);
					
				});

			}
			
					
		}

		if(content[1] == "7"){//伙伴
			long playerId;
			long crewId;
			long.TryParse(content[0],out playerId);
			long.TryParse(content[3],out crewId);
			
			ServiceRequestAction.requestServer(FriendService.showCrew(playerId,crewId),"",(e)=>{

				TipManager.AddTip((e as CrewCharactorDto).crewId.ToString() +"~~~"+(e as CrewCharactorDto).crew.name);
//				ProxyHyperlinkMissionModule.Open(e as AssistSkillDto);
			});
		}

	}


	public void SetCoolDownReturnName(string name,float timeLength, System.Action<string> callback){

		CoolDownManager.Instance.SetupCoolDown(name,timeLength,null,delegate {
			callback(name);
		});
	}

	/*
	 * 用字符串&%&来标识一下type和content，即拼合type和content
	 */
	public string PieceTypeAndContent( int type , string content )
	{
		return string.Format("type:{0}&%&{1}" , type ,content  );
	}

}

public class ChatMessageFunctionType
{
	public static int OpenPlayerInfo = 1;
	public static int ShowHeroOrItem = 2;
	public static int OrderSeekHelp  = 3;
	public static int BossSeekHelp	 = 4;
	public static int VoiceTalk		 = 5;
}

