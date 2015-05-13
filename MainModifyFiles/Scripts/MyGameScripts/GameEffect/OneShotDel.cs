using UnityEngine;
using System.Collections;

public class OneShotDel : MonoBehaviour {

	ParticleEmitter mEmitter;
	public float mStartShotClick = 0.0f;

	// Use this for initialization
	void Start () {
		mEmitter = GetComponent<ParticleEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (mStartShotClick > 0)
			mEmitter.emit = true;
		else
			mEmitter.emit = false;
	
	}
}
