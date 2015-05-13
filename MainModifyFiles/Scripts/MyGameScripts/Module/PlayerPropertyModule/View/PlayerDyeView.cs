//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for PlayerDyeView.
/// </summary>
public class PlayerDyeView : BaseView
{
	public UIButton closeBtn;
	public Transform modelAnchor;
	public UIButton randomBtn;
	public GameObject propsItemAnchor;
	public UIButton resetBtn;
	public UIButton confirmBtn;
	public UIGrid headOptionGrid;
	public UIGrid clothOptionGrid;
	public UIGrid decorationOptionGrid;
	public GameObject dyeOptionItemPrefab;

	public void Setup (Transform root)
	{
		closeBtn = root.Find("ContentFrame/CloseBtn").GetComponent<UIButton>();
		modelAnchor = root.Find("ContentFrame/LeftGroup/ModelAnchor");
		randomBtn = root.Find("ContentFrame/LeftGroup/ModelAnchor/ButtonPanel/randomBtn").GetComponent<UIButton>();
		propsItemAnchor = root.Find("ContentFrame/LeftGroup/propsItemAnchor").gameObject;
		resetBtn = root.Find("ContentFrame/RightGroup/buttonGroup/resetBtn").GetComponent<UIButton>();
		confirmBtn = root.Find("ContentFrame/RightGroup/buttonGroup/confirmBtn").GetComponent<UIButton>();
		headOptionGrid = root.Find("ContentFrame/RightGroup/headPartOptionGroup/optionItemGrid").GetComponent<UIGrid>();
		clothOptionGrid = root.Find("ContentFrame/RightGroup/clothPartOptionGroup/optionItemGrid").GetComponent<UIGrid>();
		decorationOptionGrid = root.Find("ContentFrame/RightGroup/decorationPartOptionGroup/optionItemGrid").GetComponent<UIGrid>();
		dyeOptionItemPrefab = root.Find("ContentFrame/RightGroup/headPartOptionGroup/optionItemGrid/dyeOptionItem").gameObject;
	}
}