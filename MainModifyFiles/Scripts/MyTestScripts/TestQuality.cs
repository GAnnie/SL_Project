using UnityEngine;
using System.Collections;

public class TestQuality : MonoBehaviour {

	void OnGUI()
	{
		if (GUILayout.Button ("Fastest"))
		{
			QualitySettings.SetQualityLevel(0);
		}

		GUILayout.Space (20);


		if (GUILayout.Button ("Fast"))
		{
			QualitySettings.SetQualityLevel(1);
		}

		GUILayout.Space (20);

		if (GUILayout.Button ("Simple"))
		{
			QualitySettings.SetQualityLevel(2);
		}

		GUILayout.Space (20);

		if (GUILayout.Button ("Good"))
		{
			QualitySettings.SetQualityLevel(3);
		}

		GUILayout.Space (20);

		if (GUILayout.Button ("Beautiful"))
		{
			QualitySettings.SetQualityLevel(4);
		}

		GUILayout.Space (20);

		if (GUILayout.Button ("Fantastic"))
		{
			QualitySettings.SetQualityLevel(5);
		}

		GUILayout.Space (20);

		if (GUILayout.Button ("ExitGame")) 
		{
			Application.Quit();
		}
	}
}