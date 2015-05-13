using UnityEngine;
using System.Collections;

public class UIBattleStatusEffect : MonoBehaviour
{
	private Transform myTransform;
	
	private Transform targetTransform;
	private Camera cam;

	public UISprite statusSprite = null;
	public UIFont[] fontType; //0-ReduceHP 1-AddHP  2-Crit


	public GameObject damageGO = null;
	public UILabel damageLabel = null;
	public UISprite critSprite = null;

	public GameObject skillGO = null;
	public UILabel nameLabel = null;

	public UILabel msgLabel = null;

	private static GameObject _skillEffectGO;

	void Awake()
	{
		myTransform = transform;
	}

	public static void InitEffect()
	{
		if (_skillEffectGO == null)
		{
			string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_SkillName);
			
			ResourcePoolManager.Instance.Spawn(effpath, delegate(UnityEngine.Object inst)
			                                   {
				_skillEffectGO = inst as GameObject;
				if (_skillEffectGO == null)
				{
					GameDebuger.Log("Instantiate GameObject Failed");
					return;
				}

				GameObjectExt.AddPoolChild(LayerManager.Instance.EffectsAnchor,_skillEffectGO);

				Renderer []renderList = _skillEffectGO.GetComponentsInChildren<Renderer>(true);
				foreach(Renderer render in renderList)
				{
					if (render.sharedMaterial != null)
					{
						render.sharedMaterial.renderQueue = UILayerType.RenderQueue_UIEffect;
					}
				}

				_skillEffectGO.SetActive(false);
				
			}, ResourcePoolManager.PoolType.DONT_DESTROY);
		}
	}

	public void ShowStatusEffect( Camera battleCam, GameObject target, string effectName )
	{
		if ( target == null )
			return;
		
		if ( battleCam == null )
			return;

		HideAll ();
		statusSprite.gameObject.SetActive(true);

		statusSprite.spriteName = effectName;

		statusSprite.MakePixelPerfect();
		
		targetTransform = target.transform;
		cam = battleCam;
		
//		myTransform.localScale = new Vector3( myTransform.localScale.x,
//											  myTransform.localScale.y, 1 );
		
		iTween.MoveTo( statusSprite.gameObject,iTween.Hash( "y" , myTransform.localPosition.y + 40.0f , "time" , 1f,
															"oncompletetarget", gameObject, "oncomplete", "DestroyMe", "isLocal", true ) );
	}

	public void ShowDamage(Camera battleCam, GameObject target, string msg, int fontIndex)
	{
		if ( target == null )
			return;
		
		if ( battleCam == null )
			return;

		HideAll ();
		damageLabel.gameObject.SetActive(true);

		damageLabel.font = fontType[fontIndex];
		damageLabel.fontSize = 20;
		damageLabel.text = msg;

		if (fontIndex == 2)
		{
			critSprite.gameObject.SetActive (true);
		}
		else
		{
			critSprite.gameObject.SetActive (false);
		}

		targetTransform = target.transform;
		cam = battleCam;
		
		//		myTransform.localScale = new Vector3( myTransform.localScale.x,
		//											  myTransform.localScale.y, 1 );
		
		iTween.MoveTo( damageGO,iTween.Hash( "y" , myTransform.localPosition.y + 40.0f , "time" , 1f,
		                                                   "oncompletetarget", gameObject, "oncomplete", "DestroyMe", "isLocal", true ) );
	}

	public void ShowSkillName(string name, Camera battleCam, Transform transform)
	{
		if ( transform == null )
			return;
		
		if ( battleCam == null )
			return;

		HideAll ();

		string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_SkillName);

		Debug.Log ("effpath="+effpath);

		if (_skillEffectGO != null) {
			_skillEffectGO.SetActive(true);
			GameObjectExt.AddPoolChild(LayerManager.Instance.EffectsAnchor,_skillEffectGO);
			_skillEffectGO.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		}

		skillGO.SetActive (false);

		nameLabel.text = "[b]"+name;

		targetTransform = transform;
		cam = battleCam;

		Invoke ("DoShowSkillName", 0.2f);
		Invoke ("HideSkillEff", 2f);
	}

	private void DoShowSkillName()
	{
		skillGO.SetActive (true);
		PlaySkillScale (skillGO, 1f, 0f, 0.7f, null, UITweener.Method.Linear);
	}

	private void HideSkillEff()
	{
		if (_skillEffectGO != null)
		{
			_skillEffectGO.SetActive(false);
		}
	}

	//	主界面UIBtn动画效果
	public void PlaySkillScale(GameObject go,
	                                  float fromScale, float toScale,
	                                  float duration,
	                                  EventDelegate.Callback callBack = null,
	                                  UITweener.Method method = UITweener.Method.Linear,
	                                  float delay = 0f)
	{
		TweenScale tweenScale = GameObjectExt.GetMissingComponent<TweenScale>(go);
		tweenScale.duration = duration;

		tweenScale.from = new Vector3(fromScale, fromScale, fromScale);
		tweenScale.to = new Vector3(toScale, toScale, toScale);
		tweenScale.method = method;

		go.transform.localScale = tweenScale.from;

		tweenScale.animationCurve = new AnimationCurve(
			new Keyframe(0f,0f),
			new Keyframe(0.8f,0.15f),
			new Keyframe(1f,1f)
			);
		
		tweenScale.delay = delay;
		
		tweenScale.SetOnFinished(callBack);

		tweenScale.enabled = true;
		tweenScale.ResetToBeginning ();
		tweenScale.PlayForward ();
	}

	public void ShowMsg(string msg, Camera battleCam, Transform transform)
	{
		if ( transform == null )
			return;
		
		if ( battleCam == null )
			return;
		
		HideAll ();
		msgLabel.gameObject.SetActive(true);

		msgLabel.text = msg;
		
		targetTransform = transform;
		cam = battleCam;
		
		//		myTransform.localScale = new Vector3( myTransform.localScale.x,
		//											  myTransform.localScale.y, 1 );
		
		iTween.MoveTo( msgLabel.gameObject,iTween.Hash( "y" , myTransform.localPosition.y + 70.0f , "time" , 2f,
		                                                  "oncompletetarget", gameObject, "oncomplete", "DestroyMe", "isLocal", true ) );
	}

	private void HideAll()
	{
		damageLabel.gameObject.SetActive(false);
		statusSprite.gameObject.SetActive(false);
		skillGO.SetActive (false);
		msgLabel.gameObject.SetActive (false);
		critSprite.gameObject.SetActive (false);
	}

	void LateUpdate()
	{
		if ( targetTransform == null )
			return;

		if ( cam == null )
			return;
		
		Vector3 position = cam.WorldToViewportPoint( targetTransform.position );
		position = UICamera.currentCamera.ViewportToWorldPoint( position );
		position.z = myTransform.position.z;
		
		myTransform.position = position;

		targetTransform = null;
	}
	
	void DestroyMe()
	{
		Destroy( gameObject );
	}
}
