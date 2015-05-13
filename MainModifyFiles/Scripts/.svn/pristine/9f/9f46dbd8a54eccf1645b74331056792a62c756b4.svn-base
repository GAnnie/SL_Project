using UnityEngine;
using System.Collections;

public class MeshEmitterFacing : MonoBehaviour 
{
    public GameObject _MeshObj = null;
    public Material _Mat = null;

    public bool _localSpace = false;

    public float _Pps = 20;
    
    public float _minLife = 0.25f;
    public float _maxLife = 0.5f;
    public float _midLife = 0.5f;

    public float _minRadius = 0;
    public float _maxRadius = 0;

    public float _minTheta = 0;
    public float _maxTheta = 360;

    public float _minPhi = 0;
    public float _maxPhi = 180;

    public float _spdMin = 0;
    public float _spdMax = 0;

    public float _sclStart = 1;
    public float _sclMiddle = 1;
    public float _sclEnd = 1;

    public float _minRot = 0;
    public float _maxRot = 0;

    public float _minRotSpeed = 0;
    public float _maxRotSpeed = 0;

    public Color _clrStart = Color.white;
    public Color _clrMiddle = Color.white;
    public Color _clrEnd = Color.white;

    public Vector3 _Gravity = new Vector3(0, -10, 0);

    Mesh _Mesh = null;

    float _TimeLastCreate = 0;
    bool _Emit = true;

    Vector3[] _vbSrc = null;
    Vector2[] _uvSrc = null;
    int[] _ibSrc = null;
    
    public struct Petal
    {
        public float age;
        public float life;

        public float scl;
        public Color clr;

        public Vector3 pos;
        public float rot;

        public Vector3 spd;
        public Vector3 spd2;

        public float rotSpd;
    }

    ArrayList _Petals = null;

    public bool emit
    {
        get { return _Emit; }
        set
        {
            if (value != _Emit)
            {
                _Emit = value;
                if (_Emit)
                {
                    _TimeLastCreate = 0;
                }
            }
        }
    }

    void Awake()
    {
        if (null != _MeshObj)
        {
            _minTheta *= Mathf.Deg2Rad;
            _maxTheta *= Mathf.Deg2Rad;

            _minPhi *= Mathf.Deg2Rad;
            _maxPhi *= Mathf.Deg2Rad;

            // Find Source Mesh
            Mesh sm = _MeshObj.GetComponent<MeshFilter>().sharedMesh;

            _vbSrc = new Vector3[sm.vertices.Length];
            sm.vertices.CopyTo(_vbSrc, 0);

            _uvSrc = new Vector2[sm.uv.Length];
            sm.uv.CopyTo(_uvSrc, 0);

            _ibSrc = new int[sm.triangles.Length];
            sm.triangles.CopyTo(_ibSrc, 0);

            // Attach Mesh
            MeshFilter flt = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            _Mesh = flt.mesh;

            MeshRenderer mr = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

            if (_Mat) mr.material = _Mat;
            else
                mr.material = _MeshObj.renderer.sharedMaterial;

            mr.castShadows = false;
            mr.receiveShadows = false;

            // Petal Container
            _Petals = new ArrayList();
        }
    }

    Color lerp(ref Color c1, ref Color c2, float fac)
    {
        Color c = new Color();
        c.r = Mathf.Lerp(c1.r, c2.r, fac);
        c.g = Mathf.Lerp(c1.g, c2.g, fac);
        c.b = Mathf.Lerp(c1.b, c2.b, fac);
        c.a = Mathf.Lerp(c1.a, c2.a, fac);
        return c;
    }

