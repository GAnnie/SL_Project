using UnityEngine;
using System.Collections;

public class DelDelayCC : MonoBehaviour {
	public float delayTime = 10.0f;
	private float curTime = 0.0f;
	private bool startGo = false;
	// Use this for initialization
	void Start () {
	
	}
	
	public void go()
	{
		startGo = true;
	}
	// Update is called once per frame
	void Update () {
		if( startGo )
		{
			curTime += Time.deltaTime;
			if( curTime >= delayTime )
				Destroy(this.gameObject);
		}
	}
}
