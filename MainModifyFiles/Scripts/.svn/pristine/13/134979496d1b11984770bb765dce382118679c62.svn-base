using UnityEngine;
using System.Collections;

public class MusicPlayInst : BaseStoryInst {

	//音乐指令
	public const string TYPE = "MusicPlayInst";


	//文件路径
	public string musicPath;


	//持续时间
	public float delayTime;


	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		MusicPlayInst info = new MusicPlayInst ();
		info.FillInfo (json);
		info.delayTime = json.delayTime;
		info.musicPath = json.musicPath;
		
		return info;
	}
}
