using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.chat.modules.dto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ChatItemCellController : MonoBehaviour,IViewController {

	private ChatItemCellView _view;
	ChatNotify info;
	public void InitView(){

		_view = gameObject.GetMissingComponent<ChatItemCellView> ();
		_view.Setup (this.transform);
		_view.ContentLbl.text = "";
		RegisterEvent();
	}


	//内容，左右，每行文本最大长度，最外层item的长度
	public void SetDate(ChatNotify notify,bool right,int width,int length){
//
		_view.PlayBtn.tweenTarget = null;

		_view.ContentLbl.pivot = UIWidget.Pivot.TopLeft;
		_view.ContentLbl.overflowMethod = UILabel.Overflow.ResizeFreely;		
		if(_view.ContentLbl.width <22){
			_view.ContentBg.rightAnchor.absolute = 35;
		}
		_view.ContentBg.rightAnchor.absolute = 17;
		_view.FaceBgAnchor.side =UIAnchor.Side.Left;
		_view.FaceBgAnchor.pixelOffset.x = 40;
		_view.FaceBgAnchor.Update();
		
		_view.ContentLbl.pivot = UIWidget.Pivot.TopLeft;
		_view.ContentLblAnchor.pixelOffset.x = 20;
		_view.ContentLblAnchor.Update();
		
		_view.ContentBg.flip = UIBasicSprite.Flip.Horizontally;

		_view.LvLbl.text = string.Format("[b]{0}[-]",notify.fromPlayer.grade);

		info = notify;
		List<string> tList = new List<string>();
		tList = ShowVoiceIcon(info.content);

		//帮派职位
		_view.JobLbl.enabled = false;

		//整个最外层item的长度
		_view.widget.width = length;
		//语音
		if(tList.Count >0 && tList[3].Length >10){
			if(tList[1] != PlayerModel.Instance.GetPlayerId().ToString()){
				_view.ContentLbl.text = "#leftV" +info.content;
			}
			else{
				_view.ContentLbl.text = info.content+ "#rightV";
			}
		}

		//文字
		else{
			_view.ContentLbl.text = info.content;
//			Debug.LogError("1:" +_view.ContentLbl.width);
			if(info.content.Contains("#e")){
				str = info.content;
				str1 = info.content;
				hasExpress = true;
				frame= 1;
				matchExpress();
			}

			else{
				_view.ContentLbl.text = info.content;
			}

		}


		RightOrLeft(right,width);



		//显示玩家名字
		if (PlayerModel.Instance.GetPlayerId() != info.fromPlayer.id) {
			_view.PlayerNameLbl.transform.localPosition = new Vector3(-118,24,0);
			_view.PlayerNameLbl.text = info.fromPlayer.nickname;
		}
		else{
			_view.PlayerNameLbl.enabled = false;
		}

	}

	//好友聊天处不需要显示名字
	public void HidePlayerName(){
		_view.PlayerNameLbl.gameObject.SetActive(false);
	}

	public void RightOrLeft(bool right,int width){

		if(_view.ContentLbl.width >= width){			
			_view.ContentLbl.overflowMethod = UILabel.Overflow.ResizeHeight;		
			_view.ContentLbl.width = width;
		}
		if (right) {
			_view.ContentBg.spriteName = "blue-talking-under";
			_view.ContentBg.rightAnchor.absolute = 22;
			_view.FaceBgAnchor.side =UIAnchor.Side.Right;
			_view.FaceBgAnchor.pixelOffset.x = -40;
			_view.FaceBgAnchor.Update();
			
			_view.ContentLbl.pivot = UIWidget.Pivot.TopRight;
			_view.ContentLblAnchor.pixelOffset.x = -100;
			_view.ContentLblAnchor.Update();
			
			_view.ContentBg.flip = UIBasicSprite.Flip.Nothing;
			_view.LvLbl.gameObject.SetActive(false);
		}
		else{
			_view.ContentBg.spriteName = "green-talking-under";
			if(_view.ContentLbl.width <22){
				_view.ContentBg.rightAnchor.absolute = 35;
			}

			_view.ContentBg.rightAnchor.absolute = 17;
			_view.FaceBgAnchor.side =UIAnchor.Side.Left;
			_view.FaceBgAnchor.pixelOffset.x = 40;
			_view.FaceBgAnchor.Update();
			
			_view.ContentLbl.pivot = UIWidget.Pivot.TopLeft;
			_view.ContentLblAnchor.pixelOffset.x = 20;
			_view.ContentLblAnchor.Update();
			
			_view.ContentBg.flip = UIBasicSprite.Flip.Horizontally;
			_view.LvLbl.gameObject.SetActive(true);
		}

		_view.ContentBg.MakePixelPerfect();
	}


	public void RegisterEvent(){

		EventDelegate.Set(_view.FaceIconBtn.onClick,OnFaceIconBtn);
		EventDelegate.Set(_view.PlayBtn.onClick,OnContentClick);
	}

	void HandleOnReceiveVoiceMsg (ChatNotify obj)
	{
		if(obj == info){}
	}

	public void Dispose(){

	}


	public void OnFaceIconBtn(){
		if(info.fromPlayer.id != PlayerModel.Instance.GetPlayerId() ){
			ServiceRequestAction.requestServer (PlayerService.playerInfo(info.fromPlayer.id ),"playerInfo",(e)=>{
				ProxyMainUIModule.OpenPlayerInfoView(e as SimplePlayerDto,new Vector3(-35,-32,0));
			});
		}
	}

	public List<string> ShowVoiceIcon(string str){
		List<string> _tList = new List<string>();
		string pattern = "\\[url=(\\d*),([0-9-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		foreach (Match m in Regex.Matches(str, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				_tList.Add(m.Groups[ i ].ToString());
			}
		}
		return _tList;
	}

	void OnContentClick()
	{
		string pattern = "\\[url=(\\d*),([0-9-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		List<string > tList = new List<string>();
		foreach (Match m in Regex.Matches(_view.ContentLbl.text, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				tList.Add(m.Groups[i].ToString());
//				
			}
		}

		if(tList.Count >3 && tList[3].Length >10){
			//如果满足音频的就
			VoiceRecognitionManager.Instance.GetVoiceInQiniu(tList[3],delegate(AudioClip obj) {
				
				if( obj != null )
				{
					
					VoiceRecognitionManager.Instance.PlayQiniuSoundByClip( obj );
				}
				else
				{
					TipManager.AddTip( "获取不到音频");
				}
				
			});
			return;
		}


	

		string tempStr = _view.ContentLbl.GetUrlAtPosition( UICamera.lastHit.point );
		if( !string.IsNullOrEmpty( tempStr ) )
		{
			ChatModel.Instance.AnalysisAndDecision(tempStr,UICamera.lastHit.transform.gameObject);
		}
	}


	public void ClearExpressionInfo(){
		str = "";
		str1 = "";
		hasExpress = false;
		expressionList.Clear();
		expressionDic.Clear();
		expressionCurPlayIndex.Clear();
	}

	List<string > expressionList= new List<string>();      //需要替换的表情字符串

	Dictionary<string,int> expressionDic = new Dictionary<string, int>();       //表情对应帧数
	
	Dictionary<string,int> expressionCurPlayIndex = new Dictionary<string, int>();  //当前表情帧计时器

	string str;
	string str1;
	void matchExpress () {
		
		expressionDic.Add("#e100",2);
		expressionDic.Add("#e101",3);
		
		string s = "#e";
		string pattern = @"\d{3}";
		int Star = 0;
		while (Star != -1){
			Star = str.IndexOf(s, Star);//获取字符的索引
			if (Star != -1){
				Star++;
				
				if((Star-1+2+3 <= str.Length) && Regex.IsMatch( str.Substring(Star-1+2,3),pattern)){
					if(!expressionList.Contains(str.Substring(Star-1,5))){
						expressionList.Add(str.Substring(Star-1,5));
						expressionCurPlayIndex.Add(str.Substring(Star-1,5),1);
					}

				}
			}
		}
		if(str1.Contains("#e")){
			hasExpress = true;
		}
		else{
			hasExpress = false;
		}
	}

	
	
	
	//每0.3s 改变一次
	float frame = 1;
	bool hasExpress = false;

	void Update () {

		if(hasExpress){
			frame += Time.deltaTime;
			if(frame >0.3){
				change();
				Show();
				frame =0;
			}
		}


	}
	
	//得到当前这个表情的总共帧数
	public int TotalPlayIndex(string key){
		if(expressionDic.ContainsKey(key)){
			return expressionDic[key];
		}
		return -1;
	}
	
	//得到当前表情播放到第几帧了
	public int CurrentPlayIndex(string key){
		if(expressionCurPlayIndex.ContainsKey(key)){
			return expressionCurPlayIndex[key];
		}
		return -1;
	}
	
	//改变显示
	void Show(){
		str1 = str;
		for(int i =0; i < expressionList.Count; i++){
			
			str1 = str1.Replace( expressionList[i] , 
			                    expressionList[i] + "-" + 
			                    ( (CurrentPlayIndex(expressionList[i]) % TotalPlayIndex(expressionList[i])) + 1 ) );
			
		}
		_view.ContentLbl.spacingY = 5;
		_view.ContentLbl.text = str1;
	}
	
	//帧计数
	public void change(){
		for(int i = 0; i < expressionCurPlayIndex.Count; i ++){
			
			//每0.3秒 帧数+1
			expressionCurPlayIndex [ expressionList[i] ] ++;
			
			//如果当前表情计数器大于这个表情总帧数*2000 的话，从一开始。
			if(expressionCurPlayIndex [ expressionList[i] ] > expressionDic[ expressionList[i] ] * 2000){
				expressionCurPlayIndex[ expressionList[i] ] = 1;
			}
		}
	}
}
