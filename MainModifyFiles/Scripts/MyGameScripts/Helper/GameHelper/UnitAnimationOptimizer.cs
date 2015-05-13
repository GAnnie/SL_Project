// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  UnitAnimationOptimizer.cs
// Author   : senkay
// Created  : 5/28/2013 5:25:22 PM
// Purpose  : 单位动作优化器，不可见单位停止掉动作
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationOptimizer : MonoBehaviour
{
    private Animation anim = null;

    void Start()
    {
        anim = this.gameObject.transform.parent.GetComponentInChildren<Animation>();

        if (this.renderer.isVisible == false){
            OnBecameInvisible();
        }
    }

    void OnBecameVisible()
    {
        if (anim != null)
        {
            anim.enabled = true;
        }
    }

    void OnBecameInvisible()
    {
        if (anim != null)
        {
            anim.enabled = false;
        }
    }
}
