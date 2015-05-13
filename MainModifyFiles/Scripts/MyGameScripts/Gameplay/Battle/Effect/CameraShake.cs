using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	Transform myTransform = null ;

	public float shakeIntensity = 0.3f;
	public float shakeDecay = 0.002f;
	
	float shake_decay;// = 0.002f;
	float shake_intensity;// = .3f;
	
	public float shakeDuration = 4.0f;
	//float shakeDelay = 10.0f;
	
	public bool isRunning = false;
    public bool CameraShakeRunning { get { return isRunning; } }

	public delegate void FinishShakingDelegate();
	
	public FinishShakingDelegate FinishShaking;
	
	public static CameraShake Instance = null;
	
	private Vector3 lastEulerAngles;
	private Vector3 lastPosition;
	
	private bool updateOnce = true;
	
	void Awake(){
		Instance = this;
	}
	
	void Start()
	{
		myTransform = transform;
	}
	
	public void Launch( float duration, float intensity )
	{
		shakeIntensity = intensity * 0.01f;
		shakeDuration = duration;
		
		if (updateOnce == false){
			updateOnce = true;
			lastPosition = lastPosition*-1;
			lastEulerAngles = lastEulerAngles*-1;		
	        myTransform.localPosition = transform.localPosition + lastPosition;
			myTransform.localEulerAngles = transform.localEulerAngles + lastEulerAngles;				
		}		
		
		isRunning = true;
		ShakeIt();
	}
	
	public void UpdatePostionAndRotation()
	{
	}

    public void Stop()
    {
        isRunning = false;
    }
	
	void Update()
	{
		if ( !isRunning )
			return;
		
		if ( shakeDuration > 0 )
		{
			shakeDuration -= Time.deltaTime;
			
			if ( shake_intensity <= 0 )
				shake_intensity = shakeIntensity;
		}
		else
		{
			isRunning = false;
			
			if ( FinishShaking != null)
				FinishShaking();
			
			if (updateOnce == false){
				updateOnce = true;
				lastPosition = lastPosition*-1;
				lastEulerAngles = lastEulerAngles*-1;		
		        myTransform.localPosition = transform.localPosition + lastPosition;
				myTransform.localEulerAngles = transform.localEulerAngles + lastEulerAngles;				
			}
			return;
		}
			
	    if(shake_intensity > 0)
		{
			if (updateOnce){
				lastPosition = Random.insideUnitSphere * shake_intensity * 10;
				lastEulerAngles = Random.insideUnitSphere * shake_intensity * 20;
				shake_intensity -= shake_decay;
			}else{
				lastPosition = lastPosition*-1;
				lastEulerAngles = lastEulerAngles*-1;
			}
			
			updateOnce = !updateOnce;
			
	        myTransform.localPosition = transform.localPosition + lastPosition;
			myTransform.localEulerAngles = transform.localEulerAngles + lastEulerAngles;
	    }
	}
	 
	void ShakeIt()
	{
	    shake_intensity = shakeIntensity;
	    shake_decay = shakeDecay;
	}
	
}
