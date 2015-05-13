using UnityEngine;
using System.Collections;

public class CharacterInfoItemController : MonoBehaviour {

	CharacterInfoWidget _view;
	private int _index;
	private bool _selected;
	private System.Action<int> _onSelect;

	public bool CanDrag;
	private float pressAndHoldDelay = 0.5f;
	private float mDragStartTime = 0f;
	private UIDragScrollView mDragScrollView = null;
	private UIRoot mRoot;
	private bool mPressed = false;
	private bool mDragging = false;
	private Transform _cloneIconTrans;
	private System.Action _onDragStartCallback;
	private System.Action<int> _onDragEndCallback;

	public void InitItem(int index,System.Action<int> selectCallback,System.Action onDragStart,System.Action<int> onDragEnd){
		_onDragStartCallback = onDragStart;
		_onDragEndCallback = onDragEnd;
		InitItem(index,selectCallback);
	}

	public void InitItem(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<CharacterInfoWidget> ();
		_view.Setup(this.transform);

		_index = index;
		gameObject.name = "CharacterInfoWidget_"+index;
		_onSelect = selectCallback;

		mDragScrollView = this.GetComponent<UIDragScrollView>();
		mRoot = NGUITools.FindInParents<UIRoot>(this.transform);
	}

	public void ResetItem(){
		_view.bgSprite.spriteName = "the-choice-lines";
		_view.infoLbl.text = "";
		_view.iconFlag.enabled = false;
		_view.icon.enabled = false;
		_view.selectEffect.SetActive(false);
		_view.statusSprite.enabled = false;
		_view.indexSprite.spriteName = "";
		_view.indexBgGo.SetActive(false);
		CanDrag = false;
	}

	public void SetIndexSprite(int index){
		_view.indexBgGo.SetActive(true);
		_view.indexSprite.spriteName = string.Format("num-y-{0}",index);
	}

	public void SetStatusSprite(string spriteName){
		_view.statusSprite.spriteName = spriteName;
		_view.statusSprite.enabled = true;
	}

	public void SetInfoLbl(string info){
		_view.infoLbl.text = info;
	}

	public void SetIconFlagState(bool active){
		_view.iconFlag.enabled = active;
	}

	public void SetIcon(string icon){
		_view.icon.spriteName = icon;
		_view.icon.enabled = true;
		_view.icon.MakePixelPerfect();
	}

	public void SetSelected(bool active){
		_selected = active;
//		_view.selectEffect.SetActive(active);
		_view.bgSprite.spriteName = _selected ? "the-no-choice-lines":"the-choice-lines";
	}

	public int GetIndex(){
		return _index;
	}

	void OnClick(){
		if(_onSelect != null)
			_onSelect(_index);
	}

	void OnPress(bool isPressed){
		if(!CanDrag){
			return;
		}

		if (isPressed)
		{
			mDragStartTime = RealTime.time + pressAndHoldDelay;
			mPressed = true;
		}
		else{
			StopDragging();
			mPressed = false;
		}
	}

	void Update(){
		if(mPressed && !mDragging && mDragStartTime < RealTime.time)
			StartDragging();
	}

	private void StartDragging(){
		if(!mDragging){
			_cloneIconTrans = NGUITools.AddChild(this.gameObject,_view.icon.cachedGameObject).transform;
			_cloneIconTrans.position = UICamera.lastWorldPosition;

			if (UIDragDropRoot.root != null)
				_cloneIconTrans.parent = UIDragDropRoot.root;

			mDragging = true;
			if(mDragScrollView != null)
				mDragScrollView.enabled = false;

			if(_onDragStartCallback != null)
				_onDragStartCallback();
		}
	}

	void OnDrag(Vector2 delta){
		if (!mDragging){
			//未满足按住时长，就拖动Item取消按住状态，防止在按住的情况下也可以进行拖动
			mPressed = false;
			return;
		}
		_cloneIconTrans.localPosition +=(Vector3)(mRoot.pixelSizeAdjustment*delta);
	}

	void OnDragOver(GameObject dragged){
		if(!CanDrag || _selected) return;

		CharacterInfoItemController characterInfoItem = dragged.GetComponent<CharacterInfoItemController>();
		if(characterInfoItem != null && characterInfoItem.mDragging){
			_view.selectEffect.SetActive(true);
			return;
		}

		FormationItemSlotController formationItem = dragged.GetComponent<FormationItemSlotController>();
		if(formationItem != null && formationItem.CanDrag){
			_view.selectEffect.SetActive(true);
			return;
		}
	}

	void OnDragOut(GameObject dragged){
		if(!CanDrag || _selected) return;

		_view.selectEffect.SetActive(false);
	}

	void OnDragEnd(){
		StopDragging();
	}

	private void StopDragging(){
		if (mDragging)
		{
			mDragging = false;
			NGUITools.Destroy(_cloneIconTrans.gameObject);
			if (mDragScrollView != null) 
				mDragScrollView.enabled = true;
			
			if(_onDragEndCallback != null)
				_onDragEndCallback(_index);
		}
	}
}
