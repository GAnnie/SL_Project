using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRenderTrans : MonoBehaviour {

    public List<LineRenderer> lrList = new List<LineRenderer>();

    public Transform p1;
    public Transform p2;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if( p1 != null && p2 != null )
        {
            this.gameObject.transform.position = p1.transform.position;
            foreach( LineRenderer lr in lrList )
            {
                lr.SetPosition(1, p2.transform.position-p1.transform.position);
            }
        }
    }

    void OnDestroy()
    {
        if (Debug.isDebugBuild)Debug.Log("################ on destroy");
    }
}
