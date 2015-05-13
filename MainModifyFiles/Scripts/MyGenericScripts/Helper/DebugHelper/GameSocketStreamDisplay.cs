using UnityEngine;
using System.Collections;
 
public class GameSocketStreamDisplay : MonoBehaviour
{
	
	public Rect startRect; // The rect the window is initially displayed at.
	public bool updateColor = true; // Do you want the color to change if the FPS gets low
	public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
	public  float frequency = 0.5F; // The update frequency of the fps
	public int nbDecimal = 1; // How many decimal do you want to display
 
	private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
	private string send = ""; // The stream KB of send
	private string receive = ""; // The stream KB of receive
	private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.
 
#if UNITY_DEBUG

	void Start()
	{
		startRect = new Rect(0f, Screen.height-50f, 100f, 50f );
	}
 
	void Update()
	{
		send = "Up " + HaConnector.TotalSendBytes/1024+" KB";
		receive = "Down " + HaConnector.TotalReceiveBytes/1024+" KB";
		
		color = XSocket.IsOnLink ? Color.green : Color.red;
	}

	void OnGUI()
	{
		if (AppManager.DebugMode == false){
			return;
		}		
		
		// Copy the default label skin, change the color and the alignement
		if( style == null ){
			style = new GUIStyle( GUI.skin.label );
			style.normal.textColor = Color.white;
			style.alignment = TextAnchor.MiddleCenter;
		}
 
		GUI.color = updateColor ? color : Color.white;
		startRect = GUI.Window(1, startRect, DoMyWindow, "");
	}
 
	void DoMyWindow(int windowID)
	{
		GUI.Label( new Rect(0, 0, startRect.width, startRect.height/2), send, style );
		GUI.Label( new Rect(0, startRect.height/2, startRect.width, startRect.height/2), receive, style );
		if( allowDrag ) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

#endif
}