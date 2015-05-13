using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.title.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.title.data;

public class PlayerAppellationViewController : MonoBehaviour,IViewController {

	private PlayerAppellationView _view;
	private List<GameObject> _appellationItemList;
	private List<PlayerTitleDto> _titleDtoList;
	private PlayerTitleDto _curSelectDto;

	#region IViewController implementation
	public void InitView ()
	{
		_titleDtoList = PlayerModel.Instance.GetTitleList();
		_view = gameObject.GetMissingComponent<PlayerAppellationView> ();
		_view.Setup (this.transform);

		RegisterEvent();

		_appellationItemList = new List<GameObject>(_titleDtoList.Count);
		if(_titleDtoList.Count >0){
			int _defaultSelectIndex = 0;
			for(int i=0;i<_titleDtoList.Count;++i){
				GameObject item = i==0?_view.appellationItemPrefab:NGUITools.AddChild (_view.itemGrid.gameObject,_view.appellationItemPrefab);
				UIEventListener eventListener = UIEventListener.Get(item);
				eventListener.onClick = (go)=>{
					int index = _appellationItemList.IndexOf(go);
					if(index != -1){
						OnSelectAppellationItem(index);
					}
				};
				Title titleInfo = DataCache.getDtoByCls<Title>(_titleDtoList[i].titleId);
				if(titleInfo != null){
					UILabel label = item.GetComponentInChildren<UILabel>();
					label.text = titleInfo.name;
				}
				if(_titleDtoList[i].titleId == PlayerModel.Instance.GetPlayer().titleId)
					_defaultSelectIndex = i;
				_appellationItemList.Add(item);
			}

			OnSelectAppellationItem(_defaultSelectIndex);
		}else{
			_view.appellationItemPrefab.SetActive(false);
			_view.descLbl.text = "";
		}
    }

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,OnClickCloseBtn);
		EventDelegate.Set(_view.changBtn.onClick,OnClickChangeBtn);
		EventDelegate.Set(_view.hideBtn.onClick,OnClickHideBtn);
	}

	public void Dispose ()
	{

	}
	#endregion

	private void OnClickChangeBtn(){
		if(BattleManager.Instance.IsInBattle()){
			TipManager.AddTip("战斗中不能更改称谓");
			return;
		}

		if(_curSelectDto != null && _curSelectDto.titleId != PlayerModel.Instance.GetPlayer().titleId)
			ServiceRequestAction.requestServer(TitleService.show(_curSelectDto.titleId),"ShowTitle",(e)=>{
				Title titleInfo = DataCache.getDtoByCls<Title>(_curSelectDto.titleId);
				if(titleInfo != null){
					TipManager.AddTip(string.Format("当前称谓设置为：【{0}】",titleInfo.name.WrapColor(ColorConstant.Color_Title_Str)));
					ProxyPlayerPropertyModule.ClosePlayerAppellationView();
				}

			});
	}

	private void OnClickHideBtn(){
		if(BattleManager.Instance.IsInBattle()){
			TipManager.AddTip("战斗中不能隐藏称谓");
			return;
		}

		ServiceRequestAction.requestServer(TitleService.hidden(),"HideTitle",(e)=>{
			ProxyPlayerPropertyModule.ClosePlayerAppellationView();
		});
	}

	private GameObject _lastSelectGo;
	private void OnSelectAppellationItem(int index){
		if(index < _titleDtoList.Count){
			if(_lastSelectGo != null)
				_lastSelectGo.SetActive(false);
			_lastSelectGo = _appellationItemList[index].transform.Find("selected").gameObject;
			_lastSelectGo.SetActive(true);

			_curSelectDto = _titleDtoList[index];
			Title titleInfo = DataCache.getDtoByCls<Title>(_curSelectDto.titleId);
			_view.descLbl.text = string.Format("称谓描述：\n{0}\n获得方式：\n{1}\n{2}",
			                                   titleInfo.desc,
			                                   titleInfo.access,
			                                   _curSelectDto.expiredTime == 0?"":string.Format("过期时间：\n{0}",DateUtil.getDate(_curSelectDto.expiredTime)));

			_view.descScrollView.ResetPosition();
		}
	}

	private void OnClickCloseBtn(){
		ProxyPlayerPropertyModule.ClosePlayerAppellationView();
	}
}
