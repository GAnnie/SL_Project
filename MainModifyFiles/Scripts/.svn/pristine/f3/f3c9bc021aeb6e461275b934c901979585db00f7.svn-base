using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.demo.model;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.player.dto;

public class PlayerPropertyViewController : MonoBehaviour,IViewController
{
	PlayerPropertyView _view;
	ModelDisplayController _modelController;
	List<PropertyItemController> _bpItemList;
	List<PropertyItemController> _apItemList;
	private PropertyItemController _ppItemCom;
	private int[] _lastApAddPoint = new int[5]{0,0,0,0,0};	//记录最近一次加点配置
	private PlayerPropertyInfo _playerInfo;

	#region IViewController implementation
	public void InitView ()
	{
		_playerInfo = PlayerModel.Instance.GetPlayerPropertyInfo();
		_view = gameObject.GetMissingComponent<PlayerPropertyView> ();
		_view.Setup (this.transform);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (350,350);
		_modelController.SetBoxCollider(250,250);
		_modelController.SetupModel(PlayerModel.Instance.GetPlayer());

		RegisterEvent ();

		_view.nameLbl.text = _playerInfo.playerDto.name;
		_view.IdLbl.text = _playerInfo.playerDto.playerId.ToString ();
		_view.lvLbl.text = _playerInfo.playerDto.level.ToString ();
		_view.factionLbl.text = _playerInfo.playerDto.faction.name;
		UpdatePlayerTitle();
		OnSubWealthChanged(PlayerModel.Instance.GetSubWealth());

		UpdatePlayerVipFlag();

		_apItemList = new List<PropertyItemController> (5);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (_view.apItemGrid.gameObject, itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.OnAdd += OnApItemAdd;
			controller.OnMinus += OnApItemMinus;
			_apItemList.Add (controller);
		}

		_apItemList [0].InitItem ("体质",_playerInfo.playerDto.aptitudeProperties.constitution+_playerInfo.EqAps[0],false,7);
		_apItemList [1].InitItem ("魔力",_playerInfo.playerDto.aptitudeProperties.intelligent+_playerInfo.EqAps[1],false,8);
		_apItemList [2].InitItem ("力量", _playerInfo.playerDto.aptitudeProperties.strength+_playerInfo.EqAps[2],false,9);
		_apItemList [3].InitItem ("耐力", _playerInfo.playerDto.aptitudeProperties.stamina+_playerInfo.EqAps[3],false,10);
		_apItemList [4].InitItem ("敏捷", _playerInfo.playerDto.aptitudeProperties.dexterity+_playerInfo.EqAps[4],false,11);

		GameObject ppItem = NGUITools.AddChild(_view.apItemGrid.gameObject,itemPrefab);
		_ppItemCom = ppItem.GetMissingComponent<PropertyItemController>();
		_ppItemCom.InitSimpleItem("潜力",_playerInfo.playerDto.potential,12);

		_bpItemList = new List<PropertyItemController> (6);
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (_view.bpItemGrid.gameObject, itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController> ();
			controller.SetEditable(false);
			_bpItemList.Add (controller);
		}

		_bpItemList [0].InitItem ("气血", _playerInfo.hpMax,true,1);
		_bpItemList [1].InitItem ("魔法", _playerInfo.mpMax,true,2);
		_bpItemList [2].InitItem ("攻击", _playerInfo.attack,false,3);
		_bpItemList [3].InitItem ("防御", _playerInfo.defense,false,4);
		_bpItemList [4].InitItem ("速度", _playerInfo.speed,false,5);
		_bpItemList [5].InitItem ("灵力", _playerInfo.magic,false,6);

		ExpGrade expGrade = DataCache.getDtoByCls<ExpGrade> (_playerInfo.playerDto.level+1);
		_view.expLbl.text = string.Format ("{0}/{1}", _playerInfo.playerDto.exp, expGrade.mainCharactorExp);
		_view.expSlider.value = (float)_playerInfo.playerDto.exp / (float)expGrade.mainCharactorExp;

		UpdateApItemOpBtnState ();
	}

	private void UpdatePlayerVipFlag(){
		_view.vipInfoBtn.transform.GetComponent<UISprite> ().isGrey = !PlayerModel.Instance.IsVip();
	}

	private void UpdatePlayerNickName(string nickname)
	{
		_playerInfo.playerDto.name = nickname;
		_view.nameLbl.text = nickname;
	}

