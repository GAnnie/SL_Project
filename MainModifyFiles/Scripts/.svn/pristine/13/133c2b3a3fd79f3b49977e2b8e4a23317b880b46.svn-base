using UnityEngine;
using System.Collections;

public class ScreenPresureInst : BaseStoryInst {

	//压屏指令
	public const string TYPE = "ScreenPresureInst";

	//存在时间
	public float delayTime;

	//压屏的长度
	public float length;

	//压屏需要的时间
	public float needTime;

	//是否淡入淡出
	public bool fade;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		ScreenPresureInst info = new ScreenPresureInst ();
		info.FillInfo (json);
		info.delayTime = json.delayTime;
		info.fade = json.fade;
		info.length = json.length;
		info.needTime = json.needTime;
		return info;
	}

}
