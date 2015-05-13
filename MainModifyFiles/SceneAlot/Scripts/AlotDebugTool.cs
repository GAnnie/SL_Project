// **********************************************************************
//	Copyright (C), 2011-2015, Alot Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For All Project With .cs
//  FileName:	AlotDebugTool.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		0000.00.00
//	Modify:		__

//	Url:		http://www.alot.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.services;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.whole.modules.stall.dto;

public class AlotDebugTool : MonoBehaviour {
    
	private int _heightNum = 5;

	private Rect _windowRect = new Rect();
	
	private string _numTextField = "0";

    // Use this for initialization
    void Start () {

		_windowRect = new Rect(5, Screen.height - 20, 110, _heightNum*50);

		GameDebuger.openDebugLogOrange = !GameDebuger.openDebugLogOrange;
		MissionDataModel.Instance.openExpandContent = !MissionDataModel.Instance.openExpandContent;
		
		//gameObject.SetActive(false);
		GameDebuger.OrangeDebugLog("Is a Start OnGUI debug func");
    }
    
    void OnGUI() {
		//GameDebuger.OrangeDebugLog("Is a OnGUI debug func");

		_windowRect = GUI.Window(99/*windowID*/, _windowRect, WindowDraw, "AlotTool");
    }
    
    void WindowDraw(int windowID) {
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            
		}
		
		if (GUILayout.Button("AlotDebug_O") || Input.GetKeyDown(KeyCode.O)) {
			GameDebuger.openDebugLogOrange = !GameDebuger.openDebugLogOrange;
			Debug.LogError(string.Format("The Orange Debug State -> {0}", GameDebuger.openDebugLogOrange));
		}
		
		if (GUILayout.Button("CloseTrade_D") || Input.GetKeyDown(KeyCode.D)) {
			ProxyTradeModule.Open();
		}
		
		if (GUILayout.Button("CloseTrade_C") || Input.GetKeyDown(KeyCode.C)) {
			ProxyTradeModule.Close();
		}
        
        if (GUILayout.Button("| AlotGC_G |") || Input.GetKeyDown(KeyCode.G)) {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
		}

		if (GUILayout.Button("| M-Tmp_M |") || Input.GetKeyDown(KeyCode.M)) {
			Debug.LogError("Mission 测试用");
			/*for (int i = 0, len = 10; i < len; i++) {
				TipManager.AddTip("Mission 测试用", 10.0f);
			}*/
			ProxyMissionModule.Open();
		}
		
		MissionDataModel.Instance.resquestServer = GUILayout.Toggle(MissionDataModel.Instance.resquestServer, "任务请求");
		
		if (GUILayout.Button("| 任务请求SOS_R |") || Input.GetKeyDown(KeyCode.R)) {
			Debug.LogError(string.Format("是否请求服务器：{0}", MissionDataModel.Instance.resquestServer));
		}
		
		if (GUILayout.Button("| 师门任务_F |") || Input.GetKeyDown(KeyCode.F)) {
			TipManager.AddTip(string.Format("[ff0000]触发师门任务，仅作测试使用[-]"));
			MissionDataModel.Instance.AcceptFactionMission();
		}

		/*
		if (GUILayout.Button("| WalkTo_W |") || Input.GetKeyDown(KeyCode.W)) {
			Vector3 tLastNpcPos = new Vector3(10, 0, 10);
			NavMeshHit hit;
			NavMesh.SamplePosition(tLastNpcPos, out hit, 5, -1);
			WorldManager.Instance.GetHeroView().WalkToPoint(tLastNpcPos, () => {
				Debug.LogError(" ========== 到达寻路地点 ========== ");
			});
		}
		*/

		_numTextField = GUILayout.TextField(_numTextField, 10);

        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

	// Update is called once per frame
	/*
	void Update () {
		GameDebuger.OrangeDebugLog("Is a Update debug func");
	}
	*/
}