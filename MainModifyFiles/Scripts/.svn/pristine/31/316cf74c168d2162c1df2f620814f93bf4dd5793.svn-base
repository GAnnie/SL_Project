using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.data;
using System.Linq;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.title.dto;
using com.nucleus.h1.logic.core.modules.title.data;

public class FacePanelViewController : MonoBehaviour,IViewController {

	private const string FriendBackPackItemCellPath = "Prefabs/Module/FriendModule/FriendBackPackItemCell";     //背包prefab

	private const string FriendPartnerItemCellPath = "Prefabs/Module/FriendModule/FriendPartnerItemCell";     //宠物prefab

	private const string ExpressionItemPath = "Prefabs/Module/FriendModule/ExpressionView";

	private const string HyperlinkItemCellPath = "Prefabs/Module/FriendModule/HyperlinkItemCell";   //称谓 成就 任务 prefab

	private FacePanelView _view;


	List < PackItemDto > _packDto;     //背包数据
	List< PackItemDto> _equip;          //身上装备
	List<PetPropertyInfo> PetPropertyInfoList;         //宠物信息

	List<CrewPropertyInfo> ParnerInfoList;             //伙伴信息


	List<AssistSkillVo> skillList = new List<AssistSkillVo>();           //技能信息
	

	List< PlayerMissionDto >  PlayerMissionDtoList = new List<PlayerMissionDto>();//任务 信息

	List<PlayerTitleDto> titleList = new List<PlayerTitleDto>(); //称谓信息

	Dictionary<int,ExpressionViewController> ExpressionDic = new Dictionary<int, ExpressionViewController>(20);   //表情

	Dictionary< int ,FriendBackPackItemCellController > friendBackPackItemCellDic = new Dictionary<int, FriendBackPackItemCellController>();

	Dictionary< int,FriendPartnerItemCellController > friendPartnerItemCellDic = new Dictionary<int, FriendPartnerItemCellController>();

	Dictionary < int ,HyperlinkItemCellController> hyperlinkItemCellDic = new Dictionary<int, HyperlinkItemCellController>();         //称谓 成就 任务 


	private System.Action<string,int,long,int> backPackItemCallBack = null;           //name,itemId,uniquId

	private System.Action backCloseCallback = null;
	private System.Action<int> ExpressionCallBack = null;
	private System.Action<string,int,int > HyperlinkItemCallBack = null;
	public void InitView(){
		_view = gameObject.GetMissingComponent<FacePanelView> ();
		_view.Setup (this.transform);


		if (CrewModel.Instance.GetCrewCount () <= 0) {
			CrewModel.Instance.RequestCrewInfo(null);
		}


		RegisterEvent ();
	}

	public void SetCallback (System.Action<string,int,long,int> callback, System.Action closeCallBack, System.Action<string,int,int > hyperlinkItemCallBack) {
		backPackItemCallBack = callback;
		backCloseCallback = closeCallBack;

		HyperlinkItemCallBack = hyperlinkItemCallBack;
	}
	public void SetExpressionCallback (System.Action<int> callback) {
		ExpressionCallBack = callback;
		OnExpressionBtnClick();
	}

	public void RegisterEvent(){
		EventDelegate.Set (_view.BgBoxCollider.onClick, OnClickBgBoxCollider);
		EventDelegate.Set (_view.ExpressionBtn.onClick, OnExpressionBtnClick);
		EventDelegate.Set (_view.GoodsBtn.onClick, OnGoodsBtnClick);
		EventDelegate.Set (_view.PetBtn.onClick, OnPetBtnClick);
		EventDelegate.Set (_view.PartnerBtn.onClick, OnPartnerBtnClick);
		EventDelegate.Set (_view.SkillBtn.onClick, OnSkillBtnClick);
		EventDelegate.Set (_view.AppellationBtn.onClick, OnAppellationBtnClick);
		EventDelegate.Set (_view.AssignmentBtn.onClick, OnAssignmentBtnClick);
		EventDelegate.Set (_view.AchievementBtn.onClick, OnAchievementBtnClick);
	}