	private void UpdatePlayerTitle(){
		_view.appellationLbl.text = PlayerModel.Instance.GetTitleName();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set (_view.closeBtn.onClick, CloseView);
		EventDelegate.Set (_view.vipInfoBtn.onClick, ShowVipInfo);
		EventDelegate.Set (_view.nameInfoBtn.onClick, OpenChangeNameView);
		EventDelegate.Set (_view.levelInfoBtn.onClick, ShowLevelInfo);
		EventDelegate.Set (_view.appellationBtn.onClick, ShowAppellationInfo);
		EventDelegate.Set (_view.energyInfoBtn.onClick, ShowEnergyInfo);
		EventDelegate.Set (_view.factionHintBtn.onClick,ShowFactionHint);

		EventDelegate.Set (_view.ppInfoBtn.onClick, OpenPPDetailView);
		EventDelegate.Set (_view.expSliderBtn.onClick, ShowReserverExp);
		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
		PlayerModel.Instance.OnPlayerNicknameUpdate += UpdatePlayerNickName;
		PlayerModel.Instance.OnPlayerTitleUpdate += UpdatePlayerTitle;
		PlayerModel.Instance.OnPlayerVipUpdate += UpdatePlayerVipFlag;
	}

	private void OnSubWealthChanged (SubWealthNotify notify)
	{
		_view.energyLbl.text = PlayerModel.Instance.Vigour + "/" + PlayerModel.Instance.VigourMax;
	}

	public void Dispose ()
	{
		PlayerModel.Instance.OnPlayerVipUpdate -= UpdatePlayerVipFlag;
		PlayerModel.Instance.OnPlayerTitleUpdate -= UpdatePlayerTitle;
		PlayerModel.Instance.OnPlayerNicknameUpdate -= UpdatePlayerNickName;
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
	}
	#endregion

	public void UpdateViewInfo()
	{
		_ppItemCom.SetValue(_playerInfo.playerDto.potential);
		UpdateApItemInfo(_playerInfo.playerDto.aptitudeProperties.constitution+_playerInfo.EqAps[0],
		                 _playerInfo.playerDto.aptitudeProperties.intelligent+_playerInfo.EqAps[1],
		                 _playerInfo.playerDto.aptitudeProperties.strength+_playerInfo.EqAps[2],
		                 _playerInfo.playerDto.aptitudeProperties.stamina+_playerInfo.EqAps[3],
		                 _playerInfo.playerDto.aptitudeProperties.dexterity+_playerInfo.EqAps[4],
		                 true);
		UpdateBpItemInfo (_playerInfo.hpMax, _playerInfo.mpMax, _playerInfo.attack, _playerInfo.defense, _playerInfo.speed, _playerInfo.magic,true);
		UpdateApItemOpBtnState ();
	}

	private void OpenChangeNameView ()
	{
		ProxyPlayerPropertyModule.OpenPlayerChangeNameView (false);
	}

	private void ShowVipInfo()
	{
		List<string> tips = new List<string>();
		if (PlayerModel.Instance.IsVip())
		{
			string dateStr = DateUtil.getVipDate(PlayerModel.Instance.GetPlayer().vipExpiredTime);
			tips.Add(string.Format("你已经是VIP，到期时间为：{0}，VIP的好处：", dateStr));
		}
		else
		{
			tips.Add ("VIP的好处：");
		}
		tips.Add ("1、人物属性处尊贵VIP金色标记，繁忙优先进服特权");
		tips.Add ("2、每天可领取5000银币，可购买商会贵重道具");
		tips.Add ("3、每天可领取20个还童丹，洗极品宝宝必备道具");

		string tip = string.Join ("\n", tips.ToArray());

		List<string> optionList = new List<string> ();

		List<Vip> vipList = DataCache.getArrayByClsWithoutSort<Vip>();
		foreach(Vip vip in vipList)
		{
			int day = (int)(vip.time / 86400000);
			optionList.Add(string.Format("{0}元宝购买{1}天VIP", vip.ingot, day));
		}

		ProxyWorldMapModule.OpenCommonDialogue ("NPC", tip, optionList, delegate(int index) {
			//index == vipId -1
			PlayerModel.Instance.BuyVip(index+1);
		});
	}

	private void ShowLevelInfo ()
	{
		string nextOpenInfo;
		long nextOpenTime = PlayerModel.Instance.NextServerGradeOpenTime;
		if(nextOpenTime != -1L){
			System.DateTime dt = DataHelper.ConvertIntDateTime(nextOpenTime);
			nextOpenInfo = string.Format("下次服务器开放等级时间：[5cf37c]{0:yyyy-MM-dd} 00:00[-]", dt);
		}
		else{
			if(PlayerModel.Instance.GetPlayerLevel()<PlayerModel.Instance.ServerGrade+8){
				nextOpenInfo ="已达服务器最高等级上限";
			}else{
				nextOpenInfo =string.Format("已达服务器最高等级上限\n你的人物等级已满级，获取{0}将转换为{1}",ItemIconConst.Exp,ItemIconConst.Copper);
			}
		}

		string info = string.Format("当前服务器等级：{0}\n{1}",PlayerModel.Instance.ServerGrade.ToString().WrapColor("5cf37c"),nextOpenInfo);
		GameHintManager.Open(_view.levelInfoBtn.gameObject,info);
	}

