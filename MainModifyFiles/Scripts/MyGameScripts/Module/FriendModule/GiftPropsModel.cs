using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.dto;

public class GiftPropsModel : MonoBehaviour {


	private static readonly GiftPropsModel instance = new GiftPropsModel();
	public static GiftPropsModel Instance
	{
		get
		{
			return instance;
		}
	}

	SimplePlayerDto _simplePlayerDto;
	private List<GiftItemCellController> _itemCellControllerList = new List<GiftItemCellController>();
	

	#region 获取可赠送的道具
	public List<PackItemDto> GetGiftPropsList(){
		return BackpackModel.Instance.GetGiftPropsList ();
	}
	#endregion


	public void SetPlayer(SimplePlayerDto s){
		_simplePlayerDto = s;
	}

	public SimplePlayerDto GetSimplePlayerDto(){
		return _simplePlayerDto;
	}


	public void SetGiftItemCellControllerList(List<GiftItemCellController> itemCellControllerList){
		_itemCellControllerList = itemCellControllerList;
	}
	

	public List<GiftItemCellController> GetGiftItemCellControllerList(){
		return _itemCellControllerList;
	}

}
