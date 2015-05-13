
using UnityEngine;
using System.Collections;

public class FXWindmill : MonoBehaviour
{
	public float _RotSpeed = 30.0f;
	float _Ang = 0;

	void Update ()
	{
		float timeDelta = Time.deltaTime;
		
		transform.localRotation = Quaternion.Euler (90, _Ang, 0);
		
		_Ang += timeDelta * _RotSpeed;
	}
}
