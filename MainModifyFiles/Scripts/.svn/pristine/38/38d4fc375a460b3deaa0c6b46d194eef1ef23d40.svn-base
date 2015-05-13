// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaseNpcUnit.cs
// Author   : willson
// Created  : 2014/12/23 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.scene.data;

public class BaseNpcUnit : INpcUnit
{
	protected Npc _npc;
	
	protected bool showNpc;
	
	protected GameObject _unitGameObject;
	protected GameObject _uintNpcGO;
	private Animator _anim;
	
	private BoxCollider _boxCollider;
	
	public void Show(Npc npc)
	{
		_npc = npc;
		LoadUnit();
	}

	virtual protected int GetNpcModel(){
		return _npc.modelId;
	}	
	
	//获取怪物的宠物id,如果没有的话则返回0
	virtual protected int GetMonsterPetId(){
		return 0;
	}
	
	public void RemoveNpcGO(){
		ShowChecker showChecker = _unitGameObject.GetComponent<ShowChecker>();
		if (showChecker != null){
			showChecker.RemoveChecker();
		}else{
			OnDeleteCallback(null);
		}
	}
	
	public void LoadNpcGO()
	{
		_unitGameObject.SetActive(true);
		
		if (_uintNpcGO != null){
			return;
		}

		OnNewCallback(null);
	}	
	
