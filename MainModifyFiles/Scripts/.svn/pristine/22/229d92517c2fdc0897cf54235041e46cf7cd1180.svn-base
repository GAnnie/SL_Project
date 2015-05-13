using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class CrewTavernItemController : MonoBehaviour
{

	private CrewTavernItemWidget _view;
	private Crew _crew;
	private ModelDisplayController _modelController;
	private bool _hasRecruited;

	public void InitItem (Crew crew,bool hasRecruited)
	{
		_view = gameObject.GetMissingComponent<CrewTavernItemWidget> ();
		_view.Setup (this.transform);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (256, 256);

		EventDelegate.Set (_view.buyBtn.onClick, OnClickBuyBtn);
		EventDelegate.Set (_view.hintBtn.onClick, OnClickHintBtn);

		_crew = crew;
		_modelController.SetupModel (crew);
		_view.nameLbl.text = crew.name;
		_view.factionLbl.text = crew.faction.name;
		long cost = crew.recruitFee.copper > 0L ? crew.recruitFee.copper : crew.recruitFee.ingot;
		string costStr = "";
		if (crew.recruitFee.copper > 0L) {
			costStr = string.Format ("{0} {1}", crew.recruitFee.copper, ItemIconConst.Copper);
		} else if (crew.recruitFee.ingot > 0) {
			costStr = string.Format ("{0} {1}", crew.recruitFee.ingot, ItemIconConst.Ingot);
		} else
			costStr = "免费";
		_view.detailLbl.text = string.Format ("等级：{0}\n消耗：{1}", crew.recruitLevel, costStr);

		_hasRecruited = hasRecruited;
		UpdateBuyBtnState();
	}

	private void UpdateBuyBtnState(){
		UILabel buyBtnLbl = _view.buyBtn.transform.Find ("Label").GetComponent<UILabel> ();
		if (_hasRecruited) {
			buyBtnLbl.text = "已招募".WrapColor("5cf37c");
			_view.btnSprite.enabled = false;
		} else {
			buyBtnLbl.text = "招  募";
			if (PlayerModel.Instance.GetPlayerLevel () < _crew.recruitLevel)
				_view.buyBtn.GetComponent<UISprite> ().isGrey = true;
		}
	}

	private void OnClickBuyBtn ()
	{
		if (_hasRecruited) {
			TipManager.AddTip ("该伙伴已招募");
			return;
		}

		if (PlayerModel.Instance.GetPlayerLevel () < _crew.recruitLevel) {
			TipManager.AddTip (string.Format ("招募该伙伴需要人物等级≥{0}", _crew.recruitLevel));
			return;
		}

		if (_crew.recruitFee.copper > 0L && !PlayerModel.Instance.isEnoughCopper (_crew.recruitFee.copper, true)) {
			return;
		} else if (_crew.recruitFee.ingot > 0 && !PlayerModel.Instance.isEnoughIngot (_crew.recruitFee.ingot, true)) {
			return;
		}

		CrewModel.Instance.RecruitCrew (_crew.id,()=>{
			_hasRecruited = true;
			UpdateBuyBtnState();
			this.transform.SetAsLastSibling();
			UIGrid grid =this.transform.parent.GetComponent<UIGrid>();
			grid.gameObject.SetActive(false);
			if(grid != null)
				grid.Reposition();
			grid.gameObject.SetActive(true);
		});
	}

	private void OnClickHintBtn ()
	{
		GameHintManager.Open (_view.hintBtn.gameObject, _crew.description);
	}
}
