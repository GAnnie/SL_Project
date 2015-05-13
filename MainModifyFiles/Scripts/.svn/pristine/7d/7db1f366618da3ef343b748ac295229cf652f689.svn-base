using UnityEngine;
using System.Collections;

public class CameraTranslationInst : BaseStoryInst {

	//镜头平移指令
	public const string TYPE = "CameraTranslationInst";

	//位置信息
	public float posX;
	public float posY;
	public float posZ;

	//持续时间
	public float delayTime;

	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		CameraTranslationInst info = new CameraTranslationInst ();
		info.delayTime = json.delayTime;
		info.posX = json.posX;
		info.posY = json.posY;
		info.posZ = json.posZ;
		info.FillInfo (json);
		return info;
	}

}
