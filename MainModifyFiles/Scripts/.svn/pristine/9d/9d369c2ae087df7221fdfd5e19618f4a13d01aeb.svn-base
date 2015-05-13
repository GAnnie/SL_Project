// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  CameraController.cs
// Author   : willson
// Created  : 2014/12/2 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {
	
	//oldMouse State X & Y
	private Vector3 _mouseOldPosition ;
	
	//world look at position
	private bool _worldLookAtChange = false;
	
	//main camera obj
	private GameObject cameraObj = null;
	
	//main camera
	private Camera cam = null;
    //private float MouseWheelSensitivity = 10;     //GET WARNING
	
	//camera State
    //private const int STATE_FREE = 0;             //GET WARNING
    //private const int STATE_FIRST_PERSON = 1;     //GET WARNING
    //private int _curState = STATE_FREE;           //GET WARNING

	public CameraMove cameraMove;

	public GameObject heroGO;

    public CameraShake cameraShake = null;

	public Animator cameraAnimator;
	public Animator battleCameraAnimator;
	// Use this for initialization
	void Start () 
	{
	}
	
    //private bool modifyMovePosition=false; //GET WARNING
	// Update is called once per frame
	void Update () 
	{
		if ( ( cameraObj != null ) && ( cam != null ) )
		{
			if (cameraSlerp){
				SlerpCamera();
				spinCamera();				
			}


            if (cameraShake != null && cameraShake.CameraShakeRunning)
            {
                cameraShake.UpdatePostionAndRotation();
            }
		}

		// cameraAnimator

//		if(!string.IsNullOrEmpty(_action) && _checkCameraStop)
//		{
//			AnimatorStateInfo animatorState = cameraAnimator.GetCurrentAnimatorStateInfo(0);
//
//			//帧数延迟
//			if(animatorState.IsName("Stop"))
//			{
//				cameraAnimator.enabled = false;
//				_action = null;
//				_checkCameraStop = false;
//			}
//		}
	}

	public GameObject getCamera()
	{
		return cameraObj;
	}

    public void CleanCamera()
    {
//        camInfo = null;
    }

	private bool _doingFade;
	
	//游戏初始化时，设置MainCamera参数
	public void SetupCamera(GameObject cameObj)
	{
		_doingFade = false;

//		switchCameraConfigInfo = info;
		_lookAtPosition = heroGO.transform.position;
		
		cameraObj = cameObj;
		cam = cameObj.GetComponent<Camera>();
        cameraShake = CameraShake.Instance;
		
		cameraMove.enabled = true;
		cameraMove.Follow(heroGO.transform);
		cameraMove.SyncTargetPos();
	}
	
	public void setCamera( GameObject cameObj , bool enterScene = false)
	{
		_doingFade = false;
		
        StopCameraSlerp();

//		if (camInfo != info){
			cameraObj = cameObj;
			cam = cameObj.GetComponent<Camera>();
            cameraShake = CameraShake.Instance;

//			if ( info != null ){
//				switchCameraConfigInfo = info;
//				switchCameraTriggerInfo = triggerInfo;
//			}

			/*
			if (enterScene){
				if (UIManager.Instance.UIMode == UIManager.UI_MODES.STORY){
					enterScene = false;
				}
				GamePauseController.Instance.Play();
				switchCamera(switchCameraConfigInfo, switchCameraTriggerInfo, enterScene);
			}else{

					switchCamera(switchCameraConfigInfo, switchCameraTriggerInfo);	
			}
			*/
//		}else{
//			GamePauseController.Instance.Play();
//		}
	}
	
	public void TransformCamera()
	{
		cameraObj.transform.position = GetCameraPosition(_lookAtPosition, _tiltAngle, _panAngle, _distance);
		cameraObj.transform.LookAt( _lookAtPosition );
	}
	
	
	private void fadeIn(){
		_doingFade = false;
		
//		GamePauseController.Instance.Play();

//		switchCamera(switchCameraConfigInfo, switchCameraTriggerInfo);
	}
	
	public void StopDoingFade(){
		if (_doingFade){
			CancelInvoke();
			_doingFade = false;
//			GamePauseController.Instance.Play();
//			AppUtils.FadeIn();			
//			switchCamera(switchCameraConfigInfo, switchCameraTriggerInfo);			
		}
	}
	
	private Vector3 GetCameraPosition(Vector3 lookAtPosition, float tilt, float pan, float distance){
		
		float x = lookAtPosition.x + distance*Mathf.Sin(pan * Mathf.Deg2Rad )*Mathf.Cos(tilt * Mathf.Deg2Rad );
		float z = lookAtPosition.z + distance*Mathf.Cos(pan * Mathf.Deg2Rad )*Mathf.Cos(tilt * Mathf.Deg2Rad );
		float y = lookAtPosition.y + distance*Mathf.Sin(tilt * Mathf.Deg2Rad );
		
		return new Vector3(x, y, z);
	}	
	
	private bool cameraSlerp = false;
	private Vector3 beginPosition,  endPosition;
	private float   beginTilt,      endTilt;
	private float   beginPan,	    endPan;
	private float   beginDistance, endDistance;
	private float   beginTime,  moveTime;
	private void BeginCameraSlerp( Vector3 lookAtPosition, float tilt, float pan, float distance, float time )
	{
		cameraSlerp = true;
		
		beginPosition = new Vector3( _lookAtPosition.x, _lookAtPosition.y, _lookAtPosition.z );
		endPosition	  = lookAtPosition;
		
		beginTilt = _tiltAngle;
		endTilt   = tilt;
		
		beginPan  = _panAngle;
		endPan    = pan;
		
		beginDistance = _distance;
		endDistance    = distance;
		
		beginTime = Time.time;
		moveTime  = time;
	}
	
	private void StopCameraSlerp()
	{
		if ( !cameraSlerp ) return; 
		
		cameraSlerp = false;

        _currentPanAngle = -1;
        _currentTiltAngle = -1;
        _currentDistance = -1;

		_lookAtPosition = endPosition;
		_tiltAngle		= endTilt;
		_panAngle		= endPan;
		_distance		= endDistance;
	}
	
	private void SlerpCamera()
	{
		if ( !cameraSlerp ) return;
		
		float durTime = Time.time - beginTime;
		if ( durTime < moveTime )
		{
			float t = durTime / moveTime;
			_lookAtPosition = Vector3.Slerp( beginPosition, endPosition , t );

            _currentPanAngle = -1;
            _currentTiltAngle = -1;
            _currentDistance = -1;

			_tiltAngle		= ( 1 - t ) * beginTilt + t * endTilt;
			_panAngle 		= ( 1 - t ) * beginPan  + t * endPan;
			_distance		= ( 1 - t ) * beginDistance + t * endDistance;
		}
		else
		{
			StopCameraSlerp();
		}
	}
	
	
	private Vector3 _lookAtPosition;
	private float _panAngle  = 0;
	private float _tiltAngle = 90;
	private float _distance  = 500;
	private float _currentPanAngle  = -1;
	private float _currentTiltAngle = -1;
	private float _currentDistance  = -1;
	private void spinCamera()
	{	
		if ( _tiltAngle != _currentTiltAngle || _panAngle != _currentPanAngle || _distance != _currentDistance || _worldLookAtChange )
		{
			_currentTiltAngle = _tiltAngle;
			_currentPanAngle  = _panAngle;
			_currentDistance  = _distance;
		
			cameraObj.transform.position = GetCameraPosition(_lookAtPosition, _tiltAngle, _panAngle, _distance);
			_worldLookAtChange = false;
			
			cameraObj.transform.LookAt( _lookAtPosition );
		}
	}	
	
