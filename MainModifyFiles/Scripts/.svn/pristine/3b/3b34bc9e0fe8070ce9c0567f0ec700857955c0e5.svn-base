using UnityEngine;
using System.Collections;

public class ExpressionViewController : MonoBehaviour {


	private ExpressionView _view;
	private int nameNo;
	private System.Action<int> _callBack;

	public void InitView(){
		_view = gameObject.GetMissingComponent<ExpressionView> ();
		_view.Setup (this.transform);

		RegisterEvent();
	}

	public void SetData(string icon ,int No,System.Action<int> callBack){
		_view.ExpressionBg.spriteName = icon;
		nameNo= No;
		_callBack = callBack;
	}


	public void RegisterEvent(){
		EventDelegate.Set(_view.ExpressionBtn.onClick,OnExpressionBtnClick);
	}

	public void OnExpressionBtnClick(){
		if(_callBack != null){
			_callBack(nameNo);
		}
	}
}
