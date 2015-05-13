// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AudioInfo.cs
// Author   : wenlin
// Created  : 2013/6/26 11:19:22
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AudioInfo
{
    //音乐/音效类型 ,  1: 音效  2:音乐
    public int audioType = 1;

    //音乐的名称
    public string audioName;

    //是否循环
    public bool isLoop;

    //延迟时间
    public double delayTime  = 0.0f;

    //播放时长
    public double stayTime   = 0.0f;

    //淡入时长
    public double fadeInTime = 0.0f;

    //淡出时长
    public double fadeOutTime = 0.0f;
}
