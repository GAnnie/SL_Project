using UnityEngine;
using System.Collections;

public class NormalActionInfo : BaseActionInfo
{
	public const string TYPE = "normal";

	public float startTime; //action start time
	public float delayTime;//action delayed time

	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		NormalActionInfo info = new NormalActionInfo ();
		info.FillInfo (json);
		info.startTime = json.startTime;
		info.delayTime = json.delayTime;
		
		return info;
	}
}

