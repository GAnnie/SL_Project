using UnityEngine;
using System.Collections;

public class ExpandContentTeamItemController : MonoBehaviour {

	ExpandContentTeamItem _view;
	private int _index;
	private System.Action<int> _onSelect;

	public void InitItem(int index,System.Action<int> selectCallback){
		_index = index;
		_onSelect = selectCallback;

		_view = gameObject.GetMissingComponent<ExpandContentTeamItem>();
		_view.Setup(this.transform);

		EventDelegate.Set(_view.bgBtn.onClick,OnClickBgBtn);
	}

	private void OnClickBgBtn(){
		if(_onSelect != null)
			_onSelect(_index);
	}

	public void SetIcon(string icon){
		_view.iconSprite.spriteName = icon;
	}

	public void SetInfoLbl(string info){
		_view.infoLbl.text = info;
	}

	public void SetStatusLbl(string status){
		_view.statusLbl.text = status;
	}

	public void SetFactionIcon(int factionId){
		_view.factionIcon.spriteName = string.Format("factionIcon_{0}",factionId);
	}
}
