using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BipPoint : MonoBehaviour {

    public List<Transform> wuqidian;
    public Transform wingPoint;
    public Transform beattackedPoint;
    public Transform riderPoint;

    public Transform wingBindTrans;
    public Transform riderBindTrans;
    public Transform headPoint;

    public List<Transform> strangePoints = new List<Transform>();

    void Update()
    {
        if (wingBindTrans != null)
        {
            Vector3 offpos = wingPoint.transform.position - wingBindTrans.position;
            this.gameObject.transform.position -= offpos;
            this.gameObject.transform.rotation = wingBindTrans.rotation;
        }

        if (riderBindTrans != null)
        {
            Vector3 offpos = riderPoint.transform.position - riderBindTrans.position;
            this.gameObject.transform.position -= offpos;
            this.gameObject.transform.rotation = riderBindTrans.rotation;
        }
    }
}
