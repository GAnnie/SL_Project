//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PetPropertyMainView.
/// </summary>
public class PetPropertyMainView : BaseView
{
	public UIButton closeBtn;
	public UIGrid petItemGrid;
	public Transform modelAnchor;
	public UISlider expSlider;
	public UILabel expLbl;
	public UILabel petRankingLbl;
	public UIButton petRankingBtn;
	public UIGrid petInfoTabBtnGrid;
	public UIButton freeCaptiveBtn;
	public UIButton changeNameBtn;
	public UIButton apItemOpBtn;
	public UIButton restOrBattleBtn;
	public UIGrid rightTabBtnGrid;
	public UILabel rightTabContentTitle;
	public UISprite titleSprite;
	public GameObject TabContent_PetProps;
	public PageScrollView petPropsItemGrid;
	public GameObject TabContent_ResetPetAp;
	public GameObject TabContent_BaseProperty;
	public GameObject TabContent_PetAptitude;
	public GameObject TabContent_PetSkill;
	public GameObject BaseInfoGroup;
	public UIButton ppInfoBtn;
	public UIButton petGrowthInfoBtn;
	public GameObject PetHandbookGroup;
	public UILabel restOrBattleBtnLbl;
	public UIButton petTypeBtn;
	public UIButton decorationBtn;
	public GameObject modelBtnGroup;
	public GameObject resetPropItemAnchor;
	public UIButton resetPetBtn;
	public UILabel resetPetBtnLbl;
	public UIGrid convenientHintItemGrid;
	public UIScrollView convenientHintScrollView;
	public UIButton resetPetApBtn;
	public UIGrid petEqItemGrid;
	public GameObject pageFlagGrid;
	public UIButton petChangeBtn;

	public void Setup (Transform root)
	{
		closeBtn = root.Find("ContentFrame/CloseBtn").GetComponent<UIButton>();
		petItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetItemGroup/PetItemScrollView/PetItemGrid").GetComponent<UIGrid>();
		modelAnchor = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ModelAnchor");
		expSlider = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ExpInfo/slider").GetComponent<UISlider>();
		expLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ExpInfo/slider/valueLbl").GetComponent<UILabel>();
		petRankingLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/PetRankingInfo/valueBg/valLbl").GetComponent<UILabel>();
		petRankingBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/PetRankingInfo/titleBg").GetComponent<UIButton>();
		petInfoTabBtnGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomInfoGroup/TabBtnGrid").GetComponent<UIGrid>();
		freeCaptiveBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/freeCaptiveBtn").GetComponent<UIButton>();
		changeNameBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/changeNameBtn").GetComponent<UIButton>();
		apItemOpBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/apItemOpBtn").GetComponent<UIButton>();
		restOrBattleBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/restOrBattleBtn").GetComponent<UIButton>();
		rightTabBtnGrid = root.Find("ContentFrame/ContentBg/RightTabBtnGrid").GetComponent<UIGrid>();
		rightTabContentTitle = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContentTitleBg/titleLbl").GetComponent<UILabel>();
		titleSprite = root.Find("ContentFrame/_titleBg/_titleSprite").GetComponent<UISprite>();
		TabContent_PetProps = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_PetProps").gameObject;
		petPropsItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_PetProps/PetPropsItemGrid").GetComponent<PageScrollView>();
		TabContent_ResetPetAp = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp").gameObject;
		TabContent_BaseProperty = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomInfoGroup/TabContentRoot/TabContent_BaseProperty").gameObject;
		TabContent_PetAptitude = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomInfoGroup/TabContentRoot/TabContent_PetAptitude").gameObject;
		TabContent_PetSkill = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomInfoGroup/TabContentRoot/TabContent_PetSkill").gameObject;
		BaseInfoGroup = root.Find("ContentFrame/ContentBg/BaseInfoGroup").gameObject;
		ppInfoBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomInfoGroup/TabContentRoot/TabContent_BaseProperty/ppInfoBtn").GetComponent<UIButton>();
		petGrowthInfoBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/PetGrowthInfoBtn").GetComponent<UIButton>();
		PetHandbookGroup = root.Find("ContentFrame/ContentBg/PetHandbookGroup").gameObject;
		restOrBattleBtnLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/restOrBattleBtn/Label").GetComponent<UILabel>();
		petTypeBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ModelAnchor/ButtonPanel/petTypeBtn").GetComponent<UIButton>();
		decorationBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ModelAnchor/ButtonPanel/decorationBtn").GetComponent<UIButton>();
		modelBtnGroup = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ModelAnchor/ButtonPanel").gameObject;
		resetPropItemAnchor = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp/resetPropItemAnchor").gameObject;
		resetPetBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp/resetPetBtn").GetComponent<UIButton>();
		resetPetBtnLbl = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp/resetPetBtn/Label").GetComponent<UILabel>();
		convenientHintItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp/convenientScrollView/itemGrid").GetComponent<UIGrid>();
		convenientHintScrollView = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/TabContent_ResetPetAp/convenientScrollView").GetComponent<UIScrollView>();
		resetPetApBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/BottomBtnGroup/resetPetApBtn").GetComponent<UIButton>();
		petEqItemGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/EquipmentGroup").GetComponent<UIGrid>();
		pageFlagGrid = root.Find("ContentFrame/ContentBg/BaseInfoGroup/RightTabContentRoot/pageFlagGrid").gameObject;
		petChangeBtn = root.Find("ContentFrame/ContentBg/BaseInfoGroup/PetPropertyInfoGroup/TopInfoGroup/ModelAnchor/ButtonPanel/petChangeBtn").GetComponent<UIButton>();
	}
}