	private void ShowAppellationInfo(){
		ProxyPlayerPropertyModule.OpenPlayerAppellationView();
	}

	private void ShowEnergyInfo ()
	{
		//TipManager.AddTip("活力信息");
		ProxyPlayerPropertyModule.OpenVigourWin();
	}

	private void ShowFactionHint(){
		GameHintManager.Open(_view.factionHintBtn.gameObject,GameHintManager.GetFactionHintString(PlayerModel.Instance.GetPlayer().factionId));
	}

	#region 设置自动配点方案
	private PPDetailViewController _ppDetailController;
	private void OpenPPDetailView ()
	{
		if (PlayerModel.Instance.HasCustomAptitude ()) {
			_ppDetailController = ProxyPlayerPropertyModule.OpenPPDetailView (new Vector3 (346f, 20f, 0f));
			_ppDetailController.Open (_playerInfo.playerDto.onUpgradeAptitudeProperties);
			_ppDetailController.OnSave +=OnSaveAutoApDistribution;
			_ppDetailController.OnCancel += OnCancelAutoApDistribution;
			
		} else
			TipManager.AddTip ("40级后属性点重置，可在界面手动增加");
	}

	private void OnSaveAutoApDistribution(){
//		ServiceRequestAction.requestServer(PlayerService.configAutoAssignAptitude(
//			_ppDetailController.apItemList[0].GetValue(),
//			_ppDetailController.apItemList[1].GetValue(),
//			_ppDetailController.apItemList[2].GetValue(),
//			_ppDetailController.apItemList[3].GetValue(),
//			_ppDetailController.apItemList[4].GetValue()
//			),"更新主角自动加点方案",(e)=>{
//			_ppDetailController.UpdateApInfoDto();
//			_ppDetailController.CloseView();
//
//			PlayerModel.AutoDistrubuteApPoint(_playerInfo.playerDto);
//			PlayerModel.Instance.CalculatePlayerBp();
//			UpdateViewInfo();
//
//			TipManager.AddTip("自动加点方案设置成功，相应的加点方案会在升级时自动分配");
//		});
	}

	private void OnCancelAutoApDistribution(){
//		ServiceRequestAction.requestServer(PlayerService.cancelAutoAssignAptitude(),"清空自动加点方案",(e)=>{
//			for(int i=0;i<_ppDetailController.apItemList.Count;++i){
//				_ppDetailController.apItemList[i].SetValue(0,true);
//			}
//			_ppDetailController.UpdateApInfoDto();
//		});
	}
	#endregion

	#region 重置资质属性点
	private void RestPlayerApPoint ()
	{
		ServiceRequestAction.requestServer (PlayerService.resetAptitude (), "重置人物资质属性", (e) => {
			PlayerModel.Instance.RestPlayerApPoint ();
			UpdateViewInfo();
		});
	}
	#endregion
	
	#region 更改资质属性点
	private void ConfirmPlayerAp ()
	{
		if (_ppItemCom.GetValue() != _playerInfo.playerDto.potential) {
			
			ServiceRequestAction.requestServer (PlayerService.increaseAptitude (_lastApAddPoint [0],
			                                                                                           _lastApAddPoint [1],
			                                                                                           _lastApAddPoint [2],
			                                                                                           _lastApAddPoint [3],
			                                                                                           _lastApAddPoint [4]),
			                                    "更改人物资质属性", (e) => {
				CleanUpAddPointList();
				PlayerModel.Instance.UpdatePlayerAp (_ppItemCom.GetValue(),
			                                         _apItemList [0].GetValue ()-_playerInfo.EqAps[0],
				                                     _apItemList [1].GetValue ()-_playerInfo.EqAps[1],
				                                     _apItemList [2].GetValue ()-_playerInfo.EqAps[2],
				                                     _apItemList [3].GetValue ()-_playerInfo.EqAps[3],
				                                     _apItemList [4].GetValue ()-_playerInfo.EqAps[4]);
				UpdateViewInfo();
			});
		}
	}
	#endregion

	private void OnApItemAdd (PropertyItemController item)
	{
		int index = _apItemList.IndexOf (item);
		int val = item.GetValue () + 1;
		if (_ppItemCom.GetValue() - 1 >= 0) {
			item.SetValue (val,false);
			_ppItemCom.SetValue(_ppItemCom.GetValue() - 1);
			_lastApAddPoint [index] += 1;
			CalculatePreviewBattleProperties ();
		}
	}
	
