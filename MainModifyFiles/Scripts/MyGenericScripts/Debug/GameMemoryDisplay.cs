// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  GameMemoryDisplay.cs
// Author   : senkay
// Created  : 6/26/2013 8:35:28 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMemoryDisplay : MonoBehaviour
{
    public Rect startRect; // The rect the window is initially displayed at.
    public bool updateColor = true; // Do you want the color to change if the FPS gets low
    public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
    public float frequency = 5f; // The update frequency of the fps

    private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.

#if UNITY_DEBUG
    void Start()
    {
        startRect = new Rect(0f, Screen.height - 50f, 200f, 180f);

        //if (SystemSetting.IsMobileRuntime)
        //{
            InvokeRepeating("GetFreeMemory", 0, frequency);
        //}
    }



    private long _freeMemory = 0;
    private long _totalMemory = 0;
    private uint _useHeapSize = 0;
    private uint _monoUseHeap = 0;
    private uint _monoHeapSize = 0;
    private int _assetMemory = 0;
    private const int num = 1024 * 1024;
    private void GetFreeMemory()
    {
        if (SystemSetting.IsMobileRuntime)
        {
            _freeMemory = BaoyugameSdk.getFreeMemory() / 1024;
            _totalMemory = BaoyugameSdk.getTotalMemory() / 1024;
            GameDebuger.Log("GetFreeMemory " + _freeMemory + "/" + _totalMemory);
        }
        _useHeapSize = Profiler.usedHeapSize / num;
        _monoUseHeap = Profiler.GetMonoUsedSize() / num;
        _monoHeapSize = Profiler.GetMonoHeapSize() / num;

        //_assetMemory = (AssetbundleManager.Instance.AssetMemory())/num;
    }

    void OnGUI()
    {
		if (AppManager.DebugMode == false){
			return;
		}		

        //if (SystemSetting.IsMobileRuntime)
        //{
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label);
                style.normal.textColor = Color.green;
                style.alignment = TextAnchor.MiddleCenter;
            }

            GUI.color = updateColor ? color : Color.white;
            startRect = GUI.Window(1, startRect, DoMyWindow, "");
        //}
    }

    void DoMyWindow(int windowID)
    {
		int index = 0;
		if (_monoHeapSize != 0){
			GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "MonoHeap :" + _monoHeapSize.ToString()+ "MB", style);
			index++;
		}
        if (_monoUseHeap != 0){
			GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "MonoUseHeap :" + _monoUseHeap + "MB", style);
			index++;
		}
        if (_assetMemory != 0){
			GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "AssetMemory :" + _assetMemory + "MB", style);
			index++;
		}
        if (_useHeapSize != 0){
			GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "UnityUseHeap :" + _useHeapSize + "MB", style);
			index++;
		}
        GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "FreeMemory : " + _freeMemory.ToString() + "MB", style);
		index++;
        GUI.Label(new Rect(0, index*30, startRect.width, startRect.height / 5), "TotalMemory : " + _totalMemory.ToString() + "MB", style);
		index++;
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
#endif
}
