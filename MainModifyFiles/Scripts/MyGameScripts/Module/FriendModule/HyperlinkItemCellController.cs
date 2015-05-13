using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.title.dto;
using com.nucleus.h1.logic.core.modules.title.data;

public class HyperlinkItemCellController : MonoBehaviour {
	
	private HyperlinkItemCell _view;
	private PlayerMissionDto _dto;
	private PlayerTitleDto playerTitleDto;
	private System.Action<int,int > _callBack;
	private int _index;
	private int _type;

	public void InitView(){

		_view = gameObject.GetMissingComponent<HyperlinkItemCell> ();
		_view.Setup (this.transform);
		
		RegisterEvent ();
	}

	public void SetData(int index,PlayerMissionDto dto,int type,System.Action< int ,int> callBack){
		_view.Btn.tweenTarget = null;
		_index = index;
		_dto = dto;
		_type = type;
		_callBack = callBack;
		_view.DescriptionLbl.text = string.Format("任务 - {0}",dto.mission.name); 
	}


	public void SetData(int index,PlayerTitleDto dto,int type,System.Action<int,int> callBack){
		_view.Btn.tweenTarget = null;
		_index = index;
		playerTitleDto = dto;
		_type = type;
		_callBack = callBack;

		Title titleInfo = DataCache.getDtoByCls<Title>(dto.titleId);
		_view.DescriptionLbl.text = string.Format("称谓 - {0}",titleInfo.name); 
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
			_view.Bg.spriteName = "green-little-bone";
		}
		else
		{
			_view.Bg.spriteName = "small-bone-under-lines";
		}
	}
	#endregion


	public void RegisterEvent(){
		EventDelegate.Set(_view.Btn.onClick,ClickCallBack);
	}

	public void ClickCallBack(){
		if(_callBack != null){
			_callBack(_index,_type);
		}
	}
}
