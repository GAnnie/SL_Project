﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ChannelChatItemCellController : MonoBehaviour {

	private ChannelChatItemCell _view;
	
	public void InitView(){
		
		_view = gameObject.GetMissingComponent<ChannelChatItemCell> ();
		_view.Setup (this.transform);
		
	}


	public void SetData(string channelName,string data,int type,bool isChatPanel = true){
		_view.contentLbl.overflowMethod = UILabel.Overflow.ResizeFreely;
		ClearExpressionInfo();

		_view.contentLbl.spacingY = 8;
		switch (type) {
		case 1:
			_view.contentLbl.text = string.Format ("[e61a1a]{0}[-] {1}", channelName, data);
			break;
		case 2:
			_view.contentLbl.text = string.Format ("[fff9e3]{0}[-] {1}", channelName, data);
			break;
		case 3:
			_view.contentLbl.text = string.Format ("[0c99c7]{0}[-] {1}", channelName, data);
			break;
		}
		
		if (_view.contentLbl.width > 384) {
			_view.contentLbl.overflowMethod = UILabel.Overflow.ResizeHeight;
			_view.contentLbl.width = 384;
		}

		if(_view.contentLbl.text.Contains("#e")){
			str = _view.contentLbl.text;
			str1 = _view.contentLbl.text;
//			hasExpress = true;
//			frame= 1;
			matchExpress();
		}

	
	}

	public void SetData(string data){

		//[0c99c7]帮派[-]  [fff9e3][梁丘代萌][-] [url=100026,-1,7dc51701-e87d-4a67-bf49-a8cfe12e0958,100026-635652355669335620][/url]
		//[0c99c7]帮派[-]  [fff9e3][梁丘代萌][-] #leftV[url=100026,-1,7dc51701-e87d-4a67-bf49-a8cfe12e0958,100026-635652355669335620]语音转文字失败[/url]
//		List<string> tList = new List<string>();
//		tList = ShowVoiceIcon(data);
//		if(tList.Count >0 && tList[3].Length >10){
//			_view.contentLbl.text = " #rightV"+data; 
//		}
//		else{
			_view.contentLbl.text = data;
//		}
		ClearExpressionInfo();
		_view.contentLbl.spacingY = 8;
		_view.ItemCellWidget.width = 350;
		_view.ChannelBg.rightAnchor.absolute = -302;
	
		if(_view.contentLbl.width > 350){
			_view.contentLbl.overflowMethod = UILabel.Overflow.ResizeHeight;
			_view.contentLbl.width = 350;
		}

		
		if(_view.contentLbl.text.Contains("#e")){
			str = _view.contentLbl.text;
			str1 =_view.contentLbl.text;
			matchExpress();
		}

		_view.ChannelBg.MakePixelPerfect();
	}
	
	public List<string> ShowVoiceIcon(string str){
		List<string> _tList = new List<string>();
		
		//[url=100026,-1,84fca2b3-0dbd-4516-b6bf-3fad409128f9,100026-635650540498625360]语音转文字失[/url]
		string pattern = "\\[url=(\\d*),([0-9-]*),([0-9a-zA-Z-]*),([0-9a-zA-Z-]*)\\].*\\[/url\\]";
		foreach (Match m in Regex.Matches(str, pattern)) 
		{
			for (int i = 0; i < m.Groups.Count; i++) {
				_tList.Add(m.Groups[ i ].ToString());
			}
		}
		return _tList;
	}


	void OnClick()
	{
		
//		string tempStr = _view.contentLbl.GetUrlAtPosition( UICamera.lastHit.point );
//		if( !string.IsNullOrEmpty( tempStr ) )
//		{
////			TipManager.AddTip(tempStr);
//			
//			ProxyItemTipsModule.Open(int.Parse(tempStr),UICamera.lastHit.transform.gameObject);
//		}
//		else
//			ProxyChat1Module.Open();

		string tempStr = _view.contentLbl.GetUrlAtPosition( UICamera.lastHit.point );
		if( !string.IsNullOrEmpty( tempStr ) )
		{
			ChatModel.Instance.AnalysisAndDecision(tempStr,UICamera.lastHit.transform.gameObject);
		}
		
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


	public void ClearExpressionInfo(){
		str = "";
		str1 = "";
		hasExpress = false;
		expressionList.Clear();
		expressionDic.Clear();
		expressionCurPlayIndex.Clear();
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
		_view.contentLbl.text = str1;
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
