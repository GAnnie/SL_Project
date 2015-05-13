using UnityEngine;
using System.Collections;

public class FilterItemController : MonoBehaviour {

//	public static FilterItemController current;
	FilterItemWidget _view;
	private int _filterId;
	private System.Action _onClick;

	public void InitItem(string name,int filterId,int groupId,System.Action onClick){
		_view = gameObject.GetMissingComponent<FilterItemWidget>();
		_view.Setup(this.transform);

		SetFilterId(filterId);
		SetFilterName(name);
		SetToggleGroup(groupId);
		_onClick = onClick;
	}

	public void SetFilterId(int filterId){
		_filterId = filterId;
	}

	public int GetFilterId(){
		return _filterId;
	}

	public void SetToggleGroup(int group){
		_view.filterToggle.group = group;
		_view.filterToggle.optionCanBeNone = true;
	}

	public void SetFilterName(string name){
		_view.nameLbl.text = name;
	}

	public bool GetSelected(){
		return _view.filterToggle.value;
	}

	public void SetSelected(bool active){
		_view.filterToggle.value = active;
	}

	void OnClick(){
		if (_onClick != null)
		{
//			current = this;
			_onClick();
//			current = null;
		}
		else
			SetSelected(!_view.filterToggle.value);
	}
}
