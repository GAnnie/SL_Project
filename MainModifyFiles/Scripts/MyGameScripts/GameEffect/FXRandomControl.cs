
using UnityEngine;
using System.Collections;

public class FXRandomControl : MonoBehaviour
{
    public bool _Enable = true;
    public float _EnableTimeMin = 10.0f;
    public float _EnableTimeMax = 30.0f;
    public float _DisableTimeMin = 10.0f;
    public float _DisableTimeMax = 30.0f;

    private float _RandomEnableTime;//随机开始时间，可以让特效的起始错开
    private float _RandomDisableTime;//随机停止时间
    ParticleEmitter emitter = null;

    private int runningMode = 0;
    void Start() 
    {
        _RandomDisableTime = Random.Range(_DisableTimeMin, _DisableTimeMax);
        emitter = particleEmitter;
        if (emitter != null)
        {
            emitter.emit = false;
        }
    }

	void Update ()
	{
        if (emitter == null)
            return;

        if (runningMode == 0)
        {
            _RandomDisableTime -= Time.deltaTime;
            if (_RandomDisableTime < 0.0f)
            {
                //start
                emitter.emit = true;
                _RandomEnableTime = Random.Range(_EnableTimeMin, _EnableTimeMax);
                runningMode = 1;
            }  
        }
        else
        {
            _RandomEnableTime -= Time.deltaTime;
            if (_RandomEnableTime < 0.0f)
            {
                //start
                emitter.emit = false;
                _RandomDisableTime = Random.Range(_DisableTimeMin, _DisableTimeMax);
                runningMode = 0;
            }  
        }

	}
}
