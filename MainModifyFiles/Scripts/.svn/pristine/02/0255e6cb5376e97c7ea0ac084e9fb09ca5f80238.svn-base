using UnityEngine;
using System.Collections;

public class DestroyAndCreate : MonoBehaviour {

    public float destroyTime = 5.0f;
    public GameObject go;
	Transform myTransform = null;
	// Use this for initialization
	void Start () {
		myTransform = transform;
        Invoke("dac", destroyTime);
	}

    void dac()
    {
        Instantiate(go, myTransform.position, myTransform.rotation);
        Destroy(gameObject);
    }
}
