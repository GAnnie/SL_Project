
//#if UNITY_DEBUG
using UnityEngine;
using System.Collections;

public class GameResourcePoolDisplay : MonoBehaviour
{
	
	private bool []showFlag =null;
	GUIStyle style = null;
	// Use this for initialization
	void Start () 
	{
	 	style = new GUIStyle();
	    style.normal.textColor = Color.red;
		style.fontSize = 13;
		
		showFlag = new bool[ (int)ResourcePoolManager.PoolType.PoolTypeNumber];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.color = Color.red;
		GUILayout.BeginHorizontal();
		{
			DisplayReourcePool( ResourcePoolManager.PoolType.DONT_DESTROY );
			DisplayReourcePool( ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE );
			DisplayReourcePool( ResourcePoolManager.PoolType.DESTROY_CHANGE_SCENE );
		}
		GUILayout.EndHorizontal();
		
		GUI.color = Color.white;
	}
	
	
	private void DisplayReourcePool( ResourcePoolManager.PoolType poolType )
	{
		string displayMessage = ResourcePoolManager.Instance.GetSpawnedSituation( poolType );
		GUILayout.BeginVertical();
		{
			GUILayout.Label( poolType.ToString());
			showFlag[(int)poolType] = GUILayout.Toggle( showFlag[(int)poolType], "显示");
			if( showFlag[(int)poolType])
			{
				GUILayout.TextArea( displayMessage, GUILayout.Width(200));
			}
		}
		GUILayout.EndVertical();
		GUILayout.Space( 100 );
	}
	
}

//#endif