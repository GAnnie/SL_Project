using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.assistskill.dto;

public class FriendBackPackItemCellController : MonoBehaviour {


	private const string FriendBackPackItemCellPath = "Prefabs/Module/FriendModule/FriendBackPackItemCell";
	private FriendBackPackItemCell _view;
	private int index;
	private AssistSkillVo skillInfo;
	private int type;
	private System.Action<int,int> _onClick;

	public void InitView(){

		_view = gameObject.GetMissingComponent<FriendBackPackItemCell> ();
		_view.Setup (this.transform);

		RegisterEvent ();

	}
	

	public void SetData(int i, PackItemDto dto ,int type, System.Action<int,int> Callback, bool isEquipment = false){
		index = i;
		this.type = type;
		_onClick = Callback;

		_view.CountLabel.gameObject.SetActive (true);
		_view.IconSprite.gameObject.SetActive (true);
		_view.EquipmentSprite.gameObject.SetActive (isEquipment);	

		_view.CountLabel.pivot = UIWidget.Pivot.Right;
		_view.CountLabel.text = dto.count.ToString();

		if(_view.IconSprite.atlas.GetSprite(dto.item.icon) != null){
			_view.IconSprite.spriteName = dto.item.icon;
		}
		else
			_view.IconSprite.spriteName = "0";

		



	}
	public void SetData(int i, AssistSkillVo skillInfo ,int type, System.Action<int,int> Callback, bool isEquipment = false){
		index = i;
		this.type = type;
		_onClick = Callback;

		_view.CountLabel.gameObject.SetActive (true);
		_view.IconSprite.gameObject.SetActive (true);

		_view.CountLabel.pivot = UIWidget.Pivot.Left;
		_view.CountLabel.text = skillInfo.level.ToString ();
		_view.IconSprite.spriteName = "10013";
		_view.EquipmentSprite.gameObject.SetActive(false);
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
			_view.SelectEff.gameObject.SetActive(true);
		}
		else
		{
			_view.SelectEff.gameObject.SetActive(false);
		}
	}
	#endregion



	public	void RegisterEvent(){
		EventDelegate.Set (_view.FriendBackPackItemCellBtn.onClick, OnClickCallBack);
	}	

	public void Dispose(){

	}


}
