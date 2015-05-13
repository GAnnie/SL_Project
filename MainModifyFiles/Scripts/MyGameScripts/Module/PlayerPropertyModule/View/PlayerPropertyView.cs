//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PlayerPropertyView.
/// </summary>
public class PlayerPropertyView : BaseView
{
	public UILabel IdLbl;
	public UILabel nameLbl;
	public UILabel lvLbl;
	public UILabel factionLbl;
	public UILabel guildLbl;
	public UILabel appellationLbl;
	public UILabel energyLbl;
	public UIGrid apItemGrid;
	public UIButton closeBtn;
	public UIButton energyInfoBtn;
	public UIButton nameInfoBtn;
	public UIButton ppInfoBtn;
	public Transform modelAnchor;
	public UISlider expSlider;
	public UILabel expLbl;
	public UIButton apItemOpBtn;
	public UIGrid bpItemGrid;
	public UIButton vipInfoBtn;
	public UIButton expSliderBtn;
	public UIButton factionHintBtn;
	public UIButton levelInfoBtn;
	public UIButton appellationBtn;

	public void Setup (Transform root)
	{
		IdLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/IdInfo/valueBg/valLbl").GetComponent<UILabel>();
		nameLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/NameInfo/valueBg/valLbl").GetComponent<UILabel>();
		lvLbl = root.Find("ContentFrame/ContentBg/ModelAnchor/ButtonPanel/LevelInfo/valLbl").GetComponent<UILabel>();
		factionLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/FactionInfo/valueBg/valLbl").GetComponent<UILabel>();
		guildLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/GuildInfo/valueBg/valLbl").GetComponent<UILabel>();
		appellationLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/AppellationInfo/valueBg/valLbl").GetComponent<UILabel>();
		energyLbl = root.Find("ContentFrame/ContentBg/TopRightGroup/EnergyInfo/valueBg/valLbl").GetComponent<UILabel>();
		apItemGrid = root.Find("ContentFrame/ContentBg/BottomRightGroup/ApItemGrid").GetComponent<UIGrid>();
		closeBtn = root.Find("ContentFrame/ContentBg/CloseBtn").GetComponent<UIButton>();
		energyInfoBtn = root.Find("ContentFrame/ContentBg/TopRightGroup/EnergyInfo/funBtn").GetComponent<UIButton>();
		nameInfoBtn = root.Find("ContentFrame/ContentBg/TopRightGroup/NameInfo/funBtn").GetComponent<UIButton>();
		ppInfoBtn = root.Find("ContentFrame/ContentBg/BottomRightGroup/ppInfoBtn").GetComponent<UIButton>();
		modelAnchor = root.Find("ContentFrame/ContentBg/ModelAnchor");
		expSlider = root.Find("ContentFrame/ContentBg/ExpInfo/slider").GetComponent<UISlider>();
		expLbl = root.Find("ContentFrame/ContentBg/ExpInfo/slider/valueLbl").GetComponent<UILabel>();
		apItemOpBtn = root.Find("ContentFrame/ContentBg/BottomRightGroup/APItemOpBtn").GetComponent<UIButton>();
		bpItemGrid = root.Find("ContentFrame/ContentBg/BpItemGrid").GetComponent<UIGrid>();
		vipInfoBtn = root.Find("ContentFrame/ContentBg/TopRightGroup/IdInfo/funBtn").GetComponent<UIButton>();
		expSliderBtn = root.Find("ContentFrame/ContentBg/ExpInfo/titleBg").GetComponent<UIButton>();
		factionHintBtn = root.Find("ContentFrame/ContentBg/TopRightGroup/FactionInfo/titleBg").GetComponent<UIButton>();
		levelInfoBtn = root.Find("ContentFrame/ContentBg/ModelAnchor/ButtonPanel/LevelInfo").GetComponent<UIButton>();
		appellationBtn = root.Find("ContentFrame/ContentBg/TopRightGroup/AppellationInfo/titleBg").GetComponent<UIButton>();
	}
}