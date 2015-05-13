using UnityEngine;
using System.Collections;

public class NpcAppearInst :BaseNpcInst{

	//npc 出现指令

	public const string TYPE = "NpcAppearInst";



	//模型
	public int model;

	//名字
	public string name;

	//位置信息
	public float posX;
	public float posY;
	public float posZ;


	//朝向
	public float rotY;

	//默认动作
	public string defaultAnim;

	//消失时间
	public float delayTime;

	//匹配玩家模型
	public bool copyHero;

	//模型大小
	public float scaleX;
	public float scaleY;
	public float scaleZ;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		NpcAppearInst info = new NpcAppearInst ();
		info.FillInfo (json);
		info.model = json.model;
		info.posX = json.posX;
		info.posY = json.goPosY;
		info.posZ = json.posZ;
		info.rotY = json.rotY;
		info.defaultAnim = json.defaultAnim;
		info.npcid = json.npcid;
		info.delayTime = json.delayTime;
		info.name = json.name;
		info.copyHero = json.copyHero;
		info.scaleX = json.scaleX;
		info.scaleY = json.scaleY;
		info.scaleZ = json.scaleZ;
		return info;
	}

}
