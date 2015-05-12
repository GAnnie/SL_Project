using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIViewCache : ScriptableObject
{
	public List<UIComponentInfo> componentInfoList;
	public string codePath = "";
	public string codeLuaPath = "";
}

[System.Serializable]
public class UIComponentInfo
{
	public string uid; //UID根据prefab名字+NodeName+ComponentType+Path得出
	public string comType;
	public string memberName; //默认成员名为：NodeName_ComponentType
	public string path;

	public UIComponentInfo(GameObject go,Transform prefabRoot){
		this.comType = "GameObject";
		this.memberName = go.name;
		this.path = GameObjectExt.GetHierarchyWithRoot (go.transform, prefabRoot);
		uid = MD5Hashing.HashString(string.Format("{0}:{1}|{2}",prefabRoot.name,this.memberName,this.path));
	}

	public UIComponentInfo(Component component, Transform prefabRoot)
	{
//		this.com = component;
		this.comType = component.GetType ().Name;
		this.memberName = string.Format ("{0}_{1}", component.name,comType); 
		this.path = GameObjectExt.GetHierarchyWithRoot (component.transform, prefabRoot);
		uid = MD5Hashing.HashString(string.Format("{0}:{1}|{2}",prefabRoot.name,this.memberName,this.path));
	}

	public UIComponentInfo(UIComponentInfo info)
	{
		this.uid = info.uid;
		this.comType = info.comType;
		this.memberName = info.memberName;
		this.path = info.path;
	}
	
	public string GetName()
	{
		return string.Format("{0} {1}",comType,memberName);
	}
}

