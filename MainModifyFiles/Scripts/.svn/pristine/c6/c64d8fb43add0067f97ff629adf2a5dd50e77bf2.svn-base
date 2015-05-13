using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.mail.dto;

public class GiftPropsViewController : MonoBehaviour,IViewController {

	private GiftPropsView _view;

	private const string GiftItemCellPath = "Prefabs/Module/GiftModule/GiftItemCell";
	private const string GiftItemGridPath = "Prefabs/Module/GiftModule/itemGrid";

	private const string ItemContainerName = "Prefabs/Module/GiftModule/GiftItemContainer";

	private List<PackItemDto> _giftPropsList;

	private List<GiftItemCellController> _itemCellList;

	private SimplePlayerDto _simplePlayerDto;
	private int _p = 0;

	public void InitView(){

		_view = gameObject.GetMissingComponent<GiftPropsView> ();
		_view.Setup (this.transform);

		//玩家名字
		_simplePlayerDto =GiftPropsModel.Instance.GetSimplePlayerDto ();
//		_view.NameLbl.gameObject.SetActive (false);

		//可赠送物品列表
		_giftPropsList = new List<PackItemDto> ();
		_itemCellList = new List<GiftItemCellController> ();

		RegisterEvent ();

		_giftPropsList = GiftPropsModel.Instance.GetGiftPropsList ();

		int count = _giftPropsList.Count;

		if (count == 0) {
			_p = 1;		
		}
		else{
			while (count >0) {
				count -=12;
				_p++;
			}
		}




		//根据物品 显示具体页数 <12 => 1     >12 && <=24 =>2
		AddItem(_p);	


		//*********************************** 显示左边面板信息
		int maxCount = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_COUNT, 10);
		int maxValue = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_PRICE, 1000);
		GiftDto giftDto = EmailModel.Instance.GetCount_Value ();
		
		
		_view.CountLbl.text = string.Format("{0}/{1}",0,maxCount-giftDto.giftCount);
		_view.ValuesLbl.text = string.Format ("{0}/{1}", 0, maxValue - giftDto.giftPrice);

		//************************************************************

		ResetEvent (_giftPropsList);

		//按钮变为玩家名字
		_view.GiveBtnLbl.text = string.Format ("[ffffff]{0}[-]   [5cf37c]{1}[-]","赠送",_simplePlayerDto.nickname);

	}

	private void AddItem(int page)
	{
		for (int i =0; i <(12*page); i++) {
			GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( GiftItemCellPath ) as GameObject;
			GameObject module = GameObjectExt.AddChild(_view.itemGrid.gameObject,prefab);
			GiftItemCellController com = module.GetMissingComponent<GiftItemCellController>();
			_itemCellList.Add(com);
		}
	}

	public void ResetEvent(List<PackItemDto> giftPropsList){
		for (int i = 0; i <_giftPropsList.Count; i++) {
			_itemCellList[i].InitItem(i,_giftPropsList[i],OnClick,OnReduceBtnClick);
			_itemCellList[i].SetItemCellControllerList(_itemCellList);
		}
		//GiftPropsModel.Instance.SetGiftItemCellControllerList (_itemCellList);
	}
	

	public void RegisterEvent(){
		EventDelegate.Set (_view.CloseBtn.onClick, OnCloseBtnClick);
		EventDelegate.Set (_view.GiftBtn.onClick, OnGiveBtnClick);
		EventDelegate.Set (_view.Tips.onClick, OnTipsBtnClick);
	}


	public void OnGiveBtnClick(){
		string str = "";
		int num = 0;
		string msg = "";
		for (int i =0; i<_itemCellList.Count; i++) {

			if( _itemCellList[i].GetNum() != 0){
				str += _itemCellList[i].GetIndex()+":"+_itemCellList[i].GetNum()+",";
				num += _itemCellList[i].GetNum();
				msg +=  _itemCellList[i].GetName() + "x" + _itemCellList[i].GetNum()+",";
			}
		}
		if (num == 0) {
			TipManager.AddTip("请选择要赠送的物品");	
			return;
		}
		msg = msg.Substring (0, msg.Length - 1);




		ServiceRequestAction.requestServer (MailService.gift ((long)_simplePlayerDto.id, str), "",delegate(com.nucleus.commons.message.GeneralResponse e) {
			success();
			EmailModel.Instance.SetCount_Value(e as GiftDto);
			ProxyGiftPropsModule.Close();
			TipManager.AddTip(string.Format("你向[5cf37c]{0}[-]赠送了[ffdd7b]{1}[-]",_simplePlayerDto.nickname,msg));
	},delegate(com.nucleus.commons.message.ErrorResponse errorResponse) {
			fail(errorResponse.ToString());
});
	}

	public void success(){

	}

	public void fail(string str){

	}

	public void Dispose(){
		
	}
	
	public void OnCloseBtnClick(){
		ProxyGiftPropsModule.Close ();
	}



	int last = -1;
	public void OnClick(int i){
		int count = 0;
		int value = 0;

		_itemCellList [i].SelectEff (true);
		if (_itemCellList [i].GetNum () == 0 && _itemCellList [i].GetValue () == 0) {
			_itemCellList [i].SelectEff(false);		
		}



		_view.NameLbl.gameObject.SetActive (true);
		_view.NameLbl.text =string.Format("[ffd84c]{0}[-]",_giftPropsList [i].item.name); 

		Props p = _giftPropsList [i].item as Props;

		_view.DescriptionLbl.text = string.Format ("{0}", p.introduction); 
		for(int index =0 ; index <_giftPropsList.Count;index++){
			if((_giftPropsList[index].item as Props).giftPrice ==0 ){
				count += _itemCellList[index].GetNum();
			}
			
			value += _itemCellList[index].GetValue();
		}


		int maxCount = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_COUNT, 10);
		int maxValue = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_PRICE, 1000);

		GiftDto giftDto = EmailModel.Instance.GetCount_Value ();

		count = count > maxCount - giftDto.giftCount ? maxCount - giftDto.giftCount : count;
		value = value > maxValue - giftDto.giftPrice ? maxValue - giftDto.giftPrice : value;

		_view.CountLbl.text = string.Format("{0}/{1}",count,maxCount-giftDto.giftCount);
		_view.ValuesLbl.text = string.Format ("{0}/{1}", value, maxValue-giftDto.giftPrice);

	}
	public void OnReduceBtnClick(int i){
		int count = 0;
		int value = 0;

		if (_itemCellList [i].GetNum () == 0 && _itemCellList [i].GetValue () == 0) {
			_itemCellList [i].SelectEff(false);		
		}

		for(int index =0 ; index <_giftPropsList.Count;index++){
			if((_giftPropsList[index].item as Props).giftPrice ==0 ){
				count += _itemCellList[index].GetNum();
			}
			
			value += _itemCellList[index].GetValue();
		}

		
		
		int maxCount = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_COUNT, 10);
		int maxValue = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIL_DAILY_GIFT_PRICE, 1000);
		GiftDto giftDto = EmailModel.Instance.GetCount_Value ();


		//********************************
		if ((_giftPropsList[i].item as Props).giftPrice == 0) {
			_view.CountLbl.text = string.Format("{0}/{1}",count,maxCount-giftDto.giftCount);	
		}


		_view.ValuesLbl.text = string.Format ("{0}/{1}", value, maxValue - giftDto.giftPrice);

	}

	public void OnTipsBtnClick(){
		TipManager.AddTip ("每天只能赠送价值不超过1000元宝的元宝商城道具");
	}


}
