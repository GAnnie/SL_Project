//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for NpcDialogueView.
/// </summary>
public class NpcDialogueView : BaseView
{
	public UIGrid FunctionGrid;
	public UILabel OptionTitle;
	public UILabel MsgLabel;
	public UILabel NameLabel;
	public UITexture FaceTexture;
	public UISprite OptionBg;
	public GameObject FunctionCellPrefab;
	public UIButton ClickBtnBoxCollider_UIButton;
	public UIButton BoxCollider_UIButton;

	public void Setup (Transform root)
	{
		FunctionGrid = root.Find("ContentBg/OptionBg/FunctionGrid").GetComponent<UIGrid>();
		OptionTitle = root.Find("ContentBg/OptionBg/OptionTitle").GetComponent<UILabel>();
		MsgLabel = root.Find("ContentBg/ScrollView/MsgLabel").GetComponent<UILabel>();
		NameLabel = root.Find("ContentBg/FaceTexture/NameBg/NameLabel").GetComponent<UILabel>();
		FaceTexture = root.Find("ContentBg/FaceTexture").GetComponent<UITexture>();
		OptionBg = root.Find("ContentBg/OptionBg").GetComponent<UISprite>();
		FunctionCellPrefab = root.Find("ContentBg/OptionBg/FunctionGrid/FunctionCell").gameObject;
		ClickBtnBoxCollider_UIButton = root.Find("ClickBtnBoxCollider").GetComponent<UIButton>();
		BoxCollider_UIButton = root.Find("ContentBg/BoxCollider").GetComponent<UIButton>();
	}
}