using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class WorldMapController : MonoBehaviour,IViewController
{

	private WorldMapView _view;
	private GameObject _baseButtonPrefab;
	
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<WorldMapView> ();
		_view.Setup (this.transform);
		
		_baseButtonPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BaseUI/BaseButton") as GameObject;
	}

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.CloseButton.onClick, OnCloseButtonClick);
	}
	
	private void OnCloseButtonClick()
	{
		ProxyWorldMapModule.Close();
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose(){
	}

	public void Open()
	{
		if (_view == null)
		{
			InitView ();
			RegisterEvent ();
		}
		SetData ();
	}

	private void SetData()
	{
		List<SceneMap> list = DataCache.getArrayByCls<SceneMap>();
		foreach(SceneMap map in list)
		{
//			if (map.type != 8 && map.type != 9)
//			{
				AddButton(map);
//			}
		}
	}

	private void AddButton(SceneMap map){
		GameObject btn = DoAddButton(_view.WorldTable.gameObject, map.name);
		UIButton uiButton = btn.GetComponent<UIButton>();
		EventDelegate.Set (uiButton.onClick, delegate() {
			WorldManager.Instance.Enter(map.id, false);
			ProxyWorldMapModule.Close();
		});
	}

	public GameObject DoAddButton(GameObject parent, string label, string goName="Button"){
		GameObject go = NGUITools.AddChild(parent, (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BaseUI/BaseButton"));
		go.name = goName;
		go.GetComponentInChildren< UILabel >().text = label;
		return go;
	}
}

