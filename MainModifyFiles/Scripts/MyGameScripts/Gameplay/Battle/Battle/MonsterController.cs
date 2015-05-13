﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.player.data;
using System;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.battle.dto;
using com.nucleus.h1.logic.core.modules.charactor.data;
using com.nucleus.h1.logic.core.modules.battle.data;
using com.nucleus.h1.logic.core.modules.faction.data;
using com.nucleus.h1.logic.core.modules.battlebuff.data;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class MonsterController : MonoBehaviour
{
    public enum MonsterSide
    {
        Player,
        Enemy,
        None,
    }

    public bool isStriker;

    public MonsterSide side;

	public VideoSoldier videoSoldier;

    private Dictionary<int, VideoBuffAddTargetState> buffStateMaps;
    private BattleMonsterBuff battleMonsterBuff;

    private BattleHUDText battleHUDText;

    private BattleController battleController = null;

	private BattleMonsterHPSlider hpSlider = null;
	private BattleMonsterName monsterName = null;

	public BattleTargetSelector battleTargetSelector = null;

	private CharactorProperyController _properyController = null;

    // Temporary stat
    public enum ShowMessageEffect
    {
        DODGE = 1 << 0,
        CRITICAL = 1 << 1,
    }

    public ShowMessageEffect showMessageEffect;

    public int modifyHP;
	public int modifyMP;
	public int modifySP;

	private int _currentHP; //当前HP
	public int maxHP; //最大HP

	private int _currentMP; //当前MP
	public int maxMP; //最大HP

	private int _currentSP; //当前SP
	public int maxSP; //最大SP

	public bool dead = false;
	public bool leave = false;
    //--------------------------------------------------------------

	private static string TargetSelectEffectPath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_TargetSelect);
	private static string TargetClickEffectPath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_CharactorClick);
	
	public Vector3 originPosition;
	public Vector3 originRotation;
	
	private Animator animator;
	
	Shader originalShader;
	Color originalModelColor;
	
	Material petMaterial;
	
	public BoxCollider petCollider;
	
	SkinnedMeshRenderer meshRenderer;
	
	public Transform modelRoot;
	
	public delegate void ActionEnd(string type);
	
	public ActionEnd actionEnd;
	
	private ShadowMotionGenerator shadowMotionGenerator;
	
	private GameObject petModelObject = null;
	
	private Vector3 origLocalScale;
	
	private Transform myTransform;
	
	public MonsterController target;
	public int targetReference = 0;
	
	private BattleBodyFadeInOut battleBodyFadeInOut = null;
	
	private PetLookInfo _petLookInfo = null;
	
	protected void Awake()
	{
		myTransform = transform;
	}

	public void ClearMessageEffect(bool cleanAll)
	{
		modifyHP = 0;
		modifyMP = 0;
		modifySP = 0;
		showMessageEffect = 0;
	}
	
	public void AddMessageEffect(ShowMessageEffect effect)
	{
		GameDebuger.Log(((ShowMessageEffect)(effect)).ToString());
		
		showMessageEffect = showMessageEffect | effect;
	}
	
	public bool existMessageEffect(ShowMessageEffect effect)
	{
		return ((int)(showMessageEffect & effect)) != 0;
	}

	private void OnLoadFinish(UnityEngine.Object inst)
	{
		GameObject go = inst as GameObject;
		
		petModelObject = GameObjectExt.AddPoolChild(gameObject, go);
		
		if (petModelObject == null){
			Debug.LogError("MonsterController Can't instantiate model");
			GameDebuger.Log("MonsterController Can't instantiate model");
		}
		
		ModelHelper.SetPetShadow (petModelObject);
//		ModelHelper.SetPetShader (petModelObject, false);

		ModelHelper.SetPetLook(petModelObject, _petLookInfo.texture, _petLookInfo.mutateTexture, _petLookInfo.mutateColor);
		ModelHelper.UpdateModelWeapon (petModelObject, _petLookInfo.model, _petLookInfo.wpModel);
		
		animator = petModelObject.GetComponent<Animator>();
		
		// ------------------- ------------ -----------------------
		SkinnedMeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		foreach (SkinnedMeshRenderer smr in meshRenderers)
		{
			// ------------------- Set Collider -----------------------
//			smr.material.shader = Shader.Find(smr.material.shader.name);
//			petMaterial = smr.material;
//			originalShader = smr.material.shader;
			
			if (smr.gameObject.name == "pet_"+_petLookInfo.model)
			{
				meshRenderer = smr;
			}
		}
		
		if (meshRenderer == null)
		{
			meshRenderer = meshRenderers[0];
		}
		
		Debug.Log ("meshRenderer.rootBone.name="+meshRenderer.rootBone.name);
		
		modelRoot = petModelObject.transform.GetChildTransform(meshRenderer.rootBone.name);
		if (modelRoot == null)
		{
			modelRoot = gameObject.transform.GetChild(0);
		}
		
		if (modelRoot != null)
		{
			petCollider = GameObjectExt.GetMissingComponent<BoxCollider>(modelRoot.gameObject);
			
			petCollider.center = meshRenderer.localBounds.center;
			petCollider.extents = new Vector3(0.5f, 0.5f, 0.5f);
			//petCollider.extents = meshRenderer.localBounds.extents;
			
			modelRoot.gameObject.layer = LayerMask.NameToLayer(GameTag.Tag_BattleActor);
			modelRoot.gameObject.tag = GameTag.Tag_BattleActor;
		}
		
		//-------------------- Set LayMask ------------------------
		gameObject.layer = LayerMask.NameToLayer(GameTag.Tag_BattleActor);
		gameObject.tag = GameTag.Tag_BattleActor;
		
		//-------------------------------------
		shadowMotionGenerator = gameObject.AddComponent<ShadowMotionGenerator>();
		
		if (shadowMotionGenerator == null)
			GameDebuger.Log("Can't Add Component ShadowMotionGenerator");
		else
			shadowMotionGenerator.petPrefab = petModelObject;
		
		//-------------------------------------	
		Initialize();
		
		//InitBodyFadeInOutEffect();
		
		Invoke("DelayInitBattleMount", 0.05f);
	}
	
	private void DelayInitBattleMount()
	{
		GetMountBattleEffect();
		GetMountDamageffect ();
		GetBattleGroundMount();
	}
	
	public void InitPetModel()
	{
		if (petModelObject != null)
		{
			if (videoSoldier.hp <= 0 )
			{
				InitDieState();
			}
			return;
		}
		
		if (petModelObject == null)
		{
			string petStylePath = ModelHelper.GetCharacterPrefabPath(_petLookInfo.model.ToString());
			if (ResourceLoader.IsAssetExist(petStylePath) == false)
			{
				_petLookInfo.model = ModelHelper.DefaultModelId;
				petStylePath = ModelHelper.GetCharacterPrefabPath(_petLookInfo.model.ToString());
			}

			ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
		}
	}
	
	protected void InitPet(PetLookInfo petLookInfo, bool isStriker, bool needJoinEffect)
	{
		_petLookInfo = petLookInfo;
		gameObject.tag = GameTag.Tag_BattleActor;
		
		originPosition = myTransform.position;
		originRotation = myTransform.rotation.eulerAngles;
		
		needJoinEffect = false;
		
		if (isStriker)
		{
			if (needJoinEffect)
			{
				Invoke("PlayJoinBattleEffect", 0.1f);
			}
			else
			{
				InitPetModel();
			}
		}
	}
	
	
	public void AddTrailEffect(Color color, float delayTime, float trailTime, string mountStart, string mountEnd,string matName)
	{
		InitTrail(color, delayTime, trailTime, mountStart, mountEnd,matName);
	}
	
	private void InitTrail(Color color, float delayTime, float trailTime, string mountStart, string mountEnd,string matName)
	{
		if (mountStart == "" || mountEnd == "")  //默认值启动所有锚点的刀光
		{
			for (int i = 1; i <= 3; i++)
			{
				Transform trailStart = GetMountingPoint("Mount_TrailStart_0" + i);
				Transform trailEnd = GetMountingPoint("Mount_TrailEnd_0" + i);
				PlayTrail(trailStart, trailEnd, color, delayTime, trailTime,matName);
			}
		}
		else
		{
			Transform trailStart = GetMountingPoint(mountStart);
			Transform trailEnd = GetMountingPoint(mountEnd);
			PlayTrail(trailStart, trailEnd, color, delayTime, trailTime,matName);
		}
	}
	
	private void PlayTrail(Transform trailStart, Transform trailEnd, Color color, float delayTime, float trailTime,string matName)
	{
		if (trailStart != null && trailEnd != null)
		{
			MeleeWeaponTrail mwt = GameObjectExt.GetMissingComponent<MeleeWeaponTrail>(trailStart.gameObject);
			TrailEffectTime time = GameObjectExt.GetMissingComponent<TrailEffectTime>(trailStart.gameObject);
			
			if (mwt.TrailMaterial == null || mwt.TrailMaterial.name != matName) //如果是空或不同材质，进行初始化操作
			{
				//mwt.DestroyTrail();
				mwt.TrailMaterial = (Material)ResourceLoader.Load("Materials/WeaponTrailMat/"+matName, "mat");
				mwt.Init(this.transform);
				time.Init(mwt);
			}
			
			mwt.UpdateInfo(trailStart, trailEnd, trailTime, color);
			
			//Debug.LogError("Ready to Play");
			mwt.EmitTrail();
			time.Play(delayTime);
		}
	}
	
	//	void RestoreModel()
	//	{
	//		transform.localScale = origLocalScale;
	//	}

	protected void UpdateAnimationDelaTime()
	{
		//		float t = Mathf.Clamp (Time.deltaTime * 1, 0, 0.066f);
		//		
		//		if ( animationController != null )
		//			animationController.SetDeltaTime (t);		
	}
	
	public void MoveTo(Transform target, float targetOffset, string animation, float time, Vector3 rotation)
	{
		Vector3 targetPosition = target.position + target.TransformDirection(Vector3.forward) * targetOffset;
		
		transform.LookAt(target);
		transform.eulerAngles = new Vector3(rotation.x, transform.eulerAngles.y, rotation.z);
		
		Goto(targetPosition, time, animation);
	}
	
	public void Goto(Vector3 position, float time, string animationName, float delay = 0, bool turn = false)
	{
		if (animationName != null)
		{
			PlayAnimation(animationName);
		}
		
		if (turn)
		{
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
		}
		
		iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "y", position.y, "z", position.z, "time", time, "delay", delay, "easetype", "linear", "oncomplete", "GotoComplete"));
	}
	
	void GotoComplete()
	{
		transform.eulerAngles = originRotation;
		PlayAnimation(ModelHelper.Anim_battle);
		if (actionEnd != null)
			actionEnd("move");
	}
	
	public void ReturnToOrigin()
	{
		if (transform.position != originPosition)
			GoBack(0.1f, Vector3.zero, null, false);
	}
	
	public void GoBack(float time, Vector3 rotation, string animationName, bool callbackWhenFinished = true, float delayTime = 0f)
	{
		//		transform.position = originPosition;
		//		transform.eulerAngles = originRotation;
		//
		//		GotoBackComplete(callbackWhenFinished);
		
		if (animationName != null)
		{
			PlayAnimation(animationName);
		}
		
		Vector3 position = originPosition;
		
		transform.eulerAngles = originRotation + rotation;
		
		iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "y", position.y, "z", position.z, "time", time, "delay", delayTime,
		                                      "easetype", "linear", "oncomplete", "GotoBackComplete",
		                                      "oncompleteparams", callbackWhenFinished));
	}
	
	void GotoBackComplete(bool callbackWhenFinished)
	{
		transform.eulerAngles = originRotation;
		//RotateToOriginDirection( 1.0f );
		
		//PlayAnimation( ModelHelper.Anim_idle );
		//petAnimation.wrapMode = WrapMode.Loop;
		
		if (!callbackWhenFinished)
			return;
		
		if (actionEnd != null)
			actionEnd("moveback");
	}
	
	//if hp is 20% left then play pant, if not play idle
	public void PlayStateAnimation()
	{
		CheckStateAnimation();
		
		if (!IsDead())
		{
			PlayIdleAnimation();
		}
	}
	
	//检查是否需要复活
	protected void CheckStateAnimation()
	{
		if (IsDead() == false)
		{
			if (petModelObject != null)
			{
				petModelObject.SetActive(true);
				FadeIn();
				HideShadow();
			}
		}
	}
	
	public void PlayIdleAnimation()
	{
		PlayAnimation(ModelHelper.Anim_battle);
	}
	
	public void FadeIn()
	{
		if (battleBodyFadeInOut != null)
		{
			battleBodyFadeInOut.FadeIn();
			battleBodyFadeInOut.Run();
		}
	}
	
	public void FadeOut()
	{
		if (battleBodyFadeInOut != null)
		{
			battleBodyFadeInOut.FadeOut();
			battleBodyFadeInOut.Run();
		}
	}
	
	public void PlayDieAnimation()
	{
		BattleController battleController = BattleController.Instance;
		if (battleController != null)
		{
			battleController.AddDieMonster(GetId());
		}
		
		AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(ModelHelper.Animator_Layer_BattleLayer);
		if (animatorState.IsName(ModelHelper.Anim_death) == false)
		{
			PlayAnimation(ModelHelper.Anim_death, OnDieFinishCallback);
		}
		
		//RemoveAllBuff();
		
		HideShadow();
	}
	
	private void OnDieFinishCallback( string anim, float length)
	{
		Invoke("OnFadeEffectFinish",length);
	}
	
	private void OnFadeEffectFinish()
	{
		AnimatinCallback();
	}
	
	public void InitDieState()
	{
		HideShadow();
		petModelObject.SetActive(false);
	}
	
	private void HideShadow()
	{
		//		if (petModelObject != null)
		//		{
		//			FS_ShadowSimple shadow = petModelObject.GetComponentInChildren<FS_ShadowSimple>();
		//			if (shadow != null)
		//			{
		//				shadow.enabled = false;
		//			}
		//		}
	}
	
	public void InitBodyFadeInOutEffect()
	{
		battleBodyFadeInOut = GameObjectExt.GetMissingComponent<BattleBodyFadeInOut>(gameObject);
		battleBodyFadeInOut.Init(petMaterial, meshRenderer);
	}
	
	//--------------------------------------------------------------------------
	private float _lastBodyShiftDistance = 0f;
	private bool _backToOriginPosition = false;
	public void BodyShift(float distance, float time, int direction, float shiftBackDelay = 0, bool backToOriginPosition = false)
	{
		CancelInvoke ("BodShiftComplete");
		
		_lastBodyShiftDistance = distance;
		_backToOriginPosition = backToOriginPosition;
		
		Vector3 position = transform.position;
		
		//位移方向(0/1/2/3 上/下/前/后)
		Vector3 moveDir = new Vector3();
		if (direction == 0)
			moveDir = Vector3.up;
		else if (direction == 1)
			moveDir = -Vector3.up;
		else if (direction == 2)
			moveDir = Vector3.forward;
		else if (direction == 3)
			moveDir = -Vector3.forward;
		
		position += transform.TransformDirection(moveDir) * distance;
		
		if (time == 0)
		{
			gameObject.transform.position = position;
		}
		else
		{
			iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "y", position.y, "z", position.z, "time", time,
			                                      "easetype", "linear", "oncomplete", "BodShiftCompleteDelay", "oncompleteparams", shiftBackDelay));
		}
	}
	
	void BodShiftComplete()
	{
		GoBackFromShit(0.1f);
	}
	
	void BodShiftCompleteDelay(float delay)
	{
		if (delay == 0)
		{
			BodShiftComplete();
		}
		else
		{
			Invoke ("BodShiftComplete", delay);
		}
	}
	
	public void GoBackFromShit(float time)
	{
		//PlayNormalActionInfo("hit2", 0);

		Vector3 position = originPosition;
		if (_backToOriginPosition == false) 
		{
			//位移方向(0/1/2/3 上/下/前/后)
			Vector3 moveDir =  Vector3.forward;
			position = transform.position;
			
			position += transform.TransformDirection(moveDir) * _lastBodyShiftDistance;
		}
		
		iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "y", position.y, "z", position.z, "time", time,
		                                      "easetype", "linear", "oncomplete", "BodShiftBackComplete"));
	}
	
	private void BodShiftBackComplete()
	{
		AnimatinCallback ();
	}
	
	//------------------------------------------------------------------------------
	
	void RotateToOriginDirection(float time)
	{
		iTween.RotateTo(gameObject, iTween.Hash("x", 0, "y", originRotation.y, "z", 0, "easetype", "linear", "time", time));
	}
	
	public void PlayNormalActionInfo(string name, float duration)
	{
		CancelInvoke("AnimatinCallback");
		CancelInvoke("PlayIdleAnimation");
		CancelInvoke("CheckAnimationCallback");
		CancelInvoke("CheckAnimationIdleCallback");
		
		if (duration == 0)
		{
			PlayAnimation(name, delegate(string arg1, float arg2)
			              {
				if (arg2 == 0)
				{
					Invoke("CheckAnimationCallback", 0.05f);
				}
				else
				{
					Invoke("AnimatinCallback", arg2);
				}
			});
		}
		else
		{
			PlayAnimation(name, delegate(string arg1, float arg2)
			              {
				//				if (duration > arg2 && name != ModelHelper.Anim_def)
				//				{
				//					if (arg2 == 0)
				//					{
				//						Invoke("CheckAnimationIdleCallback", 0.05f);
				//					}
				//					else
				//					{
				//						Invoke("PlayIdleAnimation", arg2);
				//					}
				//                }
			});
			Invoke("AnimatinCallback", duration);
		}
	}
	
	private void CheckAnimationCallback()
	{
		if (animator == null)
		{
			Invoke("AnimatinCallback", 0.5f);
		}
		else
		{
			AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(ModelHelper.Animator_Layer_BattleLayer);
			Invoke("AnimatinCallback", animatorState.length-0.05f);
		}
	}
	
	private void CheckAnimationIdleCallback()
	{
		if (animator == null)
		{
			Invoke("PlayIdleAnimation", 0.5f);
		}
		else
		{
			AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(ModelHelper.Animator_Layer_BattleLayer);
			Invoke("PlayIdleAnimation", animatorState.length-0.05f);
		}
	}
	
	void AnimatinCallback()
	{
		if (actionEnd != null)
		{
			actionEnd("normal");
		}
		else
		{
			PlayStateAnimation();
		}
	}
	
	public void PlayAnimation(string name, Action<string, float> animClipCallBack = null)
	{
		bool crossFade = false;
		
		bool checkSameAnim = false;
		if (name == ModelHelper.Anim_death)
		{
			//			mc.DestroyHPSlider();
			//			mc.DestroyMonsterName();
			RemoveAllBuff();
			
			checkSameAnim = true;
		}
		
		if (name == ModelHelper.Anim_battle)
		{
			//checkSameAnim = true;
		}

		ModelHelper.PlayAnimation(animator, name, crossFade, animClipCallBack, checkSameAnim, ModelHelper.Animator_Layer_BattleLayer);
		//AudioManager.Instance.PlayAnimationSound(_petStyle, name);
	}
	
	protected void Initialize()
	{
		InitHudText();

		if (currentHP <= 0)
		{
			dead = true;
			PlayDieAnimation();
		}

		foreach (VideoBuffAddTargetState state in videoSoldier.buffs)
        {
            AddBuffState(state, false);
        }
		
		PlayStateAnimation();
		Invoke ("ShowPetShoutContent", 0.5f);
	}
	
	public GameObject GetPetModelObject()
	{
		return petModelObject;
	}
	//---------------------------------------------------------------
	//hide()

	/// <summary>
	/// 0 normal do nothing
	/// 1 monster fly
	/// 2 crew flash
	/// </summary>
	/// <param name="mode">Mode.</param>
	/// <param name="delayTime">Delay time.</param>
	public void RetreatFromBattle(int mode = 0, float delayTime = 0f)
	{
		switch (mode)
		{
		case 0:
			//normal do nothing
			LeaveBattle ();
			break;
		case 1:
			//monster fly
			this.gameObject.AddMissingComponent<AutoRotationY>().rotationSpeed = 40;
			Vector3 addVec = Vector3.zero;
			if (side == MonsterSide.Enemy)
			{
				addVec = new Vector3 (10f,0f,0f);
			}
			else
			{
				addVec = new Vector3 (-10f,0f,0f);
			}
			Goto (this.transform.position + addVec, 0.7f, ModelHelper.Anim_run);
			Invoke ("LeaveBattle", 0.7f);
			break;
		case 2:
			//crew flash
			AutoFlashGameObject autoFlashGameObject = this.gameObject.AddMissingComponent<AutoFlashGameObject>();
			autoFlashGameObject.flashDelay = delayTime + 0.2f;
			autoFlashGameObject.flashSpeed = 0.2f;
			Invoke ("LeaveBattle", delayTime + 0.5f);
			break;
		}
	}

	public void LeaveBattle()
	{
		BattleController.Instance.RemoveMonster (this);
		DestroyMe();
	}

	private bool HasDestroyMe = false;
	public void DestroyMe()
	{
		if (HasDestroyMe)
		{
			return;
		}
		
		HasDestroyMe = true;

		this.gameObject.RemoveComponent<AutoRotationY>();
		this.gameObject.RemoveComponent<AutoFlashGameObject>();
		
		DestroyPetModel();
		DestroyEffect();
		Destroy(gameObject);
	}
	
	public void DestroyPetModel()
	{
		ShutdownShadownMotion();
		DestroyBattleMount();
		
		if (battleBodyFadeInOut != null)
		{
			battleBodyFadeInOut.onFadeEffectFnishDelegate = null;
			battleBodyFadeInOut.Destroy();
			battleBodyFadeInOut = null;
		}
		if (petModelObject != null)
		{
			//Utility.RemoveFollowTrail(petModelObject);
			ResourcePoolManager.Instance.Despawn(petModelObject, ResourcePoolManager.PoolType.DONT_DESTROY);
			petModelObject = null;
		}

		RemoveAllBuff();
		DestroyHPSlider();
		DestroyMonsterName();
		DestroyMonsterSelectView ();
		if (_properyController != null) 
		{
			_properyController.RemoveStatusBarFunc(OnStatusBarClick);
		}
	}
	
	private bool HasDestroyEffect = false;
	public void DestroyEffect()
	{
		HasDestroyEffect = true;
	}
	
	public void ShowObject(bool enable)
	{
		gameObject.SetActive(enable);
		
		//NGUITools.SetActiveChildren( gameObject, enable );
		
		if (petModelObject == null)
		{
			return;
		}
		
		if (enable)
		{
			meshRenderer.enabled = true;
			Transform tr = transform;
			int childCount = tr.childCount;
			
			//			for ( int i = 0; i < childCount; ++i )
			//			{
			//				tr = transform.GetChild( i );
			//				if ( tr.name == "CirclePlane(Clone)" && targetReference <= 0 )
			//					tr.gameObject.SetActive( false );
			//			}
			
			if (petModelObject != null && !IsDead())
			{
				//battleBodyFadeInOut.alpha = 1.0f;
				
				//Cannot use delay to resolve this problem !!!!!
				//Invoke("PlayStateAnimation", 0.1f );   // Resolve Problem that animation doesn't play On Active
				PlayStateAnimation();
			}
			else if (IsDead())
			{
				meshRenderer.enabled = false;
				battleBodyFadeInOut.alpha = 0.0f;
				TransparentEffectShader(0.0f);
			}
			
		}
		else if (IsDead())
		{
			battleBodyFadeInOut.alpha = 0.0f;
			TransparentEffectShader(0.0f);
			ShutdownShadownMotion();
		}
		else
		{
			battleBodyFadeInOut.alpha = 0.0f;
			ShutdownShadownMotion();
		}
	}
	
	public void SetVisible(bool visible)
	{
		meshRenderer.enabled = visible;
		petCollider.enabled = visible;
	}
	
	public void BodyHide(float delayTime)
	{
		SetVisible(false);
		Invoke("BodyRedisplay", delayTime);
	}
	
	public void BodyRedisplay()
	{
		SetVisible(true);
	}
	
	public void PlayShadowMotion(float _time, float _scale, string color, float _alpha, string _blendMode, float _generateInterval, float lifeTime)
	{
		shadowMotionGenerator.Init(petModelObject, _time, _scale, color, _alpha, _blendMode, _generateInterval, lifeTime, myTransform.eulerAngles.y);
		ShadowMotion();
	}
	
	private void ShadowMotion()
	{
		if (shadowMotionGenerator != null)
		{
			shadowMotionGenerator.Run();
		}
	}
	
	public void ShutdownShadownMotion()
	{
		if (shadowMotionGenerator != null)
		{
			shadowMotionGenerator.StopGenerate();
		}
	}
	
	public void PlayInjure()
	{
		BattleController battleController = BattleController.Instance;
		if (battleController == null)
			return;
		
		if (existMessageEffect(MonsterController.ShowMessageEffect.DODGE) || existMessageEffect(MonsterController.ShowMessageEffect.CRITICAL))
		{
			AddStatusEffect();
			AddHPMPValue();
		}
		else
		{
			AddHPMPValue();
			AddStatusEffect();
		}
	}
	
	private void AddHPMPValue()
	{
		BattleController battleController = BattleController.Instance;
		if (battleController == null)
			return;
		
		bool critical = existMessageEffect(MonsterController.ShowMessageEffect.CRITICAL);

		if ((currentHP+modifyHP) > maxHP)
		{
			modifyHP = maxHP - currentHP;
		}

		currentHP += modifyHP;
		currentMP += modifyMP;
		currentSP += modifySP;

		if (currentHP <= 0)
		{
			this.dead = true;
		}
		else
		{
			this.dead = false;
		}
		
		//PetHealthControl.ControllerBar(mc, battleController, mc.modifyHP, mc.modifyMP);
		
		//0-ReduceHP 1-AddHP 2-Crit
		
		if (modifyHP != 0)
		{
			if (critical){
				AddFloatText("CHP"+","+modifyHP);
			}else{
				AddFloatText("HP"+","+modifyHP);
			}
		}
		
		//		if (modifyMP != 0)
		//		{
		//			AddFloatText("MP"+","+modifyMP);
		//		}
		//
		//		if (modifyRage != 0)
		//		{
		//			AddFloatText("SP"+","+modifyRage);
		//		}
		
		PlayFloatText();
	}
	
	private Queue<string> floatTextQueue = new Queue<string>();
	private bool floatTextPlaying = false;
	
	private void AddFloatText(string info)
	{
		floatTextQueue.Enqueue(info);
	}
	
	private void PlayFloatText()
	{
		if (floatTextQueue.Count == 0)
		{
			return;
		}
		
		if (floatTextPlaying)
		{
			return;
		}
		
		string info = floatTextQueue.Dequeue();
		
		string[] infos = info.Split(',');
		string type = infos[0];
		int modifyValue = int.Parse(infos[1]);
		
		Transform hudTransform = GetMountDamageffect();
		
		switch(type)
		{
		case "CHP":
			BattleStatusEffectManager.Instance.PlayDamage(GetNumValue(modifyValue), Color.white, hudTransform.gameObject, 1f, 2, 5f);
			break;
		case "HP":
			BattleStatusEffectManager.Instance.PlayDamage(GetNumValue(modifyValue), Color.white, hudTransform.gameObject, 1f, (modifyValue > 0 ? 1 : 0), 1f);
			break;
		}
		
		floatTextPlaying = true;
		Invoke("DelayPlayFloatText", 0.6f);
	}
	
	private void DelayPlayFloatText()
	{
		floatTextPlaying = false;
		PlayFloatText();
	}
	
	private string GetNumValue(int num)
	{
		if (num > 0)
		{
			return "+" + num;
		}
		else
		{
			return Math.Abs(num).ToString();
		}
	}
	
	private string GetRageValue(int num)
	{
		Debug.Log( "GetRageValue : " + num );
		if (num > 0)
		{
			return "+" + num;
		}
		else
		{
			return "-" + Math.Abs(num).ToString();
		}
	}
	
	private void AddStatusEffect()
	{
		BattleStatusEffectManager.Instance.AddEffect(this);
	}
	
	private Action _playSkillCallback;
	public void PlaySkillName (Skill skill, Action playSkillCallback)
	{
		BattleStatusEffectManager.Instance.PlaySkillName (this, skill);
		
		_playSkillCallback = playSkillCallback; 
		Invoke ("DelayPlaySkillNameCallback", 1f);
	}
	
	private void DelayPlaySkillNameCallback()
	{
		if (_playSkillCallback != null)
		{
			_playSkillCallback();
			_playSkillCallback = null;
		}
	}
	
	public void PlayMsg (string msg)
	{
		BattleStatusEffectManager.Instance.PlayMsg (this, msg);
	}
	
	public Transform GetMountingPoint(string point)
	{
		return ModelHelper.GetMountingPoint (petModelObject, point);
	}
	
	public Transform GetMountHUD()
	{
		return GetMountingPoint(ModelHelper.Mount_hud);
	}
	
	public Transform GetMountFace()
	{
		return GetMountingPoint(ModelHelper.Mount_face);
	}
	
	public Transform GetMountHit()
	{
		return GetMountingPoint(ModelHelper.Mount_hit);
	}
	
	public Transform GetMountShadow()
	{
		return GetMountingPoint(ModelHelper.Mount_shadow);
	}
	
	public Transform GetMountBattleEffect()
	{
		Transform hudTransform = transform.FindChild("floatTextGO");
		
		if (hudTransform == null)
		{
			hudTransform = GetMountHUD();
			
			if (hudTransform == null)
			{
				hudTransform = transform;
			}
			else
			{
				GameObject floatTextGO = NGUITools.AddChild(gameObject);
				floatTextGO.name = "floatTextGO";
				floatTextGO.transform.position = hudTransform.position;
				hudTransform = floatTextGO.transform;
			}
		}
		
		return hudTransform;
	}
	
	public Transform GetMountDamageffect()
	{
		Transform hudTransform = transform.FindChild("damageTextGO");
		
		if (hudTransform == null)
		{
			hudTransform = GetMountHit();
			
			if (hudTransform == null)
			{
				hudTransform = transform;
			}
			else
			{
				GameObject floatTextGO = NGUITools.AddChild(gameObject);
				floatTextGO.name = "damageTextGO";
				floatTextGO.transform.position = hudTransform.position;
				hudTransform = floatTextGO.transform;
			}
		}
		
		return hudTransform;
	}
	
	//获得固定的地面锚点
	public Transform GetBattleGroundMount()
	{
		Transform hudTransform = transform.FindChild("groundMountGO");
		
		if (hudTransform == null)
		{
			hudTransform = GetMountShadow();
			
			if (hudTransform == null)
			{
				hudTransform = transform;
			}
			else
			{
				GameObject go = NGUITools.AddChild(gameObject);
				go.name = "groundMountGO";
				go.transform.position = hudTransform.position;
				hudTransform = go.transform;
			}
		}
		
		return hudTransform;
	}
	
	private void DestroyBattleMount()
	{
		Transform hudTransform = transform.FindChild("floatTextGO");
		if (hudTransform != null)
		{
			GameObject.Destroy(hudTransform.gameObject);
		}
		
		hudTransform = transform.FindChild("groundMountGO");
		if (hudTransform != null)
		{
			GameObject.Destroy(hudTransform.gameObject);
		}
	}
	
	//---------------------------------------------------------------- 
	public void SetOutlineEffect(Color color, float lineWidth)
	{
		
		if (petMaterial != null && petMaterial.shader.name == "Baoyu/Baoyu_Additive_Color_2s"){
			return;
		}
		
		Shader shader = Shader.Find("Rim Light");
		if (shader != null && petMaterial != null)
		{
			petMaterial.shader = shader;
			petMaterial.SetColor("Rim Color", color);
			petMaterial.SetFloat("Main Color", lineWidth);
		}
	}
	
	public void CancelEffect()
	{
		PlayIdleAnimation();
		CancelInvoke("PlayIdleAnimation");
	}
	
	
	public void TransparentEffectShader(float alpha = 0.4f)
	{
		if (IsDead())
			return;
		
		Shader shader = Shader.Find("Transparent/Diffuse");
		
		if (shader != null && petMaterial != null){
			petMaterial.shader = shader;
			petMaterial.SetColor("_Color", new Color(1, 1, 1, alpha));			
		}
	}
	
	public void ResetRotation()
	{
		transform.eulerAngles = originRotation;
	}
	
	public void RotationToBack()
	{
		transform.eulerAngles = new Vector3(originRotation.x, originRotation.y-180, originRotation.z);
	}
	
	public void InstantiateGameObject(string effectName, Transform target, bool isOnGround = false, bool follow = false)
	{
		if (string.IsNullOrEmpty(effectName))
			return;
		
		if ( target == null )
		{
			target = this.gameObject.transform;
		}
		
		if (target == null)
			return;
		
		ResourcePoolManager.Instance.Spawn(effectName, delegate(UnityEngine.Object inst)
		                                   {
			GameObject go = inst as GameObject;
			if (go == null)
			{
				GameDebuger.Log("Instantiate Failed");
				return;
			}
			
			AudioHelper.PlayEffectSound(effectName);
			
			Transform t = go.transform;
			
			//跟随
			if (follow)
			{
				GameObjectExt.AddPoolChild(target.gameObject, go);
				NoRotation noRotation = GameObjectExt.GetMissingComponent<NoRotation>(go);
				
				if (isOnGround)
				{
					noRotation.fixYToZero = true;
				}
			}
			else{
				GameObjectExt.AddPoolChild(LayerManager.Instance.EffectsAnchor, go);
				t.position = target.position;
			}
			
			if (isOnGround)
				t.position = new Vector3(t.position.x, 0.02f, t.position.z);
			
			if (effectName == TargetClickEffectPath)
			{
				EffectTime effectTime = GameObjectExt.GetMissingComponent<EffectTime>(go);
				effectTime.time = 2;
			}
			
			if (effectName == TargetSelectEffectPath)
			{
				targetSelectEffect = go;
			}
			
		}, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
	}
	
	public void InstantiateGameObjectToParent(string effectName, Transform target, float x = 0.0f, float y = 0.0f, float z = 0.0f)
	{
		if (string.IsNullOrEmpty(effectName))
			return;
		
		if (target == null)
			return;
		
		ResourcePoolManager.Instance.Spawn(effectName, delegate(UnityEngine.Object inst)
		                                   {
			GameObject go = inst as GameObject;
			if (go == null || HasDestroyEffect)
			{
				GameDebuger.Log("Instantiate GameObject Failed");
				return;
			}
			
			AudioHelper.PlayEffectSound(effectName);
			
			GameObject effectGO = GameObjectExt.AddPoolChild(target.gameObject, go, x, y, z);
			
			effectGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
			BattleShadow shadow = GameObjectExt.GetMissingComponent<BattleShadow>(effectGO);
			shadow.Init(target.transform);
			
		}, ResourcePoolManager.PoolType.DONT_DESTROY);
	}
	
	private void OnJoinBattleEffectFinish()
	{
		InitPetModel();
	}
	
	private GameObject targetSelectEffect;
	public void PlayTargetSelectEffect()
	{
		//		if (targetSelectEffect == null)
		//		{
		//			InstantiateGameObject(TargetSelectEffectPath, GetMountHit(), false, true);
		//		}
	}
	
	public void StopTargetSelectEffect()
	{
		//		if (targetSelectEffect != null)
		//		{
		//			ResourcePoolManager.Instance.Despawn(targetSelectEffect, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
		//			targetSelectEffect = null;
		//		}
	}
	
	public void PlayTargetClickEffect()
	{
		InstantiateGameObject(TargetClickEffectPath, GetMountShadow(), false, true);
	}
	
	private static string SummonEffectPath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_Summon);
	
	public void PlaySummonEffect()
	{
		InstantiateGameObject(SummonEffectPath, GetMountShadow(), false, false);
	}
	
	#region SELECTOR
	public void SetSelectSkill(Skill skill)
	{
		battleTargetSelector = null;
		battleTargetSelector = new BattleTargetSelector(skill);
	}
	
	public BattleTargetSelector.TargetType GetTargetType()
	{
		if (battleTargetSelector != null)
			return battleTargetSelector.getTargetType();
		
		return BattleTargetSelector.TargetType.NONE;
	}
	
	public void SetSkillTarget(long targets)
	{
		if (battleTargetSelector != null)
		{
			battleTargetSelector.SetTargets(this, targets);
		}
		
		NeedReady = false;
	}
	
	public BattleTargetSelector GetBattleTargetSelector()
	{
		return battleTargetSelector;
	}
	
	private MonsterSelectView _selectView;
	public void ShowSelectEffect()
	{
		if (_selectView == null)
		{
			_selectView = MonsterSelectView.CreateNew(this);
		}
		_selectView.Show (true);
	}
	
	public void HideSelectEffect()
	{
		if (_selectView != null)
		{
			_selectView.Show (false);
		}
	}
	
	public void DestroyMonsterSelectView()
	{
		if (_selectView != null)
		{
			_selectView.Destroy();
			_selectView = null;
		}
	}
	
	#endregion
	
	public void InitMonster(VideoSoldier data, MonsterSide _side = MonsterSide.Player, bool _isStriker = true, BattleController _battleController = null, NpcAppearanceDto npcAppearanceDto = null)
	{
		side = _side;
		
		videoSoldier = data;
		
		if (IsPlayerMainCharactor ()) {
			_properyController = MainUIViewController.Instance.GetPlayerProperyController();
		} else if (IsPlayerPet ()) {
			_properyController = MainUIViewController.Instance.GetPetProperyController();
		}
		
		if (_properyController != null) {
			_properyController.SetupStatusBarFunc(OnStatusBarClick);
		}
		
		maxHP = data.maxHp;
		currentHP = data.hp;
		
		maxMP = data.maxMp;
		currentMP = data.mp;

		maxSP = data.maxSp;
		currentSP = data.sp;

		isStriker = _isStriker;
		
		battleController = _battleController;     //GET WARNING
		
		InitBuffStateMaps();
		
		InitPet( PetLookInfo.ToInfo(data,npcAppearanceDto), isStriker, _side == MonsterSide.Enemy || battleController.IsPvPGameVideo());
		gameObject.name = side.ToString() + "_" + GetId() + "_" + GetModel();
	}
	
	private void OnStatusBarClick()
	{
		battleController.ShowMonsterStatus (this);
	}
	
	private void ShowPetShoutContent()
	{
		if (videoSoldier.monsterType == Monster.MonsterType_Baobao || videoSoldier.monsterType == Monster.MonsterType_Mutate)
		{
			Pet pet = videoSoldier.monster.pet;
			if (string.IsNullOrEmpty (pet.shoutContent) == false) {
				ProxyActorPopoModule.Open (GetId(), GetMountHUD (),  pet.shoutContent, LayerManager.Instance.BattleCamera);
			}
		}
	}

    private void InitHudText()
    {
		if (side == MonsterSide.Player)
		{
			//初始化HPSlider
			hpSlider = BattleMonsterHPSlider.CreateNew(this);
		}
		//初始化MonsterName
		monsterName = BattleMonsterName.CreateNew(this);
		ShowMonsterName(battleController.monsterNameShow, true);
    }

	public bool IsPlayerMainCharactor()
	{
		if (IsPlayerCtrlCharactor() && GetCharactorType() == GeneralCharactor.CharactorType_MainCharactor)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsPlayerPet()
	{
		if (IsPlayerCtrlCharactor() && GetCharactorType() == GeneralCharactor.CharactorType_Pet)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

    public long GetId()
    {
		return videoSoldier.id;
    }

	//武将形象
	public int GetStyle()
	{
		Monster monster = videoSoldier.monster;
		GeneralCharactor charactor = videoSoldier.charactor;

		if (monster != null)
		{
			return monster.texture;
		}
		else
		{
			return charactor.texture;
		}
	}
	
	//武将模型
	public int GetModel()
	{
		Monster monster = videoSoldier.monster;
		GeneralCharactor charactor = videoSoldier.charactor;

		if (monster != null)
		{
			return monster.modelId;
		}
		else
		{
			return charactor.modelId;
		}
	}
	
	public int GetGrade()
	{
		return videoSoldier.grade;
	}

	public Faction GetFaction()
	{
		return videoSoldier.faction;
	}

	public bool IsPet()
	{
		return GetCharactorType () == GeneralCharactor.CharactorType_Pet;
	}

	public bool IsMonster()
	{
		return GetCharactorType () == GeneralCharactor.CharactorType_Monster;
	}

	public bool IsMainCharactor()
	{
		return GetCharactorType () == GeneralCharactor.CharactorType_MainCharactor;
	}

	public int GetCharactorType()
	{
		return videoSoldier.charactorType;
	}

	public bool IsPlayerCtrlCharactor()
	{
		if (GetPlayerId() == PlayerModel.Instance.GetPlayerId())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public long GetPlayerId()
	{
		return videoSoldier.playerId;
	}

	public int currentHP
	{
		get
		{
			return _currentHP;
		}
		set
		{
			_currentHP = value;
			if (_currentHP < 0){
				_currentHP = 0;
			}else if(_currentHP > maxHP && maxHP > 0){
				_currentHP = maxHP;
			}
		}
	}

	public int currentMP
	{
		get
		{
			return _currentMP;
		}
		set
		{
			_currentMP = value;
			if (_currentMP < 0){
				_currentMP = 0;
			}else if(_currentMP > maxMP && maxMP > 0){
				_currentMP = maxMP;
			}
		}
	}

	public int currentSP
	{
		get
		{
			return _currentSP;
		}
		set
		{
			_currentSP = value;
			if (_currentSP < 0){
				_currentSP = 0;
			}else if(_currentSP > maxSP && maxSP > 0){
				_currentSP = maxSP;
			}
		}
	}

	public GeneralCharactor GetCharacter()
	{
		return videoSoldier.charactor;
	}

	public int GetSoldierQuality()
	{
		int quality = 1;
//		if (videoSoldier.monster != null)
//		{
//			quality = videoSoldier.monster.quality;
//		}
//		else
//		{
//			quality = videoSoldier.hero.star;
//		}
		return quality;
	}

    public bool IsDead()
    {
		return dead;
    }

	public bool NeedLeave()
	{
		if (IsMonster() || IsPet())
		{
			return leave;
		}
		else
		{
			return false;
		}
	}

    private void InitBuffStateMaps()
    {
        buffStateMaps = new Dictionary<int, VideoBuffAddTargetState>();

        battleMonsterBuff = gameObject.AddComponent<BattleMonsterBuff>();
        battleMonsterBuff.SetMonster(this);
    }

    #region Buff State

	public void AddBuffState(VideoBuffAddTargetState buff, bool tip = true)
    {
        List<int> removeList = new List<int>();

        if (IsUnbeatableBuff(buff))
        {
            //无敌状态把所有其它独占状态清理掉
            foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
            {
                if (isMonopolizeBuff(keyVal.Value) || isPoisonBuff(keyVal.Value))
                {
                    removeList.Add(keyVal.Key);
                }
            }
        }
        else if (isMonopolizeBuff(buff))
        {
            //独占状态把其它独占状态清理掉
            foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
            {
                if (isMonopolizeBuff(keyVal.Value))
                {
                    removeList.Add(keyVal.Key);
                }
            }
        }
        else if (isPoisonBuff(buff))
        {
            //中毒状态覆盖旧的中毒状态
            foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
            {
                if (isPoisonBuff(keyVal.Value))
                {
                    removeList.Add(keyVal.Key);
                }
            }
        }
		else if (isHideBuff(buff))
		{
			TransparentEffectShader(0.5f);
		}
        else
        {
            //其它buff根据mutexType来判断是否替换
//            foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
//            {
//				if ((keyVal.Value.buffer.mutexType == buff.buffer.mutexType && buff.buffer.mutexType != 0)
//                    || (keyVal.Value.bufferId == buff.bufferId))
//                {
//                    removeList.Add(keyVal.Key);
//                }
//            }
        }

        for (int i = 0, len = removeList.Count; i < len; i++)
        {
            RemoveBuff(removeList[i]);
        }

		if (buffStateMaps.ContainsKey (buff.battleBuffId)) 
		{
			buffStateMaps[buff.battleBuffId] = buff;
		}
		else
		{
			buffStateMaps.Add(buff.battleBuffId, buff);
			battleMonsterBuff.AddBuff(buff, tip);
		}
    }

    public void UpdateBuffState()
    {
        List<int> removeList = new List<int>();

        foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
        {
            VideoBuffAddTargetState buff = keyVal.Value;
            if (buff.round > 0)
            {
				buff.round--;

				if (buff.round == 0)
				{
					removeList.Add(keyVal.Key);
				}
			}
		}
		
		foreach (int removeKey in removeList)
        {
            RemoveBuff(removeKey);
        }
    }

	public void RemoveBuffs(VideoBuffRemoveTargetState removeState)
    {
        foreach (int buffId in removeState.buffId)
        {
            RemoveBuff(buffId);
        }
    }

    public void RemoveBuff(int buffId)
    {
        if (buffStateMaps.ContainsKey(buffId))
        {
            VideoBuffAddTargetState buff = buffStateMaps[buffId];
            if (buff != null)
            {
				if (isHideBuff(buff))
				{
					ModelHelper.SetPetShadow (petModelObject);
				}

                buffStateMaps.Remove(buffId);
                battleMonsterBuff.RemoveBuff(buff);
            }
        }
    }

    public void ClearBuff()
    {
        List<int> removeList = new List<int>();

        foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
        {
            VideoBuffAddTargetState buff = keyVal.Value;
            if (buff.round != -1)
            {
                removeList.Add(keyVal.Key);
            }
        }

        foreach (int removeKey in removeList)
        {
            RemoveBuff(removeKey);
        }
    }

//    public void ClearBuffWithAiType(BufferAiTypeClearState state)
//    {
//        List<int> removeList = new List<int>();
//
//        foreach (KeyValuePair<int, VideoBuffAddTargetState> keyVal in buffStateMaps)
//        {
//            VideoBuffAddTargetState buff = keyVal.Value;
//            if (buff.round != -1)
//            {
//                if (state.types.Contains(buff.buff.skillAiType))
//                {
//                    removeList.Add(keyVal.Key);
//                }
//            }
//        }
//
//        foreach (int removeKey in removeList)
//        {
//            RemoveBuff(removeKey);
//        }
//    }

    public void RemoveAllBuff()
    {
        buffStateMaps.Clear();
        battleMonsterBuff.RemoveAllBuff();
    }

    //是否无敌buff
    private bool IsUnbeatableBuff(VideoBuffAddTargetState buff)
    {
//        if (buff.buffId == 7 || buff.buffId == 8)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }

		return false;
    }

    //是否中毒buff
    private bool isPoisonBuff(VideoBuffAddTargetState buff)
    {
        //return buff.buffId == 1;
		return false;
    }

	//是否隐身buff
	private bool isHideBuff(VideoBuffAddTargetState buff)
	{
		return buff.battleBuff.buffType == BattleBuff.BattleBuffType_Hidden;
	}
	
	//是否独占状态buff
	private bool isMonopolizeBuff(VideoBuffAddTargetState buff)
    {
        //return buff.buffId <= 13 && buff.buffId != 1;
		return false;
    }

	public bool ContainsBuff(int buffId)
	{
		return buffStateMaps.ContainsKey (buffId);
	}
	
	//是否可以选择
	public bool CanChoose()
	{
		//        if (buffStateMaps.ContainsKey(23) || buffStateMaps.ContainsKey(26))
//        {
//            return false;
//        }
//        else
//        {
            return true;
//        }
    }

    #endregion

	public void DestroyHPSlider()
	{
		if (hpSlider != null)
		{
			hpSlider.Destroy();
			hpSlider = null;
		}
	}

	public void DestroyMonsterName()
	{
		if (monsterName != null)
		{
			monsterName.Destroy();
			monsterName = null;
		}
	}

	private bool monsterNameShow = true;
	public void ShowMonsterName(bool show, bool atonce = false)
	{
		if (monsterName != null)
		{
			if (monsterNameShow != show)
			{
				monsterNameShow = show;
				if (show)
				{
					if (atonce){
						TweenUtils.PlayAlphaTween(monsterName.gameObject, 1, 1);
					}else{
						TweenUtils.PlayAlphaTween(monsterName.gameObject, 0, 1);
					}
				}
				else
				{
					if (atonce){
						TweenUtils.PlayAlphaTween(monsterName.gameObject, 0, 0);
					}else{
						TweenUtils.PlayAlphaTween(monsterName.gameObject, 1, 0);
					}
				}
			}
		}
	}

    #region Base Action Node Player
    //---------------------------------Base Action Node Player---------------------------------------------------
    MonsterController skillTarget;

    public void SetSkillTarget(MonsterController enemyTarget)
    {
        skillTarget = enemyTarget;
		if (skillTarget == null)
		{
			Debug.LogError("SetSkillTarget is null");
		}
    }

    public MonsterController GetSkillTarget()
    {
        return skillTarget;
    }

    void Update()
    {
		if (_properyController != null)
		{
			_properyController.SetHpSlider(_currentHP, maxHP);
			_properyController.SetMpSlider(_currentMP, maxMP);

			if (IsPlayerMainCharactor())
			{
				ProxyPlayerPropertyModule.UpdateHpValue(_currentHP, maxHP);
				ProxyPlayerPropertyModule.UpdateMpValue(_currentMP, maxMP);
				//ProxyPlayerPropertyModule.UpdateSpValue(_currentMP, maxMP);
			}

			if (IsPet())
			{
				PetPropertyInfo petPropertyInfo = PetModel.Instance.GetPetInfoByUID(videoSoldier.id);
				ExpGrade expGrade = DataCache.getDtoByCls<ExpGrade> (petPropertyInfo.petDto.level+1);
				_properyController.SetExpSlider (petPropertyInfo.petDto.exp,expGrade.petExp);
			}
			else
			{
				_properyController.SetSpSlider(_currentSP, maxSP);
			}
		}
        UpdateAnimationDelaTime();
    }

    public void PlayMoveNode(MoveActionInfo node)
    {
        GameObject go = new GameObject();
        Transform targetTransform = go.transform;

        float distance = node.distance;

        if (node.center)
        {
            targetTransform.position = Vector3.zero;
            distance = 0f;
        }
		else
		{
			if (skillTarget != null)
			{
				targetTransform.position = skillTarget.transform.position;
				targetTransform.rotation = skillTarget.transform.rotation;
			}
			else
			{
				targetTransform.position = Vector3.zero;
				distance = 0f;
			}
		}

        MoveTo(targetTransform, distance, node.name, node.time, new Vector3(node.rotateX, node.rotateY, node.rotateZ));
        Destroy(go);
    }

    //-------------------------- Play Effect----------------------------------
//    public void PlayTrailEffect(TrailEffectNode node)
//    {
//        Color color = ColorExt.HexToColor(node.color);
//        color.a = node.alpha;
//
//        string matName = node.matName; // node.texture;
//
//        AddTrailEffect(color, node.delayTime, node.trailTime, node.mountTrailStart, node.mountTrailEnd,matName);
//    }
//
//    public void PlayBodyShift(BodyShiftEffectNode node)
//    {
//		BodyShift(node.distance, node.time, node.aspect);
//    }
//
//    public void PlayBodyHide(HideEffectNode node)
//    {
//        BodyHide(node.delayTime);
//    }
//
//    public void PlayShadowMotionNode(GhostEffectNode node)
//    {
//        PlayShadowMotion(node.trailingDelayTime, node.scale, node.color,
//                            node.alpha, node.blendMode, node.gapFrame, node.delayTime);
//    }
//
//    public void PlayBodyInjure(ShowInjureEffectNode node)
//    {
//        PlayInjure(node.batterIndex);
//    }
    //-------------------------------------------------------------------------

    public void PlayNormalNode(NormalActionInfo node)
    {
        PlayNormalActionInfo(node.name, node.delayTime);
    }

    public void PlayMoveBackNode(MoveBackActionInfo node)
    {
        GoBack(node.time, new Vector3(node.rotateX, 180, node.rotateZ), node.name);
    }

    #endregion


    public void PlayInjureSpecialEffect(int skillID)
    {
        if (skillID == 2)
        {
            _PlayInjurePropertyEffect("ArtResources/Prefabs/Effect/Skill/skill_1001");
            return;
        }
    }

    private void _PlayInjurePropertyEffect(string tmp)
    {
        Transform tr = GetMountHit();

        if (tr == null)
            return;
//
//        ResourcePoolManager.Instance.Spawn(tmp, delegate(UnityEngine.Object inst)
//        {
//
//            GameObject effectGO = inst as GameObject;
//            if (effectGO == null)
//                return;
//
//            EffectTime effectTime = effectGO.GetComponent<EffectTime>();
//            if (effectTime != null)
//            {
//                effectTime.Play();
//            }
//
//            AudioHelper.PlayEffectSound(tmp);
//
//            effectGO.transform.position = tr.position;
//            effectGO.transform.rotation = Quaternion.identity;
//
//            float scaleValue = 1;
//
//            if (this.videoSoldier.pet.population == 2)
//            {
//                scaleValue *= 3f;
//            }
//            else if (this.videoSoldier.pet.population == 3)
//            {
//                scaleValue *= 7f;
//            }
//
//            //Set Effect Scale
//            ParticleScaler scaler = effectGO.GetComponent<ParticleScaler>();
//            if (scaler == null)
//            {
//                scaler = effectGO.AddComponent<ParticleScaler>();
//            }
//
//            scaler.SetscaleMultiple(scaleValue);
//            scaleValue *= scaler.particleScale;
//            effectGO.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
//            //scaler.particleScale = scaleValue;	
//
//        }, ResourcePoolManager.PoolType.DESTROY_CHANGE_SCENE);
    }

	public bool NeedReady = false;

	private static string PetCatchEffect_base = "ArtResources/Prefabs/Effect/Common/Eff_game/eff_game_PetCatch_01";
	private static string PetCatchEffect_fail = "ArtResources/Prefabs/Effect/Common/Eff_game/eff_game_PetCatch_02";	
	private static string PetCatchEffect_success = "ArtResources/Prefabs/Effect/Common/Eff_game/eff_game_PetCatch_03";	

	private static string PetSummonEffect = "ArtResources/Prefabs/Effect/Common/Eff_game/eff_game_PetCatch_03";	

	public void ShowCatchEffect(bool catchSuccess)
	{
        ResourcePoolManager.Instance.Spawn(PetCatchEffect_base, delegate(UnityEngine.Object inst)
        {
            GameObject go = inst as GameObject;
            if (go == null){
				return;
			}
			
			Transform target = GetMountShadow();
            Transform t = go.transform;
            t.position = target.position;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            t.position = new Vector3(t.position.x, 0f, t.position.z);			
			
            AudioHelper.PlayEffectSound(PetCatchEffect_base);

			EffectTime effectTime = GameObjectExt.GetMissingComponent<EffectTime>(go);
			effectTime.time = 3f;
			effectTime.loopCount = 1;
			go.SetActive(false);
			effectTime.Reset();
			effectTime.Play();
			
		}, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
		
		string petCatchEffect2 = catchSuccess?PetCatchEffect_success:PetCatchEffect_fail;
        ResourcePoolManager.Instance.Spawn(petCatchEffect2, delegate(UnityEngine.Object inst)
        {
            GameObject go = inst as GameObject;
            if (go == null){
				return;
			}
			
			Transform target = GetMountShadow();
            Transform t = go.transform;
            t.position = target.position;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            t.position = new Vector3(t.position.x, 0f, t.position.z);				
			
            AudioHelper.PlayEffectSound(petCatchEffect2);

			EffectTime effectTime = GameObjectExt.GetMissingComponent<EffectTime>(go);
			effectTime.delayTime = 2f;
			effectTime.loopCount = 1;
			effectTime.time = 2f;
			go.SetActive(false);
			effectTime.Reset();
			effectTime.Play();
			
		}, ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
	}

	public string GetDebugInfo()
	{
		return "[" + videoSoldier.id + " " + videoSoldier.name + " "+currentHP+"/"+maxHP + " " + currentMP+"/"+maxMP + " "+videoSoldier.position + " " + GetCharactorType() + " " + IsDead() + "]";
	}
}
