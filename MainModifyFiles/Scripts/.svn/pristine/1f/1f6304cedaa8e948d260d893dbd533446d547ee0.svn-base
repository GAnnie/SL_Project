using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPosition : MonoBehaviour {

	public GameObject petPrefab = null;

	private string[] a_positions = new string[14];

	private string[] b_positions = new string[14];

	public bool enable = false;
	public bool enableCamera = false;

	void Awake()
	{
//		a_positions[0] = "-4.22:-0.20";
//		a_positions[1] = "-4.16:1.30";
//		a_positions[2] = "-4.26:-1.65";
//		a_positions[3] = "-4.06:2.75";
//		a_positions[4] = "-4.31:-3.19";
//		a_positions[5] = "-2.52:-0.41";
//		a_positions[6] = "-2.44:1.05";
//		a_positions[7] = "-2.58:-1.86";
//		a_positions[8] = "-2.40:2.47";
//		a_positions[9] = "-2.61:-3.34";
//		a_positions[10] = "-5.81:-0.05";
//		a_positions[11] = "-5.68:1.52";
//		a_positions[12] = "-5.94:-1.51";
//		a_positions[13] = "-7.26:0.03";
//		
//		b_positions[0] = "3.93:-1.27";
//		b_positions[1] = "3.90:-2.78";
//		b_positions[2] = "3.93:0.20";
//		b_positions[3] = "3.70:-4.23";
//		b_positions[4] = "4.03:1.71";
//		b_positions[5] = "2.38:-1.12";
//		b_positions[6] = "2.38:-2.64";
//		b_positions[7] = "2.41:0.34";
//		b_positions[8] = "2.31:-4.16";
//		b_positions[9] = "2.53:1.90";
//		b_positions[10] = "5.39:-1.41";
//		b_positions[11] = "5.33:-2.91";
//		b_positions[12] = "5.32:0.07";
//		b_positions[13] = "6.75:-1.51";

		a_positions[0] = "-3.41:-2.43";
		a_positions[1] = "-3.21:-0.84";
		a_positions[2] = "-3.59:-4.04";
		a_positions[3] = "-3.13:0.96";
		a_positions[4] = "-3.83:-5.98";
		a_positions[5] = "-1.58:-3.21";
		a_positions[6] = "-1.32:-1.59";
		a_positions[7] = "-1.82:-4.60";
		a_positions[8] = "-1.14:-0.10";
		a_positions[9] = "-2.06:-6.28";
		a_positions[10] = "-4.71:-2.16";
		a_positions[11] = "-4.51:-0.60";
		a_positions[12] = "-4.89:-3.74";
		a_positions[13] = "-5.96:-1.88";
		
		b_positions[0] = "3.72:-1.95";
		b_positions[1] = "3.48:-3.53";
		b_positions[2] = "4.04:-0.32";
		b_positions[3] = "3.20:-5.09";
		b_positions[4] = "4.36:1.31";
		b_positions[5] = "2.03:-1.32";
		b_positions[6] = "1.83:-2.88";
		b_positions[7] = "2.39:0.37";
		b_positions[8] = "1.58:-4.43";
		b_positions[9] = "2.61:1.94";
		b_positions[10] = "5.17:-2.38";
		b_positions[11] = "4.97:-3.98";
		b_positions[12] = "5.55:-0.68";
		b_positions[13] = "6.53:-2.78";
	}

	void Start () 
	{
	}

	void Update()
	{
		if (enable)
		{
			enable = false;

			if (petPrefab == null)
			{
				Debug.LogError("petPrefab is null");
			}
			else
			{
				if (_playerGroupGO == null)
				{
					_playerGroupGO = GameObject.Find("PlayerGroupGO");
					if (_playerGroupGO == null)
					{
						_playerGroupGO = new GameObject("PlayerGroupGO");
					}
				}
				
				GameObjectExt.RemoveChildren(_playerGroupGO);

				petPrefab.SetActive(false);

				Create(true);
				Create(false);
			}
		}

		if (enableCamera)
		{
			enableCamera = false;

			Camera camera = Camera.main;
			if (camera)
			{
				camera.transform.localPosition = new Vector3 (-13.65f,16.50f,22.00f);
				camera.transform.localEulerAngles = new Vector3 (30.00f,150.00f,0.00f);
				camera.orthographicSize = 4.69f;
				camera.fieldOfView = 20;
			}
			else
			{
				Debug.LogError("camera is null");
			}
		}
	}

	void OnGUI()    
	{
		if (GUILayout.Button("ExportPosition"))
		{
			if (_playerGroupGO != null)
			{
				List<string> list = new List<string>();
				list.AddRange(ExportPosition2(true));
				list.Add("");
				list.AddRange(ExportPosition2(false));
				
				string info = string.Join("\n", list.ToArray());
				Debug.Log(info);
				GameDebuger.clipBoard = info;
			}
			else
			{
				Debug.LogError("ExportPosition is null");
			}
		}

		if (GUILayout.Button("ExportCamera"))
		{
			Camera camera = Camera.main;
			if (camera)
			{
				List<string> list = new List<string>();
				list.Add(string.Format("camera.transform.localPosition = new Vector3 ({0}f,{1}f,{2}f);", camera.transform.localPosition.x.ToString("F2"), camera.transform.localPosition.y.ToString("F2"), camera.transform.localPosition.z.ToString("F2")));
				list.Add(string.Format("camera.transform.localEulerAngles = new Vector3 ({0}f,{1}f,{2}f);", camera.transform.localEulerAngles.x.ToString("F2"), camera.transform.localEulerAngles.y.ToString("F2"), camera.transform.localEulerAngles.z.ToString("F2")));
				list.Add(string.Format("camera.fieldOfView = {0};", camera.fieldOfView));

				string info = string.Join("\n", list.ToArray());
				Debug.Log(info);
				GameDebuger.clipBoard = info;
			}
			else
			{
				Debug.LogError("ExportCamera is null");
			}
		}
	}

	private string[] ExportPosition2(bool player)
	{
		string prefix = player?"A":"B";
		string[] list = new string[14];
		for (int i = 0; i< 14; i++)
		{
			GameObject go = GameObject.Find(prefix+(i+1));
			if (go != null)
			{
				list[i] = string.Format("{0}_positions[{1}] = \"{2}:{3}\";", prefix.ToLower(), i, go.transform.position.x.ToString("F2"), go.transform.position.z.ToString("F2"));
			}
		}
		return list;
	}

	private GameObject _playerGroupGO = null;

	private void Create(bool player)
	{
		//GameObject[] list = new GameObject[14];
		for (int i = 0; i< 14; i++)
		{
			GameObject petObject = Instantiate( petPrefab, Vector3.one, Quaternion.identity ) as GameObject;
			if (petObject != null)
			{
				petObject.SetActive(true);

				petObject.transform.parent = _playerGroupGO.transform;

				string[] positions = player?a_positions:b_positions;

				petObject.name = player?("A"+(i+1)):("B"+(i+1));
				string posotion = positions[i];
				string[] vec = posotion.Split(':');
				float x = float.Parse(vec[0]);
				float z = float.Parse(vec[1]);

				float rotationY = player?100f:-80f;

				petObject.transform.localPosition = new Vector3(x, 0, z);
				petObject.transform.localEulerAngles = new Vector3(0,rotationY,0);

				//list[i] = petObject;
			}
		}
	}
}
