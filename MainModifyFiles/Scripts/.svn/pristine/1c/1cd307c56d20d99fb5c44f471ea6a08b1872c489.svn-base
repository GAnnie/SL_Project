using UnityEngine;
using System.Collections;

public class AddPointSliderController : MonoBehaviour
{

	private AddPointSlider _view;

	//-1-none 0-minus 1-add
	private const int PRESS_NONE = -1;
	private const int PRESS_MINUS = 0;
	private const int PRESS_ADD = 1;
	private int pressBtnState = PRESS_NONE;
	private const float pressAndHoldDelay = 0.5f;
	private float mDragStartTime = 0f;
	private PlayerBaseInfoMainViewController _baseInfoController;
	private int _maxVal;
	private int _realVal;
	private int _addPoint;

	public int AddPoint {
		get {
			return _addPoint;
		}
	}

	private float _maxAddPointSliderVal;

	public void InitItem (string title, PlayerBaseInfoMainViewController controller)
	{
		_baseInfoController = controller;
		_view = gameObject.GetMissingComponent<AddPointSlider> ();
		_view.Setup (this.transform);

		_view.titleLbl.text = title;
		EventDelegate.Set (_view.addPointSlider.onChange, () => {
			UISlider.current.value = Mathf.Clamp(UISlider.current.value,_view.realSlider.value,_maxAddPointSliderVal);
		});

		UIEventListener thumEvListener = UIEventListener.Get(_view.thumbCollider.gameObject);
		thumEvListener.onDrag += (go,delta) => {
			if (delta.x > 0) {
				int newAddPoint = Mathf.RoundToInt ((_view.addPointSlider.value - _view.realSlider.value) * _maxVal);
				_baseInfoController.OnApSliderAdd (this, newAddPoint - _addPoint);
			} else if (delta.x < 0) {
				int newAddPoint = Mathf.RoundToInt ((_view.addPointSlider.value - _view.realSlider.value) * _maxVal);
				_baseInfoController.OnApSliderMinus (this, _addPoint - newAddPoint);
			}
		};

		EventDelegate.Set (_view.addEventTrigger.onClick, OnClickAddBtn);
		EventDelegate.Set (_view.addEventTrigger.onPress, () => {
			pressBtnState = PRESS_ADD;
			mDragStartTime = RealTime.time + pressAndHoldDelay;
		});

		EventDelegate.Set (_view.addEventTrigger.onRelease, () => {
			pressBtnState = PRESS_NONE;
		});

		EventDelegate.Set (_view.minusEventTrigger.onClick, OnClickMinusBtn);
		EventDelegate.Set (_view.minusEventTrigger.onPress, () => {
			pressBtnState = PRESS_MINUS;
			mDragStartTime = RealTime.time + pressAndHoldDelay;
		});
		
		EventDelegate.Set (_view.minusEventTrigger.onRelease, () => {
			pressBtnState = PRESS_NONE;
		});
	}

	public void ResetItem (int originVal, int realVal, int maxVal)
	{
		_maxVal = maxVal;
		_realVal = realVal;
		_addPoint = 0;
		_view.originSlider.value = (float)originVal / (float)maxVal;
		_view.realSlider.value = (float)realVal / (float)maxVal;

		UpdateMaxSliderVal ();
		SetAddPointVal (0);
	}

	public void UpdateMaxSliderVal ()
	{
		_maxAddPointSliderVal =_view.realSlider.value + (float)(_baseInfoController.RemainPotential+_addPoint)/ (float)_maxVal;
	}

	public void SetAddPointVal (int val)
	{
		_addPoint = val;
		_view.addPointSlider.value = _view.realSlider.value + (float)_addPoint / (float)_maxVal;
		if (_addPoint > 0) {
			_view.addPointLbl.text = string.Format ("+{0}", _addPoint);
			SetMinusBtnGrey (false);
		} else {
			_view.addPointLbl.text = "";
			SetMinusBtnGrey (true);
		}

		_view.originLbl.text = (_realVal + _addPoint).ToString ();
	}
	
	public void SetAddBtnGrey (bool b)
	{
		_view.addBtnSprite.isGrey = b;
	}

	public void SetMinusBtnGrey (bool b)
	{
		_view.minusBtnSprite.isGrey = b;
	}

	public void SetThumbActive(bool b){
		_view.thumbCollider.enabled = b;
	}

	private void OnClickAddBtn ()
	{
		_baseInfoController.OnApSliderAdd (this, 1);
	}

	private void OnClickMinusBtn ()
	{
		if (_addPoint > 0) {
			_baseInfoController.OnApSliderMinus (this, 1);
		}
	}

	void Update ()
	{
		if (pressBtnState == PRESS_NONE)
			return;
		if (mDragStartTime > RealTime.time)
			return;

		if (pressBtnState == PRESS_ADD)
			OnClickAddBtn ();
		else if (pressBtnState == PRESS_MINUS)
			OnClickMinusBtn ();
	}
}
