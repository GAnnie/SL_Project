
using UnityEngine;
using System.Collections;

public class FXTurnUV : MonoBehaviour
{
    private float zrotate = 0.0f;
    void Awake()
    {
    }

    void Update()
    {
        return;
        float timeDelta = Time.deltaTime;

        //m_Time += timeDelta;

        zrotate +=  timeDelta * 60;
        //zrotate += 2.0f;
        
        Quaternion temprotation = Quaternion.identity;
        temprotation.eulerAngles = new Vector3(0, 0, zrotate);
        Matrix4x4 mat0 = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0), Quaternion.identity, new Vector3(1, 1, 1));
        Matrix4x4 mat1 = Matrix4x4.TRS(new Vector3(0, 0, 0), temprotation, new Vector3(1, 1, 1));
        Matrix4x4 mat2 = Matrix4x4.TRS(new Vector3(-0.5f, -0.5f, 0), Quaternion.identity, new Vector3(1, 1, 1));
                    
        renderer.material.SetMatrix("_TexMatrix", mat0 * mat1 *mat2);

        //_Pos += timeDelta * _Speed;
    }
}
