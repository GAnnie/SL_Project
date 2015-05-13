// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TrailEffectTime.cs
// Author   : SK
// Created  : 2013/8/2
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class TrailEffectTime : MonoBehaviour
{
    MeleeWeaponTrail mWeaponTrail;

    public void Init(MeleeWeaponTrail mwt)
    {
        mWeaponTrail = mwt;
    }

    public void Play(float delayTime)
    {
        StartCoroutine(StopEffect(delayTime));
    }

    private IEnumerator StopEffect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (mWeaponTrail != null)
        {
            mWeaponTrail.StopTrail();
        }
    }
}

