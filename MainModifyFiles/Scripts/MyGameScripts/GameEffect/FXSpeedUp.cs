using UnityEngine;
using System.Collections;

public class FXSpeedUp : MonoBehaviour {

    public float m_Speed = 3;

    float m_Pos = 0;
    float m_Time = 0;

    Color temp = new Color(1, 1, 1, 1);

    void Update()
    {
        float timeDelta = Time.deltaTime;

        m_Time += timeDelta;

        Matrix4x4 mat = Matrix4x4.TRS(new Vector3(0, -m_Pos, 0), Quaternion.identity, new Vector3(1, 1, 1));
        renderer.material.SetMatrix("_TexMatrix", mat);
        float test1 = Mathf.Abs(Mathf.Sin(m_Time * Mathf.PI / 2));
        float test2 = Mathf.Abs(Mathf.Cos(m_Time * Mathf.PI / 2));
        temp.r = 1;
        temp.g = test1;
        temp.b = test2;

        renderer.material.SetColor("_ColorMove", temp);
        m_Pos += timeDelta * m_Speed;
    }
}