	private void OnApItemMinus (PropertyItemController item)
	{
		int index = _apItemList.IndexOf (item);
		int point = _ppItemCom.GetValue() + 1;
		int val = item.GetValue () - 1;
		if (_lastApAddPoint [index] - 1 >= 0) {
			item.SetValue (val,false);
			_ppItemCom.SetValue(point);
			_lastApAddPoint [index] -= 1;
			CalculatePreviewBattleProperties ();
		}
	}
	
	private void UpdateApItemOpBtnState ()
	{
		if (_playerInfo.playerDto.potential > 0) {
			for (int i=0; i<_apItemList.Count; ++i) {
				_apItemList [i].SetEditable (true);
			}
			_view.apItemOpBtn.gameObject.SetActive (true);
			_view.apItemOpBtn.GetComponentInChildren<UILabel> ().text = "更改属性";
			EventDelegate.Set (_view.apItemOpBtn.onClick, ConfirmPlayerAp);
		}else{
			_view.apItemOpBtn.gameObject.SetActive (false);
			for (int i=0; i<_apItemList.Count; ++i) {
				_apItemList [i].SetEditable (false);
			}
		}

		if (_playerInfo.playerDto.level >= DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POTENTIAL_MIN_LEVEL, 40) 
			&& !PlayerModel.Instance.HasCustomAptitude ()) {
			_view.apItemOpBtn.gameObject.SetActive (true);
			EventDelegate.Set (_view.apItemOpBtn.onClick, RestPlayerApPoint);
		}
	}

	private void UpdateBpItemInfo (int hp, int mp, int attack, int defense, int speed, int magic,bool needReset)
	{
		_bpItemList [0].SetValue (hp,needReset);
		_bpItemList [1].SetValue (mp,needReset);
		_bpItemList [2].SetValue (attack,needReset);
		_bpItemList [3].SetValue (defense,needReset);
		_bpItemList [4].SetValue (speed,needReset);
		_bpItemList [5].SetValue (magic,needReset);
	}

	private void UpdateApItemInfo(int constitution,int intelligent,int strength,int stamina,int dexterity,bool needReset){
		_apItemList [0].SetValue (constitution,needReset);
		_apItemList [1].SetValue (intelligent,needReset);
		_apItemList [2].SetValue (strength,needReset);
		_apItemList [3].SetValue (stamina,needReset);
		_apItemList [4].SetValue (dexterity,needReset);
	}

	private void CalculatePreviewBattleProperties ()
	{
		float hp = _playerInfo.hpMax;
		float attack = _playerInfo.attack;
		float defense = _playerInfo.defense;
		float speed = _playerInfo.speed;
		float magic = _playerInfo.magic;

		for (int index=0; index<_lastApAddPoint.Length; ++index) {
			int addPoint = _lastApAddPoint [index];
			if (addPoint > 0) {
				AptitudePropertyInfo apDataInfo = DataCache.getDtoByCls<AptitudePropertyInfo> (index + 1);

				hp += apDataInfo.hp * addPoint;
				attack += apDataInfo.attack * addPoint;
				defense += apDataInfo.defense * addPoint;
				speed += apDataInfo.speed * addPoint;
				magic += apDataInfo.magic * addPoint;
			}
		}

		UpdateBpItemInfo (Mathf.FloorToInt (hp),
	                      _playerInfo.mpMax,
	                      Mathf.FloorToInt (attack),
	                      Mathf.FloorToInt (defense),
	                      Mathf.FloorToInt (speed),
	                      Mathf.FloorToInt (magic),
		                  false);
	}

	private int GetApAddPointTotalCount ()
	{
		int result = 0;
		for (int i=0; i<_lastApAddPoint.Length; ++i) {
			result += _lastApAddPoint [i];
		}

		return result;
	}

	private void CleanUpAddPointList(){
		for(int i=0;i<_lastApAddPoint.Length;++i){
			_lastApAddPoint[i] = 0;
		}
	}

	private void ShowReserverExp()
	{
		GameHintManager.Open(_view.expSliderBtn.gameObject,string.Format("储备经验: {0}", PlayerModel.Instance.ReserveExp) + "\n" + "储备经验根据获得人物经验数量100%转换",GameHintViewController.Side.Right);
	}

	public void UpdateHpValue(int currentVal, int maxVal)
	{
		_bpItemList [0].SetValue (currentVal,maxVal);
	}

	public void UpdateMpValue(int currentVal, int maxVal)
	{
		_bpItemList [1].SetValue (currentVal,maxVal);
	}

	public void CloseView ()
	{
		ProxyPlayerPropertyModule.Close ();
	}
}
