//-----------------------------------------------------------------------
// <copyright file="CarRenderHelper.cs" company="Taomee Inc.">
//     Copyright (c) 2012 Taomee Inc. All rights reserved.
// </copyright>
// <author>Jay Chen</author>
// <description>
//     港口小鸟特殊脚本，用于在一定距离内播放动作，暂不制作通用脚本
// </description>
// <history created="2012/02/21">
// </history>
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PortBirdScript: MonoBehaviour
{
    public Camera _Camera = null;
    private GameObject bird = null;
    bool animPlayed = false;

    void Start()
    {
        if (null == _Camera)
        {
            Camera[] cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].name.Equals("MainCamera"))
                {
                    _Camera = cameras[i];
                }
            }
        }

        if (this.transform.GetChildCount() > 0)
        {
            bird = this.transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        if (null != _Camera)
        {
            if (bird != null)
            {
                Vector3 distanceV = _Camera.transform.position - this.transform.position;
                distanceV.y = 0.0f;
                float distance = distanceV.sqrMagnitude;
                if (distance < 1600.0f && !animPlayed)
                {
                    bird.animation.Play("Take 001", AnimationPlayMode.Stop);
                    animPlayed = true;
                }

                if (distance > 6400.0f)
                {
                    animPlayed = false;
                }
            }
        }
    }
}
