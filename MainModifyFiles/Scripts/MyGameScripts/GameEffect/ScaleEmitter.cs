using UnityEngine;
using System.Collections;

public class ScaleEmitter : MonoBehaviour {

    public Transform[] trans;
	// Use this for initialization
	void Start () {
	
	}

    void P_ScaleEmitter(float per)
    {
        this.gameObject.transform.localScale *= per;
        foreach (Transform tran in trans)
        {
            if( tran != null)
            {
                tran.particleEmitter.minSize *= per;
                tran.particleEmitter.maxSize *= per;
            }
        }
    }
}
