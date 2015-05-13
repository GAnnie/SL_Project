using UnityEngine;
using System.Collections;

public class PetConvenientHintItemController : MonoBehaviour {

	private PetConvenientHintItem _view;
	private int _index;
	private System.Action<int> _onSelect;

	public void InitItem(int index,System.Action<int> onSelect){
		_view = gameObject.GetMissingComponent<PetConvenientHintItem>();
		_view.Setup(this.transform);

		_index = index;
		_onSelect = onSelect;
		EventDelegate.Set(_view.eventTrigger.onClick,OnClickHintItem);
	}

	public void SetHintLbl(string hint){
		_view.hintLbl.text = hint;
	}

	private void OnClickHintItem(){
		if(_onSelect != null)
			_onSelect(_index);
	}
}
