using UnityEngine;
using System.Collections;

public class BaseEffectInfo
{
	public string type;
	public float playTime;

	public void FillInfo(JsonEffectInfo info)
	{
		type = info.type;
		playTime = info.playTime;
	}
}