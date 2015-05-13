using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendPartnerItemCellController : MonoBehaviour {


	private FriendPartnerItemCell _view;

	private PetPropertyInfo petInfo;
	private CrewPropertyInfo parnerInfo;
	private System.Action<int,int> _onClick;
	private int type;
	private int index;
	public void InitView(){
		_view = gameObject.GetMissingComponent<FriendPartnerItemCell> ();
		_view.Setup (this.transform);
		
		RegisterEvent ();
	}


	public void SetData(int i,PetPropertyInfo pet,int type,System.Action<int ,int> Callback ,bool inBattle = false){
		index = i;
		petInfo = pet;
		this.type = type;
		_onClick = Callback;


//		_view.IconSprite.spriteName = pet.pet
		_view.NameLbl.text = pet.petDto.name;
		_view.WarSprite.gameObject.SetActive (inBattle);
		_view.LvLbl.text = pet.petDto.level.ToString();
	}


	public void SetData(int i,CrewPropertyInfo parner,int type,System.Action<int,int> Callback,bool inBattle = false){
		index = i;
		this.type = type;
		parnerInfo = parner;
		_onClick = Callback;

		_view.NameLbl.text = parner.crew.name;
		_view.WarSprite.gameObject.SetActive (inBattle);
		_view.LvLbl.text = "0";
	}



	public void OnClickCallBack(){
		if (_onClick != null) {
			_onClick(index,type);	
		}
	}


	#region 选中状态
	private bool _selected;
	public void SetSelected(bool active)
	{
		_selected = active;
		UpdateBtnState();
	}
	
	private void UpdateBtnState(){
		if(_selected)
		{
			_view.Bg.spriteName = "the-no-choice-lines";
		}
		else
		{
			_view.Bg.spriteName = "the-choice-lines";
		}
	}
	#endregion




	public	void RegisterEvent(){
		EventDelegate.Set (_view.FriendPartnerItemCellBtn.onClick, OnClickCallBack);
	}
	
	public void Dispose(){
		
	}

}
