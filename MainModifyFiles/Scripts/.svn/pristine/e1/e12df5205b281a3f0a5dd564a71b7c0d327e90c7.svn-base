using UnityEngine;
using System.Collections;

public class GenerateShadowMotion : MonoBehaviour
{
	public GameObject go;
	public float generateInterval = 0.05f;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine( Generate() );
	
		iTween.MoveTo( gameObject, iTween.Hash( "x", -18f, "y", 0.0f, "z", 0, "looptype", "pingpong", "time", 0.5f, "easetype", "linear" ) );
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	IEnumerator Generate()
	{
		while ( true )
		{
			Instantiate( go, transform.position, Quaternion.identity );
			
			
			yield return new WaitForSeconds( generateInterval );
		}
		
        //yield return null;   //GET WARNING
	}
}
