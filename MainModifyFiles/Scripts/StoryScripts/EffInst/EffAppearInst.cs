using UnityEngine;
using System.Collections;

public class EffAppearInst : BaseStoryInst {

	//特效指令
	public const string TYPE = "EffAppearInst";

	//特效类型
	public string effPath;

	//特效的位置
	public float posX;
	public float posY;
	public float posZ;


	//持续时间
	public float delayTime;

	//是否全屏
	public bool isFullScreen = true;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		EffAppearInst info = new EffAppearInst ();
		info.FillInfo (json);
		info.effPath = json.effPath;
		info.posX = json.posX;
		info.posY = json.goPosY;
		info.posZ = json.goPosZ;
		info.delayTime = json.delayTime;
		info.isFullScreen = json.isFullScreen;
		
		return info;
	}
}