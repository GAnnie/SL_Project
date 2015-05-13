using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonSkillConfigInfo
{
	public int id = 0;
	public string name = "";
	
	public List<JsonActionInfo> attackerActions;
	public List<JsonActionInfo> injurerActions;

	public SkillConfigInfo ToSkillConfigInfo()
	{
		SkillConfigInfo info = new SkillConfigInfo ();
		info.id = this.id;
		info.name = this.name;

		info.attackerActions = ToBaseActionInfoList (attackerActions);
		info.injurerActions = ToBaseActionInfoList (injurerActions);

		return info;
	}

	private List<BaseActionInfo> ToBaseActionInfoList(List<JsonActionInfo> jsonList)
	{
		List<BaseActionInfo> list = new List<BaseActionInfo> ();

		foreach(JsonActionInfo json in jsonList)
		{
			list.Add(json.ToBaseActionInfo());
		}

		return list;
	}
}