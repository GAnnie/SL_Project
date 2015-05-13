// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TimerManager.cs
// Author   : SK
// Created  : 2013/3/27
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerManager
{
	private static GameObject Root;
	
	private static Dictionary<string, MonoTimer> TimerMaps;
	
	public static bool isTimeMaps = false;

    public static void Setup()
    {
        if (isTimeMaps)
        {
            return;
        }

		Root = GameObject.Find("MonoTimers");
		if (Root == null)
		{
			Root = new GameObject("MonoTimers");
			GameObject.DontDestroyOnLoad( Root );
		}
		
		TimerMaps = new Dictionary<string, MonoTimer>();
		isTimeMaps = true;
	}
	
	public static MonoTimer GetTimer(string name){
        Setup();

		if (TimerMaps.ContainsKey(name)){
			return TimerMaps[name];
		}else{
			GameObject timerObj = new GameObject();
			timerObj.transform.parent = Root.transform;
			MonoTimer timer = timerObj.AddComponent<MonoTimer>();
			timer.name = name;
			TimerMaps[name] = timer;
			return timer;
		}
	}
	
	public static void RemoveTimer(MonoTimer timer){
		if (TimerMaps.ContainsKey(timer.name)){
			TimerMaps.Remove(timer.name);
		}
        GameObject.Destroy(timer.gameObject);
		timer.Destroy();
	}
	
}