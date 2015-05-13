using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public sealed class BattleConfigManager
{
	public Dictionary<int, SkillConfigInfo> _configDict;
	
    private BattleConfigManager()
    {
		_configDict = new Dictionary<int, SkillConfigInfo> ();
    }
	
	private static readonly BattleConfigManager instance = new BattleConfigManager();
    public static BattleConfigManager Instance
    {
        get
		{
			return instance;
		}
    }

	private const string BattleConfig_ReadPath = "ConfigFiles/BattleConfig/BattleConfig";

    public void Setup()
    {
		JsonBattleConfigInfo config = DataHelper.GetJsonFile<JsonBattleConfigInfo>(BattleConfig_ReadPath, "bytes", false);
		
		if (config != null)
		{
			_configDict.Clear();

			foreach(JsonSkillConfigInfo info in config.list)
			{
				_configDict.Add(info.id, info.ToSkillConfigInfo());
			}
		}
    }


	public SkillConfigInfo getSkillConfigInfo(int skillID)
	{
		int key = skillID;
		
		if (_configDict == null)
		{
			return null;
		}
		
		SkillConfigInfo skillConfigInfo = null;
		
		_configDict.TryGetValue( key, out skillConfigInfo );

		return skillConfigInfo;
	}
}
