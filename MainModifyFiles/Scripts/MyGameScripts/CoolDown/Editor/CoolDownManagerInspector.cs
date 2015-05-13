using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CoolDownManager))]
public class CoolDownManagerInspector : Editor
{
	CoolDownManager mCoolDownManager;
	
	private Vector2 scrollPosition = Vector3.zero;
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.labelWidth = 80f;
		base.OnInspectorGUI();
		mCoolDownManager = target as CoolDownManager;
		 if (mCoolDownManager == null)
            return;
		
		var coolDownDic = mCoolDownManager.DictOnlyOnInspector;
		
		EditorGUILayout.Space();
		GUILayout.BeginVertical (  );
		{
			scrollPosition = GUILayout.BeginScrollView ( scrollPosition , true , true );
			{
				GUILayout.Space ( 5f );


				//字典在foreach时不能对字典中的元素做CRUD操作,这可能会导致死循环
				var keyList = new List<string> (coolDownDic.Keys);
				for (int i=0; i<keyList.Count; ++i) {
					DrawCoolDownTask(keyList[i],coolDownDic[keyList[i]]);
				}

			}
			GUILayout.EndScrollView ( );
		}
		GUILayout.EndVertical ( );
		
		this.Repaint ( );
	}
	
	void DrawCoolDownTask ( string name , CoolDownManager.Task curTask )
	{
		GUILayout.BeginVertical ( );
		{
			GUILayout.Label ( "==========================" );
			GUILayout.Label ( "name:" + name );
			GUILayout.Label ( "remain:" + curTask.remainTime ); 
			GUILayout.BeginHorizontal();
			if ( GUILayout.Button ( "Finish" ) ) FinishThis ( curTask );
			if ( GUILayout.Button ( "Cancel" ) ) CancelThis ( name );
			GUILayout.EndHorizontal();
			GUILayout.Label ( "==========================" );
			
		}
		GUILayout.EndVertical ( );
	}
	
	
	void FinishThis ( CoolDownManager.Task curTask )
	{
		if ( curTask == null ) return;
		curTask.remainTime = 0;
	}
	
	void CancelThis ( string name )
	{
		if ( mCoolDownManager == null ) return;
		mCoolDownManager.CancelCoolDown ( name );
	}
}