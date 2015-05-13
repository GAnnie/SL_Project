using UnityEngine;
using System.Collections;

public class ChatMsgViewController : MonoBehaviour {

	private ChatMsgView _view;

	public void InitView(){
		_view = gameObject.GetMissingComponent<ChatMsgView> ();
		_view.Setup (this.transform);
	}


	public void SetData(string msg){
		_view.msgLbl.text = msg;
	}

	void OnClick()
	{
		
		string tempStr = _view.msgLbl.GetUrlAtPosition( UICamera.lastHit.point );
		if( !string.IsNullOrEmpty( tempStr ) )
		{
			TipManager.AddTip(tempStr);
			
			ProxyItemTipsModule.Open(int.Parse(tempStr),UICamera.lastHit.transform.gameObject);
		}
		else
		ProxyChatModule.Open();
		
	}
}
