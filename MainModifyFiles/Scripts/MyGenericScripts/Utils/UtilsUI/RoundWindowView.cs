using UnityEngine;
using System.Collections;

public class RoundWindowView : MonoBehaviour 
{
	public UILabel infoLabel;
	public UILabel okLabel;
    public GameObject closeBtn;

	private GameObject _view;
	private WindowManager.CallBackOkHandler _callBack;

    private System.Action _closeCallBack = null;

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void Open(GameObject view , string msg , WindowManager.CallBackOkHandler callBack, System.Action closeCallBack = null, bool isShowCloseBtn = true ,bool isShowOkBtn = true)
	{
		_view = view;

		_callBack = callBack;
        _closeCallBack = closeCallBack;
	
		SetContentLbl( msg );

        if (!isShowCloseBtn && closeBtn != null)
        {
            closeBtn.SetActive(false);
        }
		if(!isShowOkBtn && okLabel != null)
		{
			okLabel.transform.parent.gameObject.SetActive(false);
		}
	}
	
	public void OnCloseBtn()
	{
        if ( _closeCallBack != null )
        {
            _closeCallBack();
        }
        //GameObject.Destroy(_view);
		//UIManager.Instance.ShowMainUI();
	}
	
	public void OnOkBtn()
	{
		OnCloseBtn();
		if(_callBack != null)
			_callBack();
	}

	private void SetContentLbl( string msg )
	{
		if( msg.Length > 18 )
		{
			infoLabel.pivot = UILabel.Pivot.Left;
		}
		else
		{
			infoLabel.pivot = UILabel.Pivot.Center;
		}
		infoLabel.text = msg;
	}
}
