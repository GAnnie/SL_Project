// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AudioOption.cs
// Author   : wenlin
// Created  : 2013/4/16 14:59:49
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;

public class AudioOption
{
    public float volum = 100;

    //StartTime
    public float startClipTime = 0.0f;
    //Stay Time 
    public float stayTime    = 0.0f;
    //Fade in Time
    public float fadeInTime  = 1.0f; 
    //Fade out Time
    public float fadeOutTime = 1.0f;
    //Is Loop
    public bool isLoop = false;
    //DelayTime
    public float delayTime = 0.0f;

    public AudioOption()
    { }
}
