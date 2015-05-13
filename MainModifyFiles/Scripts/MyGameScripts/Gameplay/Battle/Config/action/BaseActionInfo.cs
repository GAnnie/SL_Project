using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseActionInfo
{
	public string type;//动作类型
	public string name; // 播放动作名//
	public int rotateX;
	public int rotateY;
	public int rotateZ;

	public List<BaseEffectInfo> effects;

	public void FillInfo(JsonActionInfo info)
	{
		type = info.type;
		name = info.name;
		rotateX = info.rotateX;
		rotateY = info.rotateY;
		rotateZ = info.rotateZ;

		effects = ToBaseActionInfoList (info.effects);
	}

	private List<BaseEffectInfo> ToBaseActionInfoList(List<JsonEffectInfo> jsonList)
	{
		List<BaseEffectInfo> list = new List<BaseEffectInfo> ();
		
		foreach(JsonEffectInfo json in jsonList)
		{
			list.Add(json.ToBaseEffectInfo());
		}
		
		return list;
	}
}
