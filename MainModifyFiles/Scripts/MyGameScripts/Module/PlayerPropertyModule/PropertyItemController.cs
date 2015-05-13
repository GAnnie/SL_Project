using UnityEngine;
using System.Collections;

public class PropertyItemController : MonoBehaviour
{
	private int _hintId;
	private int _value;
	private int _originVal;
	private bool _isFraction;
	private float pressAndHoldDelay = 0.5f;
	private float mDragStartTime = 0f;
	private PropertyItmeWidget _view;

	public event System.Action<PropertyItemController> OnAdd;
	public event System.Action<PropertyItemController> OnMinus;

	void Awake ()
	{
		_view = gameObject.GetMissingComponent<PropertyItmeWidget> ();
		_view.Setup (this.transform);
	}

	public void InitSimpleItem(string title,int val,int hintId=0)
	{
		_view.titleLbl.text = title;
		SetValue(val);
		SetEditable (false);

		_hintId = hintId;
		EventDelegate.Set (_view.titleBtn.onClick, OnClickTitleBtn);
	}
	
	public void InitItem (string title,int val,bool isFraction=false,int hintId=0)
	{
		_isFraction = isFraction;
		_view.titleLbl.text = title;
		SetValue (val, true);

		_view.leftBtn.onClick += OnClickLeftBtn;
		_view.rightBtn.onClick += OnClickRightBtn;
		_view.leftBtn.onPress += OnPressLeftBtn;
		_view.rightBtn.onPress += OnPressRightBtn;

		_hintId = hintId;
		EventDelegate.Set (_view.titleBtn.onClick, OnClickTitleBtn);
	}

	public int GetValue ()
	{
		return _value;
	}

	public void SetBgWidth(int width){
		_view.contentBg.width = width;
	}

	public void SetEditable (bool active)
	{
		_view.leftBtn.gameObject.SetActive (active);
		_view.rightBtn.gameObject.SetActive (active);
	}

	public void SetValue(int val)
	{
		_value = val;
		_view.valLbl.text = _value.ToString ();
	}

	public void SetValue (int val, bool needReset)
	{
		if (needReset)
			_originVal = val;
		_value = val;
		UpdateValLbl ();
	}

	public void SetValue(int currentVal, int maxVal)
	{
		_view.valLbl.text = string.Format ("{0}/{1}", currentVal, maxVal);
	}

	private void UpdateValLbl ()
	{
		if (_value > _originVal)
			_view.valLbl.color = Color.red;
		else
			_view.valLbl.color = ColorConstant.Color_UI_Title;

		if (_isFraction)
			_view.valLbl.text = string.Format ("{0}/{1}", _value, _value);
		else
			_view.valLbl.text = _value.ToString ();
	}

	private void OnClickLeftBtn(GameObject go){
		Minus();
	}

	private void OnClickRightBtn(GameObject go){
		Add();
	}

	private bool pressLeftBtn;
	private void OnPressLeftBtn(GameObject go, bool state){
		pressLeftBtn = state;
		if(state)
			mDragStartTime = RealTime.time + pressAndHoldDelay;
	}

	private bool pressRightBtn;
	private void OnPressRightBtn(GameObject go, bool state){
		pressRightBtn = state;
		if(state)
			mDragStartTime = RealTime.time + pressAndHoldDelay;
	}

	private void Add ()
	{
		if (OnAdd != null)
			OnAdd (this);
	}

	private void Minus ()
	{
		if (OnMinus != null)
			OnMinus (this);
	}

	void Update(){
		if(!pressLeftBtn && !pressRightBtn)
			return;
		if(mDragStartTime > RealTime.time)
			return;

		if(pressLeftBtn){
			Minus();
		}else if(pressRightBtn){
			Add();
		}
	}

	private void OnClickTitleBtn ()
	{
		GameHintManager.Open (_view.titleBtn.gameObject, _hintId);
	}
}
