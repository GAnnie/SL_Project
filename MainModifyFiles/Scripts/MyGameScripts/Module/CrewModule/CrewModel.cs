using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class CrewPropertyInfo{
	public const int NOBATTLE_POS=999;

	public CrewCharactorDto crewDto;
	public AptitudeProperties apInfo;

	/** 敏捷 */
	public int speed;
	
	/** 攻击力 */
	public int attack;
	
	/** 防御力 */
	public int defense;
	
	/** hp */
	public int hp;
	
	/** mp */
	public int mp;
	
	/** 灵力 */
	public int magic;

	/** 阵法中的位置 999代表未出战*/
	public int pos;

	private Crew _crew;
	public  Crew crew{
		get
		{
			if (_crew !=null){
				return _crew;
			}else{
				_crew = DataCache.getDtoByCls<GeneralCharactor>(crewDto.crewId) as Crew;
				return _crew;
			}
		}
	}

	public CrewPropertyInfo (CrewCharactorDto dto){
		crewDto = dto;
		apInfo = new AptitudeProperties();
		CrewModel.CalculateCrewProperty(this);
	}
}

public class CrewModel {

	private static readonly CrewModel _instance = new CrewModel();
	public static CrewModel Instance{
		get{
			return _instance;
		}
	}

	private CrewModel(){
		_formationPosInfoDic = new Dictionary<int, Vector3>(12);
		_formationPosInfoDic.Add(1,new Vector3(0,0,0));
		_formationPosInfoDic.Add(2,new Vector3(-1,-1,0));
		_formationPosInfoDic.Add(3,new Vector3(1,1,0));
		_formationPosInfoDic.Add(4,new Vector3(-2,-2,0));
		_formationPosInfoDic.Add(5,new Vector3(2,2,0));
		_formationPosInfoDic.Add(6,new Vector3(-1,1,0));
		_formationPosInfoDic.Add(7,new Vector3(-2,0,0));
		_formationPosInfoDic.Add(8,new Vector3(0,2,0));
		_formationPosInfoDic.Add(11,new Vector3(1,-1,0));
		_formationPosInfoDic.Add(12,new Vector3(0,-2,0));
		_formationPosInfoDic.Add(13,new Vector3(2,0,0));
		_formationPosInfoDic.Add(14,new Vector3(2,-2,0));
	}

	private Dictionary<int,Vector3> _formationPosInfoDic;
	private List<CrewPropertyInfo> _crewMemberList;
	public const int MAX_BATTLECOUNT = 4;
	private int _inBattleCrewCount;

	public int InBattleCrewCount {
		get {
			return _inBattleCrewCount;
		}
	}

	public event System.Action<CrewPropertyInfo> OnAddNewCrew;
	public event System.Action OnCrewInfoListUpdate; 	//JoinBattle or OffBattle

	#region Getter
	public Vector3 GetFormationPos(int pos){
		if(_formationPosInfoDic.ContainsKey(pos)){
			return _formationPosInfoDic[pos];
		}
		return Vector3.zero;
	}

	public int GetCrewCount(){
		if(_crewMemberList != null)
		{
			return _crewMemberList.Count;
		}
		return 0;
	}


	public List<CrewPropertyInfo> GetAllCrewInfoList(){
		return _crewMemberList;
	}


	public CrewPropertyInfo GetCrewInfoByIndex(int index){
		if (index == -1 || _crewMemberList == null)
			return null;
		
		if (index < _crewMemberList.Count)
			return _crewMemberList[index];
		
		return null;
	}
		
	public CrewPropertyInfo GetCrewInfoByUID(long crewUID){
		if (_crewMemberList == null)
			return null;
		
		for (int i=0; i<_crewMemberList.Count; ++i) {
			if (_crewMemberList [i].crewDto.crewUniqueId == crewUID)
				return _crewMemberList [i];
		}
		
		return null;
	}

	public bool ContainCrew(int crewId){
		if(_crewMemberList == null) return false;
		for(int i=0;i<_crewMemberList.Count;++i){
			if(_crewMemberList[i].crewDto.crewId == crewId)
				return true;
		}

		return false;
	}
	#endregion

	#region GameLogic
	public static int SortByPos(CrewPropertyInfo a,CrewPropertyInfo b){
		return a.pos.CompareTo(b.pos);
	}
	#endregion

	#region 请求接口
	public void RequestCrewInfo (System.Action onSuccess)
	{
		if(_crewMemberList != null)
		{
			if(onSuccess !=null)
				onSuccess();
		}
		else{
			ServiceRequestAction.requestServer (CrewService.enterCrewPanel (), "获取伙伴信息", (e) => {
				CrewPanelDto crewPanelDto = e as CrewPanelDto;
				_inBattleCrewCount = 0;
				_crewMemberList = new List<CrewPropertyInfo>(crewPanelDto.crewCharactors.Count);
				for(int i=0;i<crewPanelDto.crewCharactors.Count;++i){
					CrewPropertyInfo crewInfo = new CrewPropertyInfo(crewPanelDto.crewCharactors[i]);
					_crewMemberList.Add(crewInfo);
					if(crewInfo.crewDto.inBattleTeam){
						crewInfo.pos = _inBattleCrewCount++;
					}else
						crewInfo.pos = CrewPropertyInfo.NOBATTLE_POS;
				}
				
				if (onSuccess != null)
					onSuccess ();
			});
		}
	}