	#region 表情
	public void OnExpressionBtnClick(){

		//宠物
		if (friendPartnerItemCellDic != null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}

		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}


		if(ExpressionDic.Count >0){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(true);
			}

			_view.GoodsGrid.transform.localPosition = new Vector3 (-267, 92, 0);
			_view.GoodsGrid.maxRow = 4;
			_view.GoodsGrid.maxCol = 10;
			_view.GoodsGrid.cellWidth = 71;
			_view.GoodsGrid.cellHeight = 60;
			_view.ItemScrollView.ResetPosition();
			_view.GoodsGrid.Reposition ();
			return;
		}



		if(ExpressionDic.Count == 0 ){
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(ExpressionItemPath) as GameObject;

			for(int i = 1; i < 21 ; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				ExpressionViewController con = item.GetMissingComponent<ExpressionViewController>();
				con.InitView();
				con.SetData("#"+i,i,ExpressionCallBack);
				ExpressionDic.Add(i-1,con);
			}
		}

		_view.GoodsGrid.transform.localPosition = new Vector3 (-267, 92, 0);
		_view.GoodsGrid.maxRow = 4;
		_view.GoodsGrid.maxCol = 10;
		_view.GoodsGrid.cellWidth = 71;
		_view.GoodsGrid.cellHeight = 60;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();


	}
	#endregion

	#region 表情回调
	public void OnClickExpressionCallBack(int i){
		ExpressionCallBack(i);
	}
	#endregion
	
	#region 物品
	public void OnGoodsBtnClick(){
		isDoubleClick = -1; 
		//宠物
		if (friendPartnerItemCellDic != null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}
		//表情
		if(ExpressionDic != null){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(false);
			}
		}

		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


		_equip = BackpackModel.Instance.GetBodyEquipList ();
		_packDto = BackpackModel.Instance.GetBackPackPropsList ();







		int count = _packDto.Count + _equip.Count;
		int _p = 0;
		int p = 0;
		if (count == 0) {
			_p = 1;		
		}
		else{
			while (count >0) {
				count -=24;
				_p++;
			}
		}


		//如果数量不够
		if (friendBackPackItemCellDic.Count < _p * 24) {
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(FriendBackPackItemCellPath) as GameObject;

			for(int i = friendBackPackItemCellDic.Count; i < _p *24 ; i++){

				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				FriendBackPackItemCellController con = item.GetMissingComponent<FriendBackPackItemCellController>();
				con.InitView();
				friendBackPackItemCellDic.Add (i, con);
			}
		}

		//修改前面信息
		for (int i = 0; i <_equip.Count; i++) {
			friendBackPackItemCellDic[i].SetSelected(false);
			friendBackPackItemCellDic[i].SetData(i,_equip[i],1,OnClickBackPackItemCallBack,true);
			friendBackPackItemCellDic[i].gameObject.SetActive(true);
		}

		for (int i = _equip.Count; i < (_packDto.Count + _equip.Count); i++) {
			friendBackPackItemCellDic[i].SetSelected(false);
			friendBackPackItemCellDic[i].SetData(i,_packDto[i - _equip.Count],1,OnClickBackPackItemCallBack,false);
			friendBackPackItemCellDic[i].gameObject.SetActive(true);
		}

