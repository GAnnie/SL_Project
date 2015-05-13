// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  EffectSoundConfigData.cs
// Author   : wenlin
// Created  : 2013/6/26 11:27:31
// Purpose  : 用于记录特效以音效对应的关系的数据
// **********************************************************************

using System;
using System.Collections.Generic;


public class EffectSoundConfigData
{
    //特效名称
    public string effect;

    //特效对应的音效
    public List<AudioInfo> audioInfoList = null;

    public EffectSoundConfigData()
    { }

    public EffectSoundConfigData(string effect)
    {
        this.effect = effect;

        this.audioInfoList = new List<AudioInfo>();
    }
}
