using UnityEngine;
using System.Collections;

public class NpcActionInst : BaseNpcInst {
	//npc 动作指令


	public const string TYPE = "NpcActionInst";

	//动作
	public string anim;
//	//持续时间
//	public float delayTime;

	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcActionInst info = new NpcActionInst ();
		info.FillInfo (json);
		info.npcid = json.npcid;
		info.anim = json.anim;
		return info;
	}
}
