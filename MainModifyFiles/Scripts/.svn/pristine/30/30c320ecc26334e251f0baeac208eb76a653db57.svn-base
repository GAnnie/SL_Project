using UnityEngine;
using System.Collections;

public class NpcEffInst : BaseNpcInst {

	//npc特效指令
	
	public const string TYPE = "NpcEffInst";



	//特效路径
	public string NpcEffPath;

	//持续时间
	public float delayTime;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcEffInst info = new NpcEffInst ();
		info.FillInfo (json);
		info.npcid = json.npcid;
		info.NpcEffPath = json.NpcEffPath;
		info.delayTime = json.delayTime;
		return info;
	}
}
