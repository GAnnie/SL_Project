using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.mail.dto;

public class GiftItemCellController : MonoBehaviour {

	private GiftItemCell _view;
	private int _index;
	private string _curName;
	private System.Action<int> _onClick;
	private System.Action<int> _onReduceClick;

	int _curCount = 0;
	int _curValue = 0;

	PackItemDto _packItemDto;
	bool _hasValue = false;
	int _type =0;  //0表示赠送， 其他表示鲜花

	List<GiftItemCellController> itemCellControllerList = new List<GiftItemCellController> ();

	public void InitItem(int i,PackItemDto packItemDto,System.Action<int> ClickCallback,System.Action<int> ClickReduceCallback,int type = 0){

		_view = gameObject.GetMissingComponent<GiftItemCell> ();
		_view.Setup (this.transform);
		_type = type;
		_index = i;
		_curName = packItemDto.item.name;
		gameObject.name = "ItemCell_"+packItemDto.index;

		_view.ReduceBtn.gameObject.SetActive (false);
		_view.SelectSpriteObj.SetActive(false);
		SelectEff (false);

		_packItemDto = packItemDto;
		if ((_packItemDto.item as Props).giftPrice != 0) {
			_hasValue = true;		
		}
		if(_type ==0){
			_view.CountLabel.text = string.Format("{0}/{1}",_curCount,_packItemDto.count);
		}
		else{
			_view.CountLabel.gameObject.SetActive(false);
		}


		if(_view.IconSprite.atlas.GetSprite((packItemDto.item as Props).icon) != null)
			_view.IconSprite.spriteName =(packItemDto.item as Props).icon;
		else
			_view.IconSprite.spriteName = "0";

		_onClick = ClickCallback;
		_onReduceClick = ClickReduceCallback;
		_view.DragDropItemObj.SetActive (true);
		EventDelegate.Set (_view.ReduceBtn.onClick,OnReduceBtnClick);
		EventDelegate.Set (_view.GiftItemCellBtn.onClick, OnGiftItemCellBtnClick);

	}

	public void SetItemCellControllerList(List<GiftItemCellController> _itemCellControllerList){
		itemCellControllerList = _itemCellControllerList;
	}



	public void SelectEff(bool Show = false){
		_view.SelectSpriteObj.SetActive (Show);
	}

	public void SetIcon(string iconId){

		if(_view.IconSprite.atlas.GetSprite(iconId) != null)
			_view.IconSprite.spriteName =iconId;
		else
			_view.IconSprite.spriteName = "0";
	}



	//点击--
	public void OnReduceBtnClick(){
		_curCount--;

		//如果是普通的
		if ((_packItemDto.item as Props).giftPrice == 0) {

			//数量小于1
			if (_curCount < 1) {
				_view.ReduceBtn.gameObject.SetActive (false);
				_view.SelectSpriteObj.SetActive(false);
			}	

		}

		//元宝
		else{
			_curValue -= (_packItemDto.item as Props).giftPrice;


			if(_curValue == 0 ){
				_view.ReduceBtn.gameObject.SetActive (false);
			}


		}


		if(_type ==0)
		_view.CountLabel.text = string.Format("{0}/{1}",_curCount,_packItemDto.count);
		if (_onReduceClick != null) {
			_onReduceClick(_index);	
		}
	}

	//点击++
	public void OnGiftItemCellBtnClick(){
		_curCount++;

//		itemCellControllerList =

		if (_curCount > _packItemDto.count) {
			_curCount = _packItemDto.count;		
		}

		//普通

		if ((_packItemDto.item as Props).giftPrice == 0) {
			int _c = 0;
			for(int index =0 ; index <itemCellControllerList.Count;index++){
				if(itemCellControllerList[index]  == this){
					continue;
				}
				if(itemCellControllerList[index]._hasValue !=true)
				_c += itemCellControllerList[index].GetNum();
			}
			
			
			int max = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_COUNT, 10);
			 
			GiftDto giftDto =  EmailModel.Instance.GetCount_Value ();
			
			_c = _c > max - giftDto.giftCount ? max - giftDto.giftCount : _c;

			if(_type ==0){
				if ((_curCount + _c) > (max - giftDto.giftCount)) {
					_curCount --;
					TipManager.AddTip("今日赠送物品已达上限");
					return;
				}
			}
			else{

			}

		}

		// 元宝
		else{


			_curValue = (_packItemDto.item as Props).buyPrice * _curCount;

			int max = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_PRICE, 1000);

			GiftDto giftDto = EmailModel.Instance.GetCount_Value ();

			int _v = 0;

			for(int index =0 ; index <itemCellControllerList.Count;index++){
				if(itemCellControllerList[index]  == this){
					continue;
				}
				_v += itemCellControllerList[index].GetValue();
			}
			if(_type == 0){
				if((_curValue + _v) > (max - giftDto.giftPrice)){
					_curValue = (_packItemDto.item as Props).buyPrice * (_curCount -1);
					_curCount--;
					TipManager.AddTip("今日赠送物品已达上限");
					return;
				}
			}


		}

		_view.ReduceBtn.gameObject.SetActive (true);
		if(_type == 1){
			_view.SelectSpriteObj.SetActive(true);
		}

		if(_type == 0)
		_view.CountLabel.text = string.Format("{0}/{1}",_curCount,_packItemDto.count);

		if (_onClick != null) {
			_onClick(_index);	
		}
	}

	public void SetIcon(){
		if(_view.IconSprite.atlas.GetSprite(_packItemDto.item.icon) != null)
			_view.IconSprite.spriteName = _packItemDto.item.icon;
		else
			_view.IconSprite.spriteName = "0";
	}
	
	public int GetNum(){
		return _curCount;
	}
	public string GetName(){
		return _curName;
	}

	public int GetIndex(){
		return _packItemDto.index;
	}

	public int GetValue(){
		return _curValue;
	}

	public void ResetEvent(){
//		_view.ReduceBtn.onClick -= OnReduceBtnClick;
//		_view.GiftItemCellBtn.onClick -= OnGiftItemCellBtnClick;
	}

	public bool HasValue(){
		return _hasValue;
	}
}