	private void OnNewCallback(GameObject go)
	{
		GameDebuger.Log("BaseNpcUnit OnNewCallback");
		if (_uintNpcGO != null){
			return;
		}
		
		string npcStylePath = ModelHelper.GetCharacterPrefabPath(GetNpcModel());
		ResourcePoolManager.Instance.Spawn(npcStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
	}
	
	private void OnDeleteCallback(GameObject go)
	{
		GameDebuger.Log("BaseNpcUnit OnDeleteCallback");
		
		if (_uintNpcGO != null){
			DestroyMissionMark();
			ResourcePoolManager.Instance.Despawn(_uintNpcGO, ResourcePoolManager.PoolType.DONT_DESTROY);
			_uintNpcGO = null;
			_boxCollider = null;
		}
	}
	
	private void DestroyMissionMark()
	{
//		NpcNameAndMissionMark mark = _unitGameObject.GetComponent<NpcNameAndMissionMark>();
//		if(mark == null)
//			return;
//		mark.DestroyNpcNameAndMark();
	}
	
	private void OnLoadFinish( UnityEngine.Object inst ){
		GameObject go = inst as GameObject;
		
		if (go == null)
		{
			string npcStylePath = ModelHelper.GetCharacterPrefabPath(ModelHelper.DefaultModelId);
			ResourcePoolManager.Instance.Spawn(npcStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
			return;
		}
		
		_uintNpcGO = GameObjectExt.AddPoolChild(_unitGameObject, go);
		
		UpdateNpcPosition();
		
		if (NeedTrigger())
		{
			if (this is DoubleTeleportUnit)
			{
				_boxCollider = _uintNpcGO.GetMissingComponent<BoxCollider>();
				_boxCollider.size = new Vector3(2, 1, 2);
				//_uintNpcGO.transform.localScale = new Vector3(1f, 1f, 1f);
				_uintNpcGO.tag = GameTag.Tag_Teleport;
				_boxCollider.gameObject.tag = GameTag.Tag_Teleport;
			}
			else
			{
				SkinnedMeshRenderer meshRenderer = _uintNpcGO.GetComponentInChildren< SkinnedMeshRenderer >();
				if (meshRenderer == null){
					_boxCollider = _uintNpcGO.GetMissingComponent<BoxCollider>();
					_boxCollider.center = new Vector3(0f, 1f, 0f);	
					_boxCollider.size = new Vector3(1f, 2f, 1f);						
				}else{
					Transform modelRoot = GameObjectExt.GetChildTransform(_uintNpcGO.transform, meshRenderer.rootBone.name );
					
					_boxCollider = modelRoot.gameObject.GetMissingComponent<BoxCollider>();
					
					_boxCollider.center = meshRenderer.localBounds.center;
					_boxCollider.extents = new Vector3(0.5f, 0.5f, 0.5f);						
				}

				//建筑类型的NPC
//				if (_uintNpcGO.name.IndexOf("common_100") == 0){
//					_boxCollider = _uintNpcGO.GetMissingComponent<BoxCollider>();
//					_boxCollider.center = new Vector3(0f, 1f, 0f);	
//					_boxCollider.size = new Vector3(3f, 2f, 3f);
//				}else{
//					_boxCollider = _unitGameObject.GetMissingComponent<BoxCollider>();
//					_boxCollider.center = new Vector3(0f, 1f, 0f);	
//					_boxCollider.size = new Vector3(1f, 2f, 1f);
//					_boxCollider.isTrigger = true;
//				}
				
				_anim = _uintNpcGO.GetComponent<Animator>();
				if (_anim != null)
				{
//					_anim.wrapMode = WrapMode.Loop;
					
//					if (_uintNpcGO.GetComponent<FS_ShadowSimple>() == null)
//					{
//						ModelHelper.SetPetShader(_uintNpcGO, false);
						ModelHelper.SetPetShadow(_uintNpcGO);

//						int monsterPetId = GetMonsterPetId();
//						if (monsterPetId != 0){
//							ModelHelper.SetPetLook(_uintNpcGO, monsterPetId, _npc.mutateTexture, _npc.colorParams, false, true, false);
//						}else{
//							ModelHelper.SetNpcLook(_uintNpcGO, GetNpcStyle());
//						}
						ModelHelper.SetPetLook(_uintNpcGO, _npc.modelId, _npc.mutateTexture, _npc.mutateColor);
//						ModelHelper.SetUnitAnimationOptimizer(_uintNpcGO);
//					}
				}
//				else
//				{
//					ModelHelper.SetPetShader(_uintNpcGO, false);
//				}
				
//				if (GetMonsterPetId() == 0){
//					//_uintNpcGO.transform.localScale = new Vector3(_npcConfig.scale, _npcConfig.scale, _npcConfig.scale);
//				}else{
//					_uintNpcGO.transform.localScale = new Vector3();
//				}
//				
//				float scale = GetNpcScale();
//				_uintNpcGO.transform.localScale = new Vector3(scale, scale, scale);
				
				//_unitGameObject.transform.localEulerAngles = new Vector3(0, _npcConfig.rotateY, 0);					
				
				_uintNpcGO.tag = GameTag.Tag_Npc;
				_boxCollider.gameObject.tag = GameTag.Tag_Npc;
			}
			
			_boxCollider.isTrigger = true;
		}
		
//		GamePauseController.SetLayer(_unitGameObject, LayerMask.NameToLayer("UnitLayer"));		
		
		AfterInit();
	}
	
	virtual protected float GetNpcScale()
	{
		return 1f;
	}
	
	private void LoadUnit(){
		_unitGameObject = NGUITools.AddChild(LayerManager.Instance.WorldActors);
		_unitGameObject.name = GetNpcModel()+"_"+GetNpc().id;
		
		//GamePauseController.SetLayer(_unitGameObject, LayerMask.NameToLayer("Terrain"));
		//_unitGameObject.tag = "Npc";
		
		UpdateNpcPosition();
	}
	
	public void DoAction(string action){
		if (action == ModelHelper.Anim_idle){
			ModelHelper.PlayAnimation(_anim, action, true, null);
		}else{
			ModelHelper.PlayAnimation(_anim, action, true, animClipCallBack);
		}
		if (_anim != null)
		{
//			_anim.Sample();
		}
	}
	
	private void animClipCallBack(string action , float length)
	{
		if (length == 0){
			DoAction(ModelHelper.Anim_idle);
		}
	}
	
	protected bool IsPlayAction(string actin)
	{
		if(_anim == null)
			return false;
		if(_anim.GetCurrentAnimatorStateInfo(ModelHelper.Animator_Layer_BaseLayer).IsName(actin))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public Npc GetNpc(){
		return _npc;
	}
	
	public void UpdateNpc(Npc npc){
		_npc.x = npc.x;
		_npc.y = npc.y;
		_npc.z = npc.z;
		UpdateNpcPosition();
	}
	
	private void UpdateNpcPosition()
	{
		if (GameObject.Find("SceneLayer") != null){
			Vector3 position = SceneHelper.GetSceneStandPosition(new Vector3(_npc.x,0f, _npc.z), Vector3.zero);

			if (this is DoubleTeleportUnit){
				//position.y += 0.04f;
			}
			
			_unitGameObject.transform.position = position;	
			_unitGameObject.transform.localEulerAngles = new Vector3(0,_npc.rotateY,0);
		}
	}
	
	virtual protected bool NeedTrigger(){
		return false;
	}
	
	virtual protected void AfterInit(){
		
	}
	
	public GameObject GetUnitGO(){
		return _unitGameObject;
	}
	
	public GameObject GetNpcUnitGO()
	{
		return _uintNpcGO;
	}
	
	public GameObject GetNpcColliderGO(){
		if (_boxCollider == null){
			return null;
		}else{
			return _boxCollider.gameObject;
		}
	}
	
	public BoxCollider GetNpcCollider()
	{
		return _boxCollider;
	}
	
	virtual public void SetActiveUnit(bool showNpc)
	{
		//		if(_unitGameObject != null)
		//		{
		//			_unitGameObject.SetActive(showNpc);
		//			
		//			if (showNpc && _uintNpcGO == null){
		//				WorldManager.Instance.GetNpcView().AddToLoadPool(this);
		//			}
		//		}
	}
	
	virtual public bool isActiveUnit()
	{
		if(_unitGameObject != null)
		{
			return _unitGameObject.activeSelf;
		}
		else
		{
			return false;
		}
	}
	
	virtual public void Trigger(){
	}
	
	
	
	virtual public void Destroy(){
		if ((this is DoubleTeleportUnit) == false)
		{
			BoxCollider boxCollider = _unitGameObject.GetComponent<BoxCollider>();
			if (boxCollider != null)
			{
				GameObject.Destroy(boxCollider);
			}
		}
		
		ResourcePoolManager.Instance.Despawn(_uintNpcGO, ResourcePoolManager.PoolType.DONT_DESTROY);
		_uintNpcGO = null;
		_boxCollider = null;
		
		DestroyMissionMark();
		
		GameObject.Destroy(_unitGameObject);
		_unitGameObject = null;
	}
}
