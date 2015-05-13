using UnityEngine;
using System.Collections;

public class AudioPlayInst : BaseStoryInst {

	//音效
	public const string TYPE = "AudioPlayInst";

	//文件路径
	public string audioPath;


	//持续时间
	public float delayTime;

	static public BaseStoryInst ToBaseActionInfo(JsonStoryInst json)
	{
		AudioPlayInst info = new AudioPlayInst ();
		info.FillInfo (json);
		info.delayTime = json.delayTime;
		info.audioPath = json.audioPath;
		
		return info;
	}
}
