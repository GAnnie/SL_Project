using UnityEngine;
using System.Collections;

public class BattleShadow : MonoBehaviourBase
{
	private Transform followTarget;
	private Transform myTransform;
	
	public void Init( Transform target )
	{
		myTransform = transform;
		myTransform.position = new Vector3( 0, 0.1f, 0 );
		myTransform.eulerAngles = new Vector3( 0.0f, 0, 0 );
		
		followTarget = target;
		Update();
	}
	
	void Awake()
	{
		
	}
	
	void Update()
	{ 
		if ( myTransform )
			myTransform.position = new Vector3( followTarget.position.x, 0.1f ,followTarget.position.z );
	}
}
