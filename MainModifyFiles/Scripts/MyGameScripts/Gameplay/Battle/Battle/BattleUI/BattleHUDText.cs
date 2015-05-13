using UnityEngine;
using System.Collections;

public class BattleHUDText : MonoBehaviourBase
{
	private HUDText mText = null;
	private GameObject hudGO;
	UIFollowTarget followTarget;
	
	private MonsterController _monsterController;
	
	public void SetTarget( MonsterController monsterController )
	{
		_monsterController = monsterController;
	}	
		
	void Start ()
	{
		if ( mText == null )
		{
			hudGO = NGUITools.AddChild( LayerManager.Instance.floatTipAnchor, ( GameObject ) ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/HUDText/HUDText"));
			mText = hudGO.GetComponentInChildren<HUDText>();
			
			if ( mText == null )
				return;
		}
		
		followTarget = mText.gameObject.AddComponent<UIFollowTarget>();
		followTarget.gameCamera = LayerManager.Instance.GetBattleFollowCamera();
		followTarget.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tr = _monsterController.GetMountHUD();

		if ( tr == null )
		{
			followTarget.target = this.gameObject.transform;
			return;
		}
		
		if ( followTarget != null )
			followTarget.target = tr;
	}
	
    public void AddText( string text, Color color, float stayDuration )
    {
		mText.Add( text, color, stayDuration );
	}
	
	public void CleanText(){
		mText.Clean();
	}
	
	void OnDestroy ()
	{
		GameObject.Destroy(hudGO);
		mText = null;
		hudGO = null;
	}
}
