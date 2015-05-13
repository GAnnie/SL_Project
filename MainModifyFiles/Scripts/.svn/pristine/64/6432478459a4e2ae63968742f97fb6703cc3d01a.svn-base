using UnityEngine;
using System.Collections;

public class NGUIFadeInOut : MonoBehaviour 
{
    public float fadeSpeed = 1.0f;

    private  float finishDelayTime = 0.0f;

    //public float alpha = 1.0f;

    public delegate void FinishFadeEffect();
    public FinishFadeEffect finishFadeEffect;

    public bool autoPlay = true;

    [HideInInspector]
    public bool isPlaying = false;

    public bool forwardAlpha = false;

    public string tip = "";

    private GameObject _fadeInOutSprite = null;
    private TweenAlpha _uiSpriteAlpha = null;

    private GameObject _circleSprite = null;
    private UILabel _tipLabel = null;

    private UITexture _logoSprite = null;
    private TweenAlpha _logoSpriteAlpha = null;

    private static NGUIFadeInOut fadeInOut;

    private System.Action _logoShowFinishCallBack = null;

    //由黑切入白
    public static NGUIFadeInOut FadeIn(NGUIFadeInOut.FinishFadeEffect callback = null, float fadeSpeed = 1f,float finishDelayTime = 0.0f)
    {
        fadeInOut.tip = "";
        fadeInOut.fadeSpeed = fadeSpeed;
        fadeInOut.finishDelayTime = finishDelayTime;
        fadeInOut._FadeIn(callback, finishDelayTime);
        return fadeInOut;
    }

    //由白切入黑
    public static NGUIFadeInOut FadeOut(NGUIFadeInOut.FinishFadeEffect callback = null, string tip = "", float fadeSpeed = 1f, float finishDelayTime = 0.0f)
    {
        fadeInOut.tip = tip;
        fadeInOut.fadeSpeed = fadeSpeed;
        fadeInOut.finishDelayTime = finishDelayTime;
        fadeInOut._FadeOut(callback, finishDelayTime);
        return fadeInOut;
    }

	//文字从下往上移动并突出文字效果//
	public static void FadeWithCharactorLoad( NGUIFadeInOut.FinishFadeEffect callBack = null , string tip = "" , int fadeSpeed = 1 , float finishDelayTime = 0.0f )
	{

		fadeInOut.LoadCharactor( tip , callBack ,fadeSpeed , finishDelayTime  );
	}

	private void LoadCharactor(  string tip = "" ,NGUIFadeInOut.FinishFadeEffect callBack = null , int fadeSpeed = 1 , float finishDelayTime = 0.0f )
	{
		GameObject go = NGUITools.AddChild( this.gameObject , ResourcePoolManager.Instance.SpawnUIPrefab( "Prefabs/CharactorLoadBeforeChoseRole/CharactorLoadBeforeChoseRole" ) as GameObject );
//		CharactorLoadBeforeChoseRole Charactor = go.GetComponent< CharactorLoadBeforeChoseRole >();
//		if( Charactor == null )	return;
//		Charactor.SetData( tip , callBack ,fadeSpeed ,  finishDelayTime );
	}

    //立刻黑屏
    public static void FadeOutAtOnce()
    {
        fadeInOut._Stop();
    }
	
    //取消黑屏
    public static void FadeInAtOnce()
    {
        fadeInOut._FadeInAtOnce();
    }	
	
    //是否不是全黑状态
    public static bool IsNotAllBlack()
    {
        if (fadeInOut._uiSpriteAlpha != null)
        {
            return fadeInOut._uiSpriteAlpha.alpha < 1;
        }
        return false;
    }

    public static NGUIFadeInOut GetFadeInOut()
    {
        return fadeInOut;
    }

    public static void SetTipLabelPivotToLeft(){
        fadeInOut._SetTipLabelPivotToLeft();
    }
	
	public static void ShowLogo( System.Action loginFinishCallBack = null){
		fadeInOut._ShowLogo( loginFinishCallBack);
	}
	
	public static void RemoveLogo(){
		fadeInOut._RemoveLogo();
	}

