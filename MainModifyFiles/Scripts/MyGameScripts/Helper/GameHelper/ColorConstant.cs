using UnityEngine;
using System.Collections;

/// <summary>
/// Color constant.
/// </summary>
public class ColorConstant
{
	//战斗内外名字类，玩家信息， 玩法， 活动名字，道具名
	public const string Color_Name_Str = "39EB3C";
	public static Color Color_Name = ColorExt.HexToColor(Color_Name_Str);
	
	//特殊提醒信息，如耗值
	public const string Color_Tip_Str = "fc7b6a";
	public static Color Color_Tip = ColorExt.HexToColor(Color_Tip_Str);
	
	//队伍频道信息
	public const string Color_Channel_Team_Str = "d4862f";
	public static Color Color_Channel_Team = ColorExt.HexToColor(Color_Channel_Team_Str);
	
	//综合频道信息
	public const string Color_Channel_Zonghe_Str = "a14ad9";
	public static Color Color_Channel_Zonghe = ColorExt.HexToColor(Color_Channel_Zonghe_Str);
	
	//帮派频道
	public const string Color_Channel_Guild_Str = "0882d6";
	public static Color Color_Channel_Guild = ColorExt.HexToColor(Color_Channel_Guild_Str);
	
	//系统频道
	public const string Color_Channel_System_Str = "d3a017";
	public static Color Color_Channel_System = ColorExt.HexToColor(Color_Channel_System_Str);
	
	//称谓
	public const string Color_Title_Str = "4DB0D6";//"1899FF";
	public static Color Color_Title = ColorExt.HexToColor(Color_Title_Str);
	
	//当前装备不可用，没激活状态
	public const string Color_UnActive_Str = "b4b5b5";
	public static Color Color_UnActive = ColorExt.HexToColor(Color_UnActive_Str);
	
	//名字类（玩家聊天时显示的名字为白色可查信息）
	public const string Color_ChatName_Str = "fff9e3";
	public static Color Color_ChatName = ColorExt.HexToColor(Color_ChatName_Str);
	
	//战斗内
	public const string Color_Battle_Str = "d3a017";
	public static Color Color_Battle = ColorExt.HexToColor(Color_Battle_Str);
	
	//战斗己方名字
	public const string Color_Battle_Player_Name_Str = "39EB3C";//"5cf37c";
	public static Color Color_Battle_Player_Name = ColorExt.HexToColor(Color_Battle_Player_Name_Str);
	
	//战斗敌方名字
	public const string Color_Battle_Enemy_Name_Str = "E7BD37";//"d3a017";
	public static Color Color_Battle_Enemy_Name = ColorExt.HexToColor(Color_Battle_Enemy_Name_Str);

	//技能未达到条件提示
	public const string Color_Battle_SkillCanNotUseTip_Str = "ff0000";
	public static Color Color_Battle_SkillCanNotUseTip = ColorExt.HexToColor(Color_Battle_SkillCanNotUseTip_Str);

	public const string Color_Battle_SkillCanUseTip_Str = "6F3E1A";
	public static Color Color_Battle_SkillCanUseTip = ColorExt.HexToColor(Color_Battle_SkillCanUseTip_Str);

	//小地图_NPC_闲人
	public const string Color_MiniMap_Npc_Idle_Str = "ffec6c";
	public static Color Color_MiniMap_Npc_Idle = ColorExt.HexToColor(Color_MiniMap_Npc_Idle_Str);
	
	//小地图_NPC_功能
	public const string Color_MiniMap_Npc_Function_Str = "96f86f";
	public static Color Color_MiniMap_Npc_Function = ColorExt.HexToColor(Color_MiniMap_Npc_Function_Str);
	
	//界面内 小标题及文字描述颜色
	public const string Color_UI_Title_Str = "6f3e1a";
	public static Color Color_UI_Title = ColorExt.HexToColor(Color_UI_Title_Str);
	
	//界面内 按钮字体颜色
	public const string Color_UI_Tab_Str = "fff9e3";
	public static Color Color_UI_Tab = ColorExt.HexToColor(Color_UI_Tab_Str);
	
	//提示 物品颜色
	public const string Color_Tip_Item_Str = "d3a017";
	public static Color Color_Tip_Item = ColorExt.HexToColor(Color_Tip_Item_Str);
	
	//提示 获得货币颜色
	public const string Color_Tip_GainCurrency_Str = "5cf37c";
	public static Color Color_Tip_GainCurrency = ColorExt.HexToColor(Color_Tip_GainCurrency_Str);

	//提示 消耗货币颜色
	public const string Color_Tip_LostCurrency_Str = "fc7b6a";
	public static Color Color_Tip_LostCurrency = ColorExt.HexToColor(Color_Tip_LostCurrency_Str);
	
    //tips 装备特技名字颜色
    public const string Color_Equip_Skill_Str = "e983f5";
    public static Color Color_Equip_Skill = ColorExt.HexToColor(Color_Equip_Skill_Str);
}