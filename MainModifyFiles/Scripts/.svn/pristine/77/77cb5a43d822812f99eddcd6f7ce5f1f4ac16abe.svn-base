using UnityEngine;
using System.Collections;

public class PlayerRidingPosControl : MonoBehaviour
{
	
	//public float Ydistanc = 0f;
	public float Angle;
	public Transform heroTrans;
	public Transform petRideTrans;
	public bool isControl = false;
	
	public float LimitAngle = 10;
	
	public void Setup (Transform hero, Transform mountRiding)
	{
		heroTrans = hero;
		petRideTrans = mountRiding;
		
		isControl = true;
	}
	
	public void Remove ()
	{
		isControl = false;
		heroTrans = null;
		petRideTrans = null;
		//Destroy (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isControl) {
			
			if(heroTrans && petRideTrans)
			{
				heroTrans.position = petRideTrans.position;
				
				//控制角度
				Angle = Vector3.Angle (Vector3.up, petRideTrans.up);//Quaternion.Angle (originalRotate, petRideTrans.rotation);
			
				if (Angle < LimitAngle) {
				
					heroTrans.localRotation = Quaternion.Euler (petRideTrans.rotation.eulerAngles.x, 0, 0);
				} else {
					if (LimitAngle == 0) {
						heroTrans.localRotation = Quaternion.identity;
					}
				}
			}
			
		}
	}
}
