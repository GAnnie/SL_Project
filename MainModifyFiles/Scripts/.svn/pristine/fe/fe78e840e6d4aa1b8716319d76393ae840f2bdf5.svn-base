
using UnityEngine;
using System.Collections;

public class FXFlower : MonoBehaviour
{
	public FXMgrShadow _MgrShadow = null;
	public GameObject _PetalObj = null;

	public float _Pps = 20;
	public float _Radius = 0.25f;
	public float _minSpeed = 0;
	public float _maxSpeed = 0;
	public float _minLife = 0.25f;
	public float _maxLife = 0.5f;
	public float _midLife = 0.5f;
	public Vector3 _Gravity = new Vector3 (0, -10, 0);
	public Vector3 _RotSpeed = Vector3.zero;

	// if Drop Shadow
	public Vector4 _rectShadow = new Vector4 (0, 0, 256, 256);

	Mesh _Mesh = null;

	float _TimeLastCreate = 0;
	bool _Enabled = true;

	Vector3[] _vbSrc = null;
	Vector2[] _uvSrc = null;
	int[] _ibSrc = null;

	public struct Petal
	{
		public float age;
		public float life;

		public Color clr;

		public Vector3 pos;
		public Vector3 rot;

		public Vector3 spd;
		public Vector3 spd2;
	}

	ArrayList _Petals = null;

	void Awake ()
	{
		// Find Source Mesh
		Mesh sm = _PetalObj.GetComponent<MeshFilter> ().sharedMesh;
		
		_vbSrc = new Vector3[sm.vertices.Length];
		sm.vertices.CopyTo (_vbSrc, 0);
		
		_uvSrc = new Vector2[sm.uv.Length];
		sm.uv.CopyTo (_uvSrc, 0);
		
		_ibSrc = new int[sm.triangles.Length];
		sm.triangles.CopyTo (_ibSrc, 0);
		
		// Attach Mesh
		MeshFilter flt = gameObject.AddComponent (typeof(MeshFilter)) as MeshFilter;
		_Mesh = flt.mesh;
		
		MeshRenderer mr = gameObject.AddComponent (typeof(MeshRenderer)) as MeshRenderer;
		mr.material = _PetalObj.renderer.sharedMaterial;
		
		mr.castShadows = false;
		mr.receiveShadows = false;
		
		// Petal Container
		_Petals = new ArrayList ();
	}

	void CreatePetals (int num)
	{
		Matrix4x4 mat = transform.localToWorldMatrix;
		
		for (int i = 0; i < num; ++i) {
			Petal pt;
			
			pt.age = 0;
			pt.life = Mathf.Lerp (_minLife, _maxLife, Random.value);
			
			pt.clr = Color.white;
			
			float th = Random.value * Mathf.PI * 2.0f;
			float ph = Random.value * Mathf.PI;
			
			float x = Mathf.Cos (th) * Mathf.Sin (ph);
			float z = Mathf.Sin (th) * Mathf.Sin (ph);
			float y = Mathf.Cos (ph);
			
			pt.spd = new Vector3 (x, y, z);
			pt.pos = mat.MultiplyPoint (pt.spd * _Radius);
			pt.spd = mat.MultiplyVector (pt.spd);
			pt.spd *= Mathf.Lerp (_minSpeed, _maxSpeed, Random.value);
			
			pt.spd2 = Vector3.zero;
			
			pt.rot = new Vector3 (Random.value, Random.value, Random.value);
			pt.rot *= 360;
			
			_Petals.Add (pt);
		}
	}

	void UpdatePetals (float timeDelta)
	{
		ArrayList pts = new ArrayList ();
		
		Color sc = Color.black;
		
		foreach (Petal pt in _Petals) {
			if ((pt.age + timeDelta) < pt.life) {
				Petal p = pt;
				p.age += timeDelta;
				p.spd2 += _Gravity * timeDelta;
				p.pos += (p.spd + 0.5f * p.spd2) * timeDelta;
				p.rot += _RotSpeed * timeDelta;
				
				float mid = _midLife * p.life;
				
				if (p.age < (_midLife * p.life)) {
					float fac = p.age / mid;
					p.clr.a = Mathf.Lerp (0, 1, fac);
				}
				
				else {
					float fac = (p.age - mid) / (p.life - mid);
					p.clr.a = Mathf.Lerp (1, 0, fac);
				}
				
				pts.Add (p);
				
				if (null != _MgrShadow) {
					Vector3 sp = p.pos;
					sp.y = 0.025f;
					
					sc.a = 0.35f;
					
					float sml = p.life * 0.9f;
					if (p.age > sml) {
						sc.a *= 1.0f - (p.age - sml) / (p.life - sml);
					}
					
					_MgrShadow.AddShadow (sp, Vector3.forward, Vector3.right, 0.1f, 0.1f, _rectShadow, sc);
				}
			}
		}
		
		_Petals = pts;
	}

	void Update ()
	{
		float timeDelta = Time.deltaTime;
		
		_TimeLastCreate += timeDelta;
		
		if (_Enabled) {
			int n = (int)(_TimeLastCreate * _Pps);
			if (n > 0) {
				CreatePetals (n);
				_TimeLastCreate -= n / _Pps;
			}
		}
		
		if (_Petals.Count > 0)
			UpdatePetals (timeDelta);
	}

	void LateUpdate ()
	{
		_Mesh.Clear ();
		
		int nop = _Petals.Count;
		
		if (nop <= 0)
			return;
		
		Vector3[] vb = new Vector3[nop * _vbSrc.Length];
		Vector2[] uvs = new Vector2[nop * _uvSrc.Length];
		Color[] clrs = new Color[nop * _vbSrc.Length];
		int[] ib = new int[nop * _ibSrc.Length];
		
		Matrix4x4 mat = transform.worldToLocalMatrix;
		
		for (int i = 0; i < nop; ++i) {
			Petal p = (Petal)_Petals[i];
			
			const float scl = 0.5f;
			Matrix4x4 trs = mat * Matrix4x4.TRS (p.pos, Quaternion.Euler (p.rot.x, p.rot.y, p.rot.z), Vector3.one) * Matrix4x4.Scale (new Vector3 (scl, scl, scl));
			
			int nov = _vbSrc.Length;
			int vbase = i * nov;
			
			for (int j = 0; j < nov; ++j) {
				vb[vbase + j] = trs.MultiplyPoint (_vbSrc[j]);
				uvs[vbase + j] = _uvSrc[j];
				clrs[vbase + j] = p.clr;
			}
			
			int nof = _ibSrc.Length;
			int ibase = i * nof;
			
			for (int j = 0; j < nof; ++j) {
				ib[ibase + j] = vbase + _ibSrc[j];
			}
		}
		
		_Mesh.vertices = vb;
		_Mesh.uv = uvs;
		_Mesh.colors = clrs;
		_Mesh.triangles = ib;
	}
}
