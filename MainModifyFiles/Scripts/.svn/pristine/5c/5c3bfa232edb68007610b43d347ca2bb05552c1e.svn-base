using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.friend.dto;
using com.nucleus.h1.logic.services;

public class FlowerPropsViewController : MonoBehaviour,IViewController {


	private FlowerPropsView _view;
	
	private const string GiftItemCellPath = "Prefabs/Module/GiftModule/GiftItemCell";
	private const string GiftItemGridPath = "Prefabs/Module/GiftModule/itemGrid";
	
	private const string ItemContainerName = "Prefabs/Module/GiftModule/GiftItemContainer";
	private const string PlayerInfoItemPrefabPath = "Prefabs/Module/GiftModule/FlowerPlayerItemCell";
	private List<PackItemDto> _giftPropsList;
	
	private List<GiftItemCellController> _itemCellList;

	private List<FlowerPlayerItemCellController> playerInfoItemCellControllerList = new List<FlowerPlayerItemCellController>();
	private List<FriendDto> friends;

	private int _p = 0;
	
	public void InitView(){
	
		_view = gameObject.GetMissingComponent<FlowerPropsView> ();
		_view.Setup (this.transform);

		//好友
		friends = FriendModel.Instance.GetDegreeMoreThanOneFriendList();
		friends.Sort((a,b) => b.degree.CompareTo(a.degree));



		//可赠送鲜花列表
		_giftPropsList = new List<PackItemDto> ();
		_itemCellList = new List<GiftItemCellController> ();
		
		RegisterEvent ();
		
		_giftPropsList = BackpackModel.Instance.GetFlowersList ();
		
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
		
		
		
		AddFriendItem();
		//根据物品 显示具体页数 <12 => 1     >12 && <=24 =>2
		AddItem(_p);	
		ResetEvent(_giftPropsList);
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
			_itemCellList[i].InitItem(i,_giftPropsList[i],OnClick,OnReduceBtnClick,1);
			_itemCellList[i].SetIcon();
			_itemCellList[i].SetItemCellControllerList(_itemCellList);

		}
	}

	int last = -1;
	public void OnClick(int i){
		int count = 0;
		int value = 0;
		
		_itemCellList [i].SelectEff (true);
		if (_itemCellList [i].GetNum () == 0 && _itemCellList [i].GetValue () == 0) {
			_itemCellList [i].SelectEff(false);		
		}
	}
	public void OnReduceBtnClick(int i){
		int count = 0;
		int value = 0;
		
		if (_itemCellList [i].GetNum () == 0 && _itemCellList [i].GetValue () == 0) {
			_itemCellList [i].SelectEff(false);		
		}
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
		if(lastPlayInfoBtn == -1){
			TipManager.AddTip("请选择好友");
			return;
		}
		msg = msg.Substring (0, msg.Length - 1);
		
		
//		TipManager.AddTip(msg);
//		
		ServiceRequestAction.requestServer (FriendService.giftFlowers (friends[lastPlayInfoBtn].shortPlayerDto.id, str), "",delegate(com.nucleus.commons.message.GeneralResponse e) {
			success();
			ProxyFlowerPropsModule.Close();
		},delegate(com.nucleus.commons.message.ErrorResponse errorResponse) {
			ProxyFlowerPropsModule.Close();
			fail(errorResponse.message);
		});
	}
	
	public void success(){
		
	}
	
	public void fail(string str){
		TipManager.AddTip(str);
	}
	
	public void Dispose(){
		
	}






	public void AddFriendItem(){


		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(PlayerInfoItemPrefabPath) as GameObject;
		
		for(int i = 0; i <friends.Count;i++){
			GameObject item = NGUITools.AddChild(_view.FriendItemGrid.gameObject,itemPrefab);
			FlowerPlayerItemCellController com = item.GetMissingComponent<FlowerPlayerItemCellController>();
			com.InitView();
			com.SetData(i,friends[i],ClickFriendItemCallBack);
			playerInfoItemCellControllerList.Add(com);
		}

	}

	int lastPlayInfoBtn = -1;
	public void ClickFriendItemCallBack(int i){
//		TipManager.AddTip(friends[i].shortPlayerDto.nickname);

		playerInfoItemCellControllerList[i].SetSelected(true);

		if (lastPlayInfoBtn == -1) {
			lastPlayInfoBtn = i;
		}
		else{
			if(lastPlayInfoBtn != i){
				playerInfoItemCellControllerList[lastPlayInfoBtn].SetSelected(false);
				lastPlayInfoBtn = i;
			}
		}

	}

	public void RegisterEvent(){
	
		EventDelegate.Set(_view.GiftBtn.onClick,OnGiveBtnClick);
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtnClick);
	}

	public void OnCloseBtnClick(){
		ProxyFlowerPropsModule.Close();
	}

	

}
