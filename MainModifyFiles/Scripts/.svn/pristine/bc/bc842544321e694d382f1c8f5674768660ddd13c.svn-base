// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BattleMonsterName.cs
// Author   : SK
// Created  : 2014/5/29
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.data;

public class BattleMonsterName : MonoBehaviourBase
{
	public UILabel label;

	static public BattleMonsterName CreateNew(MonsterController mc)
	{
		Transform tf = mc.GetMountShadow();

		if ( tf == null )
		{
			tf = mc.gameObject.transform;
		}

		GameObject monsterNamePrefab = Resources.Load("Prefabs/Module/Battle/BattleUI/MonsterName") as GameObject;
		GameObject monsterNameGO = NGUITools.AddChild(LayerManager.Instance.NameUIAnchor, monsterNamePrefab);
		UIFollowTarget follower = monsterNameGO.AddComponent<UIFollowTarget>();
		
		follower.gameCamera = LayerManager.Instance.GetBattleFollowCamera();
		follower.uiCamera = LayerManager.Instance.UICamera;

		
		tf.localRotation = Quaternion.identity;
		tf.localScale = Vector3.one;
		
		tf.position = new Vector3(tf.position.x, 0f, tf.position.z);	
		
		follower.target = tf;
		follower.offset = new Vector3(0,-0.4f,0);
		BattleMonsterName monsterName = monsterNameGO.GetComponent<BattleMonsterName>();
		monsterName.Setup(mc);
		return monsterName;
	}

	private MonsterController _mc;
	public void Setup(MonsterController mc)
	{
		_mc = mc;
		string showStr = mc.videoSoldier.name;
		switch(mc.videoSoldier.monsterType)
		{
		case Monster.MonsterType_Boss:
			showStr = showStr + "头领";
			break;
		case Monster.MonsterType_Baobao:
			showStr = showStr + "宝宝";
			break;
		case Monster.MonsterType_Mutate:
			showStr = "变异" + showStr;
			break;
		}

		label.fontSize = 16;
		label.text = showStr;
		if (mc.IsPet() || mc.IsMainCharactor())
		{
			label.color = ColorConstant.Color_Battle_Player_Name;
		}
		else
		{
			label.color = ColorConstant.Color_Battle_Enemy_Name;
		}
	}

//	void Update()
//	{
//		string showStr = _mc.videoSoldier.name + _mc.currentHP + "/" + _mc.GetMaxHP ();
//		label.text = showStr;
//	}

	public void Destroy()
	{
		GameObject.Destroy(this.gameObject);
	}
}

