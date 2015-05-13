using UnityEngine;
using System.Collections;

public class MessageWindowView : MonoBehaviour 
{
	public UILabel titleLabel;
	public UILabel infoLabel;
	public UILabel okLabel;
	
	
	private GameObject _view;
	private WindowManager.CallBackOkHandler _callBack;

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void Open(GameObject view,string msg,WindowManager.CallBackOkHandler callBack)
	{
		_view = view;
		
		_callBack = callBack;

		//titleLabel.text = title;
		infoLabel.text = msg;
	}
	
	public void OnCloseBtn()
	{
		GameObject.Destroy(_view);
	}
	
	public void OnOkBtn()
	{
		OnCloseBtn();
		if(_callBack != null)
			_callBack();
	}
}
