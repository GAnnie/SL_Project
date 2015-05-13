// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  PlayerUpgradeEffect.cs
// Author   : senkay
// Created  : 5/6/2013 2:19:17 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeEffect : MonoBehaviour
{
	public static bool NeedEffect = false;
	
	private GameObject effectGO;

	void Start()
	{
		PlayerModel.Instance.OnPlayerGradeUpdate += OnPlayerGradeUpdate;
	}

	private void OnPlayerGradeUpdate()
	{
		NeedEffect = true;
	}

	void Destroy()
	{
		PlayerModel.Instance.OnPlayerGradeUpdate -= OnPlayerGradeUpdate;
	}

	void Update()
	{
		if (NeedEffect)
		{
			NeedEffect = false;
			Play();
		}
		
		//if (effectGO != null && effectGO.activeSelf == false)
		//{
		//    effectGO.SetActive(true);
		//}
	}
	
	public void Play()
	{
		string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_PlayerUpgrade);
		OneShotSceneEffect.BeginFollowEffect (effpath, this.transform, 3f, 1f);

//		if (effectGO == null)
//		{
//			ResourceLoader.LoadAsync(EffectPrefabPath, delegate(AssetBase obj) {
//				if (obj == null){
//					return;
//				}
//				
//				effectGO = Utility.AddChild(this.gameObject, (GameObject)obj.asset);
//				
//				//特效时间
//				EffectTime effectTime = effectGO.GetComponent<EffectTime>();
//				if (effectTime == null)
//				{
//					effectTime = effectGO.AddComponent<EffectTime>();
//					effectTime.time = 5;
//				}				
//			});
//		}
//		else
//		{
//			effectGO.SetActive(false);
//			effectGO.SetActive(true);
//		}
		
		//AudioManager.Instance.PlaySound("sound_character/sound_levelup_01");
	}
}
