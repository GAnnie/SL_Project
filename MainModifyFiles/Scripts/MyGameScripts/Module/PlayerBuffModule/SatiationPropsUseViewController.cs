using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class SatiationPropsUseViewController : MonoBehaviour,IViewController {

	private SatiationPropsUseView _view;
	private List<BaseItemCellController> _propsSlotList;
	private List<PackItemDto> _packItemDtoList;
	private long _useCopper;
	#region IViewController implementation

	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<SatiationPropsUseView>();
		_view.Setup(this.transform);

		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		_packItemDtoList = BackpackModel.Instance.GetSatiationProps();
		_propsSlotList = new List<BaseItemCellController>(3);
		for(int i=0;i<3;++i){
			GameObject item = NGUITools.AddChild(_view.itemGrid.gameObject,itemCellPrefab);
			BaseItemCellController com = item.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPropsSlot);
			_propsSlotList.Add(com);
        }

		UpdateViewInfo();
		RegisterEvent();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.BgEventTrigger.onClick,CloseView);
		EventDelegate.Set(_view.useCopperBtn.onClick,OnClickUseCopperBtn);
		EventDelegate.Set(_view.hintBtn.onClick,OnClickHintBtn);

		BackpackModel.Instance.OnUpdateItem += UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem += RemovePropsItem;
		PlayerModel.Instance.OnSubWealthChanged += OnSatiationChange;
	}

	public void Dispose ()
	{
		BackpackModel.Instance.OnUpdateItem -= UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem -= RemovePropsItem;
		PlayerModel.Instance.OnSubWealthChanged -= OnSatiationChange;
	}
	#endregion

	private void UpdatePropsItemInfo (PackItemDto itemDto){
		for(int i=0;i<_packItemDtoList.Count;++i){
			if(_packItemDtoList[i].index == itemDto.index){
				_packItemDtoList[i]=itemDto;
				_propsSlotList[i].SetIcon(itemDto.item.icon);
				_propsSlotList[i].SetIconLbl(itemDto.count.ToString());
				return;
			}
		}
	}
	
	private void RemovePropsItem(int itemIndex){
		int removeIndex = -1;
		for(int i=0;i<_packItemDtoList.Count;++i){
			if(_packItemDtoList[i].index == itemIndex){
				removeIndex = i;
				break;
			}
		}
		
		if(removeIndex != -1){
			_packItemDtoList.RemoveAt(removeIndex);
			UpdateViewInfo();
		}
	}

	private void OnSatiationChange(SubWealthNotify notify){
		if(PlayerModel.Instance.isFullSatiation())
			CloseView();
		else
			UpdateViewInfo();
	}

	private void UpdateViewInfo(){
		_view.tipLbl.text = string.Format("补充{0}场饱食",PlayerModel.Instance.MaxSatiationVal-PlayerModel.Instance.GetSubWealth().satiation);
		_useCopper = PlayerModel.Instance.GetReplenishSatiationFee();
		_view.btnLbl.text = _useCopper.ToString();

		for(int i=0;i<_propsSlotList.Count;++i){
			_propsSlotList[i].ResetItem();
			if(i<_packItemDtoList.Count){
				_propsSlotList[i].SetIcon(_packItemDtoList[i].item.icon);
				_propsSlotList[i].SetIconLbl(_packItemDtoList[i].count.ToString());
			}
		}
	}

	private void OnSelectPropsSlot(int index){
		if(index < _packItemDtoList.Count){
			ProxyItemTipsModule.Open(_packItemDtoList[index],_propsSlotList[index].gameObject,true,null);
		}
	}

	private void OnClickUseCopperBtn(){
		if(PlayerModel.Instance.isEnoughCopper(_useCopper,true)){
			ServiceRequestAction.requestServer(PlayerService.fillSatiation(),"FillSatiation",(e)=>{
				TipManager.AddTip(string.Format("消耗{0}铜币,饱食增加至{1}场",_useCopper,PlayerModel.Instance.MaxSatiationVal));
			});
		}
	}

	private void OnClickHintBtn(){
		string hint = string.Format("饱食最多补充至{0}场",PlayerModel.Instance.MaxSatiationVal);
		GameHintManager.Open(_view.hintBtn.gameObject,hint);
	}

	private void CloseView(){
		ProxyMainUIModule.CloseSatiationPropsUseView();
		ProxyItemTipsModule.Close();
	}
}
