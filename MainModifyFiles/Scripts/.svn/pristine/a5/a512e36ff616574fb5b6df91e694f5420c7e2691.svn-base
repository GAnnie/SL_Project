using UnityEngine;
using System.Collections;

public class PropertyInfoController : MonoBehaviour {

	private PropertyInfoWidget _view;
	private int _hintId;

	public void InitItem(string title,int hintId,int width){
		_view = gameObject.GetMissingComponent<PropertyInfoWidget> ();
		_view.Setup (this.transform);

		_view.extraInfoLbl.text = "";
		_view.contentBg.width = width;
		_view.titleLbl.text = title;
		_hintId = hintId;

		EventDelegate.Set(_view.titleBtn.onClick,OnClickTitleBtn);
	}

	public void SetVal(int val){
		_view.valLbl.text = val.ToString();
	}

	public void SetValLbl(string val){
		_view.valLbl.text = val;
	}

	public void SetValLblColor(Color color){
		_view.valLbl.color = color;
	}

	public void SetExtraInfoLbl(string val){
		_view.extraInfoLbl.text = val;
	}

	private void OnClickTitleBtn ()
	{
		GameHintManager.Open (_view.titleBtn.gameObject, _hintId);
	}
}