    void Awake()
    {
        fadeInOut = this;
    }

	// Use this for initialization
	void Start () 
    {
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/NGUIFadeInOut/NGUIFadeInOutPrefab") as GameObject;
        if (prefab != null)
        {
            _fadeInOutSprite = NGUITools.AddChild(this.gameObject, prefab);
            _uiSpriteAlpha = _fadeInOutSprite.transform.FindChild("BlackSprite").gameObject.GetComponent<TweenAlpha>();
            if (_uiSpriteAlpha == null)
            {
                _uiSpriteAlpha = _fadeInOutSprite.transform.FindChild("BlackSprite").gameObject.AddComponent<TweenAlpha>();
            }

            _circleSprite = _fadeInOutSprite.transform.FindChild("CircleSprite").gameObject;
            _tipLabel = _fadeInOutSprite.transform.FindChild("TipLabel").gameObject.GetComponent<UILabel>();

            _logoSprite = _fadeInOutSprite.transform.FindChild("LogoSprite").gameObject.GetComponent<UITexture>();
            _logoSpriteAlpha = _logoSprite.gameObject.GetComponent<TweenAlpha>();
            if (_logoSpriteAlpha == null)
            {
                _logoSpriteAlpha = _logoSprite.gameObject.AddComponent<TweenAlpha>();
            }

            _logoSprite.gameObject.SetActive(false);

            _circleSprite.SetActive(false);
            _tipLabel.text = "";

            _uiSpriteAlpha.from = 1;
            _uiSpriteAlpha.to = 0;
            
            _uiSpriteAlpha.eventReceiver = this.gameObject;
            _uiSpriteAlpha.callWhenFinished = "CallFinishFadeEffect";
        }

        if (autoPlay)
        {
            _FadeIn();
        }
	}

	// Update is called once per frame
	void Update () 
    {
        if (_circleSprite != null && _circleSprite.activeSelf)
        {
            _circleSprite.transform.Rotate(0, 0, -630 * Time.deltaTime);
        }	    
	}

    //--------------------------------------------------------------------

    private void _FadeIn(FinishFadeEffect finishCallBackFunc = null, float finishDelayTime = 0.0f)
    {
        if (_fadeInOutSprite != null)
        {
            _fadeInOutSprite.SetActive(true);
        }

        if (_circleSprite != null)
        {
            _circleSprite.SetActive(false);
        }

        if (_tipLabel != null)
        {
            _tipLabel.text = "";
        }

        _tipLabel.gameObject.transform.localPosition = Vector3.zero;
        _tipLabel.pivot = UIWidget.Pivot.Center;

        this.finishFadeEffect = finishCallBackFunc;
        //_playing = true;
        if (_uiSpriteAlpha != null)
        {
            //_uiSpriteAlpha.Reset();
            isPlaying = true;
            _uiSpriteAlpha.alpha = 0.9f;
            _uiSpriteAlpha.duration = fadeSpeed;
            _uiSpriteAlpha.Play(true);
            forwardAlpha = true;
        }
        else
        {
            isPlaying = false;
        }
    }

    //--------------------------------------------------------------------

    private void _FadeOut(FinishFadeEffect finishCallBackFunc = null, float finishDelayTime = 0.0f)
    {
        if (_fadeInOutSprite != null)
        {
            _fadeInOutSprite.SetActive(true);
        }

        if (_circleSprite != null)
        {
            _circleSprite.SetActive(false);
        }

        if (_tipLabel != null)
        {
            _tipLabel.text = "";
        }

        this.finishFadeEffect = finishCallBackFunc;
        if (_uiSpriteAlpha != null)
        {
            //_uiSpriteAlpha.Reset();
            isPlaying = true;
            _uiSpriteAlpha.alpha = 0;
            _uiSpriteAlpha.duration = fadeSpeed;
            _uiSpriteAlpha.Play(false);
            forwardAlpha = false;
        }
        else
        {
            isPlaying = false;
        }
    }

    private void _Stop()
    {
		if( _uiSpriteAlpha != null )
		{
            _uiSpriteAlpha.ResetToBeginning();
			_uiSpriteAlpha.alpha = 0;

            _fadeInOutSprite.SetActive(false);

            forwardAlpha = false;
		}
        isPlaying = false;
    }

