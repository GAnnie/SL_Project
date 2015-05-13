// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  MenTest.cs
// Author   : senkay
// Created  : 6/9/2013 5:21:18 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class MenTest : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("Clean"))
        {
            //int rand = NGUITools.RandomRange(0,100);
            Resources.UnloadUnusedAssets();
			GC.Collect();
        }
    }
}
