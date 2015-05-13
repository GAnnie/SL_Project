using UnityEngine;
using System.Collections;

public class NpcDeleteInst : BaseNpcInst {

	//npc删除指令
	public const string TYPE = "NpcDeleteInst";

	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcDeleteInst info = new NpcDeleteInst ();
		info.FillInfo (json);
		info.npcid = json.npcid;
		return info;
	}

}
