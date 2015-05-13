using UnityEngine;
using System.Collections;

public class UVTitleY : MonoBehaviour {

    public float m_Speed = 3;
    float m_Pos = 0;
    float m_Time = 0;

    void Update()
    {
        float timeDelta = Time.deltaTime;

        m_Time += timeDelta;

        Matrix4x4 mat = Matrix4x4.TRS(new Vector3(0, -m_Pos, 0), Quaternion.identity, 
            new Vector3(1, 60, 1));
        renderer.material.SetMatrix("_TexMatrix", mat);
        m_Pos += timeDelta * m_Speed;
    }
}
