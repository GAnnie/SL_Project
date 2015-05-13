using UnityEngine;
using System.Collections;

public class AdjustCameraPosition : MonoBehaviour {

	public static bool BattleMode = false;
	public static bool NeedUpdate = false;

	private static float BaseScreenScale = 1024f/768f;
	private float _screenScale = 1f;
	private float AdjustScale = 0.97f;

	// Use this for initialization
	void Start () {
		ChangeCMode (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (NeedUpdate)
		{
			NeedUpdate = false;

			Camera[] cameras = this.GetComponentsInChildren<Camera>();
			foreach (Camera camera in cameras)
			{
				if (camera.gameObject.name == "BattleCamera" || (BattleMode && BattleManager.NeedBattleMap))
				{
					float newScreenScale = (float)Screen.width/(float)Screen.height;
					_screenScale = newScreenScale/BaseScreenScale;
					_screenScale *= AdjustScale;

					camera.transform.localPosition = CameraConst.BattleCameraLocalPosition;
					camera.transform.localEulerAngles = CameraConst.BattleCameraLocalEulerAngles;
					camera.fieldOfView = CameraConst.BattleCameraFieldOfView;
					camera.orthographicSize = CameraConst.BattleCameraOrthographicSize / _screenScale;
				}
				else
				{
					camera.transform.localPosition = CameraConst.WorldCameraLocalPosition;
					camera.transform.localEulerAngles = CameraConst.WorldCameraLocalEulerAngles;
					camera.fieldOfView = CameraConst.WorldCameraFieldOfView;
				}
			}
		}
	}

	public static void ChangeCMode(bool battleMode)
	{
		BattleMode = battleMode;
		NeedUpdate = true;
	}
}
