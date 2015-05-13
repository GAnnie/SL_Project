using UnityEngine;
using System.Collections;

public class JsonEffectInfo
{
	public string type;
	public float playTime;

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

	public BaseEffectInfo ToBaseEffectInfo()
	{
		BaseEffectInfo info = null;
		switch (type){
		case NormalEffectInfo.TYPE:
			info = NormalEffectInfo.ToBaseEffectInfo(this);
			break;
		case ShowInjureEffectInfo.TYPE:
			info = ShowInjureEffectInfo.ToBaseEffectInfo(this);
			break;
		case TakeDamageEffectInfo.TYPE:
			info = TakeDamageEffectInfo.ToBaseEffectInfo(this);
			break;
		case SoundEffectInfo.TYPE:
			info = SoundEffectInfo.ToBaseEffectInfo(this);
			break;
		}
		return info;
	}
}

