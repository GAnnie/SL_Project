using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FaceToCamera : MonoBehaviour
{
	public Camera targetCamera;
	public bool reverseFace = false;

	private Transform cachedTransform;
	private Transform cachedCameraTrans;

	void  Awake ()
	{
		cachedTransform = this.transform;
		// if no camera referenced, grab the main camera
		if (!targetCamera)
			targetCamera = Camera.main;

		cachedCameraTrans = targetCamera.transform;
	}
	
	void  Update ()
	{
		// rotates the object relative to the camera
		Vector3 targetPos = cachedTransform.position + cachedCameraTrans.rotation * (reverseFace ? Vector3.back : Vector3.forward) ;
		Vector3 targetOrientation = cachedCameraTrans.rotation * Vector3.up;
		cachedTransform.LookAt (targetPos, targetOrientation);
	}
}