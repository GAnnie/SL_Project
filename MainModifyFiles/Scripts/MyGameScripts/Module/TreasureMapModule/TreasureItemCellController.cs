﻿using UnityEngine;
using System.Collections;
using com.nucleus.player.data;

public class TreasureItemCellController : MonoBehaviour {
	private TreasureItemCell _view;

	public void InitView(){
		_view = gameObject.GetMissingComponent<TreasureItemCell> ();
		_view.Setup (this.transform);

		RegisterEvent();
	}

	public void RegisterEvent(){

		EventDelegate.Set(_view.IconBtn.onClick,OnIconBtnClick);
	}


	private int itemId;
	public void SetData(int enentID,int thingId){
		itemId = thingId;
		switch(enentID){
		case 1:

			if(_view.Icon.atlas.GetSprite(thingId.ToString()) != null){
				_view.Icon.spriteName =thingId.ToString();
			}
			else{
				_view.Icon.spriteName= "0";
			}

//			_view.DescriptionLbl.text = DataCache.getDtoByCls<GeneralItem>(thingId).description;

			_view.DescriptionLbl.text =string.Format("{0}X{1}",DataCache.getDtoByCls<GeneralItem>(thingId).name,1);

			break;
		case 2:
			_view.Icon.spriteName = "2";

			_view.DescriptionLbl.text = "放妖";


			break;
		case 3:
			_view.Icon.spriteName = "0";
			
			_view.DescriptionLbl.text = "放宝宝";

			break;
		}
	
	}

	public void OnIconBtnClick(){
//		ProxyItemTipsModule.Open (con.RewardItemId, con.gameObject,1);
		ProxyItemTipsModule.Open (itemId,this.gameObject);

	}

	public void SetSelected(bool b){
		_view.SelectEff.gameObject.SetActive(b);
	}
}
