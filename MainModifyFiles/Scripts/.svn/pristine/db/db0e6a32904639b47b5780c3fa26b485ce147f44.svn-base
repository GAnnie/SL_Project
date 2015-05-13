using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.player.data;

public class PlayerBaseInfoMainViewController : MonoBehaviour,IViewController
{
	private const string ADDPOINT_SLIDER_ITEM = "Prefabs/Module/PlayerPropertyModule/AddPointSlider";
	private PlayerBaseInfoMainView _view;
	private ModelDisplayController _modelController;
	private PlayerPropertyInfo _playerInfo;
	#region IViewController implementation
	public void Open (int defaultRightTab)
	{
		InitView ();

		OnSelectRightTabBtn (defaultRightTab);
	}

	public void InitView ()
	{
		_playerInfo = PlayerModel.Instance.GetPlayerPropertyInfo ();
		_view = gameObject.GetMissingComponent<PlayerBaseInfoMainView> ();
		_view.Setup (this.transform);

		_view.BaseInfoGroup.SetActive (false);
		_view.AddPointInfoGroup.SetActive (false);

		_modelController = ModelDisplayController.GenerateUICom (_view.modelAnchor);
		_modelController.Init (350, 350);
//		_modelController.SetBoxCollider(250,250);
		_modelController.SetupModel (PlayerModel.Instance.GetPlayer ());

		InitTabBtn ();
		InitBaseInfoGroup ();
		InitAddPointInfoGroup ();
		RegisterEvent ();
	}

	public void RegisterEvent ()
	{
		//BaseInfoGroup
		EventDelegate.Set (_view.closeBtn.onClick, CloseView);
		EventDelegate.Set (_view.vipBtn.onClick, ShowVipInfo);
		EventDelegate.Set (_view.changeNameBtn.onClick, OpenChangeNameView);
		EventDelegate.Set (_view.factionBtn.onClick, ShowFactionHint);
		EventDelegate.Set (_view.appellationBtn.onClick, ShowAppellationInfo);
		EventDelegate.Set (_view.energyBtn.onClick, ShowEnergyInfo);
		EventDelegate.Set (_view.expInfoBtn.onClick, ShowExpInfo);

		//AddPointInfoGroup
		EventDelegate.Set (_view.curPlanInfoBtn.onClick, TogglePlanInfoGroup);
		EventDelegate.Set (_view.planActiveBtn.onClick, OnClickActiveBtn);
		EventDelegate.Set (_view.recommendBtn.onClick, ToggleRecTipPanel);
		EventDelegate.Set (_view.resetPointBtn.onClick, OnClickResetPointBtn);
		EventDelegate.Set (_view.confirmBtn.onClick, OnClickConfirmBtn);

		PlayerModel.Instance.OnPlayerNicknameUpdate += UpdatePlayerNickName;
		PlayerModel.Instance.OnPlayerTitleUpdate += UpdatePlayerAppellation;
		PlayerModel.Instance.OnPlayerExpUpdate += UpdateExpInfo;
		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
		PlayerModel.Instance.OnPlayerVipUpdate += UpdatePlayerVipFlag;
	}

	public void Dispose ()
	{
		PlayerModel.Instance.OnPlayerVipUpdate -= UpdatePlayerVipFlag;
		PlayerModel.Instance.OnPlayerNicknameUpdate -= UpdatePlayerNickName;
		PlayerModel.Instance.OnPlayerTitleUpdate -= UpdatePlayerAppellation;
		PlayerModel.Instance.OnPlayerExpUpdate -= UpdateExpInfo;
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
	}
	#endregion

	#region RightTabBtn
	private List<TabBtnController> _rightTabBtnList;
	private TabBtnController _lastRightTabBtn;
	private GameObject _lastRightContentRoot;

