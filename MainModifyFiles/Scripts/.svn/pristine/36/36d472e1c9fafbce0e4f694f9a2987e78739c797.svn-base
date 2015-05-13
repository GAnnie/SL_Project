using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class CrewMainViewController : MonoBehaviour,IViewController {

	public const int PLAYERITEM_INDEX = -1;
	public CrewMainView _view;
	private CharacterInfoItemController _playerInfoItem;
	public List<CharacterInfoItemController> _crewItemList;
	public static int NextSelectItemIndex=-1;
	private List<TabBtnController> _rightTabBtnList;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<CrewMainView>();
		_view.Setup(this.transform);

		RegisterEvent();
		InitTabBtn();
		InitCrewItemList();

		OnSelectRightTabBtn(0);
		OnSelectSlotItem(NextSelectItemIndex);
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,CloseView);
		EventDelegate.Set (_view.restOrBattleBtn.onClick,OnClickRestOrBattle);
		CrewModel.Instance.OnAddNewCrew += AddNewCrewItem;
		CrewModel.Instance.OnCrewInfoListUpdate += UpdateCrewItemInfoList;
	}

	public void Dispose ()
	{
		CrewModel.Instance.OnAddNewCrew -= AddNewCrewItem;
		CrewModel.Instance.OnCrewInfoListUpdate -= UpdateCrewItemInfoList;
		if(UIModuleManager.Instance.ContainsModule(ProxyPlayerTeamModule.NAME))
			ProxyPlayerTeamModule.Show();
	}
	#endregion

	#region Init Func
	private void InitTabBtn(){
		_rightTabBtnList = new List<TabBtnController>(3);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.rightTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_rightTabBtnList.Add (com);
			_rightTabBtnList [i].InitItem (i, OnSelectRightTabBtn);
		}
		
		_rightTabBtnList [0].SetBtnName ("伙伴布阵");
		_rightTabBtnList [1].SetBtnName ("我的伙伴");
		_rightTabBtnList [2].SetBtnName ("客  栈");
	}

	//初始化伙伴信息项，index=-1的为玩家本身
	private void InitCrewItemList(){
		GameObject memberItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ProxyPlayerTeamModule.CHARACTERINFO_WIDGET) as GameObject;
		int crewCount = CrewModel.Instance.GetCrewCount();
		_crewItemList = new List<CharacterInfoItemController>(crewCount);
		for (int i=-1;i<crewCount; ++i) {
			GameObject item = NGUITools.AddChild (_view.crewItemGrid.gameObject, memberItemPrefab);
			item.GetMissingComponent<UIDragScrollView>();
			CharacterInfoItemController com = item.GetMissingComponent<CharacterInfoItemController> ();
			com.InitItem (i, OnSelectSlotItem,OnDragStartCrewInfoItem,OnDragEndCrewInfoItem);
			if(i == PLAYERITEM_INDEX){
				_playerInfoItem = com;
				UpdatePlayerItemInfo();
			}else{
				_crewItemList.Add (com);
				UpdateCrewItemInfo(i);
			}
		}
	}

	private void AddNewCrewItem(CrewPropertyInfo crewInfo){
		GameObject memberItemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ProxyPlayerTeamModule.CHARACTERINFO_WIDGET) as GameObject;
		GameObject item = NGUITools.AddChild (_view.crewItemGrid.gameObject, memberItemPrefab);
		item.GetMissingComponent<UIDragScrollView>();
		CharacterInfoItemController com = item.GetMissingComponent<CharacterInfoItemController> ();
		int itemIndex = _crewItemList.Count;
		com.InitItem (itemIndex, OnSelectSlotItem,OnDragStartCrewInfoItem,OnDragEndCrewInfoItem);
		_crewItemList.Add (com);
		UpdateCrewItemInfo(itemIndex);

		if(_crewMatrixController != null)
			_crewMatrixController.UpdateFormationView();
	}
	#endregion

	#region 伙伴界面主面板
	private TabBtnController _lastRightTabBtn;
	private GameObject _lastRightContentRoot;
	private CrewMatrixTabContentController _crewMatrixController;
	private CrewPropertyTabContentController _crewPropertyController;
	private CrewTavernTabContentController _crewTavernController;

	private void OnSelectRightTabBtn(int index){
		if(index == 0)
			OnSelectCrewMatrix();
		else if(index == 1){
			if(_crewItemList.Count > 0)
				OnSelectCrewProperty();
			else{
				TipManager.AddTip("暂无伙伴");
				return;
			}
		}
		else if(index == 2)
			OnSelectCrewTavern();

		if(_lastRightTabBtn != null)
			_lastRightTabBtn.SetSelected(false);
		
		_lastRightTabBtn = _rightTabBtnList[index];
		_lastRightTabBtn.SetSelected(true);
	}

	private void ChangeRightTabContent(GameObject contentRoot){
		_view.BaseInfoGroup.SetActive(true);
		_view.CrewTavernGroup.SetActive(false);
		if(_lastRightContentRoot != null)
			_lastRightContentRoot.SetActive(false);

		contentRoot.SetActive(true);
		_lastRightContentRoot = contentRoot;
	}

	private void SetupTitleSprite(string spriteName){
		_view.contentTitleSprite.spriteName = spriteName;
		_view.contentTitleSprite.MakePixelPerfect();
	}

	private void OnSelectCrewMatrix(){
		SetupTitleSprite("title_crewLayout");
		_view.crewMemberGroupBg.topAnchor.absolute = -38;
		ChangeRightTabContent(_view.TabContent_CrewMatrix);
		_playerInfoItem.gameObject.SetActive(true);
		_view.crewItemGrid.transform.localPosition = new Vector3(0,220f,0);
		_view.crewItemGrid.Reposition();
		if(_crewMatrixController == null)
		{
			_crewMatrixController = _view.TabContent_CrewMatrix.GetMissingComponent<CrewMatrixTabContentController>();
			_crewMatrixController.InitView(this);
		}
	}

	private void OnSelectCrewProperty(){
		SetupTitleSprite("title_crewProperty");
		_view.crewMemberGroupBg.topAnchor.absolute = 0;
		ChangeRightTabContent(_view.TabContent_CrewProperty);
		_playerInfoItem.gameObject.SetActive(false);
		_view.crewItemGrid.transform.localPosition = new Vector3(0,240f,0);
		_view.crewItemGrid.Reposition();
		if(_crewPropertyController == null)
		{
			_crewPropertyController = _view.TabContent_CrewMatrix.GetMissingComponent<CrewPropertyTabContentController>();
			_crewPropertyController.InitView(_view);
			CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(NextSelectItemIndex);
			_crewPropertyController.UpdateViewInfo(crewInfo);
		}

		if(_curSelectCrewItemIndex == PLAYERITEM_INDEX){
			OnSelectSlotItem(0);
		}
	}

	private void OnSelectCrewTavern(){
		SetupTitleSprite("title_crewTavern");
		_view.BaseInfoGroup.SetActive(false);
		_view.CrewTavernGroup.SetActive(true);

		if(_crewTavernController == null){
			_crewTavernController = _view.CrewTavernGroup.GetMissingComponent<CrewTavernTabContentController>();
			_crewTavernController.InitView(_view);
		}
	}

	private int _curSelectCrewItemIndex = -99;
	private void OnSelectSlotItem(int index){

		if(_curSelectCrewItemIndex != -99){
			if(_curSelectCrewItemIndex == PLAYERITEM_INDEX)
				_playerInfoItem.SetSelected(false);
			else
				_crewItemList[_curSelectCrewItemIndex].SetSelected(false);
		}

		_curSelectCrewItemIndex = index;
		NextSelectItemIndex = index;
		if(index == PLAYERITEM_INDEX)
			_playerInfoItem.SetSelected(true);
		else
			_crewItemList[index].SetSelected(true);

		UpdateRestOrBattleBtnState();

		if(index != PLAYERITEM_INDEX && _crewPropertyController != null){
			CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(index);
			_crewPropertyController.UpdateViewInfo(crewInfo);
		}
	}

	private void UpdateCrewItemInfo(int index){
		CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(index);
		_crewItemList[index].ResetItem();
		if(crewInfo != null){
			_crewItemList[index].CanDrag = true;
			_crewItemList[index].SetInfoLbl(string.Format ("{0}\n{1}级{2}", crewInfo.crew.name,PlayerModel.Instance.GetPlayerLevel(),crewInfo.crew.faction.name));
			_crewItemList[index].SetIcon("0");
			if(crewInfo.crewDto.inBattleTeam){
				_crewItemList[index].SetIconFlagState(true);
				_crewItemList[index].SetIndexSprite (index+2);
			}
		}
	}

	private void UpdateCrewItemInfoList ()
	{
		for(int i=0;i<_crewItemList.Count;++i){
			UpdateCrewItemInfo(i);
		}

		if(_crewMatrixController != null)
			_crewMatrixController.OnSelectFormationCaseToggle(PlayerModel.Instance.CurrentSelectFormationCaseIndex);

		OnSelectSlotItem(NextSelectItemIndex);
	}

	private void UpdatePlayerItemInfo(){
		_playerInfoItem.ResetItem();
		PlayerDto playerDto = PlayerModel.Instance.GetPlayer();
		_playerInfoItem.SetInfoLbl(string.Format ("{0}\n{1}级{2}",playerDto.nickname,PlayerModel.Instance.GetPlayerLevel(),playerDto.faction.name));
		_playerInfoItem.SetIcon("0");
		_playerInfoItem.SetIconFlagState(true);
		_playerInfoItem.SetIndexSprite (1);
	}
	#endregion

	#region 上下阵操作
	private void OnClickRestOrBattle(){
		CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(_curSelectCrewItemIndex);
		if(crewInfo.crewDto.inBattleTeam){
			CrewMainViewController.NextSelectItemIndex = _curSelectCrewItemIndex;
			CrewModel.Instance.OffBattle(_curSelectCrewItemIndex);
		}else{
			if(CrewModel.Instance.InBattleCrewCount < CrewModel.MAX_BATTLECOUNT)
				CrewMainViewController.NextSelectItemIndex = Mathf.Clamp(_curSelectCrewItemIndex+1,0,CrewModel.Instance.GetCrewCount()-1);
			else
				CrewMainViewController.NextSelectItemIndex = _curSelectCrewItemIndex;
			
			CrewModel.Instance.JoinBattle(_curSelectCrewItemIndex);
		}
	}
	
	public void UpdateRestOrBattleBtnState ()
	{
		if(_curSelectCrewItemIndex == PLAYERITEM_INDEX){
			_view.restOrBattleBtn.gameObject.SetActive(false);
		}
		else{
			_view.restOrBattleBtn.gameObject.SetActive(true);
			CrewPropertyInfo crewInfo = CrewModel.Instance.GetCrewInfoByIndex(_curSelectCrewItemIndex);
            if(crewInfo.crewDto.inBattleTeam)
                _view.restOrBattleBtnLbl.text = "休  息";
            else
                _view.restOrBattleBtnLbl.text = "参  战";
            
        }
    }
    #endregion

	#region 阵法槽拖拽操作
	public void OnDragStartCrewInfoItem(){
		_view.matrixSelectedEffect.SetActive(true);
	}
	
	public void OnDragEndCrewInfoItem(int dragItemIndex){
		_view.matrixSelectedEffect.SetActive(false);

		CrewPropertyInfo dragCrewItem = CrewModel.Instance.GetCrewInfoByIndex(dragItemIndex);
		FormationItemSlotController hoverFormationItem = UICamera.hoveredObject.GetComponent<FormationItemSlotController>();
		if(hoverFormationItem != null){
			if(hoverFormationItem.CanDrag){
				CrewPropertyInfo hoverCrewItem = CrewModel.Instance.GetCrewInfoByIndex(hoverFormationItem.GetIndex());
				SwithCrewPos(dragCrewItem,hoverCrewItem);
			}else{
				if(!dragCrewItem.crewDto.inBattleTeam && hoverFormationItem.GetIndex() != PLAYERITEM_INDEX){
					CrewModel.Instance.JoinBattle(dragItemIndex);
				}
			}
			return;
		}
		
		CharacterInfoItemController hoverCrewInfoItem = UICamera.hoveredObject.GetComponent<CharacterInfoItemController>();
		if(hoverCrewInfoItem != null && hoverCrewInfoItem.CanDrag)
		{
			CrewPropertyInfo hoverCrewItem = CrewModel.Instance.GetCrewInfoByIndex(hoverCrewInfoItem.GetIndex());
			SwithCrewPos(dragCrewItem,hoverCrewItem);
		}
	}

	private void SwithCrewPos(CrewPropertyInfo dragCrewItem,CrewPropertyInfo hoverCrewItem){
		if(dragCrewItem.crewDto.crewUniqueId == hoverCrewItem.crewDto.crewUniqueId) return;

		if(dragCrewItem.crewDto.inBattleTeam && hoverCrewItem.crewDto.inBattleTeam){
			CrewModel.Instance.SwitchBattlePos(dragCrewItem,hoverCrewItem);
		}else if(dragCrewItem.crewDto.inBattleTeam && !hoverCrewItem.crewDto.inBattleTeam){
			CrewModel.Instance.SwitchOnBattlePos(hoverCrewItem,dragCrewItem);
		}else if(!dragCrewItem.crewDto.inBattleTeam && hoverCrewItem.crewDto.inBattleTeam){
			CrewModel.Instance.SwitchOnBattlePos(dragCrewItem,hoverCrewItem);
		}
	}
	#endregion

	public void CloseView(){
		ProxyCrewModule.Close();
	}
}
