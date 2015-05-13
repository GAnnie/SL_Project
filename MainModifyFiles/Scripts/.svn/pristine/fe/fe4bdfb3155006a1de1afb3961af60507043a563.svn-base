using UnityEngine;
using System.Collections;

public class TabBtnController : MonoBehaviour {

	TabBtnWidget _view;
	private int _index;
	private bool _selected;
	private string _name;
	private string _style;
	private System.Action<int> _onSelect;
	
	private int _localDepth = 1;
	private const int _maxDepth = 6;

	public void InitItem(int index,System.Action<int> selectCallback){
		_view = gameObject.GetMissingComponent<TabBtnWidget>();
		_view.Setup(this.transform);

		//根据index值计算当前tabBtn深度，5-4-3-2-1
		_localDepth = _maxDepth - (index+1);
		_view.btnBg.depth = _localDepth;

		_index = index;
		//从prefab中的btnBg提取出标签页的样式
		string spriteName = _view.btnBg.spriteName;
		_style = spriteName.Remove(spriteName.LastIndexOf('_'));
		_onSelect = selectCallback;
		gameObject.name = string.Format("TabBtn_{0}",index);
	}

	public void SetBtnLblSpac(int x = 0, int y = 0) {
		_view.btnLbl.spacingX = x;
		_view.btnLbl.spacingY = y;
	}

	public void SetBtnName(string name)
	{
		_name = name;
		UpdateBtnState();
	}

	public void SetSelected(bool active)
	{
		_selected = active;
		UpdateBtnState();
	}

	private void UpdateBtnState(){
		if(_selected)
		{
			_view.btnLbl.text = StringHelper.WrapColor(_name,"ffdd7b");
			_view.btnLbl.effectStyle = UILabel.Effect.Outline8;
			_view.btnLbl.effectColor = ColorExt.HexToColor("6f3e1a");
			_view.btnBg.spriteName = string.Format("{0}_On",_style);

			_view.btnBg.depth = _maxDepth;
		}
		else
		{
			_view.btnLbl.text = StringHelper.WrapColor(_name,"6f3e1a");
			_view.btnLbl.effectStyle = UILabel.Effect.None;
			_view.btnBg.spriteName = string.Format("{0}_Off",_style);

			_view.btnBg.depth = _localDepth;
		}
	}

	public bool GetSelected(){
		return _selected;
	}

	void OnClick(){
		if(_onSelect != null)
			_onSelect(_index);
	}

	//	----------------- useing in ulua ----------------
	public void InitItemLua(int index){
		_view = gameObject.GetMissingComponent<TabBtnWidget>();
		_view.Setup(this.transform);

		_index = index;
		//从prefab中的btnBg提取出标签页的样式
		string spriteName = _view.btnBg.spriteName;
		_style = spriteName.Remove(spriteName.LastIndexOf('_'));
		gameObject.name = string.Format("TabBtn_{0}",index);
	}
}
