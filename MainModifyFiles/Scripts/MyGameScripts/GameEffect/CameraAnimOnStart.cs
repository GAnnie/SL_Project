using UnityEngine;
using System.Collections;

public class CameraAnimOnStart : MonoBehaviour {
    public int id = 0;
	// Use this for initialization
	void Start () {
        Camera.main.GetComponent<CameraAnim>().playAnim(id);
	}
	
	
}
