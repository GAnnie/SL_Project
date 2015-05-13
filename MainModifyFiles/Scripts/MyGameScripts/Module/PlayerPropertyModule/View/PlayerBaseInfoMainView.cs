//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PlayerBaseInfoMainView.
/// </summary>
public class PlayerBaseInfoMainView : BaseView
{
	public UIButton closeBtn;
	public GameObject BaseInfoGroup;
	public GameObject AddPointInfoGroup;
	public UIGrid rightTabBtnGrid;
	public Transform modelAnchor;
	public UIButton vipBtn;
	public UILabel lvLbl;
	public UILabel nameLbl;
	public UIButton changeNameBtn;
	public UIButton factionBtn;
	public UILabel idLbl;
	public UILabel guildLbl;
	public UILabel appellationLbl;
	public UIButton appellationBtn;
	public UISprite titleSprite;
	public UIGrid propertySliderGrid;
	public UIButton energyBtn;
	public UIGrid bpItemGrid;
	public UIGrid apItemGrid;
	public UISlider expSlider;
	public UILabel expLbl;
	public UIButton expInfoBtn;
	public UILabel reserveExpLbl;
	public UISprite vipBtnSprite;
	public UIButton curPlanInfoBtn;
	public GameObject planInfoGroup;
	public UIGrid planItemGrid;
	public GameObject planBtnPrefab;
	public UIGrid bpInfoGrid;
	public UILabel activeTipsLbl;
	public UIButton planActiveBtn;
	public UILabel ppLbl;
	public UIButton recommendBtn;
	public UIGrid addPointSliderGrid;
	public UIButton resetPointBtn;
	public UIButton confirmBtn;
	public UILabel curPlanInfoLbl;
	public UILabel planTipsLbl;
	public GameObject RecommendTipPanel;
	public UIGrid recTopInfoGrid;
	public UIGrid recBottomInfoGrid;
	public GameObject recTipsItemPrefab;

	public void Setup (Transform root)
	{
		closeBtn = root.Find("ContentFrame/CloseBtn").GetComponent<UIButton>();
		BaseInfoGroup = root.Find("ContentFrame/ContentBg/BaseInfoGroup").gameObject;
		AddPointInfoGroup = root.Find("ContentFrame/ContentBg/AddPointInfoGroup").gameObject;
		rightTabBtnGrid = root.Find("ContentFrame/ContentBg/RightTabBtnGrid").GetComponent<UIGrid>();
		modelAnchor = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/ModelAnchor");
		vipBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/NameInfo/vipBtn").GetComponent<UIButton>();
		lvLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/NameInfo/lvLbl").GetComponent<UILabel>();
		nameLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/NameInfo/nameLbl").GetComponent<UILabel>();
		changeNameBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/NameInfo/changeNameBtn").GetComponent<UIButton>();
		factionBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/IdInfo/factionIcon").GetComponent<UIButton>();
		idLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/IdInfo/idLbl").GetComponent<UILabel>();
		guildLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/IdInfo/guildLbl").GetComponent<UILabel>();
		appellationLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/AppellationInfo/valueBg/valLbl").GetComponent<UILabel>();
		appellationBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/AppellationInfo/appellationBtn").GetComponent<UIButton>();
		titleSprite = root.Find("ContentFrame/_titleBg/titleSprite").GetComponent<UISprite>();
		propertySliderGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/sliderGrid").GetComponent<UIGrid>();
		energyBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/energyBtn").GetComponent<UIButton>();
		bpItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/BpInfoBg/bpItemGrid").GetComponent<UIGrid>();
		apItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/ApInfoBg/apItemGrid").GetComponent<UIGrid>();
		expSlider = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/ExpInfo/expSlider").GetComponent<UISlider>();
		expLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/ExpInfo/expSlider/valueLbl").GetComponent<UILabel>();
		expInfoBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/ExpInfo/expInfoBtn").GetComponent<UIButton>();
		reserveExpLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightInfoGroup/ReserveExpInfo/reserveExpLbl").GetComponent<UILabel>();
		vipBtnSprite = root.Find("ContentFrame/ContentBg/BaseInfoGroup/LeftInfoGroup/NameInfo/vipBtn").GetComponent<UISprite>();
		curPlanInfoBtn = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/planInfoBtn").GetComponent<UIButton>();
		planInfoGroup = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/planInfoGroup").gameObject;
		planItemGrid = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/planInfoGroup/planItemGrid").GetComponent<UIGrid>();
		planBtnPrefab = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/planInfoGroup/planItemGrid/planBtnPrefab").gameObject;
		bpInfoGrid = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/bpInfo/bpItemGrid").GetComponent<UIGrid>();
		activeTipsLbl = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/TipsInfoGroup/activeTipsLbl").GetComponent<UILabel>();
		planActiveBtn = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/bottomGroup/activeBtn").GetComponent<UIButton>();
		ppLbl = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RightInfoGroup/ppInfo/bgSprite/ppLbl").GetComponent<UILabel>();
		recommendBtn = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RightInfoGroup/recommendBtn").GetComponent<UIButton>();
		addPointSliderGrid = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RightInfoGroup/sliderGrid").GetComponent<UIGrid>();
		resetPointBtn = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RightInfoGroup/bottomGroup/resetPointBtn").GetComponent<UIButton>();
		confirmBtn = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RightInfoGroup/bottomGroup/confirmBtn").GetComponent<UIButton>();
		curPlanInfoLbl = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/LeftInfoGroup/planInfoBtn/planLbl").GetComponent<UILabel>();
		planTipsLbl = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/TipsInfoGroup/planTipsLbl").GetComponent<UILabel>();
		RecommendTipPanel = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RecommendTipPanel").gameObject;
		recTopInfoGrid = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RecommendTipPanel/ContentBg/topInfoGrid").GetComponent<UIGrid>();
		recBottomInfoGrid = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RecommendTipPanel/ContentBg/bottomInfoGrid").GetComponent<UIGrid>();
		recTipsItemPrefab = root.Find("ContentFrame/ContentBg/AddPointInfoGroup/RecommendTipPanel/ContentBg/topInfoGrid/tipsItem").gameObject;
	}
}
