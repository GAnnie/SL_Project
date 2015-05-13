using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class par_set_change : MonoBehaviour {
	public List<GameObject> gos = new List<GameObject>();
    //int id = -1;  //GET WARNING
	// Use this for initialization
	void Start () {
		if( QualitySettings.GetQualityLevel() != 0 )
			change();
	}
	
	
	void change()
	{
		
		foreach (GameObject go in gos)
		{
            //go.active = true;
            go.SetActive(true);
		}
		
	}
}