	private void InitTabBtn ()
	{
		_rightTabBtnList = new List<TabBtnController> (2);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i=0; i<2; ++i) {
			GameObject item = NGUITools.AddChild (_view.rightTabBtnGrid.gameObject, tabBtnPrefab);
			
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_rightTabBtnList.Add (com);
			_rightTabBtnList [i].InitItem (i, OnSelectRightTabBtn);
		}
		
		_rightTabBtnList [0].SetBtnName ("属   性");
		_rightTabBtnList [1].SetBtnName ("加   点");
	}

	private void OnSelectRightTabBtn (int index)
	{
		if (_lastRightTabBtn != null)
			_lastRightTabBtn.SetSelected (false);
		
		_lastRightTabBtn = _rightTabBtnList [index];
		_lastRightTabBtn.SetSelected (true);

		if (index == 0)
			ChangeRightTabContent (_view.BaseInfoGroup, "title_playerBaseInfo");
		else if (index == 1)
			ChangeRightTabContent (_view.AddPointInfoGroup, "title_playerAddPoint");
	}

	private void ChangeRightTabContent (GameObject contentRoot, string titleSpriteName)
	{
		if (_lastRightContentRoot != null)
			_lastRightContentRoot.SetActive (false);
		
		contentRoot.SetActive (true);
		_lastRightContentRoot = contentRoot;
		_view.titleSprite.spriteName = titleSpriteName;
		_view.titleSprite.MakePixelPerfect ();
	}
	#endregion

	#region BaseInfoGroup
	private List<PropertySliderController> _propertySliderList;
	private List<PropertyInfoController> _bpItemList;
	private List<PropertyInfoController> _apItemList;

	private void InitBaseInfoGroup ()
	{
		_view.lvLbl.text = string.Format ("Lv.{0}", _playerInfo.playerDto.level);
		_view.factionBtn.normalSprite = string.Format ("factionIcon_{0}", _playerInfo.playerDto.factionId);
		_view.idLbl.text = string.Format ("ID {0}", _playerInfo.playerDto.id);
		_view.guildLbl.text = "帮派名";
		UpdatePlayerNickName (_playerInfo.playerDto.name);
		UpdatePlayerAppellation ();
		UpdatePlayerVipFlag ();

		//属性滑动条
		_propertySliderList = new List<PropertySliderController> (3);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYSLIDER_WIDGET) as GameObject;
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.propertySliderGrid.gameObject, itemPrefab);
			var com = item.GetMissingComponent<PropertySliderController> ();
			_propertySliderList.Add (com);
		}

		_propertySliderList [0].InitItem ("气血", 290, 1, "bloom-line");
		_propertySliderList [1].InitItem ("魔法", 290, 2, "magic-line");
		_propertySliderList [2].InitItem ("活力", 174, 0, "vitality-line");

		//战斗属性
		itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYINFO_WIDGET) as GameObject;
		_bpItemList = new List<PropertyInfoController> (4);
		for (int i=0; i<4; ++i) {
			GameObject item = NGUITools.AddChild (_view.bpItemGrid.gameObject, itemPrefab);
			var com = item.GetMissingComponent<PropertyInfoController> ();
			_bpItemList.Add (com);
		}

		_bpItemList [0].InitItem ("攻击", 3, 192);
		_bpItemList [1].InitItem ("防御", 4, 192);
		_bpItemList [2].InitItem ("速度", 5, 192);
		_bpItemList [3].InitItem ("灵力", 6, 192);

		//资质属性
		_apItemList = new List<PropertyInfoController> (6);
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (_view.apItemGrid.gameObject, itemPrefab);
			var com = item.GetMissingComponent<PropertyInfoController> ();
			_apItemList.Add (com);
		}
		
		_apItemList [0].InitItem ("体质", 7, 192);
		_apItemList [1].InitItem ("魔力", 8, 192);
		_apItemList [2].InitItem ("力量", 9, 192);
		_apItemList [3].InitItem ("耐力", 10, 192);
		_apItemList [4].InitItem ("敏捷", 11, 192);
		_apItemList [5].InitItem ("潜力", 12, 192);

		UpdatePropertyInfo ();
		UpdateExpInfo ();
		OnSubWealthChanged (null);
	}

	private void UpdateExpInfo ()
	{
		ExpGrade expGrade = DataCache.getDtoByCls<ExpGrade> (_playerInfo.playerDto.level + 1);
		_view.expLbl.text = string.Format ("{0}/{1}", _playerInfo.playerDto.exp, expGrade.mainCharactorExp);
		_view.expSlider.value = (float)_playerInfo.playerDto.exp / (float)expGrade.mainCharactorExp;
	}

	private void OnSubWealthChanged (SubWealthNotify notify)
	{
		_propertySliderList [2].SetValLbl (string.Format ("{0}/{1}", PlayerModel.Instance.Vigour, PlayerModel.Instance.VigourMax));
		_propertySliderList [2].SetSliderVal ((float)PlayerModel.Instance.Vigour / (float)PlayerModel.Instance.VigourMax);

		_view.reserveExpLbl.text = PlayerModel.Instance.ReserveExp.ToString ();
	}

	private void UpdatePropertyInfo ()
	{
		_propertySliderList [0].SetValLbl (string.Format ("{0}/{1}", _playerInfo.hpMax, _playerInfo.hpMax));
		_propertySliderList [0].SetSliderVal ((float)_playerInfo.hpMax / (float)_playerInfo.hpMax);

		_propertySliderList [1].SetValLbl (string.Format ("{0}/{1}", _playerInfo.mpMax, _playerInfo.mpMax));
		_propertySliderList [1].SetSliderVal ((float)_playerInfo.mpMax / (float)_playerInfo.mpMax);

		_bpItemList [0].SetVal (_playerInfo.attack);
		_bpItemList [1].SetVal (_playerInfo.defense);
		_bpItemList [2].SetVal (_playerInfo.speed);
		_bpItemList [3].SetVal (_playerInfo.magic);

		UpdateApItemInfo (_playerInfo.playerDto.aptitudeProperties.constitution + _playerInfo.EqAps [0],
		                 _playerInfo.playerDto.aptitudeProperties.intelligent + _playerInfo.EqAps [1],
		                 _playerInfo.playerDto.aptitudeProperties.strength + _playerInfo.EqAps [2],
		                 _playerInfo.playerDto.aptitudeProperties.stamina + _playerInfo.EqAps [3],
		                 _playerInfo.playerDto.aptitudeProperties.dexterity + _playerInfo.EqAps [4]);

		if (_playerInfo.playerDto.potential > 0)
			_apItemList [5].SetValLblColor (ColorConstant.Color_Tip_GainCurrency);
		else
			_apItemList [5].SetValLblColor (ColorConstant.Color_UI_Title);

		_apItemList [5].SetVal (_playerInfo.playerDto.potential);
	}

	private void UpdateApItemInfo (int constitution, int intelligent, int strength, int stamina, int dexterity)
	{
		_apItemList [0].SetVal (constitution);
		_apItemList [1].SetVal (intelligent);
		_apItemList [2].SetVal (strength);
		_apItemList [3].SetVal (stamina);
		_apItemList [4].SetVal (dexterity);
	}

	private void UpdatePlayerVipFlag ()
	{
		_view.vipBtnSprite.isGrey = !PlayerModel.Instance.IsVip ();
	}

	private void UpdatePlayerNickName (string nickName)
	{
		_view.nameLbl.text = nickName;
	}

	private void UpdatePlayerAppellation ()
	{
		_view.appellationLbl.text = PlayerModel.Instance.GetTitleName ();
	}

	private void ShowVipInfo ()
	{
		System.Text.StringBuilder strBuilder = new System.Text.StringBuilder ();
		if (PlayerModel.Instance.IsVip ()) {
			string dateStr = DateUtil.getVipDate (PlayerModel.Instance.GetPlayer ().vipExpiredTime);
			strBuilder.AppendLine (string.Format ("你已经是VIP，到期时间为：{0}，VIP的好处：", dateStr));
		} else {
			strBuilder.AppendLine ("VIP的好处：");
		}
		strBuilder.AppendLine ("1、人物属性处尊贵VIP金色标记，繁忙优先进服特权");
		strBuilder.AppendLine ("2、每天可领取5000银币，可购买商会贵重道具");
		strBuilder.AppendLine ("3、每天可领取20个还童丹，洗极品宝宝必备道具");
		
		string tip = strBuilder.ToString ();
		
		List<string> optionList = new List<string> ();
		
		List<Vip> vipList = DataCache.getArrayByClsWithoutSort<Vip> ();
		foreach (Vip vip in vipList) {
			int day = (int)(vip.time / 86400000);
			optionList.Add (string.Format ("{0}元宝购买{1}天VIP", vip.ingot, day));
		}
		
		ProxyWorldMapModule.OpenCommonDialogue ("NPC", tip, optionList, delegate(int index) {
			//index == vipId -1
			PlayerModel.Instance.BuyVip (index + 1);
		});
	}

	private void OpenChangeNameView ()
	{
		ProxyPlayerPropertyModule.OpenPlayerChangeNameView (false);
	}
	
	private void ShowExpInfo ()
	{
		System.Text.StringBuilder strBuilder = new System.Text.StringBuilder ();
		strBuilder.AppendLine (string.Format ("当前服务器等级：{0}", PlayerModel.Instance.ServerGrade.ToString ().WrapColor ("5cf37c")));

		long nextOpenTime = PlayerModel.Instance.NextServerGradeOpenTime;
		if (nextOpenTime != -1L) {
			System.DateTime dt = DataHelper.ConvertIntDateTime (nextOpenTime);
			strBuilder.AppendLine (string.Format ("下次服务器开放等级时间：[5cf37c]{0:yyyy-MM-dd} 00:00[-]", dt));
		} else {
			strBuilder.AppendLine ("已达服务器最高等级上限");
		}
		strBuilder.AppendLine ("人物等级≥服务器等级时，获得经验将减少1/5");
		strBuilder.AppendLine ("人物等级≥服务器等级+5时，获得经验将减少1/3");
		strBuilder.Append ("人物等级≥服务器等级+8时，获得经验将转化为铜币");

		GameHintManager.Open (_view.expInfoBtn.gameObject, strBuilder.ToString ());
	}
	
	private void ShowAppellationInfo ()
	{
		ProxyPlayerPropertyModule.OpenPlayerAppellationView ();
	}
	
	private void ShowEnergyInfo ()
	{
		ProxyPlayerPropertyModule.OpenVigourWin ();
	}
	
	private void ShowFactionHint ()
	{
		GameHintManager.Open (_view.factionBtn.gameObject, GameHintManager.GetFactionHintString (PlayerModel.Instance.GetPlayer ().factionId));
	}
	#endregion

	#region AddPointInfoGroup
	private List<PropertyInfoController> _bpInfoList;
	private List<UIButton> _planInfoItemList;
	private List<AddPointSliderController> _addPointSliderList;
	private List<UILabel> _recTopInfoList;
	private List<UILabel> _recBottomInfoList;

	private void InitAddPointInfoGroup ()
	{
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYINFO_WIDGET) as GameObject;
		_bpInfoList = new List<PropertyInfoController> (6);
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (_view.bpInfoGrid.gameObject, itemPrefab);
			var com = item.GetMissingComponent<PropertyInfoController> ();
			_bpInfoList.Add (com);
		}

		_bpInfoList [0].InitItem ("气血", 1, 200);
		_bpInfoList [1].InitItem ("魔法", 2, 200);
		_bpInfoList [2].InitItem ("攻击", 3, 200);
		_bpInfoList [3].InitItem ("防御", 4, 200);
		_bpInfoList [4].InitItem ("速度", 5, 200);
		_bpInfoList [5].InitItem ("灵力", 6, 200);

		itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (ADDPOINT_SLIDER_ITEM) as GameObject;
		_addPointSliderList = new List<AddPointSliderController> (5);
		string[] titleArrs = new string[5]{"体质","魔力","力量","耐力","敏捷"};
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (_view.addPointSliderGrid.gameObject, itemPrefab);
			var com = item.GetMissingComponent<AddPointSliderController> ();
			com.InitItem (titleArrs [i], this);
			_addPointSliderList.Add (com);
		}

		_planInfoItemList = new List<UIButton> (3);
		for (int i=0; i<3; ++i) {
			GameObject item = i == 0 ? _view.planBtnPrefab : NGUITools.AddChild (_view.planItemGrid.gameObject, _view.planBtnPrefab);
			var com = item.GetComponent<UIButton> ();
			EventDelegate.Set (com.onClick, () => {
				int index = _planInfoItemList.IndexOf (UIButton.current);
				if (index != -1) {
					OnSelectPlanInfoItem (index);
				}
			});
			item.transform.Find ("planLbl").GetComponent<UILabel> ().text = string.Format ("加点方案{0}", i + 1);
			_planInfoItemList.Add (com);
		}
		_view.planInfoGroup.SetActive (false);
		UpdatePlanInfoGroup ();

		//初始化推荐加点说明
		_recTopInfoList = new List<UILabel> (5);
		for (int i=0; i<5; ++i) {
			GameObject item = i == 0 ? _view.recTipsItemPrefab : NGUITools.AddChild (_view.recTopInfoGrid.gameObject, _view.recTipsItemPrefab);
			UILabel label = item.transform.Find ("tipsLbl").GetComponent<UILabel> ();
			_recTopInfoList.Add (label);
		}

		_recBottomInfoList = new List<UILabel> (3);
		for (int i=0; i<3; ++i) {
			GameObject item = NGUITools.AddChild (_view.recBottomInfoGrid.gameObject, _view.recTipsItemPrefab);
			UILabel label = item.transform.Find ("tipsLbl").GetComponent<UILabel> ();
			_recBottomInfoList.Add (label);
		}
		UpdateRecTipsContent();

		_view.RecommendTipPanel.SetActive (false);

		UIEventListener evListener = UIEventListener.Get (_view.RecommendTipPanel.transform.Find ("BgCollider").gameObject);
		evListener.onClick = (go) => {
			ToggleRecTipPanel ();
		};

		_curPlanPlayerInfo = new PlayerPropertyInfo (_playerInfo);
		OnSelectPlanInfoItem (_playerInfo.playerDto.pointPlan);
	}

	private void UpdateRecTipsContent(){
		string[] tipsTitleList = new string[5]{"体质","魔力","力量","耐力","敏捷"};
		int allPotentialPoint = _playerInfo.playerDto.level * 5 + 5;

		List<AptitudeTips> tipsInfoList = DataCache.getArrayByCls<AptitudeTips>();
		for(int i=0,itemIndex=0;i<tipsInfoList.Count;++i){
			AptitudeTips tipsInfo = tipsInfoList[i];
			if(tipsInfo.factionId == _playerInfo.playerDto.factionId){
				int[] apVals = new int[5]{tipsInfo.constitution,tipsInfo.intelligent,tipsInfo.strength,tipsInfo.stamina,tipsInfo.dexterity};
				System.Text.StringBuilder titleBuilder = new System.Text.StringBuilder();
				System.Text.StringBuilder recTipBuilder = new System.Text.StringBuilder();
				for(int j=0;j<apVals.Length;++j){
					if(apVals[j]>0){
						titleBuilder.Append(string.Format("{0}{1}",apVals[j],tipsTitleList[j]));
						int distribution = (int)(apVals[j] / 5.0f * allPotentialPoint);
						recTipBuilder.Append(string.Format("{0}：{1} ",tipsTitleList[j],distribution));
					}
				}
				
				_recBottomInfoList[itemIndex++].text = string.Format("[2dac0a]{0}[-] {1}\n推荐分配 {2}",titleBuilder.ToString(),tipsInfo.desc,recTipBuilder.ToString());
			}

			if(itemIndex >= 3)
				break;
		}
	}

	//更新加点方案按钮状态
	private void UpdatePlanInfoGroup ()
	{
		int openCount = PlayerModel.Instance.GetAddPointPlanOpenCount ();
		for (int i=0; i<_planInfoItemList.Count; ++i) {
			if (i < openCount)
				_planInfoItemList [i].GetComponent<UISprite> ().isGrey = false;
			else
				_planInfoItemList [i].GetComponent<UISprite> ().isGrey = true;
		}
	}

	private void TogglePlanInfoGroup ()
	{
		_view.planInfoGroup.SetActive (!_view.planInfoGroup.activeSelf);
	}

	private int _curPlanId = -1;
	private PlayerPropertyInfo _curPlanPlayerInfo;  //用于缓存当前加点方案的玩家结果属性
	private void OnSelectPlanInfoItem (int index)
	{
		if (_curPlanId == index){
			_view.planInfoGroup.SetActive (false);
			return;
		}

		if (index < PlayerModel.Instance.GetAddPointPlanOpenCount ()) {
			if (index == PlayerModel.Instance.GetActivedAddPointPlanId ()) {
				SetupApPlanInfo (index, _playerInfo.playerDto.aptitudeProperties);
				SetApSliderThumbActive (true);
			} else {
				ServiceRequestAction.requestServer (PlayerService.gainAptitudePlan (index), "GainApPlan", (e) => {
					SetupApPlanInfo (index, e as AptitudeProperties);
					SetApSliderThumbActive (false);
				});
			}
		} else {
			TipManager.AddTip (string.Format ("该加点方案需要{0}级才能使用", PlayerModel.Instance.GetAddPointPlanOpenLevel (index + 1)));
		}
	}

	private void SetupApPlanInfo (int newPlanId, AptitudeProperties newApInfo)
	{
		_curPlanId = newPlanId;
		_curPlanPlayerInfo.playerDto.aptitudeProperties = newApInfo;
		//重新计算当前剩余潜力点
		int minApVal = _curPlanPlayerInfo.playerDto.level + DataHelper.GetStaticConfigValue (H1StaticConfigs.MIN_APTITUDE_RESET_POINT, 10);
		int allApVal = newApInfo.constitution + newApInfo.intelligent + newApInfo.strength + newApInfo.stamina + newApInfo.dexterity;
		_curPlanPlayerInfo.playerDto.potential = (minApVal * 5 + _curPlanPlayerInfo.playerDto.level * 5 + 5) - allApVal;
		PlayerModel.Instance.CalculatePlayerBp (_curPlanPlayerInfo);

		_view.curPlanInfoLbl.text = string.Format ("加点方案{0}", _curPlanId + 1);
		if (_curPlanId == PlayerModel.Instance.GetActivedAddPointPlanId ()) {
			ActivePlanActiveBtn (false);
		} else {
			ActivePlanActiveBtn (true);
		}

		UpdateAddPointPropertyInfo ();
		_view.planInfoGroup.SetActive (false);
	}

	//更新战斗属性信息、潜力点、资质属性信息
	private int _remainPotential;

	public int RemainPotential {
		get {
			return _remainPotential;
		}
	}

	private void UpdateAddPointPropertyInfo ()
	{
		int[] playerBpVals = new int[6] {
			_curPlanPlayerInfo.hpMax,
			_curPlanPlayerInfo.mpMax,
			_curPlanPlayerInfo.attack,
			_curPlanPlayerInfo.defense,
			_curPlanPlayerInfo.speed,
			_curPlanPlayerInfo.magic
		};
		for (int i=0; i<playerBpVals.Length; ++i) {
			_bpInfoList [i].SetVal (playerBpVals [i]);
			_bpInfoList [i].SetExtraInfoLbl ("");
		}

		UpdateRemainPotential (_curPlanPlayerInfo.playerDto.potential);

		int[] playerApVals = _curPlanPlayerInfo.ToApInfoArray ();
		int minApVal = _curPlanPlayerInfo.playerDto.level + DataHelper.GetStaticConfigValue (H1StaticConfigs.MIN_APTITUDE_RESET_POINT, 10);
		string[] tipsTitleList = new string[5]{"体质","魔力","力量","耐力","敏捷"};
		string[] tipsContentList = new string[5]{
			"影响气血、灵力、速度",
			"影响灵力、魔法",
			"影响攻击、魔法、灵力、速度",
			"影响防御、灵力、速度",
			"影响速度"
		};
		for (int i=0; i<_addPointSliderList.Count; ++i) {
			int originVal = _curPlanPlayerInfo.EqAps [i] + minApVal; 	//固定点
			int realVal = _curPlanPlayerInfo.EqAps [i] + playerApVals [i]; //固定点+已投入点
			int maxVal = _curPlanPlayerInfo.EqAps [i] + _curPlanPlayerInfo.playerDto.level * 6 + 15; //最大值
			_addPointSliderList [i].ResetItem (originVal, realVal, maxVal);

			//更新"+"按钮状态
			if (_remainPotential > 0) {
				_addPointSliderList [i].SetAddBtnGrey (false);
			} else {
				_addPointSliderList [i].SetAddBtnGrey (true);
			}

			_recTopInfoList [i].text = string.Format ("[2dac0a]{0}[-] {1} [2dac0a]已分配:{2}点[-]", tipsTitleList [i], tipsContentList [i], playerApVals [i] - minApVal);
		}
	}

	private void UpdateRemainPotential (int val)
	{
		_remainPotential = val;
		_view.ppLbl.text = _remainPotential.ToString ();

		if (_remainPotential != _curPlanPlayerInfo.playerDto.potential)
			_view.confirmBtn.isEnabled = true;
		else
			_view.confirmBtn.isEnabled = false;
	}

	public void OnApSliderAdd (AddPointSliderController com, int point)
	{
		if (_curPlanId != PlayerModel.Instance.GetActivedAddPointPlanId ()) {
			TipManager.AddTip ("需要启用当前方案才能进行加点操作");
			return;
		}

		if (point > 0 && _remainPotential - point >= 0) {
			com.SetAddPointVal (com.AddPoint + point);
			UpdateRemainPotential (_remainPotential - point);
			CalculatePreviewBp ();
		}
	}

	public void OnApSliderMinus (AddPointSliderController com, int point)
	{
		if (point > 0 && _remainPotential + point <= _playerInfo.playerDto.potential) {
			com.SetAddPointVal (com.AddPoint - point);
			UpdateRemainPotential (_remainPotential + point);
			CalculatePreviewBp ();
		}
	}

	private void SetApSliderThumbActive (bool b)
	{
		for (int i=0; i<_addPointSliderList.Count; ++i) {
			_addPointSliderList [i].SetThumbActive (b);
		}
	}

	private void CalculatePreviewBp ()
	{
		float[] increaseBps = new float[6];
		for (int i=0; i<_addPointSliderList.Count; ++i) {
			int addPoint = _addPointSliderList [i].AddPoint;
			if (addPoint > 0) {
				AptitudePropertyInfo apDataInfo = DataCache.getDtoByCls<AptitudePropertyInfo> (i + 1);
				
				increaseBps [0] += apDataInfo.hp * addPoint;
				increaseBps [1] += apDataInfo.mp * addPoint;
				increaseBps [2] += apDataInfo.attack * addPoint;
				increaseBps [3] += apDataInfo.defense * addPoint;
				increaseBps [4] += apDataInfo.speed * addPoint;
				increaseBps [5] += apDataInfo.magic * addPoint;
			}

			_addPointSliderList [i].UpdateMaxSliderVal ();
			//更新"+"按钮状态
			if (_remainPotential > 0) {
				_addPointSliderList [i].SetAddBtnGrey (false);
			} else {
				_addPointSliderList [i].SetAddBtnGrey (true);
			}
		}

		for (int i=0; i<increaseBps.Length; ++i) {
			int delta = Mathf.FloorToInt (increaseBps [i]);
			if (delta > 0)
				_bpInfoList [i].SetExtraInfoLbl (string.Format ("+{0}", delta));
			else
				_bpInfoList [i].SetExtraInfoLbl ("");
		}
	}

	//显示加点方案"启用"按钮
	private void ActivePlanActiveBtn (bool active)
	{
		_view.planActiveBtn.gameObject.SetActive (active);
		_view.resetPointBtn.gameObject.SetActive (!active);
		_view.confirmBtn.gameObject.SetActive (!active);
		
		_view.activeTipsLbl.cachedGameObject.SetActive (!active);
		_view.planTipsLbl.cachedGameObject.SetActive (active);
	}

	private void OnClickActiveBtn ()
	{
		if (_curPlanId != PlayerModel.Instance.GetActivedAddPointPlanId ()) {
			string hint ="";
			if(_playerInfo.playerDto.changeTimes > 0){
				hint = string.Format("是否花费{0}铜币进行切换",5*CurrencyExchange.IngotToCopper(1)*_playerInfo.playerDto.changeTimes);
			}else
				hint = "每天第一次切换加点免费，确认要切换吗？";

			ProxyWindowModule.OpenConfirmWindow(hint,"",()=>{
				ServiceRequestAction.requestServer (PlayerService.aptitudePlanChange (_curPlanId), "ChangeApPlan", (e) => {
					PlayerModel.Instance.UpdateActivedAddPointPlanId (_curPlanId);
					_playerInfo.ResetPlayerInfo (_curPlanPlayerInfo);
					SetApSliderThumbActive (true);
					ActivePlanActiveBtn (false);
				});
			});
		}
	}

	private void ToggleRecTipPanel ()
	{
		_view.RecommendTipPanel.SetActive (!_view.RecommendTipPanel.activeSelf);
	}

	private void OnClickConfirmBtn ()
	{
		ServiceRequestAction.requestServer (PlayerService.increaseAptitude (_addPointSliderList [0].AddPoint,
		                                                                    _addPointSliderList [1].AddPoint,
		                                                                    _addPointSliderList [2].AddPoint,
		                                                                    _addPointSliderList [3].AddPoint,
		                                                                    _addPointSliderList [4].AddPoint),
		                                                                    "更改人物资质属性", (e) => {
			AptitudeProperties apInfo = _playerInfo.playerDto.aptitudeProperties;
			PlayerModel.Instance.UpdatePlayerAp (_remainPotential,
			                                     apInfo.constitution + _addPointSliderList [0].AddPoint,
			                                     apInfo.intelligent + _addPointSliderList [1].AddPoint,
			                                     apInfo.strength + _addPointSliderList [2].AddPoint,
			                                     apInfo.stamina + _addPointSliderList [3].AddPoint,
			                                     apInfo.dexterity + _addPointSliderList [4].AddPoint);
			RefreshPlanPropertyInfo();
		});
	}

	PlayerResetApItemLogicDef _resetApItemLogic;
	private void OnClickResetPointBtn ()
	{
		if(_resetApItemLogic == null)
			_resetApItemLogic = new PlayerResetApItemLogicDef();

		_resetApItemLogic.OpenDialogueView(null,RefreshPlanPropertyInfo);
	}

	private void RefreshPlanPropertyInfo(){
		_curPlanPlayerInfo.ResetPlayerInfo (_playerInfo);
		UpdateAddPointPropertyInfo ();
	}
	#endregion

	private void CloseView ()
	{
		ProxyPlayerPropertyModule.Close ();
	}
}
