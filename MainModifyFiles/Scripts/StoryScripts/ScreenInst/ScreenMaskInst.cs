using UnityEngine;
using System.Collections;

public class ScreenMaskInst :BaseStoryInst {

	//屏幕蒙版指令

	public const string TYPE = "ScreenMaskInst";

	//颜色值
	public float colorR;
	public float colorG;
	public float colorB;

	//透明度
	public float alpha;
	
	
	//持续时间
	public float delayTime;


	//是否淡入淡出
	public bool fade;
	public float fadeTime;

	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		ScreenMaskInst info = new ScreenMaskInst ();
		info.FillInfo (json);
		info.colorR = json.colorR;
		info.colorG = json.colorG;
		info.colorB = info.colorB;
		info.alpha = json.alpha;
		info.delayTime = json.delayTime;
		info.fade = json.fade;
		info.fadeTime = json.fadeTime;
		return info;
	}
	
}
