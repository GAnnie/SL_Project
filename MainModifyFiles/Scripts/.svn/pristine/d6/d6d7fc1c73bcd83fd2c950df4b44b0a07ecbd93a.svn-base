using UnityEngine;
using System.Collections;

public class DestroyAndCreateByUser : MonoBehaviour {

    public GameObject go;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DestoryAndCreate()
    {
        Instantiate(go, gameObject.transform.position, gameObject.transform.rotation);
        gameObject.transform.parent = null;
        Destroy(gameObject);
    }
}
