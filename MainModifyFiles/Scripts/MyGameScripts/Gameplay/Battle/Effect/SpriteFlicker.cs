// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SpriteFlicker.cs
// Author   : senkay
// Created  : 5/29/2013 2:54:53 PM
// Purpose  : sprite隔一段时间闪烁
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlicker : MonoBehaviour
{
    public float time = 0.5f;
    public bool autoPlay = false;

    void Start()
    {
        if (autoPlay)
        {
            Play();
        }
    }

    public void Play()
    {
        InvokeRepeating("flicker", 0, time);
    }

    private void flicker()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void Stop()
    {
        CancelInvoke("flicker");
    }
}
