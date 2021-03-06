﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.player.dto;

public class ModelDisplayController : MonoBehaviour {

	private const string MODEL_UICOM = "Prefabs/Module/ModelDisplayModule/ModelDisplayUIComponent";
	private const string MODEL_RENDERER = "Prefabs/Module/ModelDisplayModule/ModelDisplayRenderer";
	public static List<ModelDisplayController> comList = new List<ModelDisplayController>();
	public static List<string> _animationClipList = new List<string>{ModelHelper.Anim_attack1,ModelHelper.Anim_attack2,ModelHelper.Anim_attack3,ModelHelper.Anim_skill};

	public static ModelDisplayController GenerateUICom(Transform anchor){
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(MODEL_UICOM) as GameObject;
		GameObject uiComGo = NGUITools.AddChild(anchor.gameObject,prefab);
		int depth = UIPanel.Find(anchor).depth;
		NGUITools.AdjustDepth(uiComGo,depth+1);
		return uiComGo.GetMissingComponent<ModelDisplayController>();
    }
	
	private UITexture _mUITexture;
	
	//RenderTexture Relative Member
	private GameObject _modelRenderer;
	private Camera _mCam;
	private Transform _mModelTrans;
	private Animator _mAnimator;
	private bool _isPlaying;
	private float _yOffset;
	private float _scaleFactor;
	private PetLookInfo _petLookInfo;
	private int _weaponModel = 0;
//	#region Limit Mode
//	public static float TargetFPS = 30f;
//	private static bool isLimit=false;
//	public static void ChangeMode(){
//		isLimit = !isLimit;
//		foreach(var com in comList){
//			com.SetCameraState(isLimit);
//		}
//
//	}
//	#endregion
	
//	private float timer = 0f;
//	void Update(){
//		if(isLimit){
//			timer += Time.deltaTime;
//			if(timer > (1.0f / TargetFPS))
//			{
//				timer = 0f;
//				RenderModel();
//			}
//		}
//	}

	void OnEnable(){
		if(_modelRenderer != null)
			_modelRenderer.SetActive(true);
	}

	void OnDisable(){
		if(_modelRenderer != null)
			_modelRenderer.SetActive(false);
	}
	
	void OnDestroy(){
		if(_modelRenderer != null)
		{
			_mUITexture.enabled = false;
//			_mCam.targetTexture.DiscardContents();
//			_mCam.targetTexture.Release();
            Destroy(_mCam.targetTexture);
			_mCam.targetTexture = null;
			Destroy(_modelRenderer);
			_modelRenderer =null;
			comList.Remove(this);
		}
	}
	
	public void Init(int width,int height)
	{
		_mUITexture = this.transform.Find("ModelUITexture").GetComponent<UITexture>();
		_mUITexture.SetDimensions(width,height);
		UIEventTrigger eventTrigger = _mUITexture.GetComponent<UIEventTrigger>();
		EventDelegate.Set(eventTrigger.onClick,OnClickModel);
		EventDelegate.Set(eventTrigger.onDrag,OnDragModel);

		GameObject modelDSComPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(MODEL_RENDERER) as GameObject;
		_modelRenderer = GameObject.Instantiate(modelDSComPrefab) as GameObject;
		int index = comList.Count;
		_modelRenderer.name = "ModelRender_"+index;
		_modelRenderer.transform.position = new Vector3(-100f*index-1000f,0,0);
		
		_mCam = _modelRenderer.transform.Find("ModelCamera").GetComponent<Camera>();
		_mCam.targetTexture = new RenderTexture(Mathf.RoundToInt(width*1.5f),Mathf.RoundToInt(height*1.5f),16);
		_mCam.targetTexture.name = _modelRenderer.name;
		_mCam.Render();
//		_mCam.enabled = !isLimit;
		_mCam.enabled = false;

		_mUITexture.mainTexture = _mCam.targetTexture;
		
		comList.Add(this);
	}

	public void SetupModel(GeneralCharactor charactor, int weaponModelId){
		_weaponModel = weaponModelId;
		SetupModel(charactor);
	}

	public void SetupModel(GeneralCharactor charactor){
		_scaleFactor = charactor.uiScale;
		_yOffset = charactor.uiPos;
		_petLookInfo = PetLookInfo.ToInfo (charactor);
		_petLookInfo.mutateTexture = 0;

		UpdateModel(_petLookInfo.model);
	}

	public void SetupModel(PetPropertyInfo petInfo){
		_scaleFactor = petInfo.pet.uiScale;
		_yOffset = petInfo.pet.uiPos;
		_petLookInfo = PetLookInfo.ToInfo(petInfo.pet);
		if(!petInfo.petDto.ifMutate)
			_petLookInfo.mutateTexture = 0;

		UpdateModel(_petLookInfo.model);
	}

