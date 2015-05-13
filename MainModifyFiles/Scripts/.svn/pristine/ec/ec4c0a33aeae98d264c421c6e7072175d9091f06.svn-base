using UnityEngine;
using System.Collections;

public class StopEmitterDelayDel : MonoBehaviour 
{
	public float delDelay = 2.0f;
	public Transform[] trans;
	// Use this for initialization
	void Start () 	{
		
	}
    void P_StopEmitter()
    {
        foreach (Transform tran in trans)
        {
            tran.particleEmitter.emit = false;
        }
        Invoke("delSelf", delDelay);
    }

    void delSelf()
    {
        Destroy(this.gameObject);
    }

}
