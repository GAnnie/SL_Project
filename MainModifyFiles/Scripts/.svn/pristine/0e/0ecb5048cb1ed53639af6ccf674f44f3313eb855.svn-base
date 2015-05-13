using UnityEngine;
using System.Collections;

public class ChannlViewController : MonoBehaviour {

	ChannlView _view;
	private int _index;
	private bool _selected;
	private string _name;
	private string _style;
	private System.Action<int> _onSelect;
	
	public void InitItem(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<ChannlView>();
		_view.Setup(this.transform);
		
		_index = index;
		string spriteName = _view.Background.spriteName;
		_onSelect = selectCallback;
		gameObject.name = string.Format("FUNBtn{0}",index);
	}
	public void SetBtnName(string name)
	{
		_name = name;
		UpdateBtnState();
	}
	
	public void SetSelected(bool active)
	{
		_selected = active;
		UpdateBtnState();
	}
	
	private void UpdateBtnState(){
		if(_selected)
		{
			_view.Lbl.text = _name;
//			_view.Lbl.effectStyle = UILabel.Effect.Outline;
			_view.Background.spriteName = "channel-button-choice";
		}
		else
		{
//			_view.Lbl.text = StringHelper.WrapColor(_name,"6f3e1a");
			_view.Lbl.text = _name;
//			_view.Lbl.effectStyle = UILabel.Effect.None;
			_view.Background.spriteName = "channel-button";
		}
	}
	void OnClick(){
		if(_onSelect != null)
			_onSelect(_index);
	}

}
