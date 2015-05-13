using UnityEngine;
using System.Collections;

public class BaseToggleController : MonoBehaviour {

	private ToggleWidget _view;
	private int _index;
	private System.Action<int> _onSelect;

	public void InitItem(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<ToggleWidget>();
		_view.Setup(this.transform);

		_index = index;
		_onSelect = selectCallback;
		SetSelected(false);
		EventDelegate.Set(_view.btn.onClick,OnClickItem);
	}

	public void SetLabel(string name){
		_view.label.text = name;
	}

	public bool GetSelected(){
		return _view.hook.activeSelf;
	}
	
	public void SetSelected(bool active)
	{
		_view.hook.SetActive(active);
	}

	private void OnClickItem(){
		if(_onSelect != null)
			_onSelect(_index);
	}
}
