using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotInfo : MonoBehaviour {
	
	private static readonly RobotInfo instance = new RobotInfo();

	public static RobotInfo Instance
	{
		get
		{
			return instance;
		}
	}


	public bool Open = false;
	public GameObject go = null;
	public void SetOpen(bool b){
		Open = b;
	}

	public bool IsOPen(){
		return Open;
	}

	public bool AddNew(){
		return go == null ? true : false;
	}
	public void SetGo( GameObject _go){
		go = _go;
	}

	public  List< Vector3 > mapPointList = new List<Vector3>();

	public void SetMapPointList(List< Vector3 > list){
		mapPointList = list;
	}

	public  List< Vector3 > GetMapPointList(){
		return mapPointList;
	}

	List<RobotAutoWalk> robotList = new List<RobotAutoWalk>();

	public void SetRobotList(RobotAutoWalk list){
		robotList.Add (list);
	}

	public void SetCookDown(int t){
		for (int i = 0; i < robotList.Count; i++) {
			robotList[i].CoolDown(t);		
		}
	}
	public void DelRobotList(){
		robotList.Clear ();
	}

	List<GameObject> objList = new List<GameObject>();
	public void ShowObjList(GameObject go){
		objList.Add (go);
	}
	public List<GameObject> GetShowObjList(){
		return objList;
	}
	public void DelShowObjList(){
		objList.Clear ();
	}


}
