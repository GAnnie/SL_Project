using UnityEngine;
using System.Collections;

public class MoveBackActionInfo : BaseActionInfo
{
	public const string TYPE = "moveBack";
	
	public float time; //move'speed

	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		MoveBackActionInfo info = new MoveBackActionInfo ();
		info.FillInfo (json);
		info.time = json.time;
		
		return info;
	}
}

