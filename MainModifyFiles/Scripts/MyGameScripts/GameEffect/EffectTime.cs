using UnityEngine;
using System.Collections;
using System;

public class EffectTime : MonoBehaviour {

    public float delayTime = 0; //特效播放延时
	public float time = 5; //特效时长
    public float breakoutTime = 1; //特效爆发时间
    public int loopCount = 1;   //0 一直循环   1 一次循环   >1 循环次数
	
	public delegate void OnEffectDestroyDelegate();
	public OnEffectDestroyDelegate OnEffectDestroy;	
	
	private Action< EffectTime > OnEffectDestroyWithSelf;	
	
	private float _delayTime = 0.0f;
	private float _time      = 0.0f;
	private float _breakoutTime = 0.0f;
	private int   _loopCount = 1;
	
	private Animation[] anims = null;
	private ParticleSystem[] particleSystemAttay = null;
	private TimedObjectDestructor[] timeDestructorArray = null;
	
	private bool _isStart = false;
	private bool _isPlayering = false;
    void Start()
    {
		anims = this.GetComponentsInChildren< Animation >();
		
		particleSystemAttay = this.GetComponentsInChildren<ParticleSystem>();
		timeDestructorArray = this.GetComponentsInChildren<TimedObjectDestructor>();
		
		
		Reset();
        Play();
		_isStart = true;
    }
	
	void OnEnable()
	{
		if( _isStart && !_isPlayering )
		{
			Reset();
	        Play();		
		}
	}
	
    public void Play()
    {
		_isPlayering = true;
		if (!IsInvoking("DoPlay")){
			CancelInvoke();
			Invoke("DoPlay", _delayTime);
		}
    }

    private void DoPlay()
    {
        this.gameObject.SetActive(true);

        if (_loopCount == 0)
        {
            return;
        }

        if (_loopCount == 1)
        {
			//Debug.Log("Effect DoPlay: " +  gameObject.name );
			Invoke("DestroyObjectBySelf", _time);
        }
        else
        {
			
			//Debug.Log("Effect NextLoop: " +  gameObject.name );
            Invoke("NextLoop", _time);
        }
    }

    private void NextLoop()
    {
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);

            _loopCount--;
            DoPlay();
        }
    }
	
	public void DestroyObject()
	{
		if (OnEffectDestroy != null){
			OnEffectDestroy();
			OnEffectDestroy = null;
		}
		
		_isPlayering = false;
		CancelInvoke();

		this.transform.localEulerAngles = Vector3.zero;
		this.transform.localPosition = Vector3.zero;
		Reset();
		//Debug.Log( "Destroy Effect : " +  gameObject.name );

		if (this.gameObject.name.Contains ("_fly")) 
		{
			Destroy(gameObject);	
		}
		else 
		{
			ResourcePoolManager.Instance.Despawn(gameObject);
		}
	}

	public void DestroyObjectBySelf()
	{
		if( OnEffectDestroyWithSelf != null )
		{
			OnEffectDestroyWithSelf(this);
			OnEffectDestroyWithSelf = null;
		}		
		
		DestroyObject();
	}
	
	
	public void Reset()
	{
		 _delayTime 	= delayTime;
		 _time      	= time;
		 _breakoutTime  = breakoutTime;
		 _loopCount 	= loopCount;
		
		
//		if( anims != null )
//		{
//			foreach( Animation anim in anims )
//			{
//				Debug.Log("Animation name : " + anim.clip.name );
//				anim[anim.clip.name].normalizedTime = 0.0f;
//				anim.Play( anim.clip.name );
//			}
//		}
		
		
		if( particleSystemAttay != null )
		{
			foreach( ParticleSystem ps in particleSystemAttay )
			{
				if( ps != null )
				{
					ps.Simulate(0.0f);
					ps.Play();
				}
				else
				{
					Debug.Log( " ParticleSystem Componet is Null ");
				}
			}
		}
		
		if( timeDestructorArray != null )
		{
			foreach( TimedObjectDestructor destructor in timeDestructorArray )
			{
				destructor.Reset();
			}
		}

	}
	
	public void SetEffectDestoryWithSelf( Action< EffectTime > func )
	{
		this.OnEffectDestroyWithSelf = func;
	}
}