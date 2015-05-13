using UnityEngine;
using System.Collections;


public class NpcMoveInst :BaseNpcInst {

	public const string TYPE = "NpcMoveInst";



	//要去的位置
	public float goPosX;
	public float goPosY;
	public float goPosZ;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcMoveInst info = new NpcMoveInst ();
		info.FillInfo (json);
		info.goPosX = json.goPosX;
		info.goPosY = json.goPosY;
		info.goPosZ = json.goPosZ;
		info.npcid = json.npcid;
		return info;
	}
}
