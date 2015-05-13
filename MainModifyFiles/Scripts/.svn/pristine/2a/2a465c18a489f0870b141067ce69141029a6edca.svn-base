using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Multiple selection manager.
/// 用于游戏中多项选择列表
/// </summary>
public static class MultipleSelectionManager{
	
	private const string VIEWNAME ="Prefabs/Module/CommonUIModule/MultipleSelectionView";
	
	private static MultipleSelectionViewController _instance;
	public static MultipleSelectionViewController Open(GameObject target,List<string> btnNameList,System.Action<int> clickCallback,bool leftMode){	
		GameObject view = UIModuleManager.Instance.OpenFunModule(VIEWNAME,UILayerType.FourModule,false);
		if(_instance == null){
			_instance = view.GetMissingComponent<MultipleSelectionViewController>();
			_instance.InitView();
		}
		
		_instance.Open(target,btnNameList,clickCallback,leftMode);

		return _instance;
	}
	
	public static void Close(){
		UIModuleManager.Instance.HideModule(VIEWNAME);
	}

	public static void Dispose(){
		UIModuleManager.Instance.CloseModule(VIEWNAME);
		_instance = null;
	}
}
