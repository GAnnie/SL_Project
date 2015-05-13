using UnityEngine;
using System.Collections;

class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 1.0F;
	 public bool detachChildren = false;
	// Use this for initialization
	void Awake()
	 {
		 Invoke ("DestroyNow", timeOut);	
	}
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void DestroyNow()
	{
		//if(detachChildren)
		//	transform.DetachChildren ();
		
		//DestroyObject (gameObject);
		gameObject.SetActive(false);
	}
	
	public void Reset()
	{
		gameObject.SetActive(true);	
	}
	
}
