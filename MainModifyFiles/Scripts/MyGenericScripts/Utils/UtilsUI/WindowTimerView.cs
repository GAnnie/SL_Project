using UnityEngine;
using System.Collections;

public class WindowTimerView : MonoBehaviour 
{
	public UILabel timeLabel;
	public UILabel infoLabel;
	public UILabel okLabel;
	public UILabel cancelLabel;

    public UIButton okBtn;
    public UIButton cancelBtn;

	private GameObject _view;
	private System.Action<int> _callBack;
    private System.Action<int> _callBackCanel;

    private System.Action _closeCallBack = null;

    private MonoTimer _timer;
    private int _time;
    private string _timeCompleteMsg;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void Open(GameObject view, int time, string msg, string timeCompleteMsg, System.Action<int> callBack, System.Action<int> callBackCanel, System.Action closeCallBack = null)
	{
		_view = view;
        if (_view != null)
        {
            _view.SetActive(true);
        }

		_callBack = callBack;
		_callBackCanel = callBackCanel;
        _closeCallBack = closeCallBack;

        _time = time;
        timeLabel.text = _time.ToString();
		infoLabel.text = msg;
        _timeCompleteMsg = timeCompleteMsg;

        if (_time > 0)
        {
            startTimer();
        }

        if (callBack == null && callBackCanel != null)
        {
            NGUITools.SetActiveSelf(okBtn.gameObject, false);
            cancelBtn.gameObject.transform.localPosition = new Vector3(0, cancelBtn.gameObject.transform.localPosition.y, cancelBtn.gameObject.transform.localPosition.z);
        }
	}

    private void startTimer()
    {
        if (_timer == null)
        {
            _timer = TimerManager.GetTimer("WindowTimerView");
            _timer.Setup2Time(1, onTimer);
            _timer.Play();
        }
    }

    private void onTimer()
    {
        _time -= 1;
        if (_time <= 0)
        {
            StopTimer();
        }
        else
        {
            timeLabel.text = _time.ToString();
        }
    }

    public void StopTimer()
    {
        timeLabel.text = "";
        infoLabel.text = _timeCompleteMsg;
        if (_timer != null)
        {
            _timer.Stop();
            _timer.RemoveHandler(onTimer);
            TimerManager.RemoveTimer(_timer);
            _timer = null;
        }
    }

	public void OnCloseBtn()
	{
        if( _closeCallBack != null )
        {
            _closeCallBack();
        }
        StopTimer();
        ShowMainUI();
        
	}
	
	public void OnOkBtn()
	{
		OnCloseBtn();
		if(_callBack != null)
			_callBack(_time);
	}
	
	public void OnCancelBtn()
	{
		OnCloseBtn();
		if(_callBackCanel != null)
			_callBackCanel(_time);
	}

	void ShowMainUI()
	{
		//UIManager.Instance.ShowMainUI();
	}
}
