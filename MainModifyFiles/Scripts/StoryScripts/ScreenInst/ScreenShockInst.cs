using UnityEngine;
using System.Collections;

public class ScreenShockInst : BaseStoryInst {

	//震屏指令
	public const string TYPE = "ScreenShockInst";

	//持续时间
	public float delayTime;

	//摇晃程度
	public float sensitive;


	//是否淡入淡出

	public bool fade;



	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		ScreenShockInst info = new ScreenShockInst ();
		info.FillInfo (json);
		info.delayTime = json.delayTime;
		info.fade = json.fade;
		info.sensitive = json.sensitive;
		
		return info;
	}
}
