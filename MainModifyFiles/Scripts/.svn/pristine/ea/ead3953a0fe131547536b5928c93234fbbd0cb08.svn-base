using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.friend.dto;

public class FlowerPlayerItemCellController : MonoBehaviour {

	private FlowerPlayerItemCell _view;
	int _index;
	private bool _selected;
	private System.Action<int> _onSelect;

	public void InitView(){
		
		_view = gameObject.GetMissingComponent<FlowerPlayerItemCell> ();
		_view.Setup (this.transform);
		
	}


	public void SetData(int index,FriendDto dto, System.Action<int> selectCallback){
		_index = index;
		_view.NameLbl.text  = dto.shortPlayerDto.nickname;
		_view.RelationShipLbl.text = string.Format("[{0}]{1}友好[-]",ColorConstant.Color_UI_Title_Str,dto.degree);
		_view.LVLbl.text = string.Format("[b]{0}[-]",dto.shortPlayerDto.grade);

		_view.NameLbl.transform.localPosition = new Vector3(-50,14,0);
		_view.RelationShipLbl.fontSize = 18;
		_view.RelationShipLbl.transform.localPosition = new Vector3(-50,-15,0);

		//门派
		switch(dto.shortPlayerDto.factionId){
		case 1:
			//大唐
			_view.FactionIcon.spriteName  = "factionIcon_1";
			break;
		case 2:
			// 化生寺
			_view.FactionIcon.spriteName  = "factionIcon_2";
			break;
		case 3:
			//方寸山
			_view.FactionIcon.spriteName  = "factionIcon_3";
			break;
		case 4:
			//天宫
			_view.FactionIcon.spriteName  = "factionIcon_4";
			break;
		case 5:
			//龙宫
			_view.FactionIcon.spriteName  = "factionIcon_5";
			break;
		case 6:
			//普陀
			_view.FactionIcon.spriteName  = "factionIcon_6";
			break;
		case 7:
			//魔王
			_view.FactionIcon.spriteName  = "factionIcon_7";
			break;
		case 8:
			//狮驼
			_view.FactionIcon.spriteName  = "factionIcon_8";
			break;
		case 9:
			//盘丝
			_view.FactionIcon.spriteName  = "factionIcon_9";
			break;
		default:
			_view.FactionIcon.gameObject.SetActive(false);
			break;
		}
		_onSelect = selectCallback;
		EventDelegate.Set (_view.ClickBtn.onClick, OnClickItem);
	}

	private void OnClickItem(){
		if(_onSelect != null)
			_onSelect(_index);
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

	public void SetSelected(bool active)
	{
		_selected = active;
		UpdateBtnState();
	}
}
