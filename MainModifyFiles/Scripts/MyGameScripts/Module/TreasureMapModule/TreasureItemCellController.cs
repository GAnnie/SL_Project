using UnityEngine;
using System.Collections;

public class TreasureItemCellController : MonoBehaviour {
	private TreasureItemCell _view;

	public void InitView(){
		_view = gameObject.GetMissingComponent<TreasureItemCell> ();
		_view.Setup (this.transform);

		RegisterEvent();
	}

	public void RegisterEvent(){

	}

	public void SetData(){

		_view.DescriptionLbl.text = "11";
	}

	public void SetSelected(bool b){
		_view.SelectEff.gameObject.SetActive(b);
	}
}