    private void _FadeOutAtOnce()
    {
		if( _uiSpriteAlpha != null )
		{
            _uiSpriteAlpha.ResetToBeginning();
			_uiSpriteAlpha.alpha = 1;

            _fadeInOutSprite.SetActive(true);

            forwardAlpha = false;
		}
        isPlaying = false;
    }	
	
    private void _FadeInAtOnce()
    {
		if( _uiSpriteAlpha != null )
		{
            _uiSpriteAlpha.ResetToBeginning();
			_uiSpriteAlpha.alpha = 0;

            _fadeInOutSprite.SetActive(false);

            forwardAlpha = true;
		}
        isPlaying = false;
    }	
	
    private void _ShowLogo(System.Action loginFinishCallBack = null)
    {
		Texture2D tex = ResourceLoader.Load( PathHelper.IMAGES_PATH+"LoadingStart/loading_start", "jpg" ) as  Texture2D;
        if (tex != null)
        {
            _logoSprite.mainTexture = tex;
            _logoShowFinishCallBack = loginFinishCallBack;

            _logoSpriteAlpha.delay = 1.0f;
            _logoSpriteAlpha.alpha = 0.0f;
            _logoSpriteAlpha.from = 0.0f;
            _logoSpriteAlpha.to = 1.0f;

            _logoSprite.gameObject.SetActive(true);

            _logoSpriteAlpha.eventReceiver = this.gameObject;
            _logoSpriteAlpha.callWhenFinished = "CallFinishLogoEffect";

            _logoSpriteAlpha.duration = 2.0f;
            _logoSpriteAlpha.Play(true);
        }
        else
        {
            GameDebuger.Log("Texture : " + PathHelper.IMAGES_PATH + "LoadingStart/loading_start" + "is not Exit ");
        }

    }

    private void CallFinishLogoEffect()
    {
        Invoke("InvokeCallFinishLogoEffect", 2.0f);
    }

    private void InvokeCallFinishLogoEffect()
    {
        _logoSpriteAlpha.Play(false);
        _logoSpriteAlpha.callWhenFinished = "CallFinishLogoEffect2"; 
    }

	private void CallFinishLogoEffect2(){
		_logoSpriteAlpha.callWhenFinished = null;
		_RemoveLogo();

        if (_logoShowFinishCallBack != null)
        {
            _logoShowFinishCallBack();
            _logoShowFinishCallBack = null;
        }
	}
	
    private void _RemoveLogo()
    {
        if (_logoSprite != null)
        {
			Texture tex = _logoSprite.mainTexture;
			if (tex != null){
				_logoSprite.mainTexture = null;
				Resources.UnloadAsset(tex);
			}
            GameObject.Destroy(_logoSprite.gameObject);
            _logoSprite = null;
            _logoSpriteAlpha = null;
        }
    }

    private void _SetTipLabelPivotToLeft()
    {
        _tipLabel.gameObject.transform.localPosition = new Vector3(-240, 0, 0);
        _tipLabel.pivot = UIWidget.Pivot.Left;
    }

    //----------------------------------------------------------------------
    private void CallFinishFadeEffect()
    {
        if (string.IsNullOrEmpty(tip) == false)
        {
            _tipLabel.text = tip;
            if (tip == "进入战斗")
            {
                _circleSprite.SetActive(true);
            }
        }

        tip = "";

        if (_uiSpriteAlpha.alpha == 0)
        {
            _fadeInOutSprite.SetActive(false);
        }

        isPlaying = false;


        if (finishDelayTime > 0.0f && finishFadeEffect != null)
        {
            this.Invoke( "DelayCallBackFinishEffect", finishDelayTime );
        }
        else
        {
            if (finishFadeEffect != null)
            {
                finishFadeEffect();
                finishFadeEffect = null;
            } 
        }

    }

    private void DelayCallBackFinishEffect()
    {
        if (finishFadeEffect != null)
        {
            finishFadeEffect();
            finishFadeEffect = null;
        }
    }
}
