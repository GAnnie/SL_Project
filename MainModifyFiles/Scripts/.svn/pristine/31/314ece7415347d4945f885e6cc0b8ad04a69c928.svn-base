using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonStoryInfo  {


	#region 
	public int id;
	public int sceneid;
	public float delayTime;
	public List<JsonStoryInst> instList;
	#endregion

	public StoryInfo ToStoryInfo()
	{
		StoryInfo story = new StoryInfo ();

		story.id = this.id;
		story.sceneid = this.sceneid;
		story.delayTime = this.delayTime;
		story.instList = new List<BaseStoryInst> ();
		if (this.instList != null)
		{
			foreach(JsonStoryInst info in this.instList)
			{
				story.instList.Add(info.ToBaseActionInfo());
			}
		}

		return story;
	}
}