//	private void GotoNextNpc(){
//		//镜头切换后， 继续寻路
//        if (NpcView.EnableTrigger == true && UIManager.UILOCK == false)
//        {
//            MissionGuidePathFinder.Instance.GotoNextNpc();
//        }
//	}
	
	private void StartIOCcamUpdate(){
//		IOCcam ioc = Camera.main.transform.GetComponent<IOCcam>();
//		ioc.enableUpdate = true;
//		Invoke("StopIOCcamUpdate", 1f);
	}
	
	private void StopIOCcamUpdate()
	{
//		IOCcam ioc = Camera.main.transform.GetComponent<IOCcam>();
//		ioc.enableUpdate = false;		
	}
	
    //private bool eidtMode = false; //GET WARNING
	public void setEditMode( bool flag )
	{
        //eidtMode = flag;
	}

	public void ResetCamera()
	{
		cameraMove.transform.localPosition = Vector3.zero;
		cameraMove.target = null;
		cameraMove.followTarget = false;
		cameraMove.enabled = false;
	}

	private string _action;
	private bool _checkCameraStop = false;

	public void PlayCameraAnimator(int sceneId,int cameraId)
	{
		cameraAnimator.enabled = true;
		battleCameraAnimator.enabled = true;

		string path = string.Format("ArtRes/CameraAnimation/camera_{0}_{1}",sceneId,cameraId);
		_action = string.Format("camera_{0}_{1}",sceneId,cameraId);
		//string path = string.Format("ArtResources/Battle_{0}/Camera/camera_{0}_{1}",sceneId,cameraId);

		Debug.Log("path=" + path);
		Debug.Log("name=" + _action);

		//AnimationClip cameraAnimClip = ResourceLoader.Load(path) as AnimationClip;

		cameraAnimator.Play (_action, 0, 0f);
		Invoke("DelayCheckStop", 0.1f);

		battleCameraAnimator.Play (_action, 0, 0f);
	}

	private void DelayCheckStop()
	{
		_checkCameraStop = true;
	}
}
