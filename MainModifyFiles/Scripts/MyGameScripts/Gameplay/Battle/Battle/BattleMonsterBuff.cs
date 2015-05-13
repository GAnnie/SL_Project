// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleMonsterBuff.cs
// Author   : SK
// Created  : 2013/3/5
// Purpose  : 战斗宠物头上的Buff显示
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.battlebuff.data;

public class BattleMonsterBuff : MonoBehaviour
{
	List<VideoBuffAddTargetState> buffList;

    private Dictionary<int, GameObject> effectMaps;
		
	private MonsterController _monsterController;
	
	public void SetMonster(MonsterController monsterController){
		_monsterController = monsterController;
	}
	
	void Awake(){
		buffList = new List<VideoBuffAddTargetState>();
        effectMaps = new Dictionary<int, GameObject>();		
	}

	public void AddBuff(VideoBuffAddTargetState buffState, bool tip = true)
    {
		if ( buffState == null )
		{
			GameDebuger.Log("buffState == null");
			return;
		}
		
		GameDebuger.LogBattleInfo(_monsterController.videoSoldier.name+" AddBuff "+buffState.battleBuff.name+" id="+buffState.battleBuffId);
		
        AddBuffEffect(buffState.battleBuff);
		//TipManager.AddTip ("Buff " + buffState.battleBuff.name);

        buffList.Add(buffState);
		
//		if (!buffState.buffer.hide && tip){
//	        string info = buffState.buffer.name;
//	
//	        if (buffState.round > 0)
//	        {
//				//_monsterController.AddHUDText(info, GetBuffTipColor(buffState.buffer), 1f);
//	        }			
//		}
		
//		//影响HP和MP上限buff处理
//		if (buffState.bufferId == 113 || buffState.bufferId == 157){
//			if (buffState.buffer.type == Buffer.BufferType_Rate){
//				int maxHp = _monsterController.monsterData.maxHp;
//				maxHp = (int)((float)maxHp*(1f + (float)buffState.effect/10000));
//				_monsterController.monsterData.maxHp = maxHp;
//			}else if (buffState.buffer.type == Buffer.BufferType_Value){
//				_monsterController.monsterData.maxHp += buffState.effect;
//			}
//		}else if(buffState.bufferId == 533){
//			if (buffState.buffer.type == Buffer.BufferType_Rate){
//				int maxMp = _monsterController.monsterData.maxMp;
//				maxMp = (int)((float)maxMp*(1f + (float)buffState.effect/10000));				
//				_monsterController.monsterData.maxMp = maxMp;
//			}else if (buffState.buffer.type == Buffer.BufferType_Value){
//				_monsterController.monsterData.maxMp += buffState.effect;
//			}
//		}
	}
	
	private Color GetBuffTipColor(BattleBuff buffer){
		string colorStr = "fec742";
//		if (buffer.skillAiType == 2){
//			colorStr = "fec742";
//		}else{
//			colorStr = "b054ea";
//		}
		return ColorExt.Deserialize(colorStr);
	}
	
