using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotAutoWalk : MonoBehaviour {


 	public	NavMeshAgent navAgent;
	private float timer;
	private float frames;
	public bool show = false;

	private int coolDown;
	private List <Vector3 > mapPointList = new List<Vector3>();

	
	void Start(){
		navAgent = this.gameObject.GetComponent<NavMeshAgent> ();
//		StartCoroutine (AutoWalk ());
		coolDown = Random.Range (2, 10);
//		InvokeRepeating ("AutoWalk", coolDown, 5);
		mapPointList = RobotInfo.Instance.GetMapPointList ();
	}



	void Update(){
		frames++;

		timer += Time.deltaTime;
		if (timer > coolDown) {
			AutoWalk();
			timer = 0;
		}
	}

	public void CoolDown(int t){
		coolDown = t;
	}


	void OnBecameVisible() {
		show = true;
	}

	void OnBecameInvisible() {
		show = false;
	}
	
	//	IEnumerator AutoWalk(){
//
//		while (true) {
//
//			nav.SetDestination (Vector3.zero);
//
//			yield return new WaitForSeconds( 2f );
//		}
//	}

	public void AutoWalk(){
		navAgent.SetDestination (mapPointList[Random.Range(0,mapPointList.Count)]);
	}
	
}