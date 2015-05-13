using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingTipManager{

	public class TipMessage{
		public int level;
		public int endLevel;
		public string tips;
		public int isLoadingTip;
	}

	public class TipList{
		public List<TipMessage> message;
	}

	private static bool _isInit = false;
	private static List<TipMessage> _tipList; 		 
	private static List<TipMessage> _loadingTipList; //只显示在loading界面

	public static void Setup(){
		if(_isInit) return;

		var tipData = Resources.Load(PathHelper.SETTING_PATH+"LoadingTipsData") as TextAsset;
		_tipList = LITJson.JsonMapper.ToObject<TipList>(tipData.text).message;
		_loadingTipList = new List<TipMessage>();
		for(int i=0;i<_tipList.Count;++i){
			if(_tipList[i].isLoadingTip == 1)
				_loadingTipList.Add(_tipList[i]);
		}
	}

	public static string GetLoadingTip(){
		var tipMsg = _loadingTipList.Random();
		return tipMsg!=null?tipMsg.tips:"";
	}

	public static string GetSystemChannelTip(int playerLv){
		List<TipMessage> tmpList = new List<TipMessage>();
		for(int i=0;i<_tipList.Count;++i){
			if(playerLv >= _tipList[i].level && playerLv <= _tipList[i].endLevel)
				tmpList.Add(_tipList[i]);
		}

		var tipMsg = tmpList.Random();
		return tipMsg!=null?tipMsg.tips:"";
	}
}
