using UnityEngine;
using System.Collections;

public class BBLockY : MonoBehaviour
{
    public Camera _Camera = null;
    public float _Rot = 0;
    public float _RotSpeed = 0;

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
            //_Camera = FindObjectOfType(typeof(Camera)) as Camera;
        }

        if (_Camera != null)
        {
            Quaternion rot = Quaternion.Euler(0, _Rot, 0);

            Vector3 pos = _Camera.transform.position;
            pos.y = transform.position.y;

            transform.LookAt(pos);
            transform.rotation = transform.rotation * rot;
        }
    }

    void Update()
    {
        if (null != _Camera)
        {
            float timeDelta = Time.deltaTime;

            _Rot += timeDelta * _RotSpeed;

            Quaternion rot = Quaternion.Euler(0, _Rot, 0);

            Vector3 pos = _Camera.transform.position;
            pos.y = transform.position.y;

            transform.LookAt(pos);
            transform.rotation = transform.rotation * rot;
        }
    }
}
