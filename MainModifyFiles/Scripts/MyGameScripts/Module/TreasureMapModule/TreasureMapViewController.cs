﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.player.model;
using com.nucleus.h1.logic.core.modules.reward.data;


public class TreasureMapViewController : MonoBehaviour,IViewController {
	private const string TreasureItemCellPath = "Prefabs/Module/TreasureMapModule/TreasureItemCell";


	private TreasureMapView _view;

	List<TreasureItemCellController> treasureItemCellControllerList = new List<TreasureItemCellController>();


	List<int> TresureMapInfoList  =  new List<int>();

	private int _index;
	private int _move;
	private bool _isStop;
	private int _stopMove;
	private float _stopTime;

	private int _ItemRandomIndex;

	List < Reward >  _infoList ;

	public void InitView( ){
		_view = gameObject.GetMissingComponent<TreasureMapView> ();
		_view.Setup (this.transform);

//		Init();

		RegisterEvent();
	}

	int _useredId;
	int rewardItemId ;
	int rewardEventId;
	public void SetData(TreasureEventNotify notify ,int userId){
		_useredId = userId;
//		Debug.LogError(string.Format("<color=orange> ## {0} ## </color>", notify.fallItemId));
		rewardItemId = notify.fallItemId;
		rewardEventId = notify.eventId;
		Init();

	}




	public class Reward {

		public int eventId;
		public int thingId;
		public int eventRate;
	}



	public void Init(){


		//要生成20个
		List <Reward> rewardList = new List < Reward >(20);
		//得到当前使用宝图的数据表
		PropsTreasureEvent treasureInfo = DataCache.getDtoByCls<PropsTreasureEvent>(_useredId);


		//随机19个
		for(int i = 0; i < 19 ; i++){

			Reward info = new Reward();
			//随机一个概率
			int rate = Random.Range(0,10000);
			int section = 0;

			//当前概率落在哪个事件区间
			for(int index = 0; index < treasureInfo.eventRate.Count; index++ ){

				section +=  treasureInfo.eventRate[index];
				if( rate < section ){

					info.eventId  = treasureInfo.eventId[index];
					switch(info.eventId){

					case 1:
						//这里读取掉落表
						FallItem fallItemInfo =  DataCache.getDtoByCls<FallItem>(treasureInfo.thingId[index]);
						
						//随机一个概率值
						int itemRate = Random.Range(0,10000);
						int sectionRate = 0;
						
						//看落在哪个区间
						for(int j = 0 ; j < fallItemInfo.itemRate.Count; j++){
							sectionRate += fallItemInfo.itemRate[j];
							if(itemRate < sectionRate){
								info.thingId = fallItemInfo.itemId[j];
								rewardList.Add(info);
								break;
							}
						}

						break;
					case 2:
						rewardList.Add(info);
						break;
					case 3:
						rewardList.Add(info);
						break;
					}
					break;
				}
			}
		}


		//这是中奖的----------------------------------------------------
		Reward reward = new Reward();
		reward.eventId =rewardEventId;
		reward.thingId = rewardItemId;
		int pos= Random.Range(0,18);
		rewardList.Insert(pos,reward); 
		_ItemRandomIndex = pos;

		//-------------------------------------------------------------

		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(TreasureItemCellPath) as GameObject;
		
		for(int i = 0; i < rewardList.Count ; i++){

			GameObject item = NGUITools.AddChild(_view.ItemGrid.gameObject,itemPrefab);
			TreasureItemCellController com = item.GetMissingComponent<TreasureItemCellController>();
			com.InitView();
			com.SetData(rewardList[i].eventId,rewardList[i].thingId);
			treasureItemCellControllerList.Add(com);

	}






		_index = 0;
		_isStop = false;
		_move = 100;
		treasureItemCellControllerList[_index].SetSelected(true);
//		_ItemRandomIndex = Random.Range(0,19);
		NormalRun();
	}
	
	
	public void RegisterEvent(){

		EventDelegate.Set(_view.OkBtn.onClick,OnOKBtnClick);
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtnClick);
	}

	public void OnOKBtnClick(){
		OnOK();
	}

	public void OnCloseBtnClick(){
//		ProxyTreasureMapModule.Close();
		OnClose();
	}
	public void Dispose(){}



	private void NormalRun()
	{
		InvokeRepeating("NormalRunHandle",0.1f,0.1f);
	}
	
	private void NormalRunHandle()
	{
		treasureItemCellControllerList[_index].SetSelected(false);
		_index = (_index + 1)%treasureItemCellControllerList.Count;
		treasureItemCellControllerList[_index].SetSelected(true);
		
		_move --;
		
		if(_isStop || _move <= 0)
		{
			// 重新计算移动格子
			_move = 0;
			
			if(_ItemRandomIndex - _index > 0)
			{
				if(_ItemRandomIndex - _index >= 12)
				{
					_move = _ItemRandomIndex - _index;
				}
				else
				{
					_move = _ItemRandomIndex - _index + 20;
				}
			}
			else
			{
				if(_ItemRandomIndex + 20 - _index >= 12)
				{
					_move = _ItemRandomIndex + 20 - _index;
				}
				else
				{
					_move = _ItemRandomIndex + 20 - _index + 20;
				}
			}
			_stopMove = _move;
			OnOK();
			StopRun();
		}
	}
	
	private void StopRun()
	{
		CancelInvoke();
		
		_stopTime = 0.1f + 0.2f/(_stopMove - 5);
		Invoke("StopRunHandle",_stopTime);
	}
	
	private void StopRunHandle()
	{
		treasureItemCellControllerList[_index].SetSelected(false);
		_index = (_index + 1)%treasureItemCellControllerList.Count;
		treasureItemCellControllerList[_index].SetSelected(true);
		
		_move --;
		
		if(_move > 0)
		{
			if(_move > 5)
			{
				CancelInvoke();
				_stopTime = _stopTime + 0.2f/(_stopMove - 5);
				Invoke("StopRunHandle",_stopTime);
			}
			else
			{
				CancelInvoke();
				_stopTime = _stopTime + 0.15f;
				Invoke("StopRunHandle",_stopTime);
			}
		}
		else
		{
			CancelInvoke();
			Invoke("close", 2f);
		}
	}

	public void OnOK()
	{
		_view.OkBtn.isEnabled = false;
		_isStop = true;
	}
	
	public void OnClose()
	{
		close();
	}
	private void close()
	{
//		if(_isStop)
//		{
//			CancelInvoke();
//			OnCloseHandle();
//			
			ProxyTreasureMapModule.Close();
//		}
//		else
//		{
//			TipManager.AddTip("先确认才能关闭界面");
//		}
	}

	private void OnCloseHandle(){
//		TipManager.AddTip("停在了" + _ItemRandomIndex.ToString());
		ProxyTreasureMapModule.Close();

		TipManager.AddTip(ProxyTreasureMapModule.tipsMsg);

		//在关闭抽奖界面后检测背包是否还有藏宝图
		BackpackModel.Instance.HaveTreasureMap();
	}

}