	public void SetupModel(PlayerDto playerDto)
	{
		_weaponModel = BackpackModel.Instance.GetCurrentWeaponModel ();
		_scaleFactor = playerDto.charactor.uiScale;
		_yOffset = playerDto.charactor.uiPos;
		_petLookInfo = PetLookInfo.ToInfo (playerDto.charactor);
		_petLookInfo.mutateTexture = 0;
		_petLookInfo.mutateColor = PlayerModel.Instance.GetDyeColorParams(playerDto.hairDyeId,playerDto.dressDyeId,playerDto.accoutermentDyeId);
		
		UpdateModel(_petLookInfo.model);
		
		BackpackModel.Instance.OnWeaponModelChange += OnWeaponModelChange;
	}

	public void ChangeModelColorParams(string colorParams){
		_petLookInfo.mutateColor = colorParams;
		if(_mModelTrans != null)
			ModelHelper.SetPetLook (_mModelTrans.gameObject, _petLookInfo.texture, _petLookInfo.mutateTexture, _petLookInfo.mutateColor);
	}

	private void UpdateModel(int modelId){
		CleanUpModel();
		string petStylePath = ModelHelper.GetCharacterPrefabPath(modelId);
		ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadModelFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
	}

	public void CleanUpModel(){
		if(_mModelTrans != null)
		{
			ResourcePoolManager.Instance.Despawn(_mModelTrans.gameObject,ResourcePoolManager.PoolType.DONT_DESTROY);
			_mModelTrans = null;
			_mAnimator = null;
		}

		BackpackModel.Instance.OnWeaponModelChange -= OnWeaponModelChange;
	}

	private void OnWeaponModelChange(int model)
	{
		_weaponModel = model;

		if (_mModelTrans != null)
		{
			ModelHelper.UpdateModelWeapon (_mModelTrans.gameObject, _petLookInfo.model, _weaponModel);
		}
	}

	private void OnLoadModelFinish(UnityEngine.Object obj){

		GameObject model = obj as GameObject;
		if (model == null)
		{
			GameDebuger.Log("ModelDisplayController OnLoadFinish null and Use Default 2020");
			string petStylePath = ModelHelper.GetCharacterPrefabPath(ModelHelper.DefaultModelId);
			_yOffset = 0f;
			_scaleFactor = 0f;
			ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadModelFinish, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
			return;
		}

		model = GameObjectExt.AddPoolChild(_modelRenderer, model);

		ModelHelper.SetPetLook (model, _petLookInfo.texture, _petLookInfo.mutateTexture, _petLookInfo.mutateColor);

		ModelHelper.UpdateModelWeapon (model, _petLookInfo.model, _weaponModel);

		_mModelTrans = model.transform;
		_mAnimator = model.GetComponent<Animator>();
		_mModelTrans.localPosition += new Vector3(0f,_yOffset,0f);

		if(_scaleFactor > 0f){
			_mModelTrans.localScale = new Vector3(_scaleFactor,_scaleFactor,_scaleFactor);
		}

		//模型加载完毕，开启摄像机
		_mCam.enabled = true;
	}
	
//	private void RenderModel(){
//		if(_mCam != null)
//		{
//			//            RenderTexture currentRT = RenderTexture.active;
//			//            RenderTexture.active = _mCam.targetTexture;
//			_mCam.Render();
//			//            RenderTexture.active.DiscardContents();
//			//            RenderTexture.active = currentRT;
//		}
//	}
	
	private void OnDragModel(){ 
		if(_mModelTrans == null) return;
		Vector2 delta = UICamera.currentTouch.delta;
		_mModelTrans.Rotate(_mModelTrans.up,-Time.deltaTime * delta.x*30f);
	}
	
	private void OnClickModel(){
		if(_mAnimator == null || _isPlaying)  return;

		_isPlaying = true;
		ModelHelper.PlayAnimation(_mAnimator, _animationClipList.Random(), false);
		Invoke("PlayerIdleAnimation",0.5f);
	}

	private void PlayerIdleAnimation(){
		_isPlaying = false;
	}

	public void SetBoxColliderEnabled(bool b)
	{
		if(_mUITexture != null)
		{
			BoxCollider box = _mUITexture.GetComponent<BoxCollider>();
			if(box != null)
				box.enabled = b;
		}
	}

	public void SetBoxCollider(float width,float height){
		if(_mUITexture != null){
			BoxCollider box = _mUITexture.GetComponent<BoxCollider>();
			if(box != null){
				box.size = new Vector3(width,height,0f);
			}
		}
	}
}
	