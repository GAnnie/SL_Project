using UnityEngine;
using System.Collections;

public class TakeDamageEffectInfo : BaseEffectInfo
{
	public const string TYPE = "TakeDamage";

	static public BaseEffectInfo ToBaseEffectInfo(JsonEffectInfo json)
	{
		TakeDamageEffectInfo info = new TakeDamageEffectInfo ();
		info.FillInfo (json);
		
		return info;
	}
}

