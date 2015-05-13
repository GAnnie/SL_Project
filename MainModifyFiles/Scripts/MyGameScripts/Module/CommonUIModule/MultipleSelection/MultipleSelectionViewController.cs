using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleSelectionViewController : MonoBehaviour {
	
	private MultipleSelectionView _view;
	private List<UIButton> _btnList = new List<UIButton>();
	private int activeBtnCount;
	private System.Action<int> _clickBtnCallback;

	public void InitView(){
		_view = gameObject.GetMissingComponent<MultipleSelectionView>();
		_view.Setup(this.transform);
	}

	public void Open(GameObject target,List<string> btnNameList,System.Action<int> clickCallback,bool leftMode){
		_clickBtnCallback = clickCallback;
		InitBtnList(btnNameList);
		ChangeAnchorMode(target,leftMode);
		UICamera.onClick += ClickEventHandler;
	}

	private void InitBtnList(List<string> btnNameList){
		int btnCount = btnNameList.Count;
		if(btnCount > activeBtnCount)
		{
			if(btnCount > _btnList.Count){
				GameObject btnPrefab = _view.selectBtnPrefab;
				for(int i=_btnList.Count;i<btnCount;++i){
					GameObject newBtn = NGUITools.AddChild(_view.btnGridTrans.gameObject,btnPrefab);
					newBtn.name = "selectBtn_"+i;
					newBtn.SetActive(true);
					UIButton uiBtn = newBtn.GetComponent<UIButton>();
					EventDelegate.Set(uiBtn.onClick,OnClickSelectionBtn);
					_btnList.Add(uiBtn);
				}
			}
			
			for(int i=activeBtnCount;i<btnCount;++i){
				_btnList[i].gameObject.SetActive(true);
			}
		}
		else{
			for(int i=_btnList.Count-1;i>=btnCount;--i){
				_btnList[i].gameObject.SetActive(false);					
			}
		}

		for(int i=0;i<btnCount;++i)
		{
			UILabel btnLbl = _btnList[i].GetComponentInChildren<UILabel>();
			btnLbl.text = btnNameList[i];
		}
		activeBtnCount = btnCount;
		_view.btnGrid.Reposition();
		_view.bgSprite.height = Mathf.RoundToInt(48*btnCount);
	}

	public void SetupBtnName(int index,string name)
	{
		if(index < activeBtnCount)
		{
			UILabel btnLbl = _btnList[index].GetComponentInChildren<UILabel>();
			btnLbl.text = name;
		}
	}
	
	private void ChangeAnchorMode(GameObject target,bool leftMode){
		if(leftMode)
		{
			_view.viewAnchor.side = UIAnchor.Side.TopLeft;
			_view.bgSprite.pivot = UIWidget.Pivot.TopRight;
			_view.bgSprite.cachedTransform.localPosition = Vector3.zero;
			_view.btnGridTrans.localPosition = new Vector3(-75f,-5f,0);
			_view.arrowSprite.flip = UIBasicSprite.Flip.Nothing;
			_view.arrowSprite.cachedTransform.localPosition = new Vector3(6f,-28f,0f);
		}
		else
		{
			_view.viewAnchor.side = UIAnchor.Side.TopRight;
			_view.bgSprite.pivot = UIWidget.Pivot.TopLeft;
			_view.bgSprite.cachedTransform.localPosition = Vector3.zero;
			_view.btnGridTrans.localPosition = new Vector3(75f,-5f,0);
			_view.arrowSprite.flip = UIBasicSprite.Flip.Horizontally;
			_view.arrowSprite.cachedTransform.localPosition = new Vector3(-6f,-28f,0f);
		}

		_view.viewAnchor.container = target;
		_view.viewAnchor.Update();
	}

	private void OnClickSelectionBtn(){
		int index = _btnList.IndexOf(UIButton.current);
		if(_clickBtnCallback != null)
			_clickBtnCallback(index);

		CloseView();
	}

	void ClickEventHandler(GameObject clickGo){
		UIPanel panel = UIPanel.Find(clickGo.transform);
		if(panel != _view.uiPanel)
			CloseView();
	}
	
	void CloseView(){
		_clickBtnCallback = null;
		UICamera.onClick -= ClickEventHandler;
		MultipleSelectionManager.Close();
	}
}
