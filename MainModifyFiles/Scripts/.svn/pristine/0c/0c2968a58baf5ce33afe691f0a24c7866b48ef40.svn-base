using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerDyeViewController : MonoBehaviour,IViewController {

	private PlayerDyeView _view;
	private ModelDisplayController _modelController;
	private List<BaseItemCellController> _propsItemList;
	private int[] _propsCountList;
	private int[] _needCountList;
	private Props[] _propsInfoList;

	private Dictionary<int,List<DyeOptionItemController>> _dyeOptionItemDic;
	private Dictionary<int,DyeOptionItemController> _lastSelectOptionDic;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PlayerDyeView>();
		_view.Setup(this.transform);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (350,350);
		_modelController.SetBoxCollider(250,250);
		_modelController.SetupModel(PlayerModel.Instance.GetPlayer());
        
		RegisterEvent();

		//初始化染色道具信息
		_propsItemList = new List<BaseItemCellController>(2);
		_propsCountList = new int[2];
		_needCountList = new int[2];
		_propsInfoList = new Props[2];
		_propsInfoList[0] = DataCache.getDtoByCls<GeneralItem>(DataHelper.GetStaticConfigValue(H1StaticConfigs.PROPS_ID_ONE,10025)) as Props;
		_propsInfoList[1] = DataCache.getDtoByCls<GeneralItem>(DataHelper.GetStaticConfigValue(H1StaticConfigs.PROPS_ID_TWO,10026)) as Props;
		GameObject baseItemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		for(int i=0;i<2;++i){
			GameObject item = NGUITools.AddChild(_view.propsItemAnchor,baseItemCellPrefab);
			BaseItemCellController com = item.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPropsItem);
			_propsItemList.Add(com);

			_propsItemList[i].SetIconLblAnchor(UIWidget.Pivot.Right);
			_propsItemList[i].SetIcon(_propsInfoList[i].icon);
			_propsItemList[i].SetItemLbl(_propsInfoList[i].name);
			_propsCountList[i] = BackpackModel.Instance.GetItemCount(_propsInfoList[i].id);
		}

		//初始化配色按钮列表
		_dyeOptionItemDic = new Dictionary<int, List<DyeOptionItemController>>(3);
		_lastSelectOptionDic = new Dictionary<int, DyeOptionItemController>(3);
		var colorSchemeDic = PlayerModel.Instance.GetColorScheme();
		if(colorSchemeDic != null){
			//头发配色列表
			var partDyeList = colorSchemeDic[Dye.DyePartTypeEnum_Hair];
			_dyeOptionItemDic[Dye.DyePartTypeEnum_Hair] = new List<DyeOptionItemController>(partDyeList.Count);
			for(int i=0;i<partDyeList.Count;++i){
				GameObject item = i==0?_view.dyeOptionItemPrefab:NGUITools.AddChild(_view.headOptionGrid.gameObject,
				                                                                    _view.dyeOptionItemPrefab);
				var com = item.GetMissingComponent<DyeOptionItemController>();
				com.InitItem(partDyeList[i],OnSelectDyeOptionItem);
				_dyeOptionItemDic[Dye.DyePartTypeEnum_Hair].Add(com);

				if(PlayerModel.Instance.GetPlayer().hairDyeId == partDyeList[i].id)
				{
					com.SetSelected(true);
					_lastSelectOptionDic[Dye.DyePartTypeEnum_Hair] = com;
				}
			}

			//衣服配色列表
			partDyeList = colorSchemeDic[Dye.DyePartTypeEnum_Clothes];
			_dyeOptionItemDic[Dye.DyePartTypeEnum_Clothes] = new List<DyeOptionItemController>(partDyeList.Count);
			for(int i=0;i<partDyeList.Count;++i){
				GameObject item = NGUITools.AddChild(_view.clothOptionGrid.gameObject,_view.dyeOptionItemPrefab);
				var com = item.GetMissingComponent<DyeOptionItemController>();
				com.InitItem(partDyeList[i],OnSelectDyeOptionItem);
				_dyeOptionItemDic[Dye.DyePartTypeEnum_Clothes].Add(com);

				if(PlayerModel.Instance.GetPlayer().dressDyeId == partDyeList[i].id)
				{
					com.SetSelected(true);
					_lastSelectOptionDic[Dye.DyePartTypeEnum_Clothes] = com;
				}
			}

			//饰物配色列表
			partDyeList = colorSchemeDic[Dye.DyePartTypeEnum_Ornaments];
			_dyeOptionItemDic[Dye.DyePartTypeEnum_Ornaments] = new List<DyeOptionItemController>(partDyeList.Count);
			for(int i=0;i<partDyeList.Count;++i){
				GameObject item = NGUITools.AddChild(_view.decorationOptionGrid.gameObject,_view.dyeOptionItemPrefab);
				var com = item.GetMissingComponent<DyeOptionItemController>();
				com.InitItem(partDyeList[i],OnSelectDyeOptionItem);
				_dyeOptionItemDic[Dye.DyePartTypeEnum_Ornaments].Add(com);

				if(PlayerModel.Instance.GetPlayer().accoutermentDyeId == partDyeList[i].id)
				{
					com.SetSelected(true);
					_lastSelectOptionDic[Dye.DyePartTypeEnum_Ornaments] = com;
				}
			}

			UpdatePropsInfo();
			UpdateModelHSV();
		}
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,CloseView);
		EventDelegate.Set(_view.randomBtn.onClick,OnClickRandomBtn);
		EventDelegate.Set(_view.resetBtn.onClick,OnClickResetBtn);
		EventDelegate.Set(_view.confirmBtn.onClick,OnClickConfirmBtn);
	}
	public void Dispose ()
	{
	}
	#endregion

	private void OnSelectPropsItem(int index){
		int itemId = 0;
		if(index == 0){
			itemId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PROPS_ID_ONE,10025);
		}else if(index == 1){
			itemId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PROPS_ID_TWO,10026);
		}
		ProxyItemTipsModule.Open(itemId,_propsItemList[index].gameObject);
	}

	private void OnSelectDyeOptionItem(DyeOptionItemController selectItem){
		Dye dyeInfo = selectItem.DyeInfo;
		if(_lastSelectOptionDic[dyeInfo.dyePartType] != null){
			_lastSelectOptionDic[dyeInfo.dyePartType].SetSelected(false);
		}
		selectItem.SetSelected(true);
		_lastSelectOptionDic[dyeInfo.dyePartType] = selectItem;

		UpdatePropsInfo();
		UpdateModelHSV();
	}

	private void UpdatePropsInfo(){
		int propsCount1=0;
		int propsCount2=0;

		foreach(var optionItem in _lastSelectOptionDic){
			Dye dye = optionItem.Value.DyeInfo;
			if(dye.id != PlayerModel.Instance.GetPartDyeId(optionItem.Key)){
				propsCount1 += dye.propsCount1;
				propsCount2 += dye.propsCount2;
			}
		}

		string countStr = string.Format("{0}/{1}",_propsCountList[0],propsCount1);
		_propsItemList[0].SetIconLbl(_propsCountList[0] >= propsCount1 ? countStr:countStr.WrapColor("fc7b6a"));
		_needCountList[0] = propsCount1;

		countStr = string.Format("{0}/{1}",_propsCountList[1],propsCount2);
		_propsItemList[1].SetIconLbl(_propsCountList[1] >= propsCount2 ? countStr:countStr.WrapColor("fc7b6a"));
		_needCountList[1] = propsCount2;
	}

	private void UpdateModelHSV(){
		int hairId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Hair].DyeInfo.id;
		int clothId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Clothes].DyeInfo.id;
		int decorationId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Ornaments].DyeInfo.id;
		string colorParams = PlayerModel.Instance.GetDyeColorParams(hairId,clothId,decorationId);

		_modelController.ChangeModelColorParams(colorParams);
	}

	private void OnClickRandomBtn(){
		foreach(var list in _dyeOptionItemDic.Values){
			var selectItem = list.Random();
			OnSelectDyeOptionItem(selectItem);
		}
	}

	private void OnClickResetBtn(){
		foreach(var pair in _dyeOptionItemDic){
			var list = pair.Value;
			for(int i=0;i<list.Count;++i){
				if(list[i].DyeInfo.id == PlayerModel.Instance.GetPartDyeId(pair.Key)){
					OnSelectDyeOptionItem(list[i]);
					break;
				}
			}
		}
	}

	private void OnClickConfirmBtn(){
		bool hasChange = false;
		//检验染色方案是否有变更
		foreach(var optionItem in _lastSelectOptionDic.Values){
			if(optionItem.DyeInfo.id != PlayerModel.Instance.GetPartDyeId(optionItem.DyeInfo.dyePartType)){
				hasChange = true;
				break;
			}
		}

		if(hasChange){
			int useIngotCount = 0;
			List<ItemDto> itemDtoList = new List<ItemDto>(_propsCountList.Length);
			for(int i=0;i<_propsCountList.Length;++i){
				if(_propsCountList[i] < _needCountList[i]){
					useIngotCount += _propsInfoList[i].buyPrice*(_needCountList[i]-_propsCountList[i]);
					ItemDto item = new ItemDto();
					item.itemId = _propsInfoList[i].id;
					item.itemCount = _needCountList[i];
					itemDtoList.Add(item);
				}
			}

			if(useIngotCount > 0){
				ProxyTipsModule.Open("购买染色道具",itemDtoList,useIngotCount,(list)=>{
					RequestServer();
				});
			}else
				RequestServer();
		}else
			TipManager.AddTip("你的染色方案没有任何变化");
	}

	private void RequestServer(){
		int hairId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Hair].DyeInfo.id;
		int clothId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Clothes].DyeInfo.id;
		int decorationId = _lastSelectOptionDic[Dye.DyePartTypeEnum_Ornaments].DyeInfo.id;
		ServiceRequestAction.requestServer(PlayerService.dye(clothId,hairId,decorationId),"PlayerDye",(e)=>{
			PlayerModel.Instance.UpdateDyeIds(hairId,clothId,decorationId);
			CloseView();
		});
	}

	private void CloseView(){
		ProxyPlayerPropertyModule.ClosePlayerDyeView();
	}
}

