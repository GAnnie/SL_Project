using UnityEngine;
using System.Collections;

public class PropertySliderController : MonoBehaviour {

	private int _hintId;
	private PropertySliderWidget _view;

	private int _index;
	private System.Action<int> _onSelect;

	public void InitItem(string title,int width,int hintid,string foreground){
		InitItem(title,hintid);

		_view.sliderBg.width = width;
		_view.foreground.spriteName = foreground;
	}

	public void InitItem(string title,int hintId){
		_view = gameObject.GetMissingComponent<PropertySliderWidget>();
		_view.Setup(this.transform);

		_view.valueBtn.gameObject.SetActive(false);
		_view.titleLbl.text = title;
		_hintId = hintId;
		_view.slider.value = 0f;

		EventDelegate.Set (_view.titleBtn.onClick, OnClickTitleBtn);
	}

	public void InitItem(int index,string title,int hintId,System.Action<int> onSelect){
		InitItem(title,hintId);

		_index = index;
		_onSelect = onSelect;
		EventDelegate.Set(_view.valueBtn.onClick,OnClickValueBtn);
	}

	public string GetTitleName(){
		return _view.titleLbl.text;
	}

	public void SetSliderVal(float val){
		_view.slider.value = val;
	}

	public void SetValLbl(string info){
		_view.valueLbl.text = info;
	}

//	private void OnClickValueBtn(){
//		TipManager.AddTip("获取途径");
//	}

	private void OnClickTitleBtn ()
	{
		GameHintManager.Open (_view.titleBtn.gameObject, _hintId);
	}

	public void SetValueBtnActive(bool b){
		_view.valueBtn.gameObject.SetActive(b);
	}

	private void OnClickValueBtn(){
		if(_onSelect != null)
			_onSelect(_index);
	}
}
