// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SpriteFlickerEx.cs
// Author   : willson
// Purpose  : sprite隔一段时间闪烁
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlickerEx : MonoBehaviour
{
    public float repeatRate = 0.5f;
    public int count = 0;
    public bool autoPlay = false;

    void Start()
    {
        if (autoPlay)
        {
            Play();
        }
    }

    public void Play(int count = 5)
    {
        this.count = count;
        if (IsInvoking())
        {
            return;
        }
        InvokeRepeating("flicker", 0, repeatRate);
    }

    private void flicker()
    {
        if (count <= 0)
        {
            Stop();
            this.gameObject.SetActive(true);
            return;
        }

        this.gameObject.SetActive(!this.gameObject.activeSelf);
        count --;
    }

    public void Stop()
    {
        CancelInvoke("flicker");
    }
}
