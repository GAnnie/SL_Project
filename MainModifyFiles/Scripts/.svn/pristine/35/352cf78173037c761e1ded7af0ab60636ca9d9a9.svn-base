using UnityEngine;
using System.Collections;

public class NormalEffectInfo : BaseEffectInfo
{
	public const string TYPE = "Normal";

	public string name;
	public float delayTime;
	public int target;//特效目标  0默认， 1，场景中心 2，我方中心   3， 敌军中心
	public string mount; //特效绑定点， 只有当特效目标为0时才生效， 留空则表示人物站立点
	public bool loop;//是否循环
	public int loopCount;//循环次数
	public bool follow;//跟随
	public int scale;//缩放 单位100

	public bool hitEff; //是否受击特效
	public bool fly; //是否飞行特效
	public int flyTarget; //飞行指向目标 0默认(技能作用目标)， 1，场景中心 2，我方中心   3， 敌军中心
	public float flyTime; //飞行时间
	public string flyMount; //飞行指向目标绑定点
	
	public float offX;
	public float offY;
	public float offZ;
	
	public int rotX;
	public int rotY;
	public int rotZ;
	
	public float flyOffX; //飞行位移x
	public float flyOffY; //飞行位移y
	public float flyOffZ; //飞行位移z

	static public BaseEffectInfo ToBaseEffectInfo(JsonEffectInfo json)
	{
		NormalEffectInfo info = new NormalEffectInfo ();
		info.FillInfo (json);

		info.name = json.name;
		info.delayTime = json.delayTime;
		info.target = json.target;
		info.mount = json.mount;
		info.loop = json.loop;
		info.loopCount = json.loopCount;
		info.follow = json.follow;
		info.scale = json.scale;

		info.hitEff = json.hitEff;
		info.fly = json.fly;
		info.flyTarget = json.flyTarget;
		info.flyTime = json.flyTime;
		info.flyMount = json.flyMount;

		info.offX = json.offX;
		info.offY = json.offY;
		info.offZ = json.offZ;

		info.rotX = json.rotX;
		info.rotY = json.rotY;
		info.rotZ = json.rotZ;

		info.flyOffX = json.flyOffX;
		info.flyOffY = json.flyOffY;
		info.flyOffZ = json.flyOffZ;

		return info;
	}
}

