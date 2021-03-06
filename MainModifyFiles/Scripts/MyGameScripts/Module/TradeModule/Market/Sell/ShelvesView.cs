//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for ShelvesView.
/// </summary>
public class ShelvesView : BaseView
{
	public UIButton CutbackButton_UIButton;
	public UIButton IncreaseButton_UIButton;
	public UIButton RefButton_UIButton;
	public UIButton ShelvesButton_UIButton;
	public UILabel CountLabel_UILabel;
	public UISprite IconSprite_UISprite;
	public UILabel NameLabel_UILabel;
	public UILabel Prive_UILabel;
	public UILabel TotalPrice_UILabel;
	public UILabel Fee_UILabel;
	public UILabel IncrementLabel_UILabel;
	public UIButton CloseButton_UIButton;
	public UIButton ItemCell_UIButton;

	public void Setup (Transform root)
	{
		CutbackButton_UIButton = root.Find("RightGroup/PriceCellWidget/ModifiedBtn/CutbackButton").GetComponent<UIButton>();
		IncreaseButton_UIButton = root.Find("RightGroup/PriceCellWidget/ModifiedBtn/IncreaseButton").GetComponent<UIButton>();
		RefButton_UIButton = root.Find("RightGroup/PriceCellWidget/ModifiedBtn/RefButton").GetComponent<UIButton>();
		ShelvesButton_UIButton = root.Find("RightGroup/PriceCellWidget/ModifiedBtn/ShelvesButton").GetComponent<UIButton>();
		CountLabel_UILabel = root.Find("RightGroup/ItemCell/CountLabel").GetComponent<UILabel>();
		IconSprite_UISprite = root.Find("RightGroup/ItemCell/IconSprite").GetComponent<UISprite>();
		NameLabel_UILabel = root.Find("RightGroup/ItemCell/NameLabel").GetComponent<UILabel>();
		Prive_UILabel = root.Find("RightGroup/PriceCellWidget/valueBg/valLbl").GetComponent<UILabel>();
		TotalPrice_UILabel = root.Find("RightGroup/TotalPriceCellWidget/valueBg/valLbl").GetComponent<UILabel>();
		Fee_UILabel = root.Find("RightGroup/FeeCellWidget/valueBg/valLbl").GetComponent<UILabel>();
		IncrementLabel_UILabel = root.Find("RightGroup/IncrementLabel").GetComponent<UILabel>();
		CloseButton_UIButton = root.Find("CloseButton").GetComponent<UIButton>();
		ItemCell_UIButton = root.Find("RightGroup/ItemCell").GetComponent<UIButton>();
	}
}
