// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AnimationSoundConfigData.cs
// Author   : wenlin
// Created  : 2013/6/26 11:14:01
// Purpose  : 这个是关于角色动作以音效对应的配置数据
// **********************************************************************

using System;
using System.Collections.Generic;

public class AnimationSoundConfigData
{
    //模型对应资源ID
    public string sourceID;

    //模型动作对应的音效
    public Dictionary<string, List< AudioInfo > > animationSoundDic = null;

    public AnimationSoundConfigData()
    { }

    public AnimationSoundConfigData( string sourceID )
    {
        this.sourceID = sourceID;

        animationSoundDic = new Dictionary<string, List<AudioInfo> >();
    }
}
