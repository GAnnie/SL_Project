//-----------------------------------------------------------------------
// <copyright file="CarRenderHelper.cs" company="Taomee Inc.">
//     Copyright (c) 2012 Taomee Inc. All rights reserved.
// </copyright>
// <author>Jay Chen</author>
// <description>
//     让物体始终面向摄像机，不限于面片
// </description>
// <history created="2012/02/21">
// </history>
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class BBFull : MonoBehaviour
{
    public Camera _Camera = null;
    public float _Rot = 0;
    public float _RotSpeed = 0;
    //bool animPlayed = false;  //GET WARNING

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

        if (_Camera != null)
        {
            Quaternion rot = Quaternion.Euler(0, _Rot, 0);
            transform.LookAt(_Camera.transform.position);
            transform.rotation = transform.rotation * rot;
        }
    }

    void Update()
    {
        if (null != _Camera)
        {
            float timeDelta = Time.deltaTime;

            _Rot += timeDelta * _RotSpeed;

            Quaternion rot = Quaternion.Euler(0, 0, _Rot);
            transform.LookAt(_Camera.transform.position);
            transform.rotation = transform.rotation * rot;
        }
    }
}
