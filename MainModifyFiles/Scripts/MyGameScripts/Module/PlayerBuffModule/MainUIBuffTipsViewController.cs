using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.player.data;

public class MainUIBuffTipsViewController : MonoBehaviour,IViewController
{

	MainUIBuffTipsView _view;
	private List<MainUIBuffTipsItemController> _buffItemList;
	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<MainUIBuffTipsView> ();
		_view.Setup (this.transform);

		RegisterEvent ();

		_buffItemList = new List<MainUIBuffTipsItemController>();
		UpdateBuffItemInfo();
	}

	public void RegisterEvent ()
	{
		PlayerBuffModel.Instance.OnPlayerBuffUpdate += UpdateBuffItemInfo;
		EventDelegate.Set (_view.BgEventTrigger.onClick, CloseView);
	}

	public void Dispose ()
	{
		PlayerBuffModel.Instance.OnPlayerBuffUpdate -= UpdateBuffItemInfo;
	}
	#endregion

	private void UpdateBuffItemInfo(){
		List<int> buffInfoList = PlayerBuffModel.Instance.GetActiveBuffList ();
		if(_buffItemList.Count < buffInfoList.Count){
			for(int i=_buffItemList.Count;i<buffInfoList.Count;++i){
				GameObject item = i == 0 ? _view.buffTipsItemPrefab : NGUITools.AddChild (_view.itemTable.gameObject, _view.buffTipsItemPrefab);
				var controller = item.GetMissingComponent<MainUIBuffTipsItemController> ();
				controller.InitItem();
                _buffItemList.Add(controller);
			}
		}

		for(int i=0;i<buffInfoList.Count;++i){
			_buffItemList[i].gameObject.SetActive(true);
			_buffItemList[i].UpdateViewInfo(buffInfoList[i]);
		}

		for(int i=buffInfoList.Count;i<_buffItemList.Count;++i){
			_buffItemList[i].gameObject.SetActive(false);
		}

		_view.itemTable.repositionNow = true;
	}

	private void CloseView ()
	{
		ProxyMainUIModule.CloseBuffTipsView ();
	}
}

public class MainUIBuffTipsItemController : MonoBehaviour
{
	private UISprite _buffIcon;
	private UILabel _titleLbl;
	private UILabel _descLbl;
	private UILabel _statusLbl;
	private UIButton _funBtn;
	private int _stateId;

	public void InitItem ()
	{
		Transform root = this.transform;
		_buffIcon = root.Find ("buffIcon").GetComponent<UISprite> ();
		_titleLbl = root.Find ("buffIcon/buffNameLbl").GetComponent<UILabel> ();
		_descLbl = root.Find ("descLbl").GetComponent<UILabel> ();
		_statusLbl = root.Find ("statusLbl").GetComponent<UILabel> ();
		_funBtn = root.Find ("statusLbl/funBtn").GetComponent<UIButton> ();
		EventDelegate.Set (_funBtn.onClick, OnClickFuncBtn);
	}

	public void UpdateViewInfo(int stateId){
		_stateId = stateId;
		StateBar stateBarInfo = DataCache.getDtoByCls<StateBar>(_stateId);
		_buffIcon.spriteName = stateId.ToString();
		_titleLbl.text = stateBarInfo.name;
		_descLbl.text = stateBarInfo.description;

		_funBtn.gameObject.SetActive(true);
		if (_stateId == PlayerBuffModel.SATIATION_ID) {
			_statusLbl.text = string.Format("可持续：{0}场战斗",PlayerModel.Instance.GetSubWealth().satiation);
		} else if (_stateId == PlayerBuffModel.DOUBLEEXP_ID) {
			_statusLbl.text = string.Format("剩余：{0}点",PlayerModel.Instance.GetDoubleExpDto().openPoint);
		} else if (_stateId == PlayerBuffModel.EQBREAKDOWN_ID) {
			_statusLbl.text = "一键修复所有装备";
		}else if(_stateId == PlayerBuffModel.WORLDJUBILATION_ID){
			_funBtn.gameObject.SetActive(false);
			OnTimerUpdate(CoolDownManager.Instance.GetRemainTime("WorldJubilationStateTimer"));
			CoolDownManager.Instance.AddUpdateHandler("WorldJubilationStateTimer",OnTimerUpdate);
		}else if(_stateId == PlayerBuffModel.TRANSFORM_ID){
			TransformCardSateBarDto stateDto = PlayerBuffModel.Instance.GetStateBarDtoInfo(PlayerBuffModel.TRANSFORM_ID) as TransformCardSateBarDto;
			_statusLbl.text = string.Format("持续时间：{0}",DateUtil.getServerTime(stateDto.expiredTime));
        }
		_statusLbl.GetComponent<UIAnchor>().Update();
	}

	public void OnTimerUpdate(float remainTime){
		_statusLbl.text = string.Format("持续时间：{0}",DateUtil.getVipTime(remainTime));
	}

	private void OnClickFuncBtn ()
	{
		if (_stateId == PlayerBuffModel.SATIATION_ID) {
			ProxyMainUIModule.OpenSatiationPropsUseView();
		} else if (_stateId == PlayerBuffModel.DOUBLEEXP_ID) {
			PlayerModel.Instance.ReceiveDoubleExp();
		} else if (_stateId == PlayerBuffModel.EQBREAKDOWN_ID) {
			ProxyWindowModule.OpenConfirmWindow (string.Format ("修复所有装备需要{0}{1}", EquipmentModel.Instance.GetEquipmentRepairFee (), ItemIconConst.Copper)
			                                    , "", EquipmentModel.Instance.RepairAll);
		}else if(_stateId == PlayerBuffModel.WORLDJUBILATION_ID){
			
		}else if(_stateId == PlayerBuffModel.TRANSFORM_ID){
			
		}
	}
}