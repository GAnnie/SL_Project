using UnityEngine;
using System.Collections;

public class SoundEffectInfo : BaseEffectInfo
{
	public const string TYPE = "Sound";

	public string name;
	
	static public BaseEffectInfo ToBaseEffectInfo(JsonEffectInfo json)
	{
		SoundEffectInfo info = new SoundEffectInfo ();
		info.FillInfo (json);
		info.name = json.name;
		return info;
	}
}

