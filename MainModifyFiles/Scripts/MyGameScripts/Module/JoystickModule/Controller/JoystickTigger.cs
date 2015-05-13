// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  JoystickTigger.cs
// Author   : willson
// Created  : 2014/12/3 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;

public class JoystickTigger : MonoBehaviour
{
	private bool _startTrigger;

	private GameObject NPCObj;
	private Transform cameraV;
	private GameObject npcBubble = null;//npc头顶对话气泡//
	private Npc _npc = null;

	void Start ()
	{
		_startTrigger = false;
		
		Invoke("DelayTrigger", 1f);
	}
	
	private void DelayTrigger()
	{
		_startTrigger = true;
	}
	
	void Update()
	{

	}

	void OnTriggerEnter(Collider collider) 
	{
		if(JoystickModule.DisableMove) return;
		if (_startTrigger)
		{
			if(collider.tag == "Npc" || collider.tag == "Teleport")
			{
				NPCObj  = collider.gameObject; //得到NPC的游戏对象//
				INpcUnit npcUnit = WorldManager.Instance.GetNpcViewManager().GetNpcUnit(NPCObj);
				
				Npc npc = null;
				
				if (npcUnit != null){
					npc = npcUnit.GetNpc();
				}
				
				if(npc == null)
					return;
				
				if(collider.tag == "Teleport")
				{					
					bool mustCheck = false;
					//如果是这种类型的id，必须做碰撞
					if (npcUnit.GetNpc().id == 62331001 || npcUnit.GetNpc().id == 62208003){
						mustCheck = true;
					}
					

					StartCoroutine ( DelayClickNpc() ); 
					
				}
				else
				{

				}
			}
		}
	}
	
	private IEnumerator DelayClickNpc()
	{
		yield return null;
		WorldManager.Instance.GetNpcViewManager().Click(NPCObj);
		_startTrigger = false;
	}
	
	void OnTriggerExit (Collider collider)
	{
		if(JoystickModule.DisableMove) return;
		if(collider.tag == "Npc")
		{
			if (NPCObj != null)
			{
				//SetFindPathEnabled(true);
				NPCObj = null;
			}
		}
		else if (collider.tag == "Teleport"){
			_startTrigger = true;
			
			INpcUnit npcUnit = WorldManager.Instance.GetNpcViewManager().GetNpcUnit(collider.gameObject);
			DoubleTeleportUnit teleportUnit = npcUnit as DoubleTeleportUnit;
			if (teleportUnit != null){
				teleportUnit.StopTrigger();
			}
		}
	}
}