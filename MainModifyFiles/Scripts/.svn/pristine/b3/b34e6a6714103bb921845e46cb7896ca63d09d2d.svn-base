using UnityEngine;
using System.Collections;

public class NpcTalkInst : BaseNpcInst{
	//说话指令

	public const string TYPE = "NpcTalkInst";

	//说话内容
	public string talkStr;
	//持续时间
	public float delayTime;

	//气泡高度
	public float offY;

	//气泡存在时间
	public float existTime;




	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcTalkInst info = new NpcTalkInst ();
		info.FillInfo (json);
		info.talkStr = json.talkStr;
		info.delayTime = json.delayTime;
		info.npcid = json.npcid;
		info.offY = json.offY;
		info.existTime = json.existTime;
		return info;
	}

}
