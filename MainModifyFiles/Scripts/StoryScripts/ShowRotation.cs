using UnityEngine;
using System.Collections;

public class ShowRotation : MonoBehaviour {

	

	private Rect windowRect = new Rect(10, 100, 120, 50);


	
	void OnGUI()    
	{

		if (ShowRotationModel.Instance.IsOPen ()) {
			windowRect = GUI.Window(10,windowRect,WindowDraw,"朝向");		
		}
		
	}

	float y ;

	void Update(){
		y= WorldManager.Instance.GetHeroView().transform.rotation.eulerAngles.y;
	}

	void WindowDraw(int windowID){
		
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.green;


		GUILayout.Label (string.Format("朝向:{0}",y) ,style);

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}



}