	public void JoinBattle(int crewIndex){
		if(_inBattleCrewCount < MAX_BATTLECOUNT){
			CrewPropertyInfo crewInfo = GetCrewInfoByIndex(crewIndex);
			ServiceRequestAction.requestServer(CrewService.joinBattle(crewInfo.crewDto.crewUniqueId),"Crew JoinBattle",(e)=>{
				crewInfo.crewDto.inBattleTeam = true;
				crewInfo.pos = _inBattleCrewCount++;

				_crewMemberList.Sort(SortByPos);
				if(OnCrewInfoListUpdate != null)
					OnCrewInfoListUpdate();
			});
		}else
			TipManager.AddTip("参战伙伴已满");
	}

	public void OffBattle(int crewIndex){
		CrewPropertyInfo crewInfo = GetCrewInfoByIndex(crewIndex);
		ServiceRequestAction.requestServer(CrewService.offBattle(crewInfo.crewDto.crewUniqueId),"Crew OffBattle",(e)=>{
			crewInfo.crewDto.inBattleTeam = false;
			crewInfo.pos = CrewPropertyInfo.NOBATTLE_POS;
			--_inBattleCrewCount;

			_crewMemberList.Sort(SortByPos);
			if(OnCrewInfoListUpdate != null)
				OnCrewInfoListUpdate();
		});
	}

	public void ChangeFormationCase(int newFormationCaseIndex){
		ServiceRequestAction.requestServer(CrewService.changeFormationCase(newFormationCaseIndex),"ChangeFormationCase",(e)=>{
			PlayerModel.Instance.CurrentSelectFormationCaseIndex = newFormationCaseIndex;
			if(newFormationCaseIndex != PlayerModel.DEFENSE_FORMATION_INDEX)
				PlayerModel.Instance.ActiveFormationCaseIndex = newFormationCaseIndex;

			//根据FormationCaseChangeDto信息更新_crewMemberList中的上阵信息
			FormationCaseChangeDto formationCaseChangeDto = e as FormationCaseChangeDto;
			//重置所有伙伴上阵状态
			_inBattleCrewCount = 0;
			for(int i=0;i<_crewMemberList.Count;++i){
				_crewMemberList[i].crewDto.inBattleTeam = false;
				_crewMemberList[i].pos = CrewPropertyInfo.NOBATTLE_POS;
			}

			for(int i=0;i<formationCaseChangeDto.crewUniqueIds.Count;++i){
				long crewUID = formationCaseChangeDto.crewUniqueIds[i];
				CrewPropertyInfo crewInfo = GetCrewInfoByUID(crewUID);
				if(crewInfo != null){
					crewInfo.crewDto.inBattleTeam = true;
					crewInfo.pos = _inBattleCrewCount++;
				}
			}
			_crewMemberList.Sort(SortByPos);
			if(OnCrewInfoListUpdate != null)
				OnCrewInfoListUpdate();
		});
	}

	public void ChangeFormation(int newFormationId,System.Action onSuccess){
		ServiceRequestAction.requestServer(CrewService.changeFormation(PlayerModel.Instance.CurrentSelectFormationCaseIndex,newFormationId),"ChangeFormation",(e)=>{
			PlayerModel.Instance.SetFormationCaseId(PlayerModel.Instance.CurrentSelectFormationCaseIndex,newFormationId);
			
			if(onSuccess != null){
				onSuccess();
			}
		});
	}

	public void RecruitCrew(int crewId,System.Action onSuccess){
		ServiceRequestAction.requestServer(CrewService.recruit(crewId),"RecruitCrew",(e)=>{
			CrewCharactorDto crewDto = e as CrewCharactorDto;
			if(crewDto != null){
				CrewPropertyInfo crewInfo = new CrewPropertyInfo(crewDto);
				_crewMemberList.Add(crewInfo);
				if(crewInfo.crewDto.inBattleTeam){
					crewInfo.pos = _inBattleCrewCount++;
				}else
					crewInfo.pos = CrewPropertyInfo.NOBATTLE_POS;
                
                RecruitFee fee=crewInfo.crew.recruitFee;
				PlayerModel.Instance.UseIngot(fee.ingot);
				PlayerModel.Instance.UseCopper(fee.copper);

				if(OnAddNewCrew != null)
					OnAddNewCrew(crewInfo);

				if(onSuccess != null)
					onSuccess();
			}
		});
	}

