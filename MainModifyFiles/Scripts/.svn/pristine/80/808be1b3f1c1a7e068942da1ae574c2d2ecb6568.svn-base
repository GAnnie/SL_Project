using UnityEngine;
using System.Collections;

public class FormationItemSlotController : MonoBehaviour {

	public bool CanDrag;
	private FormationItemWidget _view;
	private int _index;
	private Transform _cloneIconTrans;
	private UIRoot mRoot;

	private System.Action _onDragStartCallback;
//	private System.Action _onDragCallback;
	private System.Action<int> _onDragEndCallback;

	public void InitItem(int index,System.Action onDragStart,System.Action<int> onDragEnd){
		_view = gameObject.GetMissingComponent<FormationItemWidget>();
		_view.Setup(this.transform);

		_index = index;
		gameObject.name = "FormationItem_"+index;
		ResetItem();

		_onDragStartCallback = onDragStart;
//		_onDragCallback = onDrag;
		_onDragEndCallback = onDragEnd;
		mRoot = NGUITools.FindInParents<UIRoot>(this.transform);
//		EventDelegate.Set(_view.eventTrigger.onDragStart,HandleDragStart);
//		EventDelegate.Set(_view.eventTrigger.onDrag,HandleOnDrag);
//		EventDelegate.Set(_view.eventTrigger.onDragOver,HandleOnDragOver);
//		EventDelegate.Set(_view.eventTrigger.onDragOut,HandleOnDragOut);
//		EventDelegate.Set(_view.eventTrigger.onDragEnd,HandleDragEnd);
	}
	
	public void SetIcon(string icon){
		_view.icon.enabled = true;
		if(_view.icon.atlas.GetSprite(icon) != null){
			_view.icon.spriteName = icon;
		}else{
			_view.icon.spriteName = "0";
		}
		_view.icon.MakePixelPerfect();
	}
	
	public void SetPosLbl(int pos){
		_view.bgSprite.SetActive(true);
		_view.indexSprite.spriteName = string.Format("num-y-{0}",pos);
	}

	public void SetSelected(bool active){
		_view.selectEffect.SetActive(active);
	}

	public void SetNameLbl(string name){
		_view.nameLbl.text = name;
	}

	public void ResetItem(){
		_view.selectEffect.SetActive(false);
		_view.icon.enabled = false;
		_view.bgSprite.SetActive(false);
		_view.indexSprite.spriteName = "";
		_view.nameLbl.text ="";
		CanDrag = false;
	}

	public int GetIndex(){
		return _index;
	}

	private void OnDragStart(){
		if(!CanDrag) return;

		_cloneIconTrans = NGUITools.AddChild(this.gameObject,_view.icon.cachedGameObject).transform;
		if (UIDragDropRoot.root != null)
			_cloneIconTrans.parent = UIDragDropRoot.root;

		if(_onDragStartCallback != null)
			_onDragStartCallback();
	}

	private void OnDrag(Vector2 delta){
		if(CanDrag && _cloneIconTrans != null){
			_cloneIconTrans.localPosition += (Vector3)(mRoot.pixelSizeAdjustment*delta);
//			if(_onDragCallback != null)
//				_onDragCallback();
		}
	}

	private void OnDragOver(GameObject dragged){
		if(_index == -1) return;

		FormationItemSlotController formationItem = dragged.GetComponent<FormationItemSlotController>();
		if(formationItem != null && !formationItem.CanDrag) return;

		CharacterInfoItemController characterInfoItem = dragged.GetComponent<CharacterInfoItemController>();
		if(characterInfoItem != null && !characterInfoItem.CanDrag)	return;

		SetSelected(true);
	}

	private void OnDragOut(GameObject dragged){
		SetSelected(false);
	}

	private void OnDragEnd(){
		if(!CanDrag) return;

		NGUITools.Destroy(_cloneIconTrans.gameObject);
		if(_onDragEndCallback != null)
			_onDragEndCallback(_index);
	}
}