    void CreatePetals(int num)
    {
        Matrix4x4 mat = transform.localToWorldMatrix;

        for (int i = 0; i < num; ++i)
        {
            Petal pt;

            pt.age = 0;
            pt.life = Mathf.Lerp(_minLife, _maxLife, Random.value);

            pt.clr = _clrStart;
            pt.scl = _sclStart;

            float th = Mathf.Lerp(_minTheta, _maxTheta, Random.value);
            float ph = Mathf.Lerp(_minPhi, _maxPhi, Random.value);            

            float x = Mathf.Cos(th) * Mathf.Sin(ph);
            float y = Mathf.Sin(th) * Mathf.Sin(ph);
            float z = Mathf.Cos(ph);

            pt.spd = new Vector3(x, y, z);

            float r = Mathf.Lerp(_minRadius, _maxRadius, Random.value);

            pt.pos = pt.spd * r;
            if (!_localSpace) pt.pos = mat.MultiplyPoint(pt.pos);
            
            if (!_localSpace) pt.spd = mat.MultiplyVector(pt.spd);
            pt.spd *= Mathf.Lerp(_spdMin, _spdMax, Random.value);

            pt.spd2 = Vector3.zero;

            pt.rot = Mathf.Lerp(_minRot, _maxRot, Random.value);
            pt.rotSpd = Mathf.Lerp(_minRotSpeed, _maxRotSpeed, Random.value);

            _Petals.Add(pt);
        }
    }

    void UpdatePetals(float timeDelta)
    {
        ArrayList pts = new ArrayList();

        Matrix4x4 mat = transform.worldToLocalMatrix;

        foreach (Petal pt in _Petals)
        {
            if ((pt.age + timeDelta) < pt.life)
            {
                Petal p = pt;

                p.age += timeDelta;

                if (!_localSpace) p.spd2 += _Gravity * timeDelta;
                else
                    p.spd2 += mat.MultiplyVector(_Gravity) * timeDelta;

                p.pos += (p.spd + 0.5f * p.spd2) * timeDelta;

                p.rot += pt.rotSpd * timeDelta;

                float mid = _midLife * p.life;

                if (p.age < (_midLife * p.life))
                {
                    float fac = p.age / mid;
                    p.clr = lerp(ref _clrStart, ref _clrMiddle, fac);
                    p.scl = Mathf.Lerp(_sclStart, _sclMiddle, fac);
                }

                else
                {
                    float fac = (p.age - mid) / (p.life - mid);                    
                    p.clr = lerp(ref _clrMiddle, ref _clrEnd, fac);
                    p.scl = Mathf.Lerp(_sclMiddle, _sclEnd, fac);
                }

                pts.Add(p);
            }
        }

        _Petals = pts;
    }

    void Update()
    {
        if (null == _Mesh) return;

        float timeDelta = Time.deltaTime;

        _TimeLastCreate += timeDelta;

        if (_Emit)
        {
            int n = (int)(_TimeLastCreate * _Pps);
            if (n > 0)
            {
                CreatePetals(n);
                _TimeLastCreate -= n / _Pps;
            }
        }

        if (_Petals.Count > 0)
            UpdatePetals(timeDelta);
    }

    Matrix4x4 buildRotFromTo(Vector3 from, Vector3 to)
    {
        return Matrix4x4.TRS(Vector3.zero, Quaternion.FromToRotation(from, to), Vector3.one);
    }

    void LateUpdate()
    {
        if (null == _Mesh) return;

        _Mesh.Clear();

        int nop = _Petals.Count;

        if (nop <= 0) return;

        Vector3[] vb = new Vector3[nop * _vbSrc.Length];
        Vector2[] uvs = new Vector2[nop * _uvSrc.Length];
        Color[] clrs = new Color[nop * _vbSrc.Length];
        int[] ib = new int[nop * _ibSrc.Length];

        Matrix4x4 mat = transform.worldToLocalMatrix;
        
        for (int i = 0; i < nop; ++i)
        {
            Petal p = (Petal)_Petals[i];

            Matrix4x4 trs = Matrix4x4.TRS(p.pos, Quaternion.FromToRotation(Vector3.forward, (p.spd + p.spd2).normalized) * Quaternion.Euler(0, 0, p.rot), Vector3.one * p.scl);
            if (!_localSpace)
                trs = mat * trs;

            int nov = _vbSrc.Length;
            int vbase = i * nov;
            
            for (int j = 0; j < nov; ++j)
            {
                vb[vbase + j] = trs.MultiplyPoint(_vbSrc[j]);
                uvs[vbase + j] = _uvSrc[j];
                clrs[vbase + j] = p.clr;
            }

            int nof = _ibSrc.Length;
            int ibase = i * nof;

            for (int j = 0; j < nof; ++j)
            {
                ib[ibase + j] = vbase + _ibSrc[j];
            }
        }

        _Mesh.vertices = vb;
        _Mesh.uv = uvs;
        _Mesh.colors = clrs;
        _Mesh.triangles = ib;
    }
}
