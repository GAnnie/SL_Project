using UnityEngine;
using System.Collections;

public class RewardItemCellController : MonoBehaviour {

	
	private RewardItemCell _view;
	private int _packId;
	
	private System.Action<RewardItemCellController> _onClickCallBack;
	
	public int Index{ get;set;}

	
	public void InitView()
	{
		
		_view = gameObject.GetMissingComponent<RewardItemCell> ();
		_view.Setup(this.transform);
		_view.CountLabel.text = "";
		_view.IconSprite.enabled = false;
		
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
	}

	void OnClick()
	{
		if(_onClickCallBack != null)
			_onClickCallBack(this);
	}
	
	public void Dispose()
	{
		
	}

	public int RewardItemId;
	public void SetMailReward(int id , int count,System.Action<RewardItemCellController> onClickCallBack){
		_view.IconSprite.enabled = true;
		RewardItemId = id;
		_view.IconSprite.spriteName = id.ToString();
		_view.CountLabel.text = count.ToString ();
		_onClickCallBack = onClickCallBack;
	}
}
