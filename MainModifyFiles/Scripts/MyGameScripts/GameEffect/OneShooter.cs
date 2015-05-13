using UnityEngine;
using System.Collections;

public class OneShooter : MonoBehaviour 
{
    public float _minFreq = 0.5f;
    public float _maxFreq = 1.0f;

    ParticleEmitter _Emitter = null;
    float _time = 0;

    void Awake()
    {
        _Emitter = GetComponent<ParticleEmitter>();

        if (_Emitter != null)
        {
            _Emitter.emit = false;
        }

        _time = Mathf.Lerp(_minFreq, _maxFreq, Random.value);
    }

    void Update()
    {
        if (null == _Emitter) return;

        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = Mathf.Lerp(_minFreq, _maxFreq, Random.value);

            float cnt = Mathf.Lerp(_Emitter.minEmission, _Emitter.maxEmission, Random.value);
            _Emitter.Emit((int)cnt);

            _Emitter.emit = false;
        }
    }
}
