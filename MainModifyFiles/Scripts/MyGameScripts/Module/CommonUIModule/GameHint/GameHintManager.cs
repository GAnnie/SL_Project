using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.core.modules.charactor.data;

/// <summary>
/// Game hint manager.
/// 用于显示游戏中的帮助提示
/// </summary>
public static class GameHintManager{

	public const float FADEOUT_TIME = 3f;
	private const string VIEWNAME ="Prefabs/Module/CommonUIModule/GameHintView";

	private static GameHintViewController _instance;
	public static void Open(GameObject target,string hint,GameHintViewController.Side pos = GameHintViewController.Side.Center){
		if(string.IsNullOrEmpty(hint))
			return;

		InitGameHintManager();
		_instance.Open(target,hint,pos);
	}

	public static void Open(GameObject target,int hintId,GameHintViewController.Side pos = GameHintViewController.Side.Center){
		if(hintId == 0) return;

		string hint = GetHintIDString(hintId);
		Open(target,hint,pos);
	}

	private static void InitGameHintManager(){
		GameObject view = UIModuleManager.Instance.OpenFunModule(VIEWNAME,UILayerType.FloatTip,false);
		if(_instance == null){
			_instance = view.GetMissingComponent<GameHintViewController>();
			_instance.hintLbl = view.GetComponentInChildren<UILabel>();
			_instance.hintBg = view.GetComponentInChildren<UISprite>();
			_instance.posAnchor = view.GetComponentInChildren<UIAnchor>();
		}
	}

	public static void Close(){
		UIModuleManager.Instance.HideModule(VIEWNAME);
	}

	public static void Dispose(){
		UIModuleManager.Instance.CloseModule(VIEWNAME);
		_instance = null;
	}
	
	public static string GetHintIDString(int hintId){
		switch(hintId){
		case 1:
			return "战斗中受到物理攻击和法术攻击则减少，为0时死亡";
		case 2:
			return "战斗中使用法术时消耗，为0时法术无法使用";
		case 3:
			return "物理攻击";
		case 4:
			return "物理防御";
		case 5:
			return "出手速度";
		case 6:
			return "法术攻击和法术防御";
		case 7:
			return "1点体质增加10点气血、0.1点速度、0.1点灵力";
		case 8:
			return "1点魔力增加0.7点灵力";
		case 9:
			return "1点力量增加0.7点攻击、0.1点速度、0.4点灵力";
		case 10:
			return "1点耐力增加1.5点防御、0.1点速度、0.1点灵力";
		case 11:
			return "1点敏捷增加0.7点速度";
		case 12:
			return "当前可支配属性点";
		case 13:
			return "影响攻击";
		case 14:
			return "影响防御";
		case 15:
			return "影响气血上限";
		case 16:
			return "影响灵力";
		case 17:
			return "影响速度";
		case 18:
			return "宠物寿命低于50将无法出战，宠物出战会扣除1点寿命，死亡会扣除50点寿命。\n可通过喂食珍珠丸子、长寿面、金香果增加寿命。";
		case 19:
			return "攻击资质影响宠物物理攻击力";
		case 20:
			return "防资资质影响宠物物理防御力";
		case 21:
			return "体资资质影响宠物气血上限";
		case 22:
			return "法资资质影响宠物灵力";
		case 23:
			return "速资资质影响宠物速度";
		case 24:
			return "主要影响气血上限，对灵力和速度有一定影响";
		case 25:
			return "主要影响灵力，对速度有一定影响";
		case 26:
			return "主要影响攻击，对灵力和速度有一定影响";
		case 27:
			return "主要影响防御，对灵力和速度有一定影响";
		case 28:
			return "影响速度";
		default:
			return "";
		}
	}

	public static string GetFactionHintString(int factionId){
		switch(factionId){
		case Faction.FactionType_DaTang:
			return "门派特色：\n攻击宠物增加5%的伤害结果";
		case Faction.FactionType_HuaSheng:
			return "门派特色：\n每回合有10%几率自动解除被封印效果";
		case Faction.FactionType_FangCun:
			return "门派特色：\n对鬼魂系敌人的物理、法术伤害加倍；\n【长生】：受法术恢复气血时效果增加20%";
		case Faction.FactionType_TianGong:
			return "门派特色：\n15%几率躲避负面法术";
		case Faction.FactionType_LongGong:
			return "门派特色：\n使用攻击性法术不受负面法术抵抗率的影响";
		case Faction.FactionType_PuTuo:
			return "门派特色：\n倒地时有20%几率出现神佑效果，气血恢复15%。\n如果有其他神佑效果则几率提高10%";
		case Faction.FactionType_MoWang:
			return "门派特色：\n10%几率招架敌人的物理攻击，5%几率招架敌人的物理法术攻击";
		case Faction.FactionType_ShiTuo:
			return "门派特色：\n宠物寿命消耗减半，宠物不会逃跑";
		case Faction.FactionType_PanSi:
			return "门派特色：\n【守护网】：被NPC击倒100%几率触发，被非NPC击倒50%几率触发，\n触发后保留1点气血，每场战斗只能触发1次";
		default:
			return "";
		}
	}

	public static string GetPetTypeHintString(int petType,bool ifMutate,bool ifBaobao){
		if(petType == Pet.PetType_Regular || petType == Pet.PetType_Rare){
			if(ifMutate)
				return "1、特殊的宠物宝宝，洗宠或在野外练级时有极低概率遇到\n2、会随机获得一个额外技能，资质较普通宝宝更好";
			else if(ifBaobao)
				return "0级的宠物，洗宠可必定获得，野外练级时有较低概率遇到";
			else
				return "最为普通的宠物，可在各野外场景遇到或在宠物交易中心购买";
		}else if(petType == Pet.PetType_Precious)
			return "珍贵的宠物，集齐80个珍兽之灵可在神兽使者处随机兑换一只";
		else if(petType == Pet.PetType_Myth)
			return "最珍贵的宠物，集齐80个神兽之灵可在神兽使者处随机兑换一只";
		else
			return "";
	}
}
