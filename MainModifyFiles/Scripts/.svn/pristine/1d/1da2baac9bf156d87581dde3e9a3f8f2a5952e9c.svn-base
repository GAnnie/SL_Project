using UnityEngine;
using System.Collections;

public class AutoRotationY : MonoBehaviour
{
	public float rotationSpeed = 1;

	private Transform _transFrom;

	// Use this for initialization
	void Start ()
	{
		_transFrom = this.transform;
	}

	// Update is called once per frame
	void Update ()
	{
		_transFrom.localEulerAngles = new Vector3( _transFrom.localEulerAngles.x, _transFrom.localEulerAngles.y + Time.timeScale*rotationSpeed, _transFrom.localEulerAngles.z );
	}
}

