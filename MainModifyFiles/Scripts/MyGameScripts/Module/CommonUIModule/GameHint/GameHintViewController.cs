using UnityEngine;
using System.Collections;

public class GameHintViewController : MonoBehaviour {
	
	public enum Side{
		Left,
		Center,
		Right
	}
	public UIAnchor posAnchor;
	public UILabel hintLbl;
	public UISprite hintBg;
	private float _time;
	
	public void Open(GameObject target,string hint,Side pos){
		hintLbl.text = hint;
	
		SetupAnchor(target,pos);
        _time = 0f;

	}

	private void SetupAnchor(GameObject target,Side pos){
		posAnchor.side = UIAnchor.Side.Top;
		if(pos == Side.Center)
			posAnchor.pixelOffset = new Vector2(-hintLbl.width / 2,10);
		else if(pos == Side.Left)
			posAnchor.pixelOffset = new Vector2(-hintLbl.width,10);
		else if(pos == Side.Right)
			posAnchor.pixelOffset = new Vector2(0,10);

		posAnchor.container = target;
		posAnchor.Update();

		hintBg.UpdateAnchors();
		UIPanel panel = UIPanel.Find(hintLbl.cachedTransform);
		panel.ConstrainTargetToBounds(hintLbl.cachedTransform,true);
	}
	
	// Use this for initialization
	void OnEnable () {
		UICamera.onSelect += ClickEventHandler;
	}
	
	void OnDisable(){
		UICamera.onSelect -= ClickEventHandler;
	}
	
	void Update(){
		_time += Time.deltaTime;
		if(_time > GameHintManager.FADEOUT_TIME)
		{
			CloseView();
		}
	}
	
	void ClickEventHandler(GameObject go,bool state){
		CloseView();
	}
	
	void CloseView(){
		GameHintManager.Close();
	}
}
