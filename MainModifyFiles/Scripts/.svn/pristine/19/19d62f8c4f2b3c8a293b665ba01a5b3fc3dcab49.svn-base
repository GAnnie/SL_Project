using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.faction.data;

public class AddFriendItemCellController : MonoBehaviour {

	private AddFriendItemCell _view;

	long playerId;

	public void InitView(){
		_view = gameObject.GetMissingComponent<AddFriendItemCell> ();
		_view.Setup (this.transform);



		RegisterEvent();
	}



	public void SetData(ShortPlayerDto dto){
		isClick = true;
		playerId = dto.id;
		_view.addBtnSprite.spriteName= "add-friend-little-icon";
		_view.NameLbl.text = dto.nickname;
		_view.LvLbl.text = string.Format("{0}级   {1}",dto.grade,DataCache.getDtoByCls<Faction>(dto.factionId).name);

		if(_view.icon.atlas.GetSprite(dto.charactorId.ToString()) != null){
			_view.icon.spriteName = string.Format("{0}",dto.charactorId);
		}
		else{
			_view.icon.spriteName = string.Format("{0}","player_default");
		}


	}


	public void RegisterEvent(){
		EventDelegate.Set(_view.addBtn.onClick,OnAddBtnClick);
	}

	bool isClick = true;
	public void OnAddBtnClick(){

		if(isClick){
			isClick =false;
			_view.addBtnSprite.spriteName = "tick";
			FriendModel.Instance.AddFriend(playerId);
		}
	}

	public void IsFriend(){
		isClick =false;
		_view.addBtnSprite.spriteName = "tick";
	}
}