//		//把当前页的显示完
		for (int i = (_equip.Count +_packDto.Count ); i < friendBackPackItemCellDic.Count; i++) {
			friendBackPackItemCellDic[i].gameObject.SetActive(true);	
		}


		
		
		_view.GoodsGrid.transform.localPosition = new Vector3 (-280, 82, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 8;
		_view.GoodsGrid.cellWidth = 78;
		_view.GoodsGrid.cellHeight = 82;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();
	}

	//物品点击回调     //1.物品  2. 技能
	int isDoubleClick = -1;
	public void OnClickBackPackItemCallBack(int i,int type){

		if(isDoubleClick == -1){
			friendBackPackItemCellDic[i].SetSelected(true);
			isDoubleClick = i;
			ShowTips();
			return;
		}
		else if(isDoubleClick != -1){
			if(isDoubleClick == i){
				isDoubleClick = -1;
			}
			else{
				friendBackPackItemCellDic[isDoubleClick].SetSelected(false);
				isDoubleClick = i;
				friendBackPackItemCellDic[i].SetSelected(true);
				ShowTips();
				return;
			}
		}



		//物品
		if (type == 1) {
			if (i >= _equip.Count) {
				backPackItemCallBack (_packDto[i - _equip.Count].item.name,_packDto[i - _equip.Count].itemId,_packDto[i - _equip.Count].uniqueId,type);	
			}
			else{
				backPackItemCallBack (_equip[i].item.name,_equip[i].itemId,_equip[i].uniqueId,type);	
			}	
		}
		//技能
		if(type == 2){
			backPackItemCallBack(skillList[i].name,skillList[i].id,0,type);
		}


	}


	#endregion

	#region 宠物
	public void OnPetBtnClick(){
		isDoubleClick = -1; 
		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}

		//表情
		if(ExpressionDic != null){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(false);
			}
		}

		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


		//把已参战的放最前面
		PetPropertyInfoList = new List<PetPropertyInfo> (PetModel.Instance.GetPetPropertyInfoList ());
		if(PetPropertyInfoList.Count > 0){
			int battlePetIndex = PetModel.Instance.GetBattlePetIndex();
			if(battlePetIndex != -1 && battlePetIndex != 0){
				PetPropertyInfo tempPet = PetPropertyInfoList[battlePetIndex];
				PetPropertyInfoList[battlePetIndex] = PetPropertyInfoList[0];
				PetPropertyInfoList[0] = tempPet;
			}
		}

		//如果数量不够
		if (PetPropertyInfoList != null && friendPartnerItemCellDic.Count < PetPropertyInfoList.Count) {
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(FriendPartnerItemCellPath) as GameObject;
			
			for(int i = friendPartnerItemCellDic.Count; i < PetPropertyInfoList.Count; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				FriendPartnerItemCellController con = item.GetMissingComponent<FriendPartnerItemCellController>();
				con.InitView();
				friendPartnerItemCellDic.Add(i,con);
			}
		}
		
		//修改前面信息
		for (int i = 0; i < PetPropertyInfoList.Count; i++) {

			friendPartnerItemCellDic[i].SetSelected(false);
			friendPartnerItemCellDic[i].SetData(i,PetPropertyInfoList[i],3,OnClickPatItemCallBack,i==0?true:false);
			friendPartnerItemCellDic[i].gameObject.SetActive(true);

		}
		//多余的隐藏掉
		for (int i = PetPropertyInfoList.Count; i <friendPartnerItemCellDic.Count; i++) {
			friendPartnerItemCellDic[i].gameObject.SetActive(false);	
		}


		_view.GoodsGrid.transform.localPosition = new Vector3 (-218, 79, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 3;
		_view.GoodsGrid.cellWidth = 210;
		_view.GoodsGrid.cellHeight = 82;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();
	}

	//宠物 伙伴点击回调
	public void OnClickPatItemCallBack(int i,int type){

		if (type == 3) {

			if(isDoubleClick == -1){
				friendPartnerItemCellDic[i].SetSelected(true);
				isDoubleClick= i;
				ShowTips();
				return;
			}
			else if(isDoubleClick != -1){
				if(isDoubleClick == i){
					isDoubleClick= -1;
				}
				else{
					friendPartnerItemCellDic[isDoubleClick].SetSelected(false);
					 isDoubleClick  = i;
					friendPartnerItemCellDic[i].SetSelected(true);
					ShowTips();
					return;
				}
			}


			backPackItemCallBack (PetPropertyInfoList[i].petDto.name,0,PetPropertyInfoList[i].petDto.id,type);
		}
		else if(type == 4){

			if(isDoubleClick == -1){
				friendPartnerItemCellDic[i].SetSelected(true);
				isDoubleClick = i;
				ShowTips();
				return;
			}
			else if(isDoubleClick != -1){
				if(isDoubleClick == i){
					isDoubleClick  = -1;
				}
				else{
					friendPartnerItemCellDic[isDoubleClick].SetSelected(false);
					isDoubleClick  = i;
					friendPartnerItemCellDic[i].SetSelected(true);
					ShowTips();
					return;
				}
			}

			backPackItemCallBack(ParnerInfoList[i].crew.name,ParnerInfoList[i].crewDto.crewId,ParnerInfoList[i].crewDto.crewUniqueId,type); 
		}

	
	}
	#endregion

	#region 伙伴
	public void OnPartnerBtnClick(){
		isDoubleClick = -1; 
		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}

		//表情
		if(ExpressionDic != null)
		foreach (ExpressionViewController con in ExpressionDic.Values) {
			con.gameObject.SetActive(false);
		}
		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


		ParnerInfoList = new List<CrewPropertyInfo>(CrewModel.Instance.GetAllCrewInfoList ());

		if (ParnerInfoList == null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
			return;
		}


	    //如果数量不够
		if (ParnerInfoList != null && friendPartnerItemCellDic.Count < ParnerInfoList.Count) {
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(FriendPartnerItemCellPath) as GameObject;

			for(int i = friendPartnerItemCellDic.Count; i < ParnerInfoList.Count; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				FriendPartnerItemCellController con = item.GetMissingComponent<FriendPartnerItemCellController>();
				con.InitView();
				friendPartnerItemCellDic.Add(i,con);
			}
		}

	


		//修改前面信息
		for (int i = 0; i < ParnerInfoList.Count; i++) {
			friendPartnerItemCellDic[i].SetSelected(false);

			if(ParnerInfoList[i].crewDto.inBattleTeam){
				friendPartnerItemCellDic[i].SetData(i,ParnerInfoList[i],4,OnClickPatItemCallBack,true);
			}
			else{
				friendPartnerItemCellDic[i].SetData(i,ParnerInfoList[i],4,OnClickPatItemCallBack);
			}
			friendPartnerItemCellDic[i].gameObject.SetActive(true);
		}
		//多余的隐藏掉
		for (int i = ParnerInfoList.Count; i <friendPartnerItemCellDic.Count; i++) {
			friendPartnerItemCellDic[i].gameObject.SetActive(false);	
		}

		_view.GoodsGrid.transform.localPosition = new Vector3 (-218, 79, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 3;
		_view.GoodsGrid.cellWidth = 210;
		_view.GoodsGrid.cellHeight = 82;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();


	}
	#endregion

	#region 技能
	public void OnSkillBtnClick(){
		isDoubleClick = -1; 
		//宠物
		if (friendPartnerItemCellDic != null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}

		//表情
		if(ExpressionDic != null){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(false);
			}
		}

		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}

		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


		skillList.Clear();
		List<AssistSkillVo> tList = AssistSkillModel.Instance.GetAssistSkills();
		for(int i = 0; i < tList.Count; i++){
			if(tList[i].IsShow()){
				skillList.Add(tList[i]);
			}
		}

		//如果数量不够
		if (skillList != null && friendBackPackItemCellDic.Count < skillList.Count) {
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(FriendBackPackItemCellPath) as GameObject;
			
			for(int i = friendBackPackItemCellDic.Count; i < skillList.Count; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				FriendBackPackItemCellController con = item.GetMissingComponent<FriendBackPackItemCellController>();
				con.InitView();
				friendBackPackItemCellDic.Add(i,con);
			}
		}
		
		//修改前面信息
		for (int i = 0; i < skillList.Count; i++) {
			friendBackPackItemCellDic[i].SetSelected(false);
			friendBackPackItemCellDic[i].SetData(i,skillList[i],2,OnClickBackPackItemCallBack);
			friendBackPackItemCellDic[i].gameObject.SetActive(true);
		}
		//多余的隐藏掉
		for (int i = skillList.Count; i <friendBackPackItemCellDic.Count; i++) {
			friendBackPackItemCellDic[i].gameObject.SetActive(false);	
		}

		_view.GoodsGrid.transform.localPosition = new Vector3 (-280, 82, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 8;
		_view.GoodsGrid.cellWidth = 78;
		_view.GoodsGrid.cellHeight = 82;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();

	}
	#endregion

	#region 称谓
	public void OnAppellationBtnClick(){

		isDoubleClick = -1; 
		//宠物
		if (friendPartnerItemCellDic != null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}
		
		//表情
		if(ExpressionDic != null){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(false);
			}
		}
		
		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}

		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


		titleList  = PlayerModel.Instance.GetTitleList();


		if(hyperlinkItemCellDic.Count < titleList.Count ){
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(HyperlinkItemCellPath) as GameObject;
			
			for(int i = hyperlinkItemCellDic.Count; i < titleList.Count; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				HyperlinkItemCellController con = item.GetMissingComponent<HyperlinkItemCellController>();
				con.InitView();
				hyperlinkItemCellDic.Add(i,con);
			}
		}
		
		for(int i = 0 ; i < titleList.Count ; i++){
			hyperlinkItemCellDic[i].SetSelected(false);
			hyperlinkItemCellDic[i].SetData(i,titleList[i],3,OnHyperlinkItemCallBack);
			hyperlinkItemCellDic[i].gameObject.SetActive(true);
		}
		
		
		for(int i = titleList.Count;i < hyperlinkItemCellDic.Count;i++){
			hyperlinkItemCellDic[i].gameObject.SetActive(false);
		}
		
		
		_view.GoodsGrid.transform.localPosition = new Vector3 (-215, 92, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 3;
		_view.GoodsGrid.cellWidth = 210;
		_view.GoodsGrid.cellHeight = 60;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();



	}

	#endregion

	#region 任务
	public void OnAssignmentBtnClick(){
		isDoubleClick = -1; 

		PlayerMissionDtoList = MissionDataModel.Instance.GetPlayerMissionDtoList();


		//物品
		if(friendBackPackItemCellDic != null)
		foreach (FriendBackPackItemCellController con in friendBackPackItemCellDic.Values) {
			con.gameObject.SetActive(false);
		}


		//宠物
		if (friendPartnerItemCellDic != null) {
			foreach(FriendPartnerItemCellController con in friendPartnerItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}
		
		//表情
		if(ExpressionDic != null){
			for(int i = 0 ; i < ExpressionDic.Count; i++){
				ExpressionDic[i].gameObject.SetActive(false);
			}
		}

		//任务
		if(hyperlinkItemCellDic != null){
			foreach (HyperlinkItemCellController con in hyperlinkItemCellDic.Values){
				con.gameObject.SetActive(false);
			}
		}


	
		if(hyperlinkItemCellDic.Count < PlayerMissionDtoList.Count ){
			GameObject Prefab = ResourcePoolManager.Instance.SpawnUIPrefab(HyperlinkItemCellPath) as GameObject;
			
			for(int i = hyperlinkItemCellDic.Count; i < PlayerMissionDtoList.Count; i++){
				GameObject item = NGUITools.AddChild (_view.GoodsGrid.gameObject, Prefab);
				HyperlinkItemCellController con = item.GetMissingComponent<HyperlinkItemCellController>();
				con.InitView();
				hyperlinkItemCellDic.Add(i,con);
			}
		}

		for(int i = 0 ; i < PlayerMissionDtoList.Count ; i++){
			hyperlinkItemCellDic[i].SetSelected(false);
			hyperlinkItemCellDic[i].SetData(i,PlayerMissionDtoList[i],1,OnHyperlinkItemCallBack);
			hyperlinkItemCellDic[i].gameObject.SetActive(true);
		}


		for(int i = PlayerMissionDtoList.Count;i < hyperlinkItemCellDic.Count;i++){
			hyperlinkItemCellDic[i].gameObject.SetActive(false);
		}


		_view.GoodsGrid.transform.localPosition = new Vector3 (-215, 92, 0);
		_view.GoodsGrid.maxRow = 3;
		_view.GoodsGrid.maxCol = 3;
		_view.GoodsGrid.cellWidth = 210;
		_view.GoodsGrid.cellHeight = 60;
		_view.ItemScrollView.ResetPosition();
		_view.GoodsGrid.Reposition ();



	}
	#endregion

	/// <summary>
	/// Raises the hyperlink item call back event.
	/// </summary>
	/// <param name="i">The index.</param>
	/// <param name="type">Type.</param>
	/// type 类型,1.任务 2 . 成就, 3. 称谓
	public void OnHyperlinkItemCallBack(int i,int type){


		switch (type) {
		case 1:

			if(isDoubleClick == -1){
				hyperlinkItemCellDic[i].SetSelected(true);
				isDoubleClick = i;
				ShowTips();
				return;
			}
			else if(isDoubleClick != -1){
				if(isDoubleClick == i){
					isDoubleClick = -1;
				}
				else{
					hyperlinkItemCellDic[isDoubleClick].SetSelected(false);
					isDoubleClick = i;
					hyperlinkItemCellDic[i].SetSelected(true);
					ShowTips();
					return;
				}
			}

			if(HyperlinkItemCallBack != null){
				HyperlinkItemCallBack(PlayerMissionDtoList[i].mission.name,PlayerMissionDtoList[i].mission.id,type);
			}
			break;
		case 2:

			break;
		case 3:

			if(isDoubleClick == -1){
				hyperlinkItemCellDic[i].SetSelected(true);
				isDoubleClick = i;
				ShowTips();
				return;
			}
			else if(isDoubleClick != -1){
				if(isDoubleClick == i){
					isDoubleClick = -1;
				}
				else{
					hyperlinkItemCellDic[isDoubleClick].SetSelected(false);
					isDoubleClick = i;
					hyperlinkItemCellDic[i].SetSelected(true);
					ShowTips();
					return;
				}
			}

			if(HyperlinkItemCallBack != null){
				Title titleInfo = DataCache.getDtoByCls<Title>(titleList[i].titleId);
				HyperlinkItemCallBack(titleInfo.name,titleInfo.id,type);
			}

			break;
		default:
			break;
		}
	}



	#region  成就
	public void OnAchievementBtnClick(){
		TipManager.AddTip ("成就");
	}
	#endregion


	#region 再次点击发送Tips
	public void ShowTips(){

		_view.Tips.gameObject.SetActive(true);
//		TweenAlpha tweenAlpha = UITweener.Begin< TweenAlpha >( _view.Tips.gameObject , 1f);
//		tweenAlpha.from = 0f;
//		tweenAlpha.to   = 255f; 
//		tweenAlpha.PlayForward();

		CoolDownManager.Instance.SetupCoolDown("ShowSendTips",2f,null,HideTips);
	}

	public void HideTips(){
		if(_view.Tips != null)
		_view.Tips.gameObject.SetActive(false);
	}

	#endregion
	
	
	public void Dispose(){

	}


	public void OnClickBgBoxCollider(){
		Destroy (this.gameObject);
		if (backCloseCallback != null) {
			backCloseCallback();
		}
	}

}
