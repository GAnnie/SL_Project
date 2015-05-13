using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.formation.data;

public class LayoutItemController : MonoBehaviour {

	private	LayoutItemWidget _view;
	private System.Action<LayoutItemController> _clickCallback;
	private Formation _formation;
	public Formation formation{
		get{ return _formation; }
	}

	public void InitItem(Formation formation,System.Action<LayoutItemController> clickCallback){
		_view = gameObject.GetMissingComponent<LayoutItemWidget>();
		_view.Setup(this.transform);

		_formation = formation;
		SetNameLbl(formation.name);
		_clickCallback  = clickCallback;

		EventDelegate.Set(_view.itemBtn.onClick,OnSelectItem);
		SetSelected(false);
	}

	public void SetSelected(bool select){
		_view.selectedEffect.SetActive(select);
	}

	public void SetNameLbl(string name){
		_view.nameLbl.text = name;
	}

	private void OnSelectItem(){
		if(_clickCallback != null)
			_clickCallback(this);
	}
}
