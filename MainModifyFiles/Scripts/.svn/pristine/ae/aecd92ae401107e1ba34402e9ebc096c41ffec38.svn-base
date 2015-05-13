using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.battle.data;

public class BattleTargetSelector
{
	public enum TargetType
	{
		ALL = 0,	 //0.全体
		PLAYER = 1,	 //1.我军
		ENEMY = 2,   //2.敌人 
		SELF = 3, //仅自己
		NONE,
	}
	
	public enum SelfType
	{
		SELF = 0, //包括自己
		NOTSELF = 1, //不包括自己
	}	

	public enum CharacterType
	{
		ALL = 0, //全部
		PET = 1, //宠物
		NOTPET = 2, //非宠物
	}	

	public enum LifeState
	{
		ALL = 0,	 //0.全部
		ALIVE = 1,	 //1.存活
		DEAD = 2,   //2.死亡
	}

	public enum SelectorType
	{
		CAPTURE = 0,
		SKILL = 1,
		ITEM = 2,
	}

	private bool 		singleTarget; //是否单体
	private TargetType  targetType; //目标类型
	private CharacterType  characterType; //角色类型
	private LifeState 	lifeState; //0全部  1存活  2死亡
	private SelectorType selectorType; //选择类型
	private SelfType selfType; //是否包括自己
	
	private int _selectParam; //参数
	
	private Skill skill;
	
	private MonsterController monsterSource;
	private long monsterTargets;
	//------------------------------------------------------------------------------------------------------
	#region
	public BattleTargetSelector( Skill monsterSkill )
	{
		if ( monsterSkill == null )
		{
			GameDebuger.Log( "BattleTargetSelector:monsterSkill == null ");
			return;
		}

		skill = monsterSkill;
		
		switch (skill.skillAiId)
		{
			//		1、敌方全体；
		case 1: 
			targetType = TargetType.ENEMY;
			characterType = CharacterType.ALL;
			selfType = SelfType.NOTSELF;
			break;
			//		2、仅自身；
		case 2:
			targetType = TargetType.SELF;
			characterType = CharacterType.ALL;
			selfType = SelfType.SELF;
			break;
			//		3、己方除自身外的单位，包括己方全部人的宠物；
		case 3:
			targetType = TargetType.PLAYER;
			characterType = CharacterType.ALL;
			selfType = SelfType.NOTSELF;
			break;
			//		4、己方所有单位，包括己方全部人的宠物；
		case 4:
			targetType = TargetType.PLAYER;
			characterType = CharacterType.ALL;
			selfType = SelfType.SELF;
			break;
			//		5、仅己方全部宠物；
		case 5:
			targetType = TargetType.PLAYER;
			characterType = CharacterType.PET;
			selfType = SelfType.NOTSELF;
			break;
			//		6、场上除自身外所有单位
		case 6:
			targetType = TargetType.ALL;
			characterType = CharacterType.ALL;
			selfType = SelfType.NOTSELF;
			break;
		default:
			targetType = TargetType.ALL;
			characterType = CharacterType.ALL;
			selfType = SelfType.SELF;
			break;
		}

		lifeState = LifeState.ALL;
		selectorType = SelectorType.SKILL;
	}
	
	public void SetTargets( MonsterController source, long targets )
	{
		monsterTargets = targets;
		monsterSource = source;
	}
	#endregion
	
	public Skill GetSkill()
	{
		return skill;
	}
	
	public bool IsSingleTarget()
	{
		return singleTarget;
	}
	
	public TargetType getTargetType()
	{
		return targetType;
	}
	
	public LifeState GetLifeState()
	{
		return lifeState;
	}
	
	public SelectorType GetSelectorType()
	{
		return selectorType;
	}
	
	public SelfType GetSelfType()
	{
		return selfType;
	}
	
	private bool IsTargetTypeMatch( MonsterController choosePet, MonsterController mc )
	{
		if ( targetType == TargetType.ALL )
			return true;
		
		if ( targetType == TargetType.ENEMY && mc.side == MonsterController.MonsterSide.Enemy )
			return true;
		
		if ( targetType == TargetType.PLAYER && mc.side == MonsterController.MonsterSide.Player )
			return true;

		if ( targetType == TargetType.SELF && choosePet.GetId() == mc.GetId())
			return true;
		
		return false;
	}
	
	private bool IsLifeStateMatch( MonsterController mc )
	{
		GameDebuger.Log( lifeState.ToString() );
		if ( lifeState == LifeState.ALL )
			return true;
		
		if ( lifeState == LifeState.DEAD && mc.IsDead() )
			return true;
		
		if ( lifeState == LifeState.ALIVE && !mc.IsDead() )
			return true;
		
		return false;
	}
	
	private bool IsSelfTypeMatch(MonsterController choosePet, MonsterController mc)
	{
		if (selfType == SelfType.NOTSELF){
			return choosePet.GetId() != mc.GetId();
		}else{
			return true;
		}
	}
	
	private bool IsCharacterTypeMatch(MonsterController choosePet, MonsterController mc)
	{
		if (characterType == CharacterType.PET) {
			return mc.IsPet();
		} else if (characterType == CharacterType.NOTPET) {
			return !mc.IsPet();
		} else {
			return true;
		}
	}

	public bool CanSetTarget( MonsterController choosePet, MonsterController mc )
	{

		if (skill != null && skill.id == BattleManager.GetDefenseSkillId()){		
			return false;
		}
		
		if ( !IsTargetTypeMatch( choosePet, mc ) )
			return false;
//		
//		if ( !IsLifeStateMatch( mc ) )
//			return false;

		if ( !IsSelfTypeMatch( choosePet, mc ) )
			return false;		

		if ( !IsCharacterTypeMatch( choosePet, mc ) )
			return false;	

		if (skill.id == BattleManager.GetCaptureSkillId())
		{
			if (mc.IsMonster())
			{
				Pet pet = mc.videoSoldier.monster.pet;
				if (pet != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
//		if ( mc.GetMP() >= skill.mp )
//			return true;
	
		return true;
	}

	public bool CanSetCaptureTarget(MonsterController choosePet, MonsterController mc)
	{
		if (skill.id == BattleManager.GetCaptureSkillId())
		{
			if (mc.IsMonster())
			{
				Pet pet = mc.videoSoldier.monster.pet;
				if (PlayerModel.Instance.GetPlayerLevel() > pet.companyLevel)
				{
					return true;
				}
				else
				{
					TipManager.AddTip(string.Format("捕捉{0}需要等级{1}级", pet.name.WrapColor(ColorConstant.Color_Tip_Item), pet.companyLevel.WrapColor(ColorConstant.Color_Tip_GainCurrency)));
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		else
		{
			return true;
		}
	}

	public bool IsMainCharactor()
	{
		return !monsterSource.IsPet ();
	}

	public bool IsItemSkill()
	{
		return skill.id == BattleManager.GetUseItemSkillId();
	}

	public int GetSkillId()
	{
		return skill.id;
	}

	public int GetSkillLogicId()
	{
		return skill.logicId;
	}

	public long GetTargetSoldierId()
	{
		return monsterTargets;
	}

	public string getSelectParam()
	{
		return string.Format ("IsMainCharactor={0} GetSkillId={1} GetTargetSoldierId={2}", IsMainCharactor (), GetSkillId (), GetTargetSoldierId ());
	}
}
