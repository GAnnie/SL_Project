using UnityEngine;
using System.Collections;

public class CameraAnimInvTime : MonoBehaviour {

    public int id = 0;
    public float maxTime = 2.0f;
    public float invTime = 1.0f;

    private float curTime = 0.0f;
    private float allTime = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (allTime >= maxTime)
            return;

        curTime += Time.deltaTime;
        if (curTime >= invTime)
        {
            Camera.main.GetComponent<CameraAnim>().playAnim(id);
            curTime -= invTime;
        }



        allTime += Time.deltaTime;
        
	}
}
