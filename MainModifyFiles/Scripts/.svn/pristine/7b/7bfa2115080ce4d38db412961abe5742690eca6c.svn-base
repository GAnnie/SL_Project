using UnityEngine;
using System.Collections;

public class UVAnimator2 : MonoBehaviour
{
    public int _uNum = 1;
    public int _vNum = 1;
    public float _FPS = 0;
    public float _uSpeed = 0;
    public float _vSpeed = 0;
    public bool _Loop = true;

    float _Time = 0;
    int _Index = 0;

    float _uOffset = 0;
    float _vOffset = 0;

    float _u = 0;
    float _v = 0;

    float _uScl = 1;
    float _vScl = 1;

    private bool m_unloopEnd = false;

    bool UpdateTexMatrix(int idx)
    {
        Texture2D tex = renderer.material.GetTexture("_MainTex") as Texture2D;
        if (null != tex)
        {
            _uScl = 1.0f / _uNum;
            _vScl = 1.0f / _vNum;

            int cx = tex.width / _uNum;
            int cy = tex.height / _vNum;

            int y = idx / _uNum;
            int x = idx - (cy * _uNum);

            float ufac = 1.0f / tex.width;
            float vfac = 1.0f / tex.height;

            _u = cx * x * ufac;

            _v = cy * y;
            _v = tex.height - _v - cy;
            _v *= vfac;

            if (0 == _uSpeed && 0 == _vSpeed)
            {
                Matrix4x4 mat = Matrix4x4.TRS(new Vector3(_u, _v, 0), Quaternion.identity, new Vector3(_uScl, _vScl, 1));
                renderer.material.SetMatrix("_TexMatrix", mat);

                return true;
            }

            return false;
        }

        return false;
    }

    void Start()
    {
        UpdateTexMatrix(_Index);
    }

    void Update()
    {
        if (0 == _FPS) return;
        if (m_unloopEnd) return;

        float timeDelta = Time.deltaTime;
        _Time += timeDelta;

        _uOffset += timeDelta * _uSpeed;
        _vOffset += timeDelta * _vSpeed;

        float spf = 1.0f / _FPS;

        int idx = (int)(_Time / spf);
        if (!_Loop)
        {
            if (idx >= _uNum * _vNum)
            {
                idx = _uNum * _vNum - 1;
                m_unloopEnd = true;
            }
        }
                
        idx = idx % (_uNum * _vNum);



        if (idx != _Index)
        {
            _Index = idx;
            if (idx < 0)
            {
                idx = -idx;
                idx = (_uNum * _vNum - idx);
            }

            bool updated = UpdateTexMatrix(idx);
            if (updated)
                return;
        }

        // Update with uSpeed or vSpeed
        float u = _u + _uOffset;
        float v = _v + _vOffset;

        Matrix4x4 mat = Matrix4x4.TRS(new Vector3(u, v, 0), Quaternion.identity, new Vector3(_uScl, _vScl, 1));
        renderer.material.SetMatrix("_TexMatrix", mat);
    }
}


