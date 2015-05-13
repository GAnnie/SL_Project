using UnityEngine;
using System.Collections;

public class NpcTurnaroundInst : BaseNpcInst {


	//转身指令
	public const string TYPE = "NpcTurnaroundInst";

	//旋转角度
	public float TurnAngle; 

	//旋转时间
	public float turnSpeed;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcTurnaroundInst info = new NpcTurnaroundInst ();
		info.FillInfo (json);
		info.npcid = json.npcid;
		info.turnSpeed = json.turnSpeed;
		info.TurnAngle = json.TurnAngle;
		return info;
	}
}
