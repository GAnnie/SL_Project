// ====================================================================================================================
// Simple movement of the camera around the scene
// Created by Leslie Young
// http://www.plyoung.com/ or http://plyoung.wordpress.com/
// ====================================================================================================================
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Transform target;			// target to follow (cam is fixed to following this around till it is NULL)
	public bool followTarget = false;	// follow the target? (only if target is not NULL)
	private Transform mTrans;

	public bool isSmooth = true;
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		mTrans = this.transform;
		if (target && followTarget) mTrans.position = target.position;
	}

	void LateUpdate()
	{
		if (target && followTarget)
		{
			if (isSmooth){
				mTrans.position = Vector3.SmoothDamp(mTrans.position,target.position,ref velocity,smoothTime);
			}else{
				SyncTargetPos();
			}
		}
	}
	
	public void SyncTargetPos()
	{
		if (target != null){
			mTrans.position = target.position;
		}
	}

	public void Follow(Transform t)
	{
		target = t;
		followTarget = true;
	}
}
