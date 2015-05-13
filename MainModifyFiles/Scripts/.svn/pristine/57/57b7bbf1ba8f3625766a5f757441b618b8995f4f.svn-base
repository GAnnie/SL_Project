using UnityEngine;
using System.Collections;

public class EmitController : MonoBehaviour 
{
	public float _timeToShot = 0;
    public float _timeToStopEmit = 1;
	
	bool _shot = false;
	float _time = 0;
	
	ParticleEmitter _Emitter = null;
    MeshEmitter _MeshEmitter = null;
    MeshEmitterFacing _MeshEmitterFacing = null;

    //public bool m_TurnOn = true;

    void Emit(bool bEmit)
    {
        if (null != _Emitter) { _Emitter.emit = bEmit; return; }
        if (null != _MeshEmitter) { _MeshEmitter.emit = bEmit; return; }
        if (null != _MeshEmitterFacing) { _MeshEmitterFacing.emit = bEmit; return; }
    }

    bool isEmit()
    {
        if (null != _Emitter) { return _Emitter.emit; }
        if (null != _MeshEmitter) { return _MeshEmitter.emit; }
        if (null != _MeshEmitterFacing) { return _MeshEmitterFacing.emit; }

        return false;
    }
	
	void Awake()
	{
		doReset();
	}

	public void doReset () 
	{
		_Emitter = GetComponent<ParticleEmitter>();
        if (null == _Emitter) _MeshEmitter = GetComponent<MeshEmitter>();
        if (null == _Emitter && null == _MeshEmitter)
        {
            _MeshEmitterFacing = GetComponent<MeshEmitterFacing>();
        }
		
		_shot = false;
		_time = 0;

        Emit(false);
	}

    public void Stop()
    {
        Emit(false);
    }

	void Update () 
	{
		if (null == _Emitter && null == _MeshEmitter && null == _MeshEmitterFacing) return;
		
		_time += Time.deltaTime;

        if (_timeToStopEmit == -1.0f)
        {
            //never stop
        }
        else
        {
            if (_shot && isEmit() && _time > (_timeToShot + _timeToStopEmit))
            {
                Emit(false);
            }
        }

		if (!_shot && _time > _timeToShot)
		{
			_shot = true;
            Emit(true);
		}				
	}
}
