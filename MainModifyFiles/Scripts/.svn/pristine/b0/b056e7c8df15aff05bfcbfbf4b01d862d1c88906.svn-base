using UnityEngine;
using System.Collections;

public class ParTool : MonoBehaviour {

    public GameObject[] goList;

    public Transform from;
    public Transform to;

    float oldLength = 1.0f;
    void Start()
    {
    }

    public void setLen(float length)
    {
        foreach (GameObject go in goList)
        {
            go.GetComponent<ParticleRenderer>().lengthScale = length;
        }
    }

    void Update()
    {
        if (from != null && to != null)
        {
            float length = Vector3.Distance(from.position, to.position);
            Debug.Log("length:" + length);
            foreach (GameObject go in goList)
            {
                if (go != null)
                {
                    go.GetComponent<ParticleRenderer>().lengthScale /= oldLength;
                    go.GetComponent<ParticleRenderer>().lengthScale *= length;
                }
            }
            oldLength = length;

            this.gameObject.transform.position = from.position;
            Vector3 dir = to.position - from.position;
            dir.Normalize();
            this.gameObject.transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
