using UnityEngine;
using System.Collections;

public class JsonStoryInst  {

	public string type;
	public float startTime;
	public int id;
	#region npcAppear

	public int npcid;

	public string name;

	
	//模型
	public int model;
	
	//位置信息
	public float posX;
	public float posY;
	public float posZ;
	
	//朝向
	public float rotY;
	
	//默认动作
	public string defaultAnim;

	//克隆当前玩家模型
	public bool copyHero;
    //模型大小
	public float scaleX;
	public float scaleY;
	public float scaleZ;
	#endregion
	


	#region npcTalk
	
	//说话内容
	public string talkStr;
	//持续时间
	public float turnSpeed;
	//popo
	public float offY;
	//气泡存在时间
	public float existTime;
	#endregion
	
	#region npcMove
	//要去的位置
	public float goPosX;
	public float goPosY;
	public float goPosZ;

	#endregion
	//npc eff
	public string NpcEffPath;
	
	public string anim;
	public float TurnAngle;
//	#region 镜头平移
//	//持续时间
//	public float delayTime;
//	#endregion

	
	#region 屏幕震动
	//持续时间
//	public float delayTime;
	//摇晃程度
	public float sensitive;
	
	//是否淡入淡出
	
	public bool fade;
	#endregion
	
	#region 屏幕蒙版
	//颜色值
	public float colorR;
	public float colorG;
	public float colorB;
	
	//透明度
	public float alpha;
	public float fadeTime;
	
//	//持续时间
//	public float delayTime;
//	
//	
//	//是否淡入淡出
//	public bool fade;
	#endregion
	
	
	
//	
//	#region 屏幕压屏
//	//持续时间
//	public float delayTime;
//	
	//压屏的长度
	public float length;
	//压屏时间
	public float needTime;
//	
//	//是否淡入淡出
//	public bool fade;
//	#endregion
	
	
	
	
	
	
	#region 特效
	//特效类型
	public string effPath;
	
//	//特效的位置
//	public float posX;
//	public float posY;
//	public float posZ;
	
	
	//持续时间
//	public float delayTime;
	
	//是否全屏
	public bool isFullScreen;
	#endregion

	#region music
	//文件路径
	public string musicPath;
	
	
//	//持续时间
	public float delayTime;
	#endregion


	#region audio
	//文件路径
	public string audioPath;
	
	
//	//持续时间
//	public float delayTime;
    #endregion

	public BaseStoryInst  ToBaseActionInfo()
	{
		BaseStoryInst info = null;
		switch (type){
		case NpcAppearInst.TYPE:
			info = NpcAppearInst.ToBaseActionInfo(this);
			break;
		case NpcTalkInst.TYPE:
			info = NpcTalkInst.ToBaseActionInfo(this);
			break;
		case NpcMoveInst.TYPE:
			info = NpcMoveInst.ToBaseActionInfo(this);
			break;
		case EffAppearInst.TYPE:
			info = EffAppearInst.ToBaseActionInfo(this);
			break;
		case CameraTranslationInst.TYPE:
			info = CameraTranslationInst.ToBaseActionInfo(this);
			break;
		case ScreenShockInst.TYPE:
			info = ScreenShockInst.ToBaseActionInfo(this);
			break;
		case ScreenMaskInst.TYPE:
			info = ScreenMaskInst.ToBaseActionInfo(this);
			break;
		case ScreenPresureInst.TYPE:
			info = ScreenPresureInst.ToBaseActionInfo(this);
			break;
		case MusicPlayInst.TYPE:
			info = MusicPlayInst.ToBaseActionInfo(this);
			break;
		case AudioPlayInst.TYPE:
			info = AudioPlayInst.ToBaseActionInfo(this);
			break;
		case NpcDeleteInst.TYPE:
			info = NpcDeleteInst.ToBaseActionInfo(this);
			break;
		case NpcActionInst.TYPE:
			info = NpcActionInst.ToBaseActionInfo(this);
			break;
		case NpcTurnaroundInst.TYPE:
			info = NpcTurnaroundInst.ToBaseActionInfo(this);
			break;
		case NpcEffInst.TYPE:
			info = NpcEffInst.ToBaseActionInfo(this);
			break;
		}
		return info;
	}


}
