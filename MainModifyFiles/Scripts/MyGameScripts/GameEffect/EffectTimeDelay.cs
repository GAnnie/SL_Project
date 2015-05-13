using UnityEngine;
using System.Collections;

public class EffectTimeDelay : MonoBehaviour 
{
	public enum EffectTimeDelayMode
	{
		None,
		GameObjectOn,
		GameObjectOff,
		EmitterOn,
		EmitterOff,
		AnimationStart,
		AnimationStop,
	}
	public EffectTimeDelayMode ModeSet;
	public float timeDelay;
	public Transform trans;
	// Use this for initialization
	void Start () 	{
		if (trans == null || ModeSet == EffectTimeDelayMode.None)
			return;
		Invoke("OnEffectEventFire", timeDelay);
	}
	
	void OnEffectEventFire()
	{
		switch (ModeSet)
		{
			case EffectTimeDelayMode.GameObjectOn:
				trans.gameObject.SetActiveRecursively(true);
				break;
			case EffectTimeDelayMode.GameObjectOff:
				trans.gameObject.SetActiveRecursively(false);
				break;
			case EffectTimeDelayMode.EmitterOn:
				if (trans.particleEmitter != null)
					trans.particleEmitter.emit = true;
				break;
			case EffectTimeDelayMode.EmitterOff:
				if (trans.particleEmitter != null)
					trans.particleEmitter.emit = false;
				break;
			case EffectTimeDelayMode.AnimationStart:
				if (trans.animation != null)
					trans.animation.Play();
				break;
			case EffectTimeDelayMode.AnimationStop:
				if (trans.animation != null)
					trans.animation.Stop();
				break;
			default:
				break;
		}
	}

}
