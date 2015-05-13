using UnityEngine;
using System.Collections;

public class BaseItemCellController : MonoBehaviour {

	private BaseItemCellWidget _view;
	private int _index;
	private System.Action<int> _onSelect;
    
	public void InitItem(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<BaseItemCellWidget>();
		_view.Setup(this.transform);

		_index = index;
		gameObject.name = "BaseItemCell_"+index;
		_onSelect = selectCallback;
		EventDelegate.Set(_view.itemBtn.onClick,OnClickItem);
		ResetItem();
	}

	public void SetSelected(bool active){
		_view.selectEffect.SetActive(active);
	}

	public void SetRedDot(bool active){
		_view.redDot.SetActive(active);
	}

	public void SetIcon(string icon){
		if(_view.icon.atlas.GetSprite(icon) != null){
			_view.icon.spriteName = icon;
		}else{
			_view.icon.spriteName = "0";
		}
		_view.icon.MakePixelPerfect();
	}

	public void SetGrey(bool active){
		_view.icon.isGrey = active;
	}

	public void SetIconLbl(string info){
		if(info != "1")
			_view.iconLbl.text = info.WrapSymbol("b");
		else
			_view.iconLbl.text = "";
	}

	public void SetIconLblAnchor(UIWidget.Pivot pivot){
		_view.iconLbl.pivot = pivot;
		if(pivot == UIWidget.Pivot.Left){
			_view.iconLbl.cachedTransform.localPosition = new Vector3(-30f,-23f,0f);
		}else if(pivot == UIWidget.Pivot.Right){
			_view.iconLbl.cachedTransform.localPosition = new Vector3(30f,-23f,0f);
        }
	}

	public void SetItemLbl(string itemName){
		if(string.IsNullOrEmpty(itemName)){
			_view.itemLbl.cachedGameObject.SetActive(false);
		}else{
			_view.itemLbl.text = itemName;
			_view.itemLbl.cachedGameObject.SetActive(true);
		}
	}

	public void ResetItem(){
		_view.icon.spriteName = "";
		_view.iconLbl.text = "";
		SetSelected(false);
		SetRedDot(false);
		SetItemLbl("");
	}

	private void OnClickItem(){
		if(_onSelect != null)
			_onSelect(_index);
	}
}
