using UnityEngine;
using System.Collections;

public class GameEffectConst
{
	//战斗角色撤退灰尘
	public const string Effect_Retreat = "game_eff_2005";

	//战斗角色选择
	public const string Effect_TargetSelect = "game_eff_2014";

	//战斗角色点击
	public const string Effect_TargetClick = "game_eff_2020";

	//角色点击光圈
	public const string Effect_CharactorClick = "game_eff_2017";

	//技能名字显示背景
	public const string Effect_SkillName = "game_eff_2002";

	//主人物升级
	public const string Effect_PlayerUpgrade = "game_eff_2001";

	//防御特效
	public const string Effect_Defence = "game_eff_2003";

	//防御特效
	public const string Effect_Catch = "game_eff_2006";

	//Summon特效
	public const string Effect_Summon = "game_eff_2007";

	//点击地面特效
	public const string Effect_TerrainClick = "game_eff_2012";

	public static string GetGameEffectPath(string effectType)
	{
		string effpath = PathHelper.GetEffectPath (effectType);
		return effpath;
	}
}