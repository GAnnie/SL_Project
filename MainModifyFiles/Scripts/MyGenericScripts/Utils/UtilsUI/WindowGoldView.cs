using UnityEngine;
using System.Collections;
using System;

public class WindowGoldView : MonoBehaviour 
{
	public UILabel titleLabel;
	public UILabel infoLabel;

	public UILabel okLabel;
    public UILabel okCostLabel;

	public UILabel cancelLabel;
    public UILabel cancelCostLabel;

	private GameObject _view;
    private System.Action<int> _callBack;
    private System.Action<int> _callBackCanel;

    private System.Action<bool> _closeCallBack = null;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void Open(GameObject view, string msg, String btnOKLabel, int btnOKCost, System.Action<int> callBack, String btnCancelLabel,int btnCancelCost,System.Action<int> callBackCanel, System.Action<bool> closeCallBack = null)
	{
		_view = view;
        _view.SetActive(true);

		_callBack = callBack;
		_callBackCanel = callBackCanel;
        _closeCallBack = closeCallBack;

		//titleLabel.text = title;
		infoLabel.text = msg;

        okLabel.text = btnOKLabel;
        okCostLabel.text = btnOKCost.ToString();

        cancelLabel.text = btnCancelLabel;
        cancelCostLabel.text = btnCancelCost.ToString();
	}
	
	public void OnCloseBtn()
	{
		CloseWindow(true);
	}
	
	public void OnOkBtn()
	{
		CloseWindow(false);
		if(_callBack != null)
            _callBack(int.Parse(okCostLabel.text));
	}
	
	public void OnCancelBtn()
	{
		CloseWindow(false);
		if(_callBackCanel != null)
            _callBackCanel(int.Parse(cancelCostLabel.text));
	}
	
	private void CloseWindow(bool onlyClosed){
        if( _closeCallBack != null )
        {
            _closeCallBack(onlyClosed);
        }		
	}
}