	//阵上伙伴位置互换
	public void SwitchBattlePos(CrewPropertyInfo crewA,CrewPropertyInfo crewB){
		ServiceRequestAction.requestServer(CrewService.exchange(crewA.crewDto.crewUniqueId,crewB.pos),"exchange",(e)=>{
			int tempPos = crewA.pos;
			crewA.pos = crewB.pos;
			crewB.pos = tempPos;
			_crewMemberList.Sort(SortByPos);
			if(OnCrewInfoListUpdate != null)
				OnCrewInfoListUpdate();
        });
    }

	//阵下伙伴与阵上伙伴互换
	public void SwitchOnBattlePos(CrewPropertyInfo offBattleCrew,CrewPropertyInfo inBattleCrew){
		ServiceRequestAction.requestServer(CrewService.replace(offBattleCrew.crewDto.crewUniqueId,inBattleCrew.pos),"replace",(e)=>{
			offBattleCrew.crewDto.inBattleTeam = true;
			inBattleCrew.crewDto.inBattleTeam = false;

			int tempPos = offBattleCrew.pos;
			offBattleCrew.pos = inBattleCrew.pos;
			inBattleCrew.pos = tempPos;

			_crewMemberList.Sort(SortByPos);
			if(OnCrewInfoListUpdate != null)
                OnCrewInfoListUpdate();
        });
    }
    #endregion
    
	#region 伙伴属性计算
	public static void CalculateCrewProperty(CrewPropertyInfo crewInfo){
		int playerLv = PlayerModel.Instance.GetPlayerLevel();
		AptitudeProperties defaultApDistrubute = crewInfo.crew.defaultAptitudeDistrubute;
		int constantVal = 10+playerLv; //初始值+每级固定分配1点
		crewInfo.apInfo.constitution = constantVal+playerLv*defaultApDistrubute.constitution;
		crewInfo.apInfo.intelligent = constantVal+playerLv*defaultApDistrubute.intelligent;
		crewInfo.apInfo.strength = constantVal+playerLv*defaultApDistrubute.strength;
		crewInfo.apInfo.stamina = constantVal+playerLv*defaultApDistrubute.stamina;
		crewInfo.apInfo.dexterity = constantVal+playerLv*defaultApDistrubute.dexterity;

		BaseAptitudeProperties baseApInfo = crewInfo.crew.baseAptitudeProperties;
		int growth = crewInfo.crew.growth;
		float hpFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_HP_FACTOR,0.85f);
		float mpFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_MP_FACTOR,1.3f);
		float atkFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_ATTACK_FACTOR,0.5f);
		float defFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_DEFENSE_FACTOR,0.45f);
		float speedFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_SPEED_FACTOR,0.85f);
		float magicFactor = DataHelper.GetStaticConfigValuef(H1StaticConfigs.CREW_MAGIC_FACTOR,0.65f);

		float hp =((baseApInfo.physical*(playerLv*5.0f+playerLv*playerLv/100.0f)+crewInfo.apInfo.constitution*5.0f*growth)/1000.0f+20.0f)*hpFactor;
		float mp =playerLv*6.5f+(crewInfo.apInfo.intelligent+crewInfo.apInfo.strength)*mpFactor;
		float atk =(((baseApInfo.attack*playerLv*2.0f*(growth/2000.0f+0.7f)+crewInfo.apInfo.strength*0.75f*growth)/1000.0f+50.0f)*4.0f/3.0f)*atkFactor;
		float def =((baseApInfo.defense*playerLv*1.75f*(growth/2000.0f+0.7f)+crewInfo.apInfo.stamina*1.5f*growth)/1000.0f)*defFactor;
		float speed =(baseApInfo.speed*(crewInfo.apInfo.constitution*0.1f+crewInfo.apInfo.intelligent*0.1f+crewInfo.apInfo.strength*0.1f+crewInfo.apInfo.stamina*0.1f+crewInfo.apInfo.dexterity*0.7f)/1000.0f*(growth/2000.0f+0.5f))*speedFactor;
		float magic =((crewInfo.apInfo.constitution*0.1f+crewInfo.apInfo.intelligent*0.7f+crewInfo.apInfo.strength*0.4f+crewInfo.apInfo.stamina*0.1f)*(growth/2000.0f+0.4f)+baseApInfo.magic/1000.0f*playerLv)*magicFactor;

		crewInfo.hp = Mathf.FloorToInt(hp);
		crewInfo.mp = Mathf.FloorToInt(mp);
		crewInfo.attack = Mathf.FloorToInt(atk);
		crewInfo.defense = Mathf.FloorToInt(def);
		crewInfo.speed = Mathf.FloorToInt(speed);
		crewInfo.magic = Mathf.FloorToInt(magic);
	}

	public void CalculateAllCrewPropertyInfo(){
		if(_crewMemberList != null){
			for(int i=0;i<_crewMemberList.Count;++i){
				CrewModel.CalculateCrewProperty(_crewMemberList[i]);
			}
		}
	}
	#endregion
}
