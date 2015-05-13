using UnityEngine;
using System.Collections;

public class MoveActionInfo : BaseActionInfo
{
	public const string TYPE = "move";

	public float time;
	public float distance;
	public bool center;

	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		MoveActionInfo info = new MoveActionInfo ();
		info.FillInfo (json);
		info.time = json.time;
		info.distance = json.distance;
		info.center = json.center;

		return info;
	}
}