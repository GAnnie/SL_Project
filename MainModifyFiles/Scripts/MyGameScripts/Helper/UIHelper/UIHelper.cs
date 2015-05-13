using UnityEngine;
using System.Collections;

public static class UIHelper {

	public static UIShowUp ShowUpUISprite( string prefabName )
	{
		GameObject go = LayerManager.Instance.floatTipAnchor;
		if ( go == null )
			return null;
		
		GameObject prefab = ResourceLoader.Load( prefabName ) as GameObject;
		if ( prefab == null)
			return null;
		
		GameObject myObject = NGUITools.AddChild( go, prefab );
		
		if ( myObject == null )
		{
			GameDebuger.Log("Instantiate " + prefab.name + "failed!!!" );
			return null;
		}
		
		return myObject.GetComponent< UIShowUp >();
	}	

	/**
	 *  UIPanel AddChild UIPanel and UIPanel AddChild UIWidget
	 **/
	public static void AdjustDepth(GameObject go, int adjustment = 1)
	{
		if (go != null)
		{
			if(go.GetComponent<UIPanel>() == null)
			{
				UIHelper.AdjustDepthWithoutPanel(go,adjustment);
			}
			else
			{
				if(go.transform.parent != null)
				{
					UIPanel parentPanel = go.transform.parent.GetComponentInParent<UIPanel>();
					if(parentPanel != null)
					{
						NGUITools.AdjustDepth(go,parentPanel.depth + adjustment);
					}
				}else
					NGUITools.AdjustDepth(go,adjustment);
			}
		}
	}

	/**
	 *  UIPanel AddChild UIWidget
	 **/
	static public int AdjustDepthWithoutPanel (GameObject go, int adjustment)
	{
		if (go != null)
		{
			UIWidget[] widgets = go.GetComponentsInChildren<UIWidget>(true);
			for (int i = 0, imax = widgets.Length; i < imax; ++i)
			{
				UIWidget w = widgets[i];
				w.depth = w.depth + adjustment;
			}
			return 2;
		}
		return 0;
	}

	static public UIButton CreateBaseBtn(GameObject parent, string label, EventDelegate.Callback callback = null,string goName="Button")
	{
		GameObject go = NGUITools.AddChild(parent, (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BaseUI/BaseButton"));
		go.name = goName;
		go.GetComponentInChildren< UILabel >().text = label;
		UIButton btn = go.GetComponent<UIButton>();
		if(btn != null && callback != null)
		{
			EventDelegate.Set(btn.onClick,callback);
		}
		return btn;
	}

	static public CostButton CreateCostBtn(GameObject parent, string label, string icon,EventDelegate.Callback callback = null,string goName="CostButton")
	{
		GameObject go = NGUITools.AddChild(parent, (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BaseUI/CostButton"));
		go.name = goName;
		CostButton costBtn = go.GetComponent< CostButton >();
		costBtn.NameStr = label;
		costBtn.CostIconSprite = icon;
		UIButton btn = go.GetComponent<UIButton>();
		if(btn != null && callback != null)
		{
			EventDelegate.Set(btn.onClick,callback);
		}
		return costBtn;
	}

	static public ItemCellController CreateItemCell(GameObject parent)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( "Prefabs/Module/BackpackModule/ItemCell" ) as GameObject;
		GameObject module = GameObjectExt.AddChild(parent,prefab);
		ItemCellController cell = module.GetMissingComponent<ItemCellController>();
		cell.InitView();
		return cell;
	}
}
