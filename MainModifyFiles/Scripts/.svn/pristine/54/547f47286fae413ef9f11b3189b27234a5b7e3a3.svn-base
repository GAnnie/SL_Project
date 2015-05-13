using UnityEngine;
using System.Collections;

public class ShowRotationModel : MonoBehaviour {



	private static readonly ShowRotationModel instance = new ShowRotationModel();
	
	public static ShowRotationModel Instance
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

	public void SetGo( GameObject _go){
		go = _go;
	}

	public bool AddNew(){
		return go == null ? true : false;
	}

}
