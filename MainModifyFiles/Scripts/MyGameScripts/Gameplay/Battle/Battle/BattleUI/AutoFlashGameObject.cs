using UnityEngine;
using System.Collections;

public class AutoFlashGameObject : MonoBehaviour
{
	public float flashDelay = 0f;
	public float flashSpeed = 1f;
	
	private Transform _transFrom;
	
	// Use this for initialization
	void Start ()
	{
		_transFrom = this.transform;
		InvokeRepeating ("OnFlash", flashDelay,flashSpeed);
	}

	void OnFlash()
	{
		this.gameObject.SetActive (!this.gameObject.activeSelf);
	}
}