	public void RemoveBuff(VideoBuffAddTargetState buffState)
	{
		if ( buffState == null )
		{
			GameDebuger.Log("buffState == null");
			return;
		}
		
		GameDebuger.LogBattleInfo(_monsterController.videoSoldier.name+" RemoveBuff "+buffState.battleBuff.name+" id="+buffState.battleBuffId);
		
		buffList.Remove(buffState);
		RemoveBuffEffect(buffState.battleBuff);

		//string info = "RemoveBuff "+buff.buffer.name;
		//mText.Add(info, Color.red, 1f);
		
		VideoSoldier monsterData = _monsterController.videoSoldier;
		
//		//影响HP和MP上限buff处理
//		if (buffState.bufferId == 113 || buffState.bufferId == 157){
//			if (buffState.buffer.type == Buffer.BufferType_Rate){
//				int maxHp = monsterData.maxHp;
//				maxHp = (int)((float)maxHp*(1f - (float)buffState.effect/10000));
//				monsterData.maxHp = maxHp;
//			}else if (buffState.buffer.type == Buffer.BufferType_Value){
//				monsterData.maxHp -= buffState.effect;
//			}
//			
//			if (monsterData.hp > monsterData.maxHp){
//				monsterData.hp = monsterData.maxHp;
//			}
//			
//		}else if(buffState.bufferId == 533){
//			if (buffState.buffer.type == Buffer.BufferType_Rate){
//				int maxMp = monsterData.maxMp;
//				maxMp = (int)((float)maxMp*(1f - (float)buffState.effect/10000));
//				monsterData.maxMp = maxMp;
//			}else if (buffState.buffer.type == Buffer.BufferType_Value){
//				monsterData.maxMp -= buffState.effect;
//			}
//			
//			if (monsterData.mp > monsterData.maxMp){
//				monsterData.mp = monsterData.maxMp;
//			}
//		}
	}

    private void AddBuffEffect(BattleBuff buffer)
    {
        if (effectMaps.ContainsKey(buffer.id))
        {
            return;
        }

		if (buffer.animationMount == null)
		{
			buffer.animationMount = ModelHelper.Mount_shadow;
		}

		if (buffer.animation != 0 && !string.IsNullOrEmpty(buffer.animationMount))
        {
			Transform mountTransform = _monsterController.transform.GetChildTransform(buffer.animationMount);
            if (mountTransform == null)
            {
                return;
            }

			string buffName = "buff_eff_" + buffer.animation;

			ResourcePoolManager.Instance.Spawn(string.Format(PathHelper.BUFF_EFFECT_PATH, buffName), 
				delegate(UnityEngine.Object inst)
				{
		            GameObject go = inst as GameObject;
					if (go == null)
		            {
		                return;
		            }

					GameObject effectGO = null;

					if (buffer.animationMount == ModelHelper.Mount_shadow)
					{
						effectGO = GameObjectExt.AddPoolChild(_monsterController.GetBattleGroundMount().gameObject, go);
						effectGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
						BattleShadow shadow = GameObjectExt.GetMissingComponent<BattleShadow>(effectGO);
						shadow.Init(_monsterController.GetBattleGroundMount());
					}
					else
					{
						//Utility.ResetPetMountRotation(_monsterController.gameObject, mountTransform);
						
						effectGO = GameObjectExt.AddPoolChild(mountTransform.gameObject, go);
						
						NoRotation noRotation = GameObjectExt.GetMissingComponent<NoRotation>(effectGO);
						noRotation.fixYToZero = false;
						
//						ParticleScaler scaler = GameObjectExt.GetMissingComponent<ParticleScaler>(effectGO);
//						
//						float scaleValue = 1;
//						
//						scaleValue /= (_monsterController.monsterData.hero.scale / 10000f);
//						
//						scaler.SetscaleMultiple( scaleValue );
					}
				
		            effectMaps[buffer.id] = effectGO;
				}, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
        }
    }
	
	void Update(){
//        foreach (GameObject effect in effectMaps.Values)
//        {
//			effect.transform.parent.transform.eulerAngles = Vector3.zero;
//        }		
	}
	
	private void RemoveBuffEffect(BattleBuff buffer)
    {
        if (effectMaps.ContainsKey(buffer.id))
        {
            GameObject effect = effectMaps[buffer.id];
			ResourcePoolManager.Instance.Despawn(effect, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
            effectMaps.Remove(buffer.id);
        }
    }

    public void RemoveAllBuff()
    {
        foreach (GameObject effect in effectMaps.Values)
        {
			ResourcePoolManager.Instance.Despawn(effect, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
        }
		if (effectMaps != null)
		{
			effectMaps.Clear();
		}
		if (buffList != null)
		{
			buffList.Clear();
		}
    }

	void OnDestroy ()
	{
		buffList = null;
	}
}