public class DyeOptionItemController : MonoBehaviour{

	private GameObject _selectFlag;
//	private UILabel _nameLbl;
	private System.Action<DyeOptionItemController> _onSelectCallback;
	private Dye _dyeInfo;
	public Dye DyeInfo {
		get {
			return _dyeInfo;
		}
	}

	public void InitItem(Dye dyeInfo,System.Action<DyeOptionItemController> onSelectCallback){
		_selectFlag = this.transform.Find("selectFlag").gameObject;
//		_nameLbl = this.transform.Find("nameLbl").GetComponent<UILabel>();

		if(!string.IsNullOrEmpty(dyeInfo.btnColor)){
			UISprite bgSprite = this.GetComponent<UISprite>();
			bgSprite.spriteName = dyeInfo.btnColor;
		}

		UIButton btn = gameObject.GetComponent<UIButton>();
		EventDelegate.Set(btn.onClick,OnSelectItem);

		SetSelected(false);
		_dyeInfo = dyeInfo;
//		_nameLbl.text = dyeInfo.id.ToString();
		_onSelectCallback = onSelectCallback;
	}

	public void OnSelectItem(){
		if(_onSelectCallback != null){
			_onSelectCallback(this);
		}
	}

	public void SetSelected(bool b){
		_selectFlag.SetActive(b);
	}
}
